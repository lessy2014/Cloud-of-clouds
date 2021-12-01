using COC.Application;

namespace COC.Infrastructure
{
    public interface IFileSystemUnit
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        
        public Account Account { get; }
    }
}