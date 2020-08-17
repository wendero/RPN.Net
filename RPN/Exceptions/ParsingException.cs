namespace RPN.Exceptions
{
    public class ParsingException : RPNException
    {
        public ParsingException(string expression) : base(string.Format("This expression can not be parsed: {0}", expression))
        { }
    }
}