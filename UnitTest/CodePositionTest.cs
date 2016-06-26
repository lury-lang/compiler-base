using System;
using System.Diagnostics.CodeAnalysis;
using Lury.Compiling.Utils;
using NUnit.Framework;

namespace UnitTest
{
    [TestFixture]
    public class CodePositionTest
    {
        private const string SourceName = "name";

        [Test]
        public void ConstructorTest1()
        {
            var pos = new CharPosition(1, 5);
            var length = 3;

            var codePos = new CodePosition(SourceName, pos, length);

            Assert.AreEqual(SourceName, codePos.SourceName);
            Assert.AreEqual(pos, codePos.CharPosition);
            Assert.AreEqual(length, codePos.Length);
        }

        [Test]
        public void ConstructorTest2()
        {
            var pos = new CharPosition(1, 5);

            var codePos = new CodePosition(SourceName, pos);

            Assert.AreEqual(SourceName, codePos.SourceName);
            Assert.AreEqual(pos, codePos.CharPosition);
            Assert.AreEqual(0, codePos.Length);
        }

        [Test]
        public void ConstructorError()
        {
            var pos = new CharPosition(1, 5);
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentOutOfRangeException>(() => new CodePosition(SourceName, pos, -1));
        }

        [Test]
        [TestCase(1, 5, 3)]
        [TestCase(2, 1, 0)]
        public void ToStringTest(int line, int column, int length)
        {
            var pos = new CharPosition(line, column);

            var codePos = new CodePosition(SourceName, pos, length);
            var codePosString = codePos.ToString();

            StringAssert.Contains(pos.ToString(), codePosString);
            StringAssert.Contains(SourceName, codePosString);
        }

        [Test]
        [TestCase(1, 5, 3, 2, 4, 2)]
        [TestCase(1, 1, 0, 1, 2, 0)]
        [TestCase(1, 1, 0, 2, 1, 0)]
        [TestCase(1, 1, 0, 1, 1, 1)]
        [SuppressMessage("ReSharper", "EqualExpressionComparison")]
        [SuppressMessage("ReSharper", "RedundantCast")]
        public void EqualsTest(
            int line1, int column1, int length1,
            int line2, int column2, int length2)
        {
            var pos1 = new CharPosition(line1, column1);
            var pos2 = new CharPosition(line2, column2);
            
            var codePos1 = new CodePosition(SourceName, pos1, length1);
            var codePos2 = new CodePosition(SourceName, pos2, length2);
            var codePos3 = new CodePosition(SourceName, pos1, length1);

            Assert.IsTrue(codePos1.Equals(codePos1));
            Assert.IsFalse(codePos1.Equals(codePos2));
            Assert.IsTrue(codePos1.Equals(codePos3));
            Assert.IsFalse(codePos1.Equals(null));
            Assert.IsFalse(codePos1.Equals(new object()));

            Assert.IsFalse(codePos1 == codePos2);
            Assert.IsTrue(codePos1 == codePos3);
            Assert.IsFalse(codePos1 == null);

            Assert.IsTrue(codePos1 != codePos2);
            Assert.IsFalse(codePos1 != codePos3);
            Assert.IsTrue(codePos1 != null);

            Assert.IsFalse(null == codePos1);
            Assert.IsTrue((CodePosition)null == (CodePosition)null);
        }

        [Test]
        [TestCase(1, 5, 3, 2, 4, 2)]
        [TestCase(1, 1, 0, 1, 2, 0)]
        [TestCase(1, 1, 0, 2, 1, 0)]
        [TestCase(1, 1, 0, 1, 1, 1)]
        public void GetHashCodeTest(
            int line1, int column1, int length1,
            int line2, int column2, int length2)
        {
            var pos1 = new CharPosition(line1, column1);
            var pos2 = new CharPosition(line2, column2);

            var codePos1 = new CodePosition(SourceName, pos1, length1);
            var codePos2 = new CodePosition(SourceName, pos2, length2);
            var codePos3 = new CodePosition(SourceName, pos1, length1);

            Assert.AreEqual(codePos1.GetHashCode(), codePos1.GetHashCode());
            Assert.AreNotEqual(codePos1.GetHashCode(), codePos2.GetHashCode());
            Assert.AreEqual(codePos1.GetHashCode(), codePos3.GetHashCode());
        }
    }
}
