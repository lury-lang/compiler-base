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
        public void ToStringTest()
        {
            var pos = new CharPosition(1, 5);
            const int length = 3;

            var codePos = new CodePosition(SourceName, pos, length);
            var codePosString = codePos.ToString();

            StringAssert.Contains(codePosString, pos.ToString());
            StringAssert.Contains(codePosString, SourceName);
        }

        [Test]
        [SuppressMessage("ReSharper", "EqualExpressionComparison")]
        [SuppressMessage("ReSharper", "RedundantCast")]
        public void EqualsTest()
        {
            var pos1 = new CharPosition(1, 5);
            const int length1 = 3;

            var pos2 = new CharPosition(2, 4);
            const int length2 = 2;

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
        public void GetHashCodeTest()
        {
            var pos1 = new CharPosition(1, 5);
            const int length1 = 3;

            var pos2 = new CharPosition(2, 4);
            const int length2 = 2;

            var codePos1 = new CodePosition(SourceName, pos1, length1);
            var codePos2 = new CodePosition(SourceName, pos2, length2);
            var codePos3 = new CodePosition(SourceName, pos1, length1);

            Assert.AreEqual(codePos1.GetHashCode(), codePos1.GetHashCode());
            Assert.AreNotEqual(codePos1.GetHashCode(), codePos2.GetHashCode());
            Assert.AreEqual(codePos1.GetHashCode(), codePos3.GetHashCode());
        }
    }
}
