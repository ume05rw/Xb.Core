using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xb.Net;

namespace TestsXb.Core.Net
{
    [TestClass()]
    public class HttpTests : TestsXb.Core.TestBase
    {
        public class HttpResultType
        {
            public string method;
            public Dictionary<string, string> headers;
            public string body;
            public Dictionary<string, string> passing_values;
            public string url;
            public string input_encode;
            public string output_encode;
        }

        [TestMethod()]
        public async Task GetResponseAsyncTestPartGet()
        {
            Xb.Net.Http query;
            Xb.Net.Http.ResponseSet responseSet;
            string param;
            string resjson;
            HttpResultType result;
            var headers = new Dictionary<HttpRequestHeader, string>();
            headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            headers.Add(HttpRequestHeader.AcceptLanguage, "ja,en-US;q=0.7,en;q=0.3");
            headers.Add(HttpRequestHeader.Cookie, "_ga=GA1.2.2086932222.1470845468; _gat=1");
            //headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            //headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:48.0) Gecko/20100101 Firefox/48.0");

            //単純なGET
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            param = null;
            query = new Xb.Net.Http("http://dobes.jp/tests/");
            responseSet = await query.GetResponseAsync();
            resjson = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resjson);
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            Assert.AreEqual("GET", result.method);
            Assert.AreEqual("", result.body);
            Assert.AreEqual("ASCII", result.input_encode); //渡し値がuriエンコードされるため、出力は常にASCIIになる。


