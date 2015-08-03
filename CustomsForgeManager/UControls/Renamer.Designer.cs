namespace CustomsForgeManager.UControls
{
    partial class Renamer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Renamer));
            this.propertiesGroupBox = new System.Windows.Forms.GroupBox();
            this.renamerPropertyDataGridView = new System.Windows.Forms.DataGridView();
            this.howToGroupBox = new System.Windows.Forms.GroupBox();
            this.instructionsLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkDeleteEmptyDir = new System.Windows.Forms.CheckBox();
            this.slashLabel = new System.Windows.Forms.Label();
            this.renameTemplateLabel = new System.Windows.Forms.Label();
            this.txtRenameTemplate = new System.Windows.Forms.TextBox();
            this.btnRenameAll = new System.Windows.Forms.Button();
            this.renamerPropertyDataSet = new System.Data.DataSet();
            this.propertiesGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.renamerPropertyDataGridView)).BeginInit();
            this.howToGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.renamerPropertyDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // propertiesGroupBox
            // 
            this.propertiesGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertiesGroupBox.Controls.Add(this.renamerPropertyDataGridView);
            this.propertiesGroupBox.Location = new System.Drawing.Point(431, 57);
            this.propertiesGroupBox.Name = "propertiesGroupBox";
            this.propertiesGroupBox.Size = new System.Drawing.Size(557, 433);
            this.propertiesGroupBox.TabIndex = 10;
            this.propertiesGroupBox.TabStop = false;
            this.propertiesGroupBox.Text = "Usuable Properties";
            // 
            // renamerPropertyDataGridView
            // 
            this.renamerPropertyDataGridView.AllowUserToAddRows = false;
            this.renamerPropertyDataGridView.AllowUserToDeleteRows = false;
            this.renamerPropertyDataGridView.AllowUserToResizeColumns = false;
            this.renamerPropertyDataGridView.AllowUserToResizeRows = false;
            this.renamerPropertyDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.renamerPropertyDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.renamerPropertyDataGridView.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.renamerPropertyDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.renamerPropertyDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renamerPropertyDataGridView.Location = new System.Drawing.Point(3, 16);
            this.renamerPropertyDataGridView.MultiSelect = false;
            this.renamerPropertyDataGridView.Name = "renamerPropertyDataGridView";
            this.renamerPropertyDataGridView.ReadOnly = true;
            this.renamerPropertyDataGridView.RowHeadersVisible = false;
            this.renamerPropertyDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.renamerPropertyDataGridView.Size = new System.Drawing.Size(551, 414);
            this.renamerPropertyDataGridView.TabIndex = 2;
            // 
            // howToGroupBox
            // 
            this.howToGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.howToGroupBox.Controls.Add(this.instructionsLabel);
            this.howToGroupBox.Controls.Add(this.label1);
            this.howToGroupBox.Location = new System.Drawing.Point(0, 57);
            this.howToGroupBox.Name = "howToGroupBox";
            this.howToGroupBox.Size = new System.Drawing.Size(434, 433);
            this.howToGroupBox.TabIndex = 9;
            this.howToGroupBox.TabStop = false;
            this.howToGroupBox.Text = "How To Use:";
            // 
            // instructionsLabel
            // 
            this.instructionsLabel.AutoSize = true;
            this.instructionsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.instructionsLabel.Location = new System.Drawing.Point(3, 16);
            this.instructionsLabel.Name = "instructionsLabel";
            this.instructionsLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.instructionsLabel.Size = new System.Drawing.Size(335, 247);
            this.instructionsLabel.TabIndex = 1;
            this.instructionsLabel.Text = resources.GetString("instructionsLabel.Text");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 0;
            // 
            // chkDeleteEmptyDir
            // 
            this.chkDeleteEmptyDir.AutoSize = true;
            this.chkDeleteEmptyDir.Location = new System.Drawing.Point(149, 37);
            this.chkDeleteEmptyDir.Name = "chkDeleteEmptyDir";
            this.chkDeleteEmptyDir.Size = new System.Drawing.Size(209, 17);
            this.chkDeleteEmptyDir.TabIndex = 15;
            this.chkDeleteEmptyDir.Text = "Delete Empty Directories after Rename";
            this.chkDeleteEmptyDir.UseVisualStyleBackColor = true;
            // 
            // slashLabel
            // 
            this.slashLabel.AutoSize = true;
            this.slashLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.slashLabel.Location = new System.Drawing.Point(106, 9);
            this.slashLabel.Name = "slashLabel";
            this.slashLabel.Size = new System.Drawing.Size(37, 20);
            this.slashLabel.TabIndex = 14;
            this.slashLabel.Text = "dlc/";
            // 
            // renameTemplateLabel
            // 
            this.renameTemplateLabel.AutoSize = true;
            this.renameTemplateLabel.Location = new System.Drawing.Point(3, 11);
            this.renameTemplateLabel.Name = "renameTemplateLabel";
            this.renameTemplateLabel.Size = new System.Drawing.Size(97, 13);
            this.renameTemplateLabel.TabIndex = 13;
            this.renameTemplateLabel.Text = "Rename Template:";
            // 
            // txtRenameTemplate
            // 
            this.txtRenameTemplate.Location = new System.Drawing.Point(149, 11);
            this.txtRenameTemplate.Name = "txtRenameTemplate";
            this.txtRenameTemplate.Size = new System.Drawing.Size(379, 20);
            this.txtRenameTemplate.TabIndex = 12;
            this.txtRenameTemplate.Text = "<title>_<artist>";
            // 
            // btnRenameAll
            // 
            this.btnRenameAll.Image = global::CustomsForgeManager.Properties.Resources.rename;
            this.btnRenameAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRenameAll.Location = new System.Drawing.Point(534, 7);
            this.btnRenameAll.Name = "btnRenameAll";
            this.btnRenameAll.Size = new System.Drawing.Size(85, 30);
            this.btnRenameAll.TabIndex = 11;
            this.btnRenameAll.Text = "Rename All";
            this.btnRenameAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRenameAll.UseVisualStyleBackColor = true;
            this.btnRenameAll.Click += new System.EventHandler(this.btnRenameAll_Click);
            // 
            // renamerPropertyDataSet
            // 
            this.renamerPropertyDataSet.DataSetName = "NewDataSet";
            // 
            // Renamer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkDeleteEmptyDir);
            this.Controls.Add(this.slashLabel);
            this.Controls.Add(this.renameTemplateLabel);
            this.Controls.Add(this.txtRenameTemplate);
            this.Controls.Add(this.btnRenameAll);
            this.Controls.Add(this.propertiesGroupBox);
            this.Controls.Add(this.howToGroupBox);
            this.Name = "Renamer";
            this.Size = new System.Drawing.Size(990, 490);
            this.propertiesGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.renamerPropertyDataGridView)).EndInit();
            this.howToGroupBox.ResumeLayout(false);
            this.howToGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.renamerPropertyDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox propertiesGroupBox;
        private System.Windows.Forms.DataGridView renamerPropertyDataGridView;
        private System.Windows.Forms.GroupBox howToGroupBox;
        private System.Windows.Forms.Label instructionsLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkDeleteEmptyDir;
        private System.Windows.Forms.Label slashLabel;
        private System.Windows.Forms.Label renameTemplateLabel;
        private System.Windows.Forms.TextBox txtRenameTemplate;
        private System.Windows.Forms.Button btnRenameAll;
        private CustomsForgeManagerLib.CustomControls.AbortableBackgroundWorker bWorker;
        private System.Data.DataSet renamerPropertyDataSet;
    }
}
