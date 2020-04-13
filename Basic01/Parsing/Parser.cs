using System;
using System.Collections.Generic;
using Basic01.Ast;
using Basic01.Ast.Expressions;
using Basic01.Ast.Statements;
using Basic01.Lexing;

namespace Basic01.Parsing
{
    /// 抽出構文器
    /// トークン列を受け取り、抽出構文木を生成する。
    public class Parser
    {
        public Token CurrentToken {get; set;}
        public Token NextToken {get; set;}
        public Lexer Lexer {get; set;}
        public List<string> Errors {get; set;}
            = new List<string>();

        public Parser(Lexer lexer)
        {
            this.Lexer = lexer;

            // ２つ分のトークンを読み込んでセットしておく
            this.CurrentToken = this.Lexer.NextToken();
            this.NextToken = this.Lexer.NextToken();
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
                default:
                    return null;
            }
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
}