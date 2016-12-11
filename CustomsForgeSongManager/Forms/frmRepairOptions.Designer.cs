namespace CustomsForgeSongManager.Forms
{
    partial class frmRepairOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRepairOptions));
            this.tcRepairOptions = new System.Windows.Forms.TabControl();
            this.tpRepairs = new System.Windows.Forms.TabPage();
            this.btnNextTabRepairs = new System.Windows.Forms.Button();
            this.rbSkipRepaired = new System.Windows.Forms.RadioButton();
            this.gbMastery = new System.Windows.Forms.GroupBox();
            this.chkIgnoreMultitoneEx = new System.Windows.Forms.CheckBox();
            this.chkRepairOrg = new System.Windows.Forms.CheckBox();
            this.chkPreserve = new System.Windows.Forms.CheckBox();
            this.tpDD = new System.Windows.Forms.TabPage();
            this.btnNextTabDD = new System.Windows.Forms.Button();
            this.chkRemoveSus = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPhraseLength = new System.Windows.Forms.NumericUpDown();
            this.rbReapplyDD = new System.Windows.Forms.RadioButton();
            this.tbRampUpPath = new CustomsForgeSongManager.LocalTools.CueTextBox();
            this.tbCFGPath = new CustomsForgeSongManager.LocalTools.CueTextBox();
            this.tpMaxFiveArrangements = new System.Windows.Forms.TabPage();
            this.btnOK = new System.Windows.Forms.Button();
            this.gbMaxPlayable = new System.Windows.Forms.GroupBox();
            this.chkIgnoreLimit = new System.Windows.Forms.CheckBox();
            this.chkRemoveMetronome = new System.Windows.Forms.CheckBox();
            this.chkRemoveGuitar = new System.Windows.Forms.CheckBox();
            this.chkRemoveBonus = new System.Windows.Forms.CheckBox();
            this.chkRemoveNdd = new System.Windows.Forms.CheckBox();
            this.chkRemoveBass = new System.Windows.Forms.CheckBox();
            this.rbRepairMaxFive = new System.Windows.Forms.RadioButton();
            this.tcRepairOptions.SuspendLayout();
            this.tpRepairs.SuspendLayout();
            this.gbMastery.SuspendLayout();
            this.tpDD.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbPhraseLength)).BeginInit();
            this.tpMaxFiveArrangements.SuspendLayout();
            this.gbMaxPlayable.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcRepairOptions
            // 
            this.tcRepairOptions.Controls.Add(this.tpRepairs);
            this.tcRepairOptions.Controls.Add(this.tpDD);
            this.tcRepairOptions.Controls.Add(this.tpMaxFiveArrangements);
            this.tcRepairOptions.Location = new System.Drawing.Point(5, 4);
            this.tcRepairOptions.Name = "tcRepairOptions";
            this.tcRepairOptions.SelectedIndex = 0;
            this.tcRepairOptions.Size = new System.Drawing.Size(263, 199);
            this.tcRepairOptions.TabIndex = 0;
            // 
            // tpRepairs
            // 
            this.tpRepairs.Controls.Add(this.btnNextTabRepairs);
            this.tpRepairs.Controls.Add(this.rbSkipRepaired);
            this.tpRepairs.Controls.Add(this.gbMastery);
            this.tpRepairs.Location = new System.Drawing.Point(4, 22);
            this.tpRepairs.Name = "tpRepairs";
            this.tpRepairs.Padding = new System.Windows.Forms.Padding(3);
            this.tpRepairs.Size = new System.Drawing.Size(255, 173);
            this.tpRepairs.TabIndex = 0;
            this.tpRepairs.Text = "Repairs";
            this.tpRepairs.UseVisualStyleBackColor = true;
            // 
            // btnNextTabRepairs
            // 
            this.btnNextTabRepairs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextTabRepairs.Location = new System.Drawing.Point(55, 133);
            this.btnNextTabRepairs.Name = "btnNextTabRepairs";
            this.btnNextTabRepairs.Padding = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.btnNextTabRepairs.Size = new System.Drawing.Size(126, 32);
            this.btnNextTabRepairs.TabIndex = 27;
            this.btnNextTabRepairs.Text = "Next";
            this.btnNextTabRepairs.UseVisualStyleBackColor = true;
            this.btnNextTabRepairs.Click += new System.EventHandler(this.btnNextTabRepairs_Click);
            // 
            // rbSkipRepaired
            // 
            this.rbSkipRepaired.AutoCheck = false;
            this.rbSkipRepaired.AutoSize = true;
            this.rbSkipRepaired.Checked = true;
            this.rbSkipRepaired.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbSkipRepaired.ForeColor = System.Drawing.Color.RoyalBlue;
            this.rbSkipRepaired.Location = new System.Drawing.Point(40, 6);
            this.rbSkipRepaired.Name = "rbSkipRepaired";
            this.rbSkipRepaired.Size = new System.Drawing.Size(174, 17);
            this.rbSkipRepaired.TabIndex = 17;
            this.rbSkipRepaired.TabStop = true;
            this.rbSkipRepaired.Text = "Skip Previously Repaired CDLC";
            this.rbSkipRepaired.UseVisualStyleBackColor = true;
            this.rbSkipRepaired.CheckedChanged += new System.EventHandler(this.RepairOptions_CheckedChanged);
            this.rbSkipRepaired.MouseClick += new System.Windows.Forms.MouseEventHandler(this.rbSkipRepaired_MouseClick);
            // 
            // gbMastery
            // 
            this.gbMastery.Controls.Add(this.chkIgnoreMultitoneEx);
            this.gbMastery.Controls.Add(this.chkRepairOrg);
            this.gbMastery.Controls.Add(this.chkPreserve);
            this.gbMastery.Location = new System.Drawing.Point(34, 25);
            this.gbMastery.Name = "gbMastery";
            this.gbMastery.Size = new System.Drawing.Size(180, 97);
            this.gbMastery.TabIndex = 15;
            this.gbMastery.TabStop = false;
            // 
            // chkIgnoreMultitoneEx
            // 
            this.chkIgnoreMultitoneEx.AutoSize = true;
            this.chkIgnoreMultitoneEx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIgnoreMultitoneEx.ForeColor = System.Drawing.Color.Red;
            this.chkIgnoreMultitoneEx.Location = new System.Drawing.Point(6, 45);
            this.chkIgnoreMultitoneEx.Name = "chkIgnoreMultitoneEx";
            this.chkIgnoreMultitoneEx.Size = new System.Drawing.Size(157, 17);
            this.chkIgnoreMultitoneEx.TabIndex = 17;
            this.chkIgnoreMultitoneEx.Text = "Ignore Multitone Exceptions";
            this.chkIgnoreMultitoneEx.UseVisualStyleBackColor = true;
            // 
            // chkRepairOrg
            // 
            this.chkRepairOrg.AutoSize = true;
            this.chkRepairOrg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRepairOrg.Location = new System.Drawing.Point(6, 74);
            this.chkRepairOrg.Name = "chkRepairOrg";
            this.chkRepairOrg.Size = new System.Drawing.Size(161, 17);
            this.chkRepairOrg.TabIndex = 7;
            this.chkRepairOrg.Text = "Run Repair Using (.org) Files";
            this.chkRepairOrg.UseVisualStyleBackColor = true;
            this.chkRepairOrg.Visible = false;
            // 
            // chkPreserve
            // 
            this.chkPreserve.AutoSize = true;
            this.chkPreserve.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPreserve.Location = new System.Drawing.Point(6, 15);
            this.chkPreserve.Name = "chkPreserve";
            this.chkPreserve.Size = new System.Drawing.Size(162, 17);
            this.chkPreserve.TabIndex = 6;
            this.chkPreserve.Text = "Preserve Existing Song Stats";
            this.chkPreserve.UseVisualStyleBackColor = true;
            // 
            // tpDD
            // 
            this.tpDD.Controls.Add(this.btnNextTabDD);
            this.tpDD.Controls.Add(this.chkRemoveSus);
            this.tpDD.Controls.Add(this.label3);
            this.tpDD.Controls.Add(this.label2);
            this.tpDD.Controls.Add(this.label1);
            this.tpDD.Controls.Add(this.tbPhraseLength);
            this.tpDD.Controls.Add(this.rbReapplyDD);
            this.tpDD.Controls.Add(this.tbRampUpPath);
            this.tpDD.Controls.Add(this.tbCFGPath);
            this.tpDD.Location = new System.Drawing.Point(4, 22);
            this.tpDD.Name = "tpDD";
            this.tpDD.Padding = new System.Windows.Forms.Padding(3);
            this.tpDD.Size = new System.Drawing.Size(255, 173);
            this.tpDD.TabIndex = 1;
            this.tpDD.Text = "DD";
            this.tpDD.UseVisualStyleBackColor = true;
            // 
            // btnNextTabDD
            // 
            this.btnNextTabDD.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextTabDD.Location = new System.Drawing.Point(66, 135);
            this.btnNextTabDD.Name = "btnNextTabDD";
            this.btnNextTabDD.Padding = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.btnNextTabDD.Size = new System.Drawing.Size(126, 32);
            this.btnNextTabDD.TabIndex = 26;
            this.btnNextTabDD.Text = "Next";
            this.btnNextTabDD.UseVisualStyleBackColor = true;
            this.btnNextTabDD.Click += new System.EventHandler(this.btnNextTabDD_Click);
            // 
            // chkRemoveSus
            // 
            this.chkRemoveSus.AutoSize = true;
            this.chkRemoveSus.Location = new System.Drawing.Point(12, 116);
            this.chkRemoveSus.Name = "chkRemoveSus";
            this.chkRemoveSus.Size = new System.Drawing.Size(102, 17);
            this.chkRemoveSus.TabIndex = 23;
            this.chkRemoveSus.Text = "Remove sustain";
            this.chkRemoveSus.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Ramp-up path:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "CFG path:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Phrase length:";
            // 
            // tbPhraseLength
            // 
            this.tbPhraseLength.Location = new System.Drawing.Point(86, 27);
            this.tbPhraseLength.Name = "tbPhraseLength";
            this.tbPhraseLength.Size = new System.Drawing.Size(35, 20);
            this.tbPhraseLength.TabIndex = 19;
            // 
            // rbReapplyDD
            // 
            this.rbReapplyDD.AutoCheck = false;
            this.rbReapplyDD.AutoSize = true;
            this.rbReapplyDD.Checked = true;
            this.rbReapplyDD.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbReapplyDD.ForeColor = System.Drawing.Color.RoyalBlue;
            this.rbReapplyDD.Location = new System.Drawing.Point(47, 4);
            this.rbReapplyDD.Name = "rbReapplyDD";
            this.rbReapplyDD.Size = new System.Drawing.Size(166, 17);
            this.rbReapplyDD.TabIndex = 18;
            this.rbReapplyDD.TabStop = true;
            this.rbReapplyDD.Text = "Reapply DD if already present";
            this.rbReapplyDD.UseVisualStyleBackColor = true;
            this.rbReapplyDD.CheckedChanged += new System.EventHandler(this.RepairOptions_CheckedChanged);
            this.rbReapplyDD.MouseClick += new System.Windows.Forms.MouseEventHandler(this.rbReapplyDD_MouseClick);
            // 
            // tbRampUpPath
            // 
            this.tbRampUpPath.Cue = "Click and select ramp-up path";
            this.tbRampUpPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.tbRampUpPath.ForeColor = System.Drawing.Color.Gray;
            this.tbRampUpPath.Location = new System.Drawing.Point(86, 85);
            this.tbRampUpPath.Name = "tbRampUpPath";
            this.tbRampUpPath.Size = new System.Drawing.Size(168, 20);
            this.tbRampUpPath.TabIndex = 25;
            this.tbRampUpPath.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tbRampUpPath_MouseClick);
            // 
            // tbCFGPath
            // 
            this.tbCFGPath.Cue = "Click and select the path of DD cfg";
            this.tbCFGPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.tbCFGPath.ForeColor = System.Drawing.Color.Gray;
            this.tbCFGPath.Location = new System.Drawing.Point(70, 56);
            this.tbCFGPath.Name = "tbCFGPath";
            this.tbCFGPath.Size = new System.Drawing.Size(184, 20);
            this.tbCFGPath.TabIndex = 24;
            this.tbCFGPath.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tbCFGPath_MouseClick);
            // 
            // tpMaxFiveArrangements
            // 
            this.tpMaxFiveArrangements.Controls.Add(this.btnOK);
            this.tpMaxFiveArrangements.Controls.Add(this.gbMaxPlayable);
            this.tpMaxFiveArrangements.Location = new System.Drawing.Point(4, 22);
            this.tpMaxFiveArrangements.Name = "tpMaxFiveArrangements";
            this.tpMaxFiveArrangements.Padding = new System.Windows.Forms.Padding(3);
            this.tpMaxFiveArrangements.Size = new System.Drawing.Size(255, 173);
            this.tpMaxFiveArrangements.TabIndex = 2;
            this.tpMaxFiveArrangements.Text = "Max5Arrs";
            this.tpMaxFiveArrangements.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(54, 135);
            this.btnOK.Name = "btnOK";
            this.btnOK.Padding = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.btnOK.Size = new System.Drawing.Size(126, 32);
            this.btnOK.TabIndex = 19;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
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
            this.gbMaxPlayable.Location = new System.Drawing.Point(6, 6);
            this.gbMaxPlayable.Name = "gbMaxPlayable";
            this.gbMaxPlayable.Size = new System.Drawing.Size(244, 110);
            this.gbMaxPlayable.TabIndex = 18;
            this.gbMaxPlayable.TabStop = false;
            // 
            // chkIgnoreLimit
            // 
            this.chkIgnoreLimit.AutoSize = true;
            this.chkIgnoreLimit.Enabled = false;
            this.chkIgnoreLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIgnoreLimit.ForeColor = System.Drawing.Color.Red;
            this.chkIgnoreLimit.Location = new System.Drawing.Point(114, 84);
            this.chkIgnoreLimit.Name = "chkIgnoreLimit";
            this.chkIgnoreLimit.Size = new System.Drawing.Size(105, 17);
            this.chkIgnoreLimit.TabIndex = 16;
            this.chkIgnoreLimit.Text = "Ignore Stop Limit";
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
            this.chkRemoveBonus.UseVisualStyleBackColor = true;
            // 
            // chkRemoveNdd
            // 
            this.chkRemoveNdd.AutoSize = true;
            this.chkRemoveNdd.Enabled = false;
            this.chkRemoveNdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRemoveNdd.Location = new System.Drawing.Point(11, 37);
            this.chkRemoveNdd.Name = "chkRemoveNdd";
            this.chkRemoveNdd.Size = new System.Drawing.Size(93, 17);
            this.chkRemoveNdd.TabIndex = 10;
            this.chkRemoveNdd.Text = "Remove NDD";
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
            this.chkRemoveBass.UseVisualStyleBackColor = true;
            // 
            // rbRepairMaxFive
            // 
            this.rbRepairMaxFive.AutoCheck = false;
            this.rbRepairMaxFive.AutoSize = true;
            this.rbRepairMaxFive.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbRepairMaxFive.ForeColor = System.Drawing.Color.RoyalBlue;
            this.rbRepairMaxFive.Location = new System.Drawing.Point(11, 13);
            this.rbRepairMaxFive.Name = "rbRepairMaxFive";
            this.rbRepairMaxFive.Size = new System.Drawing.Size(214, 17);
            this.rbRepairMaxFive.TabIndex = 12;
            this.rbRepairMaxFive.Text = "Repair Maximum Playable Arrangements";
            this.rbRepairMaxFive.UseVisualStyleBackColor = true;
            this.rbRepairMaxFive.CheckedChanged += new System.EventHandler(this.RepairOptions_CheckedChanged);
            this.rbRepairMaxFive.Click += new System.EventHandler(this.rbRepairMaxFive_Click);
            // 
            // frmRepairOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(273, 207);
            this.Controls.Add(this.tcRepairOptions);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmRepairOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose repair options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmRepairOptions_FormClosing);
            this.tcRepairOptions.ResumeLayout(false);
            this.tpRepairs.ResumeLayout(false);
            this.tpRepairs.PerformLayout();
            this.gbMastery.ResumeLayout(false);
            this.gbMastery.PerformLayout();
            this.tpDD.ResumeLayout(false);
            this.tpDD.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbPhraseLength)).EndInit();
            this.tpMaxFiveArrangements.ResumeLayout(false);
            this.gbMaxPlayable.ResumeLayout(false);
            this.gbMaxPlayable.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcRepairOptions;
        private System.Windows.Forms.TabPage tpRepairs;
        private System.Windows.Forms.TabPage tpDD;
        private System.Windows.Forms.RadioButton rbSkipRepaired;
        private System.Windows.Forms.GroupBox gbMastery;
        private System.Windows.Forms.CheckBox chkIgnoreMultitoneEx;
        private System.Windows.Forms.CheckBox chkRepairOrg;
        private System.Windows.Forms.CheckBox chkPreserve;
        private System.Windows.Forms.RadioButton rbReapplyDD;
        private System.Windows.Forms.TabPage tpMaxFiveArrangements;
        private System.Windows.Forms.GroupBox gbMaxPlayable;
        private System.Windows.Forms.CheckBox chkIgnoreLimit;
        private System.Windows.Forms.CheckBox chkRemoveMetronome;
        private System.Windows.Forms.CheckBox chkRemoveGuitar;
        private System.Windows.Forms.CheckBox chkRemoveBonus;
        private System.Windows.Forms.CheckBox chkRemoveNdd;
        private System.Windows.Forms.CheckBox chkRemoveBass;
        private System.Windows.Forms.RadioButton rbRepairMaxFive;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown tbPhraseLength;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkRemoveSus;
        private LocalTools.CueTextBox tbRampUpPath;
        private LocalTools.CueTextBox tbCFGPath;
        private System.Windows.Forms.Button btnNextTabDD;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnNextTabRepairs;
    }
}