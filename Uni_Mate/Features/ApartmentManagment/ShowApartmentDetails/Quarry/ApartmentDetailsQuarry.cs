using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Views;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Features.ApartmentManagment.ShowApartmentDetails.ApartmentDTO;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.BookingManagement;
using Uni_Mate.Models.GeneralEnum;
using Uni_Mate.Models.UserManagment;
namespace Uni_Mate.Features.ApartmentManagment.ShowApartmentDetails.Quarry
{
    public record ApartmentDetailsQuarry(int id) : IRequest<RequestResult<ApartmentDetailsDTO>>;

    public class ApartmentDetailsHandler : BaseRequestHandler<ApartmentDetailsQuarry, RequestResult<ApartmentDetailsDTO>, Apartment>
    {
        private readonly IRepository<BookRoom> _bookRoomRepo;
        private readonly IRepository<BookBed> _bookBedRepo;
        private readonly IRepositoryIdentity<Student> _studentRepo;
        public ApartmentDetailsHandler(
            BaseRequestHandlerParameter<Apartment> parameters,
            IRepositoryIdentity<Student> studentRepo,
            IRepository<BookRoom> bookRoomRepo,
            IRepository<BookBed> bookBedRepo
            ) : base(parameters)
        {
            _bookBedRepo = bookBedRepo;
            _studentRepo = studentRepo;
            _bookRoomRepo = bookRoomRepo;
        }

        public override async Task<RequestResult<ApartmentDetailsDTO>> Handle(ApartmentDetailsQuarry request, CancellationToken cancellationToken)
        {
            var userId = _userInfo.ID;
            /*
             * 1- check on the apartment 
             * 2- connect the apartment the images and the rooms 
             * 3- the rooms must assign to the beds 
             * 4- connect every room with its the user how booked 
             * 5- connect the beds in the room with the roommates 
             * 6- get the details for every student the details is (look to the ui , ux )
             * 7- we got the ImageRoom from this i can check to this image 
             */
            // To Improve In The Future ======>>>
            if (request.id <= 0)
            {
                return RequestResult<ApartmentDetailsDTO>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.InvalidData, "Apartment ID Is Invalid");
            }

            //Get Apartment With The Navigate Property To Get Access To All Relation Of The Apartment
            var apartment = await _repository.GetWithIncludeAsync(request.id, "Images", "Rooms.Beds", "ApartmentFacilities.Facility.FacilityCategory");

            if (apartment == null)
            {
                return RequestResult<ApartmentDetailsDTO>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.NotFound, "The Apartment Not Found");
            }

            var detailsApartment = new ApartmentDetailsDTO();

            #region Map The Details Of Apartment To apartmmentDTO

            ApartmentDTO.ApartmentDTO apartmentDTO = new ApartmentDTO.ApartmentDTO
            {
                Id = apartment.Id,
                Description = apartment.Description,
                Floor = apartment.Floor,
                Location = apartment.Location.ToString(),
                DescripeLocation = apartment.DescripeLocation,
                IsAvailable = apartment.IsAvailable,
                kind = apartment.Gender == Gender.Male ? "Male" : apartment.Gender == Gender.Female ? "Female" : "Null",
                Price = apartment.Rooms?.Sum(x => x.Price) ?? 0,
                RoomCount = apartment.Rooms?.Count() ?? 0,
                BedRoomCount = apartment.Rooms?.SelectMany(r => r.Beds ?? Enumerable.Empty<Bed>()).Count() ?? 0,
            };
            #endregion


            #region Map The List And The Image To ApartmentDTO

            // Select And Map It To RoomDTO As List
            var roomsDTOs = apartment.Rooms?.Select(
                i => new RoomDTO
                {
                    Id = i.Id,
                    ImageUrl = i.Image,
                }
            ).ToList();

            // Select And Map It To ImageApartDTO As List 
            var imageDTOS = apartment.Images?.Select(i => new ImageApartDTO
            {
                Id = i.Id,
                ImageUrl = i.ImageUrl,
                Kind = i.ImageType.ToString()
            }).ToList();
            #endregion

            #region AddCategoryFacilities

            // First Map It To List OF Pair 
            var facilityPairs = apartment.ApartmentFacilities

                .Where(af => af.Facility != null && af.Facility.FacilityCategory != null)

                .Select(af => new KeyValuePair<string, string>(

                    // Will Act Like A Key In Dictionary
                    af.Facility.FacilityCategory.Name,
                    // Will Act Like A Value In Dictionary
                    af.Facility.Name

                    )).ToList();


