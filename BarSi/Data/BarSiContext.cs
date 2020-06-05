using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BarSi.Models;

namespace BarSi.Data
{
    public class BarSiContext : DbContext
    {
        public BarSiContext (DbContextOptions<BarSiContext> options)
            : base(options)
        {
        }

        public DbSet<BarSi.Models.Patient> Patient { get; set; }
    }
}
