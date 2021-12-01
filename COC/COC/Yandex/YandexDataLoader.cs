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

namespace COC
{
    public static class YandexDataLoader
    {
        public static Folder GetFolders(string mail, string path, DiskHttpApi client)
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
                    var folderInside = GetFolders(mail, $"{path}/{metadata.Name}", client);
                    content.Add(metadata.Name, folderInside);
                }
                else
                    content.Add(metadata.Name, new Infrastructure.File($"Root/{mail}/yandex{path}/{metadata.Name}", mail));
            }
            var folder =  new Folder($"Root/{mail}/yandex{path}", content, mail);
            foreach (var internalFolder in folder.Content.Values.Where(x => x is Folder)) // Добавляем для внутренних папок родительскую
                //(пришлось так написать из-за того что папки начинают с самых вложенных создаваться) 
            {
                ((Folder) internalFolder).ParentFolder = folder;
            }

            return folder;
        }
    }
}