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
        private string name;
        private string email;

        private string path;
        // private string country;
        private List<Metadata> currentFolder;
        private string folder;

        public DataLoader(Dictionary<string, string> mailToToken)
        {
            this.mailToToken = mailToToken;
        }
        
        public void GetFolderData(string folder="")
        {
            this.folder = folder;
            var task = Task.Run(Run);
             
            try
            {
                task.Wait();
                if (path != "")
                {
                    Console.Write("Current folder: ");
                    Console.WriteLine(path);
                    Console.WriteLine();
                    foreach (var item in currentFolder)
                    {
                        Console.WriteLine(item.Name);
                    }
                }
                Console.WriteLine();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        public async Task Run()
        {
            if (folder == "")
            {
                /*path = "";
                Console.WriteLine("Current folder: root");
                Console.WriteLine();
                foreach (var token in tokens)
                    using (var dbc = new DropboxClient(token))
                    {
                        var id = await dbc.Users.GetCurrentAccountAsync();
                        mailsToTokens.Add(id.Email, token);
                        Console.WriteLine(id.Email);
                    }*/
            }
            else
            {
                if (folder.Split('/').Length == 1)
                {
                    currentToken = mailToToken[folder];
                    folder = "";
                }
                else if (folder.Split('/')[0].Length != 0)
                {
                    var length = folder.Split('/')[0].Length;
                    currentToken = mailToToken[folder.Split('/')[0]];
                    folder = folder.Remove(0, length);
                }
                using (var dbc = new DropboxClient(currentToken))
                {
                    var id = await dbc.Users.GetCurrentAccountAsync();
                    name = id.Name.DisplayName;
                    email = id.Email;
                    path = email + folder;
                    try
                    {
                        var list = await dbc.Files.ListFolderAsync(folder);
                        currentFolder = new List<Metadata>();
                        foreach (var item in list.Entries)
                        {
                            currentFolder.Add(item);
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
}