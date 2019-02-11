namespace hs_autonet
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.udpTimer = new System.Windows.Forms.Timer(this.components);
            this.logBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TimerTime = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.TimerTime)).BeginInit();
            this.SuspendLayout();
            // 
            // udpTimer
            // 
            this.udpTimer.Enabled = true;
            this.udpTimer.Interval = 1000;
            this.udpTimer.Tick += new System.EventHandler(this.UdpTimer_Tick);
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(12, 200);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.Size = new System.Drawing.Size(436, 224);
            this.logBox.TabIndex = 0;
            this.logBox.DoubleClick += new System.EventHandler(this.LogBox_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 185);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "log";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "TimerTickTime(ms)";
            // 
            // TimerTime
            // 
            this.TimerTime.Location = new System.Drawing.Point(12, 30);
            this.TimerTime.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.TimerTime.Name = "TimerTime";
            this.TimerTime.Size = new System.Drawing.Size(105, 21);
            this.TimerTime.TabIndex = 4;
            this.TimerTime.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.TimerTime.ValueChanged += new System.EventHandler(this.TimerTime_ValueChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 436);
            this.Controls.Add(this.TimerTime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.logBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TimerTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer udpTimer;
        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown TimerTime;
    }
}

