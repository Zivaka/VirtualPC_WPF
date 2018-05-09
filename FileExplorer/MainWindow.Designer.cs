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
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.UpButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.CurrentPathFolder = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.RefreshButton = new System.Windows.Forms.ToolStripButton();
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.FileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.закрытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.IconViewMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ImageViewMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.TileViewMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ListViewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TableViewMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.RefreshMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Icons16 = new System.Windows.Forms.ImageList(this.components);
            this.Icons32 = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.ShellContainer = new System.Windows.Forms.SplitContainer();
            this.TreeView = new System.Windows.Forms.TreeView();
            this.ShellView = new System.Windows.Forms.ListView();
            this.ToolBar.SuspendLayout();
            this.MainMenu.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ShellContainer)).BeginInit();
            this.ShellContainer.Panel1.SuspendLayout();
            this.ShellContainer.Panel2.SuspendLayout();
            this.ShellContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolBar
            // 
            this.ToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FolderBackButton,
            this.FolderForwardButton,
            this.toolStripSeparator2,
            this.UpButton,
            this.toolStripSeparator3,
            this.CurrentPathFolder,
            this.toolStripSeparator4,
            this.RefreshButton});
            this.ToolBar.Location = new System.Drawing.Point(0, 24);
            this.ToolBar.Name = "ToolBar";
            this.ToolBar.Size = new System.Drawing.Size(724, 25);
            this.ToolBar.TabIndex = 5;
            this.ToolBar.Text = "toolStrip1";
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
            // 
            // FolderForwardButton
            // 
            this.FolderForwardButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.FolderForwardButton.Enabled = false;
            this.FolderForwardButton.Image = ((System.Drawing.Image)(resources.GetObject("FolderForwardButton.Image")));
            this.FolderForwardButton.ImageTransparentColor = System.Drawing.Color.White;
            this.FolderForwardButton.Name = "FolderForwardButton";
            this.FolderForwardButton.Size = new System.Drawing.Size(23, 22);
            this.FolderForwardButton.Text = "Вперёд";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // UpButton
            // 
            this.UpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.UpButton.Image = ((System.Drawing.Image)(resources.GetObject("UpButton.Image")));
            this.UpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.UpButton.Name = "UpButton";
            this.UpButton.Size = new System.Drawing.Size(23, 22);
            this.UpButton.Text = "toolStripButton1";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // CurrentPathFolder
            // 
            this.CurrentPathFolder.Name = "CurrentPathFolder";
            this.CurrentPathFolder.Size = new System.Drawing.Size(600, 25);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // RefreshButton
            // 
            this.RefreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RefreshButton.Image = ((System.Drawing.Image)(resources.GetObject("RefreshButton.Image")));
            this.RefreshButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(23, 22);
            this.RefreshButton.Text = "toolStripButton1";
            // 
            // StatusBar
            // 
            this.StatusBar.Location = new System.Drawing.Point(0, 513);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(724, 22);
            this.StatusBar.TabIndex = 3;
            this.StatusBar.Text = "statusStrip1";
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuItem,
            this.ViewMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(724, 24);
            this.MainMenu.TabIndex = 4;
            this.MainMenu.Text = "menuStrip1";
            // 
            // FileMenuItem
            // 
            this.FileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.закрытьToolStripMenuItem});
            this.FileMenuItem.Name = "FileMenuItem";
            this.FileMenuItem.Size = new System.Drawing.Size(48, 20);
            this.FileMenuItem.Text = "Файл";
            // 
            // закрытьToolStripMenuItem
            // 
            this.закрытьToolStripMenuItem.Name = "закрытьToolStripMenuItem";
            this.закрытьToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.закрытьToolStripMenuItem.Text = "Закрыть";
            // 
            // ViewMenuItem
            // 
            this.ViewMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.IconViewMenuButton,
            this.ImageViewMenuButton,
            this.TileViewMenuButton,
            this.ListViewMenuItem,
            this.TableViewMenuButton,
            this.toolStripSeparator1,
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
            // 
            // ImageViewMenuButton
            // 
            this.ImageViewMenuButton.Name = "ImageViewMenuButton";
            this.ImageViewMenuButton.Size = new System.Drawing.Size(193, 22);
            this.ImageViewMenuButton.Text = "Список изображений";
            // 
            // TileViewMenuButton
            // 
            this.TileViewMenuButton.Name = "TileViewMenuButton";
            this.TileViewMenuButton.Size = new System.Drawing.Size(193, 22);
            this.TileViewMenuButton.Text = "Плитки";
            // 
            // ListViewMenuItem
            // 
            this.ListViewMenuItem.Name = "ListViewMenuItem";
            this.ListViewMenuItem.Size = new System.Drawing.Size(193, 22);
            this.ListViewMenuItem.Text = "Список";
            // 
            // TableViewMenuButton
            // 
            this.TableViewMenuButton.Name = "TableViewMenuButton";
            this.TableViewMenuButton.Size = new System.Drawing.Size(193, 22);
            this.TableViewMenuButton.Text = "Таблица";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(190, 6);
            // 
            // RefreshMenuButton
            // 
            this.RefreshMenuButton.Name = "RefreshMenuButton";
            this.RefreshMenuButton.Size = new System.Drawing.Size(193, 22);
            this.RefreshMenuButton.Text = "Обновить";
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
            // panel1
            // 
            this.panel1.Controls.Add(this.ShellContainer);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(724, 464);
            this.panel1.TabIndex = 10;
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
            this.ShellContainer.Size = new System.Drawing.Size(724, 464);
            this.ShellContainer.SplitterDistance = 241;
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
            this.TreeView.Size = new System.Drawing.Size(241, 464);
            this.TreeView.TabIndex = 0;
            this.TreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.SelectTreeViewNode);
            // 
            // ShellView
            // 
            this.ShellView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ShellView.LargeImageList = this.Icons32;
            this.ShellView.Location = new System.Drawing.Point(0, 0);
            this.ShellView.Name = "ShellView";
            this.ShellView.Size = new System.Drawing.Size(479, 464);
            this.ShellView.TabIndex = 0;
            this.ShellView.UseCompatibleStateImageBehavior = false;
            this.ShellView.View = System.Windows.Forms.View.Details;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 535);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ToolBar);
            this.Controls.Add(this.StatusBar);
            this.Controls.Add(this.MainMenu);
            this.DoubleBuffered = true;
            this.Name = "MainWindow";
            this.ShowIcon = false;
            this.Text = "Файловий провідник";
            this.ToolBar.ResumeLayout(false);
            this.ToolBar.PerformLayout();
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ShellContainer.Panel1.ResumeLayout(false);
            this.ShellContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ShellContainer)).EndInit();
            this.ShellContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip ToolBar;
        private System.Windows.Forms.ToolStripButton FolderBackButton;
        private System.Windows.Forms.ToolStripButton FolderForwardButton;
        private System.Windows.Forms.ToolStripTextBox CurrentPathFolder;
        private System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem FileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem закрытьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ViewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem IconViewMenuButton;
        private System.Windows.Forms.ToolStripMenuItem ImageViewMenuButton;
        private System.Windows.Forms.ToolStripMenuItem TileViewMenuButton;
        private System.Windows.Forms.ToolStripMenuItem ListViewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TableViewMenuButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem RefreshMenuButton;
        private System.Windows.Forms.ImageList Icons16;
        private System.Windows.Forms.ImageList Icons32;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer ShellContainer;
        private System.Windows.Forms.TreeView TreeView;
        private System.Windows.Forms.ListView ShellView;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton UpButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton RefreshButton;
    }
}

