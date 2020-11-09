using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace RPN.Tests
{
    public class DateTimeOperationsTests : TestBase
    {
        [TestCase("Date Year", "2020 1 date default datefmt", "2020-01-01 00:00:00")]
        [TestCase("Date Year and Month", "2020 12 2 date default datefmt", "2020-12-01 00:00:00")]
        [TestCase("Date Simple", "2020 12 31 3 date default datefmt", "2020-12-31 00:00:00")]
        public void TestDateTimeConversion(string testName, string expression, dynamic expectedValue, params object[] objects)
        {
            Test(testName, expression, expectedValue, objects);
        }
        [TestCase("Date", "2020 12 31 15 16 17 6 date")]
        public void TestDateTimeObject(string testName, string expression, params object[] objects)
        {
            Test(testName, expression, new DateTime(2020, 12, 31, 15, 16, 17), objects);
        }
        [TestCaseSource("TimeSpanCases")]
        public void TestTimeSpan(string testName, string expression, dynamic expectedValue, params object[] objects)
        {
            Test(testName, expression, expectedValue, objects);
        }

        public static IEnumerable<TestCaseData> TimeSpanCases
        {
            get
            {
                yield return new TestCaseData("TimeSpan 3 days", "3 days", new TimeSpan(3, 0, 0, 0), null);
                yield return new TestCaseData("TimeSpan 5 hours", "5 hours", new TimeSpan(0, 5, 0, 0), null);
                yield return new TestCaseData("TimeSpan 7 minutes", "7 minutes", new TimeSpan(0, 0, 7, 0), null);
                yield return new TestCaseData("TimeSpan 9 seconds", "9 seconds", new TimeSpan(0, 0, 0, 9), null);
                yield return new TestCaseData("TimeSpan full", "3 5 7 9 ts", new TimeSpan(3, 5, 7, 9), null);
                yield return new TestCaseData("Add 1 hour", "2020 1 date 1 hour +", new DateTime(2020, 1, 1, 1, 0, 0), null);
                yield return new TestCaseData("Add 1 day", "2020 1 date 1 day +", new DateTime(2020, 1, 2), null);
                yield return new TestCaseData("Subtract 1 hour", "2020 1 date 1 hour -", new DateTime(2019, 12, 31, 23, 0, 0), null);
                yield return new TestCaseData("Subtract 1 day", "2020 1 date 1 day -", new DateTime(2019, 12, 31), null);
                yield return new TestCaseData("Add 1 month", "2020 1 date 1 month +", new DateTime(2020, 2, 1), null);
                yield return new TestCaseData("Add 1 year", "2020 1 date 1 year +", new DateTime(2021, 1, 1), null);
                yield return new TestCaseData("Add 2 months", "2020 1 date 2 months +", new DateTime(2020, 3, 1), null);
                yield return new TestCaseData("Add 2 years", "2020 1 date 2 years +", new DateTime(2022, 1, 1), null);
                yield return new TestCaseData("Subtract 1 month", "2020 1 date 1 month -", new DateTime(2019, 12, 1), null);
                yield return new TestCaseData("Subtract 1 year", "2020 1 date 1 year -", new DateTime(2019, 1, 1), null);
            }
        }
    }
}