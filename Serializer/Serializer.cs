using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Formatting = Newtonsoft.Json.Formatting;

namespace MyTools
{
    
    public static class Serializer
    {

        #region settings
        public enum SerializerMode
        {
            Json,
            Xml
        }

        //common
        public static bool Minify { get; set; } = false;

        public static bool JsonCamelCase { get; set; } = false;
        //json
        public static bool JsonMissingMemberErrors { get; set; } = false;

        //xml
        public static bool XmlEmptyNamespaces { get; set; } = true;
        public static bool XmlOmitDeclaration { get; set; } = true;
        public static bool XmlClearAllAttributesAfterSerializing { get; set; }


        #endregion

        #region xml


        private static void XmlRemoveAllAttributes(XDocument doc)
        {
            foreach (var descendant in doc.Descendants()) descendant.RemoveAttributes();
        }


        private static string XmlRemoveAllAttributes(string xml)
        {
            var doc = XDocument.Parse(xml);
            XmlRemoveAllAttributes(doc);
            return doc.ToString();
        }


        private static string XmlSerialize(this object @this, Type[] includedTypes = null)
        {

            //  not working for included types :(
            //var overrides = new XmlAttributeOverrides();
            //foreach (var includedType in includedTypes)
            //{
            //    overrides.Add(includedType, new XmlAttributes());
            //}
            //var serializer= new XmlSerializer(@this.GetType(), overrides, includedTypes, null, string.Empty);

            var serializer = includedTypes != null ? new XmlSerializer(@this.GetType(), includedTypes) : new XmlSerializer(@this.GetType());

            using (var stringWriter = new StringWriter())
            {

                var settings = new XmlWriterSettings { Indent = !Minify, OmitXmlDeclaration = XmlOmitDeclaration };

                XmlSerializerNamespaces emptyNamespaces = null;
                if (XmlEmptyNamespaces)
                {
                    emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                }


                using (var writer = XmlWriter.Create(stringWriter, settings))
                {
                    if (XmlEmptyNamespaces) serializer.Serialize(writer, @this, emptyNamespaces);
                    else serializer.Serialize(writer, @this);

                    var s = stringWriter.ToString();
                    if (XmlClearAllAttributesAfterSerializing) s = XmlRemoveAllAttributes(s);
                    return s;
                }
            }
        }

        private static object XmlDeserialize(this string xml, Type type, Action<Exception> onError = null)
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
                        onError.Invoke(e);
                    }
                    else throw;
                }
            }
            return result;
        }

        private static T XmlDeserialize<T>(this string xml, Action<Exception> onError = null)
        {
            return (T)XmlDeserialize(xml, typeof(T), onError);
        }

        #endregion xml

        #region json
        private static string JsonSerialize(this object @this)
        {

            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new Newtonsoft.Json.Converters.StringEnumConverter() },
                Formatting = Minify ? Formatting.None : Formatting.Indented,
            };

            if (JsonCamelCase) settings.ContractResolver = new CamelCasePropertyNamesContractResolver();


                

            //var formatting = Minify ? Formatting.None : Formatting.Indented;
            //return JsonConvert.SerializeObject(@this, formatting, new Newtonsoft.Json.Converters.StringEnumConverter());
            return JsonConvert.SerializeObject(@this, settings);
        }

        private static T JsonDeserialize<T>(this string json, Action<Exception> onError = null)
        {
            return JsonConvert.DeserializeObject<T>(json,
                new JsonSerializerSettings
                {
                    MissingMemberHandling = JsonMissingMemberErrors ? MissingMemberHandling.Error : MissingMemberHandling.Ignore,
                    Error = (s, e) =>
                    {
                        onError?.Invoke(e.ErrorContext.Error);
                    }
                });
        }
        #endregion json
        
        #region generic
        public static T Deserialize<T>(this string objectString, SerializerMode mode = default, Action<Exception> onError = null)
        {
            switch (mode)
            {
                case SerializerMode.Xml:
                    return XmlDeserialize<T>(objectString, onError);
                case SerializerMode.Json:
                    return JsonDeserialize<T>(objectString, onError);
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public static string Serialize(this object @this, SerializerMode mode = default, Type[] includedTypes = null)
        {
            switch (mode)
            {
                case SerializerMode.Xml:
                    return XmlSerialize(@this, includedTypes);
                case SerializerMode.Json:
                    return JsonSerialize(@this);
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
        #endregion generic

        #region dyn

        
        /// <summary>
        /// Supports serializing a dynamic object
        /// </summary>
        /// <param name="o"></param>
        /// <param name="mode"></param>
        /// <param name="xmlroot">If an object has more than 1 top-level field this must be specified</param>
        /// <returns></returns>
        public static string DynSerialize(object o, string xmlroot = null, SerializerMode mode = default)
        {
            
            var json = JsonConvert.SerializeObject(o);
            if (mode == SerializerMode.Json) return json;

            var xmldoc = JsonConvert.DeserializeXmlNode(json, xmlroot);

            //beautifying (could simply do return xmldoc.OuterXml;)
            var sb = new StringBuilder();
            var xmlWriterSettings = new XmlWriterSettings { Indent = !Minify, OmitXmlDeclaration = true };
            using (var writer = XmlWriter.Create(sb, xmlWriterSettings))
            {
                xmldoc.Save(writer);
            }

            return sb.ToString();
        }

        public static dynamic DynDeserialize(this string objectString, SerializerMode mode = default)
        {
            //todo onError?
            switch (mode)
            {
                case SerializerMode.Xml:

                    var doc = XDocument.Parse(objectString); //or XDocument.Load(path)
                    var jsonText = JsonConvert.SerializeXNode(doc);
                    dynamic dyn = JObject.Parse(jsonText); //or JsonConvert.DeserializeObject<ExpandoObject>(jsonText);

                    //DO NOT DELETE: this converts JObject to ExpandoObject, may need in the future
                    //var expConverter = new ExpandoObjectConverter();
                    //dynamic dyn = JsonConvert.DeserializeObject<ExpandoObject>(jsonText, expConverter);

                    return dyn;
                case SerializerMode.Json:
                    //fooling the parser into thinking that the array is a single object https://stackoverflow.com/questions/34690581/error-reading-jobject-from-jsonreader-current-jsonreader-item-is-not-an-object
                    //if (objectString.StartsWith("[")) objectString = "{\"Array\":" + objectString + "}";
                    //return JObject.Parse(objectString);
                    return JToken.Parse(objectString);
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }





        }

        #endregion dyn

    }

}
