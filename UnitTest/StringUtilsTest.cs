using System;
using Lury.Compiling.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class StringUtilsTest
    {
        [TestMethod]
        public void GetNumberOfLineTest()
        {
            Assert.AreEqual(0, StringUtils.GetNumberOfLine(null));
            Assert.AreEqual(1, String.Empty.GetNumberOfLine());
            Assert.AreEqual(1, "\0".GetNumberOfLine());
            Assert.AreEqual(1, "abc123".GetNumberOfLine());
            Assert.AreEqual(2, "\r".GetNumberOfLine());
            Assert.AreEqual(2, "\n".GetNumberOfLine());
            Assert.AreEqual(2, "\r\n".GetNumberOfLine());
            Assert.AreEqual(2, "\u2028".GetNumberOfLine());
            Assert.AreEqual(2, "\u2029".GetNumberOfLine());
            Assert.AreEqual(3, "\n\r".GetNumberOfLine());
        }
    }
}
