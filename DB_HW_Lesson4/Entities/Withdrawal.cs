using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_HW_Lesson4.Entities
{
    public class Withdrawal
    {

        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

    }
}
