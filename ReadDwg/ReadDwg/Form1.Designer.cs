namespace ReadDwg
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.listViewFile = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.richTextMsg = new System.Windows.Forms.RichTextBox();
            this.comboCADVersion = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(119, 351);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 55);
            this.button1.TabIndex = 0;
            this.button1.Text = "打开dwg文件";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listViewFile
            // 
            this.listViewFile.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewFile.FullRowSelect = true;
            this.listViewFile.GridLines = true;
            this.listViewFile.Location = new System.Drawing.Point(12, 12);
            this.listViewFile.MultiSelect = false;
            this.listViewFile.Name = "listViewFile";
            this.listViewFile.Size = new System.Drawing.Size(315, 207);
            this.listViewFile.TabIndex = 1;
            this.listViewFile.UseCompatibleStateImageBehavior = false;
            this.listViewFile.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "字体文件名";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "状态";
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(226, 351);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(101, 55);
            this.buttonUpdate.TabIndex = 0;
            this.buttonUpdate.Text = "下载文体文件";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // richTextMsg
            // 
            this.richTextMsg.Location = new System.Drawing.Point(12, 225);
            this.richTextMsg.Name = "richTextMsg";
            this.richTextMsg.Size = new System.Drawing.Size(315, 109);
            this.richTextMsg.TabIndex = 2;
            this.richTextMsg.Text = "";
            // 
            // comboCADVersion
            // 
            this.comboCADVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboCADVersion.FormattingEnabled = true;
            this.comboCADVersion.Items.AddRange(new object[] {
            "CAD 2005",
            "CAD 2008",
            "CAD 2009",
            "CAD 2010"});
            this.comboCADVersion.Location = new System.Drawing.Point(7, 29);
            this.comboCADVersion.Name = "comboCADVersion";
            this.comboCADVersion.Size = new System.Drawing.Size(95, 20);
            this.comboCADVersion.TabIndex = 3;
            this.comboCADVersion.SelectedIndexChanged += new System.EventHandler(this.comboCADVersion_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboCADVersion);
            this.groupBox1.Location = new System.Drawing.Point(5, 340);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(108, 66);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "请选择要下载的CAD版本";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 413);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.richTextMsg);
            this.Controls.Add(this.listViewFile);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自动下载CAD字体文件";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listViewFile;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.RichTextBox richTextMsg;
        private System.Windows.Forms.ComboBox comboCADVersion;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

