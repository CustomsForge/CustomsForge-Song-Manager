﻿namespace CustomsForgeSongManager.UControls
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Renamer));
            this.propertiesGroupBox = new System.Windows.Forms.GroupBox();
            this.lblWarning = new System.Windows.Forms.Label();
            this.dgvRenamer = new System.Windows.Forms.DataGridView();
            this.howToGroupBox = new System.Windows.Forms.GroupBox();
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkDeleteEmptyDir = new System.Windows.Forms.CheckBox();
            this.renameTemplateLabel = new System.Windows.Forms.Label();
            this.txtRenameTemplate = new System.Windows.Forms.TextBox();
            this.btnRenameAll = new System.Windows.Forms.Button();
            this.renamerPropertyDataSet = new System.Data.DataSet();
            this.chkRenameOnlySelected = new System.Windows.Forms.CheckBox();
            this.lbl2 = new System.Windows.Forms.Label();
            this.btnClearTemplate = new System.Windows.Forms.Button();
            this.chkTheMover = new System.Windows.Forms.CheckBox();
            this.chkRemoveSpaces = new System.Windows.Forms.CheckBox();
            this.btnPreview = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.propertiesGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRenamer)).BeginInit();
            this.howToGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.renamerPropertyDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // propertiesGroupBox
            // 
            this.propertiesGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertiesGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.propertiesGroupBox.Controls.Add(this.lblWarning);
            this.propertiesGroupBox.Controls.Add(this.dgvRenamer);
            this.propertiesGroupBox.Location = new System.Drawing.Point(431, 62);
            this.propertiesGroupBox.Name = "propertiesGroupBox";
            this.propertiesGroupBox.Size = new System.Drawing.Size(552, 417);
            this.propertiesGroupBox.TabIndex = 10;
            this.propertiesGroupBox.TabStop = false;
            this.propertiesGroupBox.Text = "Usuable Properties";
            // 
            // lblWarning
            // 
            this.lblWarning.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblWarning.AutoSize = true;
            this.lblWarning.BackColor = System.Drawing.Color.Khaki;
            this.lblWarning.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarning.ForeColor = System.Drawing.Color.Red;
            this.lblWarning.Location = new System.Drawing.Point(144, 319);
            this.lblWarning.Margin = new System.Windows.Forms.Padding(3);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Padding = new System.Windows.Forms.Padding(5);
            this.lblWarning.Size = new System.Drawing.Size(287, 87);
            this.lblWarning.TabIndex = 4;
            this.lblWarning.Text = "WARNING ... CFSM can ruin your \r\nCDLC collection if not used properly.\r\n\r\nPlease " +
                "make a back up of your CDLC until\r\nyou get the hang of how Renamer works.";
            this.lblWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvRenamer
            // 
            this.dgvRenamer.AllowUserToAddRows = false;
            this.dgvRenamer.AllowUserToDeleteRows = false;
            this.dgvRenamer.AllowUserToResizeColumns = false;
            this.dgvRenamer.AllowUserToResizeRows = false;
            this.dgvRenamer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRenamer.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvRenamer.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvRenamer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRenamer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRenamer.Location = new System.Drawing.Point(3, 16);
            this.dgvRenamer.MultiSelect = false;
            this.dgvRenamer.Name = "dgvRenamer";
            this.dgvRenamer.ReadOnly = true;
            this.dgvRenamer.RowHeadersVisible = false;
            this.dgvRenamer.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvRenamer.Size = new System.Drawing.Size(546, 398);
            this.dgvRenamer.TabIndex = 1;
            this.dgvRenamer.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRenamerProperties_CellDoubleClick);
            // 
            // howToGroupBox
            // 
            this.howToGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.howToGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.howToGroupBox.Controls.Add(this.lblHeader);
            this.howToGroupBox.Controls.Add(this.lblInstructions);
            this.howToGroupBox.Controls.Add(this.label1);
            this.howToGroupBox.Location = new System.Drawing.Point(6, 62);
            this.howToGroupBox.Name = "howToGroupBox";
            this.howToGroupBox.Size = new System.Drawing.Size(419, 417);
            this.howToGroupBox.TabIndex = 6;
            this.howToGroupBox.TabStop = false;
            this.howToGroupBox.Text = "How To Use:";
            // 
            // lblHeader
            // 
            this.lblHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(10, 26);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(346, 112);
            this.lblHeader.TabIndex = 2;
            this.lblHeader.Text = resources.GetString("lblHeader.Text");
            // 
            // lblInstructions
            // 
            this.lblInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInstructions.AutoSize = true;
            this.lblInstructions.Location = new System.Drawing.Point(10, 147);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblInstructions.Size = new System.Drawing.Size(376, 143);
            this.lblInstructions.TabIndex = 1;
            this.lblInstructions.Text = resources.GetString("lblInstructions.Text");
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
            this.chkDeleteEmptyDir.Location = new System.Drawing.Point(415, 37);
            this.chkDeleteEmptyDir.Name = "chkDeleteEmptyDir";
            this.chkDeleteEmptyDir.Size = new System.Drawing.Size(209, 17);
            this.chkDeleteEmptyDir.TabIndex = 15;
            this.chkDeleteEmptyDir.Text = "Delete empty directories after renaming";
            this.chkDeleteEmptyDir.UseVisualStyleBackColor = true;
            // 
            // renameTemplateLabel
            // 
            this.renameTemplateLabel.AutoSize = true;
            this.renameTemplateLabel.Location = new System.Drawing.Point(3, 12);
            this.renameTemplateLabel.Name = "renameTemplateLabel";
            this.renameTemplateLabel.Size = new System.Drawing.Size(105, 13);
            this.renameTemplateLabel.TabIndex = 5;
            this.renameTemplateLabel.Text = "Renaming Template:";
            // 
            // txtRenameTemplate
            // 
            this.txtRenameTemplate.Location = new System.Drawing.Point(114, 8);
            this.txtRenameTemplate.Multiline = true;
            this.txtRenameTemplate.Name = "txtRenameTemplate";
            this.txtRenameTemplate.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRenameTemplate.Size = new System.Drawing.Size(372, 20);
            this.txtRenameTemplate.TabIndex = 0;
            this.txtRenameTemplate.Text = "<artist>_<title>_<version>";
            this.txtRenameTemplate.TextChanged += new System.EventHandler(this.txtRenameTemplate_TextChanged);
            // 
            // btnRenameAll
            // 
            this.btnRenameAll.Image = global::CustomsForgeSongManager.Properties.Resources.rename;
            this.btnRenameAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRenameAll.Location = new System.Drawing.Point(498, 4);
            this.btnRenameAll.Name = "btnRenameAll";
            this.btnRenameAll.Size = new System.Drawing.Size(122, 26);
            this.btnRenameAll.TabIndex = 11;
            this.btnRenameAll.Text = "Rename All Songs";
            this.btnRenameAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.btnRenameAll, resources.GetString("btnRenameAll.ToolTip"));
            this.btnRenameAll.UseVisualStyleBackColor = true;
            this.btnRenameAll.Click += new System.EventHandler(this.btnRenameAll_Click);
            // 
            // renamerPropertyDataSet
            // 
            this.renamerPropertyDataSet.DataSetName = "NewDataSet";
            // 
            // chkRenameOnlySelected
            // 
            this.chkRenameOnlySelected.AutoSize = true;
            this.chkRenameOnlySelected.Location = new System.Drawing.Point(658, 9);
            this.chkRenameOnlySelected.Name = "chkRenameOnlySelected";
            this.chkRenameOnlySelected.Size = new System.Drawing.Size(303, 17);
            this.chkRenameOnlySelected.TabIndex = 16;
            this.chkRenameOnlySelected.Text = "Rename only the songs that are selected in Song Manager";
            this.chkRenameOnlySelected.UseVisualStyleBackColor = true;
            // 
            // lbl2
            // 
            this.lbl2.AutoSize = true;
            this.lbl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2.Location = new System.Drawing.Point(626, 8);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(22, 16);
            this.lbl2.TabIndex = 17;
            this.lbl2.Text = "or";
            // 
            // btnClearTemplate
            // 
            this.btnClearTemplate.AutoSize = true;
            this.btnClearTemplate.Location = new System.Drawing.Point(6, 33);
            this.btnClearTemplate.Name = "btnClearTemplate";
            this.btnClearTemplate.Size = new System.Drawing.Size(96, 23);
            this.btnClearTemplate.TabIndex = 18;
            this.btnClearTemplate.Text = "Clear Template";
            this.btnClearTemplate.UseVisualStyleBackColor = true;
            this.btnClearTemplate.Click += new System.EventHandler(this.btnClearTemplate_Click);
            // 
            // chkTheMover
            // 
            this.chkTheMover.AutoSize = true;
            this.chkTheMover.Location = new System.Drawing.Point(658, 37);
            this.chkTheMover.Name = "chkTheMover";
            this.chkTheMover.Size = new System.Drawing.Size(241, 17);
            this.chkTheMover.TabIndex = 19;
            this.chkTheMover.Text = "\'The\' Mover e.g., The Beatles -> Beatles, The\r\n";
            this.chkTheMover.UseVisualStyleBackColor = true;
            // 
            // chkRemoveSpaces
            // 
            this.chkRemoveSpaces.AutoSize = true;
            this.chkRemoveSpaces.Location = new System.Drawing.Point(212, 37);
            this.chkRemoveSpaces.Name = "chkRemoveSpaces";
            this.chkRemoveSpaces.Size = new System.Drawing.Size(183, 17);
            this.chkRemoveSpaces.TabIndex = 20;
            this.chkRemoveSpaces.Text = "Remove spaces from new names\r\n";
            this.chkRemoveSpaces.UseVisualStyleBackColor = true;
            // 
            // btnPreview
            // 
            this.btnPreview.AutoSize = true;
            this.btnPreview.Location = new System.Drawing.Point(113, 33);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 23);
            this.btnPreview.TabIndex = 23;
            this.btnPreview.Text = "Preview";
            this.toolTip.SetToolTip(this.btnPreview, "The \'Show Log\' panel must be open to view previews.");
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 200;
            this.toolTip.AutoPopDelay = 12000;
            this.toolTip.InitialDelay = 200;
            this.toolTip.IsBalloon = true;
            this.toolTip.ReshowDelay = 100;
            // 
            // Renamer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.chkRemoveSpaces);
            this.Controls.Add(this.chkTheMover);
            this.Controls.Add(this.btnClearTemplate);
            this.Controls.Add(this.chkDeleteEmptyDir);
            this.Controls.Add(this.lbl2);
            this.Controls.Add(this.chkRenameOnlySelected);
            this.Controls.Add(this.renameTemplateLabel);
            this.Controls.Add(this.txtRenameTemplate);
            this.Controls.Add(this.btnRenameAll);
            this.Controls.Add(this.propertiesGroupBox);
            this.Controls.Add(this.howToGroupBox);
            this.Name = "Renamer";
            this.Size = new System.Drawing.Size(990, 490);
            this.propertiesGroupBox.ResumeLayout(false);
            this.propertiesGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRenamer)).EndInit();
            this.howToGroupBox.ResumeLayout(false);
            this.howToGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.renamerPropertyDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox propertiesGroupBox;
        private System.Windows.Forms.DataGridView dgvRenamer;
        private System.Windows.Forms.GroupBox howToGroupBox;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkDeleteEmptyDir;
        private System.Windows.Forms.Label renameTemplateLabel;
        private System.Windows.Forms.TextBox txtRenameTemplate;
        private System.Windows.Forms.Button btnRenameAll;
        private System.Data.DataSet renamerPropertyDataSet;
        private System.Windows.Forms.CheckBox chkRenameOnlySelected;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Button btnClearTemplate;
        private System.Windows.Forms.CheckBox chkTheMover;
        private System.Windows.Forms.CheckBox chkRemoveSpaces;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
