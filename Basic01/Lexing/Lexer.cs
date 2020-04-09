namespace Basic01.Lexing
{
    public class Lexer
    {
        public string _input {get; private set;}
        public char _currentChar {get; private set;}    //_positionが指す値
        public char _nextChar {get; private set;}   // 次の文字を先読みして保持
        public int _position {get; private set;}    // 現在読み出している文字位置

        // 字句解析器
        public Lexer(string input)
        {
            _input = input;
            _input.DebugLog("_input");
            ReadChar();
        }

        /// 呼び出すごとにソースコードからトークンを生成
        public Token NextToken()
        {
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
            }

            // 次の文字に進める
            ReadChar();
            token.DebugLog("token");
            return token;
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
    }
}