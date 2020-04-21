using Basic01.Lexing;
using System.Text;

namespace Basic01.Ast.Expressions
{
    // 中置演算子を用いた式（1+1, 1-1, 1*1, 1/1, 1>1, 1<1, 1==1, 1!=1など）
    public class InfixExpression : IExpression
    {

        // 構造は<expression> <infix operator> <expression>;
        public Token Token {get; set;}
        public IExpression Left {get; set;}
        public string Operator {get; set;}
        public IExpression Right {get; set;}

        // (左辺 演算子 右辺)で出力されるようにする
        public string ToCode()
        {
            var builder = new StringBuilder();
            builder.Append("(");
            builder.Append(this.Left.ToCode());
            builder.Append(" ");
            builder.Append(this.Operator);
            builder.Append(" ");
            builder.Append(this.Right.ToCode());
            builder.Append(")");
            return builder.ToString();
        }

        public string TokenLiteral() => this.Token.Literal;
    }
}