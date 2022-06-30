using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace properties
{

    /*在properties中支持 
    
    // a = [6,7,8,9]  数字集合
      只支持string=string
    end
     */
    internal enum Token
    {

        STRING,
        EQUAL,
        ENDLINE,
        END,
    }
}
