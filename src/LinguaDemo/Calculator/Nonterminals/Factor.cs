namespace LinguaDemo.Calculator
{
    public class Factor : CalculatorNonterminal
    {
        //public static void Rule(Factor result, UnaryOperator op, Expression expression)
        //{
        //    result.Value = op.Function(expression.Value);
        //}

        public static void Rule(Factor result, Number number)
        {
            result.Value = number.Value;
        }

        public static void Rule(Factor result, ParenthesisOpen parenthesisOpen, Expression expression, ParenthesisClose parenthesisClose)
        {
            result.Value = expression.Value;
        }

        public int Value { get; protected set; }
    }
}
