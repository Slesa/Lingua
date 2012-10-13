/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Lingua;

namespace LinguaDemo.Calculator
{
    [Terminal(@"\d+")]
    public class Number : CalculatorTerminal
    {
        public int Value
        {
            get { return int.Parse(Text); }
        }
    }
}
