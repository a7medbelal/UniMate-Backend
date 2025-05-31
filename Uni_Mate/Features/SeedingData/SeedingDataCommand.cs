using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Data;
using Uni_Mate.Common.Views;
using Uni_Mate.Domain;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.GeneralEnum;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.SeedingData
{
    public record SeedingDataCommand() : IRequest<RequestResult<bool>>;

    public class SeedDemoDataCommandHandler : IRequestHandler<SeedingDataCommand, RequestResult<bool>>
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        public SeedDemoDataCommandHandler(Context context,
            UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<RequestResult<bool>> Handle(SeedingDataCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Create Owner
                var owner = new Owner
                {
                    UserName = "owner1",
                    Email = "owner1@test.com",
                    Fname = "Owner",
                    Lname = "One",
                    IsActive = true,
                    role = Models.UserManagment.Enum.Role.Owner
                };

                var resultOwner = await _userManager.CreateAsync(owner, "Owner@123");
                if (!resultOwner.Succeeded)
                {
                    return RequestResult<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.InvalidData, "Failed to create owner: " + string.Join(", ", resultOwner.Errors.Select(e => e.Description)));
                }

                // 2. Create Student
                var student = new Student
                {
                    UserName = "student1",
                    Email = "student1@test.com",
                    Fname = "Student",
                    Lname = "One",
                    IsActive = true,
                    role = Models.UserManagment.Enum.Role.Student
                };

                var resultStudent = await _userManager.CreateAsync(student, "Student@123");
                if (!resultStudent.Succeeded)
                {
                    return RequestResult<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.InvalidData, "Failed to create student: " + string.Join(", ", resultStudent.Errors.Select(e => e.Description)));
                }

                // 3. Create Apartment
                var apartment = new Apartment
                {
                    Num = 101,
                    Location = Location.QenaUniversity,
                    Description = "Demo apartment",
                    Gender = Gender.Male,
                    Floor = "2",
                    IsAvailable = true,
                    DurationType = ApartmentDurationType.Monthly,
                    OwnerID = owner.Id
                };

                // 4. Create Room
                var room = new Room
                {
                    Description = "Main Room",
                    IsAvailable = true,
                    Apartment = apartment,
                    Price = 1500
                };

                // 5. Create Bed
                var bed = new Bed
                {
                    IsAvailable = true,
                    Price = 500,
                    Room = room
                };

                await _context.Apartments.AddAsync(apartment);
                await _context.Rooms.AddAsync(room);
                await _context.Beds.AddAsync(bed);
                await _context.SaveChangesAsync();

                return RequestResult<bool>.Success(true, "Demo data inserted successfully");
            }
            catch (Exception ex)
            {
                return RequestResult<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.ServerError, $"Exception occurred: {ex.Message}");
            }
        }
    }

}


