using System.IO;
using COC.Application;
using COC.Infrastructure;
using Dropbox.Api;
using Dropbox.Api.Files;
using File = System.IO.File;


namespace COC.Dropbox
{
    public static class DropboxUploader
    {
        public static void UploadFile(string pathToUpload, string fileToUploadPath, Account account)
        {
            var dropboxClient = new DropboxClient(account.ServicesTokens["dropbox"]);
            var file = File.Open(fileToUploadPath, FileMode.Open, FileAccess.Read);
            var metadata = dropboxClient.Files.UploadAsync(pathToUpload, WriteMode.Add.Instance,
                body: file).Result;
            if (metadata.IsFile)
                FileSystemManager.CurrentFolder.Content.Add(metadata.Name, new Infrastructure.File(pathToUpload, account));
            else
                DropboxDataLoader.GetFolders(account, pathToUpload, dropboxClient);
        }
    }
}