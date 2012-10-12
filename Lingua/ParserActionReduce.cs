/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Lingua
{
    /// <summary>
    /// Defines a <see cref="ParserAction" /> that causes a <see cref="Parser" /> to update a <see cref="ParserStack"/> by
    /// <list type="bullet">
    /// <item>popping the <see cref="ParserStackItem"/> objects that correspond to the RHS of a rule and</item>
    /// <item>pushing a new <c>ParserStackItem</c> object that corresponds with the LHS of the rule.</item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// A reduction occurs occurs on terminal a when the <see cref="ParserState"/> associated with the topmost <c>ParserStackItem</c> represents an LR(0) item of
    /// the form <value>X→Y₁…Yₓ○</value> and <value>a</value> is in FOLLOW(<value>X</value>) and the parser determines that <value>X</value> potentially recognizes the input.
    /// </remarks>
    public class ParserActionReduce : ParserAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParserActionReduce"/> class.
        /// </summary>
        /// <param name="rule">The <see cref="RuleType"/> associated with this action.</param>
        /// <remarks>
        /// When this action is taken, elements will 
        /// be removed from the <see cref="ParserStack"/> based on <see cref="RuleType.Rhs"/> and a new 
        /// <see cref="ParserStackItem"/> containing a <see cref="Nonterminal"/> constructed by <see cref="RuleType.Lhs"/> will be added.
        /// </remarks>
        public ParserActionReduce(RuleType rule)
            : base(ParserActionTypes.Reduce)
        {
            Rule = rule;
        }

        /// <summary>
        /// Gets the <see cref="RuleType"/> associated with this action.
        /// </summary>
        /// <remarks>
        /// When this action is taken, elements will 
        /// be removed from the <see cref="ParserStack"/> based on <see cref="RuleType.Rhs"/> and a new 
        /// <see cref="ParserStackItem"/> containing a <see cref="Nonterminal"/> constructed by <see cref="RuleType.Lhs"/> will be added.
        /// </remarks>
        public RuleType Rule { get; private set; }

        public override string ToString()
        {
            return string.Format("[Reduce {0}]", Rule);
        }
    }
}
