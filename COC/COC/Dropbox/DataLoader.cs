using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Dropbox.Api;
using System.Threading.Tasks;
using COC.Infrastructure;
using Dropbox.Api.Files;
using Dropbox.Api.Users;
using YandexDisk.Client.Http;
using YandexDisk.Client.Protocol;

// using HttpWebRequest

namespace COC.Dropbox
{
    
    public class DataLoader //TODO рефактор всего класса
    {
        private readonly Dictionary<string, Dictionary<string, string>> mailToToken;

        public void GetFolders()
        {
            var root = new Folder("Root", new Dictionary<string, IFileSystemUnit>(), null);
            foreach (var mailTokenPair in mailToToken)
            {
                var mail = mailTokenPair.Key;
                var services = mailTokenPair.Value;
                var mailFolder = new Folder($"Root/{mail}");
                var mailFolderContent = new Dictionary<string, IFileSystemUnit>();
                foreach (var service in services)
                {
                    var token = service.Value;
                    if (service.Key == "yandex")
                    {
                        var yandexClient = new DiskHttpApi(token);
                        mailFolderContent.Add("yandex", GetYandexFolder(mail, "", yandexClient));
                    }

                    if (service.Key == "dropbox")
                    {
                        var dropboxClient = new DropboxClient(token);
                        mailFolderContent.Add("dropbox", GetDropboxFolder(mail, "", dropboxClient));
                    }
                }
                mailFolder.Content = mailFolderContent;
                mailFolder.PreviousFolder = root;
                foreach (var service in mailFolder.Content)
                {
                    ((Folder) mailFolder.Content[service.Key]).PreviousFolder = mailFolder;
                }
                root.Content.Add(mailFolder.Name, mailFolder);
            }
            Folder.SetRoot(root);
            FileSystemManager.CurrentFolder = root;
        }

        public Folder GetYandexFolder(string mail, string path, DiskHttpApi client)
        {
            var tPath = "/"; //TODO кринж кринжов. Рут-папка в яндексе находится по адресу "/", а в dropbox по адресу ""
            if (path != "")  //при этом путь до остальных папок в системе выглядит абсолютно одинаково, поэтому запилил такой костыль
                tPath = path;
            var metadataList = client.MetaInfo.GetInfoAsync(new ResourceRequest{Path = tPath}).Result.Embedded.Items;
            var content = new Dictionary<string, IFileSystemUnit>();
            foreach (var metadata in metadataList)
            {
                if (metadata.Type is ResourceType.Dir)
                {
                    var folderInside = GetYandexFolder(mail, $"{path}/{metadata.Name}", client);
                    content.Add(metadata.Name, folderInside);
                }
                else
                    content.Add(metadata.Name, new Infrastructure.File($"Root/{mail}/yandex{path}/{metadata.Name}", mail));
            }
            var folder =  new Folder($"Root/{mail}/yandex{path}", content, mail);
            foreach (var internalFolder in folder.Content.Values.Where(x => x is Folder)) // Добавляем для внутренних папок родительскую
                //(пришлось так написать из-за того что папки начинают с самых вложенных создаваться) 
            {
                ((Folder) internalFolder).PreviousFolder = folder;
            }

            return folder;
        }

        public Folder GetDropboxFolder(string mail, string path, DropboxClient client)
        {
            var metadataList = client.Files.ListFolderAsync(path).Result.Entries.ToList();
            var content = new Dictionary<string, IFileSystemUnit>();
            foreach (var metadata in metadataList)
            {
                if (metadata.IsFolder)
                {
                    var folderInside = GetDropboxFolder(mail, $"{path}/{metadata.Name}", client);
                    content.Add(metadata.Name,folderInside);
                }
                else
                    content.Add(metadata.Name, new Infrastructure.File($"{path}/{metadata.Name}", mail));
            }
            var folder =  new Folder($"Root/{mail}/dropbox{path}", content, mail);
            foreach (var internalFolder in folder.Content.Values.Where(x => x is Folder)) // Добавляем для внутренних папок родительскую
                                                                                                                 //(пришлось так написать из-за того что папки начинают с самых вложенных создаваться) 
            {
                ((Folder) internalFolder).PreviousFolder = folder;
            }

            return folder;
        }

        public DataLoader(Dictionary<string, Dictionary<string, string>> mailToToken)
        {
            this.mailToToken = mailToToken;
        } 
    }
}