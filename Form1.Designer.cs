using System.Windows.Forms;

namespace NTFSScan
{

    public partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.treeViewFolders = new System.Windows.Forms.TreeView();
            this.listViewFiles = new System.Windows.Forms.ListView();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.listViewAccessRules = new System.Windows.Forms.ListView();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.labelProgress = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(203, 29);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(29, 23);
            this.buttonBrowse.TabIndex = 1;
            this.buttonBrowse.Text = "...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // treeViewFolders
            // 
            this.treeViewFolders.Location = new System.Drawing.Point(12, 67);
            this.treeViewFolders.Name = "treeViewFolders";
            this.treeViewFolders.Size = new System.Drawing.Size(185, 351);
            this.treeViewFolders.TabIndex = 2;
            this.treeViewFolders.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFolders_AfterSelect);
            // 
            // listViewFiles
            // 
            this.listViewFiles.HideSelection = false;
            this.listViewFiles.Location = new System.Drawing.Point(203, 67);
            this.listViewFiles.Name = "listViewFiles";
            this.listViewFiles.Size = new System.Drawing.Size(353, 351);
            this.listViewFiles.TabIndex = 3;
            this.listViewFiles.UseCompatibleStateImageBehavior = false;
            this.listViewFiles.SelectedIndexChanged += new System.EventHandler(this.listViewFiles_SelectedIndexChanged);
            // 
            // textBoxPath
            // 
            this.textBoxPath.Location = new System.Drawing.Point(12, 29);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(185, 23);
            this.textBoxPath.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Select drive or folder:";
            // 
            // listViewAccessRules
            // 
            this.listViewAccessRules.HideSelection = false;
            this.listViewAccessRules.Location = new System.Drawing.Point(562, 67);
            this.listViewAccessRules.Name = "listViewAccessRules";
            this.listViewAccessRules.Size = new System.Drawing.Size(226, 351);
            this.listViewAccessRules.TabIndex = 6;
            this.listViewAccessRules.UseCompatibleStateImageBehavior = false;
            this.listViewAccessRules.SelectedIndexChanged += new System.EventHandler(this.listViewAccessRules_SelectedIndexChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 424);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(776, 23);
            this.progressBar1.TabIndex = 7;
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.BackColor = System.Drawing.Color.Transparent;
            this.labelProgress.Location = new System.Drawing.Point(12, 428);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(39, 15);
            this.labelProgress.TabIndex = 8;
            this.labelProgress.Text = "Ready";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 459);
            this.Controls.Add(this.labelProgress);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.listViewAccessRules);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(this.listViewFiles);
            this.Controls.Add(this.treeViewFolders);
            this.Controls.Add(this.buttonBrowse);
            this.Name = "Form1";
            this.Text = "NTFS Scan";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button buttonBrowse;
        private TreeView treeViewFolders;
        private ListView listViewFiles;
        private TextBox textBoxPath;
        private FolderBrowserDialog folderBrowserDialog1;
        private Label label1;
        private ListView listViewAccessRules;
        private ProgressBar progressBar1;
        private Label labelProgress;
    }

}
