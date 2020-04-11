using Basic01.Ast.Expressions;
using Basic01.Lexing;

namespace Basic01.Ast.Statements
{
    /// LET <identifier> = <expression>
    public class LetStatement : IStatement
    {
        public Token Token {get; set;}  // LETトークン
        public Identifier Name {get; set;}  // 識別子
        public IExpression Value {get; set;}    // 式

        public string TokenLiteral()
            => this.Token.Literal;

    }
}