using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using CustomsForgeSongManager.DataObjects;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.DLCPackage.AggregateGraph;
using RocksmithToolkitLib.DLCPackage.Manifest2014.Tone;
using RocksmithToolkitLib.Sng;
using RocksmithToolkitLib.Extensions;
using RocksmithToolkitLib;
using RocksmithToolkitLib.XML;
using RocksmithToolkitLib.XmlRepository;
using Arrangement = RocksmithToolkitLib.DLCPackage.Arrangement;

// this code taken from toolkit ... needs to be manually updated if toolkit is revised
namespace CustomsForgeSongManager.SongEditor
{
    public partial class frmArrangement : Form
    {
        private Arrangement _arrangement;
        private ucArrangements parentControl = null;
        private Song2014 xmlSong = null;
        public bool EditMode = false;
        private bool bassFix = false;

        public Arrangement Arrangement
        {
            get
            {
                return _arrangement;
            }
            private set
            {
                _arrangement = value;

                //Song XML File
                XmlFilePath.Text = _arrangement.SongXml.File;

                //Arrangement Information
                arrangementTypeCombo.SelectedItem = _arrangement.ArrangementType;
                arrangementNameCombo.SelectedItem = _arrangement.Name;
                if (!String.IsNullOrEmpty(_arrangement.Tuning))
                    tuningComboBox.SelectedIndex = tuningComboBox.FindStringExact(_arrangement.Tuning);
                frequencyTB.Text = (_arrangement.TuningPitch > 0) ? _arrangement.TuningPitch.ToString() : "440";
                UpdateCentOffset();

                //Update it only here
                var scrollSpeed = _arrangement.ScrollSpeed;
                if (scrollSpeed == 0)
                    scrollSpeed = Convert.ToInt32(ConfigRepository.Instance().GetDecimal("creator_scrollspeed") * 10);
                scrollSpeedTrackBar.Value = Math.Min(scrollSpeed, scrollSpeedTrackBar.Maximum);
                UpdateScrollSpeedDisplay();

                Picked.Checked = _arrangement.PluckedType == PluckedType.Picked;
                BonusCheckBox.Checked = _arrangement.BonusArr;
                MetronomeCb.Checked = _arrangement.Metronome == Metronome.Generate;
                RouteMask = _arrangement.RouteMask;

                //DLC ID
                PersistentId.Text = _arrangement.Id.ToString().Replace("-", "").ToUpper();
                MasterId.Text = _arrangement.MasterId.ToString();
            }
        }

        private RouteMask RouteMask
        {
            get
            {
                if (routeMaskLeadRadio.Checked)
                    return RouteMask.Lead;
                else if (routeMaskRhythmRadio.Checked)
                    return RouteMask.Rhythm;
                else if (routeMaskBassRadio.Checked)
                    return RouteMask.Bass;
                else
                    return RouteMask.None;
            }
            set
            {
                switch (value)
                {
                    case RouteMask.Lead:
                        routeMaskLeadRadio.Checked = true;
                        break;
                    case RouteMask.Rhythm:
                        routeMaskRhythmRadio.Checked = true;
                        break;
                    case RouteMask.Bass:
                        routeMaskBassRadio.Checked = true;
                        break;
                    default:
                        routeMaskNoneRadio.Checked = true;
                        break;
                }
            }
        }

        public frmArrangement(ucArrangements control)
            : this(new Arrangement
                {
                    SongFile = new SongFile { File = "" },
                    SongXml = new SongXML { File = "" },
                    ArrangementType = ArrangementType.Guitar
                }, control)
        {
            Console.WriteLine("Debug");
        }

