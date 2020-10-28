using System;
using System.Collections.Generic;
using System.Globalization;
using RPN.Helpers;

namespace RPN.Evaluators
{
    internal class DefaultEvaluator
    {
        internal static bool Evaluate(RPNContext context)
        {
            return EvaluateNumber(context)
                || EvaluateBoolean(context)
                || EvaluateString(context)
                || EvaluateFunction(context)
                || EvaluateObject(context);
        }

        static bool EvaluateNumber(RPNContext context)
        {
            if (double.TryParse(context.Current, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
            {
                context.Stack.Push(number);
                return true;
            }
            else if (context.Current.EndsWith("%"))
            {
                var current = context.Current.TrimEnd('%');
                if (double.TryParse(current, NumberStyles.Any, CultureInfo.InvariantCulture, out number))
                {
                    var x = Convert.ToDouble(context.Stack.Pop());
                    var y = x * (number / 100);
                    context.Stack.Push(x);
                    context.Stack.Push(y);
                    return true;
                }
            }
            return false;
        }
        static bool EvaluateBoolean(RPNContext context)
        {
            if (context.Current == "true" || context.Current == "false")
            {
                context.Stack.Push(Convert.ToBoolean(context.Current));
                return true;
            }
            return false;
        }
        static bool EvaluateString(RPNContext context)
        {
            if (context.Current.StartsWith("`"))
            {
                var values = new List<string>();
                var current = context.Current.TrimStart('`');

                while (context.CurrentIndex < context.Values.Count)
                {
                    if (current.EndsWith("`"))
                    {
                        current = current.TrimEnd('`');
                        values.Add(current);
                        var join = string.Join(" ", values);
                        context.Stack.Push(join);
                        break;
                    }
                    values.Add(current);
                    context.MoveNext();
                    current = context.Current;
                }
                return true;
            }
            return false;
        }
        static bool EvaluateFunction(RPNContext context)
        {
            if (context.Current.StartsWith("@@"))
            {
                var blockExpression = context.Blocks[context.Current];

                var value = RPN.Evaluate(blockExpression);
                context.Stack.Push(value);
                return true;
            }
            else if (context.Current.StartsWith("@"))
            {
                var label = context.Current;
                var list = new List<string>();

                context.MoveNext();
                while (context.Current != label)
                {
                    list.Add(context.Current);
                    context.MoveNext();
                }
                context.Blocks.Add("@" + label, new RPNExpression(String.Join(" ", list), context.Data.ToArray()));
                return true;
            }
            return false;
        }
        static bool EvaluateObject(RPNContext context)
        {
            if (context.Current.StartsWith("$"))
            {
                var objSplit = context.Current.Replace("[", ".").Replace("]", "").Trim('.').Split(separator: new char[] { '.' }, count: 2);
                var objIdx = Convert.ToInt32(objSplit[0].Trim('$'));
                var obj = context.Data[objIdx];

                if (objSplit.Length > 1)
                {
                    var propname = objSplit[1];
                    var value = ObjectParseHelper.GetPropertyValue(propname, obj);
                    context.Stack.Push(value);
                }
                else
                {
                    context.Stack.Push(obj);
                }
                return true;
            }
            else if (context.Current == "...")
            {
                var property = context.Stack.Pop().ToString();
                var collection = context.Stack.Pop();
                var obj = ObjectParseHelper.GetCollection(collection);
                for (int k = 0; k < obj.Length; k++)
                {
                    var value = ObjectParseHelper.GetPropertyValue(property, obj[k]);
                    context.Stack.Push(value);
                }
                return true;
            }
            return false;
        }
    }
}