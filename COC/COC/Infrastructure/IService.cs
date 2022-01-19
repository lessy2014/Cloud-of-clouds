using COC.Application;

namespace COC.Infrastructure
{
    public interface IService
    {
        public IDataLoader DataLoader { get; set; }
        public IUploader Uploader { get; set; }
        public IDownloader Downloader { get; set; }
    }
}