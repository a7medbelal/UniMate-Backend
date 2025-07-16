using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Helper;
using Uni_Mate.Common.Views;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Features.Common.ApartmentManagement.ApartmerntDTO;
using Uni_Mate.Models.ApartmentManagement;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Uni_Mate.Features.FavoriteManagment.GetFavoriteAparts.Quarry
{
    public record GetFavoritesQuarry() : IRequest<RequestResult<List<GetApartmentDTO>>>; 

    public class GetFavoritesQuarryHandler : BaseRequestHandler<GetFavoritesQuarry, RequestResult<List<GetApartmentDTO>>,FavoriteApartment>
    {
        
        IRepository<Apartment> _AparmentRepository;
        IRepository<Image> _AparmentImageRepository;
        public GetFavoritesQuarryHandler(BaseRequestHandlerParameter<FavoriteApartment> parameter , IRepository<Apartment> repository, IRepository<Image> aparmentImageRepository) : base(parameter)
        {
            _AparmentRepository = repository;
            _AparmentImageRepository = aparmentImageRepository;
        }

        public override async Task<RequestResult<List<GetApartmentDTO>>> Handle(GetFavoritesQuarry request, CancellationToken cancellationToken)
        {
            var userId = _userInfo.ID;
            var favorites = await _repository
             .Get(f => f.UserId == userId).
             Select(c=> c.ApartmentId).ToListAsync(); 




            var getApartment = await _AparmentRepository
             .Get(c=>favorites.Contains(c.Id))
             .Select(x => new GetApartmentDTO
             {
                 Id = x.Id,
                 Location = x.Location,
                 Gender = x.Gender.ToString(),
                 Floor = x.Floor ?? "unknown",
                 DetailedAddress = x.DescripeLocation ?? "unknown",
                 OwnerName = (x.Owner != null ? x.Owner.Fname + " " + x.Owner.Lname : string.Empty),
                 NumberOfRooms = x.NumberOfRooms,
                 Price = x.Price,
                 Favourite = favorites.Contains(x.Id)

             }).ToListAsync();

            var apartmentIds = getApartment.Select( a=>a.Id ).ToList();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var imageTask = await _AparmentImageRepository.Get(i => apartmentIds.Contains(i.ApartmentId))
                .Select(i => new { i.ApartmentId, i.ImageUrl })
                .ToListAsync();

            Console.WriteLine($"Second Query : {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Stop();


            var imagesGrouped = imageTask
                .GroupBy(i => i.ApartmentId)
                .ToDictionary(g => g.Key, g => g.Select(i => i.ImageUrl).ToList());

            foreach (var apartment in getApartment)
            {
                apartment.Images = imagesGrouped.TryGetValue(apartment.Id, out var imgs)
                    ? imgs
                    : new List<string>();
            }


            return RequestResult<List<GetApartmentDTO>>.Success(getApartment, "Favorites loaded");
        }
    }
}
