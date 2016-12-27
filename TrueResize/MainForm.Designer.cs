namespace TrueResize
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnNext = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.openVolumeFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1 = new TrueResize.WizardControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.btnKeyfiles = new System.Windows.Forms.Button();
            this.chkUseKeyfiles = new System.Windows.Forms.CheckBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtVolumeFile = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblSelectFile = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.listDetails = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioGB = new System.Windows.Forms.RadioButton();
            this.radioMB = new System.Windows.Forms.RadioButton();
            this.lblFreeSpaceOnDrive = new System.Windows.Forms.Label();
            this.txtSizeToAdd = new System.Windows.Forms.TextBox();
            this.lblSizeToAdd = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.progressBarFillWithRandomData = new System.Windows.Forms.ProgressBar();
            this.chkFillWithRandomData = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(335, 217);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = "Next >";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnBack
            // 
            this.btnBack.Enabled = false;
            this.btnBack.Location = new System.Drawing.Point(254, 217);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 1;
            this.btnBack.Text = "< Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // openVolumeFileDialog
            // 
            this.openVolumeFileDialog.Filter = "All Supported Files (*.tc, *.vhd, *.vmdk)|*.tc;*.vhd;*.vmdk|TrueCrypt Files (*.tc" +
                ")|*.tc|VHD Files (*.vhd)|*.vhd|VMDK Files (*.vmdk)|*.vmdk|All Files (*.*)|*.*";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(424, 200);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.TabStop = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.btnKeyfiles);
            this.tabPage1.Controls.Add(this.chkUseKeyfiles);
            this.tabPage1.Controls.Add(this.btnBrowse);
            this.tabPage1.Controls.Add(this.txtVolumeFile);
            this.tabPage1.Controls.Add(this.txtPassword);
            this.tabPage1.Controls.Add(this.lblPassword);
            this.tabPage1.Controls.Add(this.lblSelectFile);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(416, 174);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label5.Location = new System.Drawing.Point(10, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Volume Location:";
            // 
            // btnKeyfiles
            // 
            this.btnKeyfiles.Enabled = false;
            this.btnKeyfiles.Location = new System.Drawing.Point(189, 105);
            this.btnKeyfiles.Name = "btnKeyfiles";
            this.btnKeyfiles.Size = new System.Drawing.Size(75, 23);
            this.btnKeyfiles.TabIndex = 7;
            this.btnKeyfiles.Text = "Keyfiles..";
            this.btnKeyfiles.UseVisualStyleBackColor = true;
            this.btnKeyfiles.Click += new System.EventHandler(this.btnKeyfiles_Click);
            // 
            // chkUseKeyfiles
            // 
            this.chkUseKeyfiles.AutoSize = true;
            this.chkUseKeyfiles.Location = new System.Drawing.Point(81, 109);
            this.chkUseKeyfiles.Name = "chkUseKeyfiles";
            this.chkUseKeyfiles.Size = new System.Drawing.Size(83, 17);
            this.chkUseKeyfiles.TabIndex = 6;
            this.chkUseKeyfiles.Text = "Use keyfiles";
            this.chkUseKeyfiles.UseVisualStyleBackColor = true;
            this.chkUseKeyfiles.CheckedChanged += new System.EventHandler(this.chkUseKeyfiles_CheckedChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(270, 33);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtVolumeFile
            // 
            this.txtVolumeFile.BackColor = System.Drawing.SystemColors.Control;
            this.txtVolumeFile.Location = new System.Drawing.Point(81, 35);
            this.txtVolumeFile.Name = "txtVolumeFile";
            this.txtVolumeFile.ReadOnly = true;
            this.txtVolumeFile.Size = new System.Drawing.Size(183, 20);
            this.txtVolumeFile.TabIndex = 3;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(81, 72);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(183, 20);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(10, 75);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 1;
            this.lblPassword.Text = "Password:";
            // 
            // lblSelectFile
            // 
            this.lblSelectFile.AutoSize = true;
            this.lblSelectFile.Location = new System.Drawing.Point(10, 35);
            this.lblSelectFile.Name = "lblSelectFile";
            this.lblSelectFile.Size = new System.Drawing.Size(56, 13);
            this.lblSelectFile.TabIndex = 0;
            this.lblSelectFile.Text = "Select file:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.listDetails);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(416, 174);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label6.Location = new System.Drawing.Point(10, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Volume Properties:";
            // 
            // listDetails
            // 
            this.listDetails.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listDetails.Location = new System.Drawing.Point(6, 33);
            this.listDetails.Name = "listDetails";
            this.listDetails.Size = new System.Drawing.Size(404, 152);
            this.listDetails.TabIndex = 0;
            this.listDetails.UseCompatibleStateImageBehavior = false;
            this.listDetails.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Property";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Value";
            this.columnHeader2.Width = 200;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.panel1);
            this.tabPage3.Controls.Add(this.lblFreeSpaceOnDrive);
            this.tabPage3.Controls.Add(this.txtSizeToAdd);
            this.tabPage3.Controls.Add(this.lblSizeToAdd);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(416, 174);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label7.Location = new System.Drawing.Point(10, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Volume Size:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioGB);
            this.panel1.Controls.Add(this.radioMB);
            this.panel1.Location = new System.Drawing.Point(218, 32);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(107, 26);
            this.panel1.TabIndex = 7;
            // 
            // radioGB
            // 
            this.radioGB.AutoSize = true;
            this.radioGB.Location = new System.Drawing.Point(54, 3);
            this.radioGB.Name = "radioGB";
            this.radioGB.Size = new System.Drawing.Size(40, 17);
            this.radioGB.TabIndex = 3;
            this.radioGB.Text = "GB";
            this.radioGB.UseVisualStyleBackColor = true;
            // 
            // radioMB
            // 
            this.radioMB.AutoSize = true;
            this.radioMB.Checked = true;
            this.radioMB.Location = new System.Drawing.Point(3, 3);
            this.radioMB.Name = "radioMB";
            this.radioMB.Size = new System.Drawing.Size(41, 17);
            this.radioMB.TabIndex = 2;
            this.radioMB.TabStop = true;
            this.radioMB.Text = "MB";
            this.radioMB.UseVisualStyleBackColor = true;
            // 
            // lblFreeSpaceOnDrive
            // 
            this.lblFreeSpaceOnDrive.AutoSize = true;
            this.lblFreeSpaceOnDrive.Location = new System.Drawing.Point(10, 65);
            this.lblFreeSpaceOnDrive.Name = "lblFreeSpaceOnDrive";
            this.lblFreeSpaceOnDrive.Size = new System.Drawing.Size(97, 13);
            this.lblFreeSpaceOnDrive.TabIndex = 4;
            this.lblFreeSpaceOnDrive.Text = "Free space on disk";
            // 
            // txtSizeToAdd
            // 
            this.txtSizeToAdd.Location = new System.Drawing.Point(111, 32);
            this.txtSizeToAdd.Name = "txtSizeToAdd";
            this.txtSizeToAdd.Size = new System.Drawing.Size(100, 20);
            this.txtSizeToAdd.TabIndex = 1;
            // 
            // lblSizeToAdd
            // 
            this.lblSizeToAdd.AutoSize = true;
            this.lblSizeToAdd.Location = new System.Drawing.Point(10, 35);
            this.lblSizeToAdd.Name = "lblSizeToAdd";
            this.lblSizeToAdd.Size = new System.Drawing.Size(99, 13);
            this.lblSizeToAdd.TabIndex = 0;
            this.lblSizeToAdd.Text = "Additional capacity:";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.progressBarFillWithRandomData);
            this.tabPage4.Controls.Add(this.chkFillWithRandomData);
            this.tabPage4.Controls.Add(this.label2);
            this.tabPage4.Controls.Add(this.label1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(416, 174);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // progressBarFillWithRandomData
            // 
            this.progressBarFillWithRandomData.Location = new System.Drawing.Point(12, 135);
            this.progressBarFillWithRandomData.Name = "progressBarFillWithRandomData";
            this.progressBarFillWithRandomData.Size = new System.Drawing.Size(394, 23);
            this.progressBarFillWithRandomData.TabIndex = 3;
            this.progressBarFillWithRandomData.Visible = false;
            // 
            // chkFillWithRandomData
            // 
            this.chkFillWithRandomData.AutoSize = true;
            this.chkFillWithRandomData.Location = new System.Drawing.Point(12, 112);
            this.chkFillWithRandomData.Name = "chkFillWithRandomData";
            this.chkFillWithRandomData.Size = new System.Drawing.Size(220, 17);
            this.chkFillWithRandomData.TabIndex = 2;
            this.chkFillWithRandomData.Text = "Fill the additional space with random data";
            this.chkFillWithRandomData.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label2.Location = new System.Drawing.Point(9, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(399, 65);
            this.label2.TabIndex = 1;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Allocated space:";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.label4);
            this.tabPage5.Controls.Add(this.label3);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(416, 174);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "tabPage5";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label4.Location = new System.Drawing.Point(9, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(339, 39);
            this.label4.TabIndex = 2;
            this.label4.Text = "The volume and underlying file-system have been successfully resized.\r\n\r\nPress Fi" +
                "nish to exit the program.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label3.Location = new System.Drawing.Point(10, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Resize completed.";
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 253);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(430, 280);
            this.MinimumSize = new System.Drawing.Size(430, 280);
            this.Name = "MainForm";
            this.Text = "TrueResize";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private WizardControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblSelectFile;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtVolumeFile;
        private System.Windows.Forms.OpenFileDialog openVolumeFileDialog;
        private System.Windows.Forms.CheckBox chkUseKeyfiles;
        private System.Windows.Forms.Button btnKeyfiles;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.ListView listDetails;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.RadioButton radioGB;
        private System.Windows.Forms.RadioButton radioMB;
        private System.Windows.Forms.TextBox txtSizeToAdd;
        private System.Windows.Forms.Label lblSizeToAdd;
        private System.Windows.Forms.Label lblFreeSpaceOnDrive;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkFillWithRandomData;
        private System.Windows.Forms.ProgressBar progressBarFillWithRandomData;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}

