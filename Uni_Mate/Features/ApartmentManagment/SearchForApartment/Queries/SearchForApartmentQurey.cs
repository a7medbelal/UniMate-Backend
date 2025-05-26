
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Buffers;
using System.Linq.Expressions;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Helper;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.AddRoomWithBedsCommands;
using Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.CategoryWithFaciltyCommand;
using Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.CategoryWithFaciltyCommands;
using Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.CreateApartmentInfoCommand;
using Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.CreateFullApartmentOrcasterartor;
//using Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.UploadApartmentCommand;
using Uni_Mate.Models;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.GeneralEnum;

namespace Uni_Mate.Features.ApartmentManagment.SearchForApartment.Queries
{
    public record SearchForApartmentQurey(string? Keyword ,  decimal? FromPrice ,decimal? ToPrice , int? Capacity, string? Location , Gender? Gender , int PageSize,int PageNumber, SortOption? SortBy = SortOption.None) : IRequest<RequestResult<Pagination<GetAparmtmentFilterDTO>>>;

    public class SearchForApartmentQureyHandler : BaseRequestHandler<SearchForApartmentQurey, RequestResult<Pagination<GetAparmtmentFilterDTO>>, Apartment>
    {
        public SearchForApartmentQureyHandler(BaseRequestHandlerParameter<Apartment> parameters)
            : base(parameters) { }

        public override async Task<RequestResult<Pagination<GetAparmtmentFilterDTO>>> Handle(SearchForApartmentQurey request, CancellationToken cancellationToken)
        {
            // make the perdicate for filter 
            var builder = Perdicate(request);
            var apartments = _repository.GetAll().Where(builder);

            // if he will sort the data
            if (request.SortBy != SortOption.None)
            {
                apartments = request.SortBy switch
                {
                    SortOption.PriceLowToHigh => apartments.OrderBy(x => x.Rooms.ToList().Sum(c => c.Price)),
                    SortOption.PriceHighToLow => apartments.OrderByDescending(x => x.Rooms.ToList().Sum(c => c.Price)),
                    SortOption.NewestFirst => apartments.OrderByDescending(x => x.CreatedDate),
                    SortOption.OldestFirst => apartments.OrderBy(x => x.CreatedDate),
                    _ => apartments
                };
            }

            // the data that i need from the database
            var apartmentsList = apartments.Select(c => new GetAparmtmentFilterDTO
            {
                Gender = c.Gender.ToString(),
                Location = c.Location,
                Floor = c.Floor ??  "unknown" ,
                OwnerName = c.Owner != null ? (c.Owner.Fname + " " + c.Owner.Lname) : "Unknown",
                NumberOfRooms = c.NumberOfRooms,
                Capecity = c.Capecity,
                Price =c.Rooms.Sum(c => c.Price),
                Images = c.Images != null
               ? c.Images.Select(i => i.ImageUrl).ToList()
                : new List<string>()
             });


            // if the data is empty return that is empty not return false 
            if (!apartments.Any())
            {
                var empty = new Pagination<GetAparmtmentFilterDTO>(new List<GetAparmtmentFilterDTO>(), 0, request.PageNumber, request.PageSize);
                return RequestResult<Pagination<GetAparmtmentFilterDTO>>.Success(empty);
            }

            //here i will get the data from the database the data that i need
            var result = await Pagination<GetAparmtmentFilterDTO>.ToPagedList(apartmentsList, request.PageNumber, request.PageSize);


            return RequestResult<Pagination<GetAparmtmentFilterDTO>>.Success(result);

        }

        // this method is used to make the perdicate for the filter
        public Expression<Func<Apartment, bool>> Perdicate(SearchForApartmentQurey request)
        {
            var predicate = PredicateBuilder.New<Apartment>(true);

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                var term = $"%{request.Keyword}%";

                predicate = predicate.And(x =>
                EF.Functions.Like(x.Description, term) ||
                EF.Functions.Like(x.DescripeLocation, term) ||
                EF.Functions.Like(x.Location, term));
            }


            if (request.FromPrice.HasValue)
            {
                predicate = predicate.And(x => x.Rooms.Sum(c => c.Price) >= request.FromPrice);
            }

            if (request.ToPrice.HasValue)
                predicate = predicate.And(x => x.Rooms.Sum(c => c.Price) <= request.ToPrice);

            if (request.Capacity.HasValue)
                predicate = predicate.And(x => x.Capecity == request.Capacity);

            if (!string.IsNullOrWhiteSpace(request.Location))
                predicate = predicate.And(x => x.Location == request.Location);

            if (request.Gender is not null)
                predicate = predicate.And(x => x.Gender == request.Gender);

            return predicate;

        }

    }
    }

