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

// using HttpWebRequest

namespace COC.Dropbox
{
    
    public class DataLoader
    {
        private readonly Dictionary<string, string> mailToToken;

        public void GetFolders()
        {
            var root = new Folder("Root", new Dictionary<string, IFileSystemUnit>());
            foreach (var mailTokenPair in mailToToken)
            {
                var mail = mailTokenPair.Key;
                var token = mailTokenPair.Value;
                using var dropboxClient = new DropboxClient(token);
                var mailFolderContent = new Dictionary<string, IFileSystemUnit>
                    {{"dropbox", GetDropboxFolder(mail, "", dropboxClient)}};
                var mailFolder = new Folder($"Root/{mail}", mailFolderContent, root);
                ((Folder) mailFolder.Content["dropbox"]).PreviousFolder = mailFolder; // Добавляем родительскую папку для папки dropbox
                //var rootFolder = GetDropboxFolder(mail, "", dropboxClient);
                root.Content.Add(mailFolder.Name, mailFolder);
            }
            Folder.SetRoot(root);
            FileSystemManager.CurrentFolder = root;
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
                    content.Add(metadata.Name, new Infrastructure.File($"{path}/{metadata.Name}"));
            }
            var folder =  new Folder($"Root/{mail}/dropbox{path}", content);
            foreach (var internalFolder in folder.Content.Values.Where(x => x is Folder)) // Добавляем для внутренних папок родительскую
                                                                                                                 //(пришлось так написать из-за того что папки начинают с самых вложенных создаваться) 
            {
                ((Folder) internalFolder).PreviousFolder = folder;
            }

            return folder;
        }

        public DataLoader(Dictionary<string, string> mailToToken)
        {
            this.mailToToken = mailToToken;
        }
    }
}