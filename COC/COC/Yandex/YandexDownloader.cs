using COC.Application;
using YandexDisk.Client.Http;

namespace COC.Yandex
{
    public class YandexDownloader: IDownloader
    {
        public string DownloadFile(string path, string token, bool isFile)
        {
            var yandexClient = new DiskHttpApi(token);
            var link = yandexClient.Files.GetDownloadLinkAsync(path).Result.Href;
            return link;
        }
    }
}