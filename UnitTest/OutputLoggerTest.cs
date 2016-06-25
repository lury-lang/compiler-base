using System;
using System.Linq;
using Lury.Compiling.Logger;
using NUnit.Framework;

namespace UnitTest
{
    [TestFixture]
    public class OutputLoggerTest
    {
        private const int Number = 0;

        [Test]
        public void OutputsTest()
        {
            var logger = new OutputLogger();
            Assert.IsNotNull(logger.Outputs);
            Assert.IsFalse(logger.Outputs.Any());

            logger.ReportInfo(Number);
            Assert.AreEqual(1, logger.Outputs.Count());
        }

        [Test]
        public void InfoOutputsTest()
        {
            var logger = new OutputLogger();

            logger.ReportInfo(Number);
            Assert.AreEqual(1, logger.InfoOutputs.Count());
        }

        [Test]
        public void WarnOutputsTest()
        {
            var logger = new OutputLogger();

            logger.ReportWarn(Number);
            Assert.AreEqual(1, logger.WarnOutputs.Count());
        }

        [Test]
        public void ErrorOutputsTest()
        {
            var logger = new OutputLogger();

            logger.ReportError(Number);
            Assert.AreEqual(1, logger.ErrorOutputs.Count());
        }

        [Test]
        public void ClearTest()
        {
            var logger = new OutputLogger();
            logger.ReportInfo(Number);
            logger.ReportWarn(Number);
            logger.ReportError(Number);
            Assert.AreEqual(3, logger.Outputs.Count());

            logger.Clear();
            Assert.AreEqual(0, logger.Outputs.Count());

            logger.Clear();
            Assert.AreEqual(0, logger.Outputs.Count());
        }

        [Test]
        public void CopyToTest()
        {
            var logger1 = new OutputLogger();
            logger1.ReportInfo(Number);
            logger1.ReportWarn(Number);
            logger1.ReportError(Number);

            var logger2 = new OutputLogger();
            logger1.CopyTo(logger2);

            Assert.AreEqual(3, logger2.Outputs.Count());
            Assert.AreEqual(1, logger2.InfoOutputs.Count());
            Assert.AreEqual(1, logger2.WarnOutputs.Count());
            Assert.AreEqual(1, logger2.ErrorOutputs.Count());
        }

        [Test]
        public void CopyToError()
        {
            var logger = new OutputLogger();
            Assert.Throws<ArgumentNullException>(() => logger.CopyTo(null));
        }
    }
}
