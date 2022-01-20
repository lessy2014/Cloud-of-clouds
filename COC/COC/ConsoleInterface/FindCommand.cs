using System.Collections.Generic;
using COC.Application;
using COC.Domain;
using CommandLine;

namespace COC.ConsoleInterface
{
    [Verb("find", HelpText = "Find all files and folders with given name")]
    public class FindCommand: ICommand
    {
        [Value(index: 0, Required = false, HelpText = "Name of file or Folder")]
        public string Name { get; set; } = "";

        [Option("ext", Required = false, HelpText = "Search files only with the specified extension")]
        public ExtensionFilter ExtensionFilter{ get; set; }

        [Option('t', "type", Required = false, HelpText = "Type of object, supported arguments: 'folder' and 'file'")] 
        public TypeFilter TypeFilter{ get; set; }

        [Option("fm", Required = false, HelpText = "Show only names that completely match the request")]
        public bool FullMatch { get; set; }
        public void Execute()
        {
            Finder.Find(FileSystemManager.CurrentFolder, Name, new List<IFilter>{ExtensionFilter, TypeFilter}, FullMatch);
        }
    }

    public interface IFilter
    {
        string Value { get; set; }
    }

    public class ExtensionFilter: IFilter
    {
        public string Value { get; set; }

        public ExtensionFilter(string value)
        {
            Value = value;
        }
    } 
    public class TypeFilter: IFilter
    {
        public string Value { get; set; }

        public TypeFilter(string type)
        {
            Value = type;
        }
    }
}