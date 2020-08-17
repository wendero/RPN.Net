using System;
using System.Text.Json;
using NUnit.Framework;

namespace RPN.Tests
{
    public class StringOperationsTests : TestBase
    {
        [TestCase("Uppercase", "bazinga ucase", "BAZINGA")]
        [TestCase("Lowercase", "BAZINGA lcase", "bazinga")]
        [TestCase("String Format", "hel wor lo ld `{0}{2} {1}{3}` strfmt", "hello world")]
        [TestCase("Date Default Format", "2018 12 31 23 59 58 123 7 date default todate", "2018-12-31 23:59:58")]
        [TestCase("Date Custom Format", "2018 12 31 23 59 58 123 7 date yyyy-MM-dd todate", "2018-12-31")]
        public void TestStringOperations(string testName, string expression, dynamic expectedValue, params object[] objects)
        {
            Test(testName, expression, expectedValue, objects);
        }
        [TestCase("Parse JSON", "`{\"Name\": \"Bazinga\"}` parse $0.Name", "Bazinga")]
        [TestCase("Parse JSON", "`{\"Key\":123}` parse $0.Key", 123)]
        public void TestParsing(string testName, string expression, dynamic expectedValue, params object[] objects)
        {
            Test(testName, expression, expectedValue, objects);
        }
        [TestCase("Date System Format", "2018 12 31 23 59 58 123 7 date")]
        public void TestDateSystemFormat(string testName, string expression)
        {
            Test(testName, expression, new DateTime(2018, 12, 31, 23, 59, 58).ToString());
        }
        [Test]
        public void TestStringify()
        {
            var json = JsonSerializer.Serialize(new { Name = "Bazinga" });
            Test("Stringify", "$0 stringify", json, new { Name = "Bazinga" });
        }
    }
}