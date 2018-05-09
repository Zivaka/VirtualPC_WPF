using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Abstractions;
using System.Linq;
using System.Windows.Forms;
using VirtualFileSystem;

namespace FileExplorer
{

    public partial class MainWindow : Form
    {
        private bool _isShellItemHover;
        private ListViewItem _shellItemHover;

        private string _folderCopiedFrom = "";
        private List<string> _fileToCopyList = new List<string>();
        private bool? _copyMode;       

        private List<FileSystemInfoBase> _currentFolderInfos;
        private List<VirtualDirectoryInfo> _folderHistory = new List<VirtualDirectoryInfo>();
        private int _currentFolderIndex = -1;

        private readonly VirtualFileSystem.VirtualFileSystem _virtualFileSystem;

        public MainWindow()
        {
            InitializeComponent();

            _virtualFileSystem = VirtualFileSystemGenerator.GenerateFileSystemFromDirectory(@"E:\VS");

            TreeView.Nodes.Clear();
            var drives = InitializeFileSystemTree(_virtualFileSystem);
            TreeView.Nodes.AddRange(drives.ToArray());

            InitializeShellView();
        }

        private void SelectTreeViewNode(object sender, TreeViewEventArgs e)
        {
            UpdatePath(e.Node.Name);            
        }

        private void ShellView_DoubleClick(object sender, EventArgs e)
        {
            if (ShellView.SelectedItems.Count == 0 && _shellItemHover == null) return;

            var currentItem = _shellItemHover ?? ShellView.SelectedItems[0];
            var item = _currentFolderInfos.Find(info => info.Name == currentItem.Text);
            if (item is VirtualDirectoryInfo)
                UpdatePath(item.FullName);
            else
            {
                //Open all sellected files
                MessageBox.Show(@"Sending message to open files...");
            }
        }

        private void CurrentPathChanged()
        {
            var currentFolder = _folderHistory[_currentFolderIndex];
            CurrentFolderPath.Text = currentFolder.FullName;

            FolderForwardButton.Enabled = _currentFolderIndex + 1 != _folderHistory.Count;
            FolderBackButton.Enabled = _currentFolderIndex - 1 != -1;
            UpButton.Enabled = currentFolder.Parent != null;

            UpdateShellView(currentFolder);
        }

        private void UpdatePath(string folderPath, bool fromHistory = false)
        {
            if (!fromHistory)
            {
                if (_currentFolderIndex != _folderHistory.Count - 1)
                    _folderHistory.RemoveRange(_currentFolderIndex + 1, _folderHistory.Count - _currentFolderIndex - 1);

                _folderHistory.Add(new VirtualDirectoryInfo(_virtualFileSystem, folderPath));
                _currentFolderIndex = _folderHistory.Count - 1;
                ClearHistory();
            }                     
            CurrentPathChanged();
        }

        private void ClearHistory(int maxCount = 20)
        {
            if (_currentFolderIndex == -1) _folderHistory.Clear();
            if (_folderHistory.Count > maxCount && _currentFolderIndex != -1)
            {
                _currentFolderIndex -= _folderHistory.Count - maxCount;
                _folderHistory.RemoveRange(0, _folderHistory.Count - maxCount);                
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

        private List<TreeNode> InitializeFileSystemTree(IVirtualFileDataAccessor fileSystem)
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
            var c1 = new ColumnHeader {Text = @"Название"};//
            c1.Width = c1.Width + 80;
            var c2 = new ColumnHeader { Text = @"Тип"};
            c2.Width = c2.Width + 60;
            var c3 = new ColumnHeader { Text = @"Розмір"};
            c3.Width = c3.Width + 60;

            ShellView.Columns.Add(c1);
            ShellView.Columns.Add(c2);
            ShellView.Columns.Add(c3);

            ShellView.ColumnClick += ClickOnColumn;          
        }

        private void ClickOnColumn(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == 0)
                ShellView.Sorting = ShellView.Sorting == SortOrder.Descending ? SortOrder.Ascending : SortOrder.Descending;            
        }

        private void UpdateShellView(VirtualDirectoryInfo currentFolder)
        {
            _currentFolderInfos = currentFolder.GetFileSystemInfos().ToList();
            ShellView.Items.Clear();
            foreach (var info in _currentFolderInfos)
            {
                int type = 0;
                long size = -1;
                if (info is VirtualFileInfo fileInfo)
                {
                    type = 1;
                    size = fileInfo.Length;
                }
                var itemData = new[] {info.Name, type == 0 ? "Файл" : "Тека", $"{(size != -1 ? $"{size} байт" : "")}"};
                if (ShellView.View == View.Tile) itemData = new[] {info.Name};
                var item = new ListViewItem(itemData, type);
                ShellView.Items.Add(item);
            }
        }

