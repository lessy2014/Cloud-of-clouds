using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Dropbox.Api;
using System.Threading.Tasks;
using COC.Dropbox;
using COC.Infrastructure;
using Dropbox.Api.Files;
using Dropbox.Api.Users;


namespace COC.ConsoleApp
{
    public static class OutputManager
    {
        public static void WriteFolderData(Folder folder)
        {
            Console.WriteLine($"Current folder: {folder.Path}\n");
            foreach (var item in folder.Content)
            {
                Console.WriteLine(item.Key);
            }
        }

        public static void WriteRootFolder()
        {
            Console.WriteLine("Current folder: Root \n");
            foreach (var folder in Folder.root.Content)
            {
                Console.WriteLine(folder.Value.Name);
            }
        }
    }
}