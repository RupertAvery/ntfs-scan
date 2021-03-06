using NTFSScan.Database;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NTFSScan
{
    public partial class Form1 : Form
    {
        private Scanner scanner;
        private CancellationTokenSource cancellationTokenSource;

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
            listViewFiles.GridLines = true;

            listViewAccessRules.View = View.Details;
            listViewAccessRules.Columns.Clear();
            listViewAccessRules.Columns.Add("Identity", 150);
            listViewAccessRules.Columns.Add("Type");
            listViewAccessRules.Columns.Add("Rights", 100);
            listViewAccessRules.FullRowSelect = true;
            listViewAccessRules.GridLines = true;
        }


        private void UpdateStatus(string text)
        {
            InvokeIfRequired(() => labelProgress.Text = $"Scanning {text}");
        }

        private async void buttonBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();

            textBoxPath.Text = folderBrowserDialog1.SelectedPath;

            cancellationTokenSource = new CancellationTokenSource();

            scanner = new Scanner(cancellationTokenSource.Token);

            //scanner.OnScanFolder = (folder) => InvokeIfRequired(() => labelProgress.Text = $"Scanning {folder}");
            scanner.OnScanFolder = UpdateStatus;

            labelProgress.Text = "Scanning...";

            treeViewFolders.Nodes.Clear();
            listViewFiles.Items.Clear();
            listViewAccessRules.Items.Clear();


            //Task.Run(() => scanner.ScanFolder(textBoxPath.Text))
            //    .ContinueWith(t =>
            //    {
            //        try
            //        {
            //            if (t.IsCompletedSuccessfully)
            //            {
            //                var folder = t.Result;

            //                InvokeIfRequired(() => labelProgress.Text = "Building Tree...");

            //                BuildTreeView(folder);

            //                InvokeIfRequired(() => labelProgress.Text = "Ready");
            //            }
            //            else
            //            {
            //                throw t.Exception;
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show(ex.Message);
            //        }
            //    });

            try
            {
                var folder = await Task.Run(() => scanner.ScanFolder(textBoxPath.Text));

                InvokeIfRequired(() => labelProgress.Text = "Building Tree...");

                BuildTreeView(folder);

                InvokeIfRequired(() => labelProgress.Text = "Ready");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void BuildTreeView(Folder folder)
        {
            var rootNode = CreateNode(folder);

            foreach (var subfolder in folder.Folders)
            {
                var subNode = AddSubNode(rootNode, subfolder);
                if (subNode != null)
                {
                    BuildChildTreeView(subNode, subfolder);
                }
            }

            InvokeIfRequired(() => rootNode.Expand());
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

        private void InvokeIfRequired(Action action)
        {
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }

        private TreeNode CreateNode(Folder folder)
        {
            var newNode = new TreeNode(folder.Name);
            newNode.Tag = folder;

            InvokeIfRequired(() => treeViewFolders.Nodes.Add(newNode));

            return newNode;
        }

        private TreeNode AddSubNode(TreeNode node, Folder folder)
        {
            var newNode = new TreeNode(folder.Name);
            newNode.Tag = folder;

            InvokeIfRequired(() => node.Nodes.Add(newNode));

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
            if (listView.SelectedItems.Count > 0)
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


        private void buttonStop_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            var folder = (Folder)treeViewFolders.Nodes[0].Tag;
            var writer = new DirectoryWriter("Server=.;Database=NTFSScan;Trusted_Connection=True;");
            writer.WriteDirectory(folder);
            MessageBox.Show("Saved directory to database", "NTFSScan", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

}