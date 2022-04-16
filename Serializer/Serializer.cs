using System;
using System.Dynamic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Formatting = Newtonsoft.Json.Formatting;

namespace MyTools
{

    //todo serialize to/from file diectly
    public static class Serializer
    {
        public enum SerializerMode
        {
            Default,
            Xml,
            Json
        }

        public static SerializerMode DefaultMode { get; set; } = SerializerMode.Json;
        public static bool JsonMissingMemberErrors { get; set; } = false;
        public static bool Minify { get; set; } = false;


        #region xml

        /// <summary>
        /// Returns plain XML with no xml? tag and no xmlns
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        private static string XmlSerialize(this object @this)
        {
            var serializer = new XmlSerializer(@this.GetType());
            using (var stringWriter = new StringWriter())
            {

                var settings = new XmlWriterSettings { Indent = !Minify, OmitXmlDeclaration = true };

                var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                using (var writer = XmlWriter.Create(stringWriter, settings))
                {
                    serializer.Serialize(writer, @this, emptyNamespaces);
                    return stringWriter.ToString();
                }
            }
        }


        private static object XmlDeserialize(this string xml, Type type, Action<string> onError = null)
        {
            var serializer = new XmlSerializer(type);
            object result = null;
            using (var reader = new StringReader(xml))
            {
                try
                {
                    result = serializer.Deserialize(reader);
                }
                catch (Exception e)
                {
                    if (onError != null)
                    {
                        onError.Invoke(e.ToStringWithInner());
                    }
                    else throw;
                }
            }
            return result;
        }

        private static T XmlDeserialize<T>(this string xml, Action<string> onError = null)
        {
            return (T)XmlDeserialize(xml, typeof(T), onError);
        }

        #endregion xml

        #region json
        private static string JsonSerialize(this object @this)
        {
            var formatting = Minify ? Formatting.None : Formatting.Indented;
            return JsonConvert.SerializeObject(@this, formatting);
        }

        private static T JsonDeserialize<T>(this string json, Action<string> onError = null)
        {

            return JsonConvert.DeserializeObject<T>(json,
                new JsonSerializerSettings
                {
                    MissingMemberHandling = JsonMissingMemberErrors ? MissingMemberHandling.Error : MissingMemberHandling.Ignore,
                    Error = (s, e) =>
                    {
                        onError?.Invoke(e.ErrorContext.Error.Message);
                    }
                });
        }
        #endregion json

        #region Binary

        //BinaryFormatter formatter = new BinaryFormatter();
        //    using (FileStream fs = new FileStream(@"k:\ыыыыыыыыыы.bin", FileMode.OpenOrCreate))
        //{
        //    formatter.Serialize(fs, tags);
        //}




        //_todo доделать когда нибудь
        //private static string BinSerialize(this object @this)
        //{
        //    var serializer = new BinaryFormatter();
        //    using (var stringWriter = new StringWriter())
        //    {
        //        serializer.Serialize(stringWriter, @this);
        //        return stringWriter.ToString();
        //    }
        //}


        //private static object XmlDeserialize(this string xml, Type type, Action<string> onError = null)
        //{
        //    var serializer = new XmlSerializer(type);
        //    object result = null;
        //    using (var reader = new StringReader(xml))
        //    {
        //        try
        //        {
        //            result = serializer.Deserialize(reader);
        //        }
        //        catch (Exception e)
        //        {
        //            if (onError != null)
        //            {
        //                onError.Invoke(e.ToStringWithInner());
        //            }
        //            else throw;
        //        }
        //    }
        //    return result;
        //}

        //private static T XmlDeserialize<T>(this string xml, Action<string> onError = null)
        //{
        //    return (T)XmlDeserialize(xml, typeof(T), onError);
        //}



        #endregion

        #region generic
        public static T Deserialize<T>(this string objectString, SerializerMode mode = SerializerMode.Default, Action<string> onError = null)
        {
            if (mode == SerializerMode.Default) mode = DefaultMode;
            switch (mode)
            {
                case SerializerMode.Default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                case SerializerMode.Xml:
                    return XmlDeserialize<T>(objectString, onError);
                case SerializerMode.Json:
                    return JsonDeserialize<T>(objectString, onError);
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public static string Serialize(this object @this, SerializerMode mode = SerializerMode.Default)
        {
            if (mode == SerializerMode.Default) mode = DefaultMode;
            switch (mode)
            {
                case SerializerMode.Default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                case SerializerMode.Xml:
                    return XmlSerialize(@this);
                case SerializerMode.Json:
                    return JsonSerialize(@this);
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
        #endregion generic


        #region dyn

        //SEE Repos\paulnsk\_Related\SerializerDynSamples.cs 

        public static string DynSerialize(object o, string xmlroot = null)
        {
            return DynSerialize(o, DefaultMode, xmlroot);
        }


        /// <summary>
        /// Supports serializing a dynamic object
        /// </summary>
        /// <param name="o"></param>
        /// <param name="mode"></param>
        /// <param name="xmlroot">If an object has more than 1 top-level field this must be specified</param>
        /// <returns></returns>
        public static string DynSerialize(object o, SerializerMode mode, string xmlroot = null)
        {
            if (mode == SerializerMode.Default) mode = DefaultMode;


            var json = JsonConvert.SerializeObject(o);
            if (mode == SerializerMode.Json) return json;

            var xmldoc = JsonConvert.DeserializeXmlNode(json, xmlroot);

            //beautifying (could simply do return xmldoc.OuterXml;)
            var sb = new StringBuilder();
            var xmlWriterSettings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };
            using (var writer = XmlWriter.Create(sb, xmlWriterSettings))
            {
                xmldoc.Save(writer);
            }

            return sb.ToString();
        }

        public static dynamic DynDeserialize(this string objectString, SerializerMode mode = SerializerMode.Default)
        {
            if (mode == SerializerMode.Default) mode = DefaultMode;
            //todo onError?
            switch (mode)
            {
                case SerializerMode.Default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                case SerializerMode.Xml:

                    var doc = XDocument.Parse(objectString); //or XDocument.Load(path)
                    var jsonText = JsonConvert.SerializeXNode(doc);
                    dynamic dyn = JObject.Parse(jsonText); //or JsonConvert.DeserializeObject<ExpandoObject>(jsonText);

                    //DO NOT DELETE: this converts JObject to ExpandoObject, may need in the future
                    //var expConverter = new ExpandoObjectConverter();
                    //dynamic dyn = JsonConvert.DeserializeObject<ExpandoObject>(jsonText, expConverter);

                    return dyn;
                case SerializerMode.Json:
                    return JObject.Parse(objectString);
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }





        }

        #endregion dyn

    }

}
