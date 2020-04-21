using Basic01.Lexing;

namespace Basic01.Ast.Expressions
{
    public class IntegerLiteral : IExpression
    {
        public Token Token {get; set;}
        public int Value {get;set;}

        public string ToCode() => this.Token.Literal;
        public string TokenLiteral() => this.Token.Literal;
    }
}
