using HospitalTask.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalTask.Data
{
    public class ApplicationDbContext:DbContext
    {
        
        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        //    :  base(options)
        //{
        //}
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<DoctorAvailability> DoctorAvailabilities { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<DoctorImage> DoctorImages { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=.;Database=HospitalDb;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
    }

}


