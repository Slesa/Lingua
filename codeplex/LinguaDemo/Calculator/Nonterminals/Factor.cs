using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinguaDemo.Calculator
{
    public class Factor : CalculatorNonterminal
    {
        #region Fields

        private int m_value;

        #endregion

        #region Rules

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
