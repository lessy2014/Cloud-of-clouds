using System;
using System.IO;
using System.Linq;
using COC.Domain;
using File = System.IO.File;

namespace COC.Application
{
    public static class Uploader
    {
        public static void UploadFile(IFileSystemUnit fileSystemUnit, string fileToUploadPath)
        {
            if (!File.Exists(fileToUploadPath) && !Directory.Exists(fileToUploadPath))
            {
                Console.WriteLine("File or folder does not exist!");
                return;
            }
            var splittedPath = fileSystemUnit.Path.Split('/');
            if (splittedPath.Length < 3)
            {
                Console.WriteLine("Upload in this folder is unsupported");
                return;
            }
            var path = string.Join("/", splittedPath.Skip(3));
            if (splittedPath.Length > 3)
                path = '/' + path;
            var account = fileSystemUnit.Account;
            var isFile = fileSystemUnit.GetType() == typeof(Domain.File);
            var fileName = fileToUploadPath.Split('\\').Last();
            if (isFile)
                Console.WriteLine("You can't upload file into file");
            fileSystemUnit.Service.Uploader.UploadFile(path + '/' + fileName, fileToUploadPath, account);
        }
    }
}