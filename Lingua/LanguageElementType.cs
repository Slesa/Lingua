﻿/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Text;
using System.Reflection;

namespace Lingua
{
    /// <summary>
    /// Defines a language element type identified by an <see cref="IGrammar" />.
    /// </summary>
    /// <remarks>
    /// There are two subclasses of <c>LanguageElementType</c>: <see cref="TerminalType" /> and <see cref="NonterminalType" />.  
    /// <see cref="ElementType" /> identifies the specific subclass of a <c>LanguageElementType</c>.
    /// </remarks>
    public abstract class LanguageElementType
    {
        readonly LanguageElementTypes _elementType;
        readonly FirstSet _first = new FirstSet();

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageElementType"/> class.
        /// </summary>
        /// <param name="elementType">A <see cref="LanguageElementTypes"/> value that identifies the subclass of an instance of <see cref="LanguageElementType"/>.</param>
        protected LanguageElementType(LanguageElementTypes elementType)
        {
            _elementType = elementType;
        }

        /// <summary>
        /// Gets a <see cref="LanguageElementTypes"/> value that identifies the subclass of an instance of <see cref="LanguageElementType"/>.
        /// </summary>
        public LanguageElementTypes ElementType
        {
            get { return _elementType; }
        }

        /// <summary>
        /// Gets the full name of the <see cref="LanguageElementType"/>.  This includes the name of the <see cref="Assembly"/> containing
        /// the <see cref="Type"/> used to construct the <see cref="LanguageElementType"/> and will always uniquely identify the <see cref="LanguageElementType"/>.
        /// </summary>
        public abstract string FullName { get; }

        /// <summary>
        /// Gets the abbreviated name of the <see cref="LanguageElementType"/>.  This excludes the name of the <see cref="Assembly"/> containing the
        /// <see cref="Type"/> used to construct the <see cref="LanguageElementType"/> and may not uniquely identify the <see cref="LanguageElementType"/>.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the set of <see cref="TerminalType"/> objects that appear in the the FIRST set of the <see cref="LanguageElementType"/>.
        /// </summary>
        public FirstSet First
        {
            get { return _first; }
        }

        /// <summary>
        /// Gets the name of the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">A <see cref="Type"/> object.</param>
        /// <returns>The name of the specified <see cref="Type"/>.</returns>
        protected string GetName(Type type)
        {
            var sb = new StringBuilder();

            var genericArguments = type.GetGenericArguments();
            if (genericArguments.Length > 0)
            {
                // Remove suffix from generic name.
                //
                var name = type.Name;
                var idxDelimiter = name.IndexOf('`'); // ` = U+0060
                if (idxDelimiter >= 0)
                {
                    name = name.Substring(0, idxDelimiter);
                }
                sb.Append(name);

                sb.Append("[");

                string prefix = null;
                foreach (var genericArgument in genericArguments)
                {
                    sb.Append(prefix); prefix = ",";
                    sb.Append(genericArgument.Name);
                }

                sb.Append("]");
            }
            else
            {
                sb.Append(type.Name);
            }

            return sb.ToString();
        }
    }
}
