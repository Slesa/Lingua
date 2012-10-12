﻿/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;

namespace Lingua
{
    /// <summary>
    /// Defines the output generated by an <see cref="IParserGenerator" />.
    /// </summary>
    public class ParserGeneratorResult
    {
        readonly List<ParserGeneratorParserConflict> _conflicts = new List<ParserGeneratorParserConflict>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ParserGeneratorResult"/> class.
        /// </summary>
        /// <param name="parser">The <see cref="IParser"/> created by the <see cref="IParserGenerator"/>.</param>
        /// <param name="conflicts">The collection of <see cref="ParserGeneratorParserConflict"/> objects desribing the rule conflicts identified by the <see cref="IParserGenerator"/>.</param>
        public ParserGeneratorResult(IParser parser, IEnumerable<ParserGeneratorParserConflict> conflicts)
        {
            Parser = parser;
            _conflicts.AddRange(conflicts);
        }

        /// <summary>
        /// Gets the <see cref="IParser"/> created by the <see cref="IParserGenerator"/>.
        /// </summary>
        public IParser Parser { get; private set; }

        /// <summary>
        /// Gets the collection of <see cref="ParserGeneratorParserConflict"/> objects desribing the rule conflicts identified by the <see cref="IParserGenerator"/>.
        /// </summary>
        public List<ParserGeneratorParserConflict> Conflicts
        {
            get { return _conflicts; }
        }
    }
}