        public frmArrangement(Arrangement arrangement, ucArrangements control)
        {
            InitializeComponent();
            FillTuningCombo(arrangement.ArrangementType);

            foreach (var val in Enum.GetValues(typeof(ArrangementType)))
                arrangementTypeCombo.Items.Add(val);

            // this is a giant EH - careful
            arrangementTypeCombo.SelectedValueChanged += (sender, e) =>
            {
                // Selecting defaults
                var selectedType = ((ArrangementType)((ComboBox)sender).SelectedItem);

                switch (selectedType)
                {
                    case ArrangementType.Bass:
                        arrangementNameCombo.Items.Clear();
                        arrangementNameCombo.Items.Add(ArrangementName.Bass);
                        arrangementNameCombo.SelectedItem = ArrangementName.Bass;
                        break;
                    case ArrangementType.Vocal:
                        arrangementNameCombo.Items.Clear();
                        arrangementNameCombo.Items.Add(ArrangementName.Vocals);
                        arrangementNameCombo.Items.Add(ArrangementName.JVocals);
                        arrangementNameCombo.SelectedItem = ArrangementName.Vocals;
                        break;
                    case ArrangementType.ShowLight:
                        arrangementNameCombo.Items.Clear();
                        arrangementNameCombo.Items.Add(ArrangementName.ShowLights);
                        arrangementNameCombo.SelectedItem = ArrangementName.ShowLights;
                        break;
                    default:
                        arrangementNameCombo.Items.Clear();
                        arrangementNameCombo.Items.Add(ArrangementName.Combo);
                        arrangementNameCombo.Items.Add(ArrangementName.Lead);
                        arrangementNameCombo.Items.Add(ArrangementName.Rhythm);
                        arrangementNameCombo.SelectedItem = ArrangementName.Lead;
                        break;
                }

                var selectedArrangementName = (ArrangementName)arrangementNameCombo.SelectedItem;

                // Disabling options that are not meant for Arrangement Types
                // Arrangement Information
                arrangementNameCombo.Enabled = selectedType != ArrangementType.Bass;
                tuningComboBox.Enabled = (selectedType != ArrangementType.Vocal && selectedType != ArrangementType.ShowLight);
                gbTuningPitch.Enabled = (selectedType != ArrangementType.Vocal && selectedType != ArrangementType.ShowLight);
                gbScrollSpeed.Enabled = (selectedType != ArrangementType.Vocal && selectedType != ArrangementType.ShowLight);
                Picked.Enabled = selectedType == ArrangementType.Bass;
                BonusCheckBox.Enabled = gbTuningPitch.Enabled;
                MetronomeCb.Enabled = gbTuningPitch.Enabled;

                // Gameplay Path
                UpdateRouteMaskPath(selectedType, selectedArrangementName);

                // Tone Selector
                gbTone.Enabled = (selectedType != ArrangementType.Vocal && selectedType != ArrangementType.ShowLight);

                // Arrangement ID
                MasterId.Enabled = true;
                PersistentId.Enabled = true;

                // Tuning Edit
                tuningEditButton.Enabled = (selectedType != ArrangementType.Vocal && selectedType != ArrangementType.ShowLight);

                // Vocal/ShowLights Edit
                typeEdit.Enabled = (selectedType == ArrangementType.Vocal || selectedType == ArrangementType.ShowLight);

                // Update tuningComboBox
                if ((selectedType != ArrangementType.Vocal && selectedType != ArrangementType.ShowLight))
                    FillTuningCombo(selectedType);

            };

            // this EH may cause serious brain damage
            arrangementNameCombo.SelectedValueChanged += (sender, e) =>
            {
                var selectedType = ((ArrangementType)arrangementTypeCombo.SelectedItem);
                var selectedArrangementName = ((ArrangementName)((ComboBox)sender).SelectedItem);
                UpdateRouteMaskPath(selectedType, selectedArrangementName);
            };

            // this EH may cause serious brain damage
            tuningComboBox.SelectedValueChanged += (sender, e) =>
            {
                // Selecting defaults
                var selectedType = (ArrangementType)arrangementTypeCombo.SelectedItem;
                var selectedTuning = (TuningDefinition)((ComboBox)sender).SelectedItem;
                tuningEditButton.Enabled = (selectedType != ArrangementType.Vocal && selectedType != ArrangementType.ShowLight) && selectedTuning != null;
            };

            parentControl = control;
            SetupTones(arrangement);
            Arrangement = arrangement; // total update by SET action
            EditMode = routeMaskNoneRadio.Checked;
        }


        private void UpdateRouteMaskPath(ArrangementType arrangementType, ArrangementName arrangementName)
        {
            gbGameplayPath.Enabled = (arrangementType != ArrangementType.Vocal && arrangementType != ArrangementType.ShowLight);

            //Enabling
            routeMaskLeadRadio.Enabled = arrangementType == ArrangementType.Guitar && (arrangementName == ArrangementName.Combo || arrangementName == ArrangementName.Lead);
            routeMaskRhythmRadio.Enabled = arrangementType == ArrangementType.Guitar && (arrangementName == ArrangementName.Combo || arrangementName == ArrangementName.Rhythm);
            routeMaskBassRadio.Enabled = arrangementType == ArrangementType.Bass;

            //Auto-checking
            routeMaskLeadRadio.Checked = arrangementType == ArrangementType.Guitar && (arrangementName == ArrangementName.Combo || arrangementName == ArrangementName.Lead);
            routeMaskRhythmRadio.Checked = arrangementType == ArrangementType.Guitar && (arrangementName == ArrangementName.Combo || arrangementName == ArrangementName.Rhythm);
            routeMaskBassRadio.Checked = arrangementType == ArrangementType.Bass;
        }

