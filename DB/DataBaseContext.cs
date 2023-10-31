using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using blockchain_prototype.Entities;
using Microsoft.EntityFrameworkCore;

namespace blockchain_prototype.DB
{
    public class DataBaseContext :DbContext
    {
        public DbSet<Wallet> Wallets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}