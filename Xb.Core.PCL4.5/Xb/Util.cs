using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Xb
{
    public class Util
    {
        /// <summary>
        /// Output message to console
        /// コンソールにメッセージを出力する。
        /// </summary>
        /// <param name="message"></param>
        public static void Out(string message)
        {
            Debug.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")}: {message}");
        }

        /// <summary>
        /// Output formatted-message to console
        /// コンソールに整形済みメッセージを出力する。
        /// </summary>
        /// <param name="format"></param>
        /// <param name="values"></param>
        public static void Out(string format, params object[] values)
        {
            Xb.Util.Out(string.Format(format, values));
        }

        /// <summary>
        /// Output Exception-Info to console
        /// コンソールに例外情報を出力する。
        /// </summary>
        /// <param name="ex"></param>
        public static void Out(Exception ex)
        {
            Xb.Util.OutHighlighted(Xb.Util.GetErrorString(ex));
        }

        /// <summary>
        /// Output Highlighted message
        /// コンソールに、強調されたメッセージを出力する。
        /// </summary>
        /// <param name="messages"></param>

        public static void OutHighlighted(params System.String[] messages)
        {
            var time = DateTime.Now;
            var list = new List<string>();

            list.Add("");
            list.Add("");
            list.Add(time.ToString("HH:mm:ss.fff") + ":");
            list.Add("##################################################");
            list.Add("#");

            foreach (string message in messages)
            {
                var lines = message.Replace("\r\n", "\n").Replace("\r", "\n").Trim('\n').Split('\n');
                foreach (var line in lines)
                {
                    list.Add($"# {line}");
                }
            }

            list.Add("#");
            list.Add("##################################################");
            list.Add("");
            list.Add("");

            Debug.WriteLine(string.Join("\r\n", list));
        }


        /// <summary>
        /// Get Formatted Exception-Info string-array
        /// 例外情報を整形した文字列配列を返す。
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string[] GetErrorString(Exception ex)
        {
            var list = new List<string>();

            if (ex.Message != null)
            {
                list.Add(ex.Message ?? "");
                list.Add("");
            }

            if (ex.StackTrace != null)
            {
                list.AddRange(ex.StackTrace.Split(new string[] { "場所", "at " },
                                                  StringSplitOptions.None)
                                           .AsEnumerable()
                                           .Select(row => "\r\nat " + row));
            }

            if (ex.InnerException != null)
            {
                //InnerExceptionを再帰取得する。
                list.Add("");
                list.Add("Inner Exception");
                list.AddRange(Xb.Util.GetErrorString(ex.InnerException));
            }

            return list.ToArray();
        }
    }
}
