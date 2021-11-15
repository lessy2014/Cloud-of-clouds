using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Dropbox.Api;
using System.Threading.Tasks;
using COC.Dropbox;
using COC.Infrastructure;
using Dropbox.Api.Files;
using Dropbox.Api.Users;

namespace COC.ConsoleApp
{
    public class Program
    {
        private static bool isRunning = true;
        static void Main(string[] args)
        {
            Initialize();
            var dataLoader = new DataLoader(TokenStorage.MailToToken);
            dataLoader.GetFolders();
            while(isRunning)
            {
                Console.Write($"{FileSystemManager.CurrentFolder.Path}> ");
                InputManager.ReadCommand(ref isRunning, TokenStorage.MailToToken);    
            }

            /*var folder1 = Infrastructure.FileSystemManager.GetFolder("bir.ssss@mail.ru");
            var folder2 = Infrastructure.FileSystemManager.GetFolder("/dropbox");
            var folder3 = Infrastructure.FileSystemManager.GetFolder("sigmarblessme@gmail.com/dropbox/Folder1");
            var folder4 = Infrastructure.FileSystemManager.GetFolder("bir.ssss@mail.ru/dropbox");
            OutputManager.WriteFolderData(folder1);
            OutputManager.WriteFolderData(folder2);
            OutputManager.WriteFolderData(folder3);
            OutputManager.WriteFolderData(folder4);*/
        }

        private static void Initialize()
        {
            string token1 = "_GIK0vRGaikAAAAAAAAAAVBniXPD8iG8Hw-LtrF-3yct_S_Gpp3-i41u5rhaVEkj";
            string token2 = "gceFacZd034AAAAAAAAAAXDBtTWhQV21RRk1UZ4kR-2mx_aauXw0N2CgbirC5YNe";
            string token3 = "AQAAAABZ-f3MAAd6tWwREaOfEE3Qiv0H4XLT0KY";
            TokenStorage.AddDropboxToken(token1);
            TokenStorage.AddDropboxToken(token2);
            TokenStorage.AddYandexToken(token3);
        }
    }
}