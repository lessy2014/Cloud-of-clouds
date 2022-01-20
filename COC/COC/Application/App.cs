using System;
using System.IO;
using System.Linq;
using COC.Dropbox;
using COC.Yandex;
using Ninject;

namespace COC.Application
{
    public class App
    {
        public static void Initialize()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + '\\' + "tokens.txt";

            Account.DeleteIncorrectLines();
            foreach (var line in File.ReadLines(path))
            {
                if (line == "")
                    continue;
                var data = line.Split(' ');
                if (data.Length != 3)
                    Console.WriteLine(
                        $"WARNING: Incorrect format of line \"{line}\". Please, remove it manually in tokens.txt or using .");
                else
                {
                    var token = data[0];
                    var name = data[1];
                    var service = data[2];
                    TokenStorage.AddToken(token, name, service);
                }
            }
        }

        public static void Start()
        {
            Initialize();
            var dataLoader = new DataLoader(TokenStorage.NameToAccount.Values.ToList());
            dataLoader.InitializeFileSystem();
        }

        public static StandardKernel ConfigureContainer()
        {
            var container = new StandardKernel();
            container.Bind<IDataLoader>().To<DropboxDataLoader>().WhenInjectedInto<Domain.Dropbox>();
            container.Bind<IDownloader>().To<DropboxDownloader>().WhenInjectedInto<Domain.Dropbox>();
            container.Bind<IUploader>().To<DropboxUploader>().WhenInjectedInto<Domain.Dropbox>();
            container.Bind<IDataLoader>().To<YandexDataLoader>().WhenInjectedInto<Domain.Yandex>();
            container.Bind<IDownloader>().To<YandexDownloader>().WhenInjectedInto<Domain.Yandex>();
            container.Bind<IUploader>().To<YandexUploader>().WhenInjectedInto<Domain.Yandex>();
            return container;
        }
    }
}