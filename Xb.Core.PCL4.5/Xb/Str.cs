using System;
using System.Collections;
using System.Collections.Generic;

namespace Xb
{
    /// <summary>
    /// String functions
    /// 文字列関連関数群
    /// </summary>
    /// <remarks></remarks>
    public class Str
    {
        /// <summary>
        /// Double-Quote
        /// </summary>
        private const string Dq = "\"";


        /// <summary>
        /// Line-Feed charactor type
        /// 改行コード指定種
        /// </summary>
        public enum LinefeedType
        {
            /// <summary>
            /// LF - Unix
            /// </summary>
            /// <remarks></remarks>
            Lf,

            /// <summary>
            /// CR + LF - Windows
            /// </summary>
            /// <remarks></remarks>
            CrLf,

            /// <summary>
            /// CR - Mac
            /// </summary>
            /// <remarks></remarks>
            Cr
        }

        
        /// <summary>
        /// Get substring left side
        /// 左から指定位置までの文字列を切り出す、もしくは指定位置までを削除する。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="length">
        /// plus: slice left char
        /// 値が正のとき 左 から切り出した文字列
        /// 
        /// minus: cut left char
        /// 値が負のとき 左 から切り取った結果残った文字列
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// target = "ABCDEFG", position = 1  -> "A"
        /// target = "ABCDEFG", position = 3  -> "ABC"
        /// target = "ABCDEFG", position = -2 -> "CDEFG"
        /// </remarks>
        public static string Left(string target, int length)
        {
            if (string.IsNullOrEmpty(target))
                return target;

            if (length == 0)
                return "";

            if (target.Length <= System.Math.Abs(length))
            {
                return (length > 0)
                        ? target
                        : "";
            }

            return (length > 0)
                        ? target.Substring(0, length)
                        : target.Substring(System.Math.Abs(length));
        }


        /// <summary>
        /// Get substring right side
        /// 右から指定位置までの文字列を切り出す、もしくは指定位置までを削除する。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="length">
        /// plus: slice right char
        /// 値が正のとき 右 から切り出した文字列
        /// 
        /// minus: cut right char
        /// 値が負のときは 右 から切り取った結果残った文字列
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// target = "ABCDEFG", position = 1  -> "G"
        /// target = "ABCDEFG", position = 3  -> "EFG"
        /// target = "ABCDEFG", position = -2 -> "ABCDE"
        /// </remarks>
        public static string Right(string target, int length)
        {
            if (string.IsNullOrEmpty(target))
                return target;

            if (length == 0)
                return "";

            if (target.Length <= System.Math.Abs(length))
            {
                return (length > 0)
                            ? target
                            : "";
            }

            return (length > 0)
                        ? target.Substring(target.Length - length)
                        : target.Substring(0, target.Length - System.Math.Abs(length));
        }


        /// <summary>
        /// Get sliced substring
        /// 文字列を、先頭/末尾から指定文字数数分切り出す。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="length">
        /// plus: slice left char
        /// 値が正のとき 左 から切り出した文字列
        /// 
        /// minus: slice right char
        /// 値が正のとき 右 から切り出した文字列
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// target = "ABCDEFG", position = 1  -> "A"
        /// target = "ABCDEFG", position = 3  -> "ABC"
        /// target = "ABCDEFG", position = -2 -> "FG"
        /// </remarks>
        public static string Slice(string target, int length)
        {
            if (string.IsNullOrEmpty(target))
                return target;

            if (length == 0)
                return "";

            if (target.Length <= System.Math.Abs(length))
                return target;

            return (length > 0)
                        ? target.Substring(0, length)
                        : target.Substring(target.Length - System.Math.Abs(length));
        }


        /// <summary>
        /// Get cutted substring
        /// 文字列を先頭/末尾から文字数単位で取り除き、余った文字列を返す。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="length">
        /// plus: cut left char
        /// 値が正のとき 左 から切り出した結果残った文字列
        /// 
        /// minus: cut right char
        /// 値が負のとき 右 から切り出した結果残った文字列
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// target = "ABCDEFG", position = 1  -> "BCDEFG"
        /// target = "ABCDEFG", position = 3  -> "DEFG"
        /// target = "ABCDEFG", position = -2 -> "ABCDE"
        /// </remarks>
        public static string SliceReverse(string target, int length)
        {
            if (string.IsNullOrEmpty(target))
                return target;

            if (length == 0)
                return target;

            if (target.Length <= System.Math.Abs(length))
                return "";

            return (length > 0)
                        ? target.Substring(length)
                        : target.Substring(0, target.Length - System.Math.Abs(length));
        }


