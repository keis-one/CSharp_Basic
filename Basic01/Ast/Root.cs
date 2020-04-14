using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basic01.Ast
{
    /// 抽象構文器(AST)でルートとなるノード
    /// INodeを実装
    /// 実装するプログラムを子要素として保持する。
    /// 全ての有効なコード文の集まり。
    public class Root : INode
    {
        public List<IStatement> Statements {get;set;}
        
        public string TokenLiteral()
        {
            return this.Statements.FirstOrDefault()?.TokenLiteral() ?? " ";
        }

        // 改行区切りで連結した文字列を返す。
        public string ToCode()
        {
            var builder = new StringBuilder();
            foreach(var ast in this.Statements)
            {
                builder.AppendLine(ast.ToCode());
            }
            // 末尾に改行コードがついてしまうため、Trimする。
            return builder.ToString().TrimEnd();
        }
    }
}