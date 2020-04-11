using System.Collections.Generic;
using System.Linq;

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
    }
}