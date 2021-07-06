using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Core.Exceptions
{
    public class VadidateException: Exception
    {
        public VadidateException(string msg = "", object Data = null) : base(msg)
        {

            

            var objectReturn = new
                {
                    Msg = msg,
                    FieldNotValid = Data

                };
                this.Data.Add("FieldError", Data);
        }
    }
}
