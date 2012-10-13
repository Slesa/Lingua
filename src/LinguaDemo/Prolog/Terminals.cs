using Lingua;

namespace LinguaDemo.Prolog
{
    [Grammar(Name="Prolog")]
    public class PrologTerminal : Terminal
    { }

    [Terminal(@"[a-z][a-zA-Z0-9_]{0,99}")]
    public class Atom : PrologTerminal
    { }

    [Terminal(@"\-{0,1}[0-9]{1,10}")]
    public class Integer : PrologTerminal
    { }

    [Terminal(@"[A-Z][a-zA-Z0-9_]{0,99}")]
    public class Variable : PrologTerminal
    { }

    [Terminal(@"\(")]
    public class OpenParenthesis : PrologTerminal
    { }

    [Terminal(@"\)")]
    public class CloseParenthesis : PrologTerminal
    { }

    [Terminal(@"\,")]
    public class Comma : PrologTerminal
    { }

    [Terminal(@"\[")]
    public class LeftBracket : PrologTerminal
    { }

    [Terminal(@"\]")]
    public class RightBracket : PrologTerminal
    { }

    [Terminal(@"\.")]
    public class Period : PrologTerminal
    { }

    [Terminal(@"\:\-")]
    public class ColonDash : PrologTerminal
    { }
}
