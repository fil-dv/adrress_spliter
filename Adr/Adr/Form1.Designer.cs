namespace Adr
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.listBox_adr = new System.Windows.Forms.ListBox();
            this.numericUpDown_threads = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_adr_count = new System.Windows.Forms.Label();
            this.button_start = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.button_path = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_threads)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox_adr
            // 
            this.listBox_adr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_adr.FormattingEnabled = true;
            this.listBox_adr.HorizontalScrollbar = true;
            this.listBox_adr.Location = new System.Drawing.Point(15, 75);
            this.listBox_adr.Name = "listBox_adr";
            this.listBox_adr.Size = new System.Drawing.Size(616, 368);
            this.listBox_adr.TabIndex = 1;
            // 
            // numericUpDown_threads
            // 
            this.numericUpDown_threads.Location = new System.Drawing.Point(307, 21);
            this.numericUpDown_threads.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDown_threads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_threads.Name = "numericUpDown_threads";
            this.numericUpDown_threads.Size = new System.Drawing.Size(52, 20);
            this.numericUpDown_threads.TabIndex = 2;
            this.numericUpDown_threads.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown_threads.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_threads.ValueChanged += new System.EventHandler(this.numericUpDown_threads_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(179, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Количество потоков:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Всего адресов:";
            // 
            // label_adr_count
            // 
            this.label_adr_count.AutoSize = true;
            this.label_adr_count.Location = new System.Drawing.Point(103, 26);
            this.label_adr_count.Name = "label_adr_count";
            this.label_adr_count.Size = new System.Drawing.Size(0, 13);
            this.label_adr_count.TabIndex = 5;
            // 
            // button_start
            // 
            this.button_start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_start.Location = new System.Drawing.Point(535, 21);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(87, 23);
            this.button_start.TabIndex = 14;
            this.button_start.Text = "Ok";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 449);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(643, 22);
            this.statusStrip1.TabIndex = 15;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // button_path
            // 
            this.button_path.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_path.Location = new System.Drawing.Point(405, 21);
            this.button_path.Name = "button_path";
            this.button_path.Size = new System.Drawing.Size(92, 23);
            this.button_path.TabIndex = 16;
            this.button_path.Text = "Папка реестра";
            this.button_path.UseVisualStyleBackColor = true;
            this.button_path.Click += new System.EventHandler(this.button_path_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 471);
            this.Controls.Add(this.button_path);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.label_adr_count);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown_threads);
            this.Controls.Add(this.listBox_adr);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Адреса";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_threads)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox listBox_adr;
        private System.Windows.Forms.NumericUpDown numericUpDown_threads;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_adr_count;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button button_path;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}

