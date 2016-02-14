using System;
using System.Linq;
using Lury.Compiling.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class CharPositionTest
    {
        [TestMethod]
        public void LineTest()
        {
            CharPosition pos = new CharPosition(1, 5);
            Assert.AreEqual(1, pos.Line);
            pos = new CharPosition(3, pos.Column);
            Assert.AreEqual(3, pos.Line);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LineError()
        {
            CharPosition pos = new CharPosition(1, 5);
            pos = new CharPosition(0, pos.Column);
        }

        [TestMethod]
        public void ColumnTest()
        {
            CharPosition pos = new CharPosition(1, 5);
            Assert.AreEqual(5, pos.Column);
            pos = new CharPosition(pos.Line, 2);
            Assert.AreEqual(2, pos.Column);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ColumnError()
        {
            CharPosition pos = new CharPosition(1, 5);
            pos = new CharPosition(pos.Line, 0);
        }

        [TestMethod]
        public void IsEmptyTest()
        {
            Assert.IsFalse(new CharPosition(1, 5).IsEmpty);
            Assert.IsFalse(new CharPosition(1, 1).IsEmpty);
            Assert.IsTrue(new CharPosition().IsEmpty);
            Assert.IsTrue(CharPosition.Empty.IsEmpty);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            CharPosition pos = new CharPosition(3, 5);
            Assert.AreEqual(3, pos.Line);
            Assert.AreEqual(5, pos.Column);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ConstructorError1()
        {
            CharPosition pos = new CharPosition(0, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ConstructorError2()
        {
            CharPosition pos = new CharPosition(3, 0);
        }

        [TestMethod]
        public void ToStringTest()
        {
            CharPosition pos = new CharPosition(3, 5);
            Assert.IsTrue(pos.ToString().Contains(3.ToString()));
            Assert.IsTrue(pos.ToString().Contains(5.ToString()));
        }

        [TestMethod]
        public void EqualsTest()
        {
            CharPosition pos1 = new CharPosition(3, 5);
            CharPosition pos2 = new CharPosition(3, 5);
            CharPosition pos3 = new CharPosition(3, 7);
            CharPosition pos4 = new CharPosition(7, 5);
            Assert.IsFalse(pos1.Equals(new object()));
            Assert.IsTrue(pos1.Equals(pos1));
            Assert.IsTrue(pos1.Equals(pos2));
            Assert.IsFalse(pos1.Equals(pos3));
            Assert.IsFalse(pos1.Equals(pos4));
        }

        [TestMethod]
        public void GetHashCodeTest()
        {
            CharPosition pos1 = new CharPosition(3, 5);
            CharPosition pos2 = new CharPosition(3, 5);

            Assert.IsTrue(pos1.GetHashCode() == pos2.GetHashCode());
        }

        [TestMethod]
        public void EqualsOperatorTest()
        {
            CharPosition pos1 = new CharPosition(3, 5);
            CharPosition pos2 = new CharPosition(3, 5);
            CharPosition pos3 = new CharPosition(3, 7);
            CharPosition pos4 = new CharPosition(7, 5);
            Assert.IsTrue(pos1 == pos2);
            Assert.IsFalse(pos1 == pos3);
            Assert.IsFalse(pos1 == pos4);
        }

        [TestMethod]
        public void NotEqualsOperatorTest()
        {
            CharPosition pos1 = new CharPosition(3, 5);
            CharPosition pos2 = new CharPosition(3, 5);
            CharPosition pos3 = new CharPosition(3, 7);
            CharPosition pos4 = new CharPosition(7, 5);
            Assert.IsFalse(pos1 != pos2);
            Assert.IsTrue(pos1 != pos3);
            Assert.IsTrue(pos1 != pos4);
        }
    }
}
