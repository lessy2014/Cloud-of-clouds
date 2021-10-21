using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Dropbox.Api;
using System.Threading.Tasks;
using Dropbox.Api.Files;
using Dropbox.Api.Users;

// using HttpWebRequest

namespace COC.Dropbox
{
    
    public class DataLoader
    {
        private readonly Dictionary<string, string> mailToToken;
        // private string currentEmail;

        public Infrastructure.Folder GetFolder(string folderPath="")
        {
            var dropboxPath = GetDropboxPath(folderPath);
            var email = GetEmail(folderPath);
            var folderContentTask = Task.Run(() => AsyncGetFolderData(dropboxPath, email));
            try
            {
                folderContentTask.Wait();
                return new Infrastructure.Folder(email+dropboxPath, folderContentTask.Result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            throw new Exception("Can not get folder data");
        }
        
        private async Task<List<Metadata>> AsyncGetFolderData(string dropboxPath, string email)
        {
            var token = mailToToken[email];
            using var dropboxClient = new DropboxClient(token);
            try
            {
                var list = await dropboxClient.Files.ListFolderAsync(dropboxPath);
                return list.Entries.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine("Whatever problem happened" + e);
            }
            throw new Exception("Can not get folder data");
        }
        
        private static string GetDropboxPath(string folderPath)
        {
            var folderInPath = folderPath.Split('/');
            return folderInPath.Length == 1 ? "" : folderPath.Substring(folderInPath[0].Length);
        }

        private string GetEmail(string folderPath)
        {
            return folderPath.Split('/')[0];
            //TODO работа с файлами последней использованной почты без повторного указания 
            // var email = folderPath.Split('/')[0];
            // if (email != "")
            //     currentEmail = email;
            // return currentEmail;
        }
        
        public DataLoader(Dictionary<string, string> mailToToken)
        {
            this.mailToToken = mailToToken;
        }
    }
}