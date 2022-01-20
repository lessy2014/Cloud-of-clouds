using System.Collections.Generic;
using System.Linq;
using COC.Application;
using COC.Domain;
using YandexDisk.Client.Http;
using YandexDisk.Client.Protocol;
using Account = COC.Application.Account;

namespace COC.Yandex
{
    public class YandexDataLoader: IDataLoader
    {
        public Folder GetFolders(Account account, string path, DiskHttpApi client)
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
                    var folderInside = GetFolders(account, $"{path}/{metadata.Name}", client);
                    content.Add(metadata.Name, folderInside);
                }
                else
                    content.Add(metadata.Name, new File($"Root/{account.AccountName}/yandex{path}/{metadata.Name}", account));
            }
            var folder = new Folder($"Root/{account.AccountName}/yandex{path}", content, account);
            foreach (var internalFolder in folder.Content.Values.Where(x => x is Folder)) 
            {                                                                                                     
                ((Folder) internalFolder).ParentFolder = folder;
            }
            return folder;
        }
        
        public Folder GetFolders(Account account, string path, string token)
        {
            var dropboxClient = new DiskHttpApi(token);
            return GetFolders(account, path, dropboxClient);
        }
    }
}