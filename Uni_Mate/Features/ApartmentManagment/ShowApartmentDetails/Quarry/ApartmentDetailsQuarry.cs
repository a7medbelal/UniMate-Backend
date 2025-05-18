using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.ShowApartmentDetails.ApartmentDTO;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.GeneralEnum;

namespace Uni_Mate.Features.ApartmentManagment.ShowApartmentDetails.Quarry
{
    public record ApartmentDetailsQuarry(int id) : IRequest<RequestResult<ApartmentDetailsDTO>>;

    public class ApartmentDetailsHandler : BaseRequestHandler<ApartmentDetailsQuarry, RequestResult<ApartmentDetailsDTO>, Apartment>
    {

        public ApartmentDetailsHandler(BaseRequestHandlerParameter<Apartment> parameters) : base(parameters)
        {
        }

        public override async Task<RequestResult<ApartmentDetailsDTO>> Handle(ApartmentDetailsQuarry request, CancellationToken cancellationToken)
        {
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
                Location = apartment.Location,
                DescripeLocation = apartment.DescripeLocation,
                IsAvailable = apartment.IsAvailable,
                kind = apartment.Gender == Gender.Male ? "Male" : apartment.Gender == Gender.Female ? "Female" : "Null",
                Price = apartment.Rooms?.Sum(x => x.Price) ?? 0,
                RoomCount = apartment.Rooms?.Count() ?? 0,
                BedRoomCount = apartment.Rooms?.SelectMany(r => r.Beds ?? Enumerable.Empty<Bed>()).Count() ?? 0,
            };
            #endregion


            detailsApartment.ApartmentDTO = apartmentDTO;

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

            #region CheckNull

            if (roomsDTOs != null)
            {
                detailsApartment.Rooms = roomsDTOs;
            }
            if (imageDTOS != null)
            {
                detailsApartment.Images = imageDTOS;
            }
            if(groupedFacilities != null)
            {
                detailsApartment.CategoryWithFacilities = groupedFacilities;
            }

            #endregion

            return RequestResult<ApartmentDetailsDTO>.Success(detailsApartment, "The Apartment With All Details");
        }
    }
}