        private void ContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if(!_isShellItemHover) ShellView.SelectedItems.Clear();
            var isAnythingSelected = _isShellItemHover;

            OpenContextMenuButton.Visible = isAnythingSelected;
            CopyContextMenuButton.Visible = isAnythingSelected;
            CutContextMenuButton.Visible = isAnythingSelected;
            RemoveContexMenuButton.Visible = isAnythingSelected;
            RenameContexMenuButton.Visible = isAnythingSelected;
      
            RefreshContextMenuButton.Visible = !isAnythingSelected;
            PasteContexMenuButton.Visible = !isAnythingSelected;
            CreateContexMenuButton.Visible = !isAnythingSelected;

            PasteContexMenuButton.Enabled = _copyMode != null;
        }

        private void AddToCutCopy(string path)
        {
            _fileToCopyList.Add(path);
            if (IsFolder(path))
            {
                foreach (var info in new VirtualDirectoryInfo(_virtualFileSystem, path).GetFileSystemInfos())
                {
                    AddToCutCopy(info.FullName);
                }             
            }
        }

        private void InitializeCutCopy(bool isCopy)
        {
            _fileToCopyList.Clear();
            _folderCopiedFrom = CurrentFolderPath.Text;
            foreach (ListViewItem item in ShellView.SelectedItems)
                AddToCutCopy(JoinPath(CurrentFolderPath.Text,item.Text));
            _copyMode = isCopy;
        }

        private void CutContextMenuButton_Click(object sender, EventArgs e)
        {
            InitializeCutCopy(false);
        }

        private void CopyContextMenuButton_Click(object sender, EventArgs e)
        {
            InitializeCutCopy(true);
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            var parrentFolder =  _folderHistory[_currentFolderIndex].Parent;
            if (parrentFolder != null) UpdatePath(parrentFolder.FullName);
        }

        private void FolderForwardButton_Click(object sender, EventArgs e)
        {
            if (_currentFolderIndex + 1 != _folderHistory.Count)
                UpdatePath(_folderHistory[++_currentFolderIndex].FullName, true);
        }

        private void FolderBackButton_Click(object sender, EventArgs e)
        {
            if (_currentFolderIndex > 0)
                UpdatePath(_folderHistory[--_currentFolderIndex].FullName, true);         
        }

        private void IconViewMenuButton_Click(object sender, EventArgs e)
        {
            ShellView.View = View.SmallIcon;
        }

        private void ImageViewMenuButton_Click(object sender, EventArgs e)
        {
            ShellView.View = View.LargeIcon;
        }

        private void TileViewMenuButton_Click(object sender, EventArgs e)
        {
            ShellView.View = View.Tile;
            ShellView.FullRowSelect = false;
            UpdateShellView(_folderHistory[_currentFolderIndex]);
        }

        private void ListViewMenuItem_Click(object sender, EventArgs e)
        {
            ShellView.View = View.List;
        }

