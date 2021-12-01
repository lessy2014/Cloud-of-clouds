using System.IO;
using Dropbox.Api;
using Dropbox.Api.Files;
using File = System.IO.File;


namespace COC.Dropbox
{
    public static class DropboxUploader
    {
        public static void UploadFile(string pathToUpload, string fileToUploadPath, string token)
        {
            var dropboxClient = new DropboxClient(token);
            var file = File.Open(fileToUploadPath, FileMode.Open);
            var _ = dropboxClient.Files.UploadAsync(pathToUpload, WriteMode.Add.Instance,
                body: file).Result;
        }
    }
}