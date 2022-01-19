namespace COC.Application
{
    public interface IDownloader
    {
        public string DownloadFile(string path, string token, bool isFile);
    }
}