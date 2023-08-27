using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_HW_Lesson4.Entities
{
    public class withdrawal
    {

        public int Id { get; set; }
        public int amount { get; set; }

        public Client Client { get; set; }

    }
}
