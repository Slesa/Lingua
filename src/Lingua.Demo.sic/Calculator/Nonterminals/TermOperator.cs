/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace LinguaDemo.Calculator
{
    public class TermOperator : CalculatorNonterminal
    {
        public static void Rule(TermOperator result, OperatorMultiplication op)
        {
            result.Function = (lhs, rhs) => lhs * rhs;
        }

        public static void Rule(TermOperator result, OperatorDivision op)
        {
            result.Function = (lhs, rhs) => lhs / rhs;
        }

        public Func<int, int, int> Function { get; private set; }
    }
}