        /// <summary>
        /// Get sliced string block left side
        /// 左から指定要素までの文字列を切り出す、もしくは指定位置までを削除する。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="length">
        /// plus: slice left block
        /// 値が正のとき 左 から切り出した要素文字列
        /// 
        /// minus: cut left block
        /// 値が負のときは 左 から切り出した結果残った要素文字列
        /// </param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        /// <remarks>
        /// target = "1/2/3/4",   index = 2   -> "1/2"
        /// target = "1/2/3/4/5", index = 3   -> "1/2/3"
        /// target = "AAaa/bb Bb/cCcCcCc/DdDDdDd", index = -1 -> "bb Bb/cCcCcCc/DdDDdDd"
        /// target = "AAaa/bb Bb/cCcCcCc/DdDDdDd", index = -3 -> "DdDDdDd"
        /// </remarks>
        public static string LeftSentence(string target, int length, string delimiter = "/")
        {
            if (string.IsNullOrEmpty(target))
                return target;

            if (length == 0)
                return "";

            if (delimiter == null)
                return target;

            var texts = new List<string>();
            texts.AddRange(target.Split(new string[] {delimiter}, StringSplitOptions.None));

            //分割した用素数より位置指定数値が大きいとき
            if (texts.Count <= System.Math.Abs(length))
            {
                return (length > 0)
                            ? target
                            : "";
            }

            return (length > 0)
                        ? string.Join(delimiter, texts.GetRange(0, length).ToArray())
                        : string.Join(delimiter, texts.GetRange(System.Math.Abs(length), 
                                                                texts.Count - System.Math.Abs(length)).ToArray());
        }


        /// <summary>
        /// Get sliced string block right side
        /// 右から指定要素までの文字列を切り出す、もしくは指定要素までを削除する。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="length">
        /// plus: slice right block
        /// 値が正のときは 右 から切り出した要素文字列
        /// 
        /// minus: cut right block
        /// 値が負のときは 右 から切り出した結果残った要素文字列
        /// </param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        /// <remarks>
        /// target = "1/2/3/4",   index = 2   -> "3/4"
        /// target = "1/2/3/4/5", index = 3   -> "3/4/5"
        /// target = "AAaa/bb Bb/cCcCcCc/DdDDdDd", index = -1 -> "AAaa/bb Bb/cCcCcCc"
        /// target = "AAaa/bb Bb/cCcCcCc/DdDDdDd", index = -3 -> "AAaa"
        /// </remarks>
        public static string RightSentence(string target, int length, string delimiter = "/")
        {
            if (string.IsNullOrEmpty(target))
                return target;

            if (length == 0)
                return "";

            if (delimiter == null)
                return target;

            var texts = new List<string>();
            texts.AddRange(target.Split(new string[] {delimiter}, StringSplitOptions.None));

            //分割した用素数より位置指定数値が大きいとき
            if (texts.Count <= System.Math.Abs(length))
            {
                return (length > 0)
                            ? target
                            : "";
            }

            return (length > 0)
                        ? string.Join(delimiter, texts.GetRange(texts.Count - length, length).ToArray())
                        : string.Join(delimiter, texts.GetRange(0, texts.Count - System.Math.Abs(length)).ToArray());
        }


