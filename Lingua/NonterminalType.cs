/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lingua
{
    /// <summary>
    /// Defines a <see cref="LanguageElementType" /> that represents a type of a nonterminal identified by an <see cref="IGrammar" />.
    /// </summary>
    /// <remarks>
    /// A <see cref="NonterminalType"/> is a factory object that can construct the associated <see cref="Nonterminal"/> object using
    /// <see cref="CreateNonterminal"/>.
    /// </remarks>
    public class NonterminalType : LanguageElementType
    {
        delegate Nonterminal Constructor();

        readonly string _fullName;
        readonly string _name;
        readonly Constructor _constructor;
        readonly bool _isStart;

        readonly List<RuleType> _rules = new List<RuleType>();
        readonly FollowSet _follow = new FollowSet();

        /// <summary>
        /// Initializes a new instance of the <see cref="NonterminalType"/> class.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the <see cref="Nonterminal"/> described by this <see cref="NonterminalType"/>.</param>
        public NonterminalType(Type type)
            : base(LanguageElementTypes.Nonterminal)
        {
            if (!type.IsSubclassOf(typeof(Nonterminal)))
            {
                throw new ArgumentException("Type must be a subclass of Nonterminal.", "type");
            }

            _fullName = type.AssemblyQualifiedName;
            _name = GetName(type);
            _constructor = () => (Nonterminal) Activator.CreateInstance(type);

            var attributes = type.GetCustomAttributes(typeof(NonterminalAttribute), false);
            foreach (var nonterminalAttribute in attributes.Cast<NonterminalAttribute>().Where(nonterminalAttribute => nonterminalAttribute.IsStart))
            {
                _isStart = true;
            }
        }

        /// <summary>
        /// Gets the full name of the <see cref="NonterminalType"/>.  This includes the name of the <see cref="Assembly"/> containing
        /// the <see cref="Type"/> used to construct the <see cref="NonterminalType"/> and will always uniquely identify the <see cref="NonterminalType"/>.
        /// </summary>
        public override string FullName
        {
            get { return _fullName; }
        }

        /// <summary>
        /// Gets the abbreviated name of the <see cref="NonterminalType"/>.  This excludes the name of the <see cref="Assembly"/> containing the
        /// <see cref="Type"/> used to construct the <see cref="NonterminalType"/> and may not uniquely identify the <see cref="NonterminalType"/>.
        /// </summary>
        public override string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets a value that indicates if this <see cref="Nonterminal"/> is the starting nonterminal of the <see cref="IGrammar"/>.
        /// </summary>
        public bool IsStart
        {
            get { return _isStart; }
        }

        /// <summary>
        /// Gets a collection of <see cref="RuleType"/> objects describing the rules for which this <see cref="NonterminalType"/> is 
        /// <see cref="RuleType.Lhs"/>.
        /// </summary>
        public List<RuleType> Rules
        {
            get { return _rules; }
        }

        /// <summary>
        /// Gets the set of <see cref="TerminalType"/> objects that appear in the the FOLLOW set of the <see cref="NonterminalType"/>.
        /// </summary>
        public FollowSet Follow
        {
            get { return _follow; }
        }

        /// <summary>
        /// Constructs an instance of a <see cref="Nonterminal"/> described by this <see cref="NonterminalType"/>.
        /// </summary>
        /// <returns>A new <see cref="Nonterminal"/>.</returns>
        public Nonterminal CreateNonterminal()
        {
            var result = _constructor();
            result.ElementType = this;
            return result;
        }
    }
}
