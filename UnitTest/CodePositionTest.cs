using System;
using System.Linq;
using Lury.Compiling.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class CodePositionTest
    {
        private const string SourceName = "name";

        [TestMethod]
        public void ConstructorTest1()
        {
            CharPosition pos = new CharPosition(1, 5);
            int length = 3;

            CodePosition codePos = new CodePosition(SourceName, pos, length);

            Assert.AreEqual(SourceName, codePos.SourceName);
            Assert.AreEqual(pos, codePos.Position);
            Assert.AreEqual(length, codePos.Length);
        }

        [TestMethod]
        public void ConstructorTest2()
        {
            CharPosition pos = new CharPosition(1, 5);

            CodePosition codePos = new CodePosition(SourceName, pos);

            Assert.AreEqual(SourceName, codePos.SourceName);
            Assert.AreEqual(pos, codePos.Position);
            Assert.AreEqual(0, codePos.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ConstructorError()
        {
            CharPosition pos = new CharPosition(1, 5);

            CodePosition codePos = new CodePosition(SourceName, pos, -1);
        }

        [TestMethod]
        public void ToStringTest()
        {
            CharPosition pos = new CharPosition(1, 5);
            int length = 3;

            CodePosition codePos = new CodePosition(SourceName, pos, length);
            string codePosString = codePos.ToString();

            StringAssert.Contains(codePosString, pos.ToString());
            StringAssert.Contains(codePosString, SourceName);
        }

        [TestMethod]
        public void EqualsTest()
        {
            CharPosition pos1 = new CharPosition(1, 5);
            int length1 = 3;

            CharPosition pos2 = new CharPosition(2, 4);
            int length2 = 2;

            CodePosition codePos1 = new CodePosition(SourceName, pos1, length1);
            CodePosition codePos2 = new CodePosition(SourceName, pos2, length2);
            CodePosition codePos3 = new CodePosition(SourceName, pos1, length1);

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

        [TestMethod]
        public void GetHashCodeTest()
        {
            CharPosition pos1 = new CharPosition(1, 5);
            int length1 = 3;

            CharPosition pos2 = new CharPosition(2, 4);
            int length2 = 2;

            CodePosition codePos1 = new CodePosition(SourceName, pos1, length1);
            CodePosition codePos2 = new CodePosition(SourceName, pos2, length2);
            CodePosition codePos3 = new CodePosition(SourceName, pos1, length1);

            Assert.AreEqual(codePos1.GetHashCode(), codePos1.GetHashCode());
            Assert.AreNotEqual(codePos1.GetHashCode(), codePos2.GetHashCode());
            Assert.AreEqual(codePos1.GetHashCode(), codePos3.GetHashCode());
        }
    }
}
