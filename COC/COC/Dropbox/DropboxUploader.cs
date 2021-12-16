using System;
using System.IO;
using COC.Application;
using COC.Infrastructure;
using Dropbox.Api;
using Dropbox.Api.Files;
using File = System.IO.File;
using Task = System.Threading.Tasks.Task;


namespace COC.Dropbox
{
    public static class DropboxUploader
    {
        public static void UploadFile(string pathToUpload, string fileToUploadPath, Account account)
        {
            var dropboxClient = new DropboxClient(account.ServicesTokens["dropbox"]);
            // var a = dropboxClient.Files.UploadSessionStartAsync().Result;
            Task.Run(() => Upload(fileToUploadPath, pathToUpload, dropboxClient, account));
            // var id = a.SessionId;
            // var b = dropboxClient.Files.UploadSessionAppendV2Async(new UploadSessionCursor(id, 0));
            // var file = File.Open(fileToUploadPath, FileMode.Open, FileAccess.Read);
            // var metadata = dropboxClient.Files.UploadAsync(pathToUpload, WriteMode.Add.Instance,
            //     body: file).Result;
            // if (metadata.IsFile)
            //     FileSystemManager.CurrentFolder.Content.Add(metadata.Name, new Infrastructure.File(pathToUpload, account));
            // else
            //     DropboxDataLoader.GetFolders(account, pathToUpload, dropboxClient);
        }

        private static async Task Upload(string localPath, string remotePath, DropboxClient client, Account account)
        {
            const int ChunkSize = 4096 * 1024;
            using (var fileStream = File.Open(localPath, FileMode.Open))
            {
                if (fileStream.Length <= ChunkSize)
                {
                    var metadata =await client.Files.UploadAsync(remotePath, body: fileStream);
                    if (metadata.IsFile)
                        FileSystemManager.CurrentFolder.Content.Add(metadata.Name, new Infrastructure.File(remotePath, account));
                    else
                        DropboxDataLoader.GetFolders(account, remotePath, client);
                }
                else
                {
                    await ChunkUpload(remotePath, fileStream, (int)ChunkSize, client, account);
                }
            }
        }

        private static async Task ChunkUpload(string path, FileStream stream, int chunkSize, DropboxClient client, Account account)
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
                            if (metadata.IsFile)
                                FileSystemManager.CurrentFolder.Content.Add(metadata.Name, new Infrastructure.File(path, account));
                            else
                                DropboxDataLoader.GetFolders(account, path, client);
                            // Console.WriteLine (fileMetadata.PathDisplay);
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