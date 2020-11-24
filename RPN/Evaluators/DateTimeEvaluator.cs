using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using RPN;
using RPN.Helpers;

namespace RPN.Evaluators
{
    internal class DateTimeEvaluator
    {
        private static string[] OPERATORS = new string[] { "date", "days", "day", "hours", "hour", "minutes", "minute", "seconds", "second", "ts", "+", "-", "year", "years", "month", "months" };

        internal static bool Evaluate(RPNContext context)
        {
            if (OPERATORS.Contains(context.Current))
            {
                switch (context.Current)
                {
                    case "date":
                        {
                            var len = context.Stack.Pop();
                            var parts = new List<int>();
                            for (var i = 0; i < len; i++)
                                parts.Add(Convert.ToInt32(context.Stack.Pop()));
                            parts.Reverse();

                            DateTime date = new DateTime();
                            date = date.AddYears(parts.Count > 0 ? parts[0] - 1 : 0);
                            date = date.AddMonths(parts.Count > 1 ? parts[1] - 1 : 0);
                            date = date.AddDays(parts.Count > 2 ? parts[2] - 1 : 0);
                            date = date.AddHours(parts.Count > 3 ? parts[3] : 0);
                            date = date.AddMinutes(parts.Count > 4 ? parts[4] : 0);
                            date = date.AddSeconds(parts.Count > 5 ? parts[5] : 0);
                            date = date.AddMilliseconds(parts.Count > 6 ? parts[6] : 0);

                            context.Stack.Push(date);
                            break;
                        }
                    case "days":
                    case "day":
                        {
                            var x = Convert.ToInt32(context.Stack.Pop());
                            var ts = new TimeSpan(x, 0, 0, 0);
                            context.Stack.Push(ts);
                            break;
                        }
                    case "hours":
                    case "hour":
                        {
                            var x = Convert.ToInt32(context.Stack.Pop());
                            var ts = new TimeSpan(0, x, 0, 0);
                            context.Stack.Push(ts);
                            break;
                        }
                    case "minutes":
                    case "minute":
                        {
                            var x = Convert.ToInt32(context.Stack.Pop());
                            var ts = new TimeSpan(0, 0, x, 0);
                            context.Stack.Push(ts);
                            break;
                        }
                    case "seconds":
                    case "second":
                        {
                            var x = Convert.ToInt32(context.Stack.Pop());
                            var ts = new TimeSpan(0, 0, 0, x);
                            context.Stack.Push(ts);
                            break;
                        }
                    case "month":
                    case "months":
                        {
                            var x = Convert.ToInt32(context.Stack.Pop());
                            var ts = new ExtendedTimeSpan(0, x);
                            context.Stack.Push(ts);
                            break;
                        }
                    case "year":
                    case "years":
                        {
                            var x = Convert.ToInt32(context.Stack.Pop());
                            var ts = new ExtendedTimeSpan(x, 0);
                            context.Stack.Push(ts);
                            break;
                        }
                    case "ts":
                        {
                            var s = Convert.ToInt32(context.Stack.Pop());
                            var m = Convert.ToInt32(context.Stack.Pop());
                            var h = Convert.ToInt32(context.Stack.Pop());
                            var d = Convert.ToInt32(context.Stack.Pop());
                            var ts = new TimeSpan(d, h, m, s);
                            context.Stack.Push(ts);
                            break;
                        }
                    case "+":
                        {
                            if (context.Stack.First() is TimeSpan)
                            {
                                return SumTimeSpan(context);
                            }

                            if (context.Stack.First() is ExtendedTimeSpan)
                            {
                                return SumExtendedTimeSpan(context);
                            }
                            return false;
                        }
                    case "-":
                        {
                            if (context.Stack.First() is TimeSpan)
                            {
                                return SubtractTimeSpan(context);
                            }

                            if (context.Stack.First() is ExtendedTimeSpan)
                            {
                                context.Stack.First().Invert();
                                return SumExtendedTimeSpan(context);
                            }
                            return false;
                        }
                }
                return true;
            }
            return false;
        }
        private static bool SumTimeSpan(RPNContext context)
        {
            var ts = (TimeSpan)context.Stack.Pop();
            var dt = (DateTime)context.Stack.Pop();

            dt = dt.Add(ts);
            context.Stack.Push(dt);
            return true;
        }
        private static bool SubtractTimeSpan(RPNContext context)
        {
            var ts = (TimeSpan)context.Stack.Pop();
            var dt = (DateTime)context.Stack.Pop();

            dt = dt.Subtract(ts);
            context.Stack.Push(dt);
            return true;
        }
        private static bool SumExtendedTimeSpan(RPNContext context)
        {
            var ts = (ExtendedTimeSpan)context.Stack.Pop();
            var dt = (DateTime)context.Stack.Pop();

            dt = dt.AddYears(ts.Years);
            dt = dt.AddMonths(ts.Months);
            context.Stack.Push(dt);
            return true;
        }
    }
}