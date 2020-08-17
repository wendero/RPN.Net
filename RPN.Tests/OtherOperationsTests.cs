using System;
using NUnit.Framework;

namespace RPN.Tests
{
    public class OtherOperationsTests : TestBase
    {
        [TestCase("Increment 1", "5 ++", 6)]
        [TestCase("Decrement 1", "5 --", 4)]
        [TestCase("Minimum", "10 20 30 40 50 5 3 2 min", 2)]
        [TestCase("Maximum", "10 20 30 40 50 5 3 2 max", 50)]
        public void TestOtherOperations(string testName, string expression, dynamic expectedValue, params object[] objects)
        {
            Test(testName, expression, expectedValue, objects);
        }
    }
}