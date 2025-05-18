using MediatR;
using System.Security.Claims;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.FavoriteManagment.AddFavoriteApartment.AddFavoriteApartCommand
{
    public record AddFavoriteCommand(int id) : IRequest<RequestResult<bool>>;

    public class AddFavoriteHandler: BaseRequestHandler<AddFavoriteCommand , RequestResult<bool>, FavoriteApartment>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AddFavoriteHandler(BaseRequestHandlerParameter<FavoriteApartment> parameter, IHttpContextAccessor httpContextAccessor) : base(parameter)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task<RequestResult<bool>> Handle(AddFavoriteCommand request, CancellationToken cancelationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "User not found");
            }

            //To Check The Existence
            var isApartExist = await _mediator.Send(new IsApartmentExist(request.id));
            if(isApartExist.isSuccess == false)
            {
                return RequestResult<bool>.Failure(isApartExist.errorCode, isApartExist.message); 
            }

            var favoriteApartment = new FavoriteApartment()
            {
                StudentId = userId,
                ApartmentId = request.id,
            };

            // Maybe The Favorite Was Added Before 
            var isFavoriteExist =await _repository.AnyAsync(f => f.StudentId == userId && f.ApartmentId == request.id);
            if (isFavoriteExist)
            {
            return RequestResult<bool>.Success(true, "Favorite Was Added Before");
            }
            await _repository.AddAsync(favoriteApartment);

            return RequestResult<bool>.Success(true, "Favorite Added Successfully");
        }
    }
}
