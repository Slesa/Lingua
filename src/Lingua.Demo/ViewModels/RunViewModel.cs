using System;
using System.Reactive;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using ReactiveUI;
using Lingua;

namespace Lingua.Demo.ViewModels
{
    public class RunViewModel : ViewModelBase
    {
        public RunViewModel()
        {
            OnEvaluateCommand = ReactiveCommand.Create(OnEvaluate);
            CreateParser();
        }
        
        ITerminalReader TerminalReader { get; set; }
        IParser Parser { get; set; }

        string _expression;
        public string Expression { 
            get { return _expression; } 
            private set { this.RaiseAndSetIfChanged(ref _expression, value); }
        }
        
        string _result;
        public string Result { 
            get { return _result; } 
            private set { this.RaiseAndSetIfChanged(ref _result, value); }
        }

        public ReactiveCommand<Unit, Unit> OnEvaluateCommand { get; }

        void OnEvaluate()
        {
            Result = "";
            TerminalReader.Open(Expression);
            Prolog.Clause result = (Prolog.Clause)Parser.Parse(TerminalReader);
            if (result != null)
            {
                Terminal token = TerminalReader.ReadTerminal();
                while (token != null)
                {
                    if (!token.ElementType.Ignore)
                    {
                        Result += $"{token.ElementType.Name}: {token.Text}" + Environment.NewLine;
                    }
                }
                token = TerminalReader.ReadTerminal();
                //Result = result; //.Value.ToString();
            }

            LinguaTrace.TraceSource.Flush();
        }

        void CreateParser()
        {
            var assembly = Assembly.GetAssembly(typeof(App));

            var grammar = new Grammar();
            grammar.Load(assembly, "Prolog");
            grammar.LoadRules(assembly, "Prolog");
            grammar.Resolve();

            Trace.WriteLine("TERMINALS");
            foreach (var terminal in grammar.GetTerminals())
            {
                Trace.WriteLine(terminal.Name);
                StringBuilder sb = new StringBuilder();
                sb.Append("  First:");
                foreach (var first in terminal.First)
                {
                    sb.Append(" ");
                    sb.Append(first == null ? "e" : first.Name);
                }
                Trace.WriteLine(sb.ToString());
            }

            Trace.WriteLine("NONTERMINALS");
            foreach (var nonterminal in grammar.GetNonterminals())
            {
                Trace.WriteLine(nonterminal.Name);

                foreach (var rule in nonterminal.Rules)
                {
                    Trace.WriteLine("  " + rule);
                }

                var sb = new StringBuilder();
                sb.Append("  First:");
                foreach (var first in nonterminal.First)
                {
                    sb.Append(" ");
                    sb.Append(first == null ? "e" : first.Name);
                }
                Trace.WriteLine(sb.ToString());

                sb = new StringBuilder();
                sb.Append("  Follow:");
                foreach (var first in nonterminal.Follow)
                {
                    sb.Append(" ");
                    sb.Append(first == null ? "e" : first.Name);
                }
                Trace.WriteLine(sb.ToString());
            }
        
            ITerminalReaderGenerator terminalReaderGenerator = new TerminalReaderGenerator();
            var terminalReaderGeneratorResult = terminalReaderGenerator.GenerateTerminalReader(grammar);
            TerminalReader = terminalReaderGeneratorResult.TerminalReader;

            IParserGenerator parserGenerator = new ParserGenerator();
            var parserGeneratorResult = parserGenerator.GenerateParser(grammar);
            Parser = parserGeneratorResult.Parser;

            ITerminalReader terminalReader = terminalReaderGeneratorResult.TerminalReader;
            terminalReader.Open("(123 + 34) / 52");

            IParser parser = parserGeneratorResult.Parser;
            parser.Parse(terminalReader);

            Terminal token = terminalReader.ReadTerminal();
            while (token != null)
            {
                if (!token.ElementType.Ignore)
                {
                    Console.WriteLine("{0}: {1}", token.ElementType.Name, token.Text);
                }
                token = terminalReader.ReadTerminal();
            }

            //Console.WriteLine("Press Enter to exit.");
            //Console.ReadLine();
        }
    }
}