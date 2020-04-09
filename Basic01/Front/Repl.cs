using Basic01.Lexing;
using System;

namespace Basic01.Front
{
    /// 入力を読み取り(Read)、評価(Eval)し、出力(Print)し、
    /// これらを繰り返す(loop)、という処理を行う機能
    /*  処理の流れは以下の通り。
        1.入力待ち
        2.入力文字列字句解析する
        3.得られたトークン列を表示
        4.1に戻る
    */
    public class Repl
    {
        const string PROMPT = ">> ";

        /// 入力を待機し、入力された文字列を字句解析して
        /// その結果得られたトークン列を表示
        public void Start()
        {
            while(true)
            {
                Console.Write(PROMPT);

                var input = Console.ReadLine();
                if(string.IsNullOrEmpty(input)) return;

                var lexer = new Lexer(input);
                for(var token = lexer.NextToken();
                    token.Type != TokenType.EOF;
                    token = lexer.NextToken()
                    )
                {
                    Console.WriteLine($"{{ Type: {token.Type.ToString()}, Literal: {token.Literal}}}");
                }
            }
        }
    }
}