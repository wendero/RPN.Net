using NUnit.Framework;

namespace RPN.Tests
{
    public class RegexOperationsTests : TestBase
    {
        [TestCase("Regex Match", @"`telefone: 371-8333 555-8888 321-4001 bazinga` rx/\d{3}\-\d{4}/ match $0[0].Value", "371-8333")]
        public void TestRegexOperations(string testName, string expression, dynamic expectedValue)
        {
            Test(testName, expression, expectedValue);
        }
    }
}