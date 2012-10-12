﻿/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Lingua
{
    /// <summary>
    /// Defines a <see cref="GeneratorState" /> created by a <see cref="ParserGenerator" />.  Represents all the LR(0) 
    /// </summary>
    public class GeneratorState
    {
        private readonly HashSet<GeneratorStateItem> _items = new HashSet<GeneratorStateItem>();
        private readonly Dictionary<LanguageElementType, GeneratorState> _transitions = new Dictionary<LanguageElementType, GeneratorState>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratorState"/> class.
        /// </summary>
        /// <param name="id">A value that uniquely identifies this <see cref="GeneratorState"/> instance.</param>
        /// <param name="items">The <see cref="GeneratorStateItem"/> objects that define this <see cref="GeneratorState"/>.</param>
        public GeneratorState(int id, IEnumerable<GeneratorStateItem> items)
        {
            Id = id;
            _items.UnionWith(items);
        }

        /// <summary>
        /// Gets a value that uniquely identifies this <see cref="GeneratorState"/> instance.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the <see cref="GeneratorStateItem"/> objects that define this <see cref="GeneratorState"/>.
        /// </summary>
        public HashSet<GeneratorStateItem> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Gets the relationships between this <see cref="GeneratorState"/> and other <see cref="GeneratorState"/> objects.
        /// </summary>
        /// <remarks>
        /// If this <see cref="GeneratorState"/> contains a <see cref="GeneratorRuleItem"/> representing an LR(0) item of the
        /// form <value>X→Y₁…○Z…Yₓ</value>, <see cref="Transitions"/> will contain an item that associates the <see cref="GeneratorState"/>
        /// containing <value>X→Y₁…Z○…Yₓ</value> with the <see cref="LanguageElementType"/> representing <value>Z</value>.
        /// </remarks>
        public Dictionary<LanguageElementType, GeneratorState> Transitions
        {
            get { return _transitions; }
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="GeneratorState"/>. 
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="GeneratorState"/>.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(Id.ToString(CultureInfo.InvariantCulture));
            foreach (var element in Items)
            {
                sb.AppendLine();
                sb.AppendFormat("  {0}", element);
            }
            foreach (var transition in Transitions)
            {
                sb.AppendLine();
                sb.AppendFormat("  {0}: {1}", transition.Value.Id, transition.Key.Name);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Computes a new set of <see cref="GeneratorStateItem"/> objects based on items in the current <see cref="GeneratorState"/>
        /// identified by the specified <see cref="LanguageElementType"/>
        /// </summary>
        /// <param name="languageElementType">A <see cref="LanguageElementType"/> object.</param>
        /// <returns>A <see cref="HashSet{GeneratorStateItem}"/> containing items identified by <paramref name="languageElementType"/>.</returns>
        /// <remarks>
        /// If this <see cref="GeneratorState"/> contains a <see cref="GeneratorRuleItem"/> representing an LR(0) item of the
        /// form <value>X→Y₁…○Z…Yₓ</value> and <see cref="LanguageElementType"/> is <value>Z</value>, the 
        /// result will contain a <see cref="GeneratorRuleItem"/> representing an LR(0) item of the
        /// form <value>X→Y₁…Z○…Yₓ</value>.
        /// </remarks>
        public HashSet<GeneratorStateItem> Apply(LanguageElementType languageElementType)
        {
            HashSet<GeneratorStateItem> result = null;

            foreach (GeneratorStateItem item in Items)
            {
                if (item.RuleItem.DotElement == languageElementType)
                {
                    if (result == null)
                    {
                        result = new HashSet<GeneratorStateItem>();
                    }
                    result.Add(new GeneratorStateItem(new GeneratorRuleItem(item.RuleItem.Rule, item.RuleItem.Dot + 1)));
                }
            }

            return result;
        }
    }
}
