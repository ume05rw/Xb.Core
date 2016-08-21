using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsXb.Core
{
    [TestClass()]
    public class DateTests
    {
        [TestMethod()]
        public void GetDateTest()
        {
            var now = DateTime.Now;
            var xbNow = Xb.Date.GetDate();

            Assert.AreEqual(now.Year, xbNow.Year);
            Assert.AreEqual(now.Month, xbNow.Month);
            Assert.AreEqual(now.Day, xbNow.Day);
            Assert.AreEqual(now.Hour, xbNow.Hour);
            Assert.AreEqual(now.Minute, xbNow.Minute);
            Assert.AreEqual(now.Second, xbNow.Second);

            var dateTimeString = "1917-03-12 22:59:05";
            var dateTime = DateTime.Parse(dateTimeString);
            var xbDate = Xb.Date.GetDate(dateTimeString);

            Assert.AreEqual(dateTime.Year, xbDate.Year);
            Assert.AreEqual(dateTime.Month, xbDate.Month);
            Assert.AreEqual(dateTime.Day, xbDate.Day);
            Assert.AreEqual(dateTime.Hour, xbDate.Hour);
            Assert.AreEqual(dateTime.Minute, xbDate.Minute);
            Assert.AreEqual(dateTime.Second, xbDate.Second);
        }

        [TestMethod()]
        public void GetDateTest2()
        {
            var dateTimeString = "2016-08-08 15:11:54";
            var dateTime = DateTime.Parse(dateTimeString);

            //milli sec
            var unixTime = 1470636714000;
            Assert.AreEqual(dateTime, Xb.Date.GetDate(unixTime, true));

            //sec
            unixTime = 1470636714;
            Assert.AreEqual(dateTime, Xb.Date.GetDate(unixTime, false));

            //sec
            unixTime = 1470636714;
            Assert.AreEqual(dateTime, Xb.Date.GetDate(unixTime));
        }

        [TestMethod()]
        public void GetUnixtimeTest()
        {
            var dateTimeString = "2016-08-08 15:11:54";
            var dateTime = DateTime.Parse(dateTimeString);

            //milli sec
            var unixTime = 1470636714000;
            Assert.AreEqual(unixTime, Xb.Date.GetUnixtime(dateTime, true));

            //sec
            unixTime = 1470636714;
            Assert.AreEqual(unixTime, Xb.Date.GetUnixtime(dateTime, false));

            //sec
            unixTime = 1470636714;
            Assert.AreEqual(unixTime, Xb.Date.GetUnixtime(dateTime));
        }

        [TestMethod()]
        public void GetLastDateTest()
        {
            var dateTime = DateTime.Parse("2016-08-08 15:11:54");
            var lastDate = DateTime.Parse("2016-08-31 00:00:00");
            Assert.AreEqual(lastDate, Xb.Date.GetLastDate(dateTime));

            dateTime = DateTime.Parse("2016-09-01 12:34:56");
            lastDate = DateTime.Parse("2016-09-30 00:00:00");
            Assert.AreEqual(lastDate, Xb.Date.GetLastDate(dateTime));

            dateTime = DateTime.Parse("2016-09-30 12:34:56");
            lastDate = DateTime.Parse("2016-09-30 00:00:00");
            Assert.AreEqual(lastDate, Xb.Date.GetLastDate(dateTime));

            dateTime = DateTime.Parse("2016-08-30 00:00:00");
            lastDate = DateTime.Parse("2016-08-31 00:00:00");
            Assert.AreEqual(lastDate, Xb.Date.GetLastDate(dateTime));

            dateTime = DateTime.Parse("2016-08-31 12:34:56");
            lastDate = DateTime.Parse("2016-08-31 00:00:00");
            Assert.AreEqual(lastDate, Xb.Date.GetLastDate(dateTime));
        }

        [TestMethod()]
        public void GetLastDateTest2()
        {
            var dateTime = DateTime.Parse("2016-08-08 15:11:54");
            var lastDate = DateTime.Parse("2016-08-31 00:00:00");
            Assert.AreEqual(lastDate, Xb.Date.GetLastDate(dateTime.Year.ToString(), dateTime.Month.ToString()));

            dateTime = DateTime.Parse("2016-09-01 12:34:56");
            lastDate = DateTime.Parse("2016-09-30 00:00:00");
            Assert.AreEqual(lastDate, Xb.Date.GetLastDate(dateTime.Year.ToString(), dateTime.Month.ToString()));

            dateTime = DateTime.Parse("2016-09-30 12:34:56");
            lastDate = DateTime.Parse("2016-09-30 00:00:00");
            Assert.AreEqual(lastDate, Xb.Date.GetLastDate(dateTime.Year.ToString(), dateTime.Month.ToString()));

            dateTime = DateTime.Parse("2016-08-30 00:00:00");
            lastDate = DateTime.Parse("2016-08-31 00:00:00");
            Assert.AreEqual(lastDate, Xb.Date.GetLastDate(dateTime.Year.ToString(), dateTime.Month.ToString()));

            dateTime = DateTime.Parse("2016-08-31 12:34:56");
            lastDate = DateTime.Parse("2016-08-31 00:00:00");
            Assert.AreEqual(lastDate, Xb.Date.GetLastDate(dateTime.Year.ToString(), dateTime.Month.ToString()));
        }

        [TestMethod()]
        public void GetLastDateTest3()
        {
            var dateTime = DateTime.Parse("2016-08-08 15:11:54");
            var lastDate = DateTime.Parse("2016-08-31 00:00:00");
            Assert.AreEqual(lastDate, Xb.Date.GetLastDate(dateTime.Year, dateTime.Month));

            dateTime = DateTime.Parse("2016-09-01 12:34:56");
            lastDate = DateTime.Parse("2016-09-30 00:00:00");
            Assert.AreEqual(lastDate, Xb.Date.GetLastDate(dateTime.Year, dateTime.Month));

            dateTime = DateTime.Parse("2016-09-30 12:34:56");
            lastDate = DateTime.Parse("2016-09-30 00:00:00");
            Assert.AreEqual(lastDate, Xb.Date.GetLastDate(dateTime.Year, dateTime.Month));

            dateTime = DateTime.Parse("2016-08-30 00:00:00");
            lastDate = DateTime.Parse("2016-08-31 00:00:00");
            Assert.AreEqual(lastDate, Xb.Date.GetLastDate(dateTime.Year, dateTime.Month));

            dateTime = DateTime.Parse("2016-08-31 12:34:56");
            lastDate = DateTime.Parse("2016-08-31 00:00:00");
            Assert.AreEqual(lastDate, Xb.Date.GetLastDate(dateTime.Year, dateTime.Month));
        }

        [TestMethod()]
        public void GetLastDayTest4()
        {
            var dateTime = DateTime.Parse("2016-08-08 15:11:54");
            var lastDate = DateTime.Parse("2016-08-31 00:00:00");
            Assert.AreEqual(lastDate.Day, Xb.Date.GetLastDay(dateTime));

            dateTime = DateTime.Parse("2016-09-01 12:34:56");
            lastDate = DateTime.Parse("2016-09-30 00:00:00");
            Assert.AreEqual(lastDate.Day, Xb.Date.GetLastDay(dateTime));

            dateTime = DateTime.Parse("2016-09-30 12:34:56");
            lastDate = DateTime.Parse("2016-09-30 00:00:00");
            Assert.AreEqual(lastDate.Day, Xb.Date.GetLastDay(dateTime));

            dateTime = DateTime.Parse("2016-08-30 00:00:00");
            lastDate = DateTime.Parse("2016-08-31 00:00:00");
            Assert.AreEqual(lastDate.Day, Xb.Date.GetLastDay(dateTime));

            dateTime = DateTime.Parse("2016-08-31 12:34:56");
            lastDate = DateTime.Parse("2016-08-31 00:00:00");
            Assert.AreEqual(lastDate.Day, Xb.Date.GetLastDay(dateTime));
        }

        [TestMethod()]
        public void GetLastDayTest5()
        {
            var dateTime = DateTime.Parse("2016-08-08 15:11:54");
            var lastDate = DateTime.Parse("2016-08-31 00:00:00");
            Assert.AreEqual(lastDate.Day, Xb.Date.GetLastDay(dateTime.Year.ToString(), dateTime.Month.ToString()));

            dateTime = DateTime.Parse("2016-09-01 12:34:56");
            lastDate = DateTime.Parse("2016-09-30 00:00:00");
            Assert.AreEqual(lastDate.Day, Xb.Date.GetLastDay(dateTime.Year.ToString(), dateTime.Month.ToString()));

            dateTime = DateTime.Parse("2016-09-30 12:34:56");
            lastDate = DateTime.Parse("2016-09-30 00:00:00");
            Assert.AreEqual(lastDate.Day, Xb.Date.GetLastDay(dateTime.Year.ToString(), dateTime.Month.ToString()));

            dateTime = DateTime.Parse("2016-08-30 00:00:00");
            lastDate = DateTime.Parse("2016-08-31 00:00:00");
            Assert.AreEqual(lastDate.Day, Xb.Date.GetLastDay(dateTime.Year.ToString(), dateTime.Month.ToString()));

            dateTime = DateTime.Parse("2016-08-31 12:34:56");
            lastDate = DateTime.Parse("2016-08-31 00:00:00");
            Assert.AreEqual(lastDate.Day, Xb.Date.GetLastDay(dateTime.Year.ToString(), dateTime.Month.ToString()));
        }

        [TestMethod()]
        public void GetLastDayTest6()
        {
            var dateTime = DateTime.Parse("2016-08-08 15:11:54");
            var lastDate = DateTime.Parse("2016-08-31 00:00:00");
            Assert.AreEqual(lastDate.Day, Xb.Date.GetLastDay(dateTime.Year, dateTime.Month));

            dateTime = DateTime.Parse("2016-09-01 12:34:56");
            lastDate = DateTime.Parse("2016-09-30 00:00:00");
            Assert.AreEqual(lastDate.Day, Xb.Date.GetLastDay(dateTime.Year, dateTime.Month));

            dateTime = DateTime.Parse("2016-09-30 12:34:56");
            lastDate = DateTime.Parse("2016-09-30 00:00:00");
            Assert.AreEqual(lastDate.Day, Xb.Date.GetLastDay(dateTime.Year, dateTime.Month));

            dateTime = DateTime.Parse("2016-08-30 00:00:00");
            lastDate = DateTime.Parse("2016-08-31 00:00:00");
            Assert.AreEqual(lastDate.Day, Xb.Date.GetLastDay(dateTime.Year, dateTime.Month));

            dateTime = DateTime.Parse("2016-08-31 12:34:56");
            lastDate = DateTime.Parse("2016-08-31 00:00:00");
            Assert.AreEqual(lastDate.Day, Xb.Date.GetLastDay(dateTime.Year, dateTime.Month));
        }

        [TestMethod()]
        public void IsDateTest()
        {
            Assert.IsTrue(Xb.Date.IsDate("1/1"));
            Assert.IsFalse(Xb.Date.IsDate("8/32"));
            Assert.IsFalse(Xb.Date.IsDate("1"));
            Assert.IsTrue(Xb.Date.IsDate("2016-09-30 12:34:56"));
            Assert.IsFalse(Xb.Date.IsDate("2016-09-30 12-34-56"));
            Assert.IsTrue(Xb.Date.IsDate("2016-09-30"));
            Assert.IsTrue(Xb.Date.IsDate("2016/09/30"));
        }

        [TestMethod()]
        public void IsTimeTest()
        {
            Assert.IsTrue(Xb.Date.IsTime("1:1"));
            Assert.IsFalse(Xb.Date.IsTime("1-1"));
            Assert.IsFalse(Xb.Date.IsTime("1"));
            Assert.IsTrue(Xb.Date.IsTime("13:59:59"));
            Assert.IsFalse(Xb.Date.IsTime("24:60:60"));
        }

        [TestMethod()]
        public void FormatDbTest()
        {
            var dateTimeString = "2016-08-08 15:11:54";
            var dateTime = DateTime.Parse(dateTimeString);
            Assert.AreEqual(dateTimeString, Xb.Date.FormatDb(dateTime));
        }

        [TestMethod()]
        public void FormatStringTest()
        {
            var year = DateTime.Now.Year.ToString().PadLeft(4, '0');
            var month = DateTime.Now.Month.ToString().PadLeft(2, '0');

            Assert.AreEqual($"{year}-{month}-08 00:00:00", Xb.Date.FormatString("8"));
            Assert.AreEqual($"{year}-08-08 00:00:00", Xb.Date.FormatString("8-8"));
            Assert.AreEqual("2016-08-08 15:11:00", Xb.Date.FormatString("08-08 15:11"));
            Assert.AreEqual("2016-08-08 15:00:00", Xb.Date.FormatString("16-8-8 15"));
            Assert.AreEqual("2016-08-08 15:11:54", Xb.Date.FormatString("2016-08-08 15:11:54"));
            Assert.AreEqual("2016／08／08～15―11―54", Xb.Date.FormatString("2016／08／08～15―11―54", "／", "―", "～"));
        }
        
        [TestMethod()]
        public void GetTimestampTest()
        {
            var now = DateTime.Now;
            Assert.AreEqual("just now", Xb.Date.GetTimestampString(now.AddSeconds(100)));
            Assert.AreEqual("just now", Xb.Date.GetTimestampString(now.AddSeconds(-119)));
            Assert.AreEqual("2 minutes ago", Xb.Date.GetTimestampString(now.AddSeconds(-121)));
            Assert.AreEqual("59 minutes ago", Xb.Date.GetTimestampString(now.AddMinutes(-59)));
            Assert.AreEqual("1 hours ago", Xb.Date.GetTimestampString(now.AddMinutes(-60)));
            Assert.AreEqual("1 hours ago", Xb.Date.GetTimestampString(now.AddMinutes(-90)));
            Assert.AreEqual("1 hours ago", Xb.Date.GetTimestampString(now.AddMinutes(-119)));
            Assert.AreEqual("2 hours ago", Xb.Date.GetTimestampString(now.AddMinutes(-120)));
            Assert.AreEqual("23 hours ago", Xb.Date.GetTimestampString(now.AddHours(-23)));
            Assert.AreEqual(now.AddHours(-24).ToString("yyyy.MM.dd"), Xb.Date.GetTimestampString(now.AddHours(-24)));

            now = DateTime.Parse("2016-08-07 15:11:54");
            Assert.AreEqual("just now", Xb.Date.GetTimestampString(now.AddSeconds(100), now));
            Assert.AreEqual("just now", Xb.Date.GetTimestampString(now.AddSeconds(-119), now));
            Assert.AreEqual("2 minutes ago", Xb.Date.GetTimestampString(now.AddSeconds(-121), now));
            Assert.AreEqual("59 minutes ago", Xb.Date.GetTimestampString(now.AddMinutes(-59), now));
            Assert.AreEqual("1 hours ago", Xb.Date.GetTimestampString(now.AddMinutes(-60), now));
            Assert.AreEqual("1 hours ago", Xb.Date.GetTimestampString(now.AddMinutes(-90), now));
            Assert.AreEqual("1 hours ago", Xb.Date.GetTimestampString(now.AddMinutes(-119), now));
            Assert.AreEqual("2 hours ago", Xb.Date.GetTimestampString(now.AddMinutes(-120), now));
            Assert.AreEqual("23 hours ago", Xb.Date.GetTimestampString(now.AddHours(-23), now));
            Assert.AreEqual(now.AddHours(-24).ToString("yyyy.MM.dd"), Xb.Date.GetTimestampString(now.AddHours(-24), now));
        }
    }
}
