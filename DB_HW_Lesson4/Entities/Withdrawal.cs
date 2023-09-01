using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_HW_Lesson4.Entities
{
    public class Withdrawal
    {

        public int id { get; set; }
        public decimal amount { get; set; }
        public int clientId { get; set; }
        public virtual Client Client { get; set; }

    }
}
