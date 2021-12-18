using System.Collections.Generic;
using System.IO;

namespace NTFSScan
{
    public class File
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public File(string path, string name)
        {
            Path = path;
            Name = name;
        }
    }

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

    public class Scanner
    {

        public Scanner()
        {
        }

        public Folder Scan(string path)
        {
            var folderObject = new Folder(path, Path.GetFileName(path));

            var files = Directory.EnumerateFiles(path);

            var fileObjects = new List<File>();

            foreach (var filePath in files)
            {
                fileObjects.Add(new File(filePath, Path.GetFileName(filePath)));
            }

            folderObject.Files = fileObjects;

            var folders = Directory.EnumerateDirectories(path);
    
            var folderObjects = new List<Folder>();

            foreach (var folderPath in folders)
            {
                folderObjects.Add(Scan(folderPath));
            }

            folderObject.Folders = folderObjects;

            return folderObject;
        }
    }

}