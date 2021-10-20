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
            // string token1 = "KRvzqGRO6vAAAAAAAAAAAc2uCm6q-u2hLD8e-IhvyDrRIX6FESDBqyp90dY31HvU"; TODO Не работает, а хочется, я так понимаю
            string token2 = "gceFacZd034AAAAAAAAAAXDBtTWhQV21RRk1UZ4kR-2mx_aauXw0N2CgbirC5YNe";
            // TokenStorage.AddToken(token1);
            TokenStorage.AddToken(token2);
            var dataLoader = new DataLoader(TokenStorage.MailToToken);
            // var folder1 = dataLoader.GetFolder("bir.ssss@mail.ru"); TODO Не работает, а хочется, я так понимаю
            var folder2 = dataLoader.GetFolder("sigmarblessme@gmail.com/Folder1");
            // var folder3 = dataLoader.GetFolder("/Folder1"); TODO Не работает, а хочется
            // var folder4 = dataLoader.GetFolder(""); TODO Не работает, а хочется
            // OutputManager.WriteFolderData(folder1);
            OutputManager.WriteFolderData(folder2);
            // OutputManager.WriteFolderData(folder3);
            // OutputManager.WriteFolderData(folder4);
        }
    }
}