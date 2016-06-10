using System.Linq;
using CFSM.GenTools;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Drawing;

namespace DF.WinForms.ThemeLib
{
    public class ColorConverter : JsonConverter
    {
        public static bool UseColorNames = true;

        public override bool CanConvert(Type objectType)
        {
            return typeof(Color).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Integer)
                return Color.FromArgb(Convert.ToInt32(reader.Value));
            else
                if (reader.TokenType == JsonToken.String)
                    return ColorTranslator.FromHtml(reader.Value.ToString());
            return Color.White;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var color = (Color)value;
            if (!UseColorNames)
                writer.WriteValue(ColorTranslator.ToHtml(Color.FromArgb(color.ToArgb())));
            else
                writer.WriteValue(ColorTranslator.ToHtml(color));
        }
    }

    public class ThemeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Theme).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            serializer.Converters.Add(new ColorConverter());
            if (reader.TokenType == JsonToken.StartObject)
            {
                reader.Read();

                Theme target = new Theme();

                while (reader.TokenType == JsonToken.PropertyName)
                {
                    string pn = reader.Value.ToString();
                    reader.Read();
                    if (pn == "extravalues")
                    {
                        reader.Read();//startarray
                        while (reader.TokenType == JsonToken.StartObject)
                        {
                            reader.Read();//startobject
                            if (reader.Value.ToString() == "type")
                            {
                                string xtype = reader.ReadAsString();
                                reader.Read();
                                reader.Read();//endobject
                                var atype = TypeExtensions.GetLoadableTypes().Where(x => x.Name == xtype).FirstOrDefault();
                                // TypeExtensions.GetLoadableTypes(this.GetType().Assembly).Where(x => x.Name == xtype).FirstOrDefault();
                                if (atype != null)
                                {
                                    var obj = serializer.Deserialize(reader, atype);
                                    target.SetObjectSetting(xtype, (ThemeSetting)obj);
                                }
                                else
                                {
                                    int so = 0;
                                    int ac = 0;
                                    while (true)
                                    {
                                        reader.Read();
                                        if (reader.TokenType == JsonToken.StartObject)
                                            so++;
                                        if (reader.TokenType == JsonToken.EndObject)
                                        {
                                            if (so == ac)
                                                break;
                                            ac++;
                                        }
                                    }
                                }
                                reader.Read();//endobject
                            }
                        }
                        reader.Read();//endarray
                        reader.Read();//endobject
                    }
                    else
                    {
                        var prop = target.GetType().GetProperty(pn);
                        if (prop != null)
                        {
                            var obj = serializer.Deserialize(reader, prop.PropertyType);
                            reader.Read();
                            var ignore =
                                     prop.GetCustomAttributes(typeof(System.Xml.Serialization.XmlIgnoreAttribute), true).Length > 0 ||
                                     prop.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Length > 0;
                            if (!ignore)
                                prop.SetValue(target, obj, new object[] { });
                        }
                    }
                }
                return target;
            }
            return null;
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Converters.Add(new ColorConverter());
            Theme t = (Theme)value;
            var props = typeof(Theme).GetProperties(System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance);
            writer.WriteStartObject();
            foreach (var prop in props)
            {
                if (prop.CanRead && prop.CanWrite)
                {
                    var ignore =
                             prop.GetCustomAttributes(typeof(System.Xml.Serialization.XmlIgnoreAttribute), true).Length > 0 ||
                             prop.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Length > 0;
                    if (!ignore)
                    {
                        writer.WritePropertyName(prop.Name);
                        serializer.Serialize(writer, prop.GetValue(t, new object[] { }));
                    }
                }
            }

            writer.WritePropertyName("extravalues");
            writer.WriteStartArray();
            var s = t.AllThemeSettings();
            foreach (var x in s)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("type");
                writer.WriteValue(x.GetType().Name);
                writer.WriteEndObject();
                serializer.Serialize(writer, x);

            }
            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }

    public class ThemeImage : IDisposable
    {
        Image FImage;

        public ThemeImage()
        {
            FImage = null;
            ImageKey = Guid.NewGuid();
        }

        public void SetImage(Image img)
        {
            this.FImage = img;
        }

        public void Dispose()
        {
            if (FImage != null)
            {
                FImage.Dispose();
                FImage = null;
            }
        }
        [JsonIgnore]
        public Image Image { get { return FImage; } }
        [Browsable(false)]
        public Guid ImageKey { get; set; }

        public override string ToString()
        {
            if (FImage == null)
                return "[None]";
            else
            {
                return String.Format("[Image] {0}x{1}", FImage.Width, FImage.Height);
            }
        }
    }



}
