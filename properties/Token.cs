using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace properties
{
    internal class Token
    {
        public TokenType type { get; }
        public object? value { get; }
        public  Token(TokenType type)
        {
            this.type = type;
        }
        public Token(TokenType type, object? value) : this(type)
        {
            this.value = value;
        }
        public override string ToString()
        {
           switch(type)
            {
                case TokenType.STRING:
                    return "string:"+value;
                case TokenType.EQUAL:
                    return "=";
                case TokenType.ENDLINE:
                    return "endline";
                case TokenType.END:
                    return "end";
                default:
                    return "";
            }
        }
    }
}
