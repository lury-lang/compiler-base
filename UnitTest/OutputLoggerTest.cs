using System.Linq;
using Lury.Compiling.Logger;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class OutputLoggerTest
    {
        private const int number = 0;

        [TestMethod]
        public void OutputsTest()
        {
            OutputLogger logger = new OutputLogger();
            Assert.IsNotNull(logger.Outputs);
            Assert.IsFalse(logger.Outputs.Any());

            logger.ReportInfo(number);
            Assert.AreEqual(1, logger.Outputs.Count());
        }

        [TestMethod]
        public void InfoOutputsTest()
        {
            OutputLogger logger = new OutputLogger();

            logger.ReportInfo(number);
            Assert.AreEqual(1, logger.InfoOutputs.Count());
        }

        [TestMethod]
        public void WarnOutputsTest()
        {
            OutputLogger logger = new OutputLogger();

            logger.ReportWarn(number);
            Assert.AreEqual(1, logger.WarnOutputs.Count());
        }

        [TestMethod]
        public void ErrorOutputsTest()
        {
            OutputLogger logger = new OutputLogger();

            logger.ReportError(number);
            Assert.AreEqual(1, logger.ErrorOutputs.Count());
        }

        [TestMethod]
        public void ClearTest()
        {
            OutputLogger logger = new OutputLogger();
            logger.ReportInfo(number);
            logger.ReportWarn(number);
            logger.ReportError(number);
            Assert.AreEqual(3, logger.Outputs.Count());

            logger.Clear();
            Assert.AreEqual(0, logger.Outputs.Count());

            logger.Clear();
            Assert.AreEqual(0, logger.Outputs.Count());
        }
    }
}
