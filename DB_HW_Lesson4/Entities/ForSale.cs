using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_HW_Lesson4.Entities
{
    public class ForSale
    {

        public int Id { get; set; }
        public int Cost { get; set; }
        public string Type { get; set; }
        public Client User { get; set; }

    }
}
