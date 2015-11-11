using CustomsForgeManager.CustomsForgeManagerLib.CustomControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
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
                       new Expression("Contains","Contains"), 
                       new Expression("StartsWith","StartsWith"), 
                       new Expression("EndsWith","EndsWith")
                    };
        }

        protected Expression[] CreateNumberExpressions()
        {
            return new Expression[] 
                    { 
                        new NoExpression("",""),
                        new BetweenExpression("Between", ""), 
                        new OperatorExpression("Less Than", "<"), 
                        new OperatorExpression("Greater Than", ">"),
                        new OperatorExpression("Less Than or Equal", "<="), 
                        new OperatorExpression("Greater Than or Equal", ">="),
                        new OperatorExpression("Equals", "=")
                    };
        }

        protected void SetUp()
        {
            if (propInfo.PropertyType == typeof(string))
            {
                cbExpression1.DataSource = CreateStringExpressions();
                cbExpression2.DataSource = CreateStringExpressions();
            }
            else
                if (propInfo.PropertyType == typeof(int) || 
                    propInfo.PropertyType == typeof(double) || 
                    propInfo.PropertyType == typeof(float))
                {
                    cbExpression1.DataSource = CreateNumberExpressions();
                    cbExpression2.DataSource = CreateNumberExpressions();
                }
        }

        protected string Compile()
        {
            string result = "";
            if (cbExpression1.SelectedItem != null && cbExpression1.SelectedIndex != 0)
            {
                Expression x = (Expression)cbExpression1.SelectedItem;
                if (x != null)
                    result = x.Compile(propInfo.Name);
            }
            if (cbExpression2.SelectedItem != null && cbExpression2.SelectedIndex != 0)
            {
                Expression x = (Expression)cbExpression2.SelectedItem;
                if (x != null)
                {
                    if (String.IsNullOrEmpty(result))
                        result = x.Compile(propInfo.Name);
                    else
                        result += (radioButton1.Checked ? " && " : " || ") + x.Compile(propInfo.Name);

                }
            }
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
        Control oldEdit1;
        Control oldEdit2;

        private void cbExpression1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbExpression1.SelectedItem != null)
            {
                if (oldEdit1 != null)
                {
                    groupBox1.Controls.Remove(oldEdit1);
                }
                Expression x = (Expression)cbExpression1.SelectedItem;
                if (x != null)
                {
                    var c = x.GetEditor();
                    if (c != null)
                    {
                        groupBox1.Controls.Add(c);
                        c.Location = new Point(198, 28);
                        c.Size = new Size(157, 20);
                        oldEdit1 = c;
                    }
                }
            }
        }

        private void cbExpression2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbExpression2.SelectedItem != null)
            {
                if (oldEdit2 != null)
                {
                    groupBox1.Controls.Remove(oldEdit2);
                }

                Expression x = (Expression)cbExpression2.SelectedItem;
                if (x != null)
                {
                    var c = x.GetEditor();
                    if (c != null)
                    {
                        groupBox1.Controls.Add(c);
                        c.Location = new Point(198, 78);
                        c.Size = new Size(157, 20);
                        oldEdit2 = c;
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

        public virtual string Compile(string propName)
        {
            return String.Format("{0}([{1}],'{2}')", FunctionName, propName, ((CueTextBox)FEditor).Text);
        }

        protected virtual Control CreateEditor()
        {
            return new CueTextBox { Cue = "Enter a value" };
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
        public BetweenExpression(string name, string value)
            : base("Between", value)
        {

        }

        private CueTextBox Value1;
        private CueTextBox Value2;

        protected override Control CreateEditor()
        {
            var panel1 = new Panel() { Size = new Size(157, 21) };
            Value1 = new CueTextBox() { Cue = "(Enter Value)", Location = new Point(0, 0), Size = new Size(71, 20) };
            Value2 = new CueTextBox() { Cue = "(Enter Value)", Location = new Point(86, 0), Size = new Size(71, 20) };
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
