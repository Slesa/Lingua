/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Lingua
{
    /// <summary>
    /// Defines an instance of a language element processed by a <see cref="IParser" />.
    /// </summary>
    /// <remarks>
    /// There are two subclasses of <c>LanguageElement</c>: <see cref="Terminal" /> and <see cref="Nonterminal" />.  
    /// <see cref="ElementType" /> identifies the specific subclass of a <c>LanguageElement</c>.
    /// </remarks>
    public abstract class LanguageElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageElement"/> class.
        /// </summary>
        public LanguageElement()
        { }

        /// <summary>
        /// Gets or sets the <see cref="LanguageElementType"/> that identifies the type of this <see cref="LanguageElement"/>.  
        /// </summary>
        /// <remarks>
        /// This property is set automatically set by <see cref="NonterminalType.CreateNonterminal"/> and <see cref="TerminalType.CreateTerminal"/>.
        /// </remarks>
        public LanguageElementType ElementType { get; set; }
    }
}
