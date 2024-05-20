
namespace IRIDIUM_GMDSS_LRIT.AdminWinFormUI
{
    partial class AdminWinFormUIFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminWinFormUIFrm));
            this.btnStartForwarder = new System.Windows.Forms.Button();
            this.btnStartSMMPClientMonitor = new System.Windows.Forms.Button();
            this.richTxtOutput = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnStartForwarder
            // 
            this.btnStartForwarder.Location = new System.Drawing.Point(13, 13);
            this.btnStartForwarder.Name = "btnStartForwarder";
            this.btnStartForwarder.Size = new System.Drawing.Size(175, 23);
            this.btnStartForwarder.TabIndex = 0;
            this.btnStartForwarder.Text = "Start Forwarder";
            this.btnStartForwarder.UseVisualStyleBackColor = true;
            this.btnStartForwarder.Click += new System.EventHandler(this.btnStartForwarder_Click);
            // 
            // btnStartSMMPClientMonitor
            // 
            this.btnStartSMMPClientMonitor.Location = new System.Drawing.Point(243, 12);
            this.btnStartSMMPClientMonitor.Name = "btnStartSMMPClientMonitor";
            this.btnStartSMMPClientMonitor.Size = new System.Drawing.Size(212, 23);
            this.btnStartSMMPClientMonitor.TabIndex = 1;
            this.btnStartSMMPClientMonitor.Text = "Start SMMP Client Monitor";
            this.btnStartSMMPClientMonitor.UseVisualStyleBackColor = true;
            this.btnStartSMMPClientMonitor.Click += new System.EventHandler(this.btnStartSMMPClientMonitor_Click);
            // 
            // richTxtOutput
            // 
            this.richTxtOutput.Location = new System.Drawing.Point(13, 59);
            this.richTxtOutput.Name = "richTxtOutput";
            this.richTxtOutput.Size = new System.Drawing.Size(775, 379);
            this.richTxtOutput.TabIndex = 2;
            this.richTxtOutput.Text = "";
            // 
            // AdminWinFormUIFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.richTxtOutput);
            this.Controls.Add(this.btnStartSMMPClientMonitor);
            this.Controls.Add(this.btnStartForwarder);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AdminWinFormUIFrm";
            this.Text = "Kemilinks Iridium GMDSS LRIT Admin UI";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStartForwarder;
        private System.Windows.Forms.Button btnStartSMMPClientMonitor;
        private System.Windows.Forms.RichTextBox richTxtOutput;
    }
}

