using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Core.Attributes
{   
    [AttributeUsage(AttributeTargets.Property)]
    class MISARequired : Attribute
    {

    }
    class MISAEmailFormat : Attribute 
    {

    }


    class MISAMaxLength : Attribute
    {
        public string Msg;
        public int MaxLength;
        public MISAMaxLength(string msg = "", int maxLength = 255 )
        {
            Msg = msg;
            MaxLength = maxLength;
        }
    }
}
