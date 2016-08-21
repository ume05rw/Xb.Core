using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsXb.Core
{
    [TestClass()]
    public class ByteTests
    {
        [TestMethod()]
        public void GetBase64StringTest()
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes("おれはジャイアン、ガキ大将");
            var base64 = "44GK44KM44Gv44K444Oj44Kk44Ki44Oz44CB44Ks44Kt5aSn5bCG";
            Assert.AreEqual(base64, Xb.Byte.GetBase64String(bytes));
        }

        [TestMethod()]
        public void GetBase64StringTest2()
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes("おれはジャイアン、ガキ大将");
            var base64 = "44GK44KM44Gv44K444Oj44Kk44Ki44Oz44CB44Ks44Kt5aSn5bCG";
            var stream = (System.IO.Stream) (new System.IO.MemoryStream(bytes));
            Assert.AreEqual(base64, Xb.Byte.GetBase64String(stream));
        }

        [TestMethod()]
        public void GetBytesTest()
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes("おれはジャイアン、ガキ大将");
            var stream = (System.IO.Stream)(new System.IO.MemoryStream(bytes));

            //Stream同志の比較が出来ないため、一旦byte配列に変換している。
            var resultBytes = Xb.Byte.GetBytes(stream);
            Assert.IsTrue(bytes.SequenceEqual(resultBytes));
        }

        [TestMethod()]
        public void GetStream1Test()
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes("おれはジャイアン、ガキ大将");
            var stream1 = (System.IO.Stream)(new System.IO.MemoryStream(bytes));
            var stream2 = Xb.Byte.GetStream(bytes);

            //Stream同志の比較が出来ないため、一旦byte配列に変換している。
            var toBytes = Xb.Byte.GetBytes(stream2);
            Assert.IsTrue(bytes.SequenceEqual(toBytes));
        }

        [TestMethod()]
        public void GetStream2Test()
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes("おれはジャイアン、ガキ大将");
            var base64 = "44GK44KM44Gv44K444Oj44Kk44Ki44Oz44CB44Ks44Kt5aSn5bCG";
            var stream2 = Xb.Byte.GetStream(base64);
            var toBytes = Xb.Byte.GetBytes(stream2);

            Assert.IsTrue(bytes.SequenceEqual(toBytes));
        }

        [TestMethod()]
        public void GetBitString1Test()
        {
            var bytes = BitConverter.GetBytes(254);
            var oneByte = bytes[0];
            var bitString = "11111110";
            Assert.AreEqual(bitString, Xb.Byte.GetBitString(oneByte));
        }

        [TestMethod()]
        public void GetBitString2Test()
        {
            var intValue = 37653;
            var bitString = "1001001100010101";
            Assert.AreEqual(bitString, Xb.Byte.GetBitString(intValue));
        }


        [TestMethod()]
        public void GetBitTest()
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes("おれはジャイアン、ガキ大将");
            var ary = new Xb.Byte.ByteArray(bytes);
            Assert.AreEqual(true, ary.GetBit(0, 0));
            Assert.AreEqual(true, ary.GetBit(0, 1));
            Assert.AreEqual(false, ary.GetBit(0, 2));
            Assert.AreEqual(false, ary.GetBit(0, 3));
            Assert.AreEqual(false, ary.GetBit(0, 4));
            Assert.AreEqual(true, ary.GetBit(0, 5));
            Assert.AreEqual(true, ary.GetBit(0, 6));
            Assert.AreEqual(true, ary.GetBit(0, 7));

            Assert.AreEqual(true, ary.GetBit(1, 0));
            Assert.AreEqual(false, ary.GetBit(1, 1));
            Assert.AreEqual(false, ary.GetBit(1, 2));
            Assert.AreEqual(false, ary.GetBit(1, 3));
            Assert.AreEqual(false, ary.GetBit(1, 4));
            Assert.AreEqual(false, ary.GetBit(1, 5));
            Assert.AreEqual(false, ary.GetBit(1, 6));
            Assert.AreEqual(true, ary.GetBit(1, 7));
        }
        
        [TestMethod()]
        public void SetBitTest()
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes("おれはジャイアン、ガキ大将");
            var ary = new Xb.Byte.ByteArray(bytes);

            ary.SetBit(0, 3, true);
            ary.SetBit(0, 6, false);
            ary.SetBit(1, 5, true);

            Assert.AreEqual(true, ary.GetBit(0, 0));
            Assert.AreEqual(true, ary.GetBit(0, 1));
            Assert.AreEqual(false, ary.GetBit(0, 2));
            Assert.AreEqual(true, ary.GetBit(0, 3));
            Assert.AreEqual(false, ary.GetBit(0, 4));
            Assert.AreEqual(true, ary.GetBit(0, 5));
            Assert.AreEqual(false, ary.GetBit(0, 6));
            Assert.AreEqual(true, ary.GetBit(0, 7));

            Assert.AreEqual(true, ary.GetBit(1, 0));
            Assert.AreEqual(false, ary.GetBit(1, 1));
            Assert.AreEqual(false, ary.GetBit(1, 2));
            Assert.AreEqual(false, ary.GetBit(1, 3));
            Assert.AreEqual(false, ary.GetBit(1, 4));
            Assert.AreEqual(true, ary.GetBit(1, 5));
            Assert.AreEqual(false, ary.GetBit(1, 6));
            Assert.AreEqual(true, ary.GetBit(1, 7));
        }

        [TestMethod()]
        public void GetIntegerTest()
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes("おれはジャイアン、ガキ大将");
            var ary = new Xb.Byte.ByteArray(bytes);
            Assert.AreEqual(8577930, ary.GetInteger(2, 3));
        }

        [TestMethod()]
        public void GetLongTest()
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes("おれはジャイアン、ガキ大将");
            var ary = new Xb.Byte.ByteArray(bytes);
            Assert.AreEqual(977314964362, ary.GetLong(2, 5));
        }
    }
}