        private void FillTuningCombo(ArrangementType arrangementType)
        {
            tuningComboBox.Items.Clear();
            var tuningDefinitions = TuningDefinitionRepository.Instance.LoadTuningDefinitions(GameVersion.RS2014);
            foreach (var tuning in tuningDefinitions)
            {
                tuningComboBox.Items.Add(tuning);
            }

            tuningComboBox.SelectedIndex = 0;
            tuningComboBox.Refresh();
        }

        private void ShowTuningForm(ArrangementType selectedType, TuningDefinition tuning)
        {
            if (tuning == null)
            {
                MessageBox.Show("Pick a tuning definition to start editing.\r\n (Current tuning is Null)", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool addNew;
            TuningDefinition formTuning;
            using (var form = new frmTuning())
            {
                form.Tuning = tuning;
                form.IsBass = selectedType == ArrangementType.Bass;

                if (DialogResult.OK != form.ShowDialog())
                    return;

                // prevent any further SET calls to form.Tuning
                formTuning = form.Tuning;
                addNew = form.AddNew;
            }

            var pcArrangements = parentControl.SongData.Arrangements;

            if (tuning.UIName != formTuning.UIName)
            {
                // Update LB slots if tuning name is changed
                for (int i = 0; i < pcArrangements.Count; i++)
                {
                    var selectedArrangement = (Arrangement)pcArrangements[i];

                    if (tuning.UIName.Equals(selectedArrangement.Tuning))
                    {
                        selectedArrangement.Tuning = formTuning.UIName;
                        pcArrangements[i] = selectedArrangement;
                    }
                }
            }

            FillTuningCombo(selectedType);

            int foundTuning = -1;
            tuningComboBox.SelectedIndex = -1;
            for (int tcbIndex = 0; tcbIndex < tuningComboBox.Items.Count; tcbIndex++)
            {
                tuningComboBox.SelectedIndex = tcbIndex;
                tuning = (TuningDefinition)tuningComboBox.Items[tcbIndex];
                if (tuning.Tuning == formTuning.Tuning)
                {
                    foundTuning = tcbIndex;
                    break;
                }
            }

            // add the custom tuning to tuningComboBox
            if (foundTuning == -1)
            {
                formTuning.Custom = true;
                tuningComboBox.Items.Add(formTuning);
                tuningComboBox.SelectedIndex = tuningComboBox.Items.Count - 1;

                if (addNew)
                    SaveTuningDefinition(formTuning);
            }
            else
                tuningComboBox.SelectedIndex = foundTuning;

            tuningComboBox.Refresh();
            Arrangement.TuningStrings = formTuning.Tuning; // forces SET update
        }

        private void UpdateCentOffset()
        {
            var value = frequencyTB.Text;
            if (!String.IsNullOrEmpty(value))
            {
                double freq = 440;
                var isValid = Double.TryParse(value, out freq);
                if (isValid && freq > 0)
                {
                    Arrangement.TuningPitch = freq;
                    string noteName;
                    TuningFrequency.Frequency2Note(freq, out noteName);
                    centOffsetDisplay.Text = String.Format("{0:0.00}", TuningFrequency.Frequency2Cents(freq));
                    noteDisplay.Text = noteName;
                }
            }
        }

        private void FillToneCombo(ComboBox combo, IEnumerable<string> toneNames, bool isBase)
        {
            var lastTone = combo.SelectedItem;
            combo.Items.Clear();
            if (!isBase)
                combo.Items.Add("");

            foreach (var tone in toneNames)
                combo.Items.Add(tone);

            combo.SelectedIndex = 0;
            if (isBase && !ReferenceEquals(lastTone, null))
            {
                combo.SelectedItem = lastTone;
            }
        }

        /// <summary>
        /// Fill toneCombo with autotone values or BaseOnly.
        /// Get tones, fill combo, select tones.
        /// </summary>
        /// <param name="arr"></param>
        private void SetupTones(Arrangement arr)
        {
            disableTonesCheckbox.Checked = false;
            var pcTones = parentControl.SongData.TonesRS2014;

            if (!String.IsNullOrEmpty(arr.ToneBase))
                if (pcTones.Count == 1 && pcTones[0].ToString() == "Default")
                    pcTones.Clear();

            var toneItems = pcTones;
            var toneNames = new List<string>();
            toneNames.AddRange(pcTones.OfType<Tone2014>().Select(t => t.Name));

            //Check if autotone tones are present and add it's if not.
            if (!toneNames.Contains(arr.ToneBase) && !String.IsNullOrEmpty(arr.ToneBase))
            {
                toneItems.Add(CreateNewTone(arr.ToneBase));
                toneNames.Add(arr.ToneBase);
            }
            if (!toneNames.Contains(arr.ToneA) && !String.IsNullOrEmpty(arr.ToneA))
            {
                toneItems.Add(CreateNewTone(arr.ToneA));
                toneNames.Add(arr.ToneA);
            }
            if (!toneNames.Contains(arr.ToneB) && !String.IsNullOrEmpty(arr.ToneB))
            {
                toneItems.Add(CreateNewTone(arr.ToneB));
                toneNames.Add(arr.ToneB);
            }
            if (!toneNames.Contains(arr.ToneC) && !String.IsNullOrEmpty(arr.ToneC))
            {
                toneItems.Add(CreateNewTone(arr.ToneC));
                toneNames.Add(arr.ToneC);
            }
            if (!toneNames.Contains(arr.ToneD) && !String.IsNullOrEmpty(arr.ToneD))
            {
                toneItems.Add(CreateNewTone(arr.ToneD));
                toneNames.Add(arr.ToneD);
            }

            // FILL TONE COMBO
            FillToneCombo(toneBaseCombo, toneNames, true);
            FillToneCombo(toneACombo, toneNames, false);
            FillToneCombo(toneBCombo, toneNames, false);
            FillToneCombo(toneCCombo, toneNames, false);
            FillToneCombo(toneDCombo, toneNames, false);

            // SELECTING TONES
            toneBaseCombo.Enabled = true;
            if (!String.IsNullOrEmpty(arr.ToneBase))
                toneBaseCombo.SelectedItem = arr.ToneBase;
            if (!String.IsNullOrEmpty(arr.ToneA))
                toneACombo.SelectedItem = arr.ToneA;
            if (!String.IsNullOrEmpty(arr.ToneB))
                toneBCombo.SelectedItem = arr.ToneB;
            if (!String.IsNullOrEmpty(arr.ToneC))
                toneCCombo.SelectedItem = arr.ToneC;
            if (!String.IsNullOrEmpty(arr.ToneD))
                toneDCombo.SelectedItem = arr.ToneD;

            // If have ToneBase and ToneB is setup it's because auto tone are setup in EoF, so, disable edit to prevent errors.
            disableTonesCheckbox.Checked = (!String.IsNullOrEmpty(arr.ToneBase) && !String.IsNullOrEmpty(arr.ToneB));
            if (disableTonesCheckbox.Checked && !EditMode)
                disableTonesCheckbox.Enabled = false;
        }

        private void SequencialToneComboEnabling()
        {
            //TODO: handle not one-by-one enabilng disabling tone slots and use data from enabled one, confused about this one.
            toneBCombo.Enabled = !String.IsNullOrEmpty((string)toneACombo.SelectedItem) && toneACombo.SelectedIndex > 0;
            toneCCombo.Enabled = !String.IsNullOrEmpty((string)toneBCombo.SelectedItem) && toneBCombo.SelectedIndex > 0;
            toneDCombo.Enabled = !String.IsNullOrEmpty((string)toneCCombo.SelectedItem) && toneCCombo.SelectedIndex > 0;
        }

        private bool IsAlreadyAdded(string xmlPath)
        {
            var pcArrangements = parentControl.SongData.Arrangements;

            for (int i = 0; i < pcArrangements.Count; i++)
            {
                var selectedArrangement = (Arrangement)pcArrangements[i];

                if (xmlPath.Equals(selectedArrangement.SongXml.File))
                {
                    if (xmlPath.Equals(Arrangement.SongXml.File))
                        continue;
                    else
                        return true;
                }
            }
            return false;
        }

        #region UI events with helpers
        private void songXmlBrowseButton_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Rocksmith Song Xml Files (*.xml)|*.xml";
                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                XmlFilePath.Text = ofd.FileName;
                string xmlFilePath = XmlFilePath.Text;
                LoadXmlArrangement(xmlFilePath);
            }
        }

