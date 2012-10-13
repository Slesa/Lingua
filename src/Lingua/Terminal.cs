/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Lingua
{
    /// <summary>
    /// Defines a <see cref="LanguageElement" /> that represents a terminal: a token recognized by an <see cref="ITerminalReader"/>
    /// which appears in the RHS of one more rules defined by an <see cref="IGrammar"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="Terminal"/> objects are created by an <see cref="ITerminalReader"/> and are consumed by an <see cref="IParser"/>.
    /// They are placed on the <see cref="ParserStack"/> when a <see cref="ParserActionShift"/> action occurs, and are subsequently
    /// removed from the <see cref="ParserStack"/> when a <see cref="ParserActionReduce"/> action occurs.
    /// </remarks>
    public class Terminal : LanguageElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Terminal"/> class.
        /// </summary>
        public Terminal()
        { }

        /// <summary>
        /// Gets or sets the input text associated with this <see cref="Terminal"/>.
        /// </summary>
        /// <remarks>
        /// This property should be set by an <see cref="ITerminalReader"/> before returning the <see cref="Terminal"/> to the caller.
        /// </remarks>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="TerminalType"/> that identifies the type of this <see cref="Terminal"/>.  
        /// </summary>
        /// <remarks>
        /// This property is set automatically set by <see cref="TerminalType.CreateTerminal"/>.
        /// </remarks>
        public new TerminalType ElementType
        {
            get { return (TerminalType)base.ElementType; }
            set { base.ElementType = value; }
        }

        public override string ToString()
        {
            return string.Format(@"[{0} ""{1}""]", ElementType, Text);
        }
    }
}
