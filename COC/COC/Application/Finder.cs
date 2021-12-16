using System;
using COC.Infrastructure;

namespace COC.Application
{
    public class Finder
    {
        public static void Find(Folder folder, string name)
        {
            if (folder.Name.Contains(name))
            {
                Console.WriteLine(folder.Path);
            }
            if (folder.Content.Count != 0)
            {
                foreach (var fileSystemUnit in folder.Content.Values)
                {
                    if (fileSystemUnit is Folder unit)
                    {
                        Find(unit, name);
                    }
                }
            }
        }
    }
}