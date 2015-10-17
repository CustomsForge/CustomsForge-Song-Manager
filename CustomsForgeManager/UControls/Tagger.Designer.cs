namespace CustomsForgeManager.UControls
{
    partial class Tagger
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grpBoxTagPacks = new System.Windows.Forms.GroupBox();
            this.checkOverwriteTagsOnTaggedSongs = new System.Windows.Forms.CheckBox();
            this.checkAddTagsToFileName = new System.Windows.Forms.CheckBox();
            this.checkDeleteExtractedWhenDone = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbThemeLegend = new System.Windows.Forms.TextBox();
            this.btnSavePreview = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBoxPreview = new System.Windows.Forms.PictureBox();
            this.comboTagPacks = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnRemoveTags = new System.Windows.Forms.Button();
            this.btnPreviewSelected = new System.Windows.Forms.Button();
            this.btnTagDLCs = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.grpBoxTagPacks.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.grpBoxTagPacks, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.38461F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 41.02564F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 43.58974F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(990, 470);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // grpBoxTagPacks
            // 
            this.grpBoxTagPacks.Controls.Add(this.checkOverwriteTagsOnTaggedSongs);
            this.grpBoxTagPacks.Controls.Add(this.checkAddTagsToFileName);
            this.grpBoxTagPacks.Controls.Add(this.checkDeleteExtractedWhenDone);
            this.grpBoxTagPacks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBoxTagPacks.Location = new System.Drawing.Point(3, 3);
            this.grpBoxTagPacks.Name = "grpBoxTagPacks";
            this.grpBoxTagPacks.Size = new System.Drawing.Size(984, 66);
            this.grpBoxTagPacks.TabIndex = 11;
            this.grpBoxTagPacks.TabStop = false;
            this.grpBoxTagPacks.Text = "Settings";
            // 
            // checkOverwriteTagsOnTaggedSongs
            // 
            this.checkOverwriteTagsOnTaggedSongs.AutoSize = true;
            this.checkOverwriteTagsOnTaggedSongs.Location = new System.Drawing.Point(585, 28);
            this.checkOverwriteTagsOnTaggedSongs.Name = "checkOverwriteTagsOnTaggedSongs";
            this.checkOverwriteTagsOnTaggedSongs.Size = new System.Drawing.Size(176, 17);
            this.checkOverwriteTagsOnTaggedSongs.TabIndex = 2;
            this.checkOverwriteTagsOnTaggedSongs.Text = "Overwrite tags on tagged songs";
            this.checkOverwriteTagsOnTaggedSongs.UseVisualStyleBackColor = true;
            // 
            // checkAddTagsToFileName
            // 
            this.checkAddTagsToFileName.AutoSize = true;
            this.checkAddTagsToFileName.Location = new System.Drawing.Point(404, 28);
            this.checkAddTagsToFileName.Name = "checkAddTagsToFileName";
            this.checkAddTagsToFileName.Size = new System.Drawing.Size(125, 17);
            this.checkAddTagsToFileName.TabIndex = 1;
            this.checkAddTagsToFileName.Text = "Add tags to file name";
            this.checkAddTagsToFileName.UseVisualStyleBackColor = true;
            // 
            // checkDeleteExtractedWhenDone
            // 
            this.checkDeleteExtractedWhenDone.AutoSize = true;
            this.checkDeleteExtractedWhenDone.Location = new System.Drawing.Point(165, 27);
            this.checkDeleteExtractedWhenDone.Name = "checkDeleteExtractedWhenDone";
            this.checkDeleteExtractedWhenDone.Size = new System.Drawing.Size(194, 17);
            this.checkDeleteExtractedWhenDone.TabIndex = 0;
            this.checkDeleteExtractedWhenDone.Text = "Delete extracted folders when done";
            this.checkDeleteExtractedWhenDone.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbThemeLegend);
            this.groupBox2.Controls.Add(this.btnSavePreview);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.pictureBoxPreview);
            this.groupBox2.Controls.Add(this.comboTagPacks);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 267);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(984, 200);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tag Packs";
            // 
            // tbThemeLegend
            // 
            this.tbThemeLegend.Location = new System.Drawing.Point(230, 83);
            this.tbThemeLegend.Multiline = true;
            this.tbThemeLegend.Name = "tbThemeLegend";
            this.tbThemeLegend.Size = new System.Drawing.Size(194, 100);
            this.tbThemeLegend.TabIndex = 11;
            // 
            // btnSavePreview
            // 
            this.btnSavePreview.Location = new System.Drawing.Point(285, 55);
            this.btnSavePreview.Name = "btnSavePreview";
            this.btnSavePreview.Size = new System.Drawing.Size(75, 23);
            this.btnSavePreview.TabIndex = 10;
            this.btnSavePreview.Text = "Save preview";
            this.btnSavePreview.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(292, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Tag packs";
            // 
            // pictureBoxPreview
            // 
            this.pictureBoxPreview.Location = new System.Drawing.Point(538, 40);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new System.Drawing.Size(128, 128);
            this.pictureBoxPreview.TabIndex = 8;
            this.pictureBoxPreview.TabStop = false;
            // 
            // comboTagPacks
            // 
            this.comboTagPacks.FormattingEnabled = true;
            this.comboTagPacks.Location = new System.Drawing.Point(254, 29);
            this.comboTagPacks.Name = "comboTagPacks";
            this.comboTagPacks.Size = new System.Drawing.Size(142, 21);
            this.comboTagPacks.TabIndex = 7;
            this.comboTagPacks.SelectedIndexChanged += new System.EventHandler(this.comboTagPacks_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Controls.Add(this.btnRemoveTags);
            this.panel1.Controls.Add(this.btnPreviewSelected);
            this.panel1.Controls.Add(this.btnTagDLCs);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 75);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(984, 186);
            this.panel1.TabIndex = 13;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(442, 153);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(67, 13);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.Text = "Tagged: 0/0";
            // 
            // btnRemoveTags
            // 
            this.btnRemoveTags.Location = new System.Drawing.Point(362, 70);
            this.btnRemoveTags.Name = "btnRemoveTags";
            this.btnRemoveTags.Size = new System.Drawing.Size(254, 34);
            this.btnRemoveTags.TabIndex = 8;
            this.btnRemoveTags.Text = "Remove tags";
            this.btnRemoveTags.UseVisualStyleBackColor = true;
            this.btnRemoveTags.Click += new System.EventHandler(this.btnRemoveTags_Click);
            // 
            // btnPreviewSelected
            // 
            this.btnPreviewSelected.Location = new System.Drawing.Point(648, 70);
            this.btnPreviewSelected.Name = "btnPreviewSelected";
            this.btnPreviewSelected.Size = new System.Drawing.Size(224, 34);
            this.btnPreviewSelected.TabIndex = 7;
            this.btnPreviewSelected.Text = "Preview selected song in Sng. Manager";
            this.btnPreviewSelected.UseVisualStyleBackColor = true;
            this.btnPreviewSelected.Click += new System.EventHandler(this.btnPreviewSelected_Click);
            // 
            // btnTagDLCs
            // 
            this.btnTagDLCs.Location = new System.Drawing.Point(77, 70);
            this.btnTagDLCs.Name = "btnTagDLCs";
            this.btnTagDLCs.Size = new System.Drawing.Size(254, 34);
            this.btnTagDLCs.TabIndex = 6;
            this.btnTagDLCs.Text = "Tag CDLCs";
            this.btnTagDLCs.UseVisualStyleBackColor = true;
            this.btnTagDLCs.Click += new System.EventHandler(this.btnTagDLCs_Click);
            // 
            // Tagger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Tagger";
            this.Size = new System.Drawing.Size(990, 470);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.grpBoxTagPacks.ResumeLayout(false);
            this.grpBoxTagPacks.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox grpBoxTagPacks;
        private System.Windows.Forms.ComboBox comboTagPacks;
        private System.Windows.Forms.Button btnTagDLCs;
        private System.Windows.Forms.PictureBox pictureBoxPreview;
        private System.Windows.Forms.CheckBox checkAddTagsToFileName;
        private System.Windows.Forms.CheckBox checkDeleteExtractedWhenDone;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox tbThemeLegend;
        private System.Windows.Forms.Button btnSavePreview;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRemoveTags;
        private System.Windows.Forms.Button btnPreviewSelected;
        private System.Windows.Forms.CheckBox checkOverwriteTagsOnTaggedSongs;
        private System.Windows.Forms.Label lblStatus;

    }
}
