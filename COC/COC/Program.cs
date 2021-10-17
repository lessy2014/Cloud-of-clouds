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
            string token1 = "KRvzqGRO6vAAAAAAAAAAAc2uCm6q-u2hLD8e-IhvyDrRIX6FESDBqyp90dY31HvU";
            string token2 = "kjAhtJMFcp4AAAAAAAAAAX_wzLndk7IlBAYVeYs0yLmkq_d_0Kt_wdQm5lvVVmNJ";
            var a = new FrmDropBoxWhtverThatMeans(token1);
            a.AddToken(token2);
            a.GetFolderData("");
            a.GetFolderData("sigmarblessme@gmail.com");
            a.GetFolderData("sigmarblessme@gmail.com/Folder1");
             // var b = new FrmDropBoxWhtverThatMeans(token2);
             // b.GetFolderData("");
             // Console.WriteLine();
             // a.GetFolderData(@"/Folder1");
             // Console.WriteLine();
             // a.GetFolderData(@"/Folder1/FolderInsideFolder1");
             // var b = new FrmDropBoxWhtverThatMeans();
             // b.GetFolderData();
        }
    }

    public class FrmDropBoxWhtverThatMeans
    {
         private string currentToken;
         private List<string> tokens;
         private Dictionary<string, string> mailsToTokens;
         private string name;
         private string email;

         private string path;
         // private string country;
         private List<Metadata> currentFolder;
         private string folder;

         public FrmDropBoxWhtverThatMeans(string token)
         {
             tokens = new List<string>();
             currentToken = token;
             tokens.Add(token);
         }

         public FrmDropBoxWhtverThatMeans()
         { 
             tokens = new List<string>();
             AddToken();
         }

         public void AddToken()
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
             currentToken = oauth2.AccessToken;
             tokens.Add(currentToken);
         }

         public void AddToken(string token)
         {
             tokens.Add(token);
             currentToken = token;
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
                mailsToTokens = new Dictionary<string, string>();
                path = "";
                Console.WriteLine("Current folder: root");
                Console.WriteLine();
                foreach (var token in tokens)
                    using (var dbc = new DropboxClient(token))
                    {
                        var id = await dbc.Users.GetCurrentAccountAsync();
                        mailsToTokens.Add(id.Email, token);
                        Console.WriteLine(id.Email);
                    }
            }
            else
            {
                if (folder.Split('/').Length == 1)
                {
                    currentToken = mailsToTokens[folder];
                    folder = "";
                }
                else if (folder.Split('/')[0].Length != 0)
                {
                    var length = folder.Split('/')[0].Length;
                    currentToken = mailsToTokens[folder.Split('/')[0]];
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