using System;
using System.Linq;
using Lury.Compiling.Logger;
using Lury.Compiling.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class CompileOutputTest
    {
        private const int number = 0;
        private const string code = "code";
        private const string sourceCode = "source code";
        private static readonly CharPosition pos = new CharPosition(1, 8);
        private const string appendix = "appendix";

        private static OutputLogger logger = new OutputLogger();

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            logger.ReportInfo(number, code, sourceCode, pos, appendix);
            logger.ReportWarn(1, code, sourceCode, pos, appendix);
            logger.ReportError(2, code, sourceCode, pos, appendix);
            logger.ReportInfo(3, code, sourceCode, pos, appendix);
            logger.ReportWarn(4, code, sourceCode, pos, appendix);
            logger.ReportError(5, code, sourceCode, pos, appendix);

            CompileOutput.MessageProviders.Add(new MessageProviderInfo());
            CompileOutput.MessageProviders.Add(new MessageProviderWarn());
            CompileOutput.MessageProviders.Add(new MessageProviderError());
        }

        [TestMethod]
        public void ConstructorTest()
        {
            CompileOutput output = logger.Outputs.First();

            Assert.AreEqual(OutputCategory.Info, output.Category);
            Assert.AreEqual(number, output.OutputNumber);
            Assert.AreEqual(code, output.Code);
            Assert.AreEqual(sourceCode, output.SourceCode);
            Assert.AreEqual(pos, output.Position);
            Assert.AreEqual(appendix, output.Appendix);
        }
    }
}
