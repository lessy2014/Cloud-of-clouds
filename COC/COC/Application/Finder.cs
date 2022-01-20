using System;
using System.Collections.Generic;
using System.Linq;
using COC.ConsoleInterface;
using COC.Domain;

namespace COC.Application
{
    public class Finder
    {
        public static void Find(Folder folder, string name, List<IFilter> filters, bool fullMatch)
        {
            try
            {
                if (name == "" && !IsHaveFilters(filters))
                {
                    Console.WriteLine("No name or filters specified");
                    return;
                }

                if (SatisfiesConditions(folder, name, filters, fullMatch))
                {
                    Console.WriteLine(folder.Path);
                }

                if (folder.Content.Count == 0) return;
                foreach (var fileSystemUnit in folder.Content.Values)
                {
                    if (SatisfiesConditions(fileSystemUnit, name, filters, fullMatch) &&
                        fileSystemUnit.GetType() == typeof(File))
                    {
                        Console.WriteLine(fileSystemUnit.Path);
                    }

                    if (fileSystemUnit is Folder unit)
                    {
                        Find(unit, name, filters, fullMatch);
                    }

                }
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Wrong type of object, only 'file' and 'folder' are supported");
            }
        }

        private static bool SatisfiesConditions(IFileSystemUnit unit, string name, List<IFilter> filters, bool fullMatch)
        {
            if (!unit.Name.Contains(name) || (fullMatch && unit.Name != name))
                return false;
            foreach (var filter in filters)
            {
                if(filter is null)
                    continue;
                var filterType = filter.GetType();
                if (filterType == typeof(ExtensionFilter) &&
                    unit.Name.Split('.').Last() != filter.Value)
                    return false;
                if (filterType == typeof(TypeFilter))
                {

                    var t = GetTypeByName(filter.Value);
                    if (unit.GetType() != t)
                        return false;
                }
            }
            return true;
        }

        private static Type GetTypeByName(string typeName)
        {
            return typeName.ToLower() switch
            {
                "folder" => typeof(Folder),
                "file" => typeof(File),
                _ => throw new ArgumentException()
            };
        }

        private static bool IsHaveFilters(List<IFilter> filters)
        {
            return filters.Any(filter => filter != null);
        }
    }
}