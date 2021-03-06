using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using RPN.Exceptions;
using RPN.Helpers;

namespace RPN
{
    internal class RPNContext
    {
        internal RPNExpression Expression { get; set; }
        internal List<dynamic> Data { get; set; } = new List<dynamic>();
        internal List<string> Values { get; set; } = new List<string>();
        internal Stack<dynamic> Stack { get; set; } = new Stack<dynamic>();
        internal Dictionary<string, RPNExpression> Blocks { get; set; } = new Dictionary<string, RPNExpression>();
        internal int CurrentIndex { get; set; } = -1;
        internal string Current
        {
            get
            {
                return this.Values[this.CurrentIndex];
            }
        }
        internal bool CanMove
        {
            get
            {
                return this.CurrentIndex < this.Values.Count - 1;
            }
        }

        internal RPNContext(RPNExpression expression)
        {
            this.Expression = expression;
            this.Load();

        }
        internal void Load()
        {
            this.Validate();

            if (this.Expression.Parameters != null)
            {
                this.Expression.Parameters.ToList().ForEach(parameter =>
                {
                    this.Data.Add(ParseParameter(parameter));
                });
            }

            this.Values.AddRange(this.Expression.Expression.Split(' '));
        }
        internal dynamic ParseParameter(object parameter)
        {
            if(parameter is string && parameter.ToString().StartsWith("{"))
            {
                try
                {
                    return JsonHelper.Parse(parameter.ToString());
                }
                catch
                {
                    return parameter.ToString();
                }
            }
            return parameter;
        }
        private void Validate()
        {
            if (this.Expression == null)
            {
                throw new NullExpressionException();
            }

            if (string.IsNullOrWhiteSpace(this.Expression.Expression))
            {
                throw new EmptyExpressionException();
            }
        }
        internal void MoveNext()
        {
            if (this.CanMove)
            {
                this.CurrentIndex++;
            }
        }
        internal T GetResult<T>()
        {
            return (T)this.Stack.Pop();
        }
        internal void SetResult<T>(T value)
        {
            this.Stack.Clear();
            this.Stack.Push(value);
            this.CurrentIndex = this.Values.Count;
        }
    }
}