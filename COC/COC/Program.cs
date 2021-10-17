using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Dropbox.Api;
using System.Threading.Tasks;
using Dropbox.Api.Files;

// using HttpWebRequest

namespace COC
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new FrmDropBoxWhtverThatMeans();
        }
    }

    public class FrmDropBoxWhtverThatMeans
    {
         private static string token = "im6lSCpPhb4AAAAAAAAAAV4EGQ-XyozZ24sRqYMFP-vcE280t4VJRsNL5keWa5iy";
         private static string nome;
         private static string email;
         private static string pais;
         static List<Metadata> entradas = new List<Metadata>();

         public FrmDropBoxWhtverThatMeans()
         {
             var task = Task.Run(Run);
             try
             {
                 task.Wait();

                 foreach (var item in entradas)
                 {
                     Console.WriteLine(item.Name);
                 }
                 
                 Console.WriteLine(nome);
                 Console.WriteLine(email);
                 Console.WriteLine(pais);
             }
             catch (Exception e)
             {
                 Console.WriteLine(e);
             }
         }

         public static async Task Run()
        {
            using (var dbc = new DropboxClient(token))
            {
                var id = await dbc.Users.GetCurrentAccountAsync();
                nome = id.Name.DisplayName;
                email = id.Email;
                pais = id.Country;
                var folder = @"/Folder1";
                folder = @"";
                try
                {
                    var list = await dbc.Files.ListFolderAsync(folder);

                    foreach (var item in list.Entries)
                    {
                        entradas.Add(item);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Whatever problem happened" + e);
                }
            }
        }
    }
}