/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Lingua
{
    /// <summary>
    /// Defines a <see cref="ParserAction" /> that causes a <see cref="Parser" /> to push a new <see cref="ParserStackItem"/> onto a <see cref="ParserStack"/>.
    /// </summary>
    /// <remarks>
    /// A shift occurs on terminal a when the <see cref="ParserState"/> associated with the topmost <c>ParserStackItem</c> represents an LR(0) item of
    /// the form X→Y₁…○a…Yₓ and the parser determines that Y₁…a○…Yₓ potentially recognizes the input.
    /// </remarks>
    public class ParserActionShift : ParserAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParserActionShift"/> class.
        /// </summary>
        /// <param name="state">The <see cref="ParserState"/> associated with this action.</param>
        /// <remarks>
        /// When this action is taken, a <see cref="ParserStackItem"/> containing this state
        /// will be added to the <see cref="ParserStack"/>.
        /// </remarks>
        public ParserActionShift(ParserState state)
            : base(ParserActionTypes.Shift)
        {
            State = state;
        }

        /// <summary>
        /// Gets the <see cref="ParserState"/> associated with this action.
        /// </summary>
        /// <remarks>
        /// When this action is taken, a <see cref="ParserStackItem"/> containing this state
        /// will be added to the <see cref="ParserStack"/>.
        /// </remarks>
        public ParserState State { get; private set; }

        public override string ToString()
        {
            return string.Format("[Shift {0}]", State);
        }
    }
}
