using COC.Infrastructure;

namespace COC.Application
{
    public interface IDataLoader
    {
        public Folder GetFolders(Account account, string path, string token);
    }
}