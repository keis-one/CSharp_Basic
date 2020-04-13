using Basic01.Lexing;

namespace Basic01.Ast.Statements
{
    public class ReturnStatement: IStatement
    {
        public Token Token {get; set;}
        public IExpression ReturnValue {get; set;}

        public string TokenLiteral() => this.Token.Literal;
    }
}