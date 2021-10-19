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


namespace COC.Dropbox
{    
    public static class TokenStorage
    {
        public static Dictionary<string, string> mailToToken = new();
        public static string GetToken()
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
                return "" ;
            }
            
            System.Diagnostics.Process.Start(url);
            int numMsWaited = 0;
            while ((numMsWaited < 30000) && (oauth2.AuthFlowState < 3)) {
                oauth2.SleepMs(100);
                numMsWaited = numMsWaited + 100;
            }

            switch (oauth2.AuthFlowState)
            {
                case <3:
                    oauth2.Cancel();
                    Console.WriteLine("No response from the browser!");
                    return "";
                case 5:
                    Console.WriteLine("OAuth2 failed to complete.");
                    Console.WriteLine(oauth2.FailureInfo);
                    return "";
                case 4:
                    Console.WriteLine("OAuth2 authorization was denied.");
                    Console.WriteLine(oauth2.AccessTokenResponse);
                    return "";
                case >5:
                    Console.WriteLine("Unexpected AuthFlowState:" + Convert.ToString(oauth2.AuthFlowState));
                    return "";
            }
            return oauth2.AccessToken;
        }

        private static string GetMail(string token)
        {
            var dbc = new DropboxClient(token);
            var a = dbc.Users.GetCurrentAccountAsync().Result;
            return a.Email;
        }
        public static void AddToken(string token)
        {
            mailToToken[GetMail(token)] = token;
        }
    }
}