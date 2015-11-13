using CustomsForgeManager.CustomsForgeManagerLib.CustomControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace CustomsForgeManager.Forms
{
    public partial class frmCustomFilter : Form
    {
        //  Object refObj;
        PropertyInfo propInfo;

        protected frmCustomFilter()
        {
            InitializeComponent();
        }

        protected Expression[] CreateStringExpressions()
        {
            return new Expression[] 
                    { 
                       new NoExpression("",""),
                       new Expression(Properties.Resources.Expression_Contains, "Contains"), 
                       new Expression(Properties.Resources.Expression_StartsWith,"StartsWith"), 
                       new Expression(Properties.Resources.Expression_EndsWith,"EndsWith"),
                       new Expression(Properties.Resources.Expression_IsLike, "Like")
                    };
        }

        protected Expression[] CreateNumberExpressions()
        {
            return new Expression[] 
                    { 
                        new NoExpression("",""),
                        new BetweenExpression(), 
                        new OperatorExpression(Properties.Resources.Expression_LessThan, "<"), 
                        new OperatorExpression(Properties.Resources.Expression_GreaterThan, ">"),
                        new OperatorExpression(Properties.Resources.Expression_LessThanOrEqual, "<="), 
                        new OperatorExpression(Properties.Resources.Expression_GreaterThanOrEqual, ">="),
                        new OperatorExpression(Properties.Resources.Expression_Equals, "=")
                    };
        }

        protected void SetUp()
        {
            AddExpressionControl();
        }

        protected string Compile()
        {
            string result = "";

            for (int i = 0; i < tblExpressions.Controls.Count; i++)
            {
                if (tblExpressions.Controls[i] is ExpressionGroupControl)
                {
                    ExpressionGroupControl egc = (ExpressionGroupControl)tblExpressions.Controls[i];
                    if (egc.cbExpression.SelectedItem != null && egc.cbExpression.SelectedIndex != 0)
                    {
                        Expression x = (Expression)egc.cbExpression.SelectedItem;
                        if (x != null)
                        {
                            if (String.IsNullOrEmpty(result))
                                result = (egc.cbNot.Checked ? "!" :"" ) + x.Compile(propInfo.Name) + (egc.IsAnd ? " && " : " || ");
                            else
                                result += (egc.cbNot.Checked ? "!" : "") + x.Compile(propInfo.Name) + (egc.IsAnd ? " && " : " || ");
                        }
                    }
                }
            }
            //trim the last and/or characters
            result = result.Remove(result.Length - 4);
            return result;
        }

        public static bool EditCustomFilter(ref String customFilter, PropertyInfo prop)
        {
            if (prop == null)
                return false;

            using (frmCustomFilter f = new frmCustomFilter())
            {
                f.groupBox1.Text = prop.Name;
                f.propInfo = prop;
                f.SetUp();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    customFilter = f.Compile();
                    return !String.IsNullOrEmpty(customFilter);
                }
            }
            return false;
        }

        public void AddExpressionControl()
        {

            var ec = new ExpressionGroupControl() { Dock = DockStyle.Fill };
            if (propInfo.PropertyType == typeof(string))
                ec.cbExpression.DataSource = CreateStringExpressions();
            else
                if (propInfo.PropertyType == typeof(int) ||
                 propInfo.PropertyType == typeof(double) ||
                 propInfo.PropertyType == typeof(float))
                    ec.cbExpression.DataSource = CreateNumberExpressions();

            tblExpressions.Controls.Add(ec, 0, tblExpressions.RowCount - 1);
            tblExpressions.RowCount += 1;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            AddExpressionControl();
        }

    }


    public class ExpressionGroupControl : UserControl
    {

        private RadioButton radioButton1;
        private RadioButton radioButton2;
        public ComboBox cbExpression;
        public CheckBox cbNot;
        private IContainer components = null;
        public ExpressionGroupControl()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            cbExpression = new ComboBox();
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            cbNot = new CheckBox();
            SuspendLayout();
            // 
            // cbExpression
            // 
            cbExpression.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbExpression.FormattingEnabled = true;
            cbExpression.Location = new System.Drawing.Point(3, 20);
            cbExpression.Name = "cbExpression";
            cbExpression.Size = new System.Drawing.Size(176, 21);
            cbExpression.TabIndex = 0;
            cbExpression.SelectedIndexChanged += cbExpression_SelectedIndexChanged;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Checked = true;
            radioButton1.Location = new System.Drawing.Point(3, 48);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new System.Drawing.Size(44, 17);
            radioButton1.TabIndex = 1;
            radioButton1.TabStop = true;
            radioButton1.Text = "And";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new System.Drawing.Point(53, 48);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new System.Drawing.Size(36, 17);
            radioButton2.TabIndex = 2;
            radioButton2.Text = "Or";
            radioButton2.UseVisualStyleBackColor = true;

            // 
            // cbNot
            // 
            cbNot.AutoSize = true;
            cbNot.Location = new System.Drawing.Point(3, 3);
            cbNot.Name = "cbNot";
            cbNot.Size = new System.Drawing.Size(36, 17);
            cbNot.TabIndex = 2;
            cbNot.Text = "Does Not";
            cbNot.UseVisualStyleBackColor = true;


            // 
            // ExpressionGroupControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(this.radioButton2);
            Controls.Add(this.cbNot);
            Controls.Add(this.radioButton1);
            Controls.Add(this.cbExpression);
            Size = new System.Drawing.Size(368, 72);
            ResumeLayout(false);
            PerformLayout();

        }

        public bool IsAnd
        {
            get
            {
                return radioButton1.Checked;
            }
        }

        Control oldEdit;

        private void cbExpression_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbExpression.SelectedItem != null)
            {
                if (oldEdit != null)
                {
                    Controls.Remove(oldEdit);
                }
                Expression x = (Expression)cbExpression.SelectedItem;
                if (x != null)
                {
                    var c = x.GetEditor();
                    if (c != null)
                    {
                        Controls.Add(c);
                        c.Location = new Point(198, 20);
                        c.Size = new Size(157, 20);
                        oldEdit = c;
                    }
                }
            }
        }

    }
    

    public class Expression
    {
        protected Control FEditor;
        public Expression(string name, string value)
        {
            this.Name = name;
            this.FunctionName = value;
            //cueTextBox1
        }

        public Control GetEditor()
        {
            if (FEditor == null)
                FEditor = CreateEditor();
            return FEditor;
        }

        public virtual Control[] GetEditorControls()
        {
            return new Control[] { GetEditor() };
        }

        public virtual string Compile(string propName)
        {
            return String.Format("{0}([{1}],'{2}')", FunctionName, propName, ((CueTextBox)FEditor).Text);
        }

        protected virtual Control CreateEditor()
        {
            return new CueTextBox { Cue = Properties.Resources.EnterValue };
        }


        public string Name { get; set; }
        public string FunctionName { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

    public class OperatorExpression : Expression
    {
        public OperatorExpression(string name, string value)
            : base(name, value)
        {

        }
        public override string Compile(string propName)
        {
            return
                 String.Format("([{1}] {0} {2})", FunctionName, propName, ((CueTextBox)FEditor).Text);
        }
    }

    public class NoExpression : Expression
    {
        public NoExpression(string name, string value)
            : base("None", "")
        {

        }

        protected override Control CreateEditor()
        {
            return null;
        }

        public override string Compile(string propName)
        {
            return "";
        }

    }

    public class BetweenExpression : Expression
    {
        public BetweenExpression() : base(Properties.Resources.Expression_Between, "") { }

        private CueTextBox Value1;
        private CueTextBox Value2;

        protected override Control CreateEditor()
        {
            var panel1 = new Panel() { Size = new Size(157, 21) };
            Value1 = new CueTextBox() { Cue = Properties.Resources.EnterValue, Location = new Point(0, 0), Size = new Size(71, 20) };
            Value2 = new CueTextBox() { Cue = Properties.Resources.EnterValue, Location = new Point(86, 0), Size = new Size(71, 20) };
            panel1.Controls.Add(Value1);
            panel1.Controls.Add(Value2);
            return panel1;
        }

        public override string Compile(string propName)
        {
            return String.Format("([{0}] >= {1} && [{0}] <= {2})", propName, Value1.Text, Value2.Text);
        }
    }
}
