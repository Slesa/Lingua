using Lingua;

namespace Lingua.Demo.Prolog
{
    [Grammar(Name="Prolog")]
    public class PrologNonterminal : Nonterminal
    { }

    // Clause ::= CompoundTerm OptionalBody
    //
    [Nonterminal(IsStart = true)]
    public class Clause : PrologNonterminal
    {
        public static void Rule(Clause lhs, CompoundTerm compoundTerm, OptionalRuleBody optionalBody)
        { }
    }

    // OptionalRuleBody ::= :- CompoundTerm AdditionalCompoundTerms
    //                  ::= nil
    //
    public class OptionalRuleBody : PrologNonterminal
    {
        public static void Rule(OptionalRuleBody lhs, ColonDash colonDash, CompoundTerm compoundTerm, AdditionalCompoundTerms additionalCompoundTerms)
        { }

        public static void Rule(OptionalRuleBody lhs)
        { }
    }

    // CompoundTerm ::= Functor ( CompoundTermBody )
    //
    public class CompoundTerm : PrologNonterminal
    {
        public static void Rule(CompoundTerm lhs, Functor functor, OpenParenthesis openParenthesis, CompoundTermBody compoundTermBody, CloseParenthesis closeParenthesis)
        { }
    }

    // Functor ::= Atom
    //
    public class Functor : PrologNonterminal
    {
        public static void Rule(Functor lhs, Atom atom)
        { }
    }

    // CompoundTermBody ::= CompoundTermMember AdditionalCompoundTerms
    //
    public class CompoundTermBody : PrologNonterminal
    {
        public static void Rule(CompoundTermBody lhs, CompoundTermMember compoundTermMember, AdditionalCompoundTerms additionalCompoundTerms)
        { }
    }

    // AdditionalCompoundTerms ::= , CompoundTermMember AdditionalCompoundTerms
    //                         ::= nil
    //
    public class AdditionalCompoundTerms : PrologNonterminal
    {
        public static void Rule(AdditionalCompoundTerms lhs, Comma comma, CompoundTermMember compoundTermMember, AdditionalCompoundTerms additionalCompoundTerms)
        { }

        public static void Rule(AdditionalCompoundTerms lhs)
        { }
    }

    // CompoundTermMember ::= Term
    //                    ::= Variable
    //                    ::= Integer
    //
    public class CompoundTermMember : PrologNonterminal
    {
        public static void Rule(CompoundTermMember lhs, Term term)
        { }

        public static void Rule(CompoundTermMember lhs, Variable variable)
        { }

        public static void Rule(CompoundTermMember lhs, Integer integer)
        { }
    }

    // Term ::= Atom OptionalTermBody
    //
    public class Term : PrologNonterminal
    {
        public static void Rule(Term lhs, Atom atom, OptionalTermBody optionalTermBody)
        { }
    }

    // OptionalTermBody ::= ( CompoundTermBody )
    //                  ::= nil
    //
    public class OptionalTermBody : PrologNonterminal
    {
        public static void Rule(OptionalTermBody lhs, OpenParenthesis openParenthesis, CompoundTermBody compoundTermBody, CloseParenthesis closeParenthesis)
        { }

        public static void Rule(OptionalTermBody lhs)
        { }
    }
}
