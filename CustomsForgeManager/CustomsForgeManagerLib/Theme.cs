using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CustomsForgeManager.CustomsForgeManagerLib
{
    public class Theme : Objects.NotifyPropChangedBase
    {
        static List<Type> extraSettingsClasses = new List<Type>();

        bool canUpdate = true;
        List<IThemeListener> Listeners = new List<IThemeListener>();
        Dictionary<string, ThemeSetting> extraSettings = new Dictionary<string, ThemeSetting>();
        private Color controlColor;
        public string ThemeName { get; set; }


        static Theme()
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                var l = asm.GetTypesAssignableFrom(typeof(ThemeSetting)).Where(t => !t.IsAbstract);
                l.ToList().ForEach(at =>
                {
                    if (!extraSettingsClasses.Contains(at))
                        extraSettingsClasses.Add(at);
                }
                );
            }
        }
        public Theme()  
        {
            controlColor = SystemColors.Control;
            extraSettingsClasses.ForEach(aa =>
            {
                var cc = aa.GetConstructor(new Type[] { typeof(Theme) });
                if (cc != null)
                {
                    var newObj = (ThemeSetting)cc.Invoke(new object[] { this });
                    if (!extraSettings.ContainsKey(newObj.GetType().Name))
                    {
                        extraSettings.Add(newObj.GetType().Name, newObj);
                    }
                } else
                {
                      cc = aa.GetConstructor(new Type[] {  });
                      if (cc != null)
                      {
                          var newObj = (ThemeSetting)cc.Invoke(new object[] { });
                          if (!extraSettings.ContainsKey(newObj.GetType().Name))
                          {
                              extraSettings.Add(newObj.GetType().Name, newObj);
                          }
                      }
                }
            });
        }


        public ThemeSetting this[string Name]
        {
            get
            {
                if (extraSettings.ContainsKey(Name))
                    return extraSettings[Name];
                return null;
            }
        }

        public T SpecificThemeSetting<T>()  where T : ThemeSetting
        {
            if (extraSettings.ContainsKey(typeof(T).Name))
                return (T)extraSettings[typeof(T).Name];
           return default(T);
        }
        
        public void SaveToStream(Stream stream)
        {
            var dom = this.XmlSerializeToDom();
            var extra = dom.CreateElement("ThemeData");
            dom.DocumentElement.AppendChild(extra);
            foreach (var item in extraSettings)
            {
                var ne = dom.CreateElement(item.Key);
                var s = ne.Value.XmlSerialize();
                ne.InnerXml = s;
                extra.AppendChild(ne);
            }
            dom.Save(stream);
        }

        public void LoadFromStream(Stream stream)
        {
            this.BeginUpdate();
            try
            {
                var x = stream.DeserializeXml<Theme>();
                var props = GetType().GetProperties(
                   System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                var emptyObjParams = new object[] { };

                foreach (var p in props)
                    if (p.CanRead && p.CanWrite)
                    {
                        var ignore = p.GetCustomAttributes(typeof(XmlIgnoreAttribute), true).Length > 0;
                        if (!ignore)
                            p.SetValue(this, p.GetValue(x, emptyObjParams), emptyObjParams);
                    }
                var dom = new XmlDocument();
                dom.Load(stream);
                var tdnode = dom.DocumentElement["ThemeData"];
                if (tdnode != null)
                {
                    foreach (XmlElement ele in tdnode.ChildNodes)
                    {
                        if (ele == null)
                            continue;
                        if (extraSettings.ContainsKey(ele.Name))
                        {
                            var obj = extraSettings[ele.Name];
                            if (obj != null)
                            {
                                string data = ele.InnerXml;
                                var newObj = Extensions.XmlDeserialize(data, obj.GetType());
                                extraSettings[ele.Name] = (ThemeSetting)newObj;                            
                            }
                        }
                    }

                }
            }
            finally
            {
                this.EndUpdate();
            }




        }

        public Color ControlColor { get { return controlColor; } set { SetPropertyField("ControlColor", ref controlColor, value); } }

        public void BeginUpdate()
        {
            canUpdate = false;
        }

        public void EndUpdate()
        {
            canUpdate = true;
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            Listeners.ForEach(l => l.ApplyTheme(this));
        }

        #region Listener methods
        public void AddListener(IThemeListener listener)
        {
            Listeners.Add(listener);
        }

        public void AddListeners(IThemeListener[] listener)
        {
            Listeners.AddRange(listener);
        }

        public void RemoveListener(IThemeListener listener)
        {
            Listeners.Remove(listener);
        }

        public void RemoveListeners(IThemeListener[] listener)
        {
            Listeners.RemoveAll(z => listener.Contains(z));
        }
        #endregion

        protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (canUpdate)
                ApplyTheme();
        }
    }

    public interface IThemeListener
    {
        void ApplyTheme(Theme sender);
    }


    public abstract class ThemeSetting
    {

    }

}
