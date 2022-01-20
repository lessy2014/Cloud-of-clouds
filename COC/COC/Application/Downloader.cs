using System;
using System.Linq;
using COC.Domain;

namespace COC.Application
{
    public static class Downloader
    {
        public static void DownloadFile(IFileSystemUnit fileSystemUnit)
        {
            var splittedPath = fileSystemUnit.Path.Split('/');
            if (splittedPath.Length < 4)
            {
                Console.WriteLine("This file or folder is not downloadable!");
                return;
            }
            
            var service = splittedPath[2];
            var path = "/" + string.Join("/", splittedPath.Skip(3));
            var token = fileSystemUnit.Account.ServicesTokens[service];
            var isFile = fileSystemUnit.GetType() == typeof(File);
            Console.WriteLine(fileSystemUnit.Service.Downloader.DownloadFile(path, token, isFile));
        }
    }
}