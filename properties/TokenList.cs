using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace properties
{
    internal class TokenList
    {

        List<Token> tokens = new List<Token>();
        public void add(TokenType type)
        {

            tokens.Add(new Token(type));
        }
        public void add(TokenType type, object? value)
        {
            tokens.Add(new Token(type, value));
        }
        public void printAllToken()
        {
            foreach (Token token in tokens)
            {
                Console.WriteLine(token.ToString());
            }
        }
        public  Token this[int index] => tokens[index];
        public Token Peek => tokens[tokens.Count() - 1];
    }
}
