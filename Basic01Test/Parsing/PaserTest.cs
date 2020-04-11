using System;
using Xunit;
using Basic01.Ast;
using Basic01.Ast.Statements;
using Basic01.Lexing;
using Basic01.Parsing;

namespace Basic01Test.Parsing
{
    public class ParserTest
    {
        [Fact]
        public void TestLetStatement1()
        {
            var input = @"LET X = 5
LET Y=10
LET XYZ = 838383
";
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var root = parser.ParseProgram();

            Assert.Equal(root.Statements.Count, 3);

            var tests = new string[] {"X", "Y", "XYZ"};
            for (int i = 0; i < tests.Length; i++)
            {
                var name = tests[i];
                var statement = root.Statements[i];
                this._TestLetStatement(statement, name);
            }
        }

        private void _TestLetStatement(IStatement statement, string name)
        {
            Assert.Equal(statement.TokenLiteral(), "LET");

            var letStatement = statement as LetStatement;
            if(letStatement == null)
                throw new Exception("statement が LetStatement ではありません。");

            Assert.Equal(letStatement.Name.TokenLiteral(), name);
            Assert.Equal(letStatement.Name.Value, name);
        }
    }
}