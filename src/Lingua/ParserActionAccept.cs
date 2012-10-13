/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Lingua
{
    /// <summary>
    /// Defines a <see cref="ParserAction" /> that causes an <see cref="IParser" /> to accept the text being parsed.
    /// </summary>
    public class ParserActionAccept : ParserAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParserActionAccept"/> class.
        /// </summary>
        /// <param name="rule">The <see cref="RuleType"/> associated with this action.  The <see cref="NonterminalType.IsStart"/> property of <see cref="RuleType.Lhs"/> should 
        /// be <value>true</value>.</param>
        public ParserActionAccept(RuleType rule)
            : base(ParserActionTypes.Accept)
        {
            Rule = rule;
        }

        /// <summary>
        /// Gets the <see cref="RuleType"/> associated with this action.  The <see cref="NonterminalType.IsStart"/> property of <see cref="RuleType.Lhs"/> should 
        /// be <value>true</value>.
        /// </summary>
        public RuleType Rule { get; private set; }

        public override string ToString()
        {
            return string.Format("[Accept {0}]", Rule);
        }
    }
}
