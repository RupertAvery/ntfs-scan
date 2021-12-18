using System;
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
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBoxPath.Text = folderBrowserDialog1.SelectedPath;
            scanner = new Scanner();

            Task.Run(() => scanner.Scan(textBoxPath.Text))
                .ContinueWith(t =>
                {
                    try
                    {
                        if (t.IsCompletedSuccessfully)
                        {
                            var folder = t.Result;
                            BuildTreeView(folder);
                        }
                    }catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
        }

        private void BuildTreeView(Folder folder)
        {
            treeViewFolders.Nodes.Clear();
            
            foreach(var subfolder in folder.Folders)
            {
                var node = CreateNode(subfolder.Name);
                if(node != null)
                {
                    BuildChildTreeView(node, subfolder);
                }
            }
        }

        private void BuildChildTreeView(TreeNode node, Folder folder)
        {
            foreach (var subfolder in folder.Folders)
            {
                var subNode = AddNode(node, subfolder.Name);
                if(subNode != null)
                {
                    BuildChildTreeView(subNode, subfolder);
                }
            }
        }

        private TreeNode CreateNode(string name)
        {
            if (this.InvokeRequired)
            {
                return (TreeNode)this.Invoke(new Func<TreeNode>(() => treeViewFolders.Nodes.Add(name)));
            }
            else
            {
                return treeViewFolders.Nodes.Add(name);
            }
        }

        private TreeNode AddNode(TreeNode node, string name)
        {
            if(this.InvokeRequired)
            {
                return (TreeNode)this.Invoke(new Action(() => node.Nodes.Add(name)));
            }
            else
            {
                return node.Nodes.Add(name);
            }
        } 

    }

}