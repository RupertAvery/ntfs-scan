using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace NTFSScan
{

    public class Scanner
    {
        public Action<string> OnScanFolder { get; set; }

        public Folder ScanFolder(string path)
        {
            return GetFolder(path);
        }

        private IEnumerable<File> GetFiles(string path)
        {

            var files = Directory.EnumerateFiles(path);

            var fileObjects = new List<File>();

            foreach (var filePath in files)
            {
                fileObjects.Add(GetFile(filePath));
            }

            return fileObjects;
        }

        private IEnumerable<Folder> GetFolders(string path)
        {
            var folders = Directory.EnumerateDirectories(path);

            var folderObjects = new List<Folder>();

            foreach (var folderPath in folders)
            {
                folderObjects.Add(GetFolder(folderPath));
            }

            return folderObjects;
        }


        private Folder GetFolder(string folderPath)
        {
            OnScanFolder?.Invoke(folderPath);

            var folderObject = new Folder(folderPath, Path.GetFileName(folderPath));

            folderObject.Files = GetFiles(folderPath);
            folderObject.Folders = GetFolders(folderPath);
            folderObject.AccessRules = GetAccessRules(folderPath);

            return folderObject;

        }

        private File GetFile(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            var fileObject = new File(filePath, Path.GetFileName(filePath), fileInfo.Length);

            fileObject.AccessRules = GetAccessRules(filePath);

            return fileObject;
        }

        private IEnumerable<FileSystemAccessRule> GetAccessRules(string path)
        {
            var sercurity = new DirectorySecurity(path, AccessControlSections.Access);
            var arc = sercurity.GetAccessRules(true, true, typeof(NTAccount));
            var accessRules = new List<FileSystemAccessRule>();
            foreach (FileSystemAccessRule rule in arc)
            {
                accessRules.Add(rule);
            }
            return accessRules;
        }


    }

}