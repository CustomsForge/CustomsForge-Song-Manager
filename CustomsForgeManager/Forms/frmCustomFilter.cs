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
        Object refObj;
        PropertyInfo propInfo;

        Expression[] StringNames =
            new Expression[] 
            { 
               new Expression("Contains","Contains"), 
               new Expression("IsLike","IsLike"), 
               new Expression("StartsWith","StartsWith"), 
               new Expression("EndsWith","EndsWith")
            };

        Expression[] IntNames = new Expression[]  
           { 
               new Expression("Between", "Between", 2), 
               new Expression("LessThan", "<"), 
               new Expression("GreaterThan", ">"),
               new Expression("Equals", "="),
            };

        protected frmCustomFilter()
        {
            InitializeComponent();
        }

        protected void SetUp()
        {
            if (propInfo.PropertyType == typeof(string))
            {
                cbExpression1.DataSource = StringNames;
                cbExpression2.DataSource = StringNames;
            }
            else
            if (propInfo.PropertyType == typeof(int) || propInfo.PropertyType == typeof(double) || propInfo.PropertyType == typeof(float))
            {
                cbExpression1.DataSource = IntNames;
                cbExpression2.DataSource = IntNames;

            }
        }

        protected string Compile()
        {
            return string.Empty;
        }

        public static bool EditCustomFilter(ref String customFilter, Object obj, PropertyInfo prop)
        {
            if (obj == null || prop == null)
                return false;

            using (frmCustomFilter f = new frmCustomFilter())
            {
                f.refObj = obj;
                f.propInfo = prop;
                f.SetUp();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    customFilter = f.Compile();

                }

            }


            return false;
        } 
    }

    public class Expression
    {
        public Expression(string name, string value)
        {
            this.Name = name;
            this.OptValue = value;
            NeededValues = 1;
        }
        public Expression(string name, string value, int Xvalue):this(name,value)
        {
            NeededValues = Xvalue;
        }

        public string Name { get; set; }
        public string OptValue { get; set; }
        public int NeededValues { get; private set; }

        public override string ToString()
        {
            return Name;
        }

    }
}
