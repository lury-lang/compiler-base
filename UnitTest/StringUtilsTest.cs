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
        [TestCase("\u2029", 2)]
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
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(-1)]
        public void GetPositionByIndexError1(int index)
        {
            Assert.Throws<ArgumentNullException>(() => StringUtils.GetPositionByIndex(null, index));
        }

        [Test]
        [TestCase("test", -1)]
        [TestCase("test", 6)]
        public void GetPositionByIndexError2(string text, int index)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => text.GetPositionByIndex(index));
        }

        [Test]
        [TestCase("abc", 0, 0)]
        [TestCase("abc", 1, 2)]
        public void GeneratePointingStrings1Test(string text, int index, int length)
        {
            CharPosition pos;
            var strs = text.GeneratePointingStrings(index, length, out pos);

            CollectionAssert.AllItemsAreNotNull(strs);
            Assert.AreEqual(2, strs.Length);
            Assert.AreEqual(text.GetPositionByIndex(index), pos);
            StringAssert.Contains(strs[0], text);
        }

        [Test]
        [TestCase(0, 0)]
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
        [TestCase("abc", 1, 2, 0)]
        [TestCase("abc", 1, 1, 1)]
        [TestCase("abc\n", 2, 1, 0)]
        [TestCase("abc\ndef", 2, 2, 2)]
        public void GeneratePointingStrings2Test(string text, int line, int column, int length)
        {
            var strs = text.GeneratePointingStrings(new CharPosition(line, column), length);

            CollectionAssert.AllItemsAreNotNull(strs);
            Assert.AreEqual(2, strs.Length);
        }

        [Test]
        [TestCase(1, 1, 0)]
        [TestCase(1, 1, 2)]
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
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(5)]
        public void GetLineError1(int line)
        {
            Assert.Throws<ArgumentNullException>(() => StringUtils.GetLine(null, line));
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
