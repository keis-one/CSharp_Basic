using System;
using Basic01.Ast.Statements;
using Basic01.Lexing;
using Basic01.Parsing;
using Xunit;

namespace Basic01Test.Ast.Statements
{
    public class ReturnStatementsTest
    {
        [Fact]
        public void TestReturnStatement1()
        {
            var input = @"RETURN 5
                RETURN 10
                RETURN = 993322
                ";

            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var root = parser.ParseProgram();

            Assert.Equal(root.Statements.Count, 3);

            foreach(var statement in root.Statements)
            {
                var returnStatement = statement as ReturnStatement;
                if (returnStatement == null)
                {
                    throw new Exception("statement が ReturnStatement ではありません。");
                }

                Assert.Equal(returnStatement.TokenLiteral(), "RETURN");
            }
        }
    }
}