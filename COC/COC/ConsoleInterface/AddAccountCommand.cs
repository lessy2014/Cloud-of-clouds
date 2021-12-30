using System;
using System.ComponentModel.Design;
using System.IO;
using System.Windows.Input;
using COC.Application;
using CommandLine;

namespace COC.ConsoleInterface
{
    [Verb("add_account", HelpText = "Add an account of one of the available services to the application")  ]
    public class AddAccountCommand: ICommand
    {
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }
        
        [Value(index: 0, Required = true, HelpText = "Name of service")]
        public string ServiceName { get; set;}

        public void Execute(object parameter)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + '\\' + "tokens.txt";
            Account account = null;
            ServiceName = ServiceName.ToLower();
            if (ServiceName == "yandex")
            {
                account = TokenStorage.AddToken(TokenStorage.GetToken(TokenStorage.YandexOAuth2), ServiceName,
                    ServiceName);
                DataLoader.GetFoldersFromNewAccount(account, ServiceName);
            }
            else if (ServiceName.ToLower() == "dropbox")
            {
                account = TokenStorage.AddToken(TokenStorage.GetToken(TokenStorage.DropboxOAuth2), ServiceName,
                    ServiceName.ToLower());
                DataLoader.GetFoldersFromNewAccount(account, ServiceName);
            }
            else
                Console.WriteLine("Unsupported service");

            if (account != null)
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(account.ServicesTokens[ServiceName] + ' ' + account.AccountName + ' ' + ServiceName);
                }	
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}