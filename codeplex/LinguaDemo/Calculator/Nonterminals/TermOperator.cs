/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

using Lingua;

namespace LinguaDemo.Calculator
{
    public class TermOperator : CalculatorNonterminal
    {
        #region Fields

        private Func<int, int, int> m_function;

        #endregion

        #region Rules

        public static void Rule(TermOperator result, OperatorMultiplication op)
        {
            result.Function = (lhs, rhs) => lhs * rhs;
        }

        public static void Rule(TermOperator result, OperatorDivision op)
        {
            result.Function = (lhs, rhs) => lhs / rhs;
        }

        #endregion

        #region Public Properties

        public Func<int, int, int> Function
        {
            get { return m_function; }
            private set { m_function = value; }
        }

        #endregion
    }
}
