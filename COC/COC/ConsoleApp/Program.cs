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
using Dropbox.Api.Files;
using Dropbox.Api.Users;

namespace COC.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            string token1 = "cj6c3BNC_tYAAAAAAAAAARMr2ctpsmeQPOVflSmUu26Jsp5_C5rvlQNcoYMBYS1l";
            string token2 = "gceFacZd034AAAAAAAAAAXDBtTWhQV21RRk1UZ4kR-2mx_aauXw0N2CgbirC5YNe";
            TokenStorage.AddToken(token1);
            TokenStorage.AddToken(token2);
            var dataLoader = new DataLoader(TokenStorage.MailToToken);
            dataLoader.GetFolders();
            OutputManager.WriteRootFolder();
            var folder1 = Infrastructure.FileSystemManager.GetFolder("bir.ssss@mail.ru");
            var folder2 = Infrastructure.FileSystemManager.GetFolder("/dropbox");
            var folder3 = Infrastructure.FileSystemManager.GetFolder("sigmarblessme@gmail.com/dropbox/Folder1");
            var folder4 = Infrastructure.FileSystemManager.GetFolder("bir.ssss@mail.ru/dropbox");
            OutputManager.WriteFolderData(folder1);
            OutputManager.WriteFolderData(folder2);
            OutputManager.WriteFolderData(folder3);
            OutputManager.WriteFolderData(folder4);
        }
    }
}