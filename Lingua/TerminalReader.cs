/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Lingua
{
    /// <summary>
    /// A concrete implementation of <see cref="ITerminalReader" /> created by <see cref="TerminalReaderGenerator" />.
    /// </summary>
    public class TerminalReader : ITerminalReader
    {
        private readonly List<TerminalType> _terminalTypes = new List<TerminalType>();
        private readonly TerminalType _stopTerminal;
        private readonly Regex _regex;

        private string _text;
        private Match _match;
        private Terminal _queuedTerminal;
        private bool _stopTerminalRead;

        /// <summary>
        /// Initializes a new instance of the <see cref="TerminalReader"/> class.
        /// </summary>
        /// <param name="terminalTypes">A collection of <see cref="TerminalType"/> objects recognized by the <see cref="TerminalReader"/></param>
        /// <param name="stopTerminal">The <see cref="TerminalType"/> of the stopping terminal.</param>
        public TerminalReader(IEnumerable<TerminalType> terminalTypes, TerminalType stopTerminal)
        {
            _terminalTypes.AddRange(terminalTypes);
            _stopTerminal = stopTerminal;
            _regex = new Regex(CreatePattern());
        }

        /// <summary>
        /// Prepares an <see cref="TerminalReader"/> to read from the specified <see cref="String"/>.
        /// </summary>
        /// <param name="text">A <see cref="String"/> containing the input text to be processed by the <see cref="TerminalReader"/>.</param>
        public void Open(string text)
        {
            _text = text;
            _match = null;
            _queuedTerminal = null;
            _stopTerminalRead = false;
        }

        /// <summary>
        /// Reads the next <see cref="Terminal"/>.
        /// </summary>
        /// <returns>The next <see cref="Terminal"/> in the input text.  If no <see cref="TerminalType"/> recognizes the input text, <value>null</value> is returned.</returns>
        public Terminal ReadTerminal()
        {
            Terminal result;

            if (_queuedTerminal != null)
            {
                result = _queuedTerminal;
                _queuedTerminal = null;
            }
            else
            {
                result = GetNextTerminal();
            }

            LinguaTrace.TraceEvent(TraceEventType.Information, LinguaTraceId.ID_PARSE_READTOKEN, "{0}", result);

            return result;
        }

        /// <summary>
        /// Returns the next <see cref="Terminal"/> in the input text without advancing the current position of the <see cref="TerminalReader"/>.
        /// </summary>
        /// <returns>The next <see cref="Terminal"/> in the input text.  If no <see cref="TerminalType"/> recognizes the input text, <value>null</value> is returned.</returns>
        public Terminal PeekTerminal()
        {
            if (_queuedTerminal == null)
            {
                _queuedTerminal = GetNextTerminal();
            }

            return _queuedTerminal;
        }

        /// <summary>
        /// Returns the specified <see cref="Terminal"/> to the input stream.
        /// </summary>
        /// <param name="terminal">A <see cref="Terminal"/> to be returned to the input stream.</param>
        public void PushTerminal(Terminal terminal)
        {
            if (_queuedTerminal != null)
            {
                throw new InvalidOperationException("Queued terminal already exists.");
            }

            _queuedTerminal = terminal;
        }

        private string CreatePattern()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(@"\G(");

            string prefix = null;
            for (int idx = 0; idx < _terminalTypes.Count; ++idx)
            {
                TerminalType terminal = _terminalTypes[idx];
                if (!string.IsNullOrEmpty(terminal.Pattern))
                {
                    sb.Append(prefix); prefix = "|";

                    sb.AppendFormat(
                        @"(?<t{0}>{1})",
                        idx,
                        terminal.Pattern);
                }
            }

            sb.Append(")");

            return sb.ToString();
        }

        private Terminal GetNextTerminal()
        {
            if (_match == null)
            {
                _match = _regex.Match(_text);
            }
            else
            {
                _match = _match.NextMatch();
            }

            if (_match.Success)
            {
                for (int idx = 0; idx < _terminalTypes.Count; ++idx)
                {
                    Group group = _match.Groups[string.Format("t{0}", idx)];
                    if (group.Success)
                    {
                        Terminal terminal = _terminalTypes[idx].CreateTerminal();
                        terminal.ElementType = _terminalTypes[idx];
                        terminal.Text = group.Value;
                        return terminal;
                    }
                }
            }

            if (!_stopTerminalRead)
            {
                _stopTerminalRead = true;

                Terminal terminal = _stopTerminal.CreateTerminal();
                terminal.ElementType = _stopTerminal;
                return terminal;
            }

            return null;
        }
    }
}
