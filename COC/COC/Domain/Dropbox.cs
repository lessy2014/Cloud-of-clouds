using COC.Application;
using Ninject;

namespace COC.Domain
{
    public class Dropbox: IService
    {
        [Inject] public IDataLoader DataLoader{ get; set; }
        [Inject] public IUploader Uploader{ get; set; }
        [Inject] public IDownloader Downloader{ get; set; }
    }
}