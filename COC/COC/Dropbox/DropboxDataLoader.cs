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
using Account = COC.Application.Account;

namespace COC
{
    public static class DropboxDataLoader
    {
        public static Folder GetFolders(Account account, string path, DropboxClient client)
        {
            var metadataList = client.Files.ListFolderAsync(path).Result.Entries.ToList();
            var content = new Dictionary<string, IFileSystemUnit>();
            foreach (var metadata in metadataList)
            {
                if (metadata.IsFolder)
                {
                    var folderInside = GetFolders(account, $"{path}/{metadata.Name}", client);
                    content.Add(metadata.Name,folderInside);
                }
                else
                    content.Add(metadata.Name, new Infrastructure.File($"Root/{account.Mail}/dropbox{path}/{metadata.Name}", account));
            }
            var folder =  new Folder($"Root/{account.Mail}/dropbox{path}", content, account);
            foreach (var internalFolder in folder.Content.Values.Where(x => x is Folder)) // Добавляем для внутренних папок родительскую
            {                                                                                                    //(пришлось так написать из-за того что папки начинают с самых вложенных создаваться) 
                ((Folder) internalFolder).ParentFolder = folder;
            }
            return folder;
        }

    }
}