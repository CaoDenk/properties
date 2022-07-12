namespace properties
{
    internal class Program
    {

      //byte[] data;
      //  public Program(byte[] data)
      //  {
      //      this.data = data;
      //  }

      
        public static void Main(string[] args)
        {
            ProperiesParser parser = new ProperiesParser();
            parser.openFile("conf.properties");
            parser.lex();
            //foreach(var i in parser.tokens)
            parser.printTokenList();
            //parser.parse();

        }
    }
    



}
