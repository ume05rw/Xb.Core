using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xb.Net;


namespace TestsXb.Core.Net
{
    [System.Xml.Serialization.XmlRoot("HttpResultType")]
    public class HttpResultType
    {
        [System.Xml.Serialization.XmlElement("method")]
        public string method { get; set; }

        [System.Xml.Serialization.XmlElement("headers")]
        public string headers { get; set; }

        [System.Xml.Serialization.XmlElement("body")]
        public string body { get; set; }

        [System.Xml.Serialization.XmlElement("passing_values")]
        public string passing_values { get; set; }

        [System.Xml.Serialization.XmlElement("url")]
        public string url { get; set; }

        [System.Xml.Serialization.XmlElement("input_encode")]
        public string input_encode { get; set; }

        [System.Xml.Serialization.XmlElement("output_encode")]
        public string output_encode { get; set; }
    }




    [TestClass()]
    public class HttpXmlTest : TestsXb.Core.TestBase
    {
        /// <summary>
        /// 文字列を二つのデリミタで区切り、Dictionary型で返す。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks>
        /// XmlSerializer.Deserializeで Dictionary 要素を得る方法がわからないため、
        /// デリミタで結合した文字列を戻り値にしている。
        /// </remarks>
        public Dictionary<string, string> GetDictionary(string value)
        {
            var result = new Dictionary<string, string>();
            var rows = value.Split(new string[] { "-=-=DLMT2=-=-" }, StringSplitOptions.None);
            foreach (var row in rows)
            {
                var elems = row.Split(new string[] { "-=-=DLMT1=-=-" }, StringSplitOptions.None);
                if (elems.Count() == 2)
                {
                    result.Add(elems[0], elems[1]);
                }
            }

            return result;
        }

        [TestMethod()]
        public async Task GetResponseAsyncTestPartGet()
        {
            Xb.Net.HttpXml query;
            Xb.Net.Http.ResponseSet responseSet;
            Dictionary<string, object> param = new Dictionary<string, object>();
            string resText;
            HttpResultType result;
            XmlSerializer serializer;
            XmlReader xmlReader;
            Dictionary<string, string> resHeaders;
            Dictionary<string, string> resPassingValues;
            var headers = new Dictionary<HttpRequestHeader, string>();
            headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            headers.Add(HttpRequestHeader.AcceptLanguage, "ja,en-US;q=0.7,en;q=0.3");
            headers.Add(HttpRequestHeader.Cookie, "_ga=GA1.2.2086932222.1470845468; _gat=1");
            

            //単純なGET
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php", null, Http.MethodType.Get);
            responseSet = await query.GetResponseAsync();
            resText = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resText);

