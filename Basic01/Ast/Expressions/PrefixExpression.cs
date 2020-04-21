using Basic01.Lexing;

namespace Basic01.Ast.Expressions
{
    /// 前置演算子を用いた式（-5, !fooなど）
    public class PrefixExpression : IExpression
    {
        // 構造は<prefix operator><expression>;
        public Token Token {get; set;}
        public string Operator {get; set;}
        public IExpression Right {get; set;}

        // 括弧でくくって出力するようにする
        public string ToCode() => $"({this.Operator}{this.Right.ToCode()})";
        public string TokenLiteral() => this.Token.Literal;
    }
}