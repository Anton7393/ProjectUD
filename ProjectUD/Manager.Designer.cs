namespace ProjectUD
{
    partial class Manager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Manager));
            this.buttonAddDownloads = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonInfo = new System.Windows.Forms.Button();
            this.contextMenuStripList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.остановитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.перезапуститьЗакачкуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.остановитьЗакачкуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.перезапуститьВсеЗакачкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.listViewExDownloads = new ProjectUD.ListViewEx();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLink = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderProcess = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderButton = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderRelaod = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel2.SuspendLayout();
            this.contextMenuStripList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonAddDownloads
            // 
            this.buttonAddDownloads.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.buttonAddDownloads.Location = new System.Drawing.Point(3, 6);
            this.buttonAddDownloads.Name = "buttonAddDownloads";
            this.buttonAddDownloads.Size = new System.Drawing.Size(166, 50);
            this.buttonAddDownloads.TabIndex = 1;
            this.buttonAddDownloads.Text = "Добавить загрузку";
            this.buttonAddDownloads.UseVisualStyleBackColor = true;
            this.buttonAddDownloads.Click += new System.EventHandler(this.buttonAddDownloads_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Red;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(696, 8);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.buttonInfo);
            this.panel2.Controls.Add(this.listViewExDownloads);
            this.panel2.Controls.Add(this.buttonAddDownloads);
            this.panel2.Location = new System.Drawing.Point(12, 14);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(670, 285);
            this.panel2.TabIndex = 4;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.button2.Image = global::ProjectUD.Properties.Resources.stop;
            this.button2.Location = new System.Drawing.Point(503, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(50, 50);
            this.button2.TabIndex = 5;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.button1.Image = global::ProjectUD.Properties.Resources.cancel;
            this.button1.Location = new System.Drawing.Point(559, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(50, 50);
            this.button1.TabIndex = 4;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonInfo
            // 
            this.buttonInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.buttonInfo.Location = new System.Drawing.Point(615, 6);
            this.buttonInfo.Name = "buttonInfo";
            this.buttonInfo.Size = new System.Drawing.Size(50, 50);
            this.buttonInfo.TabIndex = 3;
            this.buttonInfo.Text = "?";
            this.buttonInfo.UseVisualStyleBackColor = true;
            this.buttonInfo.Click += new System.EventHandler(this.buttonInfo_Click);
            // 
            // contextMenuStripList
            // 
            this.contextMenuStripList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem7,
            this.остановитьToolStripMenuItem,
            this.перезапуститьЗакачкуToolStripMenuItem,
            this.остановитьЗакачкуToolStripMenuItem,
            this.перезапуститьВсеЗакачкиToolStripMenuItem});
            this.contextMenuStripList.Name = "contextMenuStripList";
            this.contextMenuStripList.Size = new System.Drawing.Size(223, 114);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(222, 22);
            this.toolStripMenuItem7.Text = "Открыть";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.toolStripMenuItem7_Click);
            // 
            // остановитьToolStripMenuItem
            // 
            this.остановитьToolStripMenuItem.Name = "остановитьToolStripMenuItem";
            this.остановитьToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.остановитьToolStripMenuItem.Text = "Остановить закачку";
            this.остановитьToolStripMenuItem.Click += new System.EventHandler(this.остановитьToolStripMenuItem_Click);
            // 
            // перезапуститьЗакачкуToolStripMenuItem
            // 
            this.перезапуститьЗакачкуToolStripMenuItem.Name = "перезапуститьЗакачкуToolStripMenuItem";
            this.перезапуститьЗакачкуToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.перезапуститьЗакачкуToolStripMenuItem.Text = "Перезапустить закачку";
            // 
            // остановитьЗакачкуToolStripMenuItem
            // 
            this.остановитьЗакачкуToolStripMenuItem.Name = "остановитьЗакачкуToolStripMenuItem";
            this.остановитьЗакачкуToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.остановитьЗакачкуToolStripMenuItem.Text = "Остановить все закачки";
            // 
            // перезапуститьВсеЗакачкиToolStripMenuItem
            // 
            this.перезапуститьВсеЗакачкиToolStripMenuItem.Name = "перезапуститьВсеЗакачкиToolStripMenuItem";
            this.перезапуститьВсеЗакачкиToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.перезапуститьВсеЗакачкиToolStripMenuItem.Text = "Перезапустить все закачки";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipText = "Приложение UD продолжает работать, доступно через трей";
            this.notifyIcon1.BalloonTipTitle = "Приложение UD";
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem6,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(203, 136);
            this.contextMenuStrip1.Text = "Свернуть";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(202, 22);
            this.toolStripMenuItem1.Text = "Развернуть окно";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(202, 22);
            this.toolStripMenuItem2.Text = "Свернуть окно";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(202, 22);
            this.toolStripMenuItem6.Text = "Добавить закачку";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(202, 22);
            this.toolStripMenuItem3.Text = "Отменить все закачки";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(202, 22);
            this.toolStripMenuItem4.Text = "Стартовать все закачки";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(202, 22);
            this.toolStripMenuItem5.Text = "Выход";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // listViewExDownloads
            // 
            this.listViewExDownloads.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewExDownloads.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderPath,
            this.columnHeaderLink,
            this.columnHeaderProcess,
            this.columnHeaderButton,
            this.columnHeaderRelaod});
            this.listViewExDownloads.ContextMenuStrip = this.contextMenuStripList;
            this.listViewExDownloads.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.listViewExDownloads.HideSelection = false;
            this.listViewExDownloads.HoverSelection = true;
            this.listViewExDownloads.Location = new System.Drawing.Point(3, 62);
            this.listViewExDownloads.Name = "listViewExDownloads";
            this.listViewExDownloads.Size = new System.Drawing.Size(662, 218);
            this.listViewExDownloads.TabIndex = 2;
            this.listViewExDownloads.UseCompatibleStateImageBehavior = false;
            this.listViewExDownloads.View = System.Windows.Forms.View.Details;
            this.listViewExDownloads.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewExDownloads_ItemSelectionChanged);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Имя";
            this.columnHeaderName.Width = 86;
            // 
            // columnHeaderPath
            // 
            this.columnHeaderPath.Text = "Путь";
            this.columnHeaderPath.Width = 180;
            // 
            // columnHeaderLink
            // 
            this.columnHeaderLink.Text = "Ссылка";
            this.columnHeaderLink.Width = 164;
            // 
            // columnHeaderProcess
            // 
            this.columnHeaderProcess.Text = "Процесс";
            this.columnHeaderProcess.Width = 124;
            // 
            // columnHeaderButton
            // 
            this.columnHeaderButton.Text = "";
            this.columnHeaderButton.Width = 38;
            // 
            // columnHeaderRelaod
            // 
            this.columnHeaderRelaod.Text = "";
            this.columnHeaderRelaod.Width = 38;
            // 
            // Manager
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(694, 312);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(710, 350);
            this.Name = "Manager";
            this.Text = "Менеджер загрузок";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Manager_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Manager_FormClosed);
            this.Load += new System.EventHandler(this.Manager_Load);
            this.Resize += new System.EventHandler(this.Manager_Resize);
            this.panel2.ResumeLayout(false);
            this.contextMenuStripList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonAddDownloads;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private ListViewEx listViewExDownloads;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderPath;
        private System.Windows.Forms.ColumnHeader columnHeaderLink;
        private System.Windows.Forms.ColumnHeader columnHeaderProcess;
        private System.Windows.Forms.ColumnHeader columnHeaderButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonInfo;
        private System.Windows.Forms.ColumnHeader columnHeaderRelaod;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        /// <summary>Трей</summary>
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripList;
        private System.Windows.Forms.ToolStripMenuItem остановитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem перезапуститьЗакачкуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem остановитьЗакачкуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem перезапуститьВсеЗакачкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
    }
}

