using System.Collections.Generic;

namespace Basic01.Lexing
{
    public class Token
    {
        public Token(TokenType type, string literal)
        {
            this.Type = type;
            this.Literal = literal;
        }
        public TokenType Type {get; set;}
        public string Literal {get;set;}

        // 文字列を引数にそれがキーワードかどうかを判定する。
        // キーワードであれば該当するTokenTypeを返す。
        public static TokenType LookupIdentifier(string identifier)
        {
            if(Token.Keywords.ContainsKey(identifier))
            {
                return Keywords[identifier];
            }
            return TokenType.IDENT;
        }

        // キーワードを管理
        public static Dictionary<string, TokenType> Keywords
            = new Dictionary<string, TokenType>(){
                {"LET", TokenType.LET},
                {"DEF", TokenType.FUNCTION},
                {"\r\n", TokenType.RETURNCODE},
                {"RETURN", TokenType.RETURN},
                {"END", TokenType.END},
            };
    }

    public enum TokenType
    {
        // 不正なトークン, 終端
        ILLEGAL,
        EOF,
        // 識別子, 整数リテラル
        IDENT,
        INT,
        // 演算子
        ASSIGN,
        PLUS,
        MINUS, 
        ASTERISK, 
        SLASH, 
        BANG, 
        LT, 
        GT, 
        EQ, 
        NOT_EQ, 
        // デリミタ
        COMMA,
        COLON,
        SEMICOLON,
        // 括弧(){}
        LPAREN,
        RPAREN,
        LBRACE,
        RBRACE,
        // キーワード
        FUNCTION,
        LET,
        IF, 
        ELSE, 
        TRUE, 
        FALSE, 
        RETURNCODE,
        RETURN,
        END,
        // TODO:その他必要になれば追加
    }

}