namespace UareUSampleCSharp
{
    partial class Form_Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false</param>
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
            this.txtReaderSelected = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.btnEnrollmentControl = new System.Windows.Forms.Button();
            this.btnIdentificationControl = new System.Windows.Forms.Button();
            this.btnReaderSelect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtReaderSelected
            // 
            this.txtReaderSelected.Font = new System.Drawing.Font("Tahoma", 8F);
            this.txtReaderSelected.Location = new System.Drawing.Point(15, 27);
            this.txtReaderSelected.Name = "txtReaderSelected";
            this.txtReaderSelected.ReadOnly = true;
            this.txtReaderSelected.Size = new System.Drawing.Size(233, 24);
            this.txtReaderSelected.TabIndex = 7;
            // 
            // Label1
            // 
            this.Label1.Location = new System.Drawing.Point(12, 9);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(236, 15);
            this.Label1.TabIndex = 18;
            this.Label1.Text = "Selected Device";
            // 
            // btnEnrollmentControl
            // 
            this.btnEnrollmentControl.Enabled = false;
            this.btnEnrollmentControl.Location = new System.Drawing.Point(15, 82);
            this.btnEnrollmentControl.Name = "btnEnrollmentControl";
            this.btnEnrollmentControl.Size = new System.Drawing.Size(233, 44);
            this.btnEnrollmentControl.TabIndex = 16;
            this.btnEnrollmentControl.Text = "Identification Member";
            this.btnEnrollmentControl.Click += new System.EventHandler(this.btnEnrollmentControl_Click);
            // 
            // btnIdentificationControl
            // 
            this.btnIdentificationControl.Enabled = false;
            this.btnIdentificationControl.Location = new System.Drawing.Point(16, 132);
            this.btnIdentificationControl.Name = "btnIdentificationControl";
            this.btnIdentificationControl.Size = new System.Drawing.Size(232, 33);
            this.btnIdentificationControl.TabIndex = 17;
            this.btnIdentificationControl.Text = "Personal Trainer";
            this.btnIdentificationControl.Click += new System.EventHandler(this.btnIdentificationControl_Click);
            // 
            // btnReaderSelect
            // 
            this.btnReaderSelect.Location = new System.Drawing.Point(12, 53);
            this.btnReaderSelect.Name = "btnReaderSelect";
            this.btnReaderSelect.Size = new System.Drawing.Size(236, 23);
            this.btnReaderSelect.TabIndex = 8;
            this.btnReaderSelect.Text = "Init Device";
            this.btnReaderSelect.Click += new System.EventHandler(this.btnReaderSelect_Click);
            // 
            // Form_Main
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(259, 180);
            this.Controls.Add(this.btnIdentificationControl);
            this.Controls.Add(this.btnEnrollmentControl);
            this.Controls.Add(this.txtReaderSelected);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.btnReaderSelect);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(277, 227);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(277, 227);
            this.Name = "Form_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "U.are.U Sample C#";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TextBox txtReaderSelected;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Button btnEnrollmentControl;
        public System.Windows.Forms.Button btnIdentificationControl;
        internal System.Windows.Forms.Button btnReaderSelect;
    }
}