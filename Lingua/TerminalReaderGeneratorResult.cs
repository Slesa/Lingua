﻿/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Lingua
{
    /// <summary>
    /// Defines the output generated by an <see cref="ITerminalReaderGenerator" />.
    /// </summary>
    public class TerminalReaderGeneratorResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TerminalReaderGeneratorResult"/> class.
        /// </summary>
        /// <param name="terminalReader">The <see cref="ITerminalReader"/> created by the <see cref="ITerminalReaderGenerator"/>.</param>
        public TerminalReaderGeneratorResult(ITerminalReader terminalReader)
        {
            TerminalReader = terminalReader;
        }

        /// <summary>
        /// Gets the <see cref="ITerminalReader"/> created by the <see cref="ITerminalReaderGenerator"/>.
        /// </summary>
        public ITerminalReader TerminalReader { get; private set; }
    }
}