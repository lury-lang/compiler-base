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
        public void LineTest()
        {
            var pos = new CharPosition(1, 5);
            Assert.AreEqual(1, pos.Line);
            pos = new CharPosition(3, pos.Column);
            Assert.AreEqual(3, pos.Line);
        }

        [Test]
        public void LineError()
        {
            var pos = new CharPosition(1, 5);

            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentOutOfRangeException>(() => new CharPosition(0, pos.Column));
        }

        [Test]
        public void ColumnTest()
        {
            var pos = new CharPosition(1, 5);
            Assert.AreEqual(5, pos.Column);
            pos = new CharPosition(pos.Line, 2);
            Assert.AreEqual(2, pos.Column);
        }

        [Test]
        public void ColumnError()
        {
            var pos = new CharPosition(1, 5);

            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentOutOfRangeException>(() => new CharPosition(pos.Line, 0));
        }

        [Test]
        public void IsEmptyTest()
        {
            Assert.IsFalse(new CharPosition(1, 5).IsEmpty);
            Assert.IsFalse(new CharPosition(1, 1).IsEmpty);
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
