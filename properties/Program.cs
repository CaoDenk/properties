namespace properties
{
    internal class Program
    {


      
        public static void Main(string[] args)
        {
            //ProperiesParser parser = new ProperiesParser();
            //parser.openFile("conf.properties");
            //parser.lex();
            ////foreach(var i in parser.tokens)
            //parser.printTokenList();
            //parser.parse();
            Properties properties = new Properties("conf.properties");
            int res= properties.getInt("amount");
            Console.WriteLine(res);
        }
    }
    



}
