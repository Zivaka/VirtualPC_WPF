namespace FileExplorer
{
    partial class MainWindow
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.ToolBar = new System.Windows.Forms.ToolStrip();
            this.FolderBackButton = new System.Windows.Forms.ToolStripButton();
            this.FolderForwardButton = new System.Windows.Forms.ToolStripButton();
            this.ToolBarSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.UpButton = new System.Windows.Forms.ToolStripButton();
            this.ToolBarSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.CurrentFolderPath = new System.Windows.Forms.ToolStripTextBox();
            this.ToolBarSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.RefreshButton = new System.Windows.Forms.ToolStripButton();
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.FileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.IconViewMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ImageViewMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.TileViewMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ListViewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TableViewMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSaparator = new System.Windows.Forms.ToolStripSeparator();
            this.RefreshMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Icons16 = new System.Windows.Forms.ImageList(this.components);
            this.Icons32 = new System.Windows.Forms.ImageList(this.components);
            this.MainPanel = new System.Windows.Forms.Panel();
            this.ShellContainer = new System.Windows.Forms.SplitContainer();
            this.TreeView = new System.Windows.Forms.TreeView();
            this.ShellView = new System.Windows.Forms.ListView();
            this.ShellContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.OpenContextMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.RefreshContextMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.CutContextMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyContextMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.PasteContexMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.RemoveContexMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.RenameContexMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.CreateContexMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.CreateFolderContexMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.CreateTextFileContexMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.PropertiesContexMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolBar.SuspendLayout();
            this.StatusBar.SuspendLayout();
            this.MainMenu.SuspendLayout();
            this.MainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ShellContainer)).BeginInit();
            this.ShellContainer.Panel1.SuspendLayout();
            this.ShellContainer.Panel2.SuspendLayout();
            this.ShellContainer.SuspendLayout();
            this.ShellContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolBar
            // 
            this.ToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FolderBackButton,
            this.FolderForwardButton,
            this.ToolBarSeparator1,
            this.UpButton,
            this.ToolBarSeparator2,
            this.CurrentFolderPath,
            this.ToolBarSeparator3,
            this.RefreshButton});
            this.ToolBar.Location = new System.Drawing.Point(0, 24);
            this.ToolBar.Name = "ToolBar";
            this.ToolBar.Size = new System.Drawing.Size(835, 25);
            this.ToolBar.TabIndex = 5;
            // 
            // FolderBackButton
            // 
            this.FolderBackButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.FolderBackButton.Enabled = false;
            this.FolderBackButton.Image = ((System.Drawing.Image)(resources.GetObject("FolderBackButton.Image")));
            this.FolderBackButton.ImageTransparentColor = System.Drawing.Color.White;
            this.FolderBackButton.Name = "FolderBackButton";
            this.FolderBackButton.Size = new System.Drawing.Size(23, 22);
            this.FolderBackButton.Text = "Назад";
            this.FolderBackButton.Click += new System.EventHandler(this.FolderBackButton_Click);
            // 
            // FolderForwardButton
            // 
            this.FolderForwardButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.FolderForwardButton.Enabled = false;
            this.FolderForwardButton.Image = ((System.Drawing.Image)(resources.GetObject("FolderForwardButton.Image")));
            this.FolderForwardButton.ImageTransparentColor = System.Drawing.Color.White;
            this.FolderForwardButton.Name = "FolderForwardButton";
            this.FolderForwardButton.Size = new System.Drawing.Size(23, 22);
            this.FolderForwardButton.Text = "Вперед";
            this.FolderForwardButton.Click += new System.EventHandler(this.FolderForwardButton_Click);
            // 
            // ToolBarSeparator1
            // 
            this.ToolBarSeparator1.Name = "ToolBarSeparator1";
            this.ToolBarSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // UpButton
            // 
            this.UpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.UpButton.Image = ((System.Drawing.Image)(resources.GetObject("UpButton.Image")));
            this.UpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.UpButton.Name = "UpButton";
            this.UpButton.Size = new System.Drawing.Size(23, 22);
            this.UpButton.Text = "Вгору";
            this.UpButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // ToolBarSeparator2
            // 
            this.ToolBarSeparator2.Name = "ToolBarSeparator2";
            this.ToolBarSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // CurrentFolderPath
            // 
            this.CurrentFolderPath.Name = "CurrentFolderPath";
            this.CurrentFolderPath.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.CurrentFolderPath.Size = new System.Drawing.Size(600, 25);
            // 
            // ToolBarSeparator3
            // 
            this.ToolBarSeparator3.Name = "ToolBarSeparator3";
            this.ToolBarSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // RefreshButton
            // 
            this.RefreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RefreshButton.Image = ((System.Drawing.Image)(resources.GetObject("RefreshButton.Image")));
            this.RefreshButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(23, 22);
            this.RefreshButton.Text = "Обновити";
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // StatusBar
            // 
            this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.StatusBar.Location = new System.Drawing.Point(0, 513);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(835, 22);
            this.StatusBar.TabIndex = 3;
            this.StatusBar.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuItem,
            this.ViewMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(835, 24);
            this.MainMenu.TabIndex = 4;
            this.MainMenu.Text = "menuStrip1";
            // 
            // FileMenuItem
            // 
            this.FileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExitMenuItem});
            this.FileMenuItem.Name = "FileMenuItem";
            this.FileMenuItem.Size = new System.Drawing.Size(48, 20);
            this.FileMenuItem.Text = "Файл";
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.Size = new System.Drawing.Size(120, 22);
            this.ExitMenuItem.Text = "Закрыть";
            this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // ViewMenuItem
            // 
            this.ViewMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.IconViewMenuButton,
            this.ImageViewMenuButton,
            this.TileViewMenuButton,
            this.ListViewMenuItem,
            this.TableViewMenuButton,
            this.MenuSaparator,
            this.RefreshMenuButton});
            this.ViewMenuItem.Name = "ViewMenuItem";
            this.ViewMenuItem.Size = new System.Drawing.Size(39, 20);
            this.ViewMenuItem.Text = "Вид";
            // 
            // IconViewMenuButton
            // 
            this.IconViewMenuButton.Name = "IconViewMenuButton";
            this.IconViewMenuButton.Size = new System.Drawing.Size(193, 22);
            this.IconViewMenuButton.Text = "Список иконок";
            this.IconViewMenuButton.Click += new System.EventHandler(this.IconViewMenuButton_Click);
            // 
            // ImageViewMenuButton
            // 
            this.ImageViewMenuButton.Name = "ImageViewMenuButton";
            this.ImageViewMenuButton.Size = new System.Drawing.Size(193, 22);
            this.ImageViewMenuButton.Text = "Список изображений";
            this.ImageViewMenuButton.Click += new System.EventHandler(this.ImageViewMenuButton_Click);
            // 
            // TileViewMenuButton
            // 
            this.TileViewMenuButton.Name = "TileViewMenuButton";
            this.TileViewMenuButton.Size = new System.Drawing.Size(193, 22);
            this.TileViewMenuButton.Text = "Плитки";
            this.TileViewMenuButton.Click += new System.EventHandler(this.TileViewMenuButton_Click);
            // 
            // ListViewMenuItem
            // 
            this.ListViewMenuItem.Name = "ListViewMenuItem";
            this.ListViewMenuItem.Size = new System.Drawing.Size(193, 22);
            this.ListViewMenuItem.Text = "Список";
            this.ListViewMenuItem.Click += new System.EventHandler(this.ListViewMenuItem_Click);
            // 
            // TableViewMenuButton
            // 
            this.TableViewMenuButton.Name = "TableViewMenuButton";
            this.TableViewMenuButton.Size = new System.Drawing.Size(193, 22);
            this.TableViewMenuButton.Text = "Таблица";
            this.TableViewMenuButton.Click += new System.EventHandler(this.TableViewMenuButton_Click);
            // 
            // MenuSaparator
            // 
            this.MenuSaparator.Name = "MenuSaparator";
            this.MenuSaparator.Size = new System.Drawing.Size(190, 6);
            // 
            // RefreshMenuButton
            // 
            this.RefreshMenuButton.Name = "RefreshMenuButton";
            this.RefreshMenuButton.Size = new System.Drawing.Size(193, 22);
            this.RefreshMenuButton.Text = "Обновить";
            this.RefreshMenuButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // Icons16
            // 
            this.Icons16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Icons16.ImageStream")));
            this.Icons16.TransparentColor = System.Drawing.Color.Transparent;
            this.Icons16.Images.SetKeyName(0, "directory_mini.bmp");
            this.Icons16.Images.SetKeyName(1, "file_mini.bmp");
            this.Icons16.Images.SetKeyName(2, "localdriver.png");
            // 
            // Icons32
            // 
            this.Icons32.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Icons32.ImageStream")));
            this.Icons32.TransparentColor = System.Drawing.Color.Transparent;
            this.Icons32.Images.SetKeyName(0, "directory.bmp");
            this.Icons32.Images.SetKeyName(1, "file.bmp");
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.ShellContainer);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 49);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(835, 464);
            this.MainPanel.TabIndex = 10;
            // 
            // ShellContainer
            // 
            this.ShellContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ShellContainer.Location = new System.Drawing.Point(0, 0);
            this.ShellContainer.Name = "ShellContainer";
            // 
            // ShellContainer.Panel1
            // 
            this.ShellContainer.Panel1.Controls.Add(this.TreeView);
            // 
            // ShellContainer.Panel2
            // 
            this.ShellContainer.Panel2.Controls.Add(this.ShellView);
            this.ShellContainer.Size = new System.Drawing.Size(835, 464);
            this.ShellContainer.SplitterDistance = 193;
            this.ShellContainer.TabIndex = 0;
            // 
            // TreeView
            // 
            this.TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeView.ImageIndex = 0;
            this.TreeView.ImageList = this.Icons16;
            this.TreeView.Location = new System.Drawing.Point(0, 0);
            this.TreeView.Name = "TreeView";
            this.TreeView.SelectedImageIndex = 0;
            this.TreeView.Size = new System.Drawing.Size(193, 464);
            this.TreeView.TabIndex = 0;
            this.TreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.SelectTreeViewNode);
            // 
            // ShellView
            // 
            this.ShellView.ContextMenuStrip = this.ShellContextMenu;
            this.ShellView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ShellView.FullRowSelect = true;
            this.ShellView.LabelEdit = true;
            this.ShellView.LargeImageList = this.Icons32;
            this.ShellView.Location = new System.Drawing.Point(0, 0);
            this.ShellView.Name = "ShellView";
            this.ShellView.Size = new System.Drawing.Size(638, 464);
            this.ShellView.SmallImageList = this.Icons16;
            this.ShellView.TabIndex = 0;
            this.ShellView.UseCompatibleStateImageBehavior = false;
            this.ShellView.View = System.Windows.Forms.View.Tile;
            this.ShellView.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.ShellView_AfterLabelEdit);
            this.ShellView.DoubleClick += new System.EventHandler(this.ShellView_DoubleClick);
            this.ShellView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ShellView_MouseMove);
            // 
            // ShellContextMenu
            // 
            this.ShellContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenContextMenuButton,
            this.RefreshContextMenuButton,
            this.ContextMenuSeparator1,
            this.CutContextMenuButton,
            this.CopyContextMenuButton,
            this.PasteContexMenuButton,
            this.ContextMenuSeparator2,
            this.RemoveContexMenuButton,
            this.RenameContexMenuButton,
            this.CreateContexMenuButton,
            this.ContextMenuSeparator3,
            this.PropertiesContexMenuButton});
            this.ShellContextMenu.Name = "ContextMenu";
            this.ShellContextMenu.Size = new System.Drawing.Size(162, 220);
            this.ShellContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenu_Opening);
            // 
            // OpenContextMenuButton
            // 
            this.OpenContextMenuButton.Name = "OpenContextMenuButton";
            this.OpenContextMenuButton.Size = new System.Drawing.Size(161, 22);
            this.OpenContextMenuButton.Text = "Відкрити";
            this.OpenContextMenuButton.Click += new System.EventHandler(this.ShellView_DoubleClick);
            // 
            // RefreshContextMenuButton
            // 
            this.RefreshContextMenuButton.Name = "RefreshContextMenuButton";
            this.RefreshContextMenuButton.Size = new System.Drawing.Size(161, 22);
            this.RefreshContextMenuButton.Text = "Обновити";
            // 
            // ContextMenuSeparator1
            // 
            this.ContextMenuSeparator1.Name = "ContextMenuSeparator1";
            this.ContextMenuSeparator1.Size = new System.Drawing.Size(158, 6);
            // 
            // CutContextMenuButton
            // 
            this.CutContextMenuButton.Name = "CutContextMenuButton";
            this.CutContextMenuButton.Size = new System.Drawing.Size(161, 22);
            this.CutContextMenuButton.Text = "Вирізати";
            this.CutContextMenuButton.Click += new System.EventHandler(this.CutContextMenuButton_Click);
            // 
            // CopyContextMenuButton
            // 
            this.CopyContextMenuButton.Name = "CopyContextMenuButton";
            this.CopyContextMenuButton.Size = new System.Drawing.Size(161, 22);
            this.CopyContextMenuButton.Text = "Копіювати";
            this.CopyContextMenuButton.Click += new System.EventHandler(this.CopyContextMenuButton_Click);
            // 
            // PasteContexMenuButton
            // 
            this.PasteContexMenuButton.Name = "PasteContexMenuButton";
            this.PasteContexMenuButton.Size = new System.Drawing.Size(161, 22);
            this.PasteContexMenuButton.Text = "Вставити";
            this.PasteContexMenuButton.Click += new System.EventHandler(this.PasteContexMenuButton_Click);
            // 
            // ContextMenuSeparator2
            // 
            this.ContextMenuSeparator2.Name = "ContextMenuSeparator2";
            this.ContextMenuSeparator2.Size = new System.Drawing.Size(158, 6);
            // 
            // RemoveContexMenuButton
            // 
            this.RemoveContexMenuButton.Name = "RemoveContexMenuButton";
            this.RemoveContexMenuButton.Size = new System.Drawing.Size(161, 22);
            this.RemoveContexMenuButton.Text = "Видалити";
            this.RemoveContexMenuButton.Click += new System.EventHandler(this.RemoveContexMenuButton_Click);
            // 
            // RenameContexMenuButton
            // 
            this.RenameContexMenuButton.Name = "RenameContexMenuButton";
            this.RenameContexMenuButton.Size = new System.Drawing.Size(161, 22);
            this.RenameContexMenuButton.Text = "Перейменувати";
            this.RenameContexMenuButton.Click += new System.EventHandler(this.RenameContexMenuButton_Click);
            // 
            // CreateContexMenuButton
            // 
            this.CreateContexMenuButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateFolderContexMenuButton,
            this.CreateTextFileContexMenuButton});
            this.CreateContexMenuButton.Name = "CreateContexMenuButton";
            this.CreateContexMenuButton.Size = new System.Drawing.Size(161, 22);
            this.CreateContexMenuButton.Text = "Створити";
            // 
            // CreateFolderContexMenuButton
            // 
            this.CreateFolderContexMenuButton.Name = "CreateFolderContexMenuButton";
            this.CreateFolderContexMenuButton.Size = new System.Drawing.Size(186, 22);
            this.CreateFolderContexMenuButton.Text = "Папку";
            this.CreateFolderContexMenuButton.Click += new System.EventHandler(this.CreateFolderContexMenuButton_Click);
            // 
            // CreateTextFileContexMenuButton
            // 
            this.CreateTextFileContexMenuButton.Name = "CreateTextFileContexMenuButton";
            this.CreateTextFileContexMenuButton.Size = new System.Drawing.Size(186, 22);
            this.CreateTextFileContexMenuButton.Text = "Текстовий документ";
            // 
            // ContextMenuSeparator3
            // 
            this.ContextMenuSeparator3.Name = "ContextMenuSeparator3";
            this.ContextMenuSeparator3.Size = new System.Drawing.Size(158, 6);
            // 
            // PropertiesContexMenuButton
            // 
            this.PropertiesContexMenuButton.Name = "PropertiesContexMenuButton";
            this.PropertiesContexMenuButton.Size = new System.Drawing.Size(161, 22);
            this.PropertiesContexMenuButton.Text = "Властивості";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(835, 535);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.ToolBar);
            this.Controls.Add(this.StatusBar);
            this.Controls.Add(this.MainMenu);
            this.DoubleBuffered = true;
            this.Name = "MainWindow";
            this.ShowIcon = false;
            this.Text = "Файловий провідник";
            this.ToolBar.ResumeLayout(false);
            this.ToolBar.PerformLayout();
            this.StatusBar.ResumeLayout(false);
            this.StatusBar.PerformLayout();
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.MainPanel.ResumeLayout(false);
            this.ShellContainer.Panel1.ResumeLayout(false);
            this.ShellContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ShellContainer)).EndInit();
            this.ShellContainer.ResumeLayout(false);
            this.ShellContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip ToolBar;
        private System.Windows.Forms.ToolStripButton FolderBackButton;
        private System.Windows.Forms.ToolStripButton FolderForwardButton;
        private System.Windows.Forms.ToolStripTextBox CurrentFolderPath;
        private System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem FileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ViewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem IconViewMenuButton;
        private System.Windows.Forms.ToolStripMenuItem ImageViewMenuButton;
        private System.Windows.Forms.ToolStripMenuItem TileViewMenuButton;
        private System.Windows.Forms.ToolStripMenuItem ListViewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TableViewMenuButton;
        private System.Windows.Forms.ToolStripSeparator MenuSaparator;
        private System.Windows.Forms.ToolStripMenuItem RefreshMenuButton;
        private System.Windows.Forms.ImageList Icons16;
        private System.Windows.Forms.ImageList Icons32;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.SplitContainer ShellContainer;
        private System.Windows.Forms.TreeView TreeView;
        private System.Windows.Forms.ListView ShellView;
        private System.Windows.Forms.ToolStripSeparator ToolBarSeparator1;
        private System.Windows.Forms.ToolStripButton UpButton;
        private System.Windows.Forms.ToolStripSeparator ToolBarSeparator2;
        private System.Windows.Forms.ToolStripSeparator ToolBarSeparator3;
        private System.Windows.Forms.ToolStripButton RefreshButton;
        private System.Windows.Forms.ContextMenuStrip ShellContextMenu;
        private System.Windows.Forms.ToolStripMenuItem OpenContextMenuButton;
        private System.Windows.Forms.ToolStripSeparator ContextMenuSeparator1;
        private System.Windows.Forms.ToolStripMenuItem CutContextMenuButton;
        private System.Windows.Forms.ToolStripMenuItem CopyContextMenuButton;
        private System.Windows.Forms.ToolStripMenuItem PasteContexMenuButton;
        private System.Windows.Forms.ToolStripSeparator ContextMenuSeparator2;
        private System.Windows.Forms.ToolStripMenuItem RemoveContexMenuButton;
        private System.Windows.Forms.ToolStripMenuItem RenameContexMenuButton;
        private System.Windows.Forms.ToolStripSeparator ContextMenuSeparator3;
        private System.Windows.Forms.ToolStripMenuItem CreateContexMenuButton;
        private System.Windows.Forms.ToolStripMenuItem CreateFolderContexMenuButton;
        private System.Windows.Forms.ToolStripMenuItem CreateTextFileContexMenuButton;
        private System.Windows.Forms.ToolStripMenuItem PropertiesContexMenuButton;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem RefreshContextMenuButton;
    }
}

