using COC.Application;

namespace COC.Infrastructure
{
    public interface IFileSystemUnit
    {
        public string Path { get; set; }
        public string Name { get; set; }

        public Account Account { get; }

        public IService Service { get; set; }
    }
}