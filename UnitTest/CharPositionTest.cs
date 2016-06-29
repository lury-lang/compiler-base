using System;
using System.Diagnostics.CodeAnalysis;
using Lury.Compiling.Utils;
using NUnit.Framework;

namespace UnitTest
{
    [TestFixture]
    public class CharPositionTest
    {
        [Test]
        [TestCase(1, 1)]
        [TestCase(1, 5)]
        [TestCase(3, 1)]
        [TestCase(3, 5)]
        public void CtorTest(int line, int column)
        {
            var pos = new CharPosition(line, column);

            Assert.AreEqual(line, pos.Line);
            Assert.AreEqual(column, pos.Column);
        }

        [Test]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(0, 1)]
        [TestCase(0, 0)]
        public void CtorError(int line, int column)
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentOutOfRangeException>(() => new CharPosition(line, column));
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(1, 5)]
        [TestCase(3, 1)]
        [TestCase(3, 5)]
        public void IsEmptyTest1(int line, int column)
        {
            Assert.IsFalse(new CharPosition(line, column).IsEmpty);
        }

        [Test]
        public void IsEmptyTest2()
        {
            Assert.IsTrue(new CharPosition().IsEmpty);
            Assert.IsTrue(CharPosition.Empty.IsEmpty);
        }

        [Test]
        public void ConstructorTest()
        {
            var pos = new CharPosition(3, 5);
            Assert.AreEqual(3, pos.Line);
            Assert.AreEqual(5, pos.Column);
        }

        [Test]
        [TestCase(0, 5)]
        [TestCase(3, 0)]
        public void ConstructorError(int line, int column)
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentOutOfRangeException>(() => new CharPosition(line, column));
        }

        [Test]
        public void ToStringTest()
        {
            var pos = new CharPosition(3, 5);

            Assert.IsTrue(pos.ToString().Contains(3.ToString()));
            Assert.IsTrue(pos.ToString().Contains(5.ToString()));
        }

        [Test]
        [SuppressMessage("ReSharper", "EqualExpressionComparison")]
        public void EqualsTest()
        {
            var pos1 = new CharPosition(3, 5);
            var pos2 = new CharPosition(3, 5);
            var pos3 = new CharPosition(3, 7);
            var pos4 = new CharPosition(7, 5);

            Assert.IsFalse(pos1.Equals(new object()));
            Assert.IsTrue(pos1.Equals(pos1));
            Assert.IsTrue(pos1.Equals(pos2));
            Assert.IsFalse(pos1.Equals(pos3));
            Assert.IsFalse(pos1.Equals(pos4));
        }

        [Test]
        public void GetHashCodeTest()
        {
            var pos1 = new CharPosition(3, 5);
            var pos2 = new CharPosition(3, 5);

            Assert.IsTrue(pos1.GetHashCode() == pos2.GetHashCode());
        }

        [Test]
        public void EqualsOperatorTest()
        {
            var pos1 = new CharPosition(3, 5);
            var pos2 = new CharPosition(3, 5);
            var pos3 = new CharPosition(3, 7);
            var pos4 = new CharPosition(7, 5);

            Assert.IsTrue(pos1 == pos2);
            Assert.IsFalse(pos1 == pos3);
            Assert.IsFalse(pos1 == pos4);
        }

        [Test]
        public void NotEqualsOperatorTest()
        {
            var pos1 = new CharPosition(3, 5);
            var pos2 = new CharPosition(3, 5);
            var pos3 = new CharPosition(3, 7);
            var pos4 = new CharPosition(7, 5);

            Assert.IsFalse(pos1 != pos2);
            Assert.IsTrue(pos1 != pos3);
            Assert.IsTrue(pos1 != pos4);
        }
    }
}
