using Basic01.Ast;
using Basic01.Ast.Expressions;
using Basic01.Ast.Statements;
using Basic01.Lexing;
using Xunit;
using System.Collections.Generic;

namespace Basic01Test.Ast
{
    public class AstTest
    {
        [Fact]
        public void NodeToCodeTest()
        {
            var code = "LET X = abc";

            var root = new Root();
            root.Statements = new List<IStatement>();

            root.Statements.Add(
                new LetStatement()
                {
                    Token = new Token(TokenType.LET, "LET"),
                    Name = new Identifier(
                        new Token(TokenType.IDENT, "X"),
                        "X"
                    ),
                    Value = new Identifier(
                        new Token(TokenType.IDENT, "abc"),
                        "abc"
                    ),
                }
            );
            Assert.Equal(code, root.ToCode());
        }
    }
}