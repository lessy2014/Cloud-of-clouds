using System;
using System.Collections.Generic;
using Chilkat;
using Dropbox.Api;


namespace COC.Application
{    
    public static class TokenStorage
    {
        public static readonly Dictionary<string, Account> MailToAccount = new Dictionary<string, Account>();

        public static readonly List<Application.Account> Accounts = new List<Application.Account>();

        public static OAuth2 DropboxOAuth2 = new OAuth2
        {
            ListenPort = 3017,
            AuthorizationEndpoint = "https://www.dropbox.com/oauth2/authorize",
            TokenEndpoint = "https://api.dropboxapi.com/oauth2/token",
            ClientId = "ryj3w3kellyrapb",
            ClientSecret = "1ukvw813zamkzla",
            CodeChallenge = false
        };
        
        public static OAuth2 YandexOAuth2 = new OAuth2
        {
            ListenPort = 3017,
            AuthorizationEndpoint = "https://oauth.yandex.ru/authorize",
            TokenEndpoint = "https://oauth.yandex.ru/token",
            ClientId = "49e919b546904d4eaa8dede505f9c5cd",
            ClientSecret = "ceafad130ce74956bc3a9b6f971fb003",
            CodeChallenge = false
        };
        
        public static string GetToken(OAuth2 oauth2)
        {
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

        private static string GetDropboxMail(string token)
        {
            var dbc = new DropboxClient(token);
            var a = dbc.Users.GetCurrentAccountAsync().Result;
            return a.Email;
        }

        //TODO рабочий GetYandexMail
        private static string GetYandexMail(string token)
        {
            return "sigmarblessme@gmail.com"; //а что поделать...
        }
        
        public static void AddToken(string token, string service)
        {
            var mail = service switch
            {
                "dropbox" => GetDropboxMail(token),
                "yandex" => GetYandexMail(token),
                _ => throw new ArgumentException("Unknown service")
            };

            if(MailToAccount.ContainsKey(mail))
                MailToAccount[mail].ServicesTokens[service] = token;
            else
            {
                MailToAccount.Add(mail, new Account(mail)
                {
                    ServicesTokens = {[service] = token}
                });
            }
        }
    }
}