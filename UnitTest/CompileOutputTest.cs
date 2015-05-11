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
        private const int number = 42;
        private const string code = "code";
        private const string sourceCode = "source code";
        private readonly CharPosition pos = new CharPosition(1, 8);
        private const string appendix = "appendix";

        [TestMethod]
        public void ConstructorTest()
        {
            OutputLogger logger = new OutputLogger();
            logger.ReportInfo(number, code, sourceCode, pos, appendix);
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
