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
    }
}