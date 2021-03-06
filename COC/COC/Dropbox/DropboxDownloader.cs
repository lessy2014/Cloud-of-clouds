using System;
using COC.Application;
using Dropbox.Api;

namespace COC.Dropbox
{
    public class DropboxDownloader: IDownloader
    {
        public string DownloadFile(string path, string token, bool isFile)
        {
            var dropboxClient = new DropboxClient(token);
            if (isFile)
                if (!dropboxClient.Files.GetMetadataAsync(path).Result.AsFile.IsDownloadable)
                    return "This file is not downloadable";
            var link = "";
            var gotLink = false;
            var links = dropboxClient.Sharing.ListSharedLinksAsync(path).Result.Links;
            if (links.Count != 0)
            {
                foreach (var li in links)
                {
                    if (li.PathLower != path.ToLower())
                        continue;
                    gotLink = true;
                    link = li.Url;
                    break;
                }
            }
            if (!gotLink)
                link = dropboxClient.Sharing.CreateSharedLinkWithSettingsAsync(path).Result.Url;
            var parts = link.Split(new [] {"dl=0"}, StringSplitOptions.None);
            return parts[0] + "dl=1" + parts[1];
        }

    }
}