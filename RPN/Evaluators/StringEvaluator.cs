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
        private static string[] OPERATORS = new string[] { "ucase", "lcase", "strfmt", "datefmt", "stringify", "parse", "str" };

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
                    case "datefmt":
                        {
                            var temp = context.Stack.Pop();
                            dynamic value;
                            string format = null;

                            if (temp is DateTime)
                            {
                                value = temp;
                            }
                            else
                            {
                                format = temp;
                                value = context.Stack.Pop();
                            }

                            DateTime date = value is DateTime ? value : Convert.ToDateTime(value.ToString());
                            context.Stack.Push(ConvertDateTimeToString(date, format));
                        }
                        break;
                    case "str":
                        {
                            var value = context.Stack.Pop();
                            if (value is DateTime)
                            {
                                context.Stack.Push(ConvertDateTimeToString(value));
                            }
                            else
                            {
                                context.Stack.Push(value.ToString());
                            }
                        }
                        break;
                }
                return true;
            }
            return false;
        }
        private static string ConvertDateTimeToString(DateTime dateTime, string format = null)
        {
            var newFormat = format == "default" ? "yyyy-MM-dd HH:mm:ss" : format;
            return dateTime.ToString(newFormat);
        }
    }
}