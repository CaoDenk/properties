using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace properties
{
    internal class ProperiesParser
    {
        int line = 1;
        string buf;
        void skipWhite(ref int i)
        {
            while (i < buf.Length && (buf[i] == ' '
                || buf[i] == '\t'
                || buf[i] == '\b'))
            {
                i++;
            }

        }
        void skipComment(ref int i)
        {



        }
        public void parse()
        { 
            for(int i = 0; i < buf.Length; i++)
            {

                skipWhite(ref i);

                switch(buf[i])
                {
                    case '#':
                        skipWhite(ref i);
                        break;
                    case '"':
                    case '\'':
                        break;
                    case '\r':
                    case '\n':
                        line++;
                        break;
                    default:
                        break;
                }


            }
        
        
        }
    }
}
