using System.Text;
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

        public string ToCode()
        {
            var builder = new StringBuilder();
            builder.Append(this.Token?.Literal ?? "");
            builder.Append(" ");
            builder.Append(this.Name?.ToCode() ?? "");
            builder.Append(" = ");
            builder.Append(this.Value?.ToCode() ?? "");
            return builder.ToString();
        }
    }
}