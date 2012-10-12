﻿/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Reflection;

namespace Lingua
{
    /// <summary>
    /// Defines a <see cref="LanguageElementType" /> that represents a type of a terminal or token identified by an <see cref="IGrammar" />.
    /// </summary>
    /// <remarks>
    /// A <see cref="TerminalType"/> is a factory object that can construct the associated <see cref="Terminal"/> object using
    /// <see cref="CreateTerminal"/>.
    /// </remarks>
    public class TerminalType : LanguageElementType
    {
        delegate Terminal Constructor();

        private readonly string _fullName;
        private readonly string _name;
        private readonly Constructor _constructor;
        private readonly string _pattern;
        private readonly bool _isStop;
        private readonly bool _ignore;


        /// <summary>
        /// Initializes a new instance of the <see cref="TerminalType"/> class.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the <see cref="Terminal"/> described by this <see cref="TerminalType"/>.</param>
        public TerminalType(Type type)
            : base(LanguageElementTypes.Terminal)
        {
            if (!type.IsSubclassOf(typeof(Terminal)))
            {
                throw new ArgumentException("Type must be a subclass of Terminal.", "type");
            }

            _fullName = type.AssemblyQualifiedName;
            _name = GetName(type);
            _constructor = delegate() { return (Terminal)Activator.CreateInstance(type); };

            var attributes = type.GetCustomAttributes(typeof(TerminalAttribute), false);
            foreach (var attribute in attributes)
            {
                var terminalAttribute = (TerminalAttribute)attribute;
                if (terminalAttribute.IsStop)
                {
                    _isStop = true;
                }
                if (terminalAttribute.Ignore)
                {
                    _ignore = true;
                }
                _pattern = terminalAttribute.Pattern;
            }
        }

        /// <summary>
        /// Gets the full name of the <see cref="TerminalType"/>.  This includes the name of the <see cref="Assembly"/> containing
        /// the <see cref="Type"/> used to construct the <see cref="TerminalType"/> and will always uniquely identify the <see cref="TerminalType"/>.
        /// </summary>
        public override string FullName
        {
            get { return _fullName; }
        }

        /// <summary>
        /// Gets the abbreviated name of the <see cref="TerminalType"/>.  This excludes the name of the <see cref="Assembly"/> containing the
        /// <see cref="Type"/> used to construct the <see cref="TerminalType"/> and may not uniquely identify the <see cref="TerminalType"/>.
        /// </summary>
        public override string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets or sets the regular expression pattern of this <see cref="TerminalType"/>.
        /// </summary>
        public string Pattern
        {
            get { return _pattern; }
        }

        /// <summary>
        /// Gets or sets a value that indicates if this <see cref="TerminalType"/> is the stopping terminal
        /// of the <see cref="IGrammar"/>.
        /// </summary>
        public bool IsStop
        {
            get { return _isStop; }
        }

        /// <summary>
        /// Gets or sets a value that indicates if <see cref="Terminal"/> objects associated with this <see cref="TerminalType"/> should be ignored by any
        /// <see cref="IParser"/> that encounters them.
        /// </summary>
        public bool Ignore
        {
            get { return _ignore; }
        }


        public override string ToString()
        {
            return string.Format("[T {0}]", Name);
        }

        /// <summary>
        /// Constructs an instance of a <see cref="Terminal"/> described by this <see cref="TerminalType"/>.
        /// </summary>
        /// <returns>A new <see cref="Terminal"/>.</returns>
        public Terminal CreateTerminal()
        {
            return _constructor();
        }
    }
}
