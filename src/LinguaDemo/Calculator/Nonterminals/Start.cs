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
        public static void Rule(Start result, Expression expression)
        {
            result.Value = expression.Value;
        }

        public int Value { get; protected set; }
    }
}
