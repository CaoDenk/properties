using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CalcCSharp
{
    class Parse
    {

        public string expression;
        private readonly static Type t = typeof(Math);
        public readonly Dictionary<string, double> constant = new Dictionary<string, double>()
        {

            {"pi",3.1415926535897932384626433832795 },//其实只能储存小数位数14位,后面会被编译器舍去
            {"e",2.7182818284590452353602874713527 },
            {"h",6.62607015e-34 },//普朗克常量
            {"G",6.67259e10-11 } //万有引力常量
        };
        public readonly Dictionary<string, string> func = new Dictionary<string, string>()
        {
            {"sin","Sin" },
            {"arcsin","Asin" },
            {"asin","Asin" },
            {"cos","Cos" },
            {"arccos","Acos" },
            {"acos","Acos" },
            {"sqrt","Sqrt" },
            {"log","Log" },
            {"ln","Log" },
            {"log10","Log10" },
            {"tan","Tan" }
        };


        private readonly Dictionary<char, int> prio = new Dictionary<char, int>()
        {
            {'#',0 },
            {'+',1 },
            {'-',1 },
            {'*',2 },
            {'/',2 },
            {'!',2 },
            {'^',3 },

        };
        const char funcToken = '$';//记录函数位置
        public Stack<Complex> complexStack { get; } = new Stack<Complex>();
        Stack<char> operTmp = new Stack<char>();

        Stack<string> functions = new Stack<string>();
        public Parse(string expression)
        {
            this.expression = expression;
            operTmp.Push('#');
        }
        public double calculateFunction(string methodName, double num)
        {
            MethodInfo methodInfo = t.GetMethod(methodName, new Type[] { typeof(double) });
            object result = methodInfo.Invoke(null, new object[] { num });
            return (double)result;
        }
        public Complex parseAndCalc()
        {

            CheckExpression checkStr = new CheckExpression(expression);
            checkStr.check();
            for (int i = 0; i < expression.Length; i++)
            {
                switch (expression[i])
                {
                    case ' ':
                    case '\t':
                        break;
                    case '+':
                    case '-':
                        if (isSign(i))
                        {
                            complexStack.Push(new Complex(0));
                            operTmp.Push(expression[i]);
                            break;
                        }
                        else
                            goto case '^';
                    case '*':
                    case '/':
                    case '!': //单目运算                      
                    case '^':
                        comparePrioAndCalcul(expression[i]);
                        break;
                    case '(':
                        if (NeedToAddMul(i))
                            operTmp.Push('*');
                        operTmp.Push(expression[i]);
                        break;
                    case ')':
                        handleRightBrace();
                        break;
                    case '[':
                        //  parseMatrix(ref i);//怎么解析 到矩阵，用栈?
                        break;
                    case '='://赋值语句 先挖个坑
                        break;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        readNumber(ref i, out Complex res);
                        complexStack.Push(res);
                        skipWhiteSpace(ref i);
                        if (i < expression.Length)
                        {
                            if (char.IsLetter(expression[i]))
                            {
                                comparePrioAndCalcul('*');
                                i--;

                            }
                            else if (isLeftBrace(expression[i]))
                            {
                                comparePrioAndCalcul('*');
                                operTmp.Push(expression[i]);

                            }
                            else
                            {
                                i--;

                            }
                        }
                        break;
                    case 'i':
                        if (i < expression.Length && !char.IsLetter(expression[i + 1])) //这个是虚数
                        {
                            //虚数前面需要加* 比如5i->5*i，方便计算

                            if (NeedToAddMul(i))
                            {
                                operTmp.Push('*');
                            }
                            complexStack.Push(new Complex { imag = 1 });
                            break;
                        }
                        readString(ref i);
                        break;
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                    case 'g':
                    case 'h':
                    case 'j':
                    case 'k':
                    case 'l':
                    case 'm':
                    case 'o':
                    case 'p':
                    case 'q':
                    case 'r':
                    case 's':
                    case 't':
                    case 'u':
                    case 'v':
                    case 'w':
                    case 'x':
                    case 'y':
                    case 'z':
                    case 'A':
                    case 'B':
                    case 'C':
                    case 'D':
                    case 'E':
                    case 'F':
                    case 'G':
                    case 'H':
                    case 'I':
                    case 'J':
                    case 'K':
                    case 'L':
                    case 'M':
                    case 'N':
                    case 'O':
                    case 'P':
                    case 'Q':
                    case 'R':
                    case 'S':
                    case 'T':
                    case 'U':
                    case 'V':
                    case 'W':
                    case 'X':
                    case 'Y':
                    case 'Z':
                        readString(ref i);
                        break;
                    default:
                        throw new Exception("illeagle  char <" + expression[i] + '>');
                }

            }
            while (operTmp.Peek() != '#')
            {
                calculateToFar();
            }
            return complexStack.Pop();
        }
        private void handleRightBrace()//处理右括号
        {
            while (!isLeftBrace(operTmp.Peek()))
            {
                calculateToFar();
            }
            operTmp.Pop();
            if (operTmp.Peek() == funcToken)//调用函数
            {
                if (!func.ContainsKey(functions.Peek()))
                    throw new Exception("not function found  ->" + functions.Peek());
                if(complexStack.Peek().imag==0)
                {
                    double num = complexStack.Pop().real;
                    double res = calculateFunction(func[functions.Pop()], num);
                    complexStack.Push(new Complex(res));
                    operTmp.Pop();//弹出$,也就是函数符号
                }else
                {
                    throw new Exception("尚未实现虚数的函数功能");

                }
              
               
            }
        }
        private void skipWhiteSpace(ref int i)
        {
            while (i < expression.Length && char.IsWhiteSpace(expression[i]))
                i++;
        }
        private void readNumber(ref int i, out Complex res)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(expression[i]);
            i++;
            int flag = 0;//记录小数点个数
            while (i < expression.Length)
            {
                if (char.IsDigit(expression[i]))
                {
                    sb.Append(expression[i]);
                    i++;
                }
                else if (expression[i] == '.' && flag == 0)
                {
                    sb.Append(expression[i]);
                    flag++;//小数点个数++
                    i++;
                }
                else if (expression[i] == 'i') //数字后面紧跟 才是复数,5i (√)  5 i(error) 
                {
                    Complex complex = new Complex
                    {
                        imag = double.Parse(sb.ToString())
                    };
                    res = complex;
                    i++;
                    return;//已经读到运算符了
                }
                else if (expression[i] == '!')//说明是阶乘
                {
                    if (flag == 0) //必须是整数
                    {
                        int fac = int.Parse(sb.ToString());//获取阶乘
                        res = new Complex(fact(fac));
              
                        i++;
                        return;
                    }
                    else
                    {
                        throw new Exception("小数没有阶乘");
                    }

                }
                else
                {
                    break;
                }
            }


            res = new Complex(double.Parse(sb.ToString()));
           
        }


        private void readString(ref int i)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(expression[i]);
            i++;
            while (i < expression.Length && char.IsLetterOrDigit(expression[i]))
            {
                sb.Append(expression[i]);
                i++;
            }
            skipWhiteSpace(ref i);
            if (i >= expression.Length || !isLeftBrace(expression[i]))//是常量
            {
                if(!constant.ContainsKey(sb.ToString()))
                {
                    throw new Exception("not found constant variable->" + sb.ToString());
                }

                Complex c = new Complex(constant[sb.ToString()]);
                complexStack.Push(c);
                i--;
                skipWhiteSpace(ref i);  //  支持sin (x)  这种写法 
            }
            else if (isLeftBrace(expression[i]))
            {
                
                functions.Push(sb.ToString());
                operTmp.Push(funcToken);
                operTmp.Push(expression[i]);
            }
            else
                throw new Exception("not found function ->" + sb.ToString());
        }

        //比较优先级，并且计算
        private void comparePrioAndCalcul(char c)
        {
       
           if (isLeftBrace(operTmp.Peek()))//如果操作符前面是括号，操作符直接进栈
            {
                operTmp.Push(c);
                return;
            }
            while(prio[c]<=prio[operTmp.Peek()])
            {
                calculateToFar();//计算栈里的数
            }
            operTmp.Push(c);
        }
        private Complex calculate(Complex num1, Complex num2, char oper)
        {
            switch (oper)
            {
                case '+':
                    return num1 + num2;
                case '-':
                    return num1 - num2;
                case '*':
                    return num1 * num2;
                case '/':
                    return num1 / num2;
                case '^':
                   return num1^num2;
                default:
                    throw new Exception("不合法的操作符->"+oper);
            }
       
        }
        private bool isSign(int i)
        {
            do
            {
                i--;
            } while (i >= 0 && char.IsWhiteSpace(expression[i]));
            if (i<0||(i >=0 && expression[i] == '('))
                return true;
            else
                return false;

        }
        //虚数前面需要加* 比如5i->5*i
        bool isLeftBrace(char c)
        {
            switch(c)
            {
                case '(':
                case '[':
                case '{':
                    return true;
                default:
                    return false;
            }
        }
        bool isRightBrace(char c)
        {
            switch (c)
            {
                case ')':
                case ']':
                case '}':
                    return true;
                default:
                    return false;
            }
        }

        bool NeedToAddMul(int i)//需要加*？
        {
            do
            {
                i--;
            } while (i >= 0 && char.IsWhiteSpace(expression[i]));
            if (i>=0&&isRightBrace(expression[i]))  
                return true;
            else
                return false;
        }

        private int fact(int f)//阶乘 
        {
            if (f == 0 || f == 1) //0!=1 1!=1;
            {

                return 1;
            }
            else if (f > 0)
            {
                int facRes = f;//阶乘结果
                for (int j = f - 1; j > 0; j--)
                {
                    facRes *= j;

                }
                return facRes;
            }
            else
                return -fact(-f); 
        }

  
        void calculateToFar()//计算一次栈里数据
        {


            Complex c1 = complexStack.Pop();
            Complex c2 = complexStack.Pop();

            Complex result = calculate(c2, c1, operTmp.Pop());
            complexStack.Push(result);


        }


    }


}
