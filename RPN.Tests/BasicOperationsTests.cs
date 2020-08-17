using System;
using NUnit.Framework;

namespace RPN.Tests
{
    public class BasicOperationsTests : TestBase
    {
        [TestCase("Add", "1 1 + 2 2 2 + + +", 8)]
        [TestCase("Subtract", "1 1 - 2 2 2 - - -", -2)]
        [TestCase("Multiplication", "2 5 * 2 3 * *", 60)]
        [TestCase("Division", "20 2 / 2 /", 5)]
        [TestCase("Remainder", "11 3 %", 2)]
        [TestCase("Percent", "500 10% +", 550)]
        [TestCase("Percent", "500 20% -", 400)]
        [TestCase("Percentage", "1000 670 perc", 67)]
        public void TestBasicOperations(string testName, string expression, dynamic expectedValue, params object[] objects)
        {
            Test(testName, expression, expectedValue, objects);
        }
    }
}