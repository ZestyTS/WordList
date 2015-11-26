namespace WindowsFormsApplication1
{
    partial class frmWordList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWordList));
            this.lblDesc = new System.Windows.Forms.Label();
            this.lbFiles = new System.Windows.Forms.ListBox();
            this.lblFiles = new System.Windows.Forms.Label();
            this.txtWhiteList = new System.Windows.Forms.TextBox();
            this.lblWhiteList = new System.Windows.Forms.Label();
            this.btnEmail = new System.Windows.Forms.Button();
            this.btnHythen = new System.Windows.Forms.Button();
            this.btnTime = new System.Windows.Forms.Button();
            this.lbOutput = new System.Windows.Forms.ListBox();
            this.btnWordList = new System.Windows.Forms.Button();
            this.lblOutput = new System.Windows.Forms.Label();
            this.btnCurrentDir = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(28, 9);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(706, 67);
            this.lblDesc.TabIndex = 0;
            this.lblDesc.Text = resources.GetString("lblDesc.Text");
            // 
            // lbFiles
            // 
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.HorizontalScrollbar = true;
            this.lbFiles.Location = new System.Drawing.Point(31, 121);
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.Size = new System.Drawing.Size(309, 173);
            this.lbFiles.TabIndex = 1;
            // 
            // lblFiles
            // 
            this.lblFiles.AutoSize = true;
            this.lblFiles.Location = new System.Drawing.Point(142, 105);
            this.lblFiles.Name = "lblFiles";
            this.lblFiles.Size = new System.Drawing.Size(62, 13);
            this.lblFiles.TabIndex = 3;
            this.lblFiles.Text = "Files to Use";
            // 
            // txtWhiteList
            // 
            this.txtWhiteList.Location = new System.Drawing.Point(447, 137);
            this.txtWhiteList.Name = "txtWhiteList";
            this.txtWhiteList.Size = new System.Drawing.Size(287, 20);
            this.txtWhiteList.TabIndex = 4;
            // 
            // lblWhiteList
            // 
            this.lblWhiteList.AutoSize = true;
            this.lblWhiteList.Location = new System.Drawing.Point(514, 121);
            this.lblWhiteList.Name = "lblWhiteList";
            this.lblWhiteList.Size = new System.Drawing.Size(157, 13);
            this.lblWhiteList.TabIndex = 5;
            this.lblWhiteList.Text = "Whitelist For Special Characters";
            // 
            // btnEmail
            // 
            this.btnEmail.Location = new System.Drawing.Point(447, 179);
            this.btnEmail.Name = "btnEmail";
            this.btnEmail.Size = new System.Drawing.Size(91, 23);
            this.btnEmail.TabIndex = 6;
            this.btnEmail.Text = "Email";
            this.btnEmail.UseVisualStyleBackColor = true;
            this.btnEmail.Click += new System.EventHandler(this.btnEmail_Click);
            // 
            // btnHythen
            // 
            this.btnHythen.Location = new System.Drawing.Point(546, 179);
            this.btnHythen.Name = "btnHythen";
            this.btnHythen.Size = new System.Drawing.Size(91, 23);
            this.btnHythen.TabIndex = 7;
            this.btnHythen.Text = "Hythen";
            this.btnHythen.UseVisualStyleBackColor = true;
            this.btnHythen.Click += new System.EventHandler(this.btnHythen_Click);
            // 
            // btnTime
            // 
            this.btnTime.Location = new System.Drawing.Point(643, 179);
            this.btnTime.Name = "btnTime";
            this.btnTime.Size = new System.Drawing.Size(91, 23);
            this.btnTime.TabIndex = 8;
            this.btnTime.Text = "Time";
            this.btnTime.UseVisualStyleBackColor = true;
            this.btnTime.Click += new System.EventHandler(this.btnTime_Click);
            // 
            // lbOutput
            // 
            this.lbOutput.Enabled = false;
            this.lbOutput.FormattingEnabled = true;
            this.lbOutput.Location = new System.Drawing.Point(232, 418);
            this.lbOutput.Name = "lbOutput";
            this.lbOutput.Size = new System.Drawing.Size(306, 147);
            this.lbOutput.TabIndex = 9;
            this.lbOutput.Visible = false;
            // 
            // btnWordList
            // 
            this.btnWordList.Location = new System.Drawing.Point(447, 256);
            this.btnWordList.Name = "btnWordList";
            this.btnWordList.Size = new System.Drawing.Size(287, 77);
            this.btnWordList.TabIndex = 10;
            this.btnWordList.Text = "Start WordList";
            this.btnWordList.UseVisualStyleBackColor = true;
            this.btnWordList.Click += new System.EventHandler(this.btnWordList_Click);
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Location = new System.Drawing.Point(361, 402);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(39, 13);
            this.lblOutput.TabIndex = 11;
            this.lblOutput.Text = "Output";
            this.lblOutput.Visible = false;
            // 
            // btnCurrentDir
            // 
            this.btnCurrentDir.AutoSize = true;
            this.btnCurrentDir.Location = new System.Drawing.Point(64, 310);
            this.btnCurrentDir.Name = "btnCurrentDir";
            this.btnCurrentDir.Size = new System.Drawing.Size(234, 23);
            this.btnCurrentDir.TabIndex = 12;
            this.btnCurrentDir.Text = "Add .txt Files From Program\'s Current Directory";
            this.btnCurrentDir.UseVisualStyleBackColor = true;
            this.btnCurrentDir.Click += new System.EventHandler(this.btnCurrentDir_Click);
            // 
            // frmWordList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 577);
            this.Controls.Add(this.btnCurrentDir);
            this.Controls.Add(this.lblOutput);
            this.Controls.Add(this.btnWordList);
            this.Controls.Add(this.lbOutput);
            this.Controls.Add(this.btnTime);
            this.Controls.Add(this.btnHythen);
            this.Controls.Add(this.btnEmail);
            this.Controls.Add(this.lblWhiteList);
            this.Controls.Add(this.txtWhiteList);
            this.Controls.Add(this.lblFiles);
            this.Controls.Add(this.lbFiles);
            this.Controls.Add(this.lblDesc);
            this.Name = "frmWordList";
            this.Text = "WordList";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.ListBox lbFiles;
        private System.Windows.Forms.Label lblFiles;
        private System.Windows.Forms.TextBox txtWhiteList;
        private System.Windows.Forms.Label lblWhiteList;
        private System.Windows.Forms.Button btnEmail;
        private System.Windows.Forms.Button btnHythen;
        private System.Windows.Forms.Button btnTime;
        private System.Windows.Forms.ListBox lbOutput;
        private System.Windows.Forms.Button btnWordList;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.Button btnCurrentDir;
    }
}