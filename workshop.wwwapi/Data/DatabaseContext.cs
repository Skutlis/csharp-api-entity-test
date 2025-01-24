using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using workshop.wwwapi.Models;

namespace workshop.wwwapi.Data
{
    public class DatabaseContext : DbContext
    {
        private string _connectionString;
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString")!;
            
            
            this.Database.EnsureCreated();
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TODO: Appointment Key etc.. Add Here
            
            
            modelBuilder.Entity<Doctor>()
                .HasKey(d => d.Id);

            
            modelBuilder.Entity<Patient>()
                .HasKey(p => p.Id);

            
            modelBuilder.Entity<Appointment>()
                .HasKey(a => new {a.DoctorId, a.PatientId});

            //TODO: Seed Data Here

            // Populate the tables (created a seperare databaseinitializer, dont know which one actually populates the database)
            Seed s = new Seed();
            modelBuilder.Entity<Doctor>(d => d.HasData(s.Doctors));
            modelBuilder.Entity<Patient>(p => p.HasData(s.Patients));
            modelBuilder.Entity<Appointment>(a => a.HasData(s.Appointments));

            // Assign relationships
            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Appointments)
                .WithOne()
                .HasForeignKey(a => a.DoctorId);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Appointments)
                .WithOne()
                .HasForeignKey(a => a.PatientId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments);
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments);
            

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseInMemoryDatabase(databaseName: "Database");
            optionsBuilder.UseNpgsql(_connectionString);
            optionsBuilder.LogTo(message => Debug.WriteLine(message)); //see the sql EF using in the console
            
        }




        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
