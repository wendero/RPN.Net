using System;
using NUnit.Framework;

namespace RPN.Tests
{
    public abstract class TestBase
    {
        public void Test(string name, string exp, dynamic expected)
        {
            Test(name, exp, expected, null);
        }
        public void Test(string name, string exp, dynamic expected, params object[] objects)
        {
            string strVal = "";
            bool status = false;

            try
            {
                var val = RPN.Eval(exp, objects);
                strVal = val.ToString();
                status = expected == val;

                Assert.AreEqual(expected, val);
            }
            catch
            {
                throw;
            }

            Console.WriteLine($"{name} => RPN: {exp} :: Expected: {expected} :: Value: {strVal} :: Test: {(status ? "PASS" : "FAIL")}");
        }
    }
}