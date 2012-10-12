/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

using Lingua;

namespace LinguaDemo.Calculator
{
    [Nonterminal(IsStart = true)]
    public class Start : CalculatorNonterminal
    {
        #region Fields

        private int m_value;

        #endregion

        #region Rules

        public static void Rule(Start result, Expression expression)
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
