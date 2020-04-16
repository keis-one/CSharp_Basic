using System;
using System.Collections.Generic;
using Basic01.Ast;
using Basic01.Ast.Expressions;
using Basic01.Ast.Statements;
using Basic01.Lexing;

namespace Basic01.Parsing
{
    // 前置構文解析関数（prefix parsing function）
    // 左側になにもないので引数は不要
    using PrefixParseFn = Func<IExpression>;
    // 中置構文解析関数（infix parsing function）
    // 解析中の中置演算子の左側に来る式を引数として受け取る
    using InfixParseFn = Func<IExpression, IExpression>;

    /// 抽出構文器
    /// トークン列を受け取り、抽出構文木を生成する。
    public class Parser
    {
        public Token CurrentToken {get; set;}
        public Token NextToken {get; set;}
        public Lexer Lexer {get; set;}
        public List<string> Errors {get; set;}
            = new List<string>();

        // トークンの種類と解析関数の辞書を管理
        // これでこの解析器はトークンの種類がわかれば、適切な構文解析関数を呼び出せる
        public Dictionary<TokenType, PrefixParseFn> PrefixParseFns {get; set;}
        public Dictionary<TokenType, InfixParseFn> InfixParseFns {get; set;}

        public Parser(Lexer lexer)
        {
            this.Lexer = lexer;

            // ２つ分のトークンを読み込んでセットしておく
            this.CurrentToken = this.Lexer.NextToken();
            this.NextToken = this.Lexer.NextToken();

            // トークンの種類と解析関数を関連付ける
            this.RegisterPrefixParseFns();
        }

        private void RegisterPrefixParseFns()
        {
            this.PrefixParseFns = new Dictionary<TokenType, PrefixParseFn>();
            this.PrefixParseFns.Add(TokenType.IDENT, this.ParseIdentifer);
        }

        // 識別子の構文解析関数
        private IExpression ParseIdentifer()
        {
            // 現在の識別子トークンに対応する識別子のASTを生成して返す。
            // これをコンストラクタ内で TokenType.IDENT に紐づけて登録しておく。
            return new Identifier(this.CurrentToken, this.CurrentToken.Literal);
        }

        // トークンを読み進めるためのヘルパーメソッド
        public void ReadToken()
        {
            this.CurrentToken = this.NextToken;
            this.NextToken = this.Lexer.NextToken();
        }

        // 構文解析を行うためのメソッド
        // ここから全てが始まる。
        // 字句解析器からトークンを順次取り出し、抽象構文木を生成する。
        public Root ParseRoot()
        {
            return null;
        }

        // ループしながら式をパースして式のASTを生成する。
        // そして生成された式をルートに追加する。
        public Root ParseProgram()
        {
            var root = new Root();
            root.Statements = new List<IStatement>();
            
            // プログラムの終端まで
            while (this.CurrentToken.Type != TokenType.EOF)
            {
                var statement = this.ParseStatement();
                if(statement != null)
                {
                    root.Statements.Add(statement);
                }
                // パース後にトークンを読み進める。
                this.ReadToken();
            }
            return root;
        }

        // 全ての式のパターンを網羅してパースを行う。
        public IStatement ParseStatement()
        {
            switch (this.CurrentToken.Type)
            {
                case TokenType.LET:
                    // 現在のトークンがLETトークンであればParseLetStatementを呼び出す。
                    return this.ParseLetStatement();
                case TokenType.RETURN:
                    return this.ParseReturnStatement();
                default:
                    // 上記以外のトークンが来る場合にはすべて式文として解析する式文解析関数を呼び出す。
                    return this.ParseExpressionStatement();
            }
        }

        public IExpression ParseExpression(Precedence precedence)
        {
            // 現在のトークンの種類に関連つけられた解析関数を取り出す。
            this.PrefixParseFns.TryGetValue(this.CurrentToken.Type, out var prefix);
            // 関連する解析関数が存在しなければ、解析結果として Null を返す。
            if (prefix == null) return null;

            // 関連する解析関数が存在すればそれを実行し、得られる式のASTを返す。
            var leftExpression = prefix();
            return leftExpression;
        }

        public ExpressionStatement ParseExpressionStatement()
        {
            // 現在のトークンをセットし、式の解析関数 ParseExpression() を呼び出す。
            var statement = new ExpressionStatement();
            statement.Token = this.CurrentToken;

            statement.Expression = this.ParseExpression(Precedence.LOWEST);

            return statement;

        }

        // 現在のトークンからreturn文をパースしてReturnStatementを返す。
        private IStatement ParseReturnStatement()
        {
            var statement = new ReturnStatement();
            statement.Token = this.CurrentToken;
            this.ReadToken();

            // TODO:後で実装
            while (this.CurrentToken.Type != TokenType.RETURNCODE)
            {
                // リターンコードが見つかるまで
                this.ReadToken();
            }
            return statement;
        }

        // LET文をパースしてLetStatementノードを作成して返す。
        private IStatement ParseLetStatement()
        {
            var statement = new LetStatement();
            // 呼び出し元のcase条件となったトークンを設定
            statement.Token = this.CurrentToken;

            if(!this.ExpectPeek(TokenType.IDENT))
                return null;
            
            // 以下、左辺、等号、右辺をトークンを読み進める
            // 識別子　LET文の左辺
            statement.Name = new Identifier(
                this.CurrentToken, 
                this.CurrentToken.Literal);
            
            // 等号　=
            if (!this.ExpectPeek(TokenType.ASSIGN))
                return null;

                
            // 式　LET文の右辺
            // TODO:後で実装
            while (this.CurrentToken.Type != TokenType.RETURNCODE)
            {
                // 改行が見つかるまで
                this.ReadToken();
            }

            return statement;
        }

        // 次のトークンを先読みし、期待するトークンかどうかを判定する。
        private bool ExpectPeek(TokenType type)
        {
            // 次のトークンが期待するものであれば読み飛ばす
            if (this.NextToken.Type == type)
            {
                this.ReadToken();
                return true;
            }
            // そうでなければ何もせず判定結果のみ返す
            this.AddNextTokenError(type, this.NextToken.Type);
            return false;
        }

        // エラーメッセージをリストに追加する。
        private void AddNextTokenError(TokenType expected, TokenType actual)
        {
            this.Errors.Add($"{actual.ToString()} ではなく {expected.ToString()} が来なければなりません。");
        }
    }
    
    // 優先度は列挙体で定義
    public enum Precedence
    {
        LOWEST = 1,
        EQUALS,         // ==
        LESSGREATER,    // >,<
        SUM,            // +
        PRODUCT,        // *
        PREFIX,         // -x, !x
        CALL,           // myFunction(x)
    }
}