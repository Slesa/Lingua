/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Lingua
{
    /// <summary>
    /// Identifies a parsing rule conflict identified when generating an <see cref="IParser" />
    /// </summary>
    public class ParserGeneratorParserConflict
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParserGeneratorParserConflict"/> class.
        /// </summary>
        /// <param name="rule">The rule selected by the <see cref="IParserGenerator"/>.</param>
        /// <param name="conflictingRule">The rule conflicting with the rule selected by the <see cref="IParserGenerator"/>.</param>
        public ParserGeneratorParserConflict(string rule, string conflictingRule)
        {
            Rule = rule;
            ConflictingRule = conflictingRule;
        }

        /// <summary>
        /// Gets the rule selected by the <see cref="IParserGenerator"/>.
        /// </summary>
        public string Rule { get; private set; }

        /// <summary>
        /// Gets the rule conflicting with the rule selected by the <see cref="IParserGenerator"/>.
        /// </summary>
        public string ConflictingRule { get; private set; }

        public override string ToString()
        {
            return string.Format("[Conflict {0}, {1}]", Rule, ConflictingRule);
        }
    }
}
