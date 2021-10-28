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
                var rootFolder = GetFolder(mail, "", dropboxClient);
                root.Content.Add(rootFolder.Name, rootFolder);
            }
            Folder.SetRoot(root);
        }

        public Folder GetFolder(string email, string path, DropboxClient client)
        {
            var metadataList = client.Files.ListFolderAsync(path).Result.Entries.ToList();
            var content = new Dictionary<string, IFileSystemUnit>();
            foreach (var metadata in metadataList)
            {
                if (metadata.IsFolder)
                {
                    var folderInside = GetFolder(email, path + '/' + metadata.Name, client);
                    content.Add(metadata.Name,folderInside);
                }
                else
                    content.Add(metadata.Name, new Infrastructure.File(path + '/' +metadata.Name));
            }
            return new Folder(email + path, content);
        }


        public DataLoader(Dictionary<string, string> mailToToken)
        {
            this.mailToToken = mailToToken;
        }
    }
}