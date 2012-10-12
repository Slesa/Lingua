/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Reflection;
using System.Text;

namespace Lingua
{
    /// <summary>
    /// Represents a rule or production identified by an <see cref="IGrammar" />.
    /// </summary>
    public class RuleType : IComparable<RuleType>
    {
        delegate void Invoker(LanguageElement[] parameters);

        private readonly Invoker _delegate;

        private readonly NonterminalType _lhs;

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleType"/> class.
        /// </summary>
        /// <param name="methodInfo">The <see cref="MethodInfo"/> associated with this <see cref="RuleType"/>.</param>
        /// <param name="priority">The priority of this rule used to resolve conflicts with other rules during parser generation.</param>
        /// <param name="lhs">The <see cref="NonterminalType"/> representing the LHS of this rule.</param>
        /// <param name="rhs">The collection of <see cref="LanguageElementType"/> representing the RHS of this rule.</param>
        public RuleType(MethodInfo methodInfo, int priority, NonterminalType lhs, LanguageElementType[] rhs)
        {
            var sb = new StringBuilder();
            sb.Append(methodInfo.Name);
            sb.Append("[");
            string prefix = null;
            foreach (var parameter in methodInfo.GetParameters())
            {
                sb.Append(prefix); prefix = ",";
                sb.Append(parameter.ParameterType.Name);
            }
            sb.Append("]");
            var name = sb.ToString();

            FullName = string.Format("{0}::{1}", methodInfo.DeclaringType.FullName, name);
            Name = name;
            _delegate = delegate(LanguageElement[] parameters) { methodInfo.Invoke(null, parameters); };

            Priority = priority;
            _lhs = lhs;
            Rhs = rhs;
        }

        /// <summary>
        /// Gets the full name of the <see cref="RuleType"/>.  This includes the name of the <see cref="Assembly"/> containing
        /// the <see cref="MethodInfo"/> used to construct the <see cref="RuleType"/> and will always uniquely identify the <see cref="RuleType"/>.
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// Gets the abbreviated name of the <see cref="RuleType"/>.  This excludes the name of the <see cref="Assembly"/> containing the
        /// <see cref="MethodInfo"/> used to construct the <see cref="RuleType"/> and may not uniquely identify the <see cref="RuleType"/>.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the priority of this rule used to resolve conflicts with other rules during parser generation.
        /// </summary>
        public int Priority { get; private set; }

        /// <summary>
        /// Gets the <see cref="NonterminalType"/> representing the LHS of this rule.
        /// </summary>
        public NonterminalType Lhs
        {
            get { return _lhs; }
        }

        /// <summary>
        /// Gets the collection of <see cref="LanguageElementType"/> representing the RHS of this rule.
        /// </summary>
        public LanguageElementType[] Rhs { get; private set; }

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="RuleType"/>. 
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="RuleType"/>.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(Lhs.Name);

            sb.Append(" ::=");

            foreach (LanguageElementType item in Rhs)
            {
                sb.Append(" ");
                sb.Append(item.Name);
            }

            sb.Append(";");

            return sb.ToString();
        }

        /// <summary>
        /// Calls the method associated with this <see cref="RuleType"/>.
        /// </summary>
        /// <param name="parameters">The <see cref="LanguageElement"/> objects to be passed to the method.  The first object
        /// represents the LHS of the rule.  The remaining objects represent the RHS of the rule.</param>
        public void Invoke(LanguageElement[] parameters)
        {
            _delegate.DynamicInvoke(new object[] { parameters });
        }

        /// <summary>
        /// Compares this instance to a specified <see cref="RuleType"/> object.
        /// </summary>
        /// <param name="other">A <see cref="RuleType"/> object</param>
        /// <returns>A signed number indicating the relative values of this instance and <paramref name="other"/>.</returns>
        /// <remarks>
        /// <see cref="RuleType"/> objects are compared based on <see cref="RuleType.Priority"/>.
        /// </remarks>
        public int CompareTo(RuleType other)
        {
            return this.Priority.CompareTo(other.Priority);
        }
    }
}
