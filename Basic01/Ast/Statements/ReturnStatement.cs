using System.Text;
using Basic01.Lexing;

namespace Basic01.Ast.Statements
{
    public class ReturnStatement: IStatement
    {
        public Token Token {get; set;}
        public IExpression ReturnValue {get; set;}

        public string TokenLiteral() => this.Token.Literal;

        public string ToCode()
        {
            var builder = new StringBuilder();
            builder.Append(this.Token?.Literal ?? "");
            builder.Append(" ");
            builder.Append(this.ReturnValue?.ToCode() ?? "");
            builder.Append("\n");
            return builder.ToString();
        }
    }
}