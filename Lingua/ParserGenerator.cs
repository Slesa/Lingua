/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Lingua
{
    /// <summary>
    /// A concrete implementation of <see cref="IParserGenerator" />.
    /// </summary>
    /// <remarks>
    /// Generates an SLR parser.  See <seealso href="http://en.wikipedia.org/wiki/Simple_LR_parser">Simple LR Parser</seealso> for more information.
    /// </remarks>
    public class ParserGenerator : IParserGenerator
    {
        /// <summary>
        /// Constructs a new <see cref="Parser"/> which can recoganize the specified <see cref="IGrammar"/>.
        /// </summary>
        /// <param name="grammar">The <see cref="IGrammar"/> to be recognized by the <see cref="Parser"/>.</param>
        /// <returns>A <see cref="ParserGeneratorResult"/> containing <see cref="Parser"/> and information pertaining to the
        /// success or failure of the generation process.
        /// </returns>
        public ParserGeneratorResult GenerateParser(IGrammar grammar)
        {
            var conflicts = new List<ParserGeneratorParserConflict>();
            var states = CreateStates(grammar);

            // Create a parser state for each generator state.
            //
            var parserStates = states.ToDictionary(state => state, state => new ParserState(state.Id));

            foreach (var state in states)
            {
                LinguaTrace.TraceEvent(TraceEventType.Verbose, LinguaTraceId.ID_GENERATE_PROCESS_STATE, "{0}", state);

                var items = new List<GeneratorStateItem>(state.Items);
                items.Sort();

                // Construct the list of actions associated with the parser state.
                //
                var actions = new Dictionary<TerminalType, ParserAction>();
                var actionRules = new Dictionary<ParserAction, GeneratorRuleItem>();
                foreach (var item in items)
                {
                    LinguaTrace.TraceEvent(TraceEventType.Verbose, LinguaTraceId.ID_GENERATE_PROCESS_ITEM, "{0}", item);

                    if (item.RuleItem.DotElement == null)
                    {
                        foreach (var terminal in item.RuleItem.Rule.Lhs.Follow)
                        {
                            LinguaTrace.TraceEvent(TraceEventType.Verbose, LinguaTraceId.ID_GENERATE_PROCESS_TERMINAL, "{0}", terminal);

                            if (actions.ContainsKey(terminal))
                            {
                                var conflict = new ParserGeneratorParserConflict(
                                    actionRules[actions[terminal]].ToString(),
                                    item.RuleItem.ToString());

                                LinguaTrace.TraceEvent(TraceEventType.Information, LinguaTraceId.ID_GENERATE_PROCESS_CONFLICT, "{0}", conflict);

                                conflicts.Add(conflict);
                            }
                            else if (item.RuleItem.Rule.Lhs.IsStart && terminal.IsStop)
                            {
                                ParserAction action = new ParserActionAccept(item.RuleItem.Rule);

                                LinguaTrace.TraceEvent(TraceEventType.Information, LinguaTraceId.ID_GENERATE_PROCESS_ACTION, "{0}", action);

                                actions.Add(terminal, action);
                                actionRules.Add(action, item.RuleItem);
                            }
                            else
                            {
                                ParserAction action = new ParserActionReduce(item.RuleItem.Rule);

                                LinguaTrace.TraceEvent(TraceEventType.Information, LinguaTraceId.ID_GENERATE_PROCESS_ACTION, "{0}", action);

                                actions.Add(terminal, action);
                                actionRules.Add(action, item.RuleItem);
                            }
                        }
                    }
                    else if (item.RuleItem.DotElement.ElementType == LanguageElementTypes.Terminal)
                    {
                        var terminal = (TerminalType)item.RuleItem.DotElement;

                        if (actions.ContainsKey(terminal))
                        {
                            var conflict = new ParserGeneratorParserConflict(
                                actionRules[actions[terminal]].ToString(),
                                item.RuleItem.ToString());

                            LinguaTrace.TraceEvent(TraceEventType.Information, LinguaTraceId.ID_GENERATE_PROCESS_CONFLICT, "{0}", conflict);

                            conflicts.Add(conflict);
                        }
                        else
                        {
                            ParserAction action = new ParserActionShift(parserStates[state.Transitions[terminal]]);

                            LinguaTrace.TraceEvent(TraceEventType.Information, LinguaTraceId.ID_GENERATE_PROCESS_ACTION, "{0}", action);

                            actions.Add(terminal, action);
                            actionRules.Add(action, item.RuleItem);
                        }
                    }
                }

                // Construct the GOTO table 
                //
                var gotos = new Dictionary<NonterminalType, ParserState>();
                foreach (var transition in state.Transitions)
                {
                    if (transition.Key.ElementType != LanguageElementTypes.Nonterminal) continue;

                    var nonterminal = (NonterminalType)transition.Key;
                    gotos.Add(nonterminal, parserStates[transition.Value]);
                }

                // Update the parser state.
                //
                var parserState = parserStates[state];
                foreach (var action in actions)
                {
                    parserState.Actions.Add(action.Key, action.Value);
                }

                foreach (var gotoItem in gotos)
                {
                    parserState.Gotos.Add(gotoItem.Key, gotoItem.Value);
                }
            }

            var parser = new Parser(parserStates[states[0]]);

            var result = new ParserGeneratorResult(parser, conflicts);
            return result;
        }

        List<GeneratorState> CreateStates(IGrammar grammar)
        {
            var states = new List<GeneratorState>();
            var unevaluatedStates = new List<GeneratorState>();
            var stateId = 0;

            // Compute start state.
            //
            {
                var items = new HashSet<GeneratorStateItem>();
                foreach (var rule in grammar.StartNonterminal.Rules)
                {
                    items.Add(new GeneratorStateItem(new GeneratorRuleItem(rule, 0)));
                }
                ComputeClosure(grammar, items);

                var startState = new GeneratorState(stateId++, items);
                states.Add(startState);
                unevaluatedStates.Add(startState);
            }

            var languageElements = new List<LanguageElementType>();
            languageElements.AddRange(grammar.GetTerminals());
            languageElements.AddRange(grammar.GetNonterminals());

            while (unevaluatedStates.Count > 0)
            {
                // Remove one of the evaluated states and process it.
                //
                var state = unevaluatedStates[0];
                unevaluatedStates.RemoveAt(0);

                foreach (var languageElement in languageElements)
                {
                    var items = state.Apply(languageElement);
                    if (items == null) continue;

                    ComputeClosure(grammar, items);

                    var toState = states.FirstOrDefault(existingState => existingState.Items.SetEquals(items));
                    if (toState == null)
                    {
                        toState = new GeneratorState(stateId++, items);
                        states.Add(toState);
                        unevaluatedStates.Add(toState);
                    }

                    state.Transitions.Add(languageElement, toState);
                }
            }

            if (LinguaTrace.TraceSource.Switch.ShouldTrace(TraceEventType.Information))
            {
                foreach (var state in states)
                {
                    LinguaTrace.TraceEvent(TraceEventType.Information, LinguaTraceId.ID_GENERATE_STATE, "{0}", state);
                }
            }

            return states;
        }

        void ComputeClosure(IGrammar grammar, HashSet<GeneratorStateItem> items)
        {
            // Continue to loop until new more elements are added to the state.
            //
            var stateModified = true;
            while (stateModified)
            {
                var newItems = new HashSet<GeneratorStateItem>();

                // Iterate over the current elements in the state and determine (possible) new
                // elements to be added.
                //
                foreach (var item in items)
                {
                    var languageElement = item.RuleItem.DotElement;
                    if (languageElement == null || languageElement.ElementType != LanguageElementTypes.Nonterminal)
                        continue;
                    
                    var nonterminal = (NonterminalType)languageElement;
                    foreach (var rule in nonterminal.Rules)
                    {
                        var newItem = new GeneratorStateItem(new GeneratorRuleItem(rule, 0));
                        newItems.Add(newItem);
                    }
                }

                // Exit loop if all potential new elements already exist in state.  Otherwise, add new elements
                // and repeat process.
                //
                if (newItems.IsSubsetOf(items))
                {
                    stateModified = false;
                }
                else
                {
                    items.UnionWith(newItems);
                }
            }
        }
    }
}
