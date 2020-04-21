using System;
using Basic01.Ast;
using Basic01.Ast.Expressions;
using Basic01.Ast.Statements;
using Basic01.Lexing;
using Basic01.Parsing;
using Xunit;

namespace Basic01Test.Ast.Expressions
{
    public class InfixExpressionTest
    {
        [Fact]
        public void TestInfixExpressions1()
        {
            var tests = new [] {
                ("1 + 1", 1, "+", 1),
                ("1 - 1", 1, "-", 1),
                ("1 * 1", 1, "*", 1),
                ("1 / 1", 1, "/", 1),
                ("1 < 1", 1, "<", 1),
                ("1 > 1", 1, ">", 1),
                ("1 == 1", 1, "==", 1),
                ("1 != 1", 1, "!=", 1),
            };

            foreach(var (input, leftValue, op, rightValue) in tests)
            {
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
                
                var expression = statement.Expression as InfixExpression;
                if (expression == null)
                {
                    throw new Exception("expression が InfixExpression ではありません。");
                }
                this._TestIntegerLiteral(expression.Left, leftValue);

                if (expression.Operator != op)
                {
                    throw new Exception($"Operator が {expression.Operator} ではありません。({op})");
                }
                
                this._TestIntegerLiteral(expression.Right, rightValue);
            }
        }

        // 汎用メソッド
        private void _TestIntegerLiteral(IExpression expression, int value)
        {
            var integerLiteral = expression as IntegerLiteral;
            if (integerLiteral == null)
            {
                throw new Exception("Expression が IntegerLiteral ではありません。");
            }
            if (integerLiteral.Value != value)
            {
                throw new Exception($"integerLiteral.Value が {value} ではありません。");
            }
            if (integerLiteral.TokenLiteral() != $"{value}")
            {
                throw new Exception($"ident.TokenLiteral が {value} ではありません。");
            }
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
