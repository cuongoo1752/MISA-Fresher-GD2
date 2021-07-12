using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Core.Entities.Page
{
    public class OptionPage
    {
        public int page { get; set; }
        public int start { get; set; }
        public int limit { get; set; }
        public List<Sort> sort { get; set; }
        public List<Filter> filter { get; set; }

    }
}
