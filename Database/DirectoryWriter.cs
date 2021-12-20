using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace NTFSScan.Database
{
    public class DirectoryWriter
    {
        private readonly string connectionString;

        public DirectoryWriter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void WriteDirectory(Folder folder)
        {
            TraverseFolder(folder, null);
        }

        private void TraverseFolder(Folder folder, int? parentId)
        {
            // add folder entry
            var folderId = AddFolder(folder, parentId);
            AddPermissions(folderId, folder.AccessRules);

            foreach (var file in folder.Files)
            {
                // add file entry
                var fileId = AddFile(file, folderId);
                AddPermissions(fileId, file.AccessRules);
            }  

            foreach (var subFolder in folder.Folders)
            {
                TraverseFolder(subFolder, folderId);
            }
        }


        private int AddFolder(Folder folder, int? parentId)
        {
            var sql = "INSERT INTO dbo.[Directory] (ParentID, [Name], [IsFile]) VALUES (@ParentID, @Name, 0); SELECT SCOPE_IDENTITY()";

            using(var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.ExecuteScalar<int>(sql, new { 
                    ParentID = parentId,
                    Name = folder.Name,
                });
            }
        }

        private int AddFile(File file, int parentId)
        {
            var sql = "INSERT INTO dbo.[Directory] (ParentID, [Name], [IsFile], [Size]) VALUES (@ParentID, @Name, 1, @Size); SELECT SCOPE_IDENTITY()";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.ExecuteScalar<int>(sql, new
                {
                    ParentID = parentId,
                    Name = file.Name,
                    Size = file.Size,
                });
            }
        }

        private void AddPermissions(int directoryId, IEnumerable<FileSystemAccessRule> accessRules)
        {
            var values = new List<string>();

            foreach (var accessRule in accessRules)
            {
                values.Add($"(@DirectoryID, '{accessRule.IdentityReference.Value}', {(int)accessRule.AccessControlType}, {(int)accessRule.FileSystemRights})");
            }

            var sql = $"INSERT INTO dbo.[Permissions] (DirectoryID, [IdentityReference], [AccessControlType], [FileSystemRights]) VALUES {string.Join(",", values)}";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute(sql, new
                {
                    DirectoryID = directoryId,
                });
            }
        }

    }
}
