using System;
using System.Linq;
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

        [TestMethod]
        public void GetPositionByIndexTest()
        {
            Assert.AreEqual(CharPosition.BasePosition, String.Empty.GetPositionByIndex(0));
            Assert.AreEqual(CharPosition.BasePosition, "abc123".GetPositionByIndex(0));
            Assert.AreEqual(new CharPosition(1, 4), "abc123".GetPositionByIndex(3));
            Assert.AreEqual(new CharPosition(2, 1), "ab\nc123".GetPositionByIndex(3));
            Assert.AreEqual(new CharPosition(2, 1), "ab\r\nc123".GetPositionByIndex(3));
            Assert.AreEqual(new CharPosition(1, 7), "abc123".GetPositionByIndex(6));
            Assert.AreEqual(new CharPosition(2, 3), "abc\n123".GetPositionByIndex(6));
            Assert.AreEqual(new CharPosition(2, 4), "abc\n123".GetPositionByIndex(7));
            Assert.AreEqual(new CharPosition(2, 3), "abc\n123\n456".GetPositionByIndex(6));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetPositionByIndexError1()
        {
            StringUtils.GetPositionByIndex(null, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetPositionByIndexError2()
        {
            "text".GetPositionByIndex(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetPositionByIndexError3()
        {
            "text".GetPositionByIndex(6);
        }

        [TestMethod]
        public void GeneratePointingStrings1Test()
        {
            const string source = "abc";
            CharPosition pos;
            var strs = source.GeneratePointingStrings(1, 2, out pos);

            CollectionAssert.AllItemsAreNotNull(strs);
            Assert.AreEqual(2, strs.Length);
            Assert.AreEqual(new CharPosition(1, 2), pos);
            StringAssert.Contains(strs[0], source);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GeneratePointingStrings1Error1()
        {
            CharPosition pos;
            StringUtils.GeneratePointingStrings(null, 1, 2, out pos);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GeneratePointingStrings1Error2()
        {
            const string source = "abc";
            CharPosition pos;
            source.GeneratePointingStrings(-1, 2, out pos);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GeneratePointingStrings1Error3()
        {
            const string source = "abc";
            CharPosition pos;
            source.GeneratePointingStrings(3, 1, out pos);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GeneratePointingStrings1Error4()
        {
            const string source = "abc";
            CharPosition pos;
            source.GeneratePointingStrings(0, -1, out pos);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GeneratePointingStrings1Error5()
        {
            const string source = "abc";
            CharPosition pos;
            source.GeneratePointingStrings(0, 4, out pos);
        }

        [TestMethod]
        public void GeneratePointingStrings2Test()
        {
            const string source = "abc";
            var strs = source.GeneratePointingStrings(new CharPosition(1, 2), 0);

            CollectionAssert.AllItemsAreNotNull(strs);
            Assert.AreEqual(2, strs.Length);
            StringAssert.Contains(strs[0], source);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GeneratePointingStrings2Error1()
        {
            StringUtils.GeneratePointingStrings(null, new CharPosition(1, 2), 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GeneratePointingStrings2Error2()
        {
            const string source = "abc";
            source.GeneratePointingStrings(CharPosition.Empty, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GeneratePointingStrings2Error3()
        {
            const string source = "abc";
            source.GeneratePointingStrings(CharPosition.BasePosition, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GeneratePointingStrings2Error4()
        {
            const string source = "abc";
            source.GeneratePointingStrings(CharPosition.BasePosition, 4);
        }

        [TestMethod]
        public void GetLineTest()
        {
            Assert.AreEqual(string.Empty, string.Empty.GetLine(1));
            Assert.AreEqual("123", "123".GetLine(1));
            Assert.AreEqual("123", "123\n".GetLine(1));
            Assert.AreEqual(string.Empty, "123\n".GetLine(2));
            Assert.AreEqual("123", "123\n\n456".GetLine(1));
            Assert.AreEqual(string.Empty, "123\n\n456".GetLine(2));
            Assert.AreEqual("456", "123\n\n456".GetLine(3));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetLineError1()
        {
            StringUtils.GetLine(null, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetLineError2()
        {
            "test".GetLine(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetLineError3()
        {
            "test".GetLine(2);
        }

        [TestMethod]
        public void ConvertControlCharsTest()
        {
            Assert.AreEqual(@"\r\n\f\b\t", "\r\n\f\b\t".ConvertControlChars());

            var input = string.Join("", Enumerable.Repeat("\r\n\f\b\t", 500));
            var expect = string.Join("", Enumerable.Repeat(@"\r\n\f\b\t", 500));

            Assert.AreEqual(expect, input.ConvertControlChars());
        }
    }
}
