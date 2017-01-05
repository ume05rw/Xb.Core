using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsXb.Core
{
    [TestClass()]
    public class ByteTests : TestBase
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


        [TestMethod()]
        public void ByteArrayTest1()
        {
            var addr = new byte[]
            {
                (byte)192,
                (byte)168,
                (byte)254,
                (byte)105
            };
            var mask = new byte[]
            {
                (byte)255,
                (byte)255,
                (byte)255,
                (byte)0
            };

            var addrBytes = new Xb.Byte.ByteArray(addr);
            var maskBytes = new Xb.Byte.ByteArray(mask);

            this.Out(addrBytes.BitString);
            this.Out(maskBytes.BitString);

            var maskIdx = maskBytes.LastIndexOf(true);
            this.Out($"maskIdx.ByteIndex: {maskIdx.ByteIndex}, maskIdx.BitIndex: {maskIdx.BitIndex}");
            Assert.AreEqual(2, maskIdx.ByteIndex);
            Assert.AreEqual(7, maskIdx.BitIndex);
            

            var startIdx = maskIdx.Next;
            this.Out($"startIdx.ByteIndex: {startIdx.ByteIndex}, startIdx.BitIndex: {startIdx.BitIndex}");
            Assert.AreEqual(3, startIdx.ByteIndex);
            Assert.AreEqual(0, startIdx.BitIndex);

            Assert.AreEqual(3, startIdx.Next.ByteIndex);
            Assert.AreEqual(1, startIdx.Next.BitIndex);

            var prevIdx = startIdx.Prev;
            this.Out($"prevIdx.ByteIndex: {prevIdx.ByteIndex}, prevIdx.BitIndex: {prevIdx.BitIndex}");
            Assert.AreEqual(2, prevIdx.ByteIndex);
            Assert.AreEqual(7, prevIdx.BitIndex);
            Assert.AreEqual(2, prevIdx.Prev.ByteIndex);
            Assert.AreEqual(6, prevIdx.Prev.BitIndex);

            var idx = maskIdx;
            while ((idx = idx.Next).ByteIndex != -1)
            {
                addrBytes.SetBit(idx.ByteIndex, idx.BitIndex, false);
            }
            this.Out(addrBytes.BitString);
            this.Out("NetworkAddress: " + string.Join(".", addrBytes.Bytes.Select(b => ((int) b).ToString())));

            idx = maskIdx;
            while ((idx = idx.Next).ByteIndex != -1)
            {
                addrBytes.SetBit(idx.ByteIndex, idx.BitIndex, true);
            }
            this.Out(addrBytes.BitString);
            this.Out("BroadcastAddress: " + string.Join(".", addrBytes.Bytes.Select(b => ((int)b).ToString())));
        }
    }
}
