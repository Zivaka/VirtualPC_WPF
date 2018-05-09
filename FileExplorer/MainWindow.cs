using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualFileSystem;

namespace FileExplorer
{
    public partial class MainWindow : Form
    {
        private List<string> _adressHistory = new List<string>();
        private int currIndex = -1;
        
        string currListViewAdress = "";

        private readonly VirtualFileSystem.VirtualFileSystem _virtualFileSystem;

        public MainWindow()
        {
            InitializeComponent();

            _virtualFileSystem = VirtualFileSystemGenerator.GenerateFileSystemFromDirectory(@"E:\VS");
            var drives = InitializeFileSystemTree(_virtualFileSystem);
            TreeView.Nodes.AddRange(drives.ToArray());

            InitializeShellView();
        }

        private void SelectTreeViewNode(object sender, TreeViewEventArgs e)
        {
            ClearHistory();

            _adressHistory.Add(e.Node.Name);
            currIndex++;
            //проверка возможности перехода назад/вперёд
            if (currIndex + 1 == _adressHistory.Count)
                FolderForwardButton.Enabled = false;
            else
                FolderForwardButton.Enabled = true;
            if (currIndex - 1 == -1)
                FolderBackButton.Enabled = false;
            else
                FolderBackButton.Enabled = true;
            ShellView.Items.Clear();
            //currListViewAdress = e.Node.Name;
            //toolStripTextBox1.Text = currListViewAdress;
            //заполнение ListView
            try
            {
                if (ShellView.View != View.Tile)
                {
                    //FileInfo f = new FileInfo(@e.Node.Name);
                    string t = "";
                    string[] str2 = new VirtualDirectoryInfo(_virtualFileSystem, e.Node.Name).GetDirectories().Select(x => x.FullName).ToArray();//Directory.GetDirectories(@e.Node.Name);
                    ListViewItem lw = new ListViewItem();
                    foreach (string s2 in str2)
                    {
                        //f = new FileInfo(@s2);                        
                        string type = "Папка";
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t, "", type, "" }, 0);
                        lw.Name = s2;
                        ShellView.Items.Add(lw);
                    }

                    str2 = new VirtualDirectoryInfo(_virtualFileSystem, e.Node.Name).GetFiles().Select(x => x.FullName).ToArray();
                    foreach (string s2 in str2)
                    {
                        var fileData = _virtualFileSystem.GetFile(s2);
                        string type = "Файл";
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t, fileData.Contents.Length + " байт", type, "" }, 1);
                        lw.Name = s2;
                        ShellView.Items.Add(lw);
                    }
                }
                else
                {
                    //FileInfo f = new FileInfo(@e.Node.Name);
                    string t = "";
                    string[] str2 = new VirtualDirectoryInfo(_virtualFileSystem, e.Node.Name).GetDirectories().Select(x => x.FullName).ToArray();//Directory.GetDirectories(@e.Node.Name);
                    ListViewItem lw = new ListViewItem();
                    foreach (string s2 in str2)
                    {
                        //f = new FileInfo(@s2);
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t }, 0);
                        lw.Name = s2;
                        ShellView.Items.Add(lw);
                    }
                    str2 = Directory.GetFiles(@e.Node.Name);
                    foreach (string s2 in str2)
                    {
                        //f = new FileInfo(@s2);
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t }, 1);
                        lw.Name = s2;
                        ShellView.Items.Add(lw);
                    }
                }
            }
            catch { }
        }

        private void ClearHistory(int maxCount = 100)
        {
            if (currIndex == -1) _adressHistory.Clear();
            if (_adressHistory.Count > maxCount && currIndex != -1)
            {
                var halfLength = maxCount / 2;
                var historyBefore = _adressHistory.GetRange(Math.Max(currIndex - halfLength, 0), halfLength);
                var historyAfter = _adressHistory.GetRange(currIndex, Math.Min(halfLength, _adressHistory.Count - halfLength));

                _adressHistory.Clear();

                _adressHistory.AddRange(historyBefore);
                _adressHistory.AddRange(historyAfter);                
            }
        }

        private List<TreeNode> CreateFolderTree(VirtualDirectoryInfo directoryInfo)
        {
            var folderNodes = new List<TreeNode>();
            var folders = directoryInfo.GetDirectories();
            foreach (var folder in folders)
            {
                TreeNode folderNode = new TreeNode
                {
                    Name = folder.FullName,
                    Text = folder.Name,
                    SelectedImageIndex = 0,
                    ImageIndex = 0
                };

                var subfolders = CreateFolderTree((VirtualDirectoryInfo) folder);
                foreach (var subfolder in subfolders) folderNode.Nodes.Add(subfolder);

                folderNodes.Add(folderNode);
            }

            return folderNodes;
        }

        private List<TreeNode> InitializeFileSystemTree(VirtualFileSystem.VirtualFileSystem fileSystem)
        {
            var driveNodes = new List<TreeNode>();
            var drives = fileSystem.DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                TreeNode driveNode = new TreeNode
                {
                    Name = drive.Name,
                    Text = $@"Локальный диск {drive.Name}",
                    SelectedImageIndex = 2,
                    ImageIndex = 2
                };

                var subfolders = CreateFolderTree(new VirtualDirectoryInfo(fileSystem, drive.Name));
                foreach (var subfolder in subfolders) driveNode.Nodes.Add(subfolder);
                driveNodes.Add(driveNode);
            }
            return driveNodes;
        }

        private void InitializeShellView()
        {            
            ColumnHeader c = new ColumnHeader();
            c.Text = @"Назва";
            c.Width = c.Width + 80;
            ColumnHeader c2 = new ColumnHeader();
            c2.Text = @"Розмір";
            c2.Width = c2.Width + 60;
            ColumnHeader c3 = new ColumnHeader();
            c3.Text = @"Тип";
            c3.Width = c3.Width + 60;
            ShellView.Columns.Add(c);
            ShellView.Columns.Add(c2);
            ShellView.Columns.Add(c3);
            //ShellView.ColumnClick += new ColumnClickEventHandler(ClickOnColumn);
        }
    }
}
