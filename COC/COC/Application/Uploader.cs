using System;
using System.Linq;
using COC.Infrastructure;

namespace COC.Application
{
    public static class Uploader
    {
        public static void UploadFile(IFileSystemUnit fileSystemUnit, string fileToUploadPath)
        {
            var splittedPath = fileSystemUnit.Path.Split('/');
            if (splittedPath.Length < 3)
            {
                Console.WriteLine("Upload in this folder is unsupported");
                return;
            }
            var service = splittedPath[2];
            var path = string.Join("/", splittedPath.Skip(3));
            if (splittedPath.Length > 3)
                path = '/' + path;
            var account = fileSystemUnit.Account;
            // var mail = fileSystemUnit.Mail;
            // var token = fileSystemUnit.Account.ServicesTokens[service];
            var isFile = fileSystemUnit.GetType() == typeof(Infrastructure.File);
            var fileName = fileToUploadPath.Split('\\').Last();
            if (isFile)
                Console.WriteLine("You can't upload file into file");
            fileSystemUnit.Service.Uploader.UploadFile(path + '/' + fileName, fileToUploadPath, account);
        }
    }
}