using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.UserManagment;
using Uni_Mate.Models.UserManagment.Enum;

namespace Uni_Mate.Domain
{
    public class Context : IdentityDbContext<User>
    {

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        // here i added the DbSet for the user and the other models

        public DbSet<User> Users { get; set; }

        public DbSet<RoleFeature> roleFeatures { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<SocialAccount> SocialAccounts { get; set;}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            //here as i can see every quary i can see the sql sentacse about that query in the debug console
            //I made NoTracking prevent the change tracker from tracking the entities  that will be slow the perfomance 

            optionsBuilder
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .LogTo(log => Debug.WriteLine(log), LogLevel.Information)
                .EnableSensitiveDataLogging();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // appley the TPT inhertance for the User
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Owner>().ToTable("Owners");


            // store the enum as string in the database
            modelBuilder.Entity<User>()
                  .Property(u => u.Role)
                  .HasConversion<string>(); 


            // Seed admin 
            var hasher = new PasswordHasher<User>();

            // Seed admin user
            var admin = new User
            {
                Id = "1", // Use string if using Identity default keys
                Fname = "ahmed",
                Lname = "belal",
                UserName = "admin",
                Address ="qena" ,
                NormalizedUserName = "ADMIN",
                Email = "legendahmed.122@gmail.com",
                NormalizedEmail = "Legendahmed.122@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "01040363077",
                PhoneNumberConfirmed = true,
                Role = Role.Admin
            };
            admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");

            modelBuilder.Entity<User>().HasData(admin);

            base.OnModelCreating(modelBuilder);
        }


         public void SeedData(ModelBuilder modelBuilder)
        {
            //// Seed admin 
            //var hasher = new PasswordHasher<User>();

            //// Seed admin user
            //var admin = new User
            //{
            //    Id = "1", // Use string if using Identity default keys
            //    Fname ="ahmed",
            //    Lname="belal",
            //    UserName = "admin",
            //    NormalizedUserName = "ADMIN",
            //    Email = "legendahmed.122@gmail.com",
            //    NormalizedEmail = "Legendahmed.122@gmail.com",
            //    EmailConfirmed = true,
            //    PhoneNumber = "01040363077",
            //    PhoneNumberConfirmed = true,
            //    role = Role.Admin 
            //};
            //admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");

            //modelBuilder.Entity<User>().HasData(admin);
        }
    }
}
