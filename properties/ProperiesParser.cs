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
        //int len;
      TokenList tokens  =new TokenList();
      byte[] buf;
        //string buf;
      public  void openFile(string filePath)
        {
            //int buffSize = 1024 * 1024 * 4;//4M
            FileInfo fi = new FileInfo(filePath);
            int buffSize = (int)fi.Length;//配置文件大小不可能超过int的范围




            
            buf = new byte[buffSize];
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            fileStream.Read(buf, 0, buffSize);

            

            fileStream.Close();

        }
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
            do
            {
                i++;
            } while (i < buf.Length && buf[i] != '\n');


        }
        public void parse()
        { 
           
            for(int i = 0; i < buf.Length; i++)
            {

                skipWhite(ref i);

                switch((char)buf[i])
                {
                    case '#':
                        skipComment(ref i);
                        break;
                    case '=':
                        {
                            tokens.add(TokenType.EQUAL);
                        }
                        break;
                    case '\r':
                    case '\n':
                        //line++;
                        tokens.add(TokenType.ENDLINE);
                        break;
                    case '\'':
                    case '"':
                        readRString(ref i);//如果存在  name =Donald Trump 必须加引号  => name ="Donald Trump"
                        break;
                    default:
                        readString(ref i);
                        break;
                }


            }
        
        
        }

        /*
         解析带有双引号的value
         */
        void readRString(ref int i)
        {
            StringBuilder stringBuilder = new StringBuilder();
            i++;
            do
            {
                if (buf[i] == '\r')
                {
                    throw new Exception("缺少双引号");
                }
                stringBuilder.Append((char)buf[i]);

                //if()
                i++;
            } while (buf[i] != '"');

            tokens.add(TokenType.STRING,stringBuilder.ToString());
            // return stringBuilder.ToString();
        }
        /*
        解析不带有双引号的value
     */
        void readString(ref int i)
        {
            StringBuilder stringBuilder = new StringBuilder();
            do
            {
                //if (buff[i] == '\r')
                //{
                //    throw new Exception("缺少双引号");
                //}

                if (buf[i] == '\r')
                {
                    i++;
                    continue;
                }
                stringBuilder.Append((char)buf[i]);

                //if()
                i++;


                if (i >= buf.Length)
                {
                    tokens.add(TokenType.STRING, stringBuilder.ToString());
                    tokens.add(TokenType.END);
                    return;
                }
                if (buf[i] == '=')
                {
                    tokens.add(TokenType.STRING, stringBuilder.ToString());
                    tokens.add(TokenType.EQUAL);
                    return;
                }


            } while (buf[i] !='\n') ;

            tokens.add(TokenType.STRING, stringBuilder.ToString());
            tokens.add(TokenType.ENDLINE);
            // return stringBuilder.ToString();
        }


        public void printTokenList()
        {

            tokens.printAllToken();

        }
    }
}
