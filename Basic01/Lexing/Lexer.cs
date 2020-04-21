using System;
namespace Basic01.Lexing
{
    public class Lexer
    {
        public string Input {get; private set;}
        public char CurrentChar {get; private set;}    //Positionが指す値
        public char NextChar {get; private set;}   // 次の文字を先読みして保持
        public int Position {get; private set;}    // 現在読み出している文字位置

        /// 字句解析器
        /// 構文上意味のある語でトークン列を生成します。
        /// ソースコードからトークン列を生成する機能。
        public Lexer(string input)
        {
            this.Input = input;
            this.Input.DebugLog("Input");
            ReadChar();
        }

        /// 呼び出すごとにソースコードからトークンを生成
        public Token NextToken()
        {
            SkipWhiteSpace();
            Token token = null;
            switch (this.CurrentChar)
            {
                case '=':
                    if(this.NextChar == '=')
                    {
                        token = new Token(TokenType.EQ, "==");
                        this.ReadChar();
                    }
                    else
                    {
                        token = new Token(TokenType.ASSIGN, this.CurrentChar.ToString());
                    }
                    break;
                case '+':
                    token = new Token(TokenType.PLUS, this.CurrentChar.ToString());
                    break;
                case '-':
                    token = new Token(TokenType.MINUS, this.CurrentChar.ToString());
                    break;
                case '*': 
                    token = new Token(TokenType.ASTERISK, this.CurrentChar.ToString()); 
                    break; 
                case '/': 
                    token = new Token(TokenType.SLASH, this.CurrentChar.ToString()); 
                    break; 
                case '!':
                    if (this.NextChar == '=')
                    {
                        token = new Token(TokenType.NOT_EQ, "!=");
                        this.ReadChar();
                    }
                    else
                    {
                        token = new Token(TokenType.BANG, this.CurrentChar.ToString());
                    }
                    break;
                case '>': 
                    token = new Token(TokenType.GT, this.CurrentChar.ToString()); 
                    break; 
                case '<': 
                    token = new Token(TokenType.LT, this.CurrentChar.ToString()); 
                    break;
                case ',':
                    token = new Token(TokenType.COMMA, this.CurrentChar.ToString());
                    break;
                case ':':
                        token = new Token(TokenType.COLON, "\r\n");
                    break;
                case ';':
                    token = new Token(TokenType.SEMICOLON, this.CurrentChar.ToString());
                    break;
                case '(':
                    token = new Token(TokenType.LPAREN, this.CurrentChar.ToString());
                    break;
                case ')':
                    token = new Token(TokenType.RPAREN, this.CurrentChar.ToString());
                    break;
                case '{':
                    token = new Token(TokenType.LBRACE, this.CurrentChar.ToString());
                    break;
                case '}':
                    token = new Token(TokenType.RBRACE, this.CurrentChar.ToString());
                    break;
                case (char)0:
                    token = new Token(TokenType.EOF, "");
                    break;
                default:
                    if(IsLetter(this.CurrentChar))
                    {
                        var identifier = ReadIdentifier();
                        var type = Token.LookupIdentifier(identifier);
                        token = new Token(type, identifier);
                    }
                    else if(IsDigit(this.CurrentChar))
                    {
                        var number = ReadNumber();
                        token = new Token(TokenType.INT, number);
                    }
                    else if(IsReturnCode(this.CurrentChar))
                    {
                        var code = ReadReturnCode();
                        token = new Token(TokenType.RETURNCODE, code);
                    }
                    else
                    {
                        token = new Token(TokenType.ILLEGAL, this.CurrentChar.ToString());
                    }
                    break;
            }

            // 次の文字に進める
            ReadChar();
            token.DebugLog("token");

            return token;
        }

        /// 現在の文字から識別子に対応した文字である限り読み進め、
        /// 識別子に対応した文字列を返す。
        private string ReadIdentifier()
        {
            var identifier = this.CurrentChar.ToString();
            // 次の文字がLetterであればそれを読んで加える
            while (IsLetter(this.NextChar))
            {
                identifier += this.NextChar;
                this.ReadChar();
            }
            return identifier;
        }
        /// 現在の文字から識別子に対応した文字である限り読み進め、
        /// 識別子に対応した文字列を返す。
        private string ReadNumber()
        {
            var number = this.CurrentChar.ToString();
            // 次の文字が数値であればそれを読んで加える
            while (IsDigit(this.NextChar))
            {
                number += this.NextChar;
                ReadChar();
            }
            return number;
        }
        /// 現在の文字から識別子に対応した文字である限り読み進め、
        /// 識別子に対応した文字列を返す。
        private string ReadReturnCode()
        {
            var code = this.CurrentChar.ToString();
            // 次の文字が改行コードであればそれを読んで加える
            while (IsReturnCode(this.NextChar))
            {
                code += this.NextChar;
                ReadChar();
            }
            return code;
        }

        /// _もしくはa～z,A～Zのいずれかからなる文字列かどうか。
        /// 数字は含まない。
        private bool IsLetter(char c)
        {
            return ('a' <= c && c <= 'z')
                || ('A' <= c && c <= 'Z')
                || c == '_';
        }
        /// 0～9のいずれかからなる文字列かどうか。
        /// TODO:小数や16進数など複雑な数値には対応していない。
        private bool IsDigit(char c)
        {
            return ('0' <= c && c <= '9');
        }
        /// 改行コードいずれかからなる文字列かどうか。
        private bool IsReturnCode(char c)
        {
            return (c == '\r' || c == '\n');
        }

        /// 1文字を読み進めるためのメソッド
        /// 範囲外まで進むとNULL文字（0）で終端を表す。
        private void ReadChar()
        {
            if(this.Position >= this.Input.Length)
            {
                this.CurrentChar = (char)0;
            }
            else
            {
                this.CurrentChar = this.Input[this.Position];
            }

            if(this.Position + 1 >= this.Input.Length)
            {
                NextChar = (char)0;
            }
            else
            {
                this.NextChar = this.Input[this.Position + 1];
            }
            this.Position.DebugLog("Position");
            this.CurrentChar.DebugLog("CurrentChar");
            this.NextChar.DebugLog("NextChar");

            this.Position += 1;
        }

        /// 空白やタブは読み飛ばす
        private void SkipWhiteSpace()
        {
                while(this.CurrentChar == ' '
                    || this.CurrentChar == '\t')
                {
                    ReadChar();
                }
        }
    }
}