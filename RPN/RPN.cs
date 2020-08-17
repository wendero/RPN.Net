using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text.Json;
using RPN.Exceptions;
using System.Dynamic;

namespace RPN
{
    public class RPN
    {
        private static string[] OPERATORS = new string[] {
            "+", "-", "*", "/", ">", "<", "=", "==", "!=", "<>", ">=", "<=", "++", "--", "+=", "-=",
            "pop", "popx", "min", "max", "clr", "not", "!", "ret", "retif", "&", "&&", "and", "|", "||", "or", "if", "ife",
            "case", "end", "pi", "%","^","pow","log","round","exp","logb","log10","abs", "ucase",
            "lcase", "strfmt", "sum", "sumk", "todate", "fromindex", "date", "stringify", "parse", "data", "perc", "sumx",
            "10x", "E", "+-", "-+", "dpush", "dpop", "dclr", "stack", "data", "swap", "rot", "dup", "rnd", "random", "btw" };
        private static string[] MATH = new string[] {
            "acos", "asin", "atan", "atan2", "ceiling", "cos", "cosh", "floor", "sin", "tan",
            "sinh", "tanh", "truncate", "trunc", "ceil", "sqrt", };
        public static dynamic Eval(string rpn, params object[] objects)
        {
            return RPN.Eval<dynamic>(rpn, objects);
        }
        public static T Eval<T>(string rpn, params object[] objects)
        {
            try
            {
                var data = objects != null ? new List<dynamic>(objects) : new List<dynamic>();
                var split = rpn.Split(' ');
                var stack = new Stack<dynamic>();
                var blocks = new Dictionary<string, string>();

                for (int i = 0; i < split.Length; i++)
                {
                    var current = split[i];
                    double number;
                    if (double.TryParse(current, NumberStyles.Any, CultureInfo.InvariantCulture, out number))
                    {
                        stack.Push(number);
                        continue;
                    }
                    else if (MATH.Contains(current))
                    {
                        if (current == "ceil") current = "ceiling";
                        else if (current == "trunc") current = "truncate";

                        var mathMethods = typeof(Math).GetMethods();
                        MethodInfo method = mathMethods.FirstOrDefault(f => f.Name.ToLower() == current);
                        var ceilingMethod = mathMethods.Where(w => w.Name.ToLower() == "ceiling");

                        if (new string[] { "ceiling", "floor", "truncate" }.Contains(current))
                            method = mathMethods.First(f => f.Name.ToLower() == current && f.ToString().ToLower().Contains("double"));

                        if (method != null)
                        {
                            var methodParameters = new List<double>();
                            for (int j = 0; j < method.GetParameters().Count(); j++)
                                methodParameters.Add(Convert.ToDouble(stack.Pop()));
                            methodParameters.Reverse();
                            
                            var parametersArray = method.Invoke(null, methodParameters.Cast<object>().ToArray());
                            stack.Push(parametersArray);
                        }
                        break;
                    }
                    else if (OPERATORS.Contains(current))
                    {
                        switch (current)
                        {
                            case "+":
                                {
                                    var x = Convert.ToDouble(stack.Pop());
                                    var y = Convert.ToDouble(stack.Pop());
                                    stack.Push(y + x);
                                    break;
                                }
                            case "-":
                                {
                                    var x = Convert.ToDouble(stack.Pop());
                                    var y = Convert.ToDouble(stack.Pop());
                                    stack.Push(y - x);
                                    break;
                                }
                            case "*":
                                {
                                    var x = Convert.ToDouble(stack.Pop());
                                    var y = Convert.ToDouble(stack.Pop());
                                    stack.Push(y * x);
                                    break;
                                }
                            case "/":
                                {
                                    var x = Convert.ToDouble(stack.Pop());
                                    var y = Convert.ToDouble(stack.Pop());
                                    stack.Push(y / x);
                                    break;
                                }
                            case "=":
                            case "==":
                                {
                                    var x = stack.Pop().ToString();
                                    var y = stack.Pop().ToString();
                                    stack.Push(y == x);
                                    break;
                                }
                            case "!=":
                            case "<>":
                                {
                                    var x = stack.Pop().ToString();
                                    var y = stack.Pop().ToString();
                                    stack.Push(y != x);
                                    break;
                                }
                            case "!":
                            case "not":
                                {
                                    var x = Convert.ToBoolean(stack.Pop());
                                    stack.Push(!x);
                                    break;
                                }
                            case "&":
                            case "&&":
                            case "and":
                                {
                                    var x = Convert.ToBoolean(stack.Pop());
                                    var y = Convert.ToBoolean(stack.Pop());
                                    stack.Push(y && x);
                                    break;
                                }
                            case "|":
                            case "||":
                            case "or":
                                {
                                    var x = Convert.ToBoolean(stack.Pop());
                                    var y = Convert.ToBoolean(stack.Pop());
                                    stack.Push(y || x);
                                    break;
                                }
                            case ">":
                                {
                                    var x = Convert.ToDouble(stack.Pop());
                                    var y = Convert.ToDouble(stack.Pop());
                                    stack.Push(y > x);
                                    break;
                                }
                            case "<":
                                {
                                    var x = Convert.ToDouble(stack.Pop());
                                    var y = Convert.ToDouble(stack.Pop());
                                    stack.Push(y < x);
                                    break;
                                }
                            case ">=":
                                {
                                    var x = Convert.ToDouble(stack.Pop());
                                    var y = Convert.ToDouble(stack.Pop());
                                    stack.Push(y >= x);
                                    break;
                                }
                            case "<=":
                                {
                                    var x = Convert.ToDouble(stack.Pop());
                                    var y = Convert.ToDouble(stack.Pop());
                                    stack.Push(y <= x);
                                    break;
                                }
                            case "+=":
                                {
                                    var x = Convert.ToDouble(stack.Pop());
                                    var y = Convert.ToDouble(stack.Pop());
                                    stack.Push(y += x);
                                    break;
                                }
                            case "-=":
                                {
                                    var x = Convert.ToDouble(stack.Pop());
                                    var y = Convert.ToDouble(stack.Pop());
                                    stack.Push(y -= x);
                                    break;
                                }
                            case "++":
                                {
                                    var x = Convert.ToDouble(stack.Pop());
                                    stack.Push(x + 1);
                                    break;
                                }
                            case "--":
                                {
                                    var x = Convert.ToDouble(stack.Pop());
                                    stack.Push(x - 1);
                                    break;
                                }
                            case "-+":
                            case "+-":
                                {
                                    stack.Push(stack.Pop() * (-1));
                                    break;
                                }
                            case "pop":
                                {
                                    stack.Pop();
                                    break;
                                }
                            case "popx":
                                {
                                    Stack<dynamic> tempStack = new Stack<dynamic>();
                                    var x = stack.Pop();
                                    for (int ix = 0; ix < x; ix++)
                                    {
                                        tempStack.Push(stack.Pop());
                                    }

                                    stack.Pop();

                                    while (tempStack.Count > 0)
                                    {
                                        stack.Push(tempStack.Pop());
                                    }
                                    break;
                                }
                            case "clr":
                                {
                                    stack.Clear();
                                    break;
                                }
                            case "min":
                                {
                                    List<double> numbers = new List<double>();
                                    while (stack.Count > 0)
                                    {
                                        numbers.Add(Convert.ToDouble(stack.Pop()));
                                    }
                                    stack.Push(numbers.Min());
                                    break;
                                }
                            case "max":
                                {
                                    List<double> numbers = new List<double>();
                                    while (stack.Count > 0)
                                    {
                                        numbers.Add(Convert.ToDouble(stack.Pop()));
                                    }
                                    stack.Push(numbers.Max());
                                    break;
                                }
                            case "random":
                            case "rnd":
                                {
                                    stack.Push(new Random().NextDouble());
                                    break;
                                }
                            case "btw":
                                {
                                    var x = Convert.ToInt32(stack.Pop());
                                    var y = Convert.ToInt32(stack.Pop());
                                    var randomNumber = new Random().Next(y, x + 1);
                                    stack.Push(randomNumber);
                                    break;
                                }
                            case "ret":
                                {
                                    return (T)stack.Pop();
                                }
                            case "retif":
                                {
                                    var condition = Convert.ToBoolean(stack.Pop());
                                    if (condition)
                                        return (T)stack.Pop();
                                    break;
                                }
                            case "go":
                                {
                                    return (T)stack.Pop();
                                }
                            case "if":
                                {
                                    var value = stack.Pop();
                                    var condition = Convert.ToBoolean(stack.Pop());
                                    if (condition)
                                        stack.Push(value);
                                    break;
                                }
                            case "ife":
                                {
                                    var falseValue = stack.Pop();
                                    var trueValue = stack.Pop();
                                    var condition = Convert.ToBoolean(stack.Pop());
                                    if (condition)
                                        stack.Push(trueValue);
                                    else
                                        stack.Push(falseValue);
                                    break;
                                }
                            case "case":
                                {
                                    var caseTrue = stack.Pop();
                                    var caseValue = stack.Pop();
                                    var switchValue = stack.Peek();
                                    if (caseValue == switchValue)
                                    {
                                        stack.Pop();
                                        stack.Push(caseTrue);
                                        while (split[++i] != "end") ;
                                    }
                                    break;
                                }
                            case "end":
                                break;
                            case "pi":
                                stack.Push(Math.PI);
                                break;
                            case "%":
                                {
                                    var x = Convert.ToInt32(stack.Pop());
                                    var y = Convert.ToInt32(stack.Pop());
                                    stack.Push(y % x);
                                }
                                break;
                            case "^":
                            case "pow":
                                {
                                    var x = stack.Pop();
                                    var y = stack.Pop();
                                    stack.Push(Math.Pow(y, x));
                                }
                                break;
                            case "log":
                                stack.Push(Math.Log(stack.Pop()));
                                break;
                            case "round":
                                {
                                    var x = stack.Pop();
                                    var y = stack.Pop();
                                    stack.Push(Math.Round(y, Convert.ToInt32(x)));
                                }
                                break;
                            case "exp":
                                stack.Push(Math.Exp(stack.Pop()));
                                break;
                            case "10x":
                            case "E":
                                stack.Push(Math.Pow(10, stack.Pop()) * stack.Pop());
                                break;
                            case "logb":
                                {
                                    var x = stack.Pop();
                                    var y = stack.Pop();
                                    stack.Push(Math.Log(y, x));
                                }
                                break;
                            case "log10":
                                {
                                    stack.Push(Math.Log10(stack.Pop()));
                                }
                                break;
                            case "abs":
                                stack.Push(Math.Abs(stack.Pop()));
                                break;
                            case "ucase":
                                stack.Push(stack.Pop().ToString().ToUpper());
                                break;
                            case "lcase":
                                stack.Push(stack.Pop().ToString().ToLower());
                                break;
                            case "stringify":
                                stack.Push(JsonSerializer.Serialize(stack.Pop()));
                                break;
                            case "dpush":
                                data.Add(stack.Pop());
                                break;
                            case "dpop":
                                var last = data.Last();
                                stack.Push(last);
                                data.Remove(last);
                                break;
                            case "dclr":
                                data.Clear();
                                break;
                            case "data":
                                stack.Push(JsonSerializer.Serialize(data));
                                break;
                            case "swap":
                                {
                                    var x = stack.Pop();
                                    var y = stack.Pop();
                                    stack.Push(x);
                                    stack.Push(y);
                                    break;
                                }
                            case "rot":
                                {
                                    var x = stack.Pop();
                                    var y = stack.Pop();
                                    var z = stack.Pop();
                                    stack.Push(x);
                                    stack.Push(z);
                                    stack.Push(y);
                                    break;
                                }
                            case "dup":
                                {
                                    var x = stack.Pop();
                                    stack.Push(x);
                                    stack.Push(x);
                                    break;
                                }
                            case "stack":
                                stack.Push(JsonSerializer.Serialize(stack.Reverse()));
                                break;
                            case "parse":
                                data.Add(JsonHelper.Parse(stack.Pop()));
                                break;
                            case "strfmt":
                                {
                                    var value = stack.Pop().ToString();
                                    var regex = new Regex(@"{(\d*)}");
                                    MatchCollection matches = regex.Matches(value);
                                    var indexes = new List<int>();
                                    foreach (Match m in matches)
                                    {
                                        indexes.Add(Convert.ToInt32(m.Groups[1].Value));
                                    }

                                    var max = indexes.Max();
                                    for (var _is = max; _is >= 0; _is--)
                                    {
                                        var newValue = stack.Pop().ToString();
                                        value = value.Replace($"{{{_is}}}", newValue);
                                    }
                                    stack.Push(value);
                                }
                                break;
                            case "todate":
                                {
                                    var format = stack.Pop().ToString();
                                    var value = stack.Pop().ToString();

                                    DateTime date = Convert.ToDateTime(value);
                                    var newFormat = format == "default" ? "yyyy-MM-dd HH:mm:ss" : format;

                                    stack.Push(date.ToString(newFormat));
                                }
                                break;
                            case "date":
                                {
                                    var len = stack.Pop();
                                    var parts = new List<int>();
                                    for (var ix = 0; ix < len; ix++)
                                        parts.Add(Convert.ToInt32(stack.Pop()));
                                    parts.Reverse();

                                    DateTime date = new DateTime();
                                    date = date.AddYears(parts.Count > 0 ? parts[0] - 1 : 0);
                                    date = date.AddMonths(parts.Count > 1 ? parts[1] - 1 : 0);
                                    date = date.AddDays(parts.Count > 2 ? parts[2] - 1 : 0);
                                    date = date.AddHours(parts.Count > 3 ? parts[3] : 0);
                                    date = date.AddMinutes(parts.Count > 4 ? parts[4] : 0);
                                    date = date.AddSeconds(parts.Count > 5 ? parts[5] : 0);
                                    date = date.AddMilliseconds(parts.Count > 6 ? parts[6] : 0);

                                    stack.Push(date.ToString());
                                    break;
                                }
                            case "perc":
                                {
                                    var x = stack.Pop();
                                    var y = stack.Pop();
                                    stack.Push(Math.Round(Convert.ToDouble(100 * x / y), 0));
                                }
                                break;
                            case "fromindex":
                                {
                                    var index = Convert.ToInt32(stack.Pop());
                                    var len = Convert.ToInt32(stack.Pop());
                                    var array = new ArrayList();
                                    for (var ix = 1; ix <= len; ix++)
                                        array.Add(stack.Pop());

                                    stack.Push(array[index]);
                                }
                                break;
                            case "sum":
                                {
                                    double sum = 0;
                                    while (stack.Count > 0)
                                    {
                                        var value = stack.Pop();
                                        sum += value;
                                    }
                                    stack.Push(sum);
                                }
                                break;
                            case "sumx":
                            case "sumk":
                                {
                                    double sum = 0;
                                    int x = (int)stack.Pop();
                                    for (int ik = 0; ik < x; ik++)
                                    {
                                        var value = stack.Pop();
                                        sum += value;
                                    }
                                    stack.Push(sum);
                                }
                                break;
                        }
                        continue;
                    }
                    else if (current.StartsWith("`"))
                    {
                        var value = new List<string>();
                        current = current.TrimStart('`');

                        while (i < split.Length)
                        {
                            if (current.EndsWith("`"))
                            {
                                current = current.TrimEnd('`');
                                value.Add(current);
                                var join = string.Join(" ", value);
                                stack.Push(join);
                                break;
                            }
                            value.Add(current);
                            current = split[++i];
                        }
                    }
                    else if (current.EndsWith("%"))
                    {
                        current = current.TrimEnd('%');
                        if (double.TryParse(current, NumberStyles.Any, CultureInfo.InvariantCulture, out number))
                        {
                            var x = Convert.ToDouble(stack.Pop());
                            var y = x * (number / 100);
                            stack.Push(x);
                            stack.Push(y);
                            continue;
                        }
                    }
                    else if (current.StartsWith("$"))
                    {
                        var objSplit = current.Replace("[", ".").Replace("]", "").Trim('.').Split(separator: new char[] { '.' }, count: 2);
                        var objIdx = Convert.ToInt32(objSplit[0].Trim('$'));
                        var obj = data[objIdx];

                        if (objSplit.Length > 1)
                        {
                            var propname = objSplit[1];
                            var value = GetPropertyValue(propname, obj);
                            stack.Push(value);
                        }
                        else
                        {
                            stack.Push(obj);
                        }
                    }
                    else if (current.StartsWith("@@"))
                    {
                        var value = RPN.Eval(blocks[current], data);
                        stack.Push(value);
                    }
                    else if (current.StartsWith("@"))
                    {
                        var label = current;
                        var list = new List<string>();

                        current = split[++i];
                        while (current != label)
                        {
                            list.Add(current);
                            current = split[++i];
                        }
                        blocks.Add("@" + label, String.Join(" ", list));
                    }
                    else if (current == "true" || current == "false")
                    {
                        stack.Push(Convert.ToBoolean(current));
                    }
                    else if (current == "...")
                    {
                        var property = stack.Pop().ToString();
                        var collection = stack.Pop();
                        var obj = (collection is IDictionary ? (IEnumerable)((IDictionary)collection).Values : (IEnumerable)collection).Cast<object>().ToArray();
                        for (int k = 0; k < obj.Length; k++)
                        {
                            var value = GetPropertyValue(property, obj[k]);
                            stack.Push(value);
                        }
                    }
                    else
                    {
                        stack.Push(current);
                    }
                }
                return (T)stack.Pop();
            }
            catch
            {
                throw new ParsingException(rpn);
            }
        }

