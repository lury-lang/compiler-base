using System;
using System.Linq;
using Lury.Compiling.Utils;
using NUnit.Framework;

namespace UnitTest
{
    [TestFixture]
    public class StringUtilsTest
    {
        [Test]
        [TestCase(null, 0)]
        [TestCase("", 1)]
        [TestCase("\0", 1)]
        [TestCase("abc123", 1)]
        [TestCase("\r", 2)]
        [TestCase("\n", 2)]
        [TestCase("\r\n", 2)]
        [TestCase("\u2028", 2)]
        [TestCase("\u2089", 2)]
        [TestCase("\n\r", 3)]
        public void GetNumberOfLineTest(string text, int line)
        {
            Assert.AreEqual(line, text.GetNumberOfLine());
        }

        [Test]
        [TestCase("", 0, 1, 1)]
        [TestCase("abc123", 0, 1, 1)]
        [TestCase("abc123", 3, 1, 4)]
        [TestCase("ab\nc123", 3, 2, 1)]
        [TestCase("ab\r\nc123", 3, 2, 1)]
        [TestCase("abc123", 6, 1, 7)]
        [TestCase("abc\n123", 6, 2, 3)]
        [TestCase("abc\n123", 7, 2, 4)]
        [TestCase("abc\n123\n456", 6, 2, 3)]
        public void GetPositionByIndexTest(string text, int index, int line, int column)
        {
            Assert.AreEqual(new CharPosition(line, column), text.GetPositionByIndex(index));
        }

        [Test]
        [TestCase(null, 0)]
        [TestCase("test", -1)]
        [TestCase("test", 6)]
        public void GetPositionByIndexError(string text, int index)
        {
            Assert.Throws<ArgumentNullException>(() => text.GetPositionByIndex(index));
        }

        [Test]
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

        [Test]
        [TestCase(1, 2)]
        public void GeneratePointingStrings1Error1(int index, int length)
        {
            CharPosition pos;
            Assert.Throws<ArgumentNullException>(() => StringUtils.GeneratePointingStrings(null, index, length, out pos));
        }

        [Test]
        [TestCase("abc", -1, 2)]
        [TestCase("abc", 3, 1)]
        [TestCase("abc", 0, -1)]
        [TestCase("abc", 0, 4)]
        public void GeneratePointingStrings1Error2(string text, int index, int length)
        {
            CharPosition pos;
            Assert.Throws<ArgumentOutOfRangeException>(() => text.GeneratePointingStrings(index, length, out pos));
        }

        [Test]
        public void GeneratePointingStrings2Test()
        {
            const string source = "abc";
            var strs = source.GeneratePointingStrings(new CharPosition(1, 2), 0);

            CollectionAssert.AllItemsAreNotNull(strs);
            Assert.AreEqual(2, strs.Length);
            StringAssert.Contains(strs[0], source);
        }

        [Test]
        public void GeneratePointingStrings3Test()
        {
            const string source = "abc\n";
            var strs = source.GeneratePointingStrings(new CharPosition(2, 1), 0);

            CollectionAssert.AllItemsAreNotNull(strs);
            Assert.AreEqual(2, strs.Length);
        }

        [Test]
        public void GeneratePointingStrings4Test()
        {
            const string source = "abc\n";
            var strs = source.GeneratePointingStrings(new CharPosition(2, 1), 1);

            CollectionAssert.AllItemsAreNotNull(strs);
            Assert.AreEqual(2, strs.Length);
        }

        [Test]
        [TestCase(1, 2, 2)]
        public void GeneratePointingStrings2Error1(int line, int column, int length)
        {
            var charPosition = new CharPosition(line, column);

            Assert.Throws<ArgumentNullException>(() => StringUtils.GeneratePointingStrings(null, charPosition, length));
        }

        [Test]
        [TestCase("abc", 0, 0, 2)]
        [TestCase("abc", 1, 1, -1)]
        [TestCase("abc", 1, 1, 4)]
        public void GeneratePointingStrings2Error2(string text, int line, int column, int length)
        {
            var charPosition = (line == 0 && column == 0)
                                ? CharPosition.Empty
                                : new CharPosition(line, column);

            Assert.Throws<ArgumentOutOfRangeException>(() => text.GeneratePointingStrings(charPosition, length));
        }

        [Test]
        [TestCase("", 1, "")]
        [TestCase("123", 1, "123")]
        [TestCase("123\n", 1, "123")]
        [TestCase("123\n", 2, "")]
        [TestCase("123\n\n456", 1, "123")]
        [TestCase("123\n\n456", 2, "")]
        [TestCase("123\n\n456", 3, "456")]
        public void GetLineTest(string input, int line, string output)
        {
            Assert.AreEqual(output, input.GetLine(line));
        }

        [Test]
        public void GetLineError1()
        {
            Assert.Throws<ArgumentNullException>(() => StringUtils.GetLine(null, 1));
        }

        [Test]
        [TestCase("test", 0)]
        [TestCase("test", 2)]
        public void GetLineError2(string text, int line)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => text.GetLine(line));
        }

        [Test]
        public void ConvertControlCharsTest()
        {
            Assert.AreEqual(@"\r\n\f\b\t", "\r\n\f\b\t".ConvertControlChars());

            var input = string.Join("", Enumerable.Repeat("\r\n\f\b\t", 500));
            var expect = string.Join("", Enumerable.Repeat(@"\r\n\f\b\t", 500));

            Assert.AreEqual(expect, input.ConvertControlChars());
        }
    }
}
