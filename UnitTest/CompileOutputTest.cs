using System.Linq;
using Lury.Compiling.Logger;
using Lury.Compiling.Utils;
using NUnit.Framework;

namespace UnitTest
{
    [TestFixture]
    public class CompileOutputTest
    {
        private const int Number = 0;
        private const string Code = "code";
        private const string SourceCode = "source code";
        private static readonly CharPosition Pos = new CharPosition(1, 8);
        private const string Appendix = "appendix";
        private const string SourceName = "sourceName";
        private const int CodePosLength = 3;
        private static readonly CodePosition CodePos = new CodePosition(SourceName, Pos, CodePosLength);

        private static readonly OutputLogger Logger = new OutputLogger();

        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            Logger.ReportInfo(Number, Code, SourceCode, CodePos, Appendix);
            Logger.ReportWarn(1, Code, SourceCode, CodePos, Appendix);
            Logger.ReportError(2, Code, SourceCode, CodePos, Appendix);
            Logger.ReportInfo(3, Code, SourceCode, CodePos, Appendix);
            Logger.ReportWarn(4, Code, SourceCode, CodePos, Appendix);
            Logger.ReportError(5, Code, SourceCode, CodePos, Appendix);

            CompileOutput.MessageProviders.Add(new MessageProviderInfo());
            CompileOutput.MessageProviders.Add(new MessageProviderWarn());
            CompileOutput.MessageProviders.Add(new MessageProviderError());
        }

        [Test]
        public void ConstructorTest()
        {
            var output = Logger.Outputs.First();

            Assert.AreEqual(OutputCategory.Info, output.Category);
            Assert.AreEqual(Number, output.OutputNumber);
            Assert.AreEqual(Code, output.Code);
            Assert.AreEqual(SourceCode, output.SourceCode);
            Assert.AreEqual(CodePos, output.CodePosition);
            Assert.AreEqual(Appendix, output.Appendix);
        }

        [Test]
        public void OutputNumberTest()
        {
            var output = Logger.Outputs.First();
            Assert.AreEqual(Number, output.OutputNumber);
        }

        [Test]
        public void CategoryTest()
        {
            var output = Logger.Outputs.First();
            Assert.AreEqual(OutputCategory.Info, output.Category);
        }

        [Test]
        public void PositionTest()
        {
            var output = Logger.Outputs.First();
            Assert.AreEqual(CodePos, output.CodePosition);
        }

        [Test]
        public void CodeTest()
        {
            var output = Logger.Outputs.First();
            Assert.AreEqual(Code, output.Code);
        }

        [Test]
        public void SourceCodeTest()
        {
            var output = Logger.Outputs.First();
            Assert.AreEqual(SourceCode, output.SourceCode);
        }

        [Test]
        public void AppendixTest()
        {
            var output = Logger.Outputs.First();
            Assert.AreEqual(Appendix, output.Appendix);
        }

        [Test]
        public void MessageTest()
        {
            var outputs = Logger.Outputs.ToArray();
            Assert.AreEqual(MessageProviderInfo.Message, outputs[0].Message);
            Assert.AreEqual(MessageProviderWarn.Message, outputs[1].Message);
            Assert.AreEqual(MessageProviderError.Message, outputs[2].Message);
            Assert.IsNull(outputs[3].Message);
            Assert.IsNull(outputs[4].Message);
            Assert.IsNull(outputs[5].Message);
        }

        [Test]
        public void SuggestionTest()
        {
            var outputs = Logger.Outputs.ToArray();
            Assert.AreEqual(MessageProviderInfo.Suggestion, outputs[0].Suggestion);
            Assert.AreEqual(MessageProviderWarn.Suggestion, outputs[1].Suggestion);
            Assert.AreEqual(MessageProviderError.Suggestion, outputs[2].Suggestion);
            Assert.IsNull(outputs[3].Suggestion);
            Assert.IsNull(outputs[4].Suggestion);
            Assert.IsNull(outputs[5].Suggestion);
        }

        [Test]
        public void SiteLinkTest()
        {
            var outputs = Logger.Outputs.ToArray();
            Assert.AreEqual(MessageProviderInfo.SiteLink, outputs[0].SiteLink);
            Assert.AreEqual(MessageProviderWarn.SiteLink, outputs[1].SiteLink);
            Assert.AreEqual(MessageProviderError.SiteLink, outputs[2].SiteLink);
            Assert.IsNull(outputs[3].SiteLink);
            Assert.IsNull(outputs[4].SiteLink);
            Assert.IsNull(outputs[5].SiteLink);
        }
    }

    internal class MessageProviderInfo : IMessageProvider
    {
        public const string Message = "MessageInfo";
        public const string Suggestion = "SuggestionInfo";
        public const string SiteLink = "SiteLinkInfo";

        public bool GetMessage(int number, OutputCategory category, out string message)
        {
            message = null;

            if (number != 0 || category != OutputCategory.Info)
                return false;

            message = Message;
            return true;
        }

        public bool GetSuggestion(int number, OutputCategory category, out string suggestion)
        {
            suggestion = null;

            if (number != 0 || category != OutputCategory.Info)
                return false;

            suggestion = Suggestion;
            return true;
        }

        public bool GetSiteLink(int number, OutputCategory category, out string siteLink)
        {
            siteLink = null;

            if (number != 0 || category != OutputCategory.Info)
                return false;

            siteLink = SiteLink;
            return true;
        }
    }

    internal class MessageProviderWarn : IMessageProvider
    {
        public const string Message = "MessageWarn";
        public const string Suggestion = "SuggestionWarn";
        public const string SiteLink = "SiteLinkWarn";

        public bool GetMessage(int number, OutputCategory category, out string message)
        {
            message = null;

            if (number != 1 || category != OutputCategory.Warn)
                return false;

            message = Message;
            return true;
        }

        public bool GetSuggestion(int number, OutputCategory category, out string suggestion)
        {
            suggestion = null;

            if (number != 1 || category != OutputCategory.Warn)
                return false;

            suggestion = Suggestion;
            return true;
        }

        public bool GetSiteLink(int number, OutputCategory category, out string siteLink)
        {
            siteLink = null;

            if (number != 1 || category != OutputCategory.Warn)
                return false;

            siteLink = SiteLink;
            return true;
        }
    }

    internal class MessageProviderError : IMessageProvider
    {
        public const string Message = "MessageError";
        public const string Suggestion = "SuggestionError";
        public const string SiteLink = "SiteLinkError";

        public bool GetMessage(int number, OutputCategory category, out string message)
        {
            message = null;

            if (number != 2 || category != OutputCategory.Error)
                return false;

            message = Message;
            return true;
        }

        public bool GetSuggestion(int number, OutputCategory category, out string suggestion)
        {
            suggestion = null;

            if (number != 2 || category != OutputCategory.Error)
                return false;

            suggestion = Suggestion;
            return true;
        }

        public bool GetSiteLink(int number, OutputCategory category, out string siteLink)
        {
            siteLink = null;

            if (number != 2 || category != OutputCategory.Error)
                return false;

            siteLink = SiteLink;
            return true;
        }
    }
}
