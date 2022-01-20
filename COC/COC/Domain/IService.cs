using COC.Application;

namespace COC.Domain
{
    public interface IService
    {
        public IDataLoader DataLoader { get; set; }
        public IUploader Uploader { get; set; }
        public IDownloader Downloader { get; set; }
    }
}