using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.FavoriteManagment.AddFavoriteApartment.AddFavoriteApartCommand;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.FavoriteManagment.DelFavoriteApartment.DelFavoriteApartmentCommand
{
    public record DelFavoriteApartCommand(int id) : IRequest<RequestResult<bool>>;

    public class DelFavoriteApartHandler : BaseRequestHandler<DelFavoriteApartCommand, RequestResult<bool>, FavoriteApartment>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DelFavoriteApartHandler(BaseRequestHandlerParameter<FavoriteApartment> parameter, IHttpContextAccessor httpContextAccessor) : base(parameter)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task<RequestResult<bool>> Handle(DelFavoriteApartCommand request, CancellationToken cancelationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "User not found");
            }

            //To Check The Existence
            // This Command In AddFavoriteApartCommand 
            var isApartExist = await _mediator.Send(new IsApartmentExist(request.id));
            if (isApartExist.isSuccess == false)
            {
                return RequestResult<bool>.Failure(isApartExist.errorCode, isApartExist.message);
            }

            // Maybe The Favorite Was Added Before 
            var isFavoriteExist = await _repository.Get(f => f.StudentId == userId && f.ApartmentId == request.id).FirstOrDefaultAsync();
            if (isFavoriteExist == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "The Apartment Not Found In Table Favorite");
            }

            await _repository.HardDelete(isFavoriteExist);

            return RequestResult<bool>.Success(true, "The Apartment Deleted From Favorite");
        }
    }
}
