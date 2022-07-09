using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace properties
{
    internal class Token
    {
        TokenType type;
        object? value;
        public  Token(TokenType type)
        {
            this.type = type;
        }
        public Token(TokenType type, object? value) : this(type)
        {
            this.value = value;
        }
    }
}
