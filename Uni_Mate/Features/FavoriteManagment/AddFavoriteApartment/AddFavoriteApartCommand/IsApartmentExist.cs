using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.FavoriteManagment.AddFavoriteApartment.AddFavoriteApartCommand
{
    public record IsApartmentExist(int id) : IRequest<RequestResult<bool>>;

    public class IsApartmentExistHandler :BaseRequestHandler<IsApartmentExist, RequestResult<bool>,Apartment>
    {
        public IsApartmentExistHandler(BaseRequestHandlerParameter<Apartment> parameters) : base(parameters)
        {
        }

        public override async Task<RequestResult<bool>> Handle(IsApartmentExist request, CancellationToken cancellationToken)
        {
            if(request.id <= 0)
            {
                return RequestResult<bool>.Failure(ErrorCode.InvalidData, "Invalid Apartment ID");
            }

            var apartment =  await _repository.GetByIDAsync(request.id);

            if (apartment == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Apartment not found");
            }
            return RequestResult<bool>.Success(true, "Apartment Exists");
        }
    }
}
