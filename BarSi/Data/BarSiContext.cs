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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MedicalEquipmentSupply>().HasKey(mes => new { mes.HospitalId, mes.MedicalEquipmentId });

            modelBuilder.Entity<MedicalEquipmentSupply>()
                .HasOne(mes => mes.Hospital)
                .WithMany(h => h.medicalEquipmentSupplies)
                .HasForeignKey(mes => mes.HospitalId);

            modelBuilder.Entity<MedicalEquipmentSupply>()
                .HasOne(mes => mes.MedicalEquipment)
                .WithMany(me => me.medicalEquipmentSupplies)
                .HasForeignKey(mes => mes.MedicalEquipmentId);
        }

        public DbSet<BarSi.Models.Patient> Patient { get; set; }

        public DbSet<BarSi.Models.Hospital> Hospital { get; set; }

        public DbSet<BarSi.Models.City> City { get; set; }

        public DbSet<BarSi.Models.Doctor> Doctor { get; set; }

        public DbSet<BarSi.Models.PatientStatus> PatientStatus { get; set; }

        public DbSet<BarSi.Models.MedicalEquipment> MedicalEquipment { get; set; }

        public DbSet<BarSi.Models.Location> Location { get; set; }

        public DbSet<BarSi.Models.User> User { get; set; }
    }
}
