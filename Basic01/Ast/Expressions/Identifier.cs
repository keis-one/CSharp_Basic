using Basic01.Lexing;

namespace Basic01.Ast.Expressions
{
    /// 式
    public class Identifier : IExpression
    {
        public Token Token {get; set;}
        public string Value {get; set;} // 変数名を指す
        public string TokenLiteral()
            => this.Token?.Literal ?? "";

        public Identifier(Token token, string value)
        {
            Token = token;
            Value = value;
        }

        public string ToCode()
            => this.Token.Literal;
    }
}