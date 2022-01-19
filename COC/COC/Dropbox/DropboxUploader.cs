using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using COC.Application;
using COC.ConsoleInterface;
using COC.Infrastructure;
using Dropbox.Api;
using Dropbox.Api.Files;
using Ninject;
using File = System.IO.File;
using Task = System.Threading.Tasks.Task;


namespace COC.Dropbox
{
    public class DropboxUploader : IUploader
    {
        public void UploadFile(string pathToUpload, string fileToUploadPath, Account account)
        {
            var dropboxClient = new DropboxClient(account.ServicesTokens["dropbox"]);
            var name = fileToUploadPath.Split('\\').Last();
            if (File.Exists(fileToUploadPath))
            {
                Console.WriteLine("Uploading " + name);
                if (FileSystemManager.CurrentFolder.Content.ContainsKey(name) && FileSystemManager.CurrentFolder.Content[name] is Infrastructure.File)
                {
                    Console.WriteLine("File with this name already exists. You can delete it manually and then upload, rename it or upload in another folder.");
                    return;
                }
                Task.Run(() => UploadSingleFile(fileToUploadPath, pathToUpload, dropboxClient));
                FileSystemManager.CurrentFolder.Content.Add(name, new Infrastructure.File(FileSystemManager.CurrentFolder.Path + pathToUpload, account));
            }

            if (Directory.Exists(fileToUploadPath))
            {
                if (FileSystemManager.CurrentFolder.Content.ContainsKey(name) && FileSystemManager.CurrentFolder.Content[name] is Folder)
                    if (FileSystemManager.CurrentFolder.Content.ContainsKey(name))
                    {
                        Console.WriteLine("Folder with this name already exists. You can delete it manually and then upload, rename it or upload in another folder.");
                        return;
                    }
                var folder = UploadFolder(pathToUpload, fileToUploadPath, dropboxClient, account,
                    FileSystemManager.CurrentFolder);
                FileSystemManager.CurrentFolder.Content.Add(name, folder);
            }
        }

        private Folder UploadFolder(string pathToUpload, string fileToUploadPath, DropboxClient client, Account account,
            Folder parentFolder)
        {
            var directory = client.Files.CreateFolderV2Async(pathToUpload).Result;

            var pathArg =
                new Ninject.Parameters.ConstructorArgument("path", $"Root/{account.AccountName}/dropbox{pathToUpload}");
            var contentArg =
                new Ninject.Parameters.ConstructorArgument("content", new Dictionary<string, IFileSystemUnit>());
            var accountArg = new Ninject.Parameters.ConstructorArgument("account", account);
            var localFolder = Program.container.Get<Folder>(pathArg, contentArg, accountArg);
            localFolder.ParentFolder = parentFolder;
            foreach (var subdirectory in Directory.GetDirectories(fileToUploadPath))
            {
                var name = subdirectory.Split('\\').Last();
                var subFolder = UploadFolder(pathToUpload + '/' + name, subdirectory, client, account, localFolder);
                localFolder.Content.Add(name, subFolder);
            }

            foreach (var file in Directory.GetFiles(fileToUploadPath))
            {
                var name = file.Split('\\').Last();
                Console.WriteLine("Uploading " + name);
                Task.Run(() => UploadSingleFile(file, pathToUpload + '/' + name, client));
                var localFile = new Infrastructure.File(localFolder.Path + '/' + name, account);
                localFolder.Content.Add(name, localFile);
            }

            return localFolder;
        }

        private async Task UploadSingleFile(string localPath, string remotePath, DropboxClient client)
        {
            const int ChunkSize = 4096 * 4096;
            using (var fileStream = File.Open(localPath, FileMode.Open))
            {
                if (fileStream.Length <= ChunkSize)
                {
                    var metadata = client.Files.UploadAsync(remotePath, body: fileStream).Result;
                }
                else
                {
                    await ChunkUpload(remotePath, fileStream, ChunkSize, client);
                }
            }
        }

        private async Task ChunkUpload(string path, FileStream stream, int chunkSize, DropboxClient client)
        {
            ulong numChunks = (ulong) Math.Ceiling((double) stream.Length / chunkSize);
            byte[] buffer = new byte[chunkSize];
            string sessionId = null;
            for (ulong idx = 0; idx < numChunks; idx++)
            {
                var byteRead = stream.Read(buffer, 0, chunkSize);

                using (var memStream = new MemoryStream(buffer, 0, byteRead))
                {
                    if (idx == 0)
                    {
                        var result = await client.Files.UploadSessionStartAsync(false,
                            UploadSessionType.Sequential.Instance, memStream);
                        sessionId = result.SessionId;
                    }
                    else
                    {
                        var cursor = new UploadSessionCursor(sessionId, (ulong) chunkSize * idx);

                        if (idx == numChunks - 1)
                        {
                            FileMetadata metadata =
                                await client.Files.UploadSessionFinishAsync(cursor, new CommitInfo(path), memStream);
                        }
                        else
                        {
                            await client.Files.UploadSessionAppendV2Async(cursor, false, memStream);
                        }
                    }
                }
            }
        }
    }
}