        /// <summary>
        /// Get sliced string block
        /// デリミタで区切られた要素文字列を、先頭/末尾から要素数単位で切り出す。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="length">
        /// plus: slice left block
        /// 値が正のとき 左 から切り出した要素文字列
        /// 
        /// plus: slice right block
        /// 値が負のとき 右 から切り出した要素文字列
        /// </param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        /// <remarks>
        /// target = "1/2/3/4",   index = 2   -> "1/2"
        /// target = "1/2/3/4/5", index = 3   -> "1/2/3"
        /// target = "AAaa/bb Bb/cCcCcCc/DdDDdDd", index = -1 -> "DdDDdDd"
        /// target = "AAaa/bb Bb/cCcCcCc/DdDDdDd", index = -3 -> "bb Bb/cCcCcCc/DdDDdDd"
        /// </remarks>
        public static string SliceSentence(string target, int length, string delimiter = "/")
        {
            if (string.IsNullOrEmpty(target))
                return target;

            if (length == 0)
                return "";

            if (delimiter == null)
                return target;

            var texts = new List<string>();
            texts.AddRange(target.Split(new string[] {delimiter}, StringSplitOptions.None));

            //分割した用素数より位置指定数値が大きいとき、全ての文字列を返す。
            if (texts.Count <= System.Math.Abs(length))
                return target;

            return (length > 0)
                        ? string.Join(delimiter, texts.GetRange(0, length).ToArray())
                        : string.Join(delimiter, texts.GetRange(texts.Count - System.Math.Abs(length), System.Math.Abs(length)).ToArray());
        }


        /// <summary>
        /// Get cutted string block
        /// 文字列を先頭/末尾から要素数単位で取り除き、余った要素の文字列を返す。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="length">
        /// plus: cut left block
        /// 値が正のときは 左 から切り出した結果残った要素文字列
        /// 
        /// minus: cut right block
        /// 値が負のとき 右 から切り出した結果残った要素文字列
        /// </param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        /// <remarks>
        /// target = "1/2/3/4",   index = 2   -> "3/4"
        /// target = "1/2/3/4/5", index = 3   -> "4/5"
        /// target = "AAaa/bb Bb/cCcCcCc/DdDDdDd", index = -1 -> "AAaa/bb Bb/cCcCcCc"
        /// target = "AAaa/bb Bb/cCcCcCc/DdDDdDd", index = -3 -> "AAaa"
        /// </remarks>
        public static string SliceReverseSentence(string target, int length, string delimiter = "/")
        {
            if (string.IsNullOrEmpty(target))
                return target;

            if (length == 0)
                return target;

            if (delimiter == null)
                return "";

            var texts = new List<string>();
            texts.AddRange(target.Split(new string[] {delimiter}, StringSplitOptions.None));

            //分割した用素数より位置指定数値が大きいとき、全ての文字列を返す。
            if (texts.Count <= System.Math.Abs(length))
                return "";

            return (length > 0)
                        ? string.Join(delimiter, texts.GetRange(length, texts.Count - length).ToArray())
                        : string.Join(delimiter, texts.GetRange(0, texts.Count + length).ToArray());
        }


        /// <summary>
        /// Get Splitted String
        /// 文字列を分割する。
        /// </summary>
        /// <param name="delimiter"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string[] Split(string target, string delimiter = " ")
        {
            if (string.IsNullOrEmpty(target))
                return new string[] {};

            if (string.IsNullOrEmpty(delimiter))
                return new string[] { target };

            return target.Split(new string[] { delimiter }, StringSplitOptions.None);
        }


        /// <summary>
        /// Validate string has only Single-Byte charactors
        /// 文字列が全てASCIIコードか否か
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsAscii(string value)
        {
            if (string.IsNullOrEmpty(value))
                return true;

            return (System.Text.Encoding.UTF8.GetByteCount(value) == value.Length);
        }

        /// <summary>
        /// Validate string only Multi-Byte charactors
        /// 文字列が全てマルチバイトか否か
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encode">null -> utf-8</param>
        /// <returns>
        /// 半角カナを1バイトにするときは、ShiftJISで判定すること
        /// </returns>
        public static bool IsMultiByte(string value, System.Text.Encoding encode = null)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            if (encode == null)
                encode = System.Text.Encoding.UTF8;

            foreach (char c in value)
            {
                if (encode.GetByteCount(c.ToString()) <= 1)
                    return false;
            }

            return true;
        }


        /// <summary>
        /// Get Linefeed charactor
        /// 改行文字を取得する。
        /// </summary>
        /// <param name="linefeed"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetLinefeed(Xb.Str.LinefeedType linefeed = Xb.Str.LinefeedType.CrLf)
        {
            switch (linefeed)
            {
                case Xb.Str.LinefeedType.Lf:
                    return "\n";
                case Xb.Str.LinefeedType.CrLf:
                    return "\r\n";
                case Xb.Str.LinefeedType.Cr:
                    return "\r";
                default:
                    return "\r\n";
            }
        }


