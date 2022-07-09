using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace properties
{
    internal class TokenList
    {

        List<Token> tokens=new List<Token>();
        public void add(TokenType type)
        {

            tokens.Add(new Token(type));
        }
        public void add(TokenType type,object? value)
        {
            tokens.Add(new Token(type,value));
        }
    }
}
