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
            string token1 = "KRvzqGRO6vAAAAAAAAAAAc2uCm6q-u2hLD8e-IhvyDrRIX6FESDBqyp90dY31HvU";
            string token2 = "kjAhtJMFcp4AAAAAAAAAAX_wzLndk7IlBAYVeYs0yLmkq_d_0Kt_wdQm5lvVVmNJ";
            TokenGetter.AddToken(token1);
            TokenGetter.AddToken(token2);
            var dataLoader = new DataLoader(TokenGetter.mailToToken);
            dataLoader.GetFolderData("sigmarblessme@gmail.com");
            dataLoader.GetFolderData("sigmarblessme@gmail.com/Folder1");
        }
    }
}