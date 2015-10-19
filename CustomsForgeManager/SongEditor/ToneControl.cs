using System;
using System.Linq;
using System.Windows.Forms;
using RocksmithToolkitLib;
using RocksmithToolkitLib.DLCPackage.Manifest2014.Tone;
using RocksmithToolkitLib.ToolkitTone;

namespace CustomsForgeManager.SongEditor
{
    public partial class ToneControl : UserControl
    {
        private bool _refreshingCombos = false;

        private Tone2014 tone;
        public Tone2014 Tone
        {
            set
            {
                tone = value;
                if (value != null)
                    RefreshControls();
            }
            get
            {
                return tone;
            }
        }

        private const string RackSlot = "Rack";
       

        public ToneControl()
        {
            InitializeComponent();

        }

        public void Init()
        {
             toneNameBox.ReadOnly = true;
            InitializeToneInformation();
            InitializeComboBoxes();

            descriptorLabel.Enabled = true;
            descriptorCombo.Enabled = true;
            gbLoopPedalAndRacks.Text = RackSlot;
            gbPostPedal.Text = "Loop Pedal";
            loopPedalRack4Box.Enabled = true;
            loopPedalRack4KnobButton.Enabled = true;
            prePedal4Box.Enabled = true;
            prePedal4KnobButton.Enabled = true;
            postPedal4Box.Enabled = true;
            postPedal4KnobButton.Enabled = true;
        }

        public void RefreshControls()
        {
            _refreshingCombos = true;
            toneNameBox.Text = tone.Name ?? "";
            volumeBox.Value = Decimal.Round((decimal)tone.Volume, 2);

            UpdateComboSelection(ampBox, ampKnobButton, "Amp");
            UpdateComboSelection(cabinetBox, cabinetKnobButton, "Cabinet");

            UpdateComboSelection(prePedal1Box, prePedal1KnobButton, "PrePedal1");
            UpdateComboSelection(prePedal2Box, prePedal2KnobButton, "PrePedal2");
            UpdateComboSelection(prePedal3Box, prePedal3KnobButton, "PrePedal3");
            UpdateComboSelection(prePedal4Box, prePedal4KnobButton, "PrePedal4");

            UpdateComboSelection(loopPedalRack1Box, loopPedalRack1KnobButton, RackSlot + "1");
            UpdateComboSelection(loopPedalRack2Box, loopPedalRack2KnobButton, RackSlot + "2");
            UpdateComboSelection(loopPedalRack3Box, loopPedalRack3KnobButton, RackSlot + "3");
            UpdateComboSelection(loopPedalRack4Box, loopPedalRack4KnobButton, RackSlot + "4");

            UpdateComboSelection(postPedal1Box, postPedal1KnobButton, "PostPedal1");
            UpdateComboSelection(postPedal2Box, postPedal2KnobButton, "PostPedal2");
            UpdateComboSelection(postPedal3Box, postPedal3KnobButton, "PostPedal3");
            UpdateComboSelection(postPedal4Box, postPedal4KnobButton, "PostPedal4");
            _refreshingCombos = false;

            // TODO: multiple ToneDescriptors improved handling and editing
            if (tone.ToneDescriptors.Count > 0)
            {
                if (ToneDescriptor.List().Any<ToneDescriptor>(t => t.Descriptor == tone.ToneDescriptors[0]))
                    descriptorCombo.SelectedIndex = ToneDescriptor.List().TakeWhile(t => t.Descriptor != tone.ToneDescriptors[0]).Count();
            }
            else
                UpdateToneDescription(descriptorCombo);
        }

        private void UpdateComboSelection(ComboBox box, Control knobSelectButton, string pedalSlot)
        {
            box.SelectedItem = tone.GearList[pedalSlot] != null ?
                box.Items.OfType<ToolkitPedal>().First(p => p.Key == tone.GearList[pedalSlot].PedalKey) : null;

            knobSelectButton.Enabled = tone.GearList[pedalSlot] != null ?
                ((Pedal2014)tone.GearList[pedalSlot]).KnobValues.Any() : false;
        }

        private void InitializeToneInformation()
        {
            // VOLUME
            volumeBox.ValueChanged += (sender, e) =>
            {
                Tone_Volume_Tip(volumeBox, e);
                tone.Volume = (float)volumeBox.Value;
            };

            // TONE DESCRIPTOR
            var tonedesclist = ToneDescriptor.List().ToList();
            descriptorCombo.DisplayMember = "Name";
            descriptorCombo.ValueMember = "Descriptor";
            descriptorCombo.DataSource = tonedesclist;

            descriptorCombo.SelectedValueChanged += (sender, e) =>
                UpdateToneDescription((ComboBox)sender);
        }

        private void Tone_Volume_Tip(object sender, EventArgs f)
        {
            ToolTip tvt = new ToolTip() { IsBalloon = true, InitialDelay = 0, ShowAlways = true };
            tvt.SetToolTip(volumeBox, "LOWEST 0,-1,-2,-3,..., AVERAGE -12 ,...,-20,-21 HIGHER");
        }