        public bool LoadXmlArrangement(string xmlFilePath)
        {
            if (IsAlreadyAdded(xmlFilePath))
            {
                MessageBox.Show(@"XML Arrangement: " + Path.GetFileName(xmlFilePath) + "   " + Environment.NewLine +
                    @"has already been added.  Please choose a new file. ",
                   Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                bool isVocal = false;
                bool isShowlight = false;
                try
                {
                    xmlSong = Song2014.LoadFromFile(xmlFilePath);
                    Arrangement.XmlComments = Song2014.ReadXmlComments(xmlFilePath);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.Message.ToLower().Contains("<vocals"))
                        isVocal = true;
                    else if (ex.InnerException.Message.ToLower().Contains("<showlights"))
                        isShowlight = true;
                    else
                    {
                        MessageBox.Show(@"Unable to get information from XML arrangement:  " + Environment.NewLine +
                            Path.GetFileName(xmlFilePath) + Environment.NewLine +
                            @"It may not be a valid arrangement or " + Environment.NewLine +
                            @"your version of the EOF may be out of date." + Environment.NewLine +
                            ex.Message, Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }

                // SETUP FIELDS
                if (isVocal)
                {
                    arrangementTypeCombo.SelectedItem = ArrangementType.Vocal;
                    _arrangement.ArrangementType = ArrangementType.Vocal;
                }
                else if (isShowlight)
                {
                    arrangementTypeCombo.SelectedItem = ArrangementType.ShowLight;
                    _arrangement.ArrangementType = ArrangementType.ShowLight;
                }
                else
                {
                    //Detect arrangement GameVersion
                    if (!String.IsNullOrEmpty(xmlSong.Version))
                    {
                        if (xmlSong.Version != "7")
                        {
                            MessageBox.Show("Unable to edit song version: " + xmlSong.Version, Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unable to read song version.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                // SONG AND ARRANGEMENT INFO / ROUTE MASK
                BonusCheckBox.Checked = Equals(xmlSong.ArrangementProperties.BonusArr, 1);
                MetronomeCb.Checked = Equals(xmlSong.ArrangementProperties.Metronome, 2);
                if (!EditMode)
                {
                    string arr = xmlSong.Arrangement.ToLowerInvariant();
                    if (arr.Contains("guitar") || arr.Contains("lead") || arr.Contains("rhythm") || arr.Contains("combo"))
                    {
                        arrangementTypeCombo.SelectedItem = ArrangementType.Guitar;

                        if (arr.Contains("combo"))
                        {
                            arrangementNameCombo.SelectedItem = ArrangementName.Combo;
                            RouteMask = RouteMask.Lead;
                        }
                        else if (arr.Contains("guitar_22") || arr.Contains("lead") || Equals(xmlSong.ArrangementProperties.PathLead, 1))
                        {
                            arrangementNameCombo.SelectedItem = ArrangementName.Lead;
                            RouteMask = RouteMask.Lead;
                        }
                        else if (arr.Contains("guitar") || arr.Contains("rhythm") || Equals(xmlSong.ArrangementProperties.PathRhythm, 1))
                        {
                            arrangementNameCombo.SelectedItem = ArrangementName.Rhythm;
                            RouteMask = RouteMask.Rhythm;
                        }

                    }
                    else if (arr.Contains("bass"))
                    {
                        arrangementTypeCombo.SelectedItem = ArrangementType.Bass;
                        Picked.Checked = Equals(xmlSong.ArrangementProperties.BassPick, 1);

                        RouteMask = RouteMask.Bass;
                        //Low tuning fix for bass, If lowest string is B and bass fix not applied
                        if (xmlSong.Tuning.String0 < -4 && this.frequencyTB.Text == "440")
                            bassFix |= MessageBox.Show("The bass tuning may be too low.  Apply Low Bass Tuning Fix?" + Environment.NewLine +
                                                       "Note: The fix will revert if bass arrangement is re-saved in EOF.  ",
                                                       "Warning ... Low Bass Tuning", MessageBoxButtons.YesNo) == DialogResult.Yes;
                    }
                }

                //Tones setup
                Arrangement.ToneBase = xmlSong.ToneBase;
                Arrangement.ToneA = xmlSong.ToneA;
                Arrangement.ToneB = xmlSong.ToneB;
                Arrangement.ToneC = xmlSong.ToneC;
                Arrangement.ToneD = xmlSong.ToneD;
                Arrangement.ToneMultiplayer = null;

                SetupTones(Arrangement);

                // Fix Low Bass Tuning
                if (bassFix)
                {
                    bassFix = false;
                    Arrangement.SongXml.File = XmlFilePath.Text;

                    if (Arrangement.TuningStrings == null)
                    {
                        // need to load tuning here from the xml arrangement
                        Arrangement.TuningStrings = new TuningStrings();
                        Arrangement.TuningStrings = xmlSong.Tuning;
                    }

                    if (!TuningFrequency.ApplyBassFix(Arrangement))
                        MessageBox.Show("This bass arrangement is already at 220Hz pitch.  ",
                            Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        var commentsList = Arrangement.XmlComments.ToList();
                        commentsList.Add(new XComment("Low Bass Tuning Fixed"));
                        Arrangement.XmlComments = commentsList;
                    }

                    xmlSong.Tuning = Arrangement.TuningStrings;
                }

                // Setup tuning
                var selectedType = new ArrangementType();
                if (xmlSong.Arrangement.ToLower() == "bass")
                    selectedType = ArrangementType.Bass;
                else
                    selectedType = ArrangementType.Guitar;

                FillTuningCombo(selectedType);

                // find tuning in tuningComboBox list and make selection
                int foundTuning = -1;
                for (int tcbIndex = 0; tcbIndex < tuningComboBox.Items.Count; tcbIndex++)
                {
                    tuningComboBox.SelectedIndex = tcbIndex;
                    TuningDefinition tuning = (TuningDefinition)tuningComboBox.Items[tcbIndex];
                    if (tuning.Tuning == xmlSong.Tuning)
                    {
                        foundTuning = tcbIndex;
                        break;
                    }
                }

                if (foundTuning == -1 && selectedType != ArrangementType.Bass)
                {
                    tuningComboBox.SelectedIndex = 0;
                    ShowTuningForm(selectedType, new TuningDefinition { Tuning = xmlSong.Tuning, Custom = true, GameVersion = GameVersion.RS2014  });
                }

                // E Standard, Drop D, and Open E tuning are now the same for both guitar and bass
                if (foundTuning == -1 && selectedType == ArrangementType.Bass)
                {
                    tuningComboBox.SelectedIndex = 0;
                    MessageBox.Show("Toolkit was not able to automatically set tuning for" + Environment.NewLine +
                                    "Bass Arrangement: " + Path.GetFileName(xmlFilePath) + Environment.NewLine +
                                    "Use the tuning selector dropdown or Tuning Editor" + Environment.NewLine +
                                    "to customize bass tuning (as defined for six strings).  ", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                tuningComboBox.Refresh();
                Arrangement.Tuning = tuningComboBox.SelectedItem.ToString();
                Arrangement.TuningStrings = xmlSong.Tuning;
                Arrangement.CapoFret = xmlSong.Capo;
                frequencyTB.Text = Arrangement.TuningPitch.ToString();

                // bastard bass hack
                if (Arrangement.Tuning.ToLower().Contains("fixed"))
                    frequencyTB.Text = "220";

                UpdateCentOffset();
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Unable to get information from XML arrangement:  " + Environment.NewLine +
                    Path.GetFileName(xmlFilePath) + Environment.NewLine +
                    @"It may not be a valid arrangement or " + Environment.NewLine +
                    @"your version of the EOF may be out of date." + Environment.NewLine +
                    ex.Message, Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            //Validations
            var xmlfilepath = XmlFilePath.Text;
            if (!File.Exists(xmlfilepath))
                if (MessageBox.Show("Xml Arrangement file path is not valid.", Constants.ApplicationName, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                {
                    XmlFilePath.Focus();
                    return;
                }

            if (!routeMaskLeadRadio.Checked && !routeMaskRhythmRadio.Checked && !routeMaskBassRadio.Checked && (ArrangementType)arrangementTypeCombo.SelectedItem != ArrangementType.Vocal && (ArrangementType)arrangementTypeCombo.SelectedItem != ArrangementType.ShowLight)
            {
                if (MessageBox.Show("You did not select a Gameplay Path for this arrangement.", Constants.ApplicationName, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Cancel)
                {
                    gbGameplayPath.Focus();
                    return;
                }
            }

            LoadArrangementData(xmlfilepath);
            DialogResult = DialogResult.OK;
            Close();
        }

        public bool LoadArrangementData(string xmlfilepath)
        {
            //Song XML File
            Arrangement.SongXml.File = xmlfilepath;

            // SONG INFO 
            if (!ReferenceEquals(xmlSong, null))
            {
                var pcSongInfo = parentControl.SongData.SongInfo;

                if (String.IsNullOrEmpty(pcSongInfo.SongDisplayName)) pcSongInfo.SongDisplayName = xmlSong.Title ?? String.Empty;
                if (String.IsNullOrEmpty(pcSongInfo.SongDisplayNameSort)) pcSongInfo.SongDisplayNameSort = xmlSong.SongNameSort.GetValidSortableName() ?? pcSongInfo.SongDisplayName.GetValidSortableName();
                if (pcSongInfo.AverageTempo == 0) pcSongInfo.AverageTempo = (Int32)xmlSong.AverageTempo;
                if (String.IsNullOrEmpty(pcSongInfo.Artist)) pcSongInfo.Artist = xmlSong.ArtistName ?? String.Empty;
                if (String.IsNullOrEmpty(pcSongInfo.ArtistSort)) pcSongInfo.ArtistSort = xmlSong.ArtistNameSort.GetValidSortableName() ?? pcSongInfo.Artist.GetValidSortableName();
                if (String.IsNullOrEmpty(pcSongInfo.Album)) pcSongInfo.Album = xmlSong.AlbumName ?? String.Empty;
                if (pcSongInfo.SongYear == 0) pcSongInfo.SongYear = Convert.ToInt32(xmlSong.AlbumYear);
                if (String.IsNullOrEmpty(parentControl.SongData.Name)) parentControl.SongData.Name = String.Format("{0}{1}", pcSongInfo.Artist.GetValidAcronym(), pcSongInfo.SongDisplayName).GetValidKey(pcSongInfo.SongDisplayName);

                if (String.IsNullOrEmpty(pcSongInfo.AlbumSort))
                {
                    // substitute package author for AlbumSort
                    var useDefaultAuthor = ConfigRepository.Instance().GetBoolean("creator_usedefaultauthor");
                    if (useDefaultAuthor)
                        pcSongInfo.AlbumSort = ConfigRepository.Instance()["general_defaultauthor"].Trim().GetValidSortableName();
                    else
                        pcSongInfo.AlbumSort = xmlSong.AlbumNameSort.GetValidSortableName() ?? pcSongInfo.Album.GetValidSortableName();
                }
            }

            //Arrangement Information
            Arrangement.Name = (ArrangementName)arrangementNameCombo.SelectedItem;
            Arrangement.ArrangementType = (ArrangementType)arrangementTypeCombo.SelectedItem;
            Arrangement.ScrollSpeed = scrollSpeedTrackBar.Value;
            Arrangement.PluckedType = Picked.Checked ? PluckedType.Picked : PluckedType.NotPicked;
            Arrangement.BonusArr = BonusCheckBox.Checked;
            Arrangement.Metronome = MetronomeCb.Checked ? Metronome.Generate : Metronome.None;

            // Tuning
            TuningDefinition tuning = (TuningDefinition)tuningComboBox.SelectedItem;
            Arrangement.Tuning = tuning.UIName;
            Arrangement.TuningStrings = tuning.Tuning;

            // TODO: Add capo selection to arrangement form
            if (!ReferenceEquals(xmlSong, null))
                Arrangement.CapoFret = xmlSong.Capo;
            UpdateCentOffset();

            //ToneSelector
            Arrangement.ToneBase = toneBaseCombo.SelectedItem.ToString();
            Arrangement.ToneA = (toneACombo.SelectedItem != null) ? toneACombo.SelectedItem.ToString() : ""; //Only need if have more than one tone
            Arrangement.ToneB = (toneBCombo.SelectedItem != null) ? toneBCombo.SelectedItem.ToString() : "";
            Arrangement.ToneC = (toneCCombo.SelectedItem != null) ? toneCCombo.SelectedItem.ToString() : "";
            Arrangement.ToneD = (toneDCombo.SelectedItem != null) ? toneDCombo.SelectedItem.ToString() : "";

            //Gameplay Path
            Arrangement.RouteMask = RouteMask;

            //Xml data cleanup
            xmlSong = null;

            // DLC IDs
            Guid guid;
            if (Guid.TryParse(PersistentId.Text, out guid) == false)
                PersistentId.Focus();
            else
                Arrangement.Id = guid;

            int masterId;
            if (int.TryParse(MasterId.Text, out masterId) == false)
                MasterId.Focus();
            else
                Arrangement.MasterId = masterId;

            return true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void toneCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            SequencialToneComboEnabling();
        }

        private void frequencyTB_TextChanged(object sender, EventArgs e)
        {
            UpdateCentOffset();
        }

        private void ToneComboEnabled(bool enabled)
        {
            // Not disabling in gbTone to not disable labels
            toneBaseCombo.Enabled = enabled;
            toneACombo.Enabled = enabled;
            toneBCombo.Enabled = enabled;
            toneCCombo.Enabled = enabled;
            toneDCombo.Enabled = enabled;
        }

        private void disableTonesCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            var enabled = !disableTonesCheckbox.Checked;
            ToneComboEnabled(enabled);
            if (enabled)
                SequencialToneComboEnabling();
        }

        private void typeEdit_Click(object sender, EventArgs e)
        {
            if (_arrangement.ArrangementType == ArrangementType.Vocal) // (ArrangementType)arrangementTypeCombo.SelectedItem)
                vocalEdit_Click(sender, e);

            else if (_arrangement.ArrangementType == ArrangementType.ShowLight)
                showlightEdit_Click(sender, e);

            else
            {
                //Extra options like personal Audio file. #multitracks
                //guitarEdit_Click(sender, e);
            }
        }

        private void vocalEdit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Editing lyrics (vocals) is not currently supported.\r\n", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);

            //TODO: wrong behaviour with this warning message
            //if (!String.IsNullOrEmpty(parentControl.SongData.LyricArtPath) && String.IsNullOrEmpty(Arrangement.FontSng))
            //    MessageBox.Show("FYI, there is alredy defined one custom font.\r\n", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //using (var form = new VocalsForm(Arrangement.FontSng, parentControl.LyricArtPath, Arrangement.CustomFont))
            //{
            //    if (DialogResult.OK != form.ShowDialog())
            //    {
            //        return;
            //    }
            //    Arrangement.FontSng = form.SngPath;
            //    parentControl.LyricArtPath = form.ArtPath;
            //    Arrangement.CustomFont = form.IsCustom;
            //}
        }

        private void showlightEdit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Editing showlights is not currently supported.\r\n", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);

            //using (var form = new ShowLightsForm(Arrangement.SongFile.File))
            //{
            //    if (DialogResult.OK != form.ShowDialog())
            //        return;
            //    if (!String.IsNullOrEmpty(form.ShowLightsPath))
            //        Arrangement.SongXml.File = XmlFilePath.Text = form.ShowLightsPath;
            //}
        }

        private void tuningEditButton_Click(object sender, EventArgs e)
        {
            var selectedType = ((ArrangementType)((ComboBox)arrangementTypeCombo).SelectedItem);
            TuningDefinition tuning = (TuningDefinition)tuningComboBox.SelectedItem;

            ShowTuningForm(selectedType, tuning);
        }

        private void scrollSpeedTrackBar_ValueChanged(object sender, EventArgs e)
        {
            UpdateScrollSpeedDisplay();
        }

        private void UpdateScrollSpeedDisplay()
        {
            scrollSpeedDisplay.Text = String.Format("Scroll speed: {0:#.0}", Math.Truncate((decimal)scrollSpeedTrackBar.Value) / 10);
        }

        private void SaveTuningDefinition(TuningDefinition formTuning)
        {
            // can mess up the TuningDefinition.xml file on multiple adds
            TuningDefinitionRepository.Instance.Add(formTuning, true);
            TuningDefinitionRepository.Instance.Save(true);
        }

        #endregion

        private void ArrangementForm_Load(object sender, EventArgs e)
        {
            // disallow changing XML file name when in edit mode
            if (EditMode)
            {
                songXmlBrowseButton.Enabled = false;
                XmlFilePath.ReadOnly = true;
            }
        }



        private dynamic CreateNewTone(string toneName = "Default")
        {
            var name = GetUniqueToneName(toneName);

            return new Tone2014() { Name = name, Key = name };
        }

        private string GetUniqueToneName(string toneName)
        {
            var pcTones = parentControl.SongData.TonesRS2014;
            var uniqueName = toneName;
            bool isUnique = false;
            int ind = 1;

            do
            {
                isUnique = pcTones.OfType<Tone2014>().All(n => n.Name != uniqueName);

                if (!isUnique)
                    uniqueName = toneName + (++ind);

            } while (!isUnique);

            return uniqueName;
        }



    }
}

