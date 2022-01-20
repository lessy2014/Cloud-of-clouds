using System.Collections.Generic;
using System.Linq;
using COC.Application;
using COC.Domain;
using Dropbox.Api;
using Account = COC.Application.Account;

namespace COC.Dropbox
{
    public class DropboxDataLoader: IDataLoader
    {
        private static Folder GetFolders(Account account, string path, DropboxClient client)
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
                    content.Add(metadata.Name, new Domain.File($"Root/{account.AccountName}/dropbox{path}/{metadata.Name}", account));
            }
            var folder =  new Folder($"Root/{account.AccountName}/dropbox{path}", content, account);
            foreach (var internalFolder in folder.Content.Values.Where(x => x is Folder)) // Добавляем для внутренних папок родительскую
            {                                                                                                    //(пришлось так написать из-за того что папки начинают с самых вложенных создаваться) 
                ((Folder) internalFolder).ParentFolder = folder;
            }
            return folder;
        }


        public Folder GetFolders(Account account, string path, string token)
        {
            var dropboxClient = new DropboxClient(token);
            return GetFolders(account, path, dropboxClient);
        }
    }
}