        private void UpdateToneDescription(ComboBox combo)
        {
            if (_refreshingCombos)
                return;

            var descriptor = combo.SelectedItem as ToneDescriptor;
            tone.ToneDescriptors.Clear();
            tone.ToneDescriptors.Add(descriptor.Descriptor);
        }

        private void InitializeComboBoxes()
        {
            var allPedals = ToolkitPedal.LoadFromResource(GameVersion.RS2014);

            var amps = allPedals
                .Where(p => p.TypeEnum == PedalType.Amp)
                .OrderBy(p => p.DisplayName)
                .ToArray();
            var cabinets = allPedals
                .Where(p => p.TypeEnum == PedalType.Cabinet)
                .OrderBy(p => p.DisplayName)
                .ToArray();
            var loopRackPedals = allPedals
                .Where(p => p.TypeEnum == PedalType.Rack)
                .OrderBy(p => p.DisplayName)
                .ToArray();
            var prePedals = allPedals
                .Where(p => p.TypeEnum == PedalType.Pedal )
                .OrderBy(p => p.DisplayName)
                .ToArray();
            var postPedals = allPedals
                .Where(p => p.TypeEnum == PedalType.Pedal)
                .OrderBy(p => p.DisplayName)
                .ToArray();

            InitializeSelectedPedal(ampBox, ampKnobButton, "Amp", amps, false);
            InitializeSelectedPedal(cabinetBox, cabinetKnobButton, "Cabinet", cabinets, false);

            InitializeSelectedPedal(loopPedalRack1Box, loopPedalRack1KnobButton, RackSlot + "1", loopRackPedals, true);
            InitializeSelectedPedal(loopPedalRack2Box, loopPedalRack2KnobButton, RackSlot + "2", loopRackPedals, true);
            InitializeSelectedPedal(loopPedalRack3Box, loopPedalRack3KnobButton, RackSlot + "3", loopRackPedals, true);
            InitializeSelectedPedal(loopPedalRack4Box, loopPedalRack4KnobButton, RackSlot + "4", loopRackPedals, true);

            InitializeSelectedPedal(prePedal1Box, prePedal1KnobButton, "PrePedal1", prePedals, true);
            InitializeSelectedPedal(prePedal2Box, prePedal2KnobButton, "PrePedal2", prePedals, true);
            InitializeSelectedPedal(prePedal3Box, prePedal3KnobButton, "PrePedal3", prePedals, true);
            InitializeSelectedPedal(prePedal4Box, prePedal4KnobButton, "PrePedal4", prePedals, true);

            InitializeSelectedPedal(postPedal1Box, postPedal1KnobButton, "PostPedal1", postPedals, true);
            InitializeSelectedPedal(postPedal2Box, postPedal2KnobButton, "PostPedal2", postPedals, true);
            InitializeSelectedPedal(postPedal3Box, postPedal3KnobButton, "PostPedal3", postPedals, true);
            InitializeSelectedPedal(postPedal4Box, postPedal4KnobButton, "PostPedal4", postPedals, true);
        }

        private void InitializeSelectedPedal(ComboBox box, Control knobSelectButton, string pedalSlot, ToolkitPedal[] pedals, bool allowNull)
        {
            knobSelectButton.Enabled = false;
            knobSelectButton.Click += (sender, e) =>
            {
                dynamic pedal = tone.GearList[pedalSlot];
                using (var form = new frmToneKnob())
                {
                    form.Init(pedal, pedals.Single(p => p.Key == pedal.PedalKey).Knobs);
                    form.ShowDialog(this.ParentForm);
                }
            };

            box.Items.Clear();
            box.DisplayMember = "DisplayName";
            if (allowNull)
                box.Items.Add(string.Empty);
            box.Items.AddRange(pedals);
            box.SelectedValueChanged += (sender, e) =>
            {
                if (_refreshingCombos)
                    return;

                var pedal = box.SelectedItem as ToolkitPedal;
                if (pedal == null)
                {
                    tone.GearList[pedalSlot] = null;
                    knobSelectButton.Enabled = false;
                }
                else
                {
                    string pedalKey = "";

                    if (tone.GearList[pedalSlot] != null)
                        pedalKey = tone.GearList[pedalSlot].PedalKey;

                    if (pedal.Key != pedalKey)
                    {
                        var pedalSetting = pedal.MakePedalSetting(GameVersion.RS2014);
                        tone.GearList[pedalSlot] = pedalSetting;
                        knobSelectButton.Enabled = ((Pedal2014)pedalSetting).KnobValues.Any();                      
                    }
                    else
                    {
                        knobSelectButton.Enabled = ((Pedal2014)tone.GearList[pedalSlot]).KnobValues.Any();
                    }
                }
            };
        }
    }
}

