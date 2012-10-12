/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Lingua
{
    /// <summary>
    /// Defines a <see cref="LanguageElement" /> that represents a nonterminal: the LHS of one or more rules defined by an <see cref="IGrammar"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="Nonterminal"/> objects are created during the parsing process when a <see cref="ParserActionReduce"/> action occurs.
    /// The <see cref="NonterminalType"/> associated with the <see cref="RuleType.Lhs"/> of the <see cref="ParserActionReduce.Rule"/> is used to construct a <see cref="Nonterminal"/> 
    /// representing the LHS of the rule being reduced.  After executing the rule using <see cref="RuleType.Invoke"/>, a new
    /// <see cref="ParserStackItem"/> is created using the <see cref="Nonterminal"/> and is pushed into the <see cref="ParserStack"/>.
    /// </remarks>
    public class Nonterminal : LanguageElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Nonterminal"/> class.
        /// </summary>
        public Nonterminal()
        { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the <see cref="NonterminalType"/> that identifies the type of this <see cref="Nonterminal"/>.  
        /// </summary>
        /// <remarks>
        /// This property is set automatically set by <see cref="NonterminalType.CreateNonterminal"/>.
        /// </remarks>
        public new NonterminalType ElementType
        {
            get { return (NonterminalType)base.ElementType; }
            set { base.ElementType = value; }
        }

        #endregion
    }
}
