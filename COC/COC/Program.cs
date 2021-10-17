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

namespace COC
{
    class Program
    {
        static void Main(string[] args)
        {
            //string token = "KRvzqGRO6vAAAAAAAAAAAc2uCm6q-u2hLD8e-IhvyDrRIX6FESDBqyp90dY31HvU";
            //var a = new FrmDropBoxWhtverThatMeans(token);
            // a.GetFolderData("");
            // Console.WriteLine();
            // a.GetFolderData(@"/Folder1");
            // Console.WriteLine();
            // a.GetFolderData(@"/Folder1/FolderInsideFolder1");
            var b = new FrmDropBoxWhtverThatMeans();
            b.GetFolderData();
        }
    }

    public class FrmDropBoxWhtverThatMeans
    {
         private string token;
         private string name;
         private string email;

         private string path;
         // private string country;
         private List<Metadata> currentFolder;
         private string folder;

         public FrmDropBoxWhtverThatMeans(string token="")
         {
             this.token = token;
         }

         public FrmDropBoxWhtverThatMeans()
         {
             Chilkat.OAuth2 oauth2 = new Chilkat.OAuth2();
             
            oauth2.ListenPort = 3017;
            

            oauth2.AuthorizationEndpoint = "https://www.dropbox.com/oauth2/authorize";
            oauth2.TokenEndpoint = "https://api.dropboxapi.com/oauth2/token";
            
            oauth2.ClientId = "ryj3w3kellyrapb";
            oauth2.ClientSecret = "1ukvw813zamkzla";
            oauth2.CodeChallenge = false;
            
            string url = oauth2.StartAuth();
            if (oauth2.LastMethodSuccess != true) {
                Console.WriteLine(oauth2.LastErrorText);
                return;
            }
            
            System.Diagnostics.Process.Start(url);
            int numMsWaited = 0;
            while ((numMsWaited < 30000) && (oauth2.AuthFlowState < 3)) {
                oauth2.SleepMs(100);
                numMsWaited = numMsWaited + 100;
            }
            
            if (oauth2.AuthFlowState < 3) {
                oauth2.Cancel();
                Console.WriteLine("No response from the browser!");
                return;
            }
            
            if (oauth2.AuthFlowState == 5) {
                Console.WriteLine("OAuth2 failed to complete.");
                Console.WriteLine(oauth2.FailureInfo);
                return;
            }

            if (oauth2.AuthFlowState == 4) {
                Console.WriteLine("OAuth2 authorization was denied.");
                Console.WriteLine(oauth2.AccessTokenResponse);
                return;
            }

            if (oauth2.AuthFlowState != 3) {
                Console.WriteLine("Unexpected AuthFlowState:" + Convert.ToString(oauth2.AuthFlowState));
                return;
            }

            // Console.WriteLine("OAuth2 authorization granted!");
            //
            // Console.WriteLine("Access Token Response = " + oauth2.AccessTokenResponse);
            //
            // Console.WriteLine("Access Token = " + oauth2.AccessToken);
            token = oauth2.AccessToken;
            // Console.WriteLine(token);
         }

         public void GetFolderData(string folder="")
         {
             this.folder = folder;
             var task = Task.Run(Run);
             try
             {
                 task.Wait();
                 // Console.WriteLine(name);
                 // Console.WriteLine(email);
                 // Console.WriteLine(country);
                 Console.Write("Current folder: ");
                 Console.WriteLine(path);
                 Console.WriteLine();
                 foreach (var item in currentFolder)
                 {
                     Console.WriteLine(item.Name);
                 }
                 
             }
             catch (Exception e)
             {
                 Console.WriteLine(e);
             }
         }

         public async Task Run()
        {
            using (var dbc = new DropboxClient(token))
            {
                var id = await dbc.Users.GetCurrentAccountAsync();
                name = id.Name.DisplayName;
                email = id.Email;
                path = email + folder;
                // country = id.Country;
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