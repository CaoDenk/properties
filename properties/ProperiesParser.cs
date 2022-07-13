using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace properties
{
    internal class ProperiesParser
    {

        public Dictionary<string, string> properies { get; } = new Dictionary<string, string>();
        int line = 1;
        TokenList tokens = new TokenList();
        char[] buf;
        public void openFile(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            int buffSize = (int)fi.Length;//配置文件大小不可能超过int的范围

            byte[] bytes = new byte[buffSize + 1];
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            fileStream.Read(bytes, 0, buffSize);
            fileStream.Close();
            //  EF BB BF 默认特殊字符
            if (bytes[0] == 0XEF && bytes[1] == 0XBB && bytes[2] == 0XBF)
            {
                buf = Encoding.Default.GetChars(bytes, 3, bytes.Length - 3);
            }
            else
                buf = Encoding.Default.GetChars(bytes);

        
        }
   
        void skipComment(ref int i)
        {
            do
            {
                i++;
            } while (i < buf.Length && buf[i] != '\n');
            tokens.add(TokenType.ENDLINE);

        }
        public void lex()
        {

            for (int i = 0; i<buf.Length ; i++)
            {
                switch (buf[i])
                {
                    case '#':
                        skipComment(ref i);
                        break;
                    case '=':
                        tokens.add(TokenType.EQUAL);
                        break;
                    case '\r':
                        if (buf[i + 1] == '\n')
                        {
                            i += 2;
                            tokens.add(TokenType.ENDLINE);
                        }
                        break;
                    case '\n':
                        tokens.add(TokenType.ENDLINE);
                        break;
                    case '\'':
                    case '"':
                        readRString(ref i);//如果存在  name =Donald Trump 必须加引号  => name ="Donald Trump"
                        break;
                    case ' ':
                    case '\t':
                        break;
                    case '\0':
                        goto End;
                    default:
                        readString(ref i);
                        break;
                }


            }
     End:    if(tokens.Peek.type!=TokenType.END)
            {
                if (tokens.Peek.type != TokenType.ENDLINE)
                    tokens.add(TokenType.ENDLINE);
                tokens.add(TokenType.END);
            }
        }

        /*
         解析带有双引号的value
         */
        void readRString(ref int i)
        {
            StringBuilder stringBuilder = new StringBuilder();

            i++;
            while (buf[i] != '"')
            {

                stringBuilder.Append(buf[i]);
                i++;
                if (buf[i]=='\0')
                {
                    throw new Exception("缺少双引号");
                }

            }
            tokens.add(TokenType.STRING, stringBuilder.ToString());
        }


        public void printTokenList()
        {

            tokens.printAllToken();

        }

        public void parse()
        {

            int j = 0;

            while (tokens[j].type != TokenType.END)
            {

                while (tokens[j].type == TokenType.ENDLINE)
                {
                    line++;
                    j++;
                }

                if (tokens[j].type == TokenType.STRING)
                {

                    if (match(j))
                    {
                        string key = (string)tokens[j].value;
                        string value = (string)tokens[j + 2].value;
                        properies.Add(key, value);
                        j += 4;
                        line++;
                    }

                    else
                    {
                        throw new Exception("在" + line + "行，格式错误");
                    }
                }

            }

        }

        bool match( int j)
        {
            if (
                tokens[j + 1].type == TokenType.EQUAL 
                && tokens[j+2].type==TokenType.STRING
                && tokens[j+3].type==TokenType.ENDLINE)
            { 
                return true;
            }

            return false;
        }

        void readString(ref int i)
        {

            StringBuilder stringBuilder = new();
            while (i<buf.Length-1)
            {
                stringBuilder.Append(buf[i]);
                i++;

                switch(buf[i])
                {
                    case '=':
                        tokens.add(TokenType.STRING, stringBuilder.ToString());
                        tokens.add(TokenType.EQUAL);
                        return;
                    case ' ':
                    case '\t':
                        tokens.add(TokenType.STRING, stringBuilder.ToString());
                        return;
                    case '#':
                        tokens.add(TokenType.STRING, stringBuilder.ToString());
                        skipComment(ref i);
                        return;
                    case '\r':
                        if (buf[i + 1] == '\n')
                        {
                            tokens.add(TokenType.STRING, stringBuilder.ToString());
                            tokens.add(TokenType.ENDLINE);
                            i++;
                            return;
                        }
                        break;
                    case '\n':
                        tokens.add(TokenType.STRING, stringBuilder.ToString());
                        tokens.add(TokenType.ENDLINE);
                        return;
                    case '\0':
                        tokens.add(TokenType.STRING, stringBuilder.ToString());
                        tokens.add(TokenType.ENDLINE);
                        tokens.add(TokenType.END);
                        return;
                    default:
                        break;
                }
            }
        }
    }
}
