using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.BookingManagement;
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

        //public DbSet<User> Users { get; set; }

        public DbSet<RoleFeature> roleFeatures { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<SocialAccount> SocialAccounts { get; set;}
        public DbSet<FavoriteApartment> FavoriteApartments { get; set; }
        public DbSet<Booking> Bookings { get; set; }
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


            modelBuilder.Entity<BookRoom>().ToTable("BookRooms");
            modelBuilder.Entity<BookBed>().ToTable("BookBeds");
            modelBuilder.Entity<BookApartment>().ToTable("BookApartments");

            // store the enum as string in the database
            modelBuilder.Entity<User>()
                  .Property(u => u.role)
                  .HasConversion<string>();

            // Store The Enum ImageType As String In The Database
            modelBuilder.Entity<Image>()
                .Property(i => i.ImageType)
                .HasConversion<string>();

            // Configure The Image Entity To Avoid Cyclical References
            modelBuilder.Entity<Image>()
                .HasOne(i => i.Apartment)
                .WithMany(a => a.Images)
                .HasForeignKey(i => i.ApartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Add cascade delete for Apartment -> Room
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Apartment)
                .WithMany(a => a.Rooms)
                .HasForeignKey(r => r.ApartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Add cascade delete for Apartment -> ApartmentFacility
            modelBuilder.Entity<ApartmentFacility>()
                .HasOne(af => af.Apartment)
                .WithMany(a => a.ApartmentFacilities)
                .HasForeignKey(af => af.ApartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Bed>()
                .HasOne(b => b.Room)
                .WithMany(r => r.Beds)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            // Add cascade delete for Apartment -> Bed (through Room)
            // (Handled by Room -> Bed relationship, not directly Apartment -> Bed)

            // Seed admin 
            var hasher = new PasswordHasher<User>();

            // Seed admin user
            var admin = new User
            {
                Id = "1", // Use string if using Identity default keys
                Fname = "ahmed",
                Lname = "belal",
                UserName = "admin",
                //Address ="qena" ,
                NormalizedUserName = "ADMIN",
                Email = "legendahmed.122@gmail.com",
                NormalizedEmail = "Legendahmed.122@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "01040363077",
                PhoneNumberConfirmed = true,
                role = Role.Admin,
                National_Id = "1",
            };
            admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");

            modelBuilder.Entity<User>().HasData(admin);


            var student = new User
            {
                Id = "lkdjslkdfslkd", // Use string if using Identity default keys
                Fname = "Ziad",
                Lname = "Atta",
                UserName = "ZiadAtta",
                //Address ="qena" ,
                NormalizedUserName = "Student",
                Email = "ziadatta723@gmail.com",
                NormalizedEmail = "ziadatta723@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "01040363077",
                PhoneNumberConfirmed = true,
                role = Role.Student,
                National_Id = "30401252703615",
            };
            student.PasswordHash = hasher.HashPassword(admin, "Ziad23@ndmcdr");

            modelBuilder.Entity<User>().HasData(student);




            //Create the indexes for better performance 
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();    

            // Create the indexes for Search for apartments
            modelBuilder.Entity<Apartment>()
                .HasIndex(a => a.Location);

            modelBuilder.Entity<Apartment>()
               .HasIndex(a => a.Gender);

            modelBuilder.Entity<Apartment>()
                .HasIndex(a => a.Capecity); 

            modelBuilder.Entity<Apartment>()
                .HasIndex(a => a.CreatedDate);

            modelBuilder.Entity<Apartment>()
                .HasIndex(a => new { a.Location, a.Gender, a.Capecity });

            modelBuilder.Entity<Room>()
                .HasIndex(r => new { r.ApartmentId ,r.Price});


            base.OnModelCreating(modelBuilder);
        }


      
    }
}
