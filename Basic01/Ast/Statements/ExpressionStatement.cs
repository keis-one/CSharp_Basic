using Basic01.Lexing;

namespace Basic01.Ast.Statements
{
    public class ExpressionStatement : IStatement
    {
        public Token Token {get; set;}  // 式の最初のトークン
        public IExpression Expression {get; set;}   // ラップしている実際の式
        
        public string ToCode() => this.Expression?.ToCode() ?? "";

        public string TokenLiteral() => this.Token.Literal;
    }
}