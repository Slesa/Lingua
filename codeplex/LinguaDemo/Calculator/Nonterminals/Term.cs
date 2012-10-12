using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinguaDemo.Calculator
{
    public class Term : CalculatorNonterminal
    {
        #region Fields

        private int m_value;

        #endregion

        #region Rules

        public static void Rule(Term result, Term term, TermOperator op, Factor factor)
        {
            result.Value = op.Function(term.Value, factor.Value);
        }

        public static void Rule(Term result, Factor factor)
        {
            result.Value = factor.Value;
        }

        #endregion

        #region Public Properties

        public int Value
        {
            get { return m_value; }
            protected set { m_value = value; }
        }

        #endregion
    }
}
