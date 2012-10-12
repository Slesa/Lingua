/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Text.RegularExpressions;

namespace Lingua
{
    /// <summary>
    /// Specifies the attribute of a <see cref="Terminal" />-derived class.
    /// </summary>
    public class TerminalAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TerminalAttribute"/> class.
        /// </summary>
        public TerminalAttribute()
        {
            Pattern = null;
            IsStop = false;
            Ignore = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TerminalAttribute"/> class.
        /// </summary>
        /// <param name="pattern">A <see cref="Regex"/> pattern that defines the terminal.</param>
        public TerminalAttribute(string pattern)
        {
            Pattern = pattern;
            IsStop = false;
            Ignore = false;
        }

        /// <summary>
        /// Gets or sets the regular expression pattern of the <see cref="Terminal"/> adorned by this attribute.
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates if the <see cref="Terminal"/> adorned by this attribute is the stopping terminal
        /// of the <see cref="IGrammar"/>.
        /// </summary>
        public bool IsStop { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates if the <see cref="Terminal"/> adorned by this attribute should be ignored by any
        /// <see cref="IParser"/> that encounters it.
        /// </summary>
        public bool Ignore { get; set; }
    }
}
