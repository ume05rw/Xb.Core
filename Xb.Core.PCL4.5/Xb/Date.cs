using System;

namespace Xb
{
    /// <summary>
    /// Date, Time functions
    /// 日付・時刻関数群
    /// </summary>
    /// <remarks></remarks>
    public class Date
    {
        /// <summary>
        /// Unix-Time start point
        /// UNIX時間の開始日時
        /// </summary>
        private static readonly DateTime UnixtimeStartValue 
            = new DateTime(1970, 1, 1, 0, 0, 0, 0);


        /// <summary>
        /// Get DateTime from String
        /// 文字列日時値から日付型値を取得する。
        /// </summary>
        /// <param name="dateTimeString">null -> now</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime GetDate(string dateTimeString = null)
        {
            //use system date if passing null
            if (dateTimeString == null)
                return DateTime.Now;

            DateTime tmp;
            if (!System.DateTime.TryParse(dateTimeString, out tmp))
                throw new ArgumentException("Cast Failure: " + dateTimeString);

            return tmp;
        }


        /// <summary>
        /// Get DateTime from Unix-Time integer
        /// UNIXタイム値から日付型値を取得する。
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        /// <remarks>
        /// auto add offset local-timezone
        /// </remarks>
        public static DateTime GetDate(long unixTime, bool isMilliSec = false)
        {
            var time = isMilliSec
                            ? TimeSpan.FromMilliseconds(unixTime)
                            : TimeSpan.FromSeconds(unixTime);

            time += System.TimeZoneInfo.Local.BaseUtcOffset;
            
            return Xb.Date.UnixtimeStartValue.Add(time);
        }


        /// <summary>
        /// Get Unix-Time integer
        /// 渡し値日時のUnixタイムを取得する。
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static long GetUnixtime(DateTime dateTime, bool isMilliSec = false)
        {
            dateTime = dateTime.ToUniversalTime();
            var time = dateTime - Xb.Date.UnixtimeStartValue;

            return (long)Math.Floor(isMilliSec
                                        ? time.TotalMilliseconds
                                        : time.TotalSeconds);
        }


        /// <summary>
        /// Get last-date in month
        /// 渡し値日付の月末日DateTimeを返す。
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime GetLastDate(DateTime dateTime)
        {
            return DateTime.Parse(dateTime.ToString("yyyy-MM-01")).AddMonths(1).AddDays(-1);
        }


        /// <summary>
        /// Get last-date in month
        /// 渡し値日付の月末日DateTimeを返す。
        /// </summary>
        /// <param name="yyyy"></param>
        /// <param name="mm"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime GetLastDate(string yyyy, string mm)
        {
            DateTime tmp;
            if (!DateTime.TryParse(yyyy + "-" + mm + "-01", out tmp))
                throw new ArgumentException("datetime parse failure");

            return tmp.AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Get last-date in month
        /// 渡し値日付の月末日DateTimeを返す。
        /// </summary>
        /// <param name="yyyy"></param>
        /// <param name="mm"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime GetLastDate(int yyyy, int mm)
        {
            DateTime tmp;
            if (!DateTime.TryParse(yyyy.ToString() + "-" + mm.ToString() + "-01", out tmp))
                throw new ArgumentException("datetime parse failure");

            return tmp.AddMonths(1).AddDays(-1);
        }


        /// <summary>
        /// Get last-day integer in month
        /// 渡し値日付の月末日数値を返す。
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int GetLastDay(DateTime dateTime)
        {
            return Xb.Date.GetLastDate(dateTime).Day;
        }


        /// <summary>
        /// Get last-day integer in month
        /// 渡し値日付の月末日数値を返す。
        /// </summary>
        /// <param name="yyyy"></param>
        /// <param name="mm"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int GetLastDay(string yyyy, string mm)
        {
            return Xb.Date.GetLastDate(yyyy, mm).Day;
        }


        /// <summary>
        /// Get last-day integer in month
        /// 渡し値日付の月末日数値を返す。
        /// </summary>
        /// <param name="yyyy"></param>
        /// <param name="mm"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int GetLastDay(int yyyy, int mm)
        {
            return Xb.Date.GetLastDate(yyyy, mm).Day;
        }

        /// <summary>
        /// Validate DateTime-String
        /// 日付型に変換可能か否かを検証する。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsDate(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            DateTime tmp;
            return DateTime.TryParse(value, out tmp);
        }


        /// <summary>
        /// Validate Time-String
        /// 文字列が時刻フォーマット[XX:XX(:XX.XXX)]か否かを検証する。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsTime(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            DateTime tmp;
            return DateTime.TryParse("2000-01-01 " + value, out tmp);
        }


        /// <summary>
        /// Get Database-DateTime String
        /// 日付型値から、汎用DBフォーマットの日時文字列を取得する。
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string FormatDb(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }


