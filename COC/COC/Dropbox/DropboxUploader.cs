using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using COC.Application;
using COC.Infrastructure;
using Dropbox.Api;
using Dropbox.Api.Files;
using File = System.IO.File;
using Task = System.Threading.Tasks.Task;


namespace COC.Dropbox
{
    public class DropboxUploader: IUploader
    {
        //cd Leonid/dropbox/Folder1
        //upload F:\COCtest
        //upload F:\COCtest\txt2.txt
        
        public void UploadFile(string pathToUpload, string fileToUploadPath, Account account)
        {
            var dropboxClient = new DropboxClient(account.ServicesTokens["dropbox"]);
            var name = fileToUploadPath.Split('\\').Last();
            if (File.Exists(fileToUploadPath))
            {
                Console.WriteLine("Uploading " + name);
                Task.Run(() => UploadSingleFile(fileToUploadPath, pathToUpload, dropboxClient));
                FileSystemManager.CurrentFolder.Content.Add(name, new Infrastructure.File(pathToUpload, account));
            }
            if (Directory.Exists(fileToUploadPath))
            {
                var folder = UploadFolder(pathToUpload, fileToUploadPath, dropboxClient, account, FileSystemManager.CurrentFolder);
                FileSystemManager.CurrentFolder.Content.Add(name, folder);
            }
        }
        
        private Folder UploadFolder(string pathToUpload, string fileToUploadPath, DropboxClient client, Account account, Folder parentFolder)
        {
            var directory = client.Files.CreateFolderV2Async(pathToUpload).Result;
            var localFolder = new Folder($"Root/{account.AccountName}/dropbox{pathToUpload}", new Dictionary<string, IFileSystemUnit>(), account);
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
                var localFile = new Infrastructure.File(name, account);
                localFolder.Content.Add(name, localFile);
            }

            return localFolder;
        }

        private async Task UploadSingleFile(string localPath, string remotePath, DropboxClient client)
        {
            const int ChunkSize = 4096 * 1024;
            using (var fileStream = File.Open(localPath, FileMode.Open))
            {
                if (fileStream.Length <= ChunkSize)
                {
                    var metadata = client.Files.UploadAsync(remotePath, body: fileStream).Result;
                }
                else
                {
                    await ChunkUpload(remotePath, fileStream, (int)ChunkSize, client);
                }
            }
        }

        private async Task ChunkUpload(string path, FileStream stream, int chunkSize, DropboxClient client)
        {
            ulong numChunks = (ulong)Math.Ceiling((double)stream.Length / chunkSize);
            byte[] buffer = new byte[chunkSize];
            string sessionId = null;
            for (ulong idx = 0; idx < numChunks; idx++)
            {
                var byteRead = stream.Read(buffer, 0, chunkSize);

                using (var memStream = new MemoryStream(buffer, 0, byteRead))
                {
                    if (idx == 0)
                    {
                        var result = await client.Files.UploadSessionStartAsync(false, UploadSessionType.Sequential.Instance, memStream);
                        sessionId = result.SessionId;
                    }
                    else
                    {
                        var cursor = new UploadSessionCursor(sessionId, (ulong)chunkSize * idx);

                        if (idx == numChunks - 1)
                        {
                            FileMetadata metadata = await client.Files.UploadSessionFinishAsync(cursor, new CommitInfo(path), memStream);
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