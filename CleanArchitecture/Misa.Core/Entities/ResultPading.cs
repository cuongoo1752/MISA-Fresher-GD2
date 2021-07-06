using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Core.Entities
{
    
    
        public class ResultPading
        {
            public ResultPading()
            {

            }
            public ResultPading(Array data, int length)
            {
                this.Data = data;
                this.Length = length;
            }

       
            public Array Data { get; set; }

        public int Length { get; set; }
    }
    
}
