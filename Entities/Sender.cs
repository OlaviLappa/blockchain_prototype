using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blockchain_prototype.Entities
{
    public class Sender : ISender
    {
        public Wallet Wallet { get; set; }
        public string Address { get; set; }
        public string? Name { get; set; }
        public DateTime? DateAccountCreating { get; set; }
    }
}