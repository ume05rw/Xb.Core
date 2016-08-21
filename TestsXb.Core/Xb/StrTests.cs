using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsXb.Core
{
    [TestClass()]
    public class StrTests
    {
        [TestMethod()]
        public void LeftTest()
        {
            Assert.AreEqual("A", Xb.Str.Left("ABCDEFG", 1));
            Assert.AreEqual("ABC", Xb.Str.Left("ABCDEFG", 3));
            Assert.AreEqual("CDEFG", Xb.Str.Left("ABCDEFG", -2));

            Assert.AreEqual(null, Xb.Str.Left(null, 1));
            Assert.AreEqual("", Xb.Str.Left("", 1));
            Assert.AreEqual("", Xb.Str.Left("ABCDEFG", 0));
            Assert.AreEqual("ABCDEFG", Xb.Str.Left("ABCDEFG", 7));
            Assert.AreEqual("", Xb.Str.Left("ABCDEFG", -7));
            Assert.AreEqual("ABCDEFG", Xb.Str.Left("ABCDEFG", 8));
            Assert.AreEqual("", Xb.Str.Left("ABCDEFG", -8));
        }

        [TestMethod()]
        public void RightTest()
        {
            Assert.AreEqual("G", Xb.Str.Right("ABCDEFG", 1));
            Assert.AreEqual("EFG", Xb.Str.Right("ABCDEFG", 3));
            Assert.AreEqual("ABCDE", Xb.Str.Right("ABCDEFG", -2));

            Assert.AreEqual(null, Xb.Str.Right(null, 1));
            Assert.AreEqual("", Xb.Str.Right("", 1));
            Assert.AreEqual("", Xb.Str.Right("ABCDEFG", 0));
            Assert.AreEqual("ABCDEFG", Xb.Str.Right("ABCDEFG", 7));
            Assert.AreEqual("", Xb.Str.Right("ABCDEFG", -7));
            Assert.AreEqual("ABCDEFG", Xb.Str.Right("ABCDEFG", 8));
            Assert.AreEqual("", Xb.Str.Right("ABCDEFG", -8));
        }

        [TestMethod()]
        public void SliceTest()
        {
            Assert.AreEqual("A", Xb.Str.Slice("ABCDEFG", 1));
            Assert.AreEqual("ABC", Xb.Str.Slice("ABCDEFG", 3));
            Assert.AreEqual("FG", Xb.Str.Slice("ABCDEFG", -2));

            Assert.AreEqual(null, Xb.Str.Slice(null, 1));
            Assert.AreEqual("", Xb.Str.Slice("", 1));
            Assert.AreEqual("", Xb.Str.Slice("ABCDEFG", 0));
            Assert.AreEqual("ABCDEFG", Xb.Str.Slice("ABCDEFG", 7));
            Assert.AreEqual("ABCDEFG", Xb.Str.Slice("ABCDEFG", -7));
            Assert.AreEqual("ABCDEFG", Xb.Str.Slice("ABCDEFG", 8));
            Assert.AreEqual("ABCDEFG", Xb.Str.Slice("ABCDEFG", -8));
        }

        [TestMethod()]
        public void SliceReverseTest()
        {
            Assert.AreEqual("BCDEFG", Xb.Str.SliceReverse("ABCDEFG", 1));
            Assert.AreEqual("DEFG", Xb.Str.SliceReverse("ABCDEFG", 3));
            Assert.AreEqual("ABCDE", Xb.Str.SliceReverse("ABCDEFG", -2));

            Assert.AreEqual(null, Xb.Str.SliceReverse(null, 1));
            Assert.AreEqual("", Xb.Str.SliceReverse("", 1));
            Assert.AreEqual("ABCDEFG", Xb.Str.SliceReverse("ABCDEFG", 0));
            Assert.AreEqual("", Xb.Str.SliceReverse("ABCDEFG", 7));
            Assert.AreEqual("", Xb.Str.SliceReverse("ABCDEFG", -7));
            Assert.AreEqual("", Xb.Str.SliceReverse("ABCDEFG", 8));
            Assert.AreEqual("", Xb.Str.SliceReverse("ABCDEFG", -8));
        }

        [TestMethod()]
        public void LeftSentenceTest()
        {
            Assert.AreEqual("1/2", Xb.Str.LeftSentence("1/2/3/4", 2));
            Assert.AreEqual("1/2/3", Xb.Str.LeftSentence("1/2/3/4/5", 3));
            Assert.AreEqual("bb Bb/cCcCcCc/DdDDdDd", Xb.Str.LeftSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", -1));
            Assert.AreEqual("DdDDdDd", Xb.Str.LeftSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", -3));

            Assert.AreEqual(null, Xb.Str.LeftSentence(null, 1));
            Assert.AreEqual("", Xb.Str.LeftSentence("", 1));
            Assert.AreEqual("", Xb.Str.LeftSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", 0));
            Assert.AreEqual("AAaa/bb Bb/cCcCcCc/DdDDdDd", Xb.Str.LeftSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", 1, null));

            Assert.AreEqual("AAaa/bb Bb/cCcCcCc/DdDDdDd", Xb.Str.LeftSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", 4));
            Assert.AreEqual("", Xb.Str.LeftSentence("", -4));
            Assert.AreEqual("AAaa/bb Bb/cCcCcCc/DdDDdDd", Xb.Str.LeftSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", 5));
            Assert.AreEqual("", Xb.Str.LeftSentence("", -5));


            Assert.AreEqual("1 2", Xb.Str.LeftSentence("1 2 3 4", 2, " "));
            Assert.AreEqual("1 2 3", Xb.Str.LeftSentence("1 2 3 4 5", 3, " "));
            Assert.AreEqual("bb-Bb cCcCcCc DdDDdDd", Xb.Str.LeftSentence("AAaa bb-Bb cCcCcCc DdDDdDd", -1, " "));
            Assert.AreEqual("DdDDdDd", Xb.Str.LeftSentence("AAAaa bb-Bb cCcCcCc DdDDdDd", -3, " "));

            Assert.AreEqual(null, Xb.Str.LeftSentence(null, 1, " "));
            Assert.AreEqual("", Xb.Str.LeftSentence("", 1, " "));
            Assert.AreEqual("", Xb.Str.LeftSentence("AAAaa bb-Bb cCcCcCc DdDDdDd", 0, " "));
            Assert.AreEqual("AAAaa bb-Bb cCcCcCc DdDDdDd", Xb.Str.LeftSentence("AAAaa bb-Bb cCcCcCc DdDDdDd", 1, null));

            Assert.AreEqual("AAAaa bb-Bb cCcCcCc DdDDdDd", Xb.Str.LeftSentence("AAAaa bb-Bb cCcCcCc DdDDdDd", 4, " "));
            Assert.AreEqual("", Xb.Str.LeftSentence("", -4, " "));
            Assert.AreEqual("AAAaa bb-Bb cCcCcCc DdDDdDd", Xb.Str.LeftSentence("AAAaa bb-Bb cCcCcCc DdDDdDd", 5, " "));
            Assert.AreEqual("", Xb.Str.LeftSentence("", -5, " "));
        }

        [TestMethod()]
        public void RightSentenceTest()
        {
            Assert.AreEqual("3/4", Xb.Str.RightSentence("1/2/3/4", 2));
            Assert.AreEqual("3/4/5", Xb.Str.RightSentence("1/2/3/4/5", 3));
            Assert.AreEqual("AAaa/bb Bb/cCcCcCc", Xb.Str.RightSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", -1));
            Assert.AreEqual("AAaa", Xb.Str.RightSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", -3));

            Assert.AreEqual(null, Xb.Str.RightSentence(null, 1));
            Assert.AreEqual("", Xb.Str.RightSentence("", 1));
            Assert.AreEqual("", Xb.Str.RightSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", 0));
            Assert.AreEqual("AAaa/bb Bb/cCcCcCc/DdDDdDd", Xb.Str.RightSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", 1, null));

            Assert.AreEqual("AAaa/bb Bb/cCcCcCc/DdDDdDd", Xb.Str.RightSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", 4));
            Assert.AreEqual("", Xb.Str.RightSentence("", -4));
            Assert.AreEqual("AAaa/bb Bb/cCcCcCc/DdDDdDd", Xb.Str.RightSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", 5));
            Assert.AreEqual("", Xb.Str.RightSentence("", -5));


            Assert.AreEqual("3 4", Xb.Str.RightSentence("1 2 3 4", 2, " "));
            Assert.AreEqual("3 4 5", Xb.Str.RightSentence("1 2 3 4 5", 3, " "));
            Assert.AreEqual("AAaa bb-Bb cCcCcCc", Xb.Str.RightSentence("AAaa bb-Bb cCcCcCc DdDDdDd", -1, " "));
            Assert.AreEqual("AAAaa", Xb.Str.RightSentence("AAAaa bb-Bb cCcCcCc DdDDdDd", -3, " "));

            Assert.AreEqual(null, Xb.Str.RightSentence(null, 1, " "));
            Assert.AreEqual("", Xb.Str.RightSentence("", 1, " "));
            Assert.AreEqual("", Xb.Str.RightSentence("AAAaa bb-Bb cCcCcCc DdDDdDd", 0, " "));
            Assert.AreEqual("AAAaa bb-Bb cCcCcCc DdDDdDd", Xb.Str.RightSentence("AAAaa bb-Bb cCcCcCc DdDDdDd", 1, null));

            Assert.AreEqual("AAAaa bb-Bb cCcCcCc DdDDdDd", Xb.Str.RightSentence("AAAaa bb-Bb cCcCcCc DdDDdDd", 4, " "));
            Assert.AreEqual("", Xb.Str.RightSentence("", -4, " "));
            Assert.AreEqual("AAAaa bb-Bb cCcCcCc DdDDdDd", Xb.Str.RightSentence("AAAaa bb-Bb cCcCcCc DdDDdDd", 5, " "));
            Assert.AreEqual("", Xb.Str.RightSentence("", -5, " "));
        }

        [TestMethod()]
        public void SliceSentenceTest()
        {
            Assert.AreEqual("1/2", Xb.Str.SliceSentence("1/2/3/4", 2));
            Assert.AreEqual("1/2/3", Xb.Str.SliceSentence("1/2/3/4/5", 3));
            Assert.AreEqual("DdDDdDd", Xb.Str.SliceSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", -1));
            Assert.AreEqual("bb Bb/cCcCcCc/DdDDdDd", Xb.Str.SliceSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", -3));

            Assert.AreEqual(null, Xb.Str.SliceSentence(null, 1));
            Assert.AreEqual("", Xb.Str.SliceSentence("", 1));
            Assert.AreEqual("", Xb.Str.SliceSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", 0));
            Assert.AreEqual("AAaa/bb Bb/cCcCcCc/DdDDdDd", Xb.Str.SliceSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", 1, null));

            Assert.AreEqual("AAaa/bb Bb/cCcCcCc/DdDDdDd", Xb.Str.SliceSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", 4));
            Assert.AreEqual("AAaa/bb Bb/cCcCcCc/DdDDdDd", Xb.Str.SliceSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", -4));
            Assert.AreEqual("AAaa/bb Bb/cCcCcCc/DdDDdDd", Xb.Str.SliceSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", 5));
            Assert.AreEqual("AAaa/bb Bb/cCcCcCc/DdDDdDd", Xb.Str.SliceSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", -5));


            Assert.AreEqual("1 2", Xb.Str.SliceSentence("1 2 3 4", 2, " "));
            Assert.AreEqual("1 2 3", Xb.Str.SliceSentence("1 2 3 4 5", 3, " "));
            Assert.AreEqual("DdDDdDd", Xb.Str.SliceSentence("AAaa bb-Bb cCcCcCc DdDDdDd", -1, " "));
            Assert.AreEqual("bb-Bb cCcCcCc DdDDdDd", Xb.Str.SliceSentence("AAAaa bb-Bb cCcCcCc DdDDdDd", -3, " "));

            Assert.AreEqual(null, Xb.Str.SliceSentence(null, 1, " "));
            Assert.AreEqual("", Xb.Str.SliceSentence("", 1, " "));
            Assert.AreEqual("", Xb.Str.SliceSentence("AAAaa bb-Bb cCcCcCc DdDDdDd", 0, " "));
            Assert.AreEqual("AAAaa bb-Bb cCcCcCc DdDDdDd", Xb.Str.SliceSentence("AAAaa bb-Bb cCcCcCc DdDDdDd", 1, null));

            Assert.AreEqual("AAAaa bb-Bb cCcCcCc DdDDdDd", Xb.Str.SliceSentence("AAAaa bb-Bb cCcCcCc DdDDdDd", 4, " "));
            Assert.AreEqual("AAAaa bb-Bb cCcCcCc DdDDdDd", Xb.Str.SliceSentence("AAAaa bb-Bb cCcCcCc DdDDdDd", -4, " "));
            Assert.AreEqual("AAAaa bb-Bb cCcCcCc DdDDdDd", Xb.Str.SliceSentence("AAAaa bb-Bb cCcCcCc DdDDdDd", 5, " "));
            Assert.AreEqual("AAAaa bb-Bb cCcCcCc DdDDdDd", Xb.Str.SliceSentence("AAAaa bb-Bb cCcCcCc DdDDdDd", -5, " "));
        }

        [TestMethod()]
        public void SliceReverseSentenceTest()
        {
            Assert.AreEqual("3/4", Xb.Str.SliceReverseSentence("1/2/3/4", 2));
            Assert.AreEqual("4/5", Xb.Str.SliceReverseSentence("1/2/3/4/5", 3));
            Assert.AreEqual("AAaa/bb Bb/cCcCcCc", Xb.Str.SliceReverseSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", -1));
            Assert.AreEqual("AAaa", Xb.Str.SliceReverseSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", -3));

            Assert.AreEqual(null, Xb.Str.SliceReverseSentence(null, 1));
            Assert.AreEqual("", Xb.Str.SliceReverseSentence("", 1));
            Assert.AreEqual("AAaa/bb Bb/cCcCcCc/DdDDdDd", Xb.Str.SliceReverseSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", 0));
            Assert.AreEqual("", Xb.Str.SliceReverseSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", 1, null));

            Assert.AreEqual("", Xb.Str.SliceReverseSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", 4));
            Assert.AreEqual("", Xb.Str.SliceReverseSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", -4));
            Assert.AreEqual("", Xb.Str.SliceReverseSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", 5));
            Assert.AreEqual("", Xb.Str.SliceReverseSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", -5));


            Assert.AreEqual("3 4", Xb.Str.SliceReverseSentence("1 2 3 4", 2, " "));
            Assert.AreEqual("4 5", Xb.Str.SliceReverseSentence("1 2 3 4 5", 3, " "));
            Assert.AreEqual("AAaa bb-Bb cCcCcCc", Xb.Str.SliceReverseSentence("AAaa bb-Bb cCcCcCc DdDDdDd", -1, " "));
            Assert.AreEqual("AAaa", Xb.Str.SliceReverseSentence("AAaa bb-Bb cCcCcCc DdDDdDd", -3, " "));

            Assert.AreEqual(null, Xb.Str.SliceReverseSentence(null, 1, " "));
            Assert.AreEqual("", Xb.Str.SliceReverseSentence("", 1, " "));
            Assert.AreEqual("AAaa bb-Bb cCcCcCc DdDDdDd", Xb.Str.SliceReverseSentence("AAaa bb-Bb cCcCcCc DdDDdDd", 0, " "));
            Assert.AreEqual("", Xb.Str.SliceReverseSentence("AAaa bb-Bb cCcCcCc DdDDdDd", 1, null));

            Assert.AreEqual("", Xb.Str.SliceReverseSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", 4, " "));
            Assert.AreEqual("", Xb.Str.SliceReverseSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", -4, " "));
            Assert.AreEqual("", Xb.Str.SliceReverseSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", 5, " "));
            Assert.AreEqual("", Xb.Str.SliceReverseSentence("AAaa/bb Bb/cCcCcCc/DdDDdDd", -5, " "));
        }

        [TestMethod()]
        public void SplitTest()
        {
            string[] nosplitAry;
            string targetStr;
            string[] splitted;

            var nullAry = new string[] {};

            targetStr = "fuck apple why cannot send imessage?";
            nosplitAry = new string[] { targetStr };
            splitted = new string[] { "fuck", "apple", "why", "cannot", "send", "imessage?" };
            Assert.IsTrue(splitted.SequenceEqual(Xb.Str.Split(targetStr)));
            Assert.IsTrue(splitted.SequenceEqual(Xb.Str.Split(targetStr, " ")));
            Assert.IsFalse(splitted.SequenceEqual(Xb.Str.Split(targetStr, "a")));

            targetStr = "漢字です分割１２あいうえ分割ＤＵＲ分割";
            nosplitAry = new string[] { targetStr };
            splitted = new string[] { "漢字です", "１２あいうえ", "ＤＵＲ", "" };
            Assert.IsTrue(splitted.SequenceEqual(Xb.Str.Split(targetStr, "分割")));
            Assert.IsFalse(splitted.SequenceEqual(Xb.Str.Split(targetStr)));
            Assert.IsFalse(splitted.SequenceEqual(Xb.Str.Split(targetStr, " ")));

            Assert.IsTrue(nosplitAry.SequenceEqual(Xb.Str.Split(targetStr, null)));
            Assert.IsTrue(nosplitAry.SequenceEqual(Xb.Str.Split(targetStr, "")));
            Assert.IsTrue(nullAry.SequenceEqual(Xb.Str.Split(null, ",")));
            Assert.IsTrue(nullAry.SequenceEqual(Xb.Str.Split("", ",")));
        }

        [TestMethod()]
        public void IsAsciiTest()
        {
            Assert.IsTrue(Xb.Str.IsAscii(null));
            Assert.IsTrue(Xb.Str.IsAscii(""));
            Assert.IsTrue(Xb.Str.IsAscii("i 8hH6 $5  hello?"));
            Assert.IsFalse(Xb.Str.IsAscii("i 8hH6 ＄  hello?"));
            Assert.IsFalse(Xb.Str.IsAscii("ＡＢＣＤＥ"));
        }

        [TestMethod()]
        public void IsMultiByteTest()
        {
            Assert.IsFalse(Xb.Str.IsMultiByte(null));
            Assert.IsFalse(Xb.Str.IsMultiByte(""));
            Assert.IsFalse(Xb.Str.IsMultiByte("i 8hH6 $5  hello?"));
            Assert.IsFalse(Xb.Str.IsMultiByte("i 8hH6 ＄  hello?"));
            Assert.IsTrue(Xb.Str.IsMultiByte("ＡＢＣＤＥ"));
            Assert.IsTrue(Xb.Str.IsMultiByte("ＡＢＣＤＥｱｲｳｴｵ"));
            Assert.IsFalse(Xb.Str.IsMultiByte("ＡＢＣＤＥｱｲｳｴｵ", System.Text.Encoding.GetEncoding("Shift_JIS")));
        }

        [TestMethod()]
        public void GetLinefeedTest()
        {
            Assert.AreEqual("\n", Xb.Str.GetLinefeed(Xb.Str.LinefeedType.Lf));
            Assert.AreEqual("\r\n", Xb.Str.GetLinefeed(Xb.Str.LinefeedType.CrLf));
            Assert.AreEqual("\r", Xb.Str.GetLinefeed(Xb.Str.LinefeedType.Cr));
            Assert.AreEqual("\r\n", Xb.Str.GetLinefeed());
        }

        [TestMethod()]
        public void GetMultiLineTest()
        {
            string target;
            string[] splitted;

            target = "AAAあ\r\nいうkdあ漢\r\n字86D\r\nsかｈこまっ\r\nたもんだ";
            splitted = new string[] { "AAAあ", "いうkdあ漢", "字86D", "sかｈこまっ", "たもんだ" };
            Assert.IsTrue(splitted.SequenceEqual(Xb.Str.GetMultiLine(target)));

            target = "AAAあ\r\nいうkdあ漢\r字86D\nsかｈこまっ\r\nたもんだ\r\n";
            splitted = new string[] { "AAAあ", "いうkdあ漢\r字86D\nsかｈこまっ", "たもんだ" };
            Assert.IsTrue(splitted.SequenceEqual(Xb.Str.GetMultiLine(target)));

            target = "AAAあ\rいうkdあ漢\r字86D\rsかｈこまっ\rたもんだ\r";
            splitted = new string[] { "AAAあ", "いうkdあ漢", "字86D", "sかｈこまっ", "たもんだ" };
            Assert.IsTrue(splitted.SequenceEqual(Xb.Str.GetMultiLine(target)));

            target = "AAAあ\rいうkdあ漢字86Dsかｈこまっ\rたもんだ";
            splitted = new string[] { "AAAあ", "いうkdあ漢字86Dsかｈこまっ", "たもんだ" };
            Assert.IsTrue(splitted.SequenceEqual(Xb.Str.GetMultiLine(target)));

            target = "AAAあ\nいうkdあ漢\n字86D\nsかｈこまっ\nたもんだ\n";
            splitted = new string[] { "AAAあ", "いうkdあ漢", "字86D", "sかｈこまっ", "たもんだ" };
            Assert.IsTrue(splitted.SequenceEqual(Xb.Str.GetMultiLine(target)));

            target = "AAAあ\nいうkdあ漢字86Dsかｈこまっ\nたもんだ";
            splitted = new string[] { "AAAあ", "いうkdあ漢字86Dsかｈこまっ", "たもんだ" };
            Assert.IsTrue(splitted.SequenceEqual(Xb.Str.GetMultiLine(target)));
        }

        [TestMethod()]
        public void GetBytesTest()
        {
            string target = "1234567890";
            Encoding encode = Encoding.UTF8;
            Assert.IsTrue(encode.GetBytes(target).SequenceEqual(Xb.Str.GetBytes(target)));
            Assert.IsTrue(encode.GetBytes(target).SequenceEqual(Xb.Str.GetBytes(target, encode)));

            target = "ABCｶﾅ漢字";
            Assert.IsTrue(encode.GetBytes(target).SequenceEqual(Xb.Str.GetBytes(target)));
            Assert.IsTrue(encode.GetBytes(target).SequenceEqual(Xb.Str.GetBytes(target, encode)));

            encode = Encoding.GetEncoding("Shift_JIS");
            Assert.IsFalse(encode.GetBytes(target).SequenceEqual(Xb.Str.GetBytes(target)));
            Assert.IsTrue(encode.GetBytes(target).SequenceEqual(Xb.Str.GetBytes(target, encode)));

        }

        [TestMethod()]
        public void GetByteLengthTest()
        {
            string target = "1234567890";
            Encoding encode = Encoding.UTF8;
            Assert.AreEqual(encode.GetBytes(target).Length, Xb.Str.GetByteLength(target));
            Assert.AreEqual(encode.GetBytes(target).Length, Xb.Str.GetByteLength(target, encode));

            target = "ABCｶﾅ漢字";
            Assert.AreEqual(encode.GetBytes(target).Length, Xb.Str.GetByteLength(target));
            Assert.AreEqual(encode.GetBytes(target).Length, Xb.Str.GetByteLength(target, encode));

            encode = Encoding.GetEncoding("Shift_JIS");
            Assert.AreEqual(encode.GetBytes(target).Length, Xb.Str.GetByteLength(target, encode));
        }

        [TestMethod()]
        public void EscapeHtmlTest()
        {
            var html = "<html>\"hello\"\r\nday & night</html>";
            var escaped = "&lt;html&gt;&quot;hello&quot;\r\nday &amp; night&lt;/html&gt;";
            Assert.AreEqual(escaped, Xb.Str.EscapeHtml(html));
        }

        [TestMethod()]
        public void UnescapeHtmlTest()
        {
            var html = "<html>\"hello\"\r\nday & night</html>";
            var escaped = "&lt;html&gt;&quot;hello&quot;\r\nday &amp; night&lt;/html&gt;";
            Assert.AreEqual(html, Xb.Str.UnescapeHtml(escaped));

        }

        [TestMethod()]
        public void MySqlQuoteTest()
        {
            var sqlvalue = "it's amazing '\\1,234-'";
            var quoted = "'it\\'s amazing \\'\\\\1,234-\\''";
            Assert.AreEqual(quoted, Xb.Str.MySqlQuote(sqlvalue));

            Assert.AreEqual("''", Xb.Str.MySqlQuote(null));
        }

        [TestMethod()]
        public void SqlQuoteTest()
        {
            var sqlvalue = "it's amazing '\\1,234-'";
            var quoted = "'it''s amazing ''\\1,234-'''";
            Assert.AreEqual(quoted, Xb.Str.SqlQuote(sqlvalue));

            Assert.AreEqual("''", Xb.Str.SqlQuote(null));
        }

        [TestMethod()]
        public void DquoteTest()
        {
            var value = "i said \"HELLO WORLD\".";
            var quoted = "\"i said \\\"HELLO WORLD\\\".\"";
            Assert.AreEqual(quoted, Xb.Str.Dquote(value));

            Assert.AreEqual("\"\"", Xb.Str.Dquote(null));
        }

        [TestMethod()]
        public void CsvDquoteTest()
        {
            var value = "i said, \"HELLO WORLD\".";
            var quoted = "\"i said\\, \"\"HELLO WORLD\"\".\"";
            Assert.AreEqual(quoted, Xb.Str.CsvDquote(value));

            Assert.AreEqual("\"\"", Xb.Str.CsvDquote(null));
        }

        [TestMethod()]
        public void GetStringTest()
        {
            string target = "1234567890";
            Encoding encode = Encoding.UTF8;
            byte[] bytes = encode.GetBytes(target);

            Assert.AreEqual(target, Xb.Str.GetString(bytes));

            target = "ABCｶﾅ漢字";
            bytes = encode.GetBytes(target);
            Assert.AreEqual(target, Xb.Str.GetString(bytes));

            encode = Encoding.GetEncoding("Shift_JIS");
            bytes = encode.GetBytes(target);
            Assert.AreEqual(target, Xb.Str.GetString(bytes));
        }


        [TestMethod()]
        public void GetStringTest2()
        {
            string target = "1234567890";
            Encoding encode = Encoding.UTF8;
            byte[] bytes = encode.GetBytes(target);
            var stream = (System.IO.Stream) (new System.IO.MemoryStream(bytes));

            Assert.AreEqual(target, Xb.Str.GetString(stream));

            target = "ABCｶﾅ漢字";
            bytes = encode.GetBytes(target);
            stream = (System.IO.Stream)(new System.IO.MemoryStream(bytes));
            Assert.AreEqual(target, Xb.Str.GetString(stream));

            encode = Encoding.GetEncoding("Shift_JIS");
            bytes = encode.GetBytes(target);
            stream = (System.IO.Stream)(new System.IO.MemoryStream(bytes));
            Assert.AreEqual(target, Xb.Str.GetString(stream));
        }

        [TestMethod()]
        public void GetEncodeTest()
        {
            string target = "1234567890";
            Encoding encode = Encoding.UTF8;
            byte[] bytes = encode.GetBytes(target);

            Assert.AreEqual(Encoding.ASCII, Xb.Str.GetEncode(bytes));

            target = "ABCｶﾅ漢字";
            bytes = encode.GetBytes(target);
            Assert.AreEqual(encode, Xb.Str.GetEncode(bytes));

            encode = Encoding.GetEncoding("Shift_JIS");
            bytes = encode.GetBytes(target);
            Assert.AreEqual(encode, Xb.Str.GetEncode(bytes));

            encode = Encoding.GetEncoding("euc-jp");
            bytes = encode.GetBytes(target);
            Assert.AreEqual(encode, Xb.Str.GetEncode(bytes));

            encode = Encoding.GetEncoding("iso-2022-jp");
            bytes = encode.GetBytes(target);
            Assert.AreEqual(encode, Xb.Str.GetEncode(bytes));
        }
    }
}
