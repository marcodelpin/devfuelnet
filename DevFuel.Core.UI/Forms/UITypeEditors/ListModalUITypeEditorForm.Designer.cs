namespace DevFuel.Core.UI.Drawing.Design
{
    partial class ListModalUITypeEditorForm
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
            this.tscMain = new System.Windows.Forms.ToolStripContainer();
            this.splitter = new System.Windows.Forms.SplitContainer();
            this.lbxItems = new System.Windows.Forms.ListBox();
            this.gridSelection = new System.Windows.Forms.PropertyGrid();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripSplitButton();
            this.btnRemove = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnMoveUp = new System.Windows.Forms.ToolStripButton();
            this.btnMoveDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tscMain.ContentPanel.SuspendLayout();
            this.tscMain.TopToolStripPanel.SuspendLayout();
            this.tscMain.SuspendLayout();
            this.splitter.Panel1.SuspendLayout();
            this.splitter.Panel2.SuspendLayout();
            this.splitter.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tscMain
            // 
            this.tscMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // tscMain.ContentPanel
            // 
            this.tscMain.ContentPanel.Controls.Add(this.splitter);
            this.tscMain.ContentPanel.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
            this.tscMain.ContentPanel.Size = new System.Drawing.Size(592, 291);
            this.tscMain.Location = new System.Drawing.Point(0, 0);
            this.tscMain.Name = "tscMain";
            this.tscMain.Size = new System.Drawing.Size(592, 316);
            this.tscMain.TabIndex = 13;
            this.tscMain.Text = "Actions";
            // 
            // tscMain.TopToolStripPanel
            // 
            this.tscMain.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // splitter
            // 
            this.splitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitter.Location = new System.Drawing.Point(10, 10);
            this.splitter.Name = "splitter";
            // 
            // splitter.Panel1
            // 
            this.splitter.Panel1.Controls.Add(this.lbxItems);
            // 
            // splitter.Panel2
            // 
            this.splitter.Panel2.Controls.Add(this.gridSelection);
            this.splitter.Size = new System.Drawing.Size(572, 281);
            this.splitter.SplitterDistance = 275;
            this.splitter.SplitterWidth = 10;
            this.splitter.TabIndex = 10;
            // 
            // lbxItems
            // 
            this.lbxItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxItems.FormattingEnabled = true;
            this.lbxItems.Location = new System.Drawing.Point(0, 0);
            this.lbxItems.Name = "lbxItems";
            this.lbxItems.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbxItems.Size = new System.Drawing.Size(275, 277);
            this.lbxItems.TabIndex = 13;
            this.lbxItems.SelectedIndexChanged += new System.EventHandler(this.lbxItems_SelectedIndexChanged);
            this.lbxItems.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.lbxItems_Format);
            // 
            // gridSelection
            // 
            this.gridSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSelection.Location = new System.Drawing.Point(0, 0);
            this.gridSelection.Name = "gridSelection";
            this.gridSelection.Size = new System.Drawing.Size(287, 281);
            this.gridSelection.TabIndex = 3;
            this.gridSelection.ToolbarVisible = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnRemove,
            this.toolStripSeparator1,
            this.btnMoveUp,
            this.btnMoveDown,
            this.toolStripSeparator2});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(156, 25);
            this.toolStrip1.TabIndex = 11;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAdd.Image = global::DevFuel.Core.UI.Properties.Resources.edit_add;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(32, 22);
            this.btnAdd.Text = "Add Item";
            this.btnAdd.ButtonClick += new System.EventHandler(this.btnAdd_ButtonClick);
            // 
            // btnRemove
            // 
            this.btnRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRemove.Image = global::DevFuel.Core.UI.Properties.Resources.edit_remove;
            this.btnRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(23, 22);
            this.btnRemove.Text = "Remove Item";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveUp.Image = global::DevFuel.Core.UI.Properties.Resources.up;
            this.btnMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(23, 22);
            this.btnMoveUp.Text = "Move Item Up";
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveDown.Image = global::DevFuel.Core.UI.Properties.Resources.down;
            this.btnMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(23, 22);
            this.btnMoveDown.Text = "Move Item Down";
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(416, 331);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.Location = new System.Drawing.Point(503, 331);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(79, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ListModalUITypeEditorForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(592, 366);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tscMain);
            this.Name = "ListModalUITypeEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "{0} List Editor";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ListEditorForm_Load);
            this.tscMain.ContentPanel.ResumeLayout(false);
            this.tscMain.TopToolStripPanel.ResumeLayout(false);
            this.tscMain.TopToolStripPanel.PerformLayout();
            this.tscMain.ResumeLayout(false);
            this.tscMain.PerformLayout();
            this.splitter.Panel1.ResumeLayout(false);
            this.splitter.Panel2.ResumeLayout(false);
            this.splitter.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// Main Tool Strip Container
        /// </summary>
        protected System.Windows.Forms.ToolStripContainer tscMain;
        /// <summary>
        /// OK Button
        /// </summary>
        protected System.Windows.Forms.Button btnOK;
        /// <summary>
        /// Cancel Button
        /// </summary>
        protected System.Windows.Forms.Button btnCancel;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.SplitContainer splitter;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.ListBox lbxItems;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.PropertyGrid gridSelection;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.ToolStrip toolStrip1;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.ToolStripSplitButton btnAdd;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.ToolStripButton btnRemove;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.ToolStripButton btnMoveUp;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.ToolStripButton btnMoveDown;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}