using System.ComponentModel;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib.CustomControls;

namespace CustomsForgeManager.UControls
{
    partial class Settings
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.tlpSettings_Wrapper = new System.Windows.Forms.TableLayoutPanel();
            this.chkIncludeRS1DLC = new System.Windows.Forms.CheckBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lnkSelectAll = new System.Windows.Forms.LinkLabel();
            this.lblDisabledColumns = new System.Windows.Forms.Label();
            this.listDisabledColumns = new System.Windows.Forms.ListView();
            this.columnSelect = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSettingsColumnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSettingsColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSettingsWidth = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cueRsDir = new CustomsForgeManager.CustomsForgeManagerLib.CustomControls.CueTextBox();
            this.lblSettingsRSDir = new System.Windows.Forms.Label();
            this.btnSettingsLoad = new System.Windows.Forms.Button();
            this.btnSettingsSave = new System.Windows.Forms.Button();
            this.chkEnableLogBallon = new System.Windows.Forms.CheckBox();
            this.tbCreator = new CustomsForgeManager.CustomsForgeManagerLib.CustomControls.CueTextBox();
            this.tlpSettings_Wrapper.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSettings_Wrapper
            // 
            this.tlpSettings_Wrapper.ColumnCount = 3;
            this.tlpSettings_Wrapper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.88513F));
            this.tlpSettings_Wrapper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.94813F));
            this.tlpSettings_Wrapper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.16674F));
            this.tlpSettings_Wrapper.Controls.Add(this.chkIncludeRS1DLC, 0, 2);
            this.tlpSettings_Wrapper.Controls.Add(this.panel5, 2, 1);
            this.tlpSettings_Wrapper.Controls.Add(this.cueRsDir, 2, 0);
            this.tlpSettings_Wrapper.Controls.Add(this.lblSettingsRSDir, 0, 0);
            this.tlpSettings_Wrapper.Controls.Add(this.btnSettingsLoad, 0, 6);
            this.tlpSettings_Wrapper.Controls.Add(this.btnSettingsSave, 1, 6);
            this.tlpSettings_Wrapper.Controls.Add(this.chkEnableLogBallon, 0, 3);
            this.tlpSettings_Wrapper.Controls.Add(this.tbCreator, 0, 4);
            this.tlpSettings_Wrapper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSettings_Wrapper.Location = new System.Drawing.Point(0, 0);
            this.tlpSettings_Wrapper.Name = "tlpSettings_Wrapper";
            this.tlpSettings_Wrapper.RowCount = 9;
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 256F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSettings_Wrapper.Size = new System.Drawing.Size(866, 490);
            this.tlpSettings_Wrapper.TabIndex = 1;
            // 
            // chkIncludeRS1DLC
            // 
            this.chkIncludeRS1DLC.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.chkIncludeRS1DLC, 2);
            this.chkIncludeRS1DLC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIncludeRS1DLC.Location = new System.Drawing.Point(3, 52);
            this.chkIncludeRS1DLC.Name = "chkIncludeRS1DLC";
            this.chkIncludeRS1DLC.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.chkIncludeRS1DLC.Size = new System.Drawing.Size(205, 19);
            this.chkIncludeRS1DLC.TabIndex = 3;
            this.chkIncludeRS1DLC.Text = "Include RS1 Compatibility Pack";
            this.chkIncludeRS1DLC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkIncludeRS1DLC.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.lnkSelectAll);
            this.panel5.Controls.Add(this.lblDisabledColumns);
            this.panel5.Controls.Add(this.listDisabledColumns);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(347, 28);
            this.panel5.Name = "panel5";
            this.tlpSettings_Wrapper.SetRowSpan(this.panel5, 7);
            this.panel5.Size = new System.Drawing.Size(516, 419);
            this.panel5.TabIndex = 4;
            // 
            // lnkSelectAll
            // 
            this.lnkSelectAll.AutoSize = true;
            this.lnkSelectAll.Location = new System.Drawing.Point(3, 5);
            this.lnkSelectAll.Name = "lnkSelectAll";
            this.lnkSelectAll.Size = new System.Drawing.Size(98, 13);
            this.lnkSelectAll.TabIndex = 6;
            this.lnkSelectAll.TabStop = true;
            this.lnkSelectAll.Text = "Select/Deselect All";
            this.lnkSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSelectAll_LinkClicked);
            // 
            // lblDisabledColumns
            // 
            this.lblDisabledColumns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDisabledColumns.AutoSize = true;
            this.lblDisabledColumns.Location = new System.Drawing.Point(234, 5);
            this.lblDisabledColumns.Name = "lblDisabledColumns";
            this.lblDisabledColumns.Size = new System.Drawing.Size(120, 13);
            this.lblDisabledColumns.TabIndex = 1;
            this.lblDisabledColumns.Text = "Song Manager Columns";
            this.lblDisabledColumns.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // listDisabledColumns
            // 
            this.listDisabledColumns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listDisabledColumns.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.listDisabledColumns.CheckBoxes = true;
            this.listDisabledColumns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnSelect,
            this.colSettingsColumnName,
            this.colSettingsColumnHeader,
            this.colSettingsWidth});
            this.listDisabledColumns.FullRowSelect = true;
            this.listDisabledColumns.GridLines = true;
            this.listDisabledColumns.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listDisabledColumns.Location = new System.Drawing.Point(0, 24);
            this.listDisabledColumns.MultiSelect = false;
            this.listDisabledColumns.Name = "listDisabledColumns";
            this.listDisabledColumns.Size = new System.Drawing.Size(513, 440);
            this.listDisabledColumns.TabIndex = 5;
            this.listDisabledColumns.UseCompatibleStateImageBehavior = false;
            this.listDisabledColumns.View = System.Windows.Forms.View.Details;
            this.listDisabledColumns.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listDisabledColumns_ItemChecked);
            // 
            // columnSelect
            // 
            this.columnSelect.Text = "";
            this.columnSelect.Width = 25;
            // 
            // colSettingsColumnName
            // 
            this.colSettingsColumnName.Text = "Column Name";
            this.colSettingsColumnName.Width = 150;
            // 
            // colSettingsColumnHeader
            // 
            this.colSettingsColumnHeader.Text = "Column Header";
            this.colSettingsColumnHeader.Width = 150;
            // 
            // colSettingsWidth
            // 
            this.colSettingsWidth.Text = "Column Width";
            this.colSettingsWidth.Width = 150;
            // 
            // cueRsDir
            // 
            this.cueRsDir.Cue = "Click here and specify Rocksmith installation directory";
            this.cueRsDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cueRsDir.ForeColor = System.Drawing.Color.Gray;
            this.cueRsDir.Location = new System.Drawing.Point(347, 3);
            this.cueRsDir.Multiline = true;
            this.cueRsDir.Name = "cueRsDir";
            this.cueRsDir.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.cueRsDir.Size = new System.Drawing.Size(516, 19);
            this.cueRsDir.TabIndex = 8;
            this.cueRsDir.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cueRsDir_MouseClick);
            // 
            // lblSettingsRSDir
            // 
            this.lblSettingsRSDir.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblSettingsRSDir.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.lblSettingsRSDir, 2);
            this.lblSettingsRSDir.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSettingsRSDir.Location = new System.Drawing.Point(126, 3);
            this.lblSettingsRSDir.Margin = new System.Windows.Forms.Padding(3);
            this.lblSettingsRSDir.Name = "lblSettingsRSDir";
            this.lblSettingsRSDir.Padding = new System.Windows.Forms.Padding(3);
            this.lblSettingsRSDir.Size = new System.Drawing.Size(215, 19);
            this.lblSettingsRSDir.TabIndex = 7;
            this.lblSettingsRSDir.Text = "Rocksmith Installation Directory:";
            this.lblSettingsRSDir.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSettingsLoad
            // 
            this.btnSettingsLoad.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSettingsLoad.Image = global::CustomsForgeManager.Properties.Resources.load;
            this.btnSettingsLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSettingsLoad.Location = new System.Drawing.Point(43, 396);
            this.btnSettingsLoad.Name = "btnSettingsLoad";
            this.btnSettingsLoad.Size = new System.Drawing.Size(102, 34);
            this.btnSettingsLoad.TabIndex = 0;
            this.btnSettingsLoad.Text = "Load Settings";
            this.btnSettingsLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSettingsLoad.UseVisualStyleBackColor = true;
            this.btnSettingsLoad.Click += new System.EventHandler(this.btnSettingsLoad_Click);
            // 
            // btnSettingsSave
            // 
            this.btnSettingsSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSettingsSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSettingsSave.Image = global::CustomsForgeManager.Properties.Resources.save;
            this.btnSettingsSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSettingsSave.Location = new System.Drawing.Point(192, 396);
            this.btnSettingsSave.Name = "btnSettingsSave";
            this.btnSettingsSave.Size = new System.Drawing.Size(103, 34);
            this.btnSettingsSave.TabIndex = 0;
            this.btnSettingsSave.Text = "Save Settings";
            this.btnSettingsSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSettingsSave.UseVisualStyleBackColor = true;
            this.btnSettingsSave.Click += new System.EventHandler(this.btnSettingsSave_Click);
            // 
            // chkEnableLogBallon
            // 
            this.chkEnableLogBallon.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.chkEnableLogBallon, 2);
            this.chkEnableLogBallon.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableLogBallon.Location = new System.Drawing.Point(3, 84);
            this.chkEnableLogBallon.Name = "chkEnableLogBallon";
            this.chkEnableLogBallon.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.chkEnableLogBallon.Size = new System.Drawing.Size(143, 19);
            this.chkEnableLogBallon.TabIndex = 5;
            this.chkEnableLogBallon.Text = "Enable Log Baloon ";
            this.chkEnableLogBallon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkEnableLogBallon.UseVisualStyleBackColor = true;
            this.chkEnableLogBallon.CheckedChanged += new System.EventHandler(this.chkEnableLogBaloon_CheckedChanged);
            // 
            // tbCreator
            // 
            this.tlpSettings_Wrapper.SetColumnSpan(this.tbCreator, 2);
            this.tbCreator.Cue = "Your CDLC Creator Name";
            this.tbCreator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.tbCreator.ForeColor = System.Drawing.Color.Gray;
            this.tbCreator.Location = new System.Drawing.Point(3, 116);
            this.tbCreator.Name = "tbCreator";
            this.tbCreator.Size = new System.Drawing.Size(338, 20);
            this.tbCreator.TabIndex = 9;
            this.tbCreator.TextChanged += new System.EventHandler(this.tbCreator_TextChanged);
            // 
            // Settings
            // 
            this.Controls.Add(this.tlpSettings_Wrapper);
            this.Name = "Settings";
            this.Size = new System.Drawing.Size(866, 490);
            this.tlpSettings_Wrapper.ResumeLayout(false);
            this.tlpSettings_Wrapper.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel tlpSettings_Wrapper;
        private Button btnSettingsLoad;
        private Panel panel5;
        private LinkLabel lnkSelectAll;
        private Label lblDisabledColumns;
        private ListView listDisabledColumns;
        private ColumnHeader columnSelect;
        private ColumnHeader colSettingsColumnName;
        private ColumnHeader colSettingsColumnHeader;
        private ColumnHeader colSettingsWidth;
        private Button btnSettingsSave;
        private Label lblSettingsRSDir;
        private CueTextBox cueRsDir;
        public CheckBox chkIncludeRS1DLC;
        private CheckBox chkEnableLogBallon;
        private CueTextBox tbCreator;
    }
}
