namespace LinguaDemo.Calculator
{
    public class Term : CalculatorNonterminal
    {
        public static void Rule(Term result, Term term, TermOperator op, Factor factor)
        {
            result.Value = op.Function(term.Value, factor.Value);
        }

        public static void Rule(Term result, Factor factor)
        {
            result.Value = factor.Value;
        }

        public int Value { get; protected set; }
    }
}