        private void TableViewMenuButton_Click(object sender, EventArgs e)
        {
            ShellView.View = View.Details;
            ShellView.FullRowSelect = true;
            UpdateShellView(_folderHistory[_currentFolderIndex]);
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            CurrentPathChanged();
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void RenameContexMenuButton_Click(object sender, EventArgs e)
        {
            if(_shellItemHover != null)
                _shellItemHover.BeginEdit();
            else if (ShellView.SelectedItems.Count > 0)
                ShellView.SelectedItems[0].BeginEdit();        
        }

        private void ShellView_MouseMove(object sender, MouseEventArgs e)
        {
            _shellItemHover = ShellView.GetItemAt(e.Location.X, e.Location.Y);
            _isShellItemHover = _shellItemHover != null;
        }

        private string JoinPath(string path1, string path2)
        {
            if (path1.EndsWith("\\")) return $"{path1}{path2}";
            return $"{path1}\\{path2}";
        }

        private bool IsFolder(string path)
        {
            if (_virtualFileSystem.AllDirectories.Contains(FixPath(path))) return true;
            return false;
        }

        private bool IsFile(string path)
        {
            if (_virtualFileSystem.AllFiles.Contains(path)) return true;
            return false;
        }

        private string FixPath(string path)
        {
            if (IsFile(path) || path.EndsWith("\\")) return path;
            return path + "\\";
        }

        private string GetName(string path)
        {
            return path.Split('\\').LastOrDefault(x => !string.IsNullOrWhiteSpace(x));
        }

        private void RemoveContexMenuButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"Ви впевнені, що бажаєте видалити безповоротно файли?", @"Видалити",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (ListViewItem item in ShellView.SelectedItems)
                {
                    var pathToRem = JoinPath(CurrentFolderPath.Text, item.Text);
                    if(IsFolder(pathToRem))
                        new VirtualFileInfo(_virtualFileSystem, pathToRem).Delete();
                    else
                    {
                        new VirtualDirectoryInfo(_virtualFileSystem, pathToRem).Delete(true);
                        RemoveFolderFromTreeView(pathToRem);
                    }                      
                }
                RefreshButton_Click(null, null);
            }            
        }

        private void AddFolderToTreeView(VirtualDirectoryInfo folder)
        {
            var findResults = TreeView.Nodes.Find(folder.Parent.FullName, true);
            if (findResults.Length == 0) return;
            var nodeWhereAdd = findResults[0];
            TreeNode folderNode = new TreeNode
            {
                Name = folder.FullName,
                Text = folder.Name,
                SelectedImageIndex = 0,
                ImageIndex = 0
            };
            nodeWhereAdd.Nodes.Add(folderNode);
        }

        private void RenameSubFoldersInTreeView(TreeNode root, string newPath)
        {
            root.Text = GetName(newPath);
            root.Name = newPath;
            foreach (TreeNode node in root.Nodes)           
                RenameSubFoldersInTreeView(node, JoinPath(newPath, node.Text));        
        }

        private void RenameFolderInTreeView(string path, string newPath)
        {
            var findResults = TreeView.Nodes.Find(path, true);
            if (findResults.Length == 0) return;
            var nodeToRename = findResults[0];
            RenameSubFoldersInTreeView(nodeToRename, newPath);
        }

        private void RemoveFolderFromTreeView(string path)
        {
            var findResults = TreeView.Nodes.Find(path, true);
            if (findResults.Length == 0) return;
            var nodeToRemove = findResults[0];
            nodeToRemove.Parent?.Nodes.Remove(nodeToRemove);
        }

        private void CreateFolderContexMenuButton_Click(object sender, EventArgs e)
        {
            var newFolderPath = JoinPath(CurrentFolderPath.Text, "New folder");
            _virtualFileSystem.AddDirectory(newFolderPath);
            AddFolderToTreeView(new VirtualDirectoryInfo(_virtualFileSystem, newFolderPath));
            RefreshButton_Click(null, null);
        }

        private void PasteContexMenuButton_Click(object sender, EventArgs e)
        {
            foreach (var file in _fileToCopyList)
            {
                var newFileName = FixPath(file).Replace(FixPath(_folderCopiedFrom), FixPath(CurrentFolderPath.Text));

                if (_virtualFileSystem.FileExists(newFileName)) continue;

                if (IsFolder(file))
                {
                    if (_copyMode == true)
                    {
                        _virtualFileSystem.AddDirectory(newFileName);
                    }
                    if (_copyMode == false)
                    {                     
                        new VirtualDirectoryInfo(_virtualFileSystem, file).MoveTo(newFileName);
                        RemoveFolderFromTreeView(file);
                    }
                    AddFolderToTreeView(new VirtualDirectoryInfo(_virtualFileSystem, newFileName));
                }
                else
                {
                    if (_copyMode == true)
                    {
                        _virtualFileSystem.AddFile(newFileName, _virtualFileSystem.GetFile(file));
                    }
                    if (_copyMode == false)
                    {
                        new VirtualFileInfo(_virtualFileSystem, file).MoveTo(newFileName);                 
                    }            
                }
            }
            _fileToCopyList.Clear();
            _copyMode = null;
            RefreshButton_Click(null, null);
        }

        private void ShellView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            var currentItem = ShellView.Items[e.Item];
            var newPath = JoinPath(CurrentFolderPath.Text, e.Label);
            var currentInfo = _currentFolderInfos.FirstOrDefault(info => info.Name == currentItem.Text) as VirtualDirectoryInfo;
            RenameFolderInTreeView(currentInfo?.FullName, newPath);
            currentInfo?.MoveTo(newPath);
        }
    }
}