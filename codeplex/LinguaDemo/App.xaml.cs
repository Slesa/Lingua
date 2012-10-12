/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows;

using Lingua;

namespace LinguaDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private ITerminalReader m_terminalReader;
        private IParser m_parser;

        #endregion

        #region Public Properties

        public ITerminalReader TerminalReader
        {
            get { return m_terminalReader; }
        }

        public IParser Parser
        {
            get { return m_parser; }
        }

        #endregion

        #region Event Handlers

        private void LinguaDemo_Startup(object sender, StartupEventArgs e)
        { }

        #endregion

        #region Hidden Members

        public void CreateParser()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(App));

            Grammar grammar = new Grammar();
            grammar.Load(assembly, "Prolog");
            grammar.LoadRules(assembly, "Prolog");
            grammar.Resolve();

            Trace.WriteLine("TERMINALS");
            foreach (TerminalType terminal in grammar.GetTerminals())
            {
                Trace.WriteLine(terminal.Name);
                StringBuilder sb = new StringBuilder();
                sb.Append("  First:");
                foreach (TerminalType first in terminal.First)
                {
                    sb.Append(" ");
                    if (first == null)
                    {
                        sb.Append("e");
                    }
                    else
                    {
                        sb.Append(first.Name);
                    }
                }
                Trace.WriteLine(sb.ToString());
            }

            Trace.WriteLine("NONTERMINALS");
            foreach (NonterminalType nonterminal in grammar.GetNonterminals())
            {
                StringBuilder sb;

                Trace.WriteLine(nonterminal.Name);

                foreach (RuleType rule in nonterminal.Rules)
                {
                    Trace.WriteLine("  " + rule.ToString());
                }

                sb = new StringBuilder();
                sb.Append("  First:");
                foreach (TerminalType first in nonterminal.First)
                {
                    sb.Append(" ");
                    if (first == null)
                    {
                        sb.Append("e");
                    }
                    else
                    {
                        sb.Append(first.Name);
                    }
                }
                Trace.WriteLine(sb.ToString());

                sb = new StringBuilder();
                sb.Append("  Follow:");
                foreach (TerminalType first in nonterminal.Follow)
                {
                    sb.Append(" ");
                    if (first == null)
                    {
                        sb.Append("e");
                    }
                    else
                    {
                        sb.Append(first.Name);
                    }
                }
                Trace.WriteLine(sb.ToString());
            }

            ITerminalReaderGenerator terminalReaderGenerator = new TerminalReaderGenerator();
            TerminalReaderGeneratorResult terminalReaderGeneratorResult = terminalReaderGenerator.GenerateTerminalReader(grammar);
            m_terminalReader = terminalReaderGeneratorResult.TerminalReader;

            IParserGenerator parserGenerator = new ParserGenerator();
            ParserGeneratorResult parserGeneratorResult = parserGenerator.GenerateParser(grammar);
            m_parser = parserGeneratorResult.Parser;

            //ITerminalReader terminalReader = terminalReaderGeneratorResult.TerminalReader;

            //terminalReader.Open("(123 + 34) / 52");

            //IParser parser = parserGeneratorResult.Parser;
            //parser.Parse(terminalReader);

            //Terminal token = terminalReader.ReadTerminal();
            //while (token != null)
            //{
            //    if (!token.ElementType.Ignore)
            //    {
            //        Console.WriteLine("{0}: {1}", token.ElementType.Name, token.Text);
            //    }
            //    token = terminalReader.ReadTerminal();
            //}

            //Console.WriteLine("Press Enter to exit.");
            //Console.ReadLine();
        }

        #endregion
    }
}
