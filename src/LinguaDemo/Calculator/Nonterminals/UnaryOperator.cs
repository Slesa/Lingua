/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace LinguaDemo.Calculator
{
    public class UnaryOperator : CalculatorNonterminal
    {
        public static void Rule(UnaryOperator result, OperatorSubtraction op)
        {
            result.Function = (value) => -value;
        }

        public Func<int, int> Function { get; private set; }
    }
}
