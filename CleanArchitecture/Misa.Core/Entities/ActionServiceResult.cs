using Misa.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Core.Entities
{
    public class ActionServiceResult
    {
        public ActionServiceResult()
        {

        }

        public ActionServiceResult(bool success, string msg, MISACode code, int total = 0)
        {
            this.Success = success;
            this.Messenge = msg;
            this.Code = code;
            this.Total = total;
        }

        public ActionServiceResult(bool success, string msg, MISACode code, object data, int total = 0)
        {
            this.Success = success;
            this.Messenge = msg;
            this.Code = code;
            this.Data = data;
            this.Total = total;
        }
        public MISACode Code { get; set; }
        public object Data { get; set; }
        public bool Success { get; set; }

        public string Messenge { get; set; }
        public int? Total { get; set; }
    }

}
