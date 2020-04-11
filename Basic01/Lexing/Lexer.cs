using System;
namespace Basic01.Lexing
{
    public class Lexer
    {
        public string _input {get; private set;}
        public char _currentChar {get; private set;}    //_positionが指す値
        public char _nextChar {get; private set;}   // 次の文字を先読みして保持
        public int _position {get; private set;}    // 現在読み出している文字位置

        /// 字句解析器
        /// 構文上意味のある語でトークン列を生成します。
        /// ソースコードからトークン列を生成する機能。
        public Lexer(string input)
        {
            _input = input;
            _input.DebugLog("_input");
            ReadChar();
        }

        /// 呼び出すごとにソースコードからトークンを生成
        public Token NextToken()
        {
            SkipWhiteSpace();
            Token token = null;
            switch (_currentChar)
            {
                case '=':
                    token = new Token(TokenType.ASSIGN, _currentChar.ToString());
                    break;
                case '+':
                    token = new Token(TokenType.PLUS, _currentChar.ToString());
                    break;
                case ',':
                    token = new Token(TokenType.COMMA, _currentChar.ToString());
                    break;
                case ';':
                    token = new Token(TokenType.SEMICOLON, _currentChar.ToString());
                    break;
                case '(':
                    token = new Token(TokenType.LPAREN, _currentChar.ToString());
                    break;
                case ')':
                    token = new Token(TokenType.RPAREN, _currentChar.ToString());
                    break;
                case '{':
                    token = new Token(TokenType.LBRACE, _currentChar.ToString());
                    break;
                case '}':
                    token = new Token(TokenType.RBRACE, _currentChar.ToString());
                    break;
                case (char)0:
                    token = new Token(TokenType.EOF, "");
                    break;
                default:
                    if(IsLetter(_currentChar))
                    {
                        var identifier = ReadIdentifier();
                        var type = Token.LookupIdentifier(identifier);
                        token = new Token(type, identifier);
                    }
                    else if(IsDigit(_currentChar))
                    {
                        var number = ReadNumber();
                        token = new Token(TokenType.INT, number);
                    }
                    else if(IsReturnCode(_currentChar))
                    {
                        var code = ReadReturnCode();
                        token = new Token(TokenType.RETURNCODE, code);
                    }
                    else
                    {
                        token = new Token(TokenType.ILLEGAL, _currentChar.ToString());
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
            var identifier = _currentChar.ToString();
            // 次の文字がLetterであればそれを読んで加える
            while (IsLetter(_nextChar))
            {
                identifier += _nextChar;
                ReadChar();
            }
            return identifier;
        }
        /// 現在の文字から識別子に対応した文字である限り読み進め、
        /// 識別子に対応した文字列を返す。
        private string ReadNumber()
        {
            var number = _currentChar.ToString();
            // 次の文字が数値であればそれを読んで加える
            while (IsDigit(_nextChar))
            {
                number += _nextChar;
                ReadChar();
            }
            return number;
        }
        /// 現在の文字から識別子に対応した文字である限り読み進め、
        /// 識別子に対応した文字列を返す。
        private string ReadReturnCode()
        {
            var code = _currentChar.ToString();
            // 次の文字が改行コードであればそれを読んで加える
            while (IsReturnCode(_nextChar))
            {
                code += _nextChar;
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
        /// 0～9のいずれかからなる文字列かどうか。
        /// TODO:小数や16進数など複雑な数値には対応していない。
        private bool IsReturnCode(char c)
        {
            return (c == '\r' || c == '\n');
        }

        /// 1文字を読み進めるためのメソッド
        /// 範囲外まで進むとNULL文字（0）で終端を表す。
        private void ReadChar()
        {
            if(_position >= _input.Length)
            {
                _currentChar = (char)0;
            }
            else
            {
                _currentChar = _input[_position];
            }

            if(_position + 1 >= _input.Length)
            {
                _nextChar = (char)0;
            }
            else
            {
                _nextChar = _input[_position + 1];
            }
            _position.DebugLog("_position");
            _currentChar.DebugLog("_currentChar");
            _nextChar.DebugLog("_nextChar");

            _position += 1;
        }

        /// 空白やタブは読み飛ばす
        private void SkipWhiteSpace()
        {
                while(_currentChar == ' '
                    || _currentChar == '\t')
                {
                    ReadChar();
                }
        }
    }
}