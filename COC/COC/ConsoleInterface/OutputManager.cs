using System;
using COC.Domain;

namespace COC.ConsoleInterface
{
    public static class OutputManager
    {
        public static void WriteFolderData(Folder folder)
        {
            foreach (var item in folder.Content)
            {
                Console.WriteLine(item.Key);
            }
        }

        public static void WriteRootFolder()
        {
            Console.WriteLine("Current folder: Root \n");
            foreach (var folder in Folder.Root.Content)
            {
                Console.WriteLine(folder.Value.Name);
            }
        }
    }
}