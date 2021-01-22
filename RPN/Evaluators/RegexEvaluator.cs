using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using RPN.Helpers;

namespace RPN.Evaluators
{
    internal class RegexEvaluator
    {
        private static string[] OPERATORS = new string[] { "match" };

        internal static bool Evaluate(RPNContext context)
        {
            if (OPERATORS.Contains(context.Current))
            {
                switch (context.Current)
                {
                    case "match":
                        {
                            string rx = context.Stack.Pop().ToString();
                            string input = context.Stack.Pop().ToString();
                            var regex = new Regex(rx);
                            var matches = regex.Matches(input);
                            context.Data.Add(matches);
                            break;
                        }
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