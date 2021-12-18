using System.Collections.Generic;

namespace NTFSScan
{
    public class Folder
    {
        public IEnumerable<Folder> Folders { get; set;}
        public IEnumerable<File> Files { get; set;}
        public string Path { get; set; }
        public string Name { get; set; }
        public Folder(string path, string name)
        {
            Path = path;
            Name = name;
        }
    }

}