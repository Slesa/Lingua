/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace Lingua
{
    /// <summary>
    /// Specifies the grammar attribute of a <see cref="Terminal" /> or <see cref="Nonterminal" />-derived class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class GrammarAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrammarAttribute"/> class.
        /// </summary>
        public GrammarAttribute()
        { }

        /// <summary>
        /// Gets or sets a value that specifies the name of the Grammar the <see cref="Terminal" /> or <see cref="Nonterminal" /> belongs to.
        /// </summary>
        public string Name { get; set; }
    }
}
