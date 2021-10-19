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
        private Dictionary<string, string> mailToToken;
        private string currentToken;
        private List<string> tokens;
        private string email;
        private List<Metadata> currentFolderContent;
        private string pathInDropbox;

        public DataLoader(Dictionary<string, string> mailToToken)
        {
            this.mailToToken = mailToToken;
        }

        private void SetupPath(string folderPath)
        {
            var folderInPath = folderPath.Split('/');
            email = folderInPath[0];
            if (folderInPath.Length == 1)
            {
                pathInDropbox = "";
            }
            else if (pathInDropbox.Split('/')[0].Length != 0)
            {
                pathInDropbox = folderPath.Substring(folderInPath[0].Length);
            }
        }
        
        public Infrastructure.Folder GetFolder(string folderPath="")
        {
            SetupPath(folderPath);
            var task = Task.Run(AsyncGetFolderData);
            try
            {
                task.Wait();
                return new Infrastructure.Folder(email+pathInDropbox, currentFolderContent);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            throw new Exception("Can not get folder data");

        }

        private async Task AsyncGetFolderData()
        {
            currentToken = mailToToken[email];
            using (var dropboxClient = new DropboxClient(currentToken))
            {
                var id = await dropboxClient.Users.GetCurrentAccountAsync();
                email = id.Email;
                try
                {
                    var list = await dropboxClient.Files.ListFolderAsync(pathInDropbox);
                    currentFolderContent = new List<Metadata>();
                    foreach (var item in list.Entries)
                    {
                        currentFolderContent.Add(item);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Whatever problem happened" + e);
                }
            }
        }
    }
}