        /// <summary>
        /// Get Linefeed-Splitted multiline strings
        /// 改行文字を判定して改行処理を通した文字列配列を取得する。
        /// </summary>
        /// <param name="multiLineText"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string[] GetMultiLine(string multiLineText)
        {
            if (string.IsNullOrEmpty(multiLineText))
                return new string[] {};

            string[] result;
            if (multiLineText.IndexOf("\r\n", StringComparison.Ordinal) != -1)
            {
                result = multiLineText.Split(new string[] {"\r\n"}, StringSplitOptions.None);
            }
            else if (multiLineText.IndexOf('\n') != -1)
            {
                result = multiLineText.Split(new string[] { "\n" }, StringSplitOptions.None);
            }
            else if (multiLineText.IndexOf('\r') != -1)
            {
                result = multiLineText.Split(new string[] { "\r" }, StringSplitOptions.None);
            }
            else
            {
                return new string[] { multiLineText };
            }

            if (result.Length > 1 
                && string.IsNullOrEmpty(result[result.Length - 1]))
            {
                Array.Resize(ref result, result.Length - 1);
            }

            return result;
        }


        /// <summary>
        /// Get Byte-Array from string
        /// 文字列からバイト配列を取得する。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="encode">null to utf-8</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static System.Byte[] GetBytes(string target, System.Text.Encoding encode = null)
        {
            if (target == null)
                return new System.Byte[] {};

            if (encode == null)
                encode = System.Text.Encoding.UTF8;

            return encode.GetBytes(target);
        }


        /// <summary>
        /// Get Byte-Length from string
        /// 文字列のバイト長を取得する。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="encode">ull to utf-8</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int GetByteLength(string target, System.Text.Encoding encode = null)
        {
            return GetBytes(target, encode).Length;
        }


        /// <summary>
        /// Escape Html-Special-Charactors
        /// HTML用特殊文字をエスケープする。
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        /// <remarks>
        /// Like: PHP - htmlspecialchars(html, ENT_COMPAT)
        /// </remarks>
        public static string EscapeHtml(string html)
        {
            if (html == null)
                html = "";

            return html.Replace("&", "&amp;")
                       .Replace(Xb.Str.Dq, "&quot;")
                       .Replace("<", "&lt;")
                       .Replace(">", "&gt;");
        }


        /// <summary>
        /// Unescape Html-Special-Charactors
        /// エスケープされたHTML特殊文字を戻す。
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string UnescapeHtml(string html)
        {
            if (html == null)
                html = "";

            return html.Replace("&amp;", "&")
                       .Replace("&quot;", Xb.Str.Dq)
                       .Replace("&lt;", "<")
                       .Replace("&gt;", ">");
        }


        /// <summary>
        /// Quote string value, and Escape for MySql
        /// 文字列をシングルクォートで囲む。文字列中にシングルクォートがある場合、MySQL式のエスケープを行う。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string MySqlQuote(string text)
        {
            if (text == null)
                text = "";

            return "'" + text.Replace("\\", "\\\\").Replace("'", "\\'") + "'";
        }


        /// <summary>
        /// Quote string value, and Escape for Microsoft Sql Server
        /// 文字列をシングルクォートで囲む。文字列中にシングルクォートがある場合、SQL-Server/SQLite式のエスケープを行う。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string SqlQuote(string text)
        {
            if (text == null)
                text = "";

            return "'" + text.Replace("'", "''") + "'";
        }


        /// <summary>
        /// Double-Quote string value, and Escape for JSON
        /// 文字列をダブルクォートで囲む。文字列中にダブルクォートがある場合、JSON式エスケープを行う。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Dquote(string text)
        {
            if (text == null)
                text = "";

            return Xb.Str.Dq + text.Replace(Xb.Str.Dq, "\\" + Xb.Str.Dq) + Xb.Str.Dq;
        }


        /// <summary>
        /// Double-Quote string value, and Escape for CSV
        /// 文字列をダブルクォートで囲む。文字列中にダブルクォートがある場合、CSV式エスケープを行う。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string CsvDquote(string text)
        {
            if (text == null)
                text = "";

            return Xb.Str.Dq 
                    + text.Replace(Xb.Str.Dq, Xb.Str.Dq + Xb.Str.Dq).Replace(",", "\\,") 
                    + Xb.Str.Dq;
        }


