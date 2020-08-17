using System;
using NUnit.Framework;

namespace RPN.Tests
{
    public class ControlOperationsTests : TestBase
    {
        [TestCase("Stack", "5 2 3 stack", "[5,2,3]")]
        [TestCase("Swap", "1 5 swap stack", "[5,1]")]
        [TestCase("Rotate", "1 2 3 rot stack", "[3,1,2]")]
        [TestCase("Duplicate", "100 dup +", 200)]
        [TestCase("Pop (discard entry)", "3 5 pop", 3)]
        [TestCase("Clear stack", "10 20 30 40 50 5 3 2 clr 1", 1)]
        [TestCase("Pop X", "1 2 4 8 50 16 32 64 3 popx sum", 127)]
        [TestCase("Data push", "5 dpush data", "[5]")]
        [TestCase("Data pop", "5 dpush dpop", 5)]
        [TestCase("Data clear", "5 dpush dclr data", "[]")]
        [TestCase("Return", "10 20 30 40 50 5 3 20 ret 30", 20)]
        [TestCase("Return ", "10 20 30 40 50 5 3 20 Bazinga ret", "Bazinga")]
        [TestCase("Return If", "tapang bazinga false ! retif", "bazinga")]
        [TestCase("Function and Call", "@a 10 20 + @a 3 @@a + 11 /", 3)]
        [TestCase("If", "true 10 if", 10)]
        [TestCase("If", "5 false 10 if", 5)]
        [TestCase("If-then-Else", "5 true 10 20 ife", 10)]
        [TestCase("If-then-Else", "5 false 10 20 ife", 20)]
        [TestCase("Case", "3 1 um case 2 dois case 3 tres case end", "tres")]
        [TestCase("Case", "1 1 um case 2 dois case 3 tres case end", "um")]
        [TestCase("Case", "5 1 um case 2 dois case 3 tres case outro end", "outro")]
        [TestCase("From Index", "10 20 30 40 3 1 fromindex", 30)]
        public void TestControlOperations(string testName, string expression, dynamic expectedValue, params object[] objects)
        {
            Test(testName, expression, expectedValue, objects);
        }
    }
}