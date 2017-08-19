namespace ServerTogetherCSharp
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.IDC_MAIN_LISTBOX = new System.Windows.Forms.ListBox();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.IDC_LABLE_LOGDATE = new System.Windows.Forms.Label();
            this.IDC_LABLE_GEBIN_TIME = new System.Windows.Forms.Label();
            this.IDC_LABLE_NOW_TIME = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // IDC_MAIN_LISTBOX
            // 
            this.IDC_MAIN_LISTBOX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.IDC_MAIN_LISTBOX.ForeColor = System.Drawing.Color.Black;
            this.IDC_MAIN_LISTBOX.FormattingEnabled = true;
            this.IDC_MAIN_LISTBOX.HorizontalScrollbar = true;
            this.IDC_MAIN_LISTBOX.ItemHeight = 12;
            this.IDC_MAIN_LISTBOX.Location = new System.Drawing.Point(26, 53);
            this.IDC_MAIN_LISTBOX.Name = "IDC_MAIN_LISTBOX";
            this.IDC_MAIN_LISTBOX.Size = new System.Drawing.Size(685, 196);
            this.IDC_MAIN_LISTBOX.TabIndex = 0;
            // 
            // Timer
            // 
            this.Timer.Enabled = true;
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // IDC_LABLE_LOGDATE
            // 
            this.IDC_LABLE_LOGDATE.AutoSize = true;
            this.IDC_LABLE_LOGDATE.BackColor = System.Drawing.Color.Transparent;
            this.IDC_LABLE_LOGDATE.Location = new System.Drawing.Point(284, 38);
            this.IDC_LABLE_LOGDATE.Name = "IDC_LABLE_LOGDATE";
            this.IDC_LABLE_LOGDATE.Size = new System.Drawing.Size(41, 12);
            this.IDC_LABLE_LOGDATE.TabIndex = 1;
            this.IDC_LABLE_LOGDATE.Text = "label1";
            // 
            // IDC_LABLE_GEBIN_TIME
            // 
            this.IDC_LABLE_GEBIN_TIME.AutoSize = true;
            this.IDC_LABLE_GEBIN_TIME.BackColor = System.Drawing.Color.Transparent;
            this.IDC_LABLE_GEBIN_TIME.Location = new System.Drawing.Point(12, 9);
            this.IDC_LABLE_GEBIN_TIME.Name = "IDC_LABLE_GEBIN_TIME";
            this.IDC_LABLE_GEBIN_TIME.Size = new System.Drawing.Size(41, 12);
            this.IDC_LABLE_GEBIN_TIME.TabIndex = 2;
            this.IDC_LABLE_GEBIN_TIME.Text = "label1";
            // 
            // IDC_LABLE_NOW_TIME
            // 
            this.IDC_LABLE_NOW_TIME.AutoSize = true;
            this.IDC_LABLE_NOW_TIME.BackColor = System.Drawing.Color.Transparent;
            this.IDC_LABLE_NOW_TIME.Location = new System.Drawing.Point(546, 9);
            this.IDC_LABLE_NOW_TIME.Name = "IDC_LABLE_NOW_TIME";
            this.IDC_LABLE_NOW_TIME.Size = new System.Drawing.Size(41, 12);
            this.IDC_LABLE_NOW_TIME.TabIndex = 3;
            this.IDC_LABLE_NOW_TIME.Text = "label2";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::ServerTogetherCSharp.Properties.Resources.anyouhui110389502128799e2b5;
            this.ClientSize = new System.Drawing.Size(745, 290);
            this.Controls.Add(this.IDC_LABLE_NOW_TIME);
            this.Controls.Add(this.IDC_LABLE_GEBIN_TIME);
            this.Controls.Add(this.IDC_LABLE_LOGDATE);
            this.Controls.Add(this.IDC_MAIN_LISTBOX);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "养殖场服务器";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox IDC_MAIN_LISTBOX;
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.Label IDC_LABLE_LOGDATE;
        private System.Windows.Forms.Label IDC_LABLE_GEBIN_TIME;
        private System.Windows.Forms.Label IDC_LABLE_NOW_TIME;
    }
}

