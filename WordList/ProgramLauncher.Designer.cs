namespace WindowsFormsApplication1
{
    partial class frmProgramLauncher
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
            this.btnWordList = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnWordList
            // 
            this.btnWordList.Location = new System.Drawing.Point(59, 35);
            this.btnWordList.Name = "btnWordList";
            this.btnWordList.Size = new System.Drawing.Size(156, 95);
            this.btnWordList.TabIndex = 0;
            this.btnWordList.Text = "WordList";
            this.btnWordList.UseVisualStyleBackColor = true;
            this.btnWordList.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmProgramLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btnWordList);
            this.Name = "frmProgramLauncher";
            this.Text = "Program Launcher";
            this.Load += new System.EventHandler(this.frmProgramLauncher_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnWordList;
    }
}

