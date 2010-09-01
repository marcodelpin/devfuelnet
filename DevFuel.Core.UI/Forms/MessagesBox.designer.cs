namespace DevFuel.Core.UI.Forms
{
    partial class MessagesBox
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
            this.tcErrors = new System.Windows.Forms.TabControl();
            this.pageMessage = new System.Windows.Forms.TabPage();
            this.tbxMessage = new System.Windows.Forms.TextBox();
            this.pageInnerException = new System.Windows.Forms.TabPage();
            this.tbxInner = new System.Windows.Forms.TextBox();
            this.btnDone = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.tcErrors.SuspendLayout();
            this.pageMessage.SuspendLayout();
            this.pageInnerException.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcErrors
            // 
            this.tcErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcErrors.Controls.Add(this.pageMessage);
            this.tcErrors.Controls.Add(this.pageInnerException);
            this.tcErrors.Location = new System.Drawing.Point(12, 12);
            this.tcErrors.Margin = new System.Windows.Forms.Padding(5);
            this.tcErrors.Name = "tcErrors";
            this.tcErrors.SelectedIndex = 0;
            this.tcErrors.Size = new System.Drawing.Size(431, 282);
            this.tcErrors.TabIndex = 0;
            // 
            // pageMessage
            // 
            this.pageMessage.Controls.Add(this.tbxMessage);
            this.pageMessage.Location = new System.Drawing.Point(4, 22);
            this.pageMessage.Name = "pageMessage";
            this.pageMessage.Padding = new System.Windows.Forms.Padding(3);
            this.pageMessage.Size = new System.Drawing.Size(423, 256);
            this.pageMessage.TabIndex = 0;
            this.pageMessage.Text = "Message(s)";
            this.pageMessage.UseVisualStyleBackColor = true;
            // 
            // tbxMessage
            // 
            this.tbxMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxMessage.Location = new System.Drawing.Point(3, 3);
            this.tbxMessage.Multiline = true;
            this.tbxMessage.Name = "tbxMessage";
            this.tbxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxMessage.Size = new System.Drawing.Size(417, 250);
            this.tbxMessage.TabIndex = 0;
            // 
            // pageInnerException
            // 
            this.pageInnerException.Controls.Add(this.tbxInner);
            this.pageInnerException.Location = new System.Drawing.Point(4, 22);
            this.pageInnerException.Name = "pageInnerException";
            this.pageInnerException.Padding = new System.Windows.Forms.Padding(3);
            this.pageInnerException.Size = new System.Drawing.Size(423, 256);
            this.pageInnerException.TabIndex = 1;
            this.pageInnerException.Text = "Details";
            this.pageInnerException.UseVisualStyleBackColor = true;
            // 
            // tbxInner
            // 
            this.tbxInner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxInner.Location = new System.Drawing.Point(3, 3);
            this.tbxInner.Multiline = true;
            this.tbxInner.Name = "tbxInner";
            this.tbxInner.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxInner.Size = new System.Drawing.Size(417, 250);
            this.tbxInner.TabIndex = 0;
            // 
            // btnDone
            // 
            this.btnDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDone.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnDone.Location = new System.Drawing.Point(363, 302);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 26);
            this.btnDone.TabIndex = 1;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCopy.Location = new System.Drawing.Point(12, 302);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(109, 26);
            this.btnCopy.TabIndex = 2;
            this.btnCopy.Text = "Copy to Clipboard";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // MessagesBox
            // 
            this.AcceptButton = this.btnDone;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 340);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.tcErrors);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(522, 456);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(227, 195);
            this.Name = "MessagesBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Message(s)";
            this.Load += new System.EventHandler(this.ExceptionBox_Load);
            this.tcErrors.ResumeLayout(false);
            this.pageMessage.ResumeLayout(false);
            this.pageMessage.PerformLayout();
            this.pageInnerException.ResumeLayout(false);
            this.pageInnerException.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcErrors;
        private System.Windows.Forms.TabPage pageMessage;
        private System.Windows.Forms.TabPage pageInnerException;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.TextBox tbxMessage;
        private System.Windows.Forms.TextBox tbxInner;
    }
}