using System;
using Xunit;
using System.Collections.Generic;
using Basic01.Lexing;

namespace Basic01Test.Lexing
{
    public class LexerTest
    {
        [Fact]
        public void TestNextToken1()
        {
            var input = "=+(){},;";

            var testTokens = new List<Token>();
            testTokens.Add(new Token(TokenType.ASSIGN, "="));
            testTokens.Add(new Token(TokenType.PLUS, "+"));
            testTokens.Add(new Token(TokenType.LPAREN, "("));
            testTokens.Add(new Token(TokenType.RPAREN, ")"));
            testTokens.Add(new Token(TokenType.LBRACE, "{"));
            testTokens.Add(new Token(TokenType.RBRACE, "}"));
            testTokens.Add(new Token(TokenType.COMMA, ","));
            testTokens.Add(new Token(TokenType.SEMICOLON, ";"));
            testTokens.Add(new Token(TokenType.EOF, ""));

            var lexer = new Lexer(input);

            foreach(var testToken in testTokens)
            {
                var token = lexer.NextToken();
                Assert.Equal(testToken.Type, token.Type);   // トークンの種類の一致
                Assert.Equal(testToken.Literal, token.Literal);   // トークンのリテラルの一致
            }
        }


        [Fact]
        public void TestNextToken2()
        {
            var input = @"LET FIVE=5";

            var testTokens = new List<Token>();
            // LET FIVE=5
            testTokens.Add(new Token(TokenType.LET, "LET"));
            testTokens.Add(new Token(TokenType.IDENT, "FIVE"));
            testTokens.Add(new Token(TokenType.ASSIGN, "="));
            testTokens.Add(new Token(TokenType.INT, "5"));
            testTokens.Add(new Token(TokenType.EOF, ""));

            var lexer = new Lexer(input);

            foreach(var testToken in testTokens)
            {
                var token = lexer.NextToken();
                Assert.Equal(testToken.Type, token.Type);   // トークンの種類の一致
                Assert.Equal(testToken.Literal, token.Literal);   // トークンのリテラルの一致
            }
        }

        [Fact]
        public void TestNextToken3()
        {
            var input = @"LET FIVE=5
LET TEN=10

DEF ADD(X,Y)
 RETURN X+Y
END

LET RESULT = ADD(FIVE, TEN)
";

            var testTokens = new List<Token>();
            // LET FIVE=5
            testTokens.Add(new Token(TokenType.LET, "LET"));
            testTokens.Add(new Token(TokenType.IDENT, "FIVE"));
            testTokens.Add(new Token(TokenType.ASSIGN, "="));
            testTokens.Add(new Token(TokenType.INT, "5"));
            testTokens.Add(new Token(TokenType.RETURNCODE, "\r\n"));
            // LET TEN=10
            testTokens.Add(new Token(TokenType.LET, "LET"));
            testTokens.Add(new Token(TokenType.IDENT, "TEN"));
            testTokens.Add(new Token(TokenType.ASSIGN, "="));
            testTokens.Add(new Token(TokenType.INT, "10"));
            testTokens.Add(new Token(TokenType.RETURNCODE, "\r\n\r\n"));

            // DEF ADD(X,Y)
            testTokens.Add(new Token(TokenType.FUNCTION, "DEF"));
            testTokens.Add(new Token(TokenType.IDENT, "ADD"));
            testTokens.Add(new Token(TokenType.LPAREN, "("));
            testTokens.Add(new Token(TokenType.IDENT, "X"));
            testTokens.Add(new Token(TokenType.COMMA, ","));
            testTokens.Add(new Token(TokenType.IDENT, "Y"));
            testTokens.Add(new Token(TokenType.RPAREN, ")"));
            testTokens.Add(new Token(TokenType.RETURNCODE, "\r\n"));

            // RETURN X+Y
            testTokens.Add(new Token(TokenType.RETURN, "RETURN"));
            testTokens.Add(new Token(TokenType.IDENT, "X"));
            testTokens.Add(new Token(TokenType.PLUS, "+"));
            testTokens.Add(new Token(TokenType.IDENT, "Y"));
            testTokens.Add(new Token(TokenType.RETURNCODE, "\r\n"));

            // END
            testTokens.Add(new Token(TokenType.END, "END"));
            testTokens.Add(new Token(TokenType.RETURNCODE, "\r\n\r\n"));

            // LET RESULT = ADD(FIVE, TEN)
            testTokens.Add(new Token(TokenType.LET, "LET"));
            testTokens.Add(new Token(TokenType.IDENT, "RESULT"));
            testTokens.Add(new Token(TokenType.ASSIGN, "="));
            testTokens.Add(new Token(TokenType.IDENT, "ADD"));
            testTokens.Add(new Token(TokenType.LPAREN, "("));
            testTokens.Add(new Token(TokenType.IDENT, "FIVE"));
            testTokens.Add(new Token(TokenType.COMMA, ","));
            testTokens.Add(new Token(TokenType.IDENT, "TEN"));
            testTokens.Add(new Token(TokenType.RPAREN, ")"));
            testTokens.Add(new Token(TokenType.RETURNCODE, "\r\n"));

            testTokens.Add(new Token(TokenType.EOF, ""));

            var lexer = new Lexer(input);

            foreach(var testToken in testTokens)
            {
                var token = lexer.NextToken();
                Assert.Equal(testToken.Type, token.Type);   // トークンの種類の一致
                Assert.Equal(testToken.Literal, token.Literal);   // トークンのリテラルの一致
            }
        }
    }
}