            /*
             *  To Make The FacilityPairs By Key And Value and Must The Key Be Unique 
             *  This Dictionary Is of <string , List<string>> 
             *  Example :
             *  Pair<kitchen,"van"> ,Pair <"kitchen , "fridge"
             *  Will Be Kitchen As Key Van And Fridge Be As List IF Facilities Like ["fridge ", "Van"] As Value
             */
            var groupedFacilities = facilityPairs

                .GroupBy(pair => pair.Key)

                .ToDictionary(

                group => group.Key,

                group => group.Select(pair => pair.Value).ToList()

                );



            #endregion

            #region SleepPlaces here is extra bugs 

            /*
             * 1- make a list of sleep places 
             * 2- make every details related to this sleep place to it 
             * 3- elso map the student details and every student related to this sleep place to this 
             * 4- check if the booking a room is available or booking a bed is available 
             * 5- 
             */

            // make a list of sleep places
            var sleepPlaces = new List<SleepPlace>();

            bool isRequestApartAvailable = true;

            foreach (var room in apartment.Rooms ?? new List<Room>())
            {
                var beds = room.Beds ?? new List<Bed>();
                var bookedBeds = beds.Where(b => !b.IsAvailable).ToList();
                var numPeople = bookedBeds.Count;
                var totalBeds = beds.Count;
                var numBedNotBooked = beds.Count(b => b.IsAvailable);

                var students = new List<StudentDTO>();

                var bookRoom = _bookRoomRepo.Get(x => x.RoomId == room.Id && x.ApartmentId == room.ApartmentId&& x.Status == BookingStatus.Approved).FirstOrDefault();


                if (bookRoom == null)
                {
                    foreach (var bed in bookedBeds)
                    {
                        var booking = _bookBedRepo.Get(bk => bk.BedId == bed.Id).FirstOrDefault();
                        if (booking != null)
                        {
                            var student = await _studentRepo.GetByIDAsync(booking.StudentId);
                            if (student != null)
                            {
                                isRequestApartAvailable = false;
                                students.Add(new StudentDTO
                                {
                                    Collage = student.Faculty,
                                    Level = student.AcademicYear,
                                    // To Solve
                                    Location = student.Governomet
                                });
                            }
                        }
                    }
                    var sleepPlace = new SleepPlace
                    {
                        RoomId = room.Id,
                        ImageRoomUrl = room.Image,
                        PricePerBed = beds.FirstOrDefault()?.Price ?? 0,
                        IsFull = totalBeds == numPeople,
                        NumOfBeds = totalBeds,
                        NumBedNotBooked = numBedNotBooked,
                        BedRequestAvailable = numBedNotBooked > 0,
                        RoomRequestAvailable = totalBeds == numBedNotBooked,
                        StudentDTOs = students
                    };

                    sleepPlaces.Add(sleepPlace);
                }

                else
                {

                    var student = await _studentRepo.GetByIDAsync(bookRoom.StudentId);
                    if (student != null)
                    {
                        isRequestApartAvailable = false;
                        students.Add(new StudentDTO
                        {
                            Collage = student.Faculty,
                            Level = student.AcademicYear,
                            // To Solve
                            Location = student.Governomet
                        });
                    }

                    var sleepPlace = new SleepPlace
                    {
                        RoomId = room.Id,
                        ImageRoomUrl = room.Image,
                        PricePerBed = beds.FirstOrDefault()?.Price ?? 0,
                        IsFull =true,
                        NumOfBeds = totalBeds,
                        NumBedNotBooked = 0,
                        BedRequestAvailable = false,
                        RoomRequestAvailable = false,
                        StudentDTOs = students
                    };

                    sleepPlaces.Add(sleepPlace);
                }


            }

            // Check If Apartment request Available 
            apartmentDTO.BookEntireApartment = isRequestApartAvailable;

            detailsApartment.SleepPlaces = sleepPlaces;
            #endregion
            detailsApartment.ApartmentDTO = apartmentDTO;
            #region CheckNull

            if (roomsDTOs != null)
            {
                detailsApartment.Rooms = roomsDTOs;
            }
            if (imageDTOS != null)
            {
                detailsApartment.Images = imageDTOS;
            }
            if (groupedFacilities != null)
            {
                detailsApartment.CategoryWithFacilities = groupedFacilities;
            }

            #endregion

            return RequestResult<ApartmentDetailsDTO>.Success(detailsApartment, "The Apartment With All Details");
        }
    }
}