//Win.Forms Custom Theme library. Originally written for the CustomsForge Song manager

using CFSM.GenTools;
using CFSM.RSTKLib.PSARC;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace DF.WinForms.ThemeLib
{
    public class ThemeKeyAttribute : Attribute
    {
        public ThemeKeyAttribute(string keyName) { KeyName = keyName; }
        public string KeyName { get; private set; }
    }

    public class ThemeFileVersionAttribute : Attribute
    {
        public ThemeFileVersionAttribute(string version) { Version = version; }
        public string Version { get; private set; }
    }

    public class ThemeFileExtensionAttribute : Attribute
    {
        public ThemeFileExtensionAttribute(string extension) { Extension = extension; }
        public string Extension { get; private set; }
    }

    /// <summary>
    ///base class for theme settings.
    ///Theme loading/saving ignores both XmlIgnore and JsonIgnore attributes.
    ///Use the ThemeImage class for storing images
    /// </summary>
    public abstract class ThemeSetting
    {
        protected Theme theme;
        string FKey;
        public ThemeSetting()
        {
            Key = this.GetType().Name;
            ResetSettings();
        }

        public ThemeSetting(Theme Theme)
        {
            Key = this.GetType().Name;
            ResetSettings();
            this.theme = Theme;
        }

        public virtual void ResetSettings() { }


        [Browsable(false), XmlIgnore, JsonIgnore]
        public virtual string Key
        {
            get
            {
                var att = this.GetType().GetCustomAttributes(true).Where(ca => ca.GetType() == typeof(ThemeKeyAttribute)).FirstOrDefault();
                if (att != null)
                    return ((ThemeKeyAttribute)att).KeyName;
                return (String.IsNullOrEmpty(FKey)) ? this.GetType().Name : FKey;
            }
            protected set { FKey = value; }
        }
    }

    public interface IThemeListener
    {
        void ApplyTheme(Theme sender);
    }

    [JsonConverter(typeof(ThemeConverter)), ThemeFileVersion("1.0")]
    public class Theme : NotifyPropChangedBase
    {
        private const string theme_extX = ".cfsmtheme";


        public static string GetThemeExt(Type T) 
        {
            var attr = T.GetCustomAttributes(false).Where(a =>
                a.GetType() == typeof(ThemeFileExtensionAttribute)).FirstOrDefault();
            if (attr != null)
                return ((ThemeFileExtensionAttribute)attr).Extension;
            return theme_extX;
        }

        public static string GetThemeExt<T>() where T : Theme 
        {
                var attr = typeof(T).GetCustomAttributes(false).Where(a =>
                    a.GetType() == typeof(ThemeFileExtensionAttribute)).FirstOrDefault();
                if (attr != null)
                    return ((ThemeFileExtensionAttribute)attr).Extension;
                return theme_extX;
        }

        public string themeFileVersion
        {
            get
            {
                var attr = (ThemeFileVersionAttribute)this.GetType().GetCustomAttributes(false).Where(a =>
                    a.GetType() == typeof(ThemeFileVersionAttribute)).FirstOrDefault();
                if (attr != null)
                    return attr.Version;
                return "1.0";
            }
        }

        static List<Type> extraSettingsClasses = new List<Type>();

        #region private fields
        bool canUpdate = true;
        List<IThemeListener> Listeners = new List<IThemeListener>();
        Dictionary<string, ThemeSetting> extraSettings = new Dictionary<string, ThemeSetting>();
        private Color controlColor;
        private Color textColor;
        #endregion

        static Theme()
        {
            if (extraSettingsClasses.Count > 0)
                return;

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
            ThemeFileVersion = themeFileVersion;
            FEnabled = true;
            LoadDefault();
        }

        #region Public Properties
        public string ThemeName { get; set; }
        public Font Font { get; set; }
    
        public Color ControlColor
        {
            get { return controlColor; }
            set
            {
                if (value.A != 255)
                    value = Color.FromArgb(value.R, value.G, value.B);
                controlColor = value;
                //  SetPropertyField("ControlColor", ref controlColor, value);

            }
        }

        public Color TextColor
        {
            get { return textColor; }
            set
            {
                if (value.A != 255)
                    value = Color.FromArgb(value.R, value.G, value.B);
                textColor = value;
                //SetPropertyField("TextColor", ref textColor, value);

            }
        }

        public Color BorderColor { get; set; }

        public ThemeSetting this[string Name]
        {
            get
            {
                if (extraSettings.ContainsKey(Name))
                    return extraSettings[Name];
                return null;
            }
        }

        [Browsable(false)]
        public string ThemeFileVersion { get; set; }

        [Browsable(false), JsonIgnore, XmlIgnore]
        public string ThemeDirectory { get; set; }

        bool FEnabled;
        [Browsable(false), JsonIgnore, XmlIgnore,DefaultValue(true)]
        public bool Enabled { get { return FEnabled; } set 
        { 
            SetPropertyField("Enabled", ref FEnabled, value); 
        } 
        }

        #endregion

      

        private void LoadDefault()
        {
            BeginUpdate();
            try
            {
                ThemeName = "Default";
                ControlColor = SystemColors.Control;
                TextColor = SystemColors.ControlText;
                BorderColor = SystemColors.ActiveBorder;
                this.Font = new Font("Arial", 8);
                extraSettings.Clear();
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
                    }
                    else
                    {
                        cc = aa.GetConstructor(new Type[] { });
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
            finally
            {
                EndUpdate();
            }
        }

   
        public T GetThemeSetting<T>() where T : ThemeSetting
        {
            if (extraSettings.ContainsKey(typeof(T).Name))
                return (T)extraSettings[typeof(T).Name];
            return default(T);
        }

        public void SetObjectSetting(string Name, ThemeSetting obj)
        {
            if (extraSettings.ContainsKey(Name))
                extraSettings[Name] = obj;
            else
                extraSettings.Add(Name, obj);
        }

        public List<ThemeSetting> AllThemeSettings()
        {
            return extraSettings.Select(z => z.Value).ToList();
        }

        public string WriteTheme()
        {
            return JsonConvert.SerializeObject(this,
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                });
        }

        public void SaveToFile(string Filename)
        {
            using (var FS = File.Create(Filename))
                SaveToStream(FS);
        }

        public void SaveToStream(Stream stream)
        {
            using (PSARC themeArc = new PSARC(true))
            {
                MemoryStream ms = new MemoryStream();
                var x = new StreamWriter(ms);
                x.Write(WriteTheme());
                x.Flush();
                ms.Position = 0;
                themeArc.AddEntry("theme.data", ms);
                AllThemeSettings().ForEach(ts =>
                {
                    var t = ts.GetType();
                    var props =
                        t.GetProperties(System.Reflection.BindingFlags.Public |
                        System.Reflection.BindingFlags.Instance).Where(
                        p => p.PropertyType == typeof(ThemeImage) && p.CanRead && p.CanWrite
                        );
                    object[] emptyParam = new object[] { };
                    foreach (var p in props)
                    {
                        ThemeImage ti = (ThemeImage)p.GetValue(ts, emptyParam);
                        if (ti.Image != null)
                        {
                            MemoryStream imgStream = new MemoryStream();
                            ti.Image.Save(imgStream, System.Drawing.Imaging.ImageFormat.Png);
                            imgStream.Position = 0;
                            themeArc.AddEntry(ti.ImageKey + ".png", imgStream);
                        }
                    }
                });
                themeArc.TOC.Insert(0, new Entry() { Name = "NamesBlock.bin" });
                themeArc.TOC.RemoveAll(e => e.Name == "");
                themeArc.Write(stream);
            }
        }

       
        public virtual void LoadTheme(string ThemeName)
        {
            if (ThemeName.ToLower() == "default")
                LoadDefault();
            else
                LoadFromFile(Path.Combine(ThemeDirectory, ThemeName + GetThemeExt(this.GetType())));
        }

        public void LoadFromFile(string Filename)
        {
            if (File.Exists(Filename))
                using (var FS = File.OpenRead(Filename))
                    LoadFromStream(FS);
        }

        public void LoadFromStream(Stream stream)
        {
            this.BeginUpdate();
            try
            {
                using (PSARC themeArc = new PSARC(true))
                {
                    themeArc.Read(stream);

                    var td = themeArc.TOC.Where(e => e.Name == "theme.data").FirstOrDefault();
                    if (td == null)
                        return;

                    StreamReader sw = new StreamReader(td.Data);
                    var x = JsonConvert.DeserializeObject<Theme>(sw.ReadToEnd());

                    var props = GetType().GetProperties(
                       System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Where(
                       p => p.CanWrite && p.CanRead);

                    var emptyObjParams = new object[] { };

                    foreach (var p in props)
                    {
                        var ignore = p.GetCustomAttributes(typeof(XmlIgnoreAttribute), true).Length > 0 ||
                             p.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Length > 0;
                        if (!ignore)
                        {
                            var aValue = p.GetValue(x, emptyObjParams);
                            if (aValue != null && p.PropertyType == typeof(ThemeImage))
                            {
                                ThemeImage ti = (ThemeImage)aValue;
                                var img = themeArc.TOC.Where(e => e.Name == ti.ImageKey + ".png").FirstOrDefault();
                                if (img != null)
                                {
                                    ti.SetImage(Image.FromStream(img.Data));
                                }
                            }
                            p.SetValue(this, aValue, emptyObjParams);
                        }
                    }
                    //  extraSettings.Clear();
                    foreach (var z in x.extraSettings)
                    {

                        var propsX = z.Value.GetType().GetProperties(
                                            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Where(
                                            p => p.CanWrite && p.CanRead && p.PropertyType == typeof(ThemeImage));

                        foreach (var p in propsX)
                        {
                            var aValue = p.GetValue(z.Value, emptyObjParams);
                            if (aValue != null)
                            {
                                ThemeImage ti = (ThemeImage)aValue;
                                var img = themeArc.TOC.Where(e => e.Name == ti.ImageKey + ".png").FirstOrDefault();
                                if (img != null)
                                {
                                    ti.SetImage(Image.FromStream(img.Data));
                                }
                            }
                        }


                        if (extraSettings.ContainsKey(z.Key))
                            extraSettings[z.Key] = z.Value;
                        else
                            extraSettings.Add(z.Key, z.Value);
                    }
                }
            }
            finally
            {
                this.EndUpdate();
            }
        }

        public void BeginUpdate()
        {
            canUpdate = false;
        }

        public void EndUpdate()
        {
            canUpdate = true;
            ApplyTheme();
        }

      
        bool inPreview = false;
        NoCloseStream oldData;
        public void BeginPreview()
        {
            if (!inPreview)
            {
                oldData = new NoCloseStream();
                SaveToStream(oldData);
                inPreview = true;
            }
        }


        public void EndPreview()
        {
            if (inPreview)
            {
                oldData.Position = 0;
                LoadFromStream(oldData);
                oldData.canClose = true;
                oldData.Close();
                oldData = null;
                inPreview = false;
            }
        }


        public void ApplyTheme()
        {
            if (canUpdate && Enabled)
                Listeners.ForEach(l => l.ApplyTheme(this));
        }

        #region Listener methods
        public void AddListener(IThemeListener listener)
        {
            if (!Listeners.Contains(listener))
                Listeners.Add(listener);
        }

        public void AddListeners(IThemeListener[] listener)
        {
            Listeners.AddRange(listener.Where(z => !Listeners.Contains(z)));
        }

        public void RemoveListener(IThemeListener listener)
        {
            Listeners.Remove(listener);
        }

        public void RemoveListeners(IThemeListener[] listener)
        {
            Listeners.RemoveAll(z => listener.Contains(z));
        }

        public void RemoveListeners()
        {
            Listeners.Clear();
        }
        #endregion

    }




}