        private static object GetPropertyValue(string propertyName, object inputObject)
        {
            if (inputObject == null)
                return null;

            dynamic objectClone = inputObject;
            var propertySplit = propertyName.Replace("[", ".").Replace("]", "").Trim('.').Split('.', count: 2);
            var specificName = propertySplit[0];
            var isRecursive = propertySplit.Length > 1;

            if (objectClone is ExpandoObject)
            {
                return ((IDictionary<string, object>)objectClone)[specificName];
            }
            else if (objectClone is IEnumerable)
            {
                if (objectClone is IDictionary)
                {
                    var dictionary = (IDictionary)objectClone;
                    var property = dictionary.GetType().GetProperty(specificName);

                    if (property == null)
                    {
                        var entryValue = dictionary[specificName];

                        return isRecursive ? GetPropertyValue(propertySplit[1], entryValue) : entryValue;
                    }
                    return property.GetValue(dictionary, null);
                }
                else //is array
                {
                    var array = ((IEnumerable)objectClone).Cast<object>().ToArray();

                    var property = array.GetType().GetProperty(specificName); //check if the field is a property or an item

                    if (property == null)
                    {
                        var idx = int.Parse(specificName);
                        var arrayItem = array[idx];

                        return isRecursive ? GetPropertyValue(propertySplit[1], arrayItem) : arrayItem;
                    }
                    return property.GetValue(array, null);
                }
            }
            else
            {
                var property = objectClone.GetType().GetProperty(specificName);
                var propertyValue = property.GetValue(objectClone);

                return isRecursive ? GetPropertyValue(propertySplit[1], propertyValue) : propertyValue;
            }
        }
    }
}