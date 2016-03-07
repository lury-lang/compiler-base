using System;
using System.Linq;
using Lury.Compiling.Logger;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class OutputLoggerTest
    {
        private const int Number = 0;

        [TestMethod]
        public void OutputsTest()
        {
            OutputLogger logger = new OutputLogger();
            Assert.IsNotNull(logger.Outputs);
            Assert.IsFalse(logger.Outputs.Any());

            logger.ReportInfo(Number);
            Assert.AreEqual(1, logger.Outputs.Count());
        }

        [TestMethod]
        public void InfoOutputsTest()
        {
            OutputLogger logger = new OutputLogger();

            logger.ReportInfo(Number);
            Assert.AreEqual(1, logger.InfoOutputs.Count());
        }

        [TestMethod]
        public void WarnOutputsTest()
        {
            OutputLogger logger = new OutputLogger();

            logger.ReportWarn(Number);
            Assert.AreEqual(1, logger.WarnOutputs.Count());
        }

        [TestMethod]
        public void ErrorOutputsTest()
        {
            OutputLogger logger = new OutputLogger();

            logger.ReportError(Number);
            Assert.AreEqual(1, logger.ErrorOutputs.Count());
        }

        [TestMethod]
        public void ClearTest()
        {
            OutputLogger logger = new OutputLogger();
            logger.ReportInfo(Number);
            logger.ReportWarn(Number);
            logger.ReportError(Number);
            Assert.AreEqual(3, logger.Outputs.Count());

            logger.Clear();
            Assert.AreEqual(0, logger.Outputs.Count());

            logger.Clear();
            Assert.AreEqual(0, logger.Outputs.Count());
        }

        [TestMethod]
        public void CopyToTest()
        {
            OutputLogger logger1 = new OutputLogger();
            logger1.ReportInfo(Number);
            logger1.ReportWarn(Number);
            logger1.ReportError(Number);

            OutputLogger logger2 = new OutputLogger();
            logger1.CopyTo(logger2);

            Assert.AreEqual(3, logger2.Outputs.Count());
            Assert.AreEqual(1, logger2.InfoOutputs.Count());
            Assert.AreEqual(1, logger2.WarnOutputs.Count());
            Assert.AreEqual(1, logger2.ErrorOutputs.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyToError()
        {
            OutputLogger logger = new OutputLogger();
            logger.CopyTo(null);
        }
    }
}
