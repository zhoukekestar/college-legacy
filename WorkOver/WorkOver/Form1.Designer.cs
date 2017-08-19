namespace WorkOver
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.IDC_COMBOX_HOUR1 = new System.Windows.Forms.ComboBox();
            this.IDC_COMBOX_HOUR2 = new System.Windows.Forms.ComboBox();
            this.IDC_COMBOX_MINU1 = new System.Windows.Forms.ComboBox();
            this.IDC_COMBOX_MINU2 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.IDC_TIMER = new System.Windows.Forms.Timer(this.components);
            this.IDC_LABEL_TIME = new System.Windows.Forms.Label();
            this.notifyicon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "小时";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "小时";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(109, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "分钟";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(109, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "分钟";
            // 
            // IDC_COMBOX_HOUR1
            // 
            this.IDC_COMBOX_HOUR1.FormattingEnabled = true;
            this.IDC_COMBOX_HOUR1.Location = new System.Drawing.Point(47, 24);
            this.IDC_COMBOX_HOUR1.Name = "IDC_COMBOX_HOUR1";
            this.IDC_COMBOX_HOUR1.Size = new System.Drawing.Size(47, 20);
            this.IDC_COMBOX_HOUR1.TabIndex = 8;
            // 
            // IDC_COMBOX_HOUR2
            // 
            this.IDC_COMBOX_HOUR2.FormattingEnabled = true;
            this.IDC_COMBOX_HOUR2.Location = new System.Drawing.Point(47, 86);
            this.IDC_COMBOX_HOUR2.Name = "IDC_COMBOX_HOUR2";
            this.IDC_COMBOX_HOUR2.Size = new System.Drawing.Size(47, 20);
            this.IDC_COMBOX_HOUR2.TabIndex = 8;
            // 
            // IDC_COMBOX_MINU1
            // 
            this.IDC_COMBOX_MINU1.FormattingEnabled = true;
            this.IDC_COMBOX_MINU1.Location = new System.Drawing.Point(144, 24);
            this.IDC_COMBOX_MINU1.Name = "IDC_COMBOX_MINU1";
            this.IDC_COMBOX_MINU1.Size = new System.Drawing.Size(47, 20);
            this.IDC_COMBOX_MINU1.TabIndex = 8;
            // 
            // IDC_COMBOX_MINU2
            // 
            this.IDC_COMBOX_MINU2.FormattingEnabled = true;
            this.IDC_COMBOX_MINU2.Location = new System.Drawing.Point(144, 86);
            this.IDC_COMBOX_MINU2.Name = "IDC_COMBOX_MINU2";
            this.IDC_COMBOX_MINU2.Size = new System.Drawing.Size(47, 20);
            this.IDC_COMBOX_MINU2.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(45, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 12);
            this.label5.TabIndex = 9;
            // 
            // IDC_TIMER
            // 
            this.IDC_TIMER.Enabled = true;
            this.IDC_TIMER.Interval = 500;
            this.IDC_TIMER.Tick += new System.EventHandler(this.IDC_TIMER_Tick);
            // 
            // IDC_LABEL_TIME
            // 
            this.IDC_LABEL_TIME.AutoSize = true;
            this.IDC_LABEL_TIME.Location = new System.Drawing.Point(47, 122);
            this.IDC_LABEL_TIME.Name = "IDC_LABEL_TIME";
            this.IDC_LABEL_TIME.Size = new System.Drawing.Size(35, 12);
            this.IDC_LABEL_TIME.TabIndex = 10;
            this.IDC_LABEL_TIME.Text = "TIMER";
            // 
            // notifyicon
            // 
            this.notifyicon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyicon.Icon")));
            this.notifyicon.Text = "notifyicon";
            this.notifyicon.Visible = true;
            this.notifyicon.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 143);
            this.Controls.Add(this.IDC_LABEL_TIME);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.IDC_COMBOX_MINU2);
            this.Controls.Add(this.IDC_COMBOX_MINU1);
            this.Controls.Add(this.IDC_COMBOX_HOUR2);
            this.Controls.Add(this.IDC_COMBOX_HOUR1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "提醒";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox IDC_COMBOX_HOUR1;
        private System.Windows.Forms.ComboBox IDC_COMBOX_HOUR2;
        private System.Windows.Forms.ComboBox IDC_COMBOX_MINU1;
        private System.Windows.Forms.ComboBox IDC_COMBOX_MINU2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Timer IDC_TIMER;
        private System.Windows.Forms.Label IDC_LABEL_TIME;
        private System.Windows.Forms.NotifyIcon notifyicon;
    }
}

