/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace Lingua
{
    /// <summary>
    /// Specifies the attribute of a <see cref="Nonterminal" />-derived class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class NonterminalAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NonterminalAttribute"/> class.
        /// </summary>
        public NonterminalAttribute()
        { }

        /// <summary>
        /// Gets or sets a value that indicates if the <see cref="Nonterminal"/> adorned by this attribute is the starting nonterminal
        /// of the <see cref="IGrammar"/>.
        /// </summary>
        public bool IsStart { get; set; }
    }
}
