namespace RPN
{
    public class RPNExpression
    {
        public string Expression { get; set; }
        public object[] Parameters { get; set; }

        public RPNExpression(string expression, params object[] parameters)
        {
            this.Expression = expression;
            this.Parameters = parameters;
        }
        public RPNExpression(string expression) : this(expression, null) { }
    }
}