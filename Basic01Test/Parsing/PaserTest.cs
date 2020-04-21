using System;
using Xunit;
using Basic01.Ast;
using Basic01.Ast.Statements;
using Basic01.Lexing;
using Basic01.Parsing;
using Basic01.Ast.Expressions;

namespace Basic01Test.Parsing
{
    public class ParserTest
    {
        [Fact]
        // foobar を構文解析し、正しく式のASTが生成できているかを確認
        public void TestIdentiferExpression1()
        {
            var input = @"foobar";

            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var root = parser.ParseProgram();
            this._CheckParserErrors(parser);

            Assert.Equal(root.Statements.Count, 1);

            var statement = root.Statements[0] as ExpressionStatement;
            if (statement == null)
            {
                throw new Exception("statement が ExpressionStatement ではありません。");
            }

            var ident = statement.Expression as Identifier;
            if (ident == null)
            {
                throw new Exception("Expression が Identifier ではありません。");
            }
            if (ident.Value != "foobar")
            {
                throw new Exception("ident.Value が foobar ではありません。");
            }
            if (ident.TokenLiteral() != "foobar")
            {
                throw new Exception("ident.TokenLiteral が foobar ではありません。");
            }
        }

        [Fact]
        public void TestLetStatement1()
        {
            var input = @"LET X = 5
LET Y=10
LET XYZ =  1000
";
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var root = parser.ParseProgram();
            this._CheckParserErrors(parser);

            /// 構文はX,Y,XYZの３ルート
            Assert.Equal(root.Statements.Count, 3);

            var tests = new string[] {"X", "Y", "XYZ"};
            for (int i = 0; i < tests.Length; i++)
            {
                var name = tests[i];
                var statement = root.Statements[i];
                this._TestLetStatement(statement, name);
            }
        }

        [Fact]
        // 演算子の優先度に応じて解析できているのかを確認
        public void TestOperatorPrecedenceParsing()
        {
            var tests = new[]
            {
                ("a + b", "(a + b)"),
                ("!-a", "(!(-a))"),
                ("a + b - c", "((a + b) - c)"),
                ("a * b / c", "((a * b) / c)"),
                ("a + b * c", "(a + (b * c))"),
                ("a + b * c + d / e - f", "(((a + (b * c)) + (d / e)) - f)"),
                ("1 + 2 -3 * 4", "((1 + 2) - (3 * 4))"),
                (@"1 + 2: -3 * 4", "(1 + 2)\r\n((-3) * 4)"),
                (@"1 + 2
 -3 * 4", "(1 + 2)\r\n((-3) * 4)"),
                ("5 > 4 == 3 < 4", "((5 > 4) == (3 < 4))"),
                ("3 + 4 * 5 == 3 * 1 + 4 * 5", "((3 + (4 * 5)) == ((3 * 1) + (4 * 5)))"),
            };
            foreach (var (input, code) in tests)
            {
                var lexer = new Lexer(input);
                var parser = new Parser(lexer);
                var root = parser.ParseProgram();
                this._CheckParserErrors(parser);

                var actual = root.ToCode();
                Assert.Equal(code, actual);
            }
        }
        
        // LET文のテスト
        private void _TestLetStatement(IStatement statement, string name)
        {
            // キーワードはLET
            Assert.Equal(statement.TokenLiteral(), "LET");

            var letStatement = statement as LetStatement;
            if(letStatement == null)
                throw new Exception("statement が LetStatement ではありません。");

            // 識別子の確認
            Assert.Equal(letStatement.Name.TokenLiteral(), name);
            Assert.Equal(letStatement.Name.Value, name);
        }

        // 汎用メソッド
        private void _CheckParserErrors(Parser parser)
        {
            if (parser.Errors.Count == 0) return;
            var message = "\n" + string.Join("\n", parser.Errors);
            throw new Exception(message);
        }
    }
}