        /// <summary>
        /// Get string from Byte-Array, auto detect Japanese-Encode
        /// バイト配列を文字列に変換する。
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        /// <remarks>
        /// エンコードが分かっているときは、Encoding.GetString()を使うこと。
        /// </remarks>
        public static string GetString(byte[] bytes)
        {
            if (bytes == null)
                return "";

            return Xb.Str.GetEncode(bytes).GetString(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Get string from Stream, auto detect Japanese-Encode
        /// バイト配列を文字列に変換する。
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <remarks>
        /// エンコードが分かっているときは、Encoding.GetString()を使うこと。
        /// </remarks>
        public static string GetString(System.IO.Stream stream)
        {
            if (stream == null)
                return "";
            var bytes = Xb.Byte.GetBytes(stream);
            return Xb.Str.GetEncode(bytes).GetString(bytes, 0, bytes.Length);
        }


        /// <summary>
        /// Detect Encode from Byte-Array(for Japanese)
        /// 文字コードを判定する
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="forceJapaneseDetection"></param>
        /// <returns></returns>
        /// <remarks>
        /// not found, return ASCII.
        /// 
        /// thanks:
        /// http://dobon.net/vb/dotnet/string/detectcode.html
        /// 
        /// Jcode.pmのgetcodeメソッドを移植したものです。
        /// Jcode.pm(http://openlab.ring.gr.jp/Jcode/index-j.html)
        /// Jcode.pmのCopyright: Copyright 1999-2005 Dan Kogai
        /// </remarks>
        public static System.Text.Encoding GetEncode(byte[] bytes, bool forceJapaneseDetection = false)
        {
            const byte bEscape = 0x1b;
            const byte bAt = 0x40;
            const byte bDollar = 0x24;
            const byte bAnd = 0x26;
            const byte bOpen = 0x28;
            const byte bB = 0x42;
            const byte bD = 0x44;
            const byte bJ = 0x4a;
            const byte bI = 0x49;

            int len = bytes.Length;
            byte b1 = 0;
            byte b2 = 0;
            byte b3 = 0;
            byte b4 = 0;

            //Encode::is_utf8 は無視

            bool isBinary = false;
            for (var i = 0; i <= len - 1; i++)
            {
                b1 = bytes[i];
                if (b1 <= 0x6 || b1 == 0x7f || b1 == 0xff)
                {
                    //'binary'
                    isBinary = true;
                    if (b1 == 0x0 && i < len - 1 && bytes[i + 1] <= 0x7f)
                    {
                        //smells like raw unicode
                        return System.Text.Encoding.Unicode;
                    }
                }
            }
            if (isBinary && forceJapaneseDetection == false)
            {
                return System.Text.Encoding.GetEncoding("us-ascii");
            }

            //not Japanese
            var notJapanese = true;
            for (var i = 0; i <= len - 1; i++)
            {
                b1 = bytes[i];
                if (b1 == bEscape || 0x80 <= b1)
                {
                    notJapanese = false;
                    break;
                }
            }
            if (notJapanese && forceJapaneseDetection == false)
            {
                return System.Text.Encoding.GetEncoding("us-ascii");
            }

            for (var i = 0; i <= len - 3; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                b3 = bytes[i + 2];

                if (b1 == bEscape)
                {
                    if (b2 == bDollar 
                        && b3 == bAt)
                    {
                        //JIS_0208 1978
                        //JIS
                        return System.Text.Encoding.GetEncoding("iso-2022-jp");
                    }
                    else if (b2 == bDollar 
                             && b3 == bB)
                    {
                        //JIS_0208 1983
                        //JIS
                        return System.Text.Encoding.GetEncoding("iso-2022-jp");
                    }
                    else if (b2 == bOpen 
                             && (b3 == bB || b3 == bJ))
                    {
                        //JIS_ASC
                        //JIS
                        return System.Text.Encoding.GetEncoding("iso-2022-jp");
                    }
                    else if (b2 == bOpen 
                             && b3 == bI)
                    {
                        //JIS_KANA
                        //JIS
                        return System.Text.Encoding.GetEncoding("iso-2022-jp");
                    }
                    if (i < len - 3)
                    {
                        b4 = bytes[i + 3];
                        if (b2 == bDollar 
                            && b3 == bOpen 
                            && b4 == bD)
                        {
                            //JIS_0212
                            //JIS
                            return System.Text.Encoding.GetEncoding("iso-2022-jp");
                        }
                        if (i < len - 5 
                            && b2 == bAnd 
                            && b3 == bAt 
                            && b4 == bEscape 
                            && bytes[i + 4] == bDollar 
                            && bytes[i + 5] == bB)
                        {
                            //JIS_0208 1990
                            //JIS
                            return System.Text.Encoding.GetEncoding("iso-2022-jp");
                        }
                    }
                }
            }

            //should be euc|sjis|utf8
            //use of (?:) by Hiroki Ohzaki <ohzaki@iod.ricoh.co.jp>
            int sjis = 0;
            int euc = 0;
            int utf8 = 0;
            for (var i = 0; i <= len - 2; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                if (
                    (
                        (0x81 <= b1 && b1 <= 0x9f) 
                        || (0xe0 <= b1 && b1 <= 0xfc)
                    )
                    && ( (0x40 <= b2 && b2 <= 0x7e) 
                         || (0x80 <= b2 && b2 <= 0xfc))
                    )
                {
                    //SJIS_C
                    sjis += 2;
                    i += 1;
                }
            }
            for (var i = 0; i <= len - 2; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                if (
                    (
                        (0xa1 <= b1 && b1 <= 0xfe) 
                        && (0xa1 <= b2 && b2 <= 0xfe)
                    ) 
                    || (
                        b1 == 0x8e 
                        && (0xa1 <= b2 && b2 <= 0xdf)
                    )
                )
                {
                    //EUC_C
                    //EUC_KANA
                    euc += 2;
                    i += 1;
                }
                else if (i < len - 2)
                {
                    b3 = bytes[i + 2];
                    if (b1 == 0x8f 
                        && (0xa1 <= b2 && b2 <= 0xfe) 
                        && (0xa1 <= b3 && b3 <= 0xfe))
                    {
                        //EUC_0212
                        euc += 3;
                        i += 2;
                    }
                }
            }
            for (var i = 0; i <= len - 2; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                if ((0xc0 <= b1 && b1 <= 0xdf) 
                    && (0x80 <= b2 && b2 <= 0xbf))
                {
                    //UTF8
                    utf8 += 2;
                    i += 1;
                }
                else if (i < len - 2)
                {
                    b3 = bytes[i + 2];
                    if ((0xe0 <= b1 && b1 <= 0xef) 
                        && (0x80 <= b2 && b2 <= 0xbf) 
                        && (0x80 <= b3 && b3 <= 0xbf))
                    {
                        //UTF8
                        utf8 += 3;
                        i += 2;
                    }
                }
            }
            //M. Takahashi's suggestion
            //utf8 += utf8 / 2;

            if (euc > sjis && euc > utf8)
            {
                //EUC
                return System.Text.Encoding.GetEncoding("euc-jp");
            }
            else if (sjis > euc && sjis > utf8)
            {
                //SJIS
                return System.Text.Encoding.GetEncoding("shift_jis");
            }
            else if (utf8 > euc && utf8 > sjis)
            {
                //UTF8
                return System.Text.Encoding.UTF8;
            }

            return System.Text.Encoding.GetEncoding("us-ascii");
        }

        /// <summary>
        /// Detect Encode from Byte-Array(for Japanese)
        /// 文字コードを判定する
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="forceJapaneseDetection"></param>
        /// <returns></returns>
        /// <remarks>
        /// not found, return ASCII.
        /// 
        /// thanks:
        /// http://dobon.net/vb/dotnet/string/detectcode.html
        /// 
        /// Jcode.pmのgetcodeメソッドを移植したものです。
        /// Jcode.pm(http://openlab.ring.gr.jp/Jcode/index-j.html)
        /// Jcode.pmのCopyright: Copyright 1999-2005 Dan Kogai
        /// </remarks>
        public static System.Text.Encoding GetEncode(System.IO.Stream stream, bool forceJapaneseDetection = false)
        {
            return Xb.Str.GetEncode(Xb.Byte.GetBytes(stream), forceJapaneseDetection);
        }
    }
}