            //パラメータ付きGET, UTF-8指定
            param = "encode=utf8&teststring=日本語やで";
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                param,
                Http.MethodType.Get,
                headers);
            responseSet = await query.GetResponseAsync();
            resjson = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resjson);
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            Assert.AreEqual("GET", result.method);
            Assert.AreEqual("", result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("utf8", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きGET, SJIS指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("shift_jis");
            param = "encode=sjis&teststring=" + System.Web.HttpUtility.UrlEncode("日本語やで", Xb.Net.Http.Encode);
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                param,
                Http.MethodType.Get,
                headers);
            responseSet = await query.GetResponseAsync();
            resjson = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resjson);
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            Assert.AreEqual("GET", result.method);
            Assert.AreEqual("", result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("sjis", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("SJIS", result.output_encode);


            //パラメータ付きGET, EUC-JP指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("euc-jp");
            param = "encode=eucjp&teststring=" + System.Web.HttpUtility.UrlEncode("日本語やで", Xb.Net.Http.Encode);
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                param,
                Http.MethodType.Get,
                headers);
            responseSet = await query.GetResponseAsync();
            resjson = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resjson);
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            Assert.AreEqual("GET", result.method);
            Assert.AreEqual("", result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("eucjp", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("EUC-JP", result.output_encode);
        }

        [TestMethod()]
        public async Task GetResponseAsyncPostTest()
        {
            Xb.Net.Http query;
            Xb.Net.Http.ResponseSet responseSet;
            string param;
            string resjson;
            HttpResultType result;
            var headers = new Dictionary<HttpRequestHeader, string>();
            headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            headers.Add(HttpRequestHeader.AcceptLanguage, "ja,en-US;q=0.7,en;q=0.3");
            headers.Add(HttpRequestHeader.Cookie, "_ga=GA1.2.2086932222.1470845468; _gat=1");
            //headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            //headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:48.0) Gecko/20100101 Firefox/48.0");

            //パラメータ付きPOST, UTF-8指定
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            param = "encode=utf8&teststring=" + System.Web.HttpUtility.UrlEncode("日本語やで", Xb.Net.Http.Encode);
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                param,
                Http.MethodType.Post,
                headers);
            responseSet = await query.GetResponseAsync();
            resjson = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resjson);
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);

            Assert.AreEqual("POST", result.method);
            Assert.AreEqual(param, result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("utf8", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きPOST, SJIS指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("shift_jis");
            param = "encode=sjis&teststring=" + System.Web.HttpUtility.UrlEncode("日本語やで", Xb.Net.Http.Encode);
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                param,
                Http.MethodType.Post,
                headers);
            responseSet = await query.GetResponseAsync();
            resjson = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resjson);
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            Assert.AreEqual("POST", result.method);
            Assert.AreEqual(param, result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("sjis", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("SJIS", result.output_encode);


            //パラメータ付きPOST, EUC-JP指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("euc-jp");
            param = "encode=eucjp&teststring=" + System.Web.HttpUtility.UrlEncode("日本語やで", Xb.Net.Http.Encode);
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                param,
                Http.MethodType.Post,
                headers);
            responseSet = await query.GetResponseAsync();
            resjson = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resjson);
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            Assert.AreEqual("POST", result.method);
            Assert.AreEqual(param, result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("eucjp", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("EUC-JP", result.output_encode);
        }

        [TestMethod()]
        public async Task GetResponseAsyncPutTest()
        {
            Xb.Net.Http query;
            Xb.Net.Http.ResponseSet responseSet;
            string param;
            string resjson;
            HttpResultType result;
            var headers = new Dictionary<HttpRequestHeader, string>();
            headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            headers.Add(HttpRequestHeader.AcceptLanguage, "ja,en-US;q=0.7,en;q=0.3");
            headers.Add(HttpRequestHeader.Cookie, "_ga=GA1.2.2086932222.1470845468; _gat=1");
            //headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            //headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:48.0) Gecko/20100101 Firefox/48.0");

            //パラメータ付きPUT, UTF-8指定
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            param = "encode=utf8&teststring=" + System.Web.HttpUtility.UrlEncode("日本語やで", Xb.Net.Http.Encode);
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                param,
                Http.MethodType.Put,
                headers);
            responseSet = await query.GetResponseAsync();
            resjson = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resjson);
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            Assert.AreEqual("PUT", result.method);
            Assert.AreEqual(param, result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("utf8", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きPUT, SJIS指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("shift_jis");
            param = "encode=sjis&teststring=" + System.Web.HttpUtility.UrlEncode("日本語やで", Xb.Net.Http.Encode);
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                param,
                Http.MethodType.Put,
                headers);
            responseSet = await query.GetResponseAsync();
            resjson = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resjson);
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            Assert.AreEqual("PUT", result.method);
            Assert.AreEqual(param, result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("sjis", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("SJIS", result.output_encode);


            //パラメータ付きPUT, EUC-JP指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("euc-jp");
            param = "encode=eucjp&teststring=" + System.Web.HttpUtility.UrlEncode("日本語やで", Xb.Net.Http.Encode);
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                param,
                Http.MethodType.Put,
                headers);
            responseSet = await query.GetResponseAsync();
            resjson = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resjson);
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            Assert.AreEqual("PUT", result.method);
            Assert.AreEqual(param, result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("eucjp", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("EUC-JP", result.output_encode);
        }

        [TestMethod()]
        public async Task GetResponseAsyncDeleteTest()
        {
            Xb.Net.Http query;
            Xb.Net.Http.ResponseSet responseSet;
            string param;
            string resjson;
            HttpResultType result;
            var headers = new Dictionary<HttpRequestHeader, string>();
            headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            headers.Add(HttpRequestHeader.AcceptLanguage, "ja,en-US;q=0.7,en;q=0.3");
            headers.Add(HttpRequestHeader.Cookie, "_ga=GA1.2.2086932222.1470845468; _gat=1");
            //headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            //headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:48.0) Gecko/20100101 Firefox/48.0");

            //パラメータ付きDELETE, UTF-8指定
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            param = "encode=utf8&teststring=" + System.Web.HttpUtility.UrlEncode("日本語やで", Xb.Net.Http.Encode);
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                param,
                Http.MethodType.Delete,
                headers);
            responseSet = await query.GetResponseAsync();
            resjson = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resjson);
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            Assert.AreEqual("DELETE", result.method);
            Assert.AreEqual(param, result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("utf8", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きDELETE, SJIS指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("shift_jis");
            param = "encode=sjis&teststring=" + System.Web.HttpUtility.UrlEncode("日本語やで", Xb.Net.Http.Encode);
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                param,
                Http.MethodType.Delete,
                headers);
            responseSet = await query.GetResponseAsync();
            resjson = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resjson);
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            Assert.AreEqual("DELETE", result.method);
            Assert.AreEqual(param, result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("sjis", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("SJIS", result.output_encode);


            //パラメータ付きDELETE, EUC-JP指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("euc-jp");
            param = "encode=eucjp&teststring=" + System.Web.HttpUtility.UrlEncode("日本語やで", Xb.Net.Http.Encode);
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                param,
                Http.MethodType.Delete,
                headers);
            responseSet = await query.GetResponseAsync();
            resjson = Xb.Net.Http.Encode.GetString(Xb.Byte.GetBytes(responseSet.Stream));
            this.Out(resjson);
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            Assert.AreEqual("DELETE", result.method);
            Assert.AreEqual(param, result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("eucjp", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("EUC-JP", result.output_encode);
        }


        [TestMethod()]
        public async Task QueryTimeoutTest()
        {
            string param;
            Xb.Net.Http.ResponseSet responseSet;

            //パラメータ付きDELETE, UTF-8指定
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            param = "encode=utf8&teststring=" + System.Web.HttpUtility.UrlEncode("日本語やで", Xb.Net.Http.Encode);
            var query = new Xb.Net.Http("http://dobes.jp/tests/?wait=10",
                param,
                Http.MethodType.Post);
            query.Timeout = 5;
            try
            {
                responseSet = await query.GetResponseAsync();
            }
            catch (TimeoutException)
            {
                this.Out("Timeout Ok.");
            }
            catch (Exception ex)
            {
                this.Out(string.Join("\r\n", Xb.Util.GetErrorString(ex)));
                Assert.Fail("what happened?");
            }
        }

        [TestMethod()]
        public async Task GetAsyncGetTest()
        {
            Xb.Net.Http query;
            string param;
            string resjson;
            HttpResultType result;
            var headers = new Dictionary<HttpRequestHeader, string>();
            headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            headers.Add(HttpRequestHeader.AcceptLanguage, "ja,en-US;q=0.7,en;q=0.3");
            headers.Add(HttpRequestHeader.Cookie, "_ga=GA1.2.2086932222.1470845468; _gat=1");
            //headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            //headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:48.0) Gecko/20100101 Firefox/48.0");

            //単純なGET
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            param = null;

            query = new Xb.Net.Http("http://dobes.jp/tests/",
                                        param,
                                        Http.MethodType.Get,
                                        headers);
            resjson = await query.GetAsync();
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            this.Out(resjson);
            Assert.AreEqual("GET", result.method);
            Assert.AreEqual("", result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);
            Assert.AreEqual("ASCII", result.input_encode); //渡し値がuriエンコードされるため、出力は常にASCIIになる。


            //パラメータ付きGET, UTF-8指定
            param = "encode=utf8&teststring=日本語やで";
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                                    param,
                                    Http.MethodType.Get,
                                    headers);
            resjson = await query.GetAsync();
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            this.Out(resjson);
            Assert.AreEqual("GET", result.method);
            Assert.AreEqual("", result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("utf8", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きGET, SJIS指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("shift_jis");
            param = "encode=sjis&teststring=" + System.Web.HttpUtility.UrlEncode("日本語やで", Xb.Net.Http.Encode);
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                                    param,
                                    Http.MethodType.Get,
                                    headers);
            resjson = await query.GetAsync();
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            this.Out(resjson);
            Assert.AreEqual("GET", result.method);
            Assert.AreEqual("", result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("sjis", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("SJIS", result.output_encode);


            //パラメータ付きGET, EUC-JP指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("euc-jp");
            param = "encode=eucjp&teststring=" + System.Web.HttpUtility.UrlEncode("日本語やで", Xb.Net.Http.Encode);
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                                    param,
                                    Http.MethodType.Get,
                                    headers);
            resjson = await query.GetAsync();
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            this.Out(resjson);
            Assert.AreEqual("GET", result.method);
            Assert.AreEqual("", result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("eucjp", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("EUC-JP", result.output_encode);
        }

        [TestMethod()]
        public async Task GetAsyncPostTest()
        {
            Xb.Net.Http query;
            string param;
            string resjson;
            HttpResultType result;
            var headers = new Dictionary<HttpRequestHeader, string>();
            headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            headers.Add(HttpRequestHeader.AcceptLanguage, "ja,en-US;q=0.7,en;q=0.3");
            headers.Add(HttpRequestHeader.Cookie, "_ga=GA1.2.2086932222.1470845468; _gat=1");
            //headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            //headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:48.0) Gecko/20100101 Firefox/48.0");

            //パラメータ付きPOST, UTF-8指定
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            param = "encode=utf8&teststring=" + System.Web.HttpUtility.UrlEncode("日本語やで", Xb.Net.Http.Encode);
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                                    param,
                                    Http.MethodType.Post,
                                    headers);
            resjson = await query.GetAsync();
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            this.Out(resjson);
            Assert.AreEqual("POST", result.method);
            Assert.AreEqual(param, result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("utf8", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きPOST, SJIS指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("shift_jis");
            param = "encode=sjis&teststring=" + System.Web.HttpUtility.UrlEncode("日本語やで", Xb.Net.Http.Encode);
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                                    param,
                                    Http.MethodType.Post,
                                    headers);
            resjson = await query.GetAsync();
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            this.Out(resjson);
            Assert.AreEqual("POST", result.method);
            Assert.AreEqual(param, result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("sjis", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("SJIS", result.output_encode);


            //パラメータ付きPOST, EUC-JP指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("euc-jp");
            param = "encode=eucjp&teststring=" + System.Web.HttpUtility.UrlEncode("日本語やで", Xb.Net.Http.Encode);
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                                    param,
                                    Http.MethodType.Post,
                                    headers);
            resjson = await query.GetAsync();
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            this.Out(resjson);
            Assert.AreEqual("POST", result.method);
            Assert.AreEqual(param, result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("eucjp", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("EUC-JP", result.output_encode);
        }

        [TestMethod()]
        public async Task GetAsyncPutTest()
        {
            Xb.Net.Http query;
            string param;
            string resjson;
            HttpResultType result;
            var headers = new Dictionary<HttpRequestHeader, string>();
            headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            headers.Add(HttpRequestHeader.AcceptLanguage, "ja,en-US;q=0.7,en;q=0.3");
            headers.Add(HttpRequestHeader.Cookie, "_ga=GA1.2.2086932222.1470845468; _gat=1");
            //headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            //headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:48.0) Gecko/20100101 Firefox/48.0");

            //パラメータ付きPUT, UTF-8指定
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            param = "encode=utf8&teststring=" + System.Web.HttpUtility.UrlEncode("日本語やで", Xb.Net.Http.Encode);
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                                    param,
                                    Http.MethodType.Put,
                                    headers);
            resjson = await query.GetAsync();
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            this.Out(resjson);
            Assert.AreEqual("PUT", result.method);
            Assert.AreEqual(param, result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("utf8", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きPUT, SJIS指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("shift_jis");
            param = "encode=sjis&teststring=" + System.Web.HttpUtility.UrlEncode("日本語やで", Xb.Net.Http.Encode);
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                                    param,
                                    Http.MethodType.Put,
                                    headers);
            resjson = await query.GetAsync();
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            this.Out(resjson);
            Assert.AreEqual("PUT", result.method);
            Assert.AreEqual(param, result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("sjis", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("SJIS", result.output_encode);


            //パラメータ付きPUT, EUC-JP指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("euc-jp");
            param = "encode=eucjp&teststring=" + System.Web.HttpUtility.UrlEncode("日本語やで", Xb.Net.Http.Encode);
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                                    param,
                                    Http.MethodType.Put,
                                    headers);
            resjson = await query.GetAsync();
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            this.Out(resjson);
            Assert.AreEqual("PUT", result.method);
            Assert.AreEqual(param, result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("eucjp", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("EUC-JP", result.output_encode);
        }

        [TestMethod()]
        public async Task GetAsyncDeleteTest()
        {
            Xb.Net.Http query;
            string param;
            string resjson;
            HttpResultType result;
            var headers = new Dictionary<HttpRequestHeader, string>();
            headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            headers.Add(HttpRequestHeader.AcceptLanguage, "ja,en-US;q=0.7,en;q=0.3");
            headers.Add(HttpRequestHeader.Cookie, "_ga=GA1.2.2086932222.1470845468; _gat=1");
            //headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            //headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:48.0) Gecko/20100101 Firefox/48.0");

            //パラメータ付きDELETE, UTF-8指定
            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            param = "encode=utf8&teststring=" + System.Web.HttpUtility.UrlEncode("日本語やで", Xb.Net.Http.Encode);
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                                   param,
                                   Http.MethodType.Delete,
                                   headers);
            resjson = await query.GetAsync();
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            this.Out(resjson);
            Assert.AreEqual("DELETE", result.method);
            Assert.AreEqual(param, result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("utf8", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("UTF-8", result.output_encode);


            //パラメータ付きDELETE, SJIS指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("shift_jis");
            param = "encode=sjis&teststring=" + System.Web.HttpUtility.UrlEncode("日本語やで", Xb.Net.Http.Encode);
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                                    param,
                                    Http.MethodType.Delete,
                                    headers);
            resjson = await query.GetAsync();
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            this.Out(resjson);
            Assert.AreEqual("DELETE", result.method);
            Assert.AreEqual(param, result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("sjis", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("SJIS", result.output_encode);


            //パラメータ付きDELETE, EUC-JP指定
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("euc-jp");
            param = "encode=eucjp&teststring=" + System.Web.HttpUtility.UrlEncode("日本語やで", Xb.Net.Http.Encode);
            query = new Xb.Net.Http("http://dobes.jp/tests/",
                                    param,
                                    Http.MethodType.Delete,
                                    headers);
            resjson = await query.GetAsync();
            result = JsonConvert.DeserializeObject<HttpResultType>(resjson);
            this.Out(resjson);
            Assert.AreEqual("DELETE", result.method);
            Assert.AreEqual(param, result.body);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Encoding"));
            Assert.AreEqual("gzip, deflate, br", result.headers["Accept-Encoding"]);
            Assert.IsTrue(result.headers.ContainsKey("Accept-Language"));
            Assert.AreEqual("ja,en-US;q=0.7,en;q=0.3", result.headers["Accept-Language"]);
            Assert.IsTrue(result.headers.ContainsKey("Cookie"));
            Assert.AreEqual("_ga=GA1.2.2086932222.1470845468; _gat=1", result.headers["Cookie"]);

            Assert.IsTrue(result.passing_values.ContainsKey("encode"));
            Assert.AreEqual("eucjp", result.passing_values["encode"]);
            Assert.IsTrue(result.passing_values.ContainsKey("teststring"));
            Assert.AreEqual("日本語やで", result.passing_values["teststring"]);

            Assert.AreEqual("ASCII", result.input_encode);
            Assert.AreEqual("EUC-JP", result.output_encode);
        }

        [TestMethod()]
        public void GetParamStringTest()
        {
            string result;
            var param = new Dictionary<string, object>();

            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            param.Add("text_eng", "hello");
            param.Add("test_jpn", "おちこんだりもしたけれど、私はげんきです");
            param.Add("text_mix", "なんてfucking!\"#$%&'()=-~^\\|@`[{]};+:*,<.>/?_だぜ。");
            
            result = string.Format("text_eng={0}&test_jpn={1}&text_mix={2}",
                "hello",
                "%E3%81%8A%E3%81%A1%E3%81%93%E3%82%93%E3%81%A0%E3%82%8A%E3%82%82%E3%81%97%E3%81%9F%E3%81%91%E3%82%8C%E3%81%A9%E3%80%81%E7%A7%81%E3%81%AF%E3%81%92%E3%82%93%E3%81%8D%E3%81%A7%E3%81%99",
                "%E3%81%AA%E3%82%93%E3%81%A6fucking%21%22%23%24%25%26%27%28%29%3D-~%5E%5C%7C%40%60%5B%7B%5D%7D%3B%2B%3A%2A%2C%3C.%3E%2F%3F_%E3%81%A0%E3%81%9C%E3%80%82");
            Assert.AreEqual(result, Xb.Net.Http.GetParamString(param));

            param.Clear();
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("shift_jis");
            param.Add("text_eng", "hello");
            param.Add("test_jpn", System.Web.HttpUtility.UrlEncode("おちこんだりもしたけれど、私はげんきです", Xb.Net.Http.Encode));
            param.Add("text_mix", System.Web.HttpUtility.UrlEncode("なんてfucking!\"#$%&'()=-~^\\|@`[{]};+:*,<.>/?_だぜ。", Xb.Net.Http.Encode));

            result = string.Format("text_eng={0}&test_jpn={1}&text_mix={2}",
                "hello",
                System.Web.HttpUtility.UrlEncode("おちこんだりもしたけれど、私はげんきです", Xb.Net.Http.Encode),
                System.Web.HttpUtility.UrlEncode("なんてfucking!\"#$%&'()=-~^\\|@`[{]};+:*,<.>/?_だぜ。", Xb.Net.Http.Encode));
            Assert.AreEqual(result, Xb.Net.Http.GetParamString(param));

            param.Clear();
            Xb.Net.Http.Encode = System.Text.Encoding.GetEncoding("euc-jp");
            param.Add("text_eng", "hello");
            param.Add("test_jpn", System.Web.HttpUtility.UrlEncode("おちこんだりもしたけれど、私はげんきです", Xb.Net.Http.Encode));
            param.Add("text_mix", System.Web.HttpUtility.UrlEncode("なんてfucking!\"#$%&'()=-~^\\|@`[{]};+:*,<.>/?_だぜ。", Xb.Net.Http.Encode));

            result = string.Format("text_eng={0}&test_jpn={1}&text_mix={2}",
                "hello",
                System.Web.HttpUtility.UrlEncode("おちこんだりもしたけれど、私はげんきです", Xb.Net.Http.Encode),
                System.Web.HttpUtility.UrlEncode("なんてfucking!\"#$%&'()=-~^\\|@`[{]};+:*,<.>/?_だぜ。", Xb.Net.Http.Encode));
            Assert.AreEqual(result, Xb.Net.Http.GetParamString(param));
        }

        [TestMethod()]
        public void EncodeUriTest()
        {
            string before, after;

            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            before = string.Format("text_eng={0}&test_jpn={1}&text_mix={2}",
                "hello",
                "おちこんだりもしたけれど、私はげんきです",
                "なんてfucking!\"#$%&'()=-~^\\|@`[{]};+:*,<.>/?_だぜ。");
            after = string.Format("text_eng={0}&test_jpn={1}&text_mix={2}",
                "hello",
                "%E3%81%8A%E3%81%A1%E3%81%93%E3%82%93%E3%81%A0%E3%82%8A%E3%82%82%E3%81%97%E3%81%9F%E3%81%91%E3%82%8C%E3%81%A9%E3%80%81%E7%A7%81%E3%81%AF%E3%81%92%E3%82%93%E3%81%8D%E3%81%A7%E3%81%99",
                "%E3%81%AA%E3%82%93%E3%81%A6fucking%21%22%23%24%25%26%27%28%29%3D-~%5E%5C%7C%40%60%5B%7B%5D%7D%3B%2B%3A%2A%2C%3C.%3E%2F%3F_%E3%81%A0%E3%81%9C%E3%80%82");

            //URLの基本構造を崩さずエンコードするため、厳密なエンコードとはイコールにならない。
            //エンコード対象外：「!」「(」「)」「*」「.」「?」「#」「$」「&」「@」「/」「[」「]」「+」「=」「'」「;」「:」「,」
            Assert.IsFalse(after.Equals(Xb.Net.Http.EncodeUri(before)));

            before = string.Format("text_eng={0}&test_jpn={1}&text_mix={2}",
                "hello",
                "おちこんだりもしたけれど、私はげんきです",
                "なんてfucking!\"#$%&'()=-~^\\|@`[{]};+:*,<.>/?_だぜ。");
            after = string.Format("text_eng={0}&test_jpn={1}&text_mix={2}",
                "hello",
                "%E3%81%8A%E3%81%A1%E3%81%93%E3%82%93%E3%81%A0%E3%82%8A%E3%82%82%E3%81%97%E3%81%9F%E3%81%91%E3%82%8C%E3%81%A9%E3%80%81%E7%A7%81%E3%81%AF%E3%81%92%E3%82%93%E3%81%8D%E3%81%A7%E3%81%99",
                "%E3%81%AA%E3%82%93%E3%81%A6fucking!%22#$%25&'()=-~%5E%5C%7C@%60[%7B]%7D;+:*,%3C.%3E/?_%E3%81%A0%E3%81%9C%E3%80%82");
            Assert.AreEqual(after, Xb.Net.Http.EncodeUri(before));
        }

        [TestMethod()]
        public void DecodeUriTest()
        {
            string after, before;

            Xb.Net.Http.Encode = System.Text.Encoding.UTF8;
            after = string.Format("text_eng={0}&test_jpn={1}&text_mix={2}",
                "hello",
                "おちこんだりもしたけれど、私はげんきです",
                "なんてfucking!\"#$%&'()=-~^\\|@`[{]};+:*,<.>/?_だぜ。");
            before = string.Format("text_eng={0}&test_jpn={1}&text_mix={2}",
                "hello",
                "%E3%81%8A%E3%81%A1%E3%81%93%E3%82%93%E3%81%A0%E3%82%8A%E3%82%82%E3%81%97%E3%81%9F%E3%81%91%E3%82%8C%E3%81%A9%E3%80%81%E7%A7%81%E3%81%AF%E3%81%92%E3%82%93%E3%81%8D%E3%81%A7%E3%81%99",
                "%E3%81%AA%E3%82%93%E3%81%A6fucking%21%22%23%24%25%26%27%28%29%3D-~%5E%5C%7C%40%60%5B%7B%5D%7D%3B%2B%3A%2A%2C%3C.%3E%2F%3F_%E3%81%A0%E3%81%9C%E3%80%82");
            Assert.IsTrue(after.Equals(Xb.Net.Http.DecodeUri(before)));

            after = string.Format("text_eng={0}&test_jpn={1}&text_mix={2}",
                "hello",
                "おちこんだりもしたけれど、私はげんきです",
                "なんてfucking!\"#$%&'()=-~^\\|@`[{]};+:*,<.>/?_だぜ。");
            before = string.Format("text_eng={0}&test_jpn={1}&text_mix={2}",
                "hello",
                "%E3%81%8A%E3%81%A1%E3%81%93%E3%82%93%E3%81%A0%E3%82%8A%E3%82%82%E3%81%97%E3%81%9F%E3%81%91%E3%82%8C%E3%81%A9%E3%80%81%E7%A7%81%E3%81%AF%E3%81%92%E3%82%93%E3%81%8D%E3%81%A7%E3%81%99",
                "%E3%81%AA%E3%82%93%E3%81%A6fucking!%22#$%25&'()=-~%5E%5C%7C@%60[%7B]%7D;+:*,%3C.%3E/?_%E3%81%A0%E3%81%9C%E3%80%82");
            Assert.AreEqual(after, Xb.Net.Http.DecodeUri(before));
        }

        [TestMethod()]
        public void GetFilenameTest()
        {
            Assert.AreEqual("getfilename.html", Xb.Net.Http.GetFilename("http://www.atmarkit.co.jp/fdotnet/dotnettips/164getfilename/getfilename.html"));
            Assert.AreEqual("", Xb.Net.Http.GetFilename("http://www.atmarkit.co.jp/fdotnet/dotnettips/164getfilename/"));
            Assert.AreEqual("164getfilename", Xb.Net.Http.GetFilename("http://www.atmarkit.co.jp/fdotnet/dotnettips/164getfilename"));
            Assert.AreEqual("", Xb.Net.Http.GetFilename(""));
            Assert.AreEqual("", Xb.Net.Http.GetFilename(null));
        }

        [TestMethod()]
        public void GetDirectoryTest()
        {
            Assert.AreEqual("http://www.atmarkit.co.jp/fdotnet/dotnettips/164getfilename", Xb.Net.Http.GetDirectory("http://www.atmarkit.co.jp/fdotnet/dotnettips/164getfilename/getfilename.html"));
            Assert.AreEqual("http://www.atmarkit.co.jp/fdotnet/dotnettips/164getfilename", Xb.Net.Http.GetDirectory("http://www.atmarkit.co.jp/fdotnet/dotnettips/164getfilename/"));
            Assert.AreEqual("https://www.atmarkit.co.jp/fdotnet/dotnettips", Xb.Net.Http.GetDirectory("https://www.atmarkit.co.jp/fdotnet/dotnettips/164getfilename"));
            Assert.AreEqual("", Xb.Net.Http.GetDirectory(""));
            Assert.AreEqual("", Xb.Net.Http.GetDirectory(null));
        }

        [TestMethod()]
        public void IsValidUrlTest()
        {
            Assert.IsTrue(Xb.Net.Http.IsValidUrl("http://hello.com"));
            Assert.IsTrue(Xb.Net.Http.IsValidUrl("https://hello.com"));
            Assert.IsTrue(Xb.Net.Http.IsValidUrl("HTTP://hello.com"));
            Assert.IsTrue(Xb.Net.Http.IsValidUrl("HTTPS://hello.com"));
            Assert.IsFalse(Xb.Net.Http.IsValidUrl("hello.com"));
            Assert.IsFalse(Xb.Net.Http.IsValidUrl("http://"));
            Assert.IsFalse(Xb.Net.Http.IsValidUrl("https://"));
            Assert.IsTrue(Xb.Net.Http.IsValidUrl("http://a"));
            Assert.IsTrue(Xb.Net.Http.IsValidUrl("https://1"));
        }
    }
}
