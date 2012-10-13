/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Lingua;

namespace LinguaDemo.Calculator
{
    [Terminal(@"\s+", Ignore=true)]
    public class Whitespace : CalculatorTerminal
    {
    }
}