        /// <summary>
        /// Format DateTime String
        /// 日時文字列のフォーマットを整える。
        /// </summary>
        /// <param name="dateString"></param>
        /// <param name="dateSplitter"></param>
        /// <param name="timeSplitter"></param>
        /// <param name="spacer"></param>
        /// <returns></returns>
        /// <remarks>
        /// i forgot base logic url...
        /// </remarks>
        public static string FormatString(string dateString, 
                                          string dateSplitter = "-", 
                                          string timeSplitter = ":", 
                                          string spacer = " ")
        {
            if (dateString == null) dateString = "";
            if (dateSplitter == null) dateSplitter = "-";
            if (timeSplitter == null) timeSplitter = ":";
            if (spacer == null) spacer = " ";

            var block = dateString.Split(new string[] { spacer }, StringSplitOptions.None);

            if (block.Length <= 1)
            {
                Array.Resize(ref block, 2);
                block[1] = "00:00:00";
            }

            var elmsDate = block[0].Split(new string[] { dateSplitter }, StringSplitOptions.None);
            var elmsTime = block[1].Split(new string[] { timeSplitter }, StringSplitOptions.None);
            var intsDate = new int[3];
            var intsTime = new int[3];

            if (elmsDate.Length == 1)
            {
                var newElmsDate = new string[3];
                newElmsDate[0] = DateTime.Now.Year.ToString();
                newElmsDate[1] = DateTime.Now.Month.ToString();
                newElmsDate[2] = elmsDate[0];
                elmsDate = newElmsDate;
            }
            else if (elmsDate.Length == 2)
            {
                var newElmsDate = new string[3];
                newElmsDate[0] = DateTime.Now.Year.ToString();
                newElmsDate[1] = elmsDate[0];
                newElmsDate[2] = elmsDate[1];
                elmsDate = newElmsDate;
            }

            if (elmsTime.Length != 3)
            {
                Array.Resize(ref elmsTime, 3);
            }

            for (var i = 0; i <= 2; i++)
            {
                int ival;
                intsDate[i] = 0;
                if (int.TryParse(elmsDate[i], out ival))
                {
                    intsDate[i] = ival;
                }

                intsTime[i] = 0;
                if (int.TryParse(elmsTime[i], out ival))
                {
                    intsTime[i] = ival;
                }
            }

            // year elmsDate[0]
            if (intsDate[0] < 99) intsDate[0] += (int)(Math.Floor((double)(DateTime.Now.Year / 1000)) * 1000);
            elmsDate[0] = intsDate[0].ToString().PadLeft(4, '0');

            // month elmsDate[1]
            if (1 > intsDate[1] | intsDate[1] > 12) intsDate[1] = 1;
            elmsDate[1] = intsDate[1].ToString().PadLeft(2, '0');

            // day elmsDate[2]
            if (1 > intsDate[2] | intsDate[2] > 31) intsDate[2] = 1;
            elmsDate[2] = intsDate[2].ToString().PadLeft(2, '0');

            // hour elmsTime[0]
            if (1 > intsTime[0] | intsTime[0] > 23) intsTime[0] = 0;
            elmsTime[0] = intsTime[0].ToString().PadLeft(2, '0');

            // minute elmsTime[1]
            if (1 > intsTime[1] | intsTime[1] > 59) intsTime[1] = 0;
            elmsTime[1] = intsTime[1].ToString().PadLeft(2, '0');

            // second elmsTime[2]
            if (1 > intsTime[2] | intsTime[2] > 59) intsTime[2] = 0;
            elmsTime[2] = intsTime[2].ToString().PadLeft(2, '0');

            return string.Join(dateSplitter, elmsDate) 
                   + spacer 
                   + string.Join(timeSplitter, elmsTime);
        }


        /// <summary>
        /// Get Update-Time sring
        /// 更新日時表現の文字列を取得する。
        /// </summary>
        /// <param name="targetDateTime"></param>
        /// <param name="afterDateTime">null -> Now</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetTimestampString(DateTime targetDateTime, DateTime? afterDateTime = null)
        {
            if (afterDateTime == null)
                afterDateTime = DateTime.Now;

            var time = Convert.ToInt64(((DateTime)afterDateTime - targetDateTime).TotalMilliseconds);

            if (time < 120000)
            {
                // within 2min
                return "just now";
            }
            else if (time < 3600000)
            {
                // within 1hour
                return Math.Floor((double)(time / 60000)).ToString() + " minutes ago";
            }
            else if (time < 86400000)
            {
                // within 1day
                return Math.Floor((double)(time / 3600000)).ToString() + " hours ago";
            }

            //TODO: wants "days ago"?

            return targetDateTime.ToString("yyyy.MM.dd");
        }
    }
}
