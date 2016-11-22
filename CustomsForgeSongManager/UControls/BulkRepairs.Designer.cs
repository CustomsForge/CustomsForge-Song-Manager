namespace CustomsForgeSongManager.UControls
{
    partial class BulkRepairs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BulkRepairs));
            this.tlpRepairs = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.rbSkipRepaired = new System.Windows.Forms.RadioButton();
            this.rbAddDD = new System.Windows.Forms.RadioButton();
            this.gbMaxPlayable = new System.Windows.Forms.GroupBox();
            this.chkIgnoreLimit = new System.Windows.Forms.CheckBox();
            this.chkRemoveMetronome = new System.Windows.Forms.CheckBox();
            this.chkRemoveGuitar = new System.Windows.Forms.CheckBox();
            this.chkRemoveBonus = new System.Windows.Forms.CheckBox();
            this.chkRemoveNdd = new System.Windows.Forms.CheckBox();
            this.chkRemoveBass = new System.Windows.Forms.CheckBox();
            this.rbRepairMaxFive = new System.Windows.Forms.RadioButton();
            this.btnRestoreMax = new System.Windows.Forms.Button();
            this.gbMastery = new System.Windows.Forms.GroupBox();
            this.rbRepairMastery = new System.Windows.Forms.RadioButton();
            this.chkRepairOrg = new System.Windows.Forms.CheckBox();
            this.chkPreserve = new System.Windows.Forms.CheckBox();
            this.btnDeleteCorruptSongs = new System.Windows.Forms.Button();
            this.btnArchiveCorruptSongs = new System.Windows.Forms.Button();
            this.btnRestoreOrg = new System.Windows.Forms.Button();
            this.btnRepairSongs = new System.Windows.Forms.Button();
            this.pnlMiddle = new System.Windows.Forms.Panel();
            this.gbRepairStatus = new System.Windows.Forms.GroupBox();
            this.dgvRepair = new DataGridViewTools.SubclassedDataGridView();
            this.colFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnViewErrorLog = new System.Windows.Forms.Button();
            this.btnCleanDlcFolder = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnRestoreCorrupt = new System.Windows.Forms.Button();
            this.tlpRepairs.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.gbMaxPlayable.SuspendLayout();
            this.gbMastery.SuspendLayout();
            this.pnlMiddle.SuspendLayout();
            this.gbRepairStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRepair)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRepairs
            // 
            this.tlpRepairs.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tlpRepairs.ColumnCount = 1;
            this.tlpRepairs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRepairs.Controls.Add(this.pnlTop, 0, 0);
            this.tlpRepairs.Controls.Add(this.pnlMiddle, 0, 1);
            this.tlpRepairs.Controls.Add(this.pnlBottom, 0, 2);
            this.tlpRepairs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRepairs.Location = new System.Drawing.Point(0, 0);
            this.tlpRepairs.Name = "tlpRepairs";
            this.tlpRepairs.RowCount = 3;
            this.tlpRepairs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 173F));
            this.tlpRepairs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRepairs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tlpRepairs.Size = new System.Drawing.Size(899, 490);
            this.tlpRepairs.TabIndex = 2;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.btnHelp);
            this.pnlTop.Controls.Add(this.btnRestoreCorrupt);
            this.pnlTop.Controls.Add(this.rbSkipRepaired);
            this.pnlTop.Controls.Add(this.btnCleanDlcFolder);
            this.pnlTop.Controls.Add(this.rbAddDD);
            this.pnlTop.Controls.Add(this.gbMaxPlayable);
            this.pnlTop.Controls.Add(this.btnRestoreMax);
            this.pnlTop.Controls.Add(this.gbMastery);
            this.pnlTop.Controls.Add(this.btnDeleteCorruptSongs);
            this.pnlTop.Controls.Add(this.btnArchiveCorruptSongs);
            this.pnlTop.Controls.Add(this.btnRestoreOrg);
            this.pnlTop.Controls.Add(this.btnRepairSongs);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTop.Location = new System.Drawing.Point(2, 2);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(895, 173);
            this.pnlTop.TabIndex = 4;
            this.toolTip.SetToolTip(this.pnlTop, "If needed, the repair options may be\r\nrun again by using the \'Run Repair Using\r\n(" +
                    ".org) Files\' checkbox option or by first\r\npressing the \'Restore (.org) Backups\' " +
                    "button.");
            // 
            // rbSkipRepaired
            // 
            this.rbSkipRepaired.AutoCheck = false;
            this.rbSkipRepaired.AutoSize = true;
            this.rbSkipRepaired.Checked = true;
            this.rbSkipRepaired.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbSkipRepaired.ForeColor = System.Drawing.Color.RoyalBlue;
            this.rbSkipRepaired.Location = new System.Drawing.Point(385, 15);
            this.rbSkipRepaired.Name = "rbSkipRepaired";
            this.rbSkipRepaired.Size = new System.Drawing.Size(174, 17);
            this.rbSkipRepaired.TabIndex = 13;
            this.rbSkipRepaired.TabStop = true;
            this.rbSkipRepaired.Text = "Skip Previously Repaired CDLC";
            this.toolTip.SetToolTip(this.rbSkipRepaired, "If selected, skips previously repaired CDCL.\r\n");
            this.rbSkipRepaired.UseVisualStyleBackColor = true;
            this.rbSkipRepaired.Click += new System.EventHandler(this.rbSkipRepaired_Click);
            // 
            // rbAddDD
            // 
            this.rbAddDD.AutoCheck = false;
            this.rbAddDD.AutoSize = true;
            this.rbAddDD.Checked = true;
            this.rbAddDD.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbAddDD.ForeColor = System.Drawing.Color.RoyalBlue;
            this.rbAddDD.Location = new System.Drawing.Point(586, 15);
            this.rbAddDD.Name = "rbAddDD";
            this.rbAddDD.Size = new System.Drawing.Size(170, 17);
            this.rbAddDD.TabIndex = 12;
            this.rbAddDD.TabStop = true;
            this.rbAddDD.Text = "Add DD (if not already present)";
            this.toolTip.SetToolTip(this.rbAddDD, resources.GetString("rbAddDD.ToolTip"));
            this.rbAddDD.UseVisualStyleBackColor = true;
            this.rbAddDD.Click += new System.EventHandler(this.rbAddDD_Click);
            // 
            // gbMaxPlayable
            // 
            this.gbMaxPlayable.Controls.Add(this.chkIgnoreLimit);
            this.gbMaxPlayable.Controls.Add(this.chkRemoveMetronome);
            this.gbMaxPlayable.Controls.Add(this.chkRemoveGuitar);
            this.gbMaxPlayable.Controls.Add(this.chkRemoveBonus);
            this.gbMaxPlayable.Controls.Add(this.chkRemoveNdd);
            this.gbMaxPlayable.Controls.Add(this.chkRemoveBass);
            this.gbMaxPlayable.Controls.Add(this.rbRepairMaxFive);
            this.gbMaxPlayable.Location = new System.Drawing.Point(576, 38);
            this.gbMaxPlayable.Name = "gbMaxPlayable";
            this.gbMaxPlayable.Size = new System.Drawing.Size(243, 109);
            this.gbMaxPlayable.TabIndex = 10;
            this.gbMaxPlayable.TabStop = false;
            this.toolTip.SetToolTip(this.gbMaxPlayable, "WARNING:\r\nUse removal criteria sparingly so that \r\nyour CDLC are not rendered use" +
                    "less.");
            // 
            // chkIgnoreLimit
            // 
            this.chkIgnoreLimit.AutoSize = true;
            this.chkIgnoreLimit.Enabled = false;
            this.chkIgnoreLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIgnoreLimit.ForeColor = System.Drawing.Color.IndianRed;
            this.chkIgnoreLimit.Location = new System.Drawing.Point(114, 84);
            this.chkIgnoreLimit.Name = "chkIgnoreLimit";
            this.chkIgnoreLimit.Size = new System.Drawing.Size(105, 17);
            this.chkIgnoreLimit.TabIndex = 16;
            this.chkIgnoreLimit.Text = "Ignore Stop Limit";
            this.toolTip.SetToolTip(this.chkIgnoreLimit, resources.GetString("chkIgnoreLimit.ToolTip"));
            this.chkIgnoreLimit.UseVisualStyleBackColor = true;
            // 
            // chkRemoveMetronome
            // 
            this.chkRemoveMetronome.AutoSize = true;
            this.chkRemoveMetronome.Enabled = false;
            this.chkRemoveMetronome.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRemoveMetronome.Location = new System.Drawing.Point(114, 61);
            this.chkRemoveMetronome.Name = "chkRemoveMetronome";
            this.chkRemoveMetronome.Size = new System.Drawing.Size(122, 17);
            this.chkRemoveMetronome.TabIndex = 15;
            this.chkRemoveMetronome.Text = "Remove Metronome";
            this.toolTip.SetToolTip(this.chkRemoveMetronome, "If checked removes Metronome\r\narrangements from CDLC files.");
            this.chkRemoveMetronome.UseVisualStyleBackColor = true;
            // 
            // chkRemoveGuitar
            // 
            this.chkRemoveGuitar.AutoSize = true;
            this.chkRemoveGuitar.Enabled = false;
            this.chkRemoveGuitar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRemoveGuitar.Location = new System.Drawing.Point(11, 84);
            this.chkRemoveGuitar.Name = "chkRemoveGuitar";
            this.chkRemoveGuitar.Size = new System.Drawing.Size(97, 17);
            this.chkRemoveGuitar.TabIndex = 13;
            this.chkRemoveGuitar.Text = "Remove Guitar";
            this.toolTip.SetToolTip(this.chkRemoveGuitar, "If checked removes Guitar\r\narrangements from CDLC files.");
            this.chkRemoveGuitar.UseVisualStyleBackColor = true;
            // 
            // chkRemoveBonus
            // 
            this.chkRemoveBonus.AutoSize = true;
            this.chkRemoveBonus.Enabled = false;
            this.chkRemoveBonus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRemoveBonus.Location = new System.Drawing.Point(114, 37);
            this.chkRemoveBonus.Name = "chkRemoveBonus";
            this.chkRemoveBonus.Size = new System.Drawing.Size(99, 17);
            this.chkRemoveBonus.TabIndex = 14;
            this.chkRemoveBonus.Text = "Remove Bonus";
            this.toolTip.SetToolTip(this.chkRemoveBonus, "If checked removes Bonus\r\narrangements from CDLC files.\r\n\r\n\'Remove Bonus\' + \'Add " +
                    "DD\' = LaceyB Repair");
            this.chkRemoveBonus.UseVisualStyleBackColor = true;
            // 
            // chkRemoveNdd
            // 
            this.chkRemoveNdd.AutoSize = true;
            this.chkRemoveNdd.Enabled = false;
            this.chkRemoveNdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRemoveNdd.Location = new System.Drawing.Point(10, 37);
            this.chkRemoveNdd.Name = "chkRemoveNdd";
            this.chkRemoveNdd.Size = new System.Drawing.Size(93, 17);
            this.chkRemoveNdd.TabIndex = 10;
            this.chkRemoveNdd.Text = "Remove NDD";
            this.toolTip.SetToolTip(this.chkRemoveNdd, "If checked removes NDD (No Dynamic Difficulty)\r\narrangements from the CDLC file.\r" +
                    "\n\r\nMay not be combined with \'Add DD\' radio button.");
            this.chkRemoveNdd.UseVisualStyleBackColor = true;
            // 
            // chkRemoveBass
            // 
            this.chkRemoveBass.AutoSize = true;
            this.chkRemoveBass.Enabled = false;
            this.chkRemoveBass.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRemoveBass.Location = new System.Drawing.Point(11, 61);
            this.chkRemoveBass.Name = "chkRemoveBass";
            this.chkRemoveBass.Size = new System.Drawing.Size(92, 17);
            this.chkRemoveBass.TabIndex = 11;
            this.chkRemoveBass.Text = "Remove Bass";
            this.toolTip.SetToolTip(this.chkRemoveBass, "If checked removes Bass\r\narrangements from CDLC files.");
            this.chkRemoveBass.UseVisualStyleBackColor = true;
            // 
            // rbRepairMaxFive
            // 
            this.rbRepairMaxFive.AutoCheck = false;
            this.rbRepairMaxFive.AutoSize = true;
            this.rbRepairMaxFive.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbRepairMaxFive.ForeColor = System.Drawing.Color.RoyalBlue;
            this.rbRepairMaxFive.Location = new System.Drawing.Point(10, 13);
            this.rbRepairMaxFive.Name = "rbRepairMaxFive";
            this.rbRepairMaxFive.Size = new System.Drawing.Size(214, 17);
            this.rbRepairMaxFive.TabIndex = 12;
            this.rbRepairMaxFive.Text = "Repair Maximum Playable Arrangements";
            this.toolTip.SetToolTip(this.rbRepairMaxFive, "A CDLC must have a type of arrangement\r\nfor it to be removed.  Arrangement types\r" +
                    "\nare not added by this repair feature, only\r\nremoved by it.");
            this.rbRepairMaxFive.UseVisualStyleBackColor = true;
            this.rbRepairMaxFive.Click += new System.EventHandler(this.rbRepairMaxFive_Click);
            // 
            // btnRestoreMax
            // 
            this.btnRestoreMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRestoreMax.Image = ((System.Drawing.Image)(resources.GetObject("btnRestoreMax.Image")));
            this.btnRestoreMax.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRestoreMax.Location = new System.Drawing.Point(20, 93);
            this.btnRestoreMax.Name = "btnRestoreMax";
            this.btnRestoreMax.Padding = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.btnRestoreMax.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnRestoreMax.Size = new System.Drawing.Size(160, 32);
            this.btnRestoreMax.TabIndex = 11;
            this.btnRestoreMax.Text = "   Restore (.max) Backups";
            this.toolTip.SetToolTip(this.btnRestoreMax, resources.GetString("btnRestoreMax.ToolTip"));
            this.btnRestoreMax.UseVisualStyleBackColor = true;
            this.btnRestoreMax.Click += new System.EventHandler(this.btnRestoreMax_Click);
            // 
            // gbMastery
            // 
            this.gbMastery.Controls.Add(this.rbRepairMastery);
            this.gbMastery.Controls.Add(this.chkRepairOrg);
            this.gbMastery.Controls.Add(this.chkPreserve);
            this.gbMastery.Location = new System.Drawing.Point(385, 38);
            this.gbMastery.Name = "gbMastery";
            this.gbMastery.Size = new System.Drawing.Size(174, 86);
            this.gbMastery.TabIndex = 10;
            this.gbMastery.TabStop = false;
            // 
            // rbRepairMastery
            // 
            this.rbRepairMastery.AutoCheck = false;
            this.rbRepairMastery.AutoSize = true;
            this.rbRepairMastery.Checked = true;
            this.rbRepairMastery.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbRepairMastery.ForeColor = System.Drawing.Color.RoyalBlue;
            this.rbRepairMastery.Location = new System.Drawing.Point(6, 13);
            this.rbRepairMastery.Name = "rbRepairMastery";
            this.rbRepairMastery.Size = new System.Drawing.Size(147, 17);
            this.rbRepairMastery.TabIndex = 9;
            this.rbRepairMastery.TabStop = true;
            this.rbRepairMastery.Text = "Repair 100% Mastery Bug";
            this.rbRepairMastery.UseVisualStyleBackColor = true;
            this.rbRepairMastery.Click += new System.EventHandler(this.rbRepairMastery_Click);
            // 
            // chkRepairOrg
            // 
            this.chkRepairOrg.AutoSize = true;
            this.chkRepairOrg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRepairOrg.Location = new System.Drawing.Point(6, 61);
            this.chkRepairOrg.Name = "chkRepairOrg";
            this.chkRepairOrg.Size = new System.Drawing.Size(161, 17);
            this.chkRepairOrg.TabIndex = 7;
            this.chkRepairOrg.Text = "Run Repair Using (.org) Files";
            this.toolTip.SetToolTip(this.chkRepairOrg, resources.GetString("chkRepairOrg.ToolTip"));
            this.chkRepairOrg.UseVisualStyleBackColor = true;
            // 
            // chkPreserve
            // 
            this.chkPreserve.AutoSize = true;
            this.chkPreserve.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPreserve.Location = new System.Drawing.Point(6, 37);
            this.chkPreserve.Name = "chkPreserve";
            this.chkPreserve.Size = new System.Drawing.Size(162, 17);
            this.chkPreserve.TabIndex = 6;
            this.chkPreserve.Text = "Preserve Existing Song Stats";
            this.toolTip.SetToolTip(this.chkPreserve, "If checked preserves the existing song\r\nstats for CDLC that have not been\r\nplayed" +
                    " in Rocksmith 2014 Remastered.");
            this.chkPreserve.UseVisualStyleBackColor = true;
            // 
            // btnDeleteCorruptSongs
            // 
            this.btnDeleteCorruptSongs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteCorruptSongs.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteCorruptSongs.Image")));
            this.btnDeleteCorruptSongs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteCorruptSongs.Location = new System.Drawing.Point(190, 93);
            this.btnDeleteCorruptSongs.Name = "btnDeleteCorruptSongs";
            this.btnDeleteCorruptSongs.Padding = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.btnDeleteCorruptSongs.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnDeleteCorruptSongs.Size = new System.Drawing.Size(160, 32);
            this.btnDeleteCorruptSongs.TabIndex = 9;
            this.btnDeleteCorruptSongs.Text = "   Delete Corrupt CDLC";
            this.toolTip.SetToolTip(this.btnDeleteCorruptSongs, "Delete corrupt CDLC (.cor.psarc) files.");
            this.btnDeleteCorruptSongs.UseVisualStyleBackColor = true;
            this.btnDeleteCorruptSongs.Click += new System.EventHandler(this.btnDeleteCorruptSongs_Click);
            // 
            // btnArchiveCorruptSongs
            // 
            this.btnArchiveCorruptSongs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnArchiveCorruptSongs.Image = ((System.Drawing.Image)(resources.GetObject("btnArchiveCorruptSongs.Image")));
            this.btnArchiveCorruptSongs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnArchiveCorruptSongs.Location = new System.Drawing.Point(190, 54);
            this.btnArchiveCorruptSongs.Name = "btnArchiveCorruptSongs";
            this.btnArchiveCorruptSongs.Padding = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.btnArchiveCorruptSongs.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnArchiveCorruptSongs.Size = new System.Drawing.Size(160, 32);
            this.btnArchiveCorruptSongs.TabIndex = 8;
            this.btnArchiveCorruptSongs.Text = "   Archive Corrupt CDLC";
            this.toolTip.SetToolTip(this.btnArchiveCorruptSongs, "Archive corrupt CDLC to a zip file.");
            this.btnArchiveCorruptSongs.UseVisualStyleBackColor = true;
            this.btnArchiveCorruptSongs.Click += new System.EventHandler(this.btnArchiveCorruptSongs_Click);
            // 
            // btnRestoreOrg
            // 
            this.btnRestoreOrg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRestoreOrg.Image = ((System.Drawing.Image)(resources.GetObject("btnRestoreOrg.Image")));
            this.btnRestoreOrg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRestoreOrg.Location = new System.Drawing.Point(20, 54);
            this.btnRestoreOrg.Name = "btnRestoreOrg";
            this.btnRestoreOrg.Padding = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.btnRestoreOrg.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnRestoreOrg.Size = new System.Drawing.Size(160, 32);
            this.btnRestoreOrg.TabIndex = 4;
            this.btnRestoreOrg.Text = "     Restore (.org) Backups";
            this.toolTip.SetToolTip(this.btnRestoreOrg, "WARNING:\r\nOverwrites files that have the same name.\r\n\r\nRestore original CDLC that" +
                    " have the \r\n100% Mastery Bub (.org) files to the \'dlc\'\r\nfolder so that a full re" +
                    "pair may be run again.\r\n");
            this.btnRestoreOrg.UseVisualStyleBackColor = true;
            this.btnRestoreOrg.Click += new System.EventHandler(this.btnRestoreOrg_Click);
            // 
            // btnRepairSongs
            // 
            this.btnRepairSongs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRepairSongs.Image = ((System.Drawing.Image)(resources.GetObject("btnRepairSongs.Image")));
            this.btnRepairSongs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRepairSongs.Location = new System.Drawing.Point(20, 15);
            this.btnRepairSongs.Name = "btnRepairSongs";
            this.btnRepairSongs.Padding = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.btnRepairSongs.Size = new System.Drawing.Size(330, 32);
            this.btnRepairSongs.TabIndex = 3;
            this.btnRepairSongs.Text = "Repair 100% Mastery Bug";
            this.toolTip.SetToolTip(this.btnRepairSongs, "Repair all CDLC that are located\r\ninside the \'dlc\' folder or subfolders.\r\n\r\nCheck" +
                    " the appropriate repair options.");
            this.btnRepairSongs.UseVisualStyleBackColor = true;
            this.btnRepairSongs.Click += new System.EventHandler(this.btnRepairSongs_Click);
            // 
            // pnlMiddle
            // 
            this.pnlMiddle.Controls.Add(this.gbRepairStatus);
            this.pnlMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMiddle.Location = new System.Drawing.Point(5, 180);
            this.pnlMiddle.Name = "pnlMiddle";
            this.pnlMiddle.Size = new System.Drawing.Size(889, 268);
            this.pnlMiddle.TabIndex = 7;
            // 
            // gbRepairStatus
            // 
            this.gbRepairStatus.Controls.Add(this.dgvRepair);
            this.gbRepairStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbRepairStatus.Location = new System.Drawing.Point(0, 0);
            this.gbRepairStatus.Name = "gbRepairStatus";
            this.gbRepairStatus.Size = new System.Drawing.Size(889, 268);
            this.gbRepairStatus.TabIndex = 11;
            this.gbRepairStatus.TabStop = false;
            this.gbRepairStatus.Text = "Repair Status:";
            // 
            // dgvRepair
            // 
            this.dgvRepair.AllowUserToAddRows = false;
            this.dgvRepair.AllowUserToDeleteRows = false;
            this.dgvRepair.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRepair.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRepair.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRepair.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFileName,
            this.colMessage});
            this.dgvRepair.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvRepair.HorizontalScrollBarVisible = false;
            this.dgvRepair.Location = new System.Drawing.Point(6, 19);
            this.dgvRepair.Name = "dgvRepair";
            this.dgvRepair.RowHeadersVisible = false;
            this.dgvRepair.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvRepair.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRepair.Size = new System.Drawing.Size(877, 243);
            this.dgvRepair.TabIndex = 9;
            this.dgvRepair.VerticalScrollBarVisible = true;
            // 
            // colFileName
            // 
            this.colFileName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colFileName.HeaderText = "CDLC File Name";
            this.colFileName.Name = "colFileName";
            this.colFileName.Width = 450;
            // 
            // colMessage
            // 
            this.colMessage.HeaderText = "Message";
            this.colMessage.Name = "colMessage";
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnViewErrorLog);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(2, 453);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(895, 35);
            this.pnlBottom.TabIndex = 8;
            this.toolTip.SetToolTip(this.pnlBottom, "Show Help and Usage Instructions");
            // 
            // btnHelp
            // 
            this.btnHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHelp.Image = global::CustomsForgeSongManager.Properties.Resources.Help;
            this.btnHelp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHelp.Location = new System.Drawing.Point(430, 129);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Padding = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.btnHelp.Size = new System.Drawing.Size(73, 32);
            this.btnHelp.TabIndex = 11;
            this.btnHelp.Text = "   Help";
            this.toolTip.SetToolTip(this.btnHelp, "Show \'remastered_error.log\' on the screen.");
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnViewErrorLog
            // 
            this.btnViewErrorLog.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnViewErrorLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewErrorLog.Image = ((System.Drawing.Image)(resources.GetObject("btnViewErrorLog.Image")));
            this.btnViewErrorLog.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnViewErrorLog.Location = new System.Drawing.Point(367, 2);
            this.btnViewErrorLog.Name = "btnViewErrorLog";
            this.btnViewErrorLog.Padding = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.btnViewErrorLog.Size = new System.Drawing.Size(160, 32);
            this.btnViewErrorLog.TabIndex = 10;
            this.btnViewErrorLog.Text = "   View Error Log";
            this.toolTip.SetToolTip(this.btnViewErrorLog, "Show \'remastered_error.log\' on the screen.");
            this.btnViewErrorLog.UseVisualStyleBackColor = true;
            this.btnViewErrorLog.Click += new System.EventHandler(this.btnViewErrorLog_Click);
            // 
            // btnCleanDlcFolder
            // 
            this.btnCleanDlcFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCleanDlcFolder.Image = ((System.Drawing.Image)(resources.GetObject("btnCleanDlcFolder.Image")));
            this.btnCleanDlcFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCleanDlcFolder.Location = new System.Drawing.Point(190, 132);
            this.btnCleanDlcFolder.Name = "btnCleanDlcFolder";
            this.btnCleanDlcFolder.Padding = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.btnCleanDlcFolder.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnCleanDlcFolder.Size = new System.Drawing.Size(160, 32);
            this.btnCleanDlcFolder.TabIndex = 5;
            this.btnCleanDlcFolder.Text = "   Cleanup \'dlc\' Folder";
            this.toolTip.SetToolTip(this.btnCleanDlcFolder, "Removes (.org), (.max), and (.cor) files\r\nif they exist from the \'dlc\' folder and" +
                    " \r\nsaves them to the \'backup\' folder.");
            this.btnCleanDlcFolder.UseVisualStyleBackColor = true;
            this.btnCleanDlcFolder.Click += new System.EventHandler(this.btnCleanDlcFolder_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 12000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            // 
            // btnRestoreCorrupt
            // 
            this.btnRestoreCorrupt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRestoreCorrupt.Image = ((System.Drawing.Image)(resources.GetObject("btnRestoreCorrupt.Image")));
            this.btnRestoreCorrupt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRestoreCorrupt.Location = new System.Drawing.Point(20, 132);
            this.btnRestoreCorrupt.Name = "btnRestoreCorrupt";
            this.btnRestoreCorrupt.Padding = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.btnRestoreCorrupt.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnRestoreCorrupt.Size = new System.Drawing.Size(160, 32);
            this.btnRestoreCorrupt.TabIndex = 12;
            this.btnRestoreCorrupt.Text = "     Restore (.cor) Backups";
            this.toolTip.SetToolTip(this.btnRestoreCorrupt, resources.GetString("btnRestoreCorrupt.ToolTip"));
            this.btnRestoreCorrupt.UseVisualStyleBackColor = true;
            this.btnRestoreCorrupt.Click += new System.EventHandler(this.btnRestoreCorrupt_Click);
            // 
            // BulkRepairs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpRepairs);
            this.Name = "BulkRepairs";
            this.Size = new System.Drawing.Size(899, 490);
            this.tlpRepairs.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.gbMaxPlayable.ResumeLayout(false);
            this.gbMaxPlayable.PerformLayout();
            this.gbMastery.ResumeLayout(false);
            this.gbMastery.PerformLayout();
            this.pnlMiddle.ResumeLayout(false);
            this.gbRepairStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRepair)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRepairs;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Button btnRepairSongs;
        private System.Windows.Forms.Panel pnlMiddle;
        private System.Windows.Forms.Button btnRestoreOrg;
        private System.Windows.Forms.Button btnCleanDlcFolder;
        private System.Windows.Forms.Button btnViewErrorLog;
        private System.Windows.Forms.CheckBox chkRepairOrg;
        private System.Windows.Forms.CheckBox chkPreserve;
        private System.Windows.Forms.Button btnDeleteCorruptSongs;
        private System.Windows.Forms.Button btnArchiveCorruptSongs;
        private System.Windows.Forms.GroupBox gbRepairStatus;
        private System.Windows.Forms.ToolTip toolTip;
        private DataGridViewTools.SubclassedDataGridView dgvRepair;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMessage;
        private System.Windows.Forms.GroupBox gbMastery;
        private System.Windows.Forms.CheckBox chkRemoveMetronome;
        private System.Windows.Forms.CheckBox chkRemoveBonus;
        private System.Windows.Forms.CheckBox chkRemoveGuitar;
        private System.Windows.Forms.RadioButton rbRepairMaxFive;
        private System.Windows.Forms.CheckBox chkRemoveBass;
        private System.Windows.Forms.CheckBox chkRemoveNdd;
        private System.Windows.Forms.RadioButton rbRepairMastery;
        private System.Windows.Forms.Button btnRestoreMax;
        private System.Windows.Forms.GroupBox gbMaxPlayable;
        private System.Windows.Forms.CheckBox chkIgnoreLimit;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.RadioButton rbAddDD;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.RadioButton rbSkipRepaired;
        private System.Windows.Forms.Button btnRestoreCorrupt;

    }
}
