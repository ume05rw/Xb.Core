using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Xb.Net
{
    /// <summary>
    /// Http on XML functions
    /// XMLベースのHTTP関連関数群
    /// </summary>
    public class HttpXml : Xb.Net.Http
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="url"></param>
        /// <param name="passingValues"></param>
        /// <param name="method"></param>
        /// <param name="headers"></param>
        /// <remarks>
        /// thanks;
        /// http://stackoverflow.com/questions/1799767/easy-way-to-convert-a-dictionarystring-string-to-xml-and-visa-versa
        /// </remarks>
        public HttpXml(string url,
                       Dictionary<string, object> passingValues = null,
                       Xb.Net.Http.MethodType method = Xb.Net.Http.MethodType.Post,
                       Dictionary<HttpRequestHeader, string> headers = null)
            : base(url,
                   null,
                   method,
                   headers)
        {
            this.SetPassingValues(passingValues);
        }


        /// <summary>
        /// Convert object to Json-String, and Set it PassingValues
        /// 渡し値objectをJSONにキャストし、PassingValuesにセットする。
        /// </summary>
        /// <param name="passingValues"></param>
        /// <remarks>
        /// thanks:
        /// http://stackoverflow.com/questions/1799767/easy-way-to-convert-a-dictionarystring-string-to-xml-and-visa-versa
        /// </remarks>
        private void SetPassingValues(Dictionary<string, object> passingValues)
        {
            if (passingValues == null
                || passingValues.Count <= 0)
            {
                this.PassingValues = "";
                return;
            }

            if (this.Method == MethodType.Get)
            {
                this.PassingValues = Xb.Net.HttpXml.GetParamString(passingValues);
            }
            else
            {
                var builder = new StringBuilder();
                builder.AppendFormat("<?xml version=\"1.0\" encoding=\"{0}\"?><data>", Xb.Net.Http.Encode.WebName);

                foreach (KeyValuePair<string, object> pair in passingValues)
                {
                    builder.AppendFormat("<{0}>{1}</{0}>", pair.Key, pair.Value);
                }
                builder.Append("</data>");

                this.PassingValues = builder.ToString();
            }
        }

        
        /// <summary>
        /// Get WebResponse, Stream by url
        /// 渡し値URLから取得したWebResponse, Streamを返す。
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public override async Task<Xb.Net.Http.ResponseSet> GetResponseAsync()
        {
            //DO NOT uri-encode passing-string(XML)
            var request = await this.GetRequestAsync(false)
                                    .Timeout(TimeSpan.FromSeconds(this.Timeout));
            if (request == null)
                return null;

            var response = await this.GetResponseAsync(request)
                                     .Timeout(TimeSpan.FromSeconds(this.Timeout));
            if (response == null)
                return null;

            var stream = await this.GetResponseStreamAsync(response)
                                   .Timeout(TimeSpan.FromSeconds(this.Timeout));

            return new Xb.Net.Http.ResponseSet(response, stream);
        }


        /// <summary>
        /// Get Response from url, Cast response to T
        /// 指定URLから応答を取得し、指定の型にキャストして返す。
        /// </summary>
        /// <returns>Ordered-Type</returns>
        /// <remarks>
        /// thanks:
        /// http://dobon.net/vb/dotnet/file/xmlserializer.html
        /// </remarks>
        public async Task<T> GetAsync<T>()
        {
            var resSet = await this.GetResponseAsync();

            try
            {
                var serializer = new XmlSerializer(typeof(T));
                var reader = new System.IO.StreamReader(resSet.Stream, Xb.Net.Http.Encode);
                T result = (T)(serializer.Deserialize(reader));

                serializer = null;
                reader.Dispose();
                resSet.Dispose();

                return result;
            }
            catch (Exception ex)
            {
                Xb.Util.Out("Xb.Net.HttpXml.GetAsync<T>: XmlSerializer.Deserialize failure {0}", resSet);
                Xb.Util.Out(ex);
                throw ex;
            }
        }



        /// <summary>
        /// Convert Associative-Array to Http-Parameter-String
        /// 連想配列をHTTPパラメータ文字列に整形する。
        /// </summary>
        /// <param name="values"></param>
        /// <returns>LIKE: "name1=value1&name2=value2&name3=...."</returns>
        /// <remarks>
        /// Not Support Array Value.
        /// 注意：単純配列を渡すことが出来ない。List, Arrayなど。
        /// </remarks>
        public new static string GetParamString(Dictionary<string, object> values)
        {
            if (values == null)
                return "";

            bool exist = false;
            var result = new StringBuilder();
            foreach (KeyValuePair<string, object> pair in values)
            {
                if (exist)
                    result.Append("&");

                var pairValue = pair.Value == null
                                    ? "null"
                                    : pair.Value.ToString();
                
                result.Append(pair.Key);
                result.Append("=");
                result.Append(pairValue);

                exist = true;
            }
            return result.ToString();
        }
    }
}
