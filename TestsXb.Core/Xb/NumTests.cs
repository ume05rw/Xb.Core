using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xb;

namespace TestsXb.Core
{
    [TestClass()]
    public class NumTests
    {
        [TestMethod()]
        public void GetRoundedValueTest()
        {
            Assert.AreEqual(10, Xb.Num.Round((decimal)10.39));
            Assert.AreEqual(11, Xb.Num.Round((decimal)10.50));
            Assert.AreEqual(10, Xb.Num.Round((decimal)10.49, Num.RoundType.HalfUp));
            Assert.AreEqual(11, Xb.Num.Round((decimal)10.50, Num.RoundType.HalfUp));
            Assert.AreEqual(10, Xb.Num.Round((decimal)9.01, Num.RoundType.Cell));
            Assert.AreEqual(10, Xb.Num.Round((decimal)10.99, Num.RoundType.Floor));
            Assert.AreEqual(110, Xb.Num.Round((decimal)111.12, Num.RoundType.Floor, -1));
            Assert.AreEqual(120, Xb.Num.Round((decimal)111.12, Num.RoundType.Cell, -1));
            Assert.AreEqual(110, Xb.Num.Round((decimal)114.99, Num.RoundType.HalfUp, -1));
            Assert.AreEqual(120, Xb.Num.Round((decimal)115.00, Num.RoundType.HalfUp, -1));
            Assert.AreEqual((decimal)115.12, Xb.Num.Round((decimal)115.1230, Num.RoundType.Floor, 2));
            Assert.AreEqual((decimal)115.13, Xb.Num.Round((decimal)115.1230, Num.RoundType.Cell, 2));
            Assert.AreEqual((decimal)115.12, Xb.Num.Round((decimal)115.1249, Num.RoundType.HalfUp, 2));
            Assert.AreEqual((decimal)115.13, Xb.Num.Round((decimal)115.1250, Num.RoundType.HalfUp, 2));
        }

        [TestMethod()]
        public void IsNumericTest()
        {
            Assert.IsTrue(Xb.Num.IsNumeric("123"));
            Assert.IsTrue(Xb.Num.IsNumeric("1.23"));
            Assert.IsFalse(Xb.Num.IsNumeric(null));
            Assert.IsFalse(Xb.Num.IsNumeric("1n"));
            Assert.IsFalse(Xb.Num.IsNumeric("hello"));
            Assert.IsFalse(Xb.Num.IsNumeric("0x13"));
        }
    }
}
