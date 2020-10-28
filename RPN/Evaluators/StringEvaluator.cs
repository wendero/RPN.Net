using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using RPN.Helpers;

namespace RPN.Evaluators
{
    internal class StringEvaluator
    {
        private static string[] OPERATORS = new string[] { "ucase", "lcase", "strfmt", "todate", "date", "stringify", "parse" };

        internal static bool Evaluate(RPNContext context)
        {
            if (OPERATORS.Contains(context.Current))
            {
                switch (context.Current)
                {
                    case "ucase":
                        context.Stack.Push(context.Stack.Pop().ToString().ToUpper());
                        break;
                    case "lcase":
                        context.Stack.Push(context.Stack.Pop().ToString().ToLower());
                        break;
                    case "stringify":
                        context.Stack.Push(JsonSerializer.Serialize(context.Stack.Pop()));
                        break;
                    case "parse":
                        context.Data.Add(JsonHelper.Parse(context.Stack.Pop()));
                        break;
                    case "strfmt":
                        {
                            var value = context.Stack.Pop().ToString();
                            var regex = new Regex(@"{(\d*)}");
                            var matches = regex.Matches(value);
                            var indexes = new List<int>();
                            foreach (var match in matches)
                            {
                                indexes.Add(Convert.ToInt32(match.Groups[1].Value));
                            }

                            var max = indexes.Max();
                            for (var i = max; i >= 0; i--)
                            {
                                var newValue = context.Stack.Pop().ToString();
                                value = value.Replace($"{{{i}}}", newValue);
                            }
                            context.Stack.Push(value);
                        }
                        break;
                    case "todate":
                        {
                            var format = context.Stack.Pop().ToString();
                            var value = context.Stack.Pop().ToString();

                            DateTime date = Convert.ToDateTime(value);
                            var newFormat = format == "default" ? "yyyy-MM-dd HH:mm:ss" : format;

                            context.Stack.Push(date.ToString(newFormat));
                        }
                        break;
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

                            context.Stack.Push(date.ToString());
                            break;
                        }
                }
                return true;
            }
            return false;
        }
    }
}