using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Xb.Net
{
    /// <summary>
    /// Http functions
    /// Http関連関数群
    /// </summary>
    /// <remarks></remarks>
    public class Http
    {
        /// <summary>
        /// Method type
        /// メソッド
        /// </summary>
        /// <remarks></remarks>
        public enum MethodType
        {
            /// <summary>
            /// GET
            /// </summary>
            /// <remarks></remarks>
            Get,

            /// <summary>
            /// POST - Normal
            /// </summary>
            /// <remarks></remarks>
            Post,

            /// <summary>
            /// PUT - Normal
            /// </summary>
            /// <remarks></remarks>
            Put,

            /// <summary>
            /// DELETE - Normal
            /// </summary>
            /// <remarks></remarks>
            Delete
        }


        /// <summary>
        /// Http-Query retry count
        /// </summary>
        private const int RetryCount = 5;


        /// <summary>
        /// Encode for http
        /// Httpクエリ時のエンコード
        /// </summary>
        /// <returns></returns>
        public static System.Text.Encoding Encode { get; set; } = System.Text.Encoding.UTF8;


#region "Instance Logic"

        /// <summary>
        /// Result WebResponse and Stream
        /// </summary>
        public class ResponseSet : IDisposable
        {
            /// <summary>
            /// WebResponse Object
            /// </summary>
            public System.Net.WebResponse Response { get; private set; }

            /// <summary>
            /// Stream Object
            /// </summary>
            public System.IO.Stream Stream { get; private set; }

            public ResponseSet(System.Net.WebResponse response, System.IO.Stream stream)
            {
                this.Response = response;
                this.Stream = stream;
            }

            public void Dispose()
            {
                this.Response?.Dispose();
                this.Response = null;

                this.Stream?.Dispose();
                this.Stream = null;
            }
        }

        /// <summary>
        /// Timeout(Second)
        /// </summary>
        public int Timeout { get; set; } = 30;

        /// <summary>
        /// Query Url
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Logic Builded Url
        /// </summary>
        public string BuildedUrl { get; private set; }

        /// <summary>
        /// Value strings on body
        /// </summary>
        public string PassingValues { get; set; }

        /// <summary>
        /// Http-Method Type
        /// </summary>
        public Xb.Net.Http.MethodType Method { get; private set; }

        /// <summary>
        /// Header Values
        /// </summary>
        public Dictionary<HttpRequestHeader, string> Headers { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="url"></param>
        /// <param name="passingValues"></param>
        /// <param name="method"></param>
        /// <param name="headers"></param>
        public Http(string url,
                    string passingValues = null,
                    Xb.Net.Http.MethodType method = Xb.Net.Http.MethodType.Get,
                    Dictionary<HttpRequestHeader, string> headers = null)
        {
            this.Url = url;
            this.PassingValues = passingValues;
            this.Method = method;
            this.Headers = headers;
        }


        /// <summary>
        /// Get WebResponse and Stream by url
        /// 渡し値URLから取得したWebResponse, Streamを返す。
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public virtual async Task<Xb.Net.Http.ResponseSet> GetResponseAsync()
        {
            //uri-encode passing-string 
            var request = await this.GetRequestAsync(true)
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
        /// Get String by url
        /// 渡し値URLから取得した文字列を返す。
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public async Task<string> GetAsync()
        {
            var responseSet = await this.GetResponseAsync();
            if (responseSet == null)
                return null;

            var reader = new System.IO.StreamReader(responseSet.Stream, Xb.Net.Http.Encode);
            string result = reader.ReadToEnd();
            reader.Dispose();
            responseSet.Dispose();

            return result;
        }


        /// <summary>
        /// Get WebRequest-Object
        /// Httpリクエストの渡し値を整形し、WebRequestオブジェクトを返す。
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GETパラメータにマルチバイト文字を含んだURL文字列、かつURI-Encodeしていないとき、
        /// 自動でUTF-8によるURI-Encodeを実行してしまう。
        /// </remarks>
        protected async Task<System.Net.WebRequest> GetRequestAsync(bool doUriEncode)
        {
            this.BuildedUrl = this.Url;

            if (string.IsNullOrEmpty(this.PassingValues))
            {
                this.PassingValues = "";
            }
            else
            {
                if (doUriEncode)
                {
                    if (System.Text.Encoding.UTF8.GetBytes(this.PassingValues).Length != this.PassingValues.Length)
                    {
                        //passing string is not ascii
                        if (Xb.Net.Http.Encode.Equals(System.Text.Encoding.UTF8))
                        {
                            this.PassingValues = Xb.Net.Http.EncodeUri(this.PassingValues);
                        }
                        else
                        {
                            //can use "System.Uri.EscapeUriString" only utf8
                            throw new ArgumentException("use uri-encoded string, multibyte without utf-8");
                        }
                    }
                }
            }

            this.BuildedUrl += (this.BuildedUrl.IndexOf("?") >= 0 
                                    ? "&" 
                                    : "?")
                            + (this.Method == Xb.Net.Http.MethodType.Get
                                && !string.IsNullOrEmpty(this.PassingValues)
                                    ? this.PassingValues + "&"
                                    : "")
                            + "noCache="
                            + DateTime.Now.ToFileTimeUtc();

            var request = System.Net.WebRequest.Create(this.BuildedUrl);

            //passing http header, add it.
            //渡し値HTTP-Headerが存在するとき、追加しておく。
            if (this.Headers != null)
            {
                try
                {
                    foreach (KeyValuePair<HttpRequestHeader, string> pair in this.Headers)
                    {
                        request.Headers[pair.Key] = pair.Value;
                    }
                }
                catch (Exception ex)
                {
                    Xb.Util.Out(ex);
                    throw;
                }
            }

            if (this.Method == Xb.Net.Http.MethodType.Get)
            {
                //GET method has params in url string, no tasks.
                //HTTP-METHOD=GETのとき
                //URLにパラメータをセットしてあるので、処理不要。
            }
            else
            {
                // Http-Method post, put, delete
                switch (this.Method)
                {
                    case Xb.Net.Http.MethodType.Put:
                        request.Method = "PUT";
                        break;
                    case Xb.Net.Http.MethodType.Delete:
                        request.Method = "DELETE";
                        break;
                    case Xb.Net.Http.MethodType.Get:
                        request.Method = "GET";
                        break;
                    default:
                        request.Method = "POST";
                        break;
                }

                request.ContentType = "application/x-www-form-urlencoded";

                if (!string.IsNullOrEmpty(this.PassingValues))
                {
                    var paramBytes = Xb.Net.Http.Encode.GetBytes(this.PassingValues);
                    var requestStream = await request.GetRequestStreamAsync();
                    requestStream.Write(paramBytes, 0, paramBytes.Length);
                    requestStream.Dispose();
                }
            }

            return request;
        }


        /// <summary>
        /// Get WebResponse from WebRequest
        /// Httpリクエストから、WebResponseを取得する。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected async Task<WebResponse> GetResponseAsync(WebRequest request)
        {
            System.Net.WebResponse response = null;

            //query repeat retry-count
            //リトライ回数分だけ、接続を試す。
            for (var i = 1; i <= Xb.Net.Http.RetryCount; i++)
            {
                //Xb.Util.Out("Xb.Net.Http.GetResponseAsync Try: " + i);
                try
                {
                    response = await request.GetResponseAsync();

                    //on success, exit for loop.
                    break;
                }
                catch (WebException we)
                {
                    //caught WebException, return Exception's WebResponse
                    Xb.Util.Out("Xb.Net.Http.GetResponseAsync: catch WebException");
                    return we.Response;
                }
                catch (TimeoutException te)
                {
                    //caught TimeoutException, throw this
                    Xb.Util.Out("Xb.Net.Http.GetResponseAsync: catch TimeoutException");
                    throw te;
                }
                catch (Exception)
                {
                    Xb.Util.Out("Xb.Net.Http.GetResponseAsync: catch OtherException");
                    if (i >= Xb.Net.Http.RetryCount)
                    {
                        request = null;
                        return null;
                    }
                }

                await Task.Delay(500 * i);
            }

            return response;
        }


        /// <summary>
        /// Get Stream from WebResponse
        /// レスポンスからStreamを取得する。
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        protected async Task<System.IO.Stream> GetResponseStreamAsync(WebResponse response)
        {
            System.IO.Stream responseStream = null;

            //do repeat retry-count
            //リトライ回数分だけ、データ取得を試す。
            for (var i = 1; i <= Xb.Net.Http.RetryCount; i++)
            {
                try
                {
                    responseStream = response.GetResponseStream();

                    //on success, exit for loop.
                    break;
                }
                catch (TimeoutException te)
                {
                    throw te;
                }
                catch (Exception)
                {
                    if (i >= Xb.Net.Http.RetryCount)
                        return null;
                }

                await Task.Delay(500 * i);
            }

            return responseStream;
        }

#endregion


#region "Static Methods"

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
        public static string GetParamString(Dictionary<string, object> values)
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

                if (!Xb.Net.Http.CanEncode(pair.Key)
                    || !Xb.Net.Http.CanEncode(pairValue))
                    throw new ArgumentException("use uri-encoded string, multibyte without utf-8");

                if (Xb.Net.Http.Encode.GetBytes(pairValue).Length != pairValue.Length)
                    pairValue = System.Uri.EscapeDataString(pairValue);

                result.Append(System.Uri.EscapeDataString(pair.Key));
                result.Append("=");
                result.Append(pairValue);

                exist = true;
            }
            return result.ToString();
        }


        /// <summary>
        /// Validate string Uri-Encodable
        /// URIエンコードが可能か否かを判定する。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool CanEncode(string value)
        {
            if (string.IsNullOrEmpty(value))
                return true;

            if (Xb.Net.Http.Encode.Equals(System.Text.Encoding.UTF8))
                return true;

            if (Xb.Net.Http.Encode.GetBytes(value).Length == value.Length)
                return true;

            return false;
        }


        /// <summary>
        /// Get Uri-Encode string
        /// URIエンコードした文字列を取得する。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string EncodeUri(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";

            if (!Xb.Net.Http.CanEncode(text))
                throw new InvalidOperationException("use only utf-8");

            return System.Uri.EscapeUriString(text);
        }


        /// <summary>
        /// Get Uri-Decode string
        /// URIデコードした文字列を取得する。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string DecodeUri(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";

            if (!Xb.Net.Http.CanEncode(text))
                throw new InvalidOperationException("use only utf-8");

            return System.Uri.UnescapeDataString(text);
        }


        /// <summary>
        /// Get File Name from url
        /// URLからファイル名箇所を切り出す。
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <remarks>
        /// thanks:
        /// http://www.atmarkit.co.jp/fdotnet/dotnettips/164getfilename/getfilename.html
        /// </remarks>
        public static string GetFilename(string url)
        {
            if (string.IsNullOrEmpty(url))
                return "";

            var result = System.IO.Path.GetFileName(url);

            if (string.IsNullOrEmpty(result))
                return "";

            return result;
        }


        /// <summary>
        /// Get Directory Name from url
        /// URLからディレクトリ名を切り出す。
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetDirectory(string url)
        {
            if (string.IsNullOrEmpty(url))
                return "";

            return System.IO.Path.GetDirectoryName(url).Replace(":\\", "://").Replace("\\", "/");
        }


        /// <summary>
        /// Validate url string
        /// 渡し値URLが文字列として正しいか否かを検証する。
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <remarks>
        /// http, https のみOK、ftp等はNG
        /// </remarks>
        public static bool IsValidUrl(string url)
        {
            //渡し値がNullのとき、NG
            if (url == null)
                return false;

            if (Xb.Str.Left(url, 7).ToLower() == "http://")
            {
                if (url.Length <= 7)
                    return false;
            }
            else if (Xb.Str.Left(url, 8).ToLower() == "https://")
            {
                if (url.Length <= 8)
                    return false;
            }
            else
            {
                return false;
            }

            return true;
        }

#endregion

    }
}
