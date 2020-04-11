namespace Basic01.Ast
{
    public interface INode
    {
        // ノードに紐付けられるトークンを返すメソッド
        // ※デバッグでのみ使用
        string TokenLiteral();
    }
}