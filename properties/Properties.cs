using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcCSharp;
namespace properties
{
    internal class Properties
    {
        Dictionary<string, string> properties;
        public Properties(string file)
        {
            ProperiesParser properiesParser = new ProperiesParser();
            properiesParser.openFile(file);
            properiesParser.lex();
            properiesParser.parse();
            properties = properiesParser.properies;

        }
       public  int getInt(string key)
        {


            properties.TryGetValue(key, out string value);
            Parse paser = new Parse(value);
            Complex res = paser.parseAndCalc();
            return  (int)res.real;
        }
        public string getString(string key)
        {
            properties.TryGetValue(key, out string value);
            return value;
        }
    }
}
