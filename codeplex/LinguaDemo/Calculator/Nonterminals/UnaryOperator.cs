/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

using Lingua;

namespace LinguaDemo.Calculator
{
    public class UnaryOperator : CalculatorNonterminal
    {
        #region Fields

        private Func<int, int> m_function;

        #endregion

        #region Rules

        public static void Rule(UnaryOperator result, OperatorSubtraction op)
        {
            result.Function = (value) => -value;
        }

        #endregion

        #region Public Properties

        public Func<int, int> Function
        {
            get { return m_function; }
            private set { m_function = value; }
        }

        #endregion
    }
}
