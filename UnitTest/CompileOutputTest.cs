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
        private const string sourceName = "sourceName";
        private const int codePosLength = 3;
        private static readonly CodePosition codePos = new CodePosition(sourceName, pos, codePosLength);

        private static OutputLogger logger = new OutputLogger();

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            logger.ReportInfo(number, code, sourceCode, codePos, appendix);
            logger.ReportWarn(1, code, sourceCode, codePos, appendix);
            logger.ReportError(2, code, sourceCode, codePos, appendix);
            logger.ReportInfo(3, code, sourceCode, codePos, appendix);
            logger.ReportWarn(4, code, sourceCode, codePos, appendix);
            logger.ReportError(5, code, sourceCode, codePos, appendix);

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
            Assert.AreEqual(codePos, output.Position);
            Assert.AreEqual(appendix, output.Appendix);
        }

        [TestMethod]
        public void OutputNumberTest()
        {
            CompileOutput output = logger.Outputs.First();
            Assert.AreEqual(number, output.OutputNumber);
        }

        [TestMethod]
        public void CategoryTest()
        {
            CompileOutput output = logger.Outputs.First();
            Assert.AreEqual(OutputCategory.Info, output.Category);
        }

        [TestMethod]
        public void PositionTest()
        {
            CompileOutput output = logger.Outputs.First();
            Assert.AreEqual(codePos, output.Position);
        }

        [TestMethod]
        public void CodeTest()
        {
            CompileOutput output = logger.Outputs.First();
            Assert.AreEqual(code, output.Code);
        }

        [TestMethod]
        public void SourceCodeTest()
        {
            CompileOutput output = logger.Outputs.First();
            Assert.AreEqual(sourceCode, output.SourceCode);
        }

        [TestMethod]
        public void AppendixTest()
        {
            CompileOutput output = logger.Outputs.First();
            Assert.AreEqual(appendix, output.Appendix);
        }

        [TestMethod]
        public void MessageTest()
        {
            CompileOutput[] outputs = logger.Outputs.ToArray();
            Assert.AreEqual(MessageProviderInfo.Message, outputs[0].Message);
            Assert.AreEqual(MessageProviderWarn.Message, outputs[1].Message);
            Assert.AreEqual(MessageProviderError.Message, outputs[2].Message);
            Assert.IsNull(outputs[3].Message);
            Assert.IsNull(outputs[4].Message);
            Assert.IsNull(outputs[5].Message);
        }

        [TestMethod]
        public void SuggestionTest()
        {
            CompileOutput[] outputs = logger.Outputs.ToArray();
            Assert.AreEqual(MessageProviderInfo.Suggestion, outputs[0].Suggestion);
            Assert.AreEqual(MessageProviderWarn.Suggestion, outputs[1].Suggestion);
            Assert.AreEqual(MessageProviderError.Suggestion, outputs[2].Suggestion);
            Assert.IsNull(outputs[3].Suggestion);
            Assert.IsNull(outputs[4].Suggestion);
            Assert.IsNull(outputs[5].Suggestion);
        }

        [TestMethod]
        public void SiteLinkTest()
        {
            CompileOutput[] outputs = logger.Outputs.ToArray();
            Assert.AreEqual(MessageProviderInfo.SiteLink, outputs[0].SiteLink);
            Assert.AreEqual(MessageProviderWarn.SiteLink, outputs[1].SiteLink);
            Assert.AreEqual(MessageProviderError.SiteLink, outputs[2].SiteLink);
            Assert.IsNull(outputs[3].SiteLink);
            Assert.IsNull(outputs[4].SiteLink);
            Assert.IsNull(outputs[5].SiteLink);
        }
    }

    class MessageProviderInfo : IMessageProvider
    {
        public const string Message = "MessageInfo";
        public const string Suggestion = "SuggestionInfo";
        public const string SiteLink = "SiteLinkInfo";

        public bool GetMessage(int number, OutputCategory category, out string message)
        {
            message = null;

            if (number == 0 && category == OutputCategory.Info)
            {
                message = Message;
                return true;
            }
            else
                return false;
        }

        public bool GetSuggestion(int number, OutputCategory category, out string suggestion)
        {
            suggestion = null;

            if (number == 0 && category == OutputCategory.Info)
            {
                suggestion = Suggestion;
                return true;
            }
            else
                return false;
        }

        public bool GetSiteLink(int number, OutputCategory category, out string siteLink)
        {
            siteLink = null;

            if (number == 0 && category == OutputCategory.Info)
            {
                siteLink = SiteLink;
                return true;
            }
            else
                return false;
        }
    }

    class MessageProviderWarn : IMessageProvider
    {
        public const string Message = "MessageWarn";
        public const string Suggestion = "SuggestionWarn";
        public const string SiteLink = "SiteLinkWarn";

        public bool GetMessage(int number, OutputCategory category, out string message)
        {
            message = null;

            if (number == 1 && category == OutputCategory.Warn)
            {
                message = Message;
                return true;
            }
            else
                return false;
        }

        public bool GetSuggestion(int number, OutputCategory category, out string suggestion)
        {
            suggestion = null;

            if (number == 1 && category == OutputCategory.Warn)
            {
                suggestion = Suggestion;
                return true;
            }
            else
                return false;
        }

        public bool GetSiteLink(int number, OutputCategory category, out string siteLink)
        {
            siteLink = null;

            if (number == 1 && category == OutputCategory.Warn)
            {
                siteLink = SiteLink;
                return true;
            }
            else
                return false;
        }
    }

    class MessageProviderError : IMessageProvider
    {
        public const string Message = "MessageError";
        public const string Suggestion = "SuggestionError";
        public const string SiteLink = "SiteLinkError";

        public bool GetMessage(int number, OutputCategory category, out string message)
        {
            message = null;

            if (number == 2 && category == OutputCategory.Error)
            {
                message = Message;
                return true;
            }
            else
                return false;
        }

        public bool GetSuggestion(int number, OutputCategory category, out string suggestion)
        {
            suggestion = null;

            if (number == 2 && category == OutputCategory.Error)
            {
                suggestion = Suggestion;
                return true;
            }
            else
                return false;
        }

        public bool GetSiteLink(int number, OutputCategory category, out string siteLink)
        {
            siteLink = null;

            if (number == 2 && category == OutputCategory.Error)
            {
                siteLink = SiteLink;
                return true;
            }
            else
                return false;
        }
    }
}
