using System.Collections.Generic;
using System.IO;

namespace NTFSScan
{

    public class Scanner
    {
        public Folder Scan(string path)
        {
            var folderObject = new Folder(path, Path.GetFileName(path));

            var files = Directory.EnumerateFiles(path);

            var fileObjects = new List<File>();

            foreach (var filePath in files)
            {
                var fileInfo = new FileInfo(filePath);

                fileObjects.Add(new File(filePath, Path.GetFileName(filePath), fileInfo.Length));
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