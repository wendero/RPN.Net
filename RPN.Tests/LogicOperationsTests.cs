using System;
using NUnit.Framework;

namespace RPN.Tests
{
    public class LogicOperationsTests : TestBase
    {
        [TestCase("Greater than", "5 3 >", true)]
        [TestCase("Less than", "5 3 <", false)]
        [TestCase("Equals Integer", "3 3 =", true)]
        [TestCase("Equals Decimal True", "3.5 3.5 =", true)]
        [TestCase("Equals Decimal False", "3.5 3.6 =", false)]
        [TestCase("Equals Boolean", "true true =", true)]
        [TestCase("Equals Text", "`abc` `abc` =", true)]
        [TestCase("Different False", "3 3 !=", false)]
        [TestCase("Different True", "3 4 !=", true)]
        [TestCase("Different <>", "3 3 <>", false)]
        [TestCase("Logical And", "true true &", true)]
        [TestCase("Logical Or", "true false |", true)]
        [TestCase("Logical Not", "true !", false)]
        [TestCase("Greater or equals to", "5 3 >= 3 3 >= &", true)]
        [TestCase("Less or equals to", "3 5 <= 3 3 <= &", true)]
        public void TestLogicOperations(string testName, string expression, dynamic expectedValue, params object[] objects)
        {
            Test(testName, expression, expectedValue, objects);
        }
    }
}