            serializer = new XmlSerializer(typeof(HttpResultType));
            xmlReader = XmlReader.Create(new StringReader(resText));
            result = (HttpResultType)serializer.Deserialize(xmlReader);
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("GET", result.method);
            Assert.AreEqual("", result.body);
            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きGET, UTF-8指定
            param.Clear();
            param.Add("encode", "utf8");
            param.Add("teststring", "日本語やで");
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php",
                param,
                Http.MethodType.Get,
                headers);
            responseSet = await query.GetResponseAsync();
            resText = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resText);

            serializer = new XmlSerializer(typeof(HttpResultType));
            xmlReader = XmlReader.Create(new StringReader(resText));
            result = (HttpResultType)serializer.Deserialize(xmlReader);
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("GET", result.method);
            Assert.AreEqual("", result.body);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("utf8", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きGET, SJIS指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("shift_jis");
            param.Clear();
            param.Add("encode", "sjis");
            param.Add("teststring", "日本語やで");
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php",
                param,
                Http.MethodType.Get,
                headers);
            responseSet = await query.GetResponseAsync();
            resText = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resText);

            serializer = new XmlSerializer(typeof(HttpResultType));
            xmlReader = XmlReader.Create(new StringReader(resText));
            result = (HttpResultType)serializer.Deserialize(xmlReader);
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("GET", result.method);
            Assert.AreEqual("", result.body);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("sjis", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("SJIS", result.output_encode);


            //パラメータ付きGET, EUC-JP指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("euc-jp");
            param.Clear();
            param.Add("encode", "eucjp");
            param.Add("teststring", "日本語やで");
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php",
                param,
                Http.MethodType.Get,
                headers);
            responseSet = await query.GetResponseAsync();
            resText = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resText);

            serializer = new XmlSerializer(typeof(HttpResultType));
            xmlReader = XmlReader.Create(new StringReader(resText));
            result = (HttpResultType)serializer.Deserialize(xmlReader);
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("GET", result.method);
            Assert.AreEqual("", result.body);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("eucjp", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("EUC-JP", result.output_encode);
        }

        [TestMethod()]
        public async Task GetResponseAsyncTestPartPost()
        {
            Xb.Net.HttpXml query;
            Xb.Net.Http.ResponseSet responseSet;
            Dictionary<string, object> param = new Dictionary<string, object>();
            string resText;
            HttpResultType result;
            XmlSerializer serializer;
            XmlReader xmlReader;
            Dictionary<string, string> resHeaders;
            Dictionary<string, string> resPassingValues;
            var headers = new Dictionary<HttpRequestHeader, string>();
            headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            headers.Add(HttpRequestHeader.AcceptLanguage, "ja,en-US;q=0.7,en;q=0.3");
            headers.Add(HttpRequestHeader.Cookie, "_ga=GA1.2.2086932222.1470845468; _gat=1");


            //単純なGET
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php");
            responseSet = await query.GetResponseAsync();
            resText = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resText);

            serializer = new XmlSerializer(typeof(HttpResultType));
            xmlReader = XmlReader.Create(new StringReader(resText));
            result = (HttpResultType)serializer.Deserialize(xmlReader);
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("POST", result.method);
            Assert.AreEqual("", result.body);
            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きGET, UTF-8指定
            param.Clear();
            param.Add("encode", "utf8");
            param.Add("teststring", "日本語やで");
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php",
                param,
                Http.MethodType.Post,
                headers);
            responseSet = await query.GetResponseAsync();
            resText = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resText);

            serializer = new XmlSerializer(typeof(HttpResultType));
            xmlReader = XmlReader.Create(new StringReader(resText));
            result = (HttpResultType)serializer.Deserialize(xmlReader);
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("POST", result.method);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("utf8", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("UTF-8", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きGET, SJIS指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("shift_jis");
            param.Clear();
            param.Add("encode", "sjis");
            param.Add("teststring", "日本語やで");
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php",
                param,
                Http.MethodType.Post,
                headers);
            responseSet = await query.GetResponseAsync();
            resText = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resText);

            serializer = new XmlSerializer(typeof(HttpResultType));
            xmlReader = XmlReader.Create(new StringReader(resText));
            result = (HttpResultType)serializer.Deserialize(xmlReader);
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("POST", result.method);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("sjis", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("SJIS", result.input_encode);
            Assert.AreEqual("SJIS", result.output_encode);


            //パラメータ付きGET, EUC-JP指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("euc-jp");
            param.Clear();
            param.Add("encode", "eucjp");
            param.Add("teststring", "日本語やで");
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php",
                param,
                Http.MethodType.Post,
                headers);
            responseSet = await query.GetResponseAsync();
            resText = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resText);

            serializer = new XmlSerializer(typeof(HttpResultType));
            xmlReader = XmlReader.Create(new StringReader(resText));
            result = (HttpResultType)serializer.Deserialize(xmlReader);
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("POST", result.method);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("eucjp", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("EUC-JP", result.input_encode);
            Assert.AreEqual("EUC-JP", result.output_encode);
        }

        [TestMethod()]
        public async Task GetResponseAsyncTestPartPut()
        {
            Xb.Net.HttpXml query;
            Xb.Net.Http.ResponseSet responseSet;
            Dictionary<string, object> param = new Dictionary<string, object>();
            string resText;
            HttpResultType result;
            XmlSerializer serializer;
            XmlReader xmlReader;
            Dictionary<string, string> resHeaders;
            Dictionary<string, string> resPassingValues;
            var headers = new Dictionary<HttpRequestHeader, string>();
            headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            headers.Add(HttpRequestHeader.AcceptLanguage, "ja,en-US;q=0.7,en;q=0.3");
            headers.Add(HttpRequestHeader.Cookie, "_ga=GA1.2.2086932222.1470845468; _gat=1");


            //単純なGET
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php", null, Http.MethodType.Put);
            responseSet = await query.GetResponseAsync();
            resText = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resText);

            serializer = new XmlSerializer(typeof(HttpResultType));
            xmlReader = XmlReader.Create(new StringReader(resText));
            result = (HttpResultType)serializer.Deserialize(xmlReader);
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("PUT", result.method);
            Assert.AreEqual("", result.body);
            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きGET, UTF-8指定
            param.Clear();
            param.Add("encode", "utf8");
            param.Add("teststring", "日本語やで");
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php",
                param,
                Http.MethodType.Put,
                headers);
            responseSet = await query.GetResponseAsync();
            resText = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resText);

            serializer = new XmlSerializer(typeof(HttpResultType));
            xmlReader = XmlReader.Create(new StringReader(resText));
            result = (HttpResultType)serializer.Deserialize(xmlReader);
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("PUT", result.method);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("utf8", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("UTF-8", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きGET, SJIS指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("shift_jis");
            param.Clear();
            param.Add("encode", "sjis");
            param.Add("teststring", "日本語やで");
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php",
                param,
                Http.MethodType.Put,
                headers);
            responseSet = await query.GetResponseAsync();
            resText = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resText);

            serializer = new XmlSerializer(typeof(HttpResultType));
            xmlReader = XmlReader.Create(new StringReader(resText));
            result = (HttpResultType)serializer.Deserialize(xmlReader);
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("PUT", result.method);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("sjis", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("SJIS", result.input_encode);
            Assert.AreEqual("SJIS", result.output_encode);


            //パラメータ付きGET, EUC-JP指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("euc-jp");
            param.Clear();
            param.Add("encode", "eucjp");
            param.Add("teststring", "日本語やで");
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php",
                param,
                Http.MethodType.Put,
                headers);
            responseSet = await query.GetResponseAsync();
            resText = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resText);

            serializer = new XmlSerializer(typeof(HttpResultType));
            xmlReader = XmlReader.Create(new StringReader(resText));
            result = (HttpResultType)serializer.Deserialize(xmlReader);
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("PUT", result.method);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("eucjp", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("EUC-JP", result.input_encode);
            Assert.AreEqual("EUC-JP", result.output_encode);
        }


        [TestMethod()]
        public async Task GetResponseAsyncTestPartDelete()
        {
            Xb.Net.HttpXml query;
            Xb.Net.Http.ResponseSet responseSet;
            Dictionary<string, object> param = new Dictionary<string, object>();
            string resText;
            HttpResultType result;
            XmlSerializer serializer;
            XmlReader xmlReader;
            Dictionary<string, string> resHeaders;
            Dictionary<string, string> resPassingValues;
            var headers = new Dictionary<HttpRequestHeader, string>();
            headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            headers.Add(HttpRequestHeader.AcceptLanguage, "ja,en-US;q=0.7,en;q=0.3");
            headers.Add(HttpRequestHeader.Cookie, "_ga=GA1.2.2086932222.1470845468; _gat=1");


            //単純なGET
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php", null, Http.MethodType.Delete);
            responseSet = await query.GetResponseAsync();
            resText = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resText);

            serializer = new XmlSerializer(typeof(HttpResultType));
            xmlReader = XmlReader.Create(new StringReader(resText));
            result = (HttpResultType)serializer.Deserialize(xmlReader);
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("DELETE", result.method);
            Assert.AreEqual("", result.body);
            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きGET, UTF-8指定
            param.Clear();
            param.Add("encode", "utf8");
            param.Add("teststring", "日本語やで");
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php", param, Http.MethodType.Delete, headers);
            responseSet = await query.GetResponseAsync();
            resText = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resText);

            serializer = new XmlSerializer(typeof(HttpResultType));
            xmlReader = XmlReader.Create(new StringReader(resText));
            result = (HttpResultType)serializer.Deserialize(xmlReader);
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("DELETE", result.method);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("utf8", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("UTF-8", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きGET, SJIS指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("shift_jis");
            param.Clear();
            param.Add("encode", "sjis");
            param.Add("teststring", "日本語やで");
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php", param, Http.MethodType.Delete, headers);
            responseSet = await query.GetResponseAsync();
            resText = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resText);

            serializer = new XmlSerializer(typeof(HttpResultType));
            xmlReader = XmlReader.Create(new StringReader(resText));
            result = (HttpResultType)serializer.Deserialize(xmlReader);
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("DELETE", result.method);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("sjis", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("SJIS", result.input_encode);
            Assert.AreEqual("SJIS", result.output_encode);


            //パラメータ付きGET, EUC-JP指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("euc-jp");
            param.Clear();
            param.Add("encode", "eucjp");
            param.Add("teststring", "日本語やで");
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php", param, Http.MethodType.Delete, headers);
            responseSet = await query.GetResponseAsync();
            resText = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resText);

            serializer = new XmlSerializer(typeof(HttpResultType));
            xmlReader = XmlReader.Create(new StringReader(resText));
            result = (HttpResultType)serializer.Deserialize(xmlReader);
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("DELETE", result.method);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("eucjp", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("EUC-JP", result.input_encode);
            Assert.AreEqual("EUC-JP", result.output_encode);
        }


        [TestMethod()]
        public async Task GetAsyncTPartGet()
        {
            Xb.Net.HttpXml query;
            Dictionary<string, object> param = new Dictionary<string, object>();
            HttpResultType result;
            Dictionary<string, string> resHeaders;
            Dictionary<string, string> resPassingValues;
            var headers = new Dictionary<HttpRequestHeader, string>();
            headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            headers.Add(HttpRequestHeader.AcceptLanguage, "ja,en-US;q=0.7,en;q=0.3");
            headers.Add(HttpRequestHeader.Cookie, "_ga=GA1.2.2086932222.1470845468; _gat=1");


            //単純なGET
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php", null, Http.MethodType.Get);
            result = await query.GetAsync<HttpResultType>();
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("GET", result.method);
            Assert.AreEqual("", result.body);
            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きGET, UTF-8指定
            param.Clear();
            param.Add("encode", "utf8");
            param.Add("teststring", "日本語やで");
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php", param, Http.MethodType.Get, headers);
            result = await query.GetAsync<HttpResultType>();
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("GET", result.method);
            Assert.AreEqual("", result.body);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("utf8", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きGET, SJIS指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("shift_jis");
            param.Clear();
            param.Add("encode", "sjis");
            param.Add("teststring", "日本語やで");
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php", param, Http.MethodType.Get, headers);
            result = await query.GetAsync<HttpResultType>();
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("GET", result.method);
            Assert.AreEqual("", result.body);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("sjis", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("SJIS", result.output_encode);


            //パラメータ付きGET, EUC-JP指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("euc-jp");
            param.Clear();
            param.Add("encode", "eucjp");
            param.Add("teststring", "日本語やで");
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php", param, Http.MethodType.Get, headers);
            result = await query.GetAsync<HttpResultType>();
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("GET", result.method);
            Assert.AreEqual("", result.body);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("eucjp", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("EUC-JP", result.output_encode);
        }

        [TestMethod()]
        public async Task GetAsyncTPartPost()
        {
            Xb.Net.HttpXml query;
            Dictionary<string, object> param = new Dictionary<string, object>();
            HttpResultType result;
            Dictionary<string, string> resHeaders;
            Dictionary<string, string> resPassingValues;
            var headers = new Dictionary<HttpRequestHeader, string>();
            headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            headers.Add(HttpRequestHeader.AcceptLanguage, "ja,en-US;q=0.7,en;q=0.3");
            headers.Add(HttpRequestHeader.Cookie, "_ga=GA1.2.2086932222.1470845468; _gat=1");


            //単純なGET
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php");
            result = await query.GetAsync<HttpResultType>();
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("POST", result.method);
            Assert.AreEqual("", result.body);
            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きGET, UTF-8指定
            param.Clear();
            param.Add("encode", "utf8");
            param.Add("teststring", "日本語やで");
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php", param, Http.MethodType.Post, headers);
            result = await query.GetAsync<HttpResultType>();
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("POST", result.method);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("utf8", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("UTF-8", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きGET, SJIS指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("shift_jis");
            param.Clear();
            param.Add("encode", "sjis");
            param.Add("teststring", "日本語やで");
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php", param, Http.MethodType.Post, headers);
            result = await query.GetAsync<HttpResultType>();
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("POST", result.method);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("sjis", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("SJIS", result.input_encode);
            Assert.AreEqual("SJIS", result.output_encode);


            //パラメータ付きGET, EUC-JP指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("euc-jp");
            param.Clear();
            param.Add("encode", "eucjp");
            param.Add("teststring", "日本語やで");
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php", param, Http.MethodType.Post, headers);
            result = await query.GetAsync<HttpResultType>();
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("POST", result.method);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("eucjp", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("EUC-JP", result.input_encode);
            Assert.AreEqual("EUC-JP", result.output_encode);
        }

        [TestMethod()]
        public async Task GetAsyncTPartPut()
        {
            Xb.Net.HttpXml query;
            Dictionary<string, object> param = new Dictionary<string, object>();
            HttpResultType result;
            Dictionary<string, string> resHeaders;
            Dictionary<string, string> resPassingValues;
            var headers = new Dictionary<HttpRequestHeader, string>();
            headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            headers.Add(HttpRequestHeader.AcceptLanguage, "ja,en-US;q=0.7,en;q=0.3");
            headers.Add(HttpRequestHeader.Cookie, "_ga=GA1.2.2086932222.1470845468; _gat=1");


            //単純なGET
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php", null, Http.MethodType.Put);
            result = await query.GetAsync<HttpResultType>();
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("PUT", result.method);
            Assert.AreEqual("", result.body);
            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きGET, UTF-8指定
            param.Clear();
            param.Add("encode", "utf8");
            param.Add("teststring", "日本語やで");
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php", param, Http.MethodType.Put, headers);
            result = await query.GetAsync<HttpResultType>();
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("PUT", result.method);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("utf8", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("UTF-8", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きGET, SJIS指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("shift_jis");
            param.Clear();
            param.Add("encode", "sjis");
            param.Add("teststring", "日本語やで");
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php", param, Http.MethodType.Put, headers);
            result = await query.GetAsync<HttpResultType>();
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("PUT", result.method);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("sjis", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("SJIS", result.input_encode);
            Assert.AreEqual("SJIS", result.output_encode);


            //パラメータ付きGET, EUC-JP指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("euc-jp");
            param.Clear();
            param.Add("encode", "eucjp");
            param.Add("teststring", "日本語やで");
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php", param, Http.MethodType.Put, headers);
            result = await query.GetAsync<HttpResultType>();
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("PUT", result.method);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("eucjp", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("EUC-JP", result.input_encode);
            Assert.AreEqual("EUC-JP", result.output_encode);
        }

        [TestMethod()]
        public async Task GetAsyncTPartDelete()
        {
            Xb.Net.HttpXml query;
            Dictionary<string, object> param = new Dictionary<string, object>();
            HttpResultType result;
            Dictionary<string, string> resHeaders;
            Dictionary<string, string> resPassingValues;
            var headers = new Dictionary<HttpRequestHeader, string>();
            headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            headers.Add(HttpRequestHeader.AcceptLanguage, "ja,en-US;q=0.7,en;q=0.3");
            headers.Add(HttpRequestHeader.Cookie, "_ga=GA1.2.2086932222.1470845468; _gat=1");


            //単純なGET
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php", null, Http.MethodType.Delete);
            result = await query.GetAsync<HttpResultType>();
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("DELETE", result.method);
            Assert.AreEqual("", result.body);
            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きGET, UTF-8指定
            param.Clear();
            param.Add("encode", "utf8");
            param.Add("teststring", "日本語やで");
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php", param, Http.MethodType.Delete, headers);
            result = await query.GetAsync<HttpResultType>();
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("DELETE", result.method);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("utf8", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("UTF-8", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きGET, SJIS指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("shift_jis");
            param.Clear();
            param.Add("encode", "sjis");
            param.Add("teststring", "日本語やで");
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php", param, Http.MethodType.Delete, headers);
            result = await query.GetAsync<HttpResultType>();
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("DELETE", result.method);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("sjis", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("SJIS", result.input_encode);
            Assert.AreEqual("SJIS", result.output_encode);


            //パラメータ付きGET, EUC-JP指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("euc-jp");
            param.Clear();
            param.Add("encode", "eucjp");
            param.Add("teststring", "日本語やで");
            query = new Xb.Net.HttpXml("http://dobes.jp/tests/xml.php", param, Http.MethodType.Delete, headers);
            result = await query.GetAsync<HttpResultType>();
            resHeaders = this.GetDictionary(result.headers);
            resPassingValues = this.GetDictionary(result.passing_values);

            Assert.AreEqual("DELETE", result.method);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", resHeaders["Accept-Encoding"]);
            Assert.IsTrue(resHeaders.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", resHeaders["Accept-Language"]);
            Assert.IsTrue(resHeaders.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", resHeaders["Cookie"]);

            Assert.IsTrue(resPassingValues.ContainsKey("encode"));
            Assert.AreEqual("eucjp", resPassingValues["encode"]);
            Assert.IsTrue(resPassingValues.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", resPassingValues["teststring"]);

            Assert.AreEqual("EUC-JP", result.input_encode);
            Assert.AreEqual("EUC-JP", result.output_encode);
        }
    }
}
