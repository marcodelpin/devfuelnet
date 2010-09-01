namespace DevFuel.Core.UI.Forms
{
    partial class LookupPropertyDropDownControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lbxOptions = new System.Windows.Forms.ListBox();
            this.contextMnu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnNone = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.miEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.miDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMnu.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbxOptions
            // 
            this.lbxOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbxOptions.FormattingEnabled = true;
            this.lbxOptions.Location = new System.Drawing.Point(3, 32);
            this.lbxOptions.Name = "lbxOptions";
            this.lbxOptions.Size = new System.Drawing.Size(216, 238);
            this.lbxOptions.TabIndex = 2;
            this.lbxOptions.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lbxOptions_MouseClick);
            this.lbxOptions.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.lbxOptions_Format);
            // 
            // contextMnu
            // 
            this.contextMnu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miEdit,
            this.miDelete});
            this.contextMnu.Name = "contextMenuStrip1";
            this.contextMnu.Size = new System.Drawing.Size(118, 48);
            this.contextMnu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMnu_Opening);
            // 
            // btnNone
            // 
            this.btnNone.Image = global::DevFuel.Core.UI.Properties.Resources.messagebox_critical;
            this.btnNone.Location = new System.Drawing.Point(34, 3);
            this.btnNone.Name = "btnNone";
            this.btnNone.Size = new System.Drawing.Size(25, 23);
            this.btnNone.TabIndex = 4;
            this.btnNone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNone.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNone.UseVisualStyleBackColor = true;
            this.btnNone.Click += new System.EventHandler(this.btnNone_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::DevFuel.Core.UI.Properties.Resources.edit_add;
            this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdd.Location = new System.Drawing.Point(3, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(25, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // miEdit
            // 
            this.miEdit.Image = global::DevFuel.Core.UI.Properties.Resources.kate;
            this.miEdit.Name = "miEdit";
            this.miEdit.Size = new System.Drawing.Size(152, 22);
            this.miEdit.Text = "Edit...";
            this.miEdit.Click += new System.EventHandler(this.miEdit_Click);
            // 
            // miDelete
            // 
            this.miDelete.Image = global::DevFuel.Core.UI.Properties.Resources.edit_remove;
            this.miDelete.Name = "miDelete";
            this.miDelete.Size = new System.Drawing.Size(152, 22);
            this.miDelete.Text = "Delete...";
            this.miDelete.Click += new System.EventHandler(this.miDelete_Click);
            // 
            // LookupPropertyDropDownControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lbxOptions);
            this.Controls.Add(this.btnNone);
            this.Controls.Add(this.btnAdd);
            this.Name = "LookupPropertyDropDownControl";
            this.Size = new System.Drawing.Size(222, 280);
            this.contextMnu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbxOptions;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnNone;
        private System.Windows.Forms.ContextMenuStrip contextMnu;
        private System.Windows.Forms.ToolStripMenuItem miEdit;
        private System.Windows.Forms.ToolStripMenuItem miDelete;
    }
}
