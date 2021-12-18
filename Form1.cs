using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NTFSScan
{
    public partial class Form1 : Form
    {
        private Scanner scanner;

        public Form1()
        {
            InitializeComponent();

            // Set the view to display columns
            listViewFiles.View = View.Details;
            // Create the columns definitions
            listViewFiles.Columns.Clear();
            listViewFiles.Columns.Add("FileName", 150);
            listViewFiles.Columns.Add("Size");
            listViewFiles.FullRowSelect = true;

            listViewAccessRules.View = View.Details;
            listViewAccessRules.Columns.Clear();
            listViewAccessRules.Columns.Add("Identity");
            listViewAccessRules.Columns.Add("Type");
            listViewAccessRules.Columns.Add("Rights");
            listViewAccessRules.FullRowSelect = true;
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();

            textBoxPath.Text = folderBrowserDialog1.SelectedPath;

            scanner = new Scanner();

            Task.Run(() => scanner.ScanFolder(textBoxPath.Text))
                .ContinueWith(t =>
                {
                    try
                    {
                        if (t.IsCompletedSuccessfully)
                        {
                            var folder = t.Result;
                            BuildTreeView(folder);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
        }

        private void BuildTreeView(Folder folder)
        {
            ClearNodes();

            var rootNode = CreateNode(folder);

            foreach (var subfolder in folder.Folders)
            {
                var subNode = AddSubNode(rootNode, subfolder);
                if (subNode != null)
                {
                    BuildChildTreeView(subNode, subfolder);
                }
            }

            Expand(rootNode);
        }

        private void BuildChildTreeView(TreeNode node, Folder folder)
        {
            foreach (var subfolder in folder.Folders)
            {
                var subNode = AddSubNode(node, subfolder);
                if (subNode != null)
                {
                    BuildChildTreeView(subNode, subfolder);
                }
            }
        }

        private void ClearNodes()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => treeViewFolders.Nodes.Clear()));
            }
            else
            {
                treeViewFolders.Nodes.Clear();
            }
        }

        private void Expand(TreeNode node)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => node.Expand()));
            }
            else
            {
                node.Expand();
            }
        }

        private TreeNode CreateNode(Folder folder)
        {
            var newNode = new TreeNode(folder.Name);
            newNode.Tag = folder;

            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => treeViewFolders.Nodes.Add(newNode)));
            }
            else
            {
                treeViewFolders.Nodes.Add(newNode);
            }

            return newNode;
        }

        private TreeNode AddSubNode(TreeNode node, Folder folder)
        {
            var newNode = new TreeNode(folder.Name);
            newNode.Tag = folder;

            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => node.Nodes.Add(newNode)));
            }
            else
            {
                node.Nodes.Add(newNode);
            }

            return newNode;
        }

        private void treeViewFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var folder = (Folder)e.Node.Tag;
            if (folder != null)
            {
                listViewFiles.Items.Clear();
                foreach (var file in folder.Files)
                {
                    var item = listViewFiles.Items.Add(file.Name);
                    item.Tag = file;
                    item.SubItems.Add(ToFileSizeString(file.Size));
                }

                ShowAccessRules(folder.AccessRules);
            }
        }

        private string ToFileSizeString(long size)
        {
            if (size >= 1073741824)
            {
                var fsize = size / 1073741824f;
                return $"{fsize:F2}GB";
            }
            else if (size >= 1048576)
            {
                var fsize = size / 1048576f;
                return $"{fsize:F2}MB";
            }
            else if (size >= 1024)
            {
                var fsize = size / 1024f;
                return $"{fsize:F2}KB";
            }
            return $"{size}B";
        }

        private void listViewFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            var listView = (ListView)sender;
            if(listView.SelectedItems.Count > 0)
            {
                var item = listView.SelectedItems[0];
                var file = (File)item.Tag;
                ShowAccessRules(file.AccessRules);
            }
        }

        private void ShowAccessRules(IEnumerable<FileSystemAccessRule> accessRules)
        {
            listViewAccessRules.Items.Clear();
            foreach (var rule in accessRules)
            {
                listViewAccessRules.Items.Add(new ListViewItem(new[] { 
                    rule.IdentityReference.Value, 
                    rule.AccessControlType.ToString(), 
                    rule.FileSystemRights.ToString() }
                ));
            }
        }

        private void listViewAccessRules_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

}