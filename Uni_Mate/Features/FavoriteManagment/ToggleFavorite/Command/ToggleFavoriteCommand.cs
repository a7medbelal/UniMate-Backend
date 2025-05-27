using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.FavoriteManagment.ToggleFavorite.Quarry;
using Uni_Mate.Models.ApartmentManagement;


namespace Uni_Mate.Features.FavoriteManagment.ToggleFavorite.Command
{
    public record ToggleFavoriteCommand(int id) : IRequest<RequestResult<bool>>;

    public class ToggleFavoriteCommandHandler : BaseRequestHandler<ToggleFavoriteCommand, RequestResult<bool>, FavoriteApartment>
    {
        public ToggleFavoriteCommandHandler(BaseRequestHandlerParameter<FavoriteApartment> parameter) : base(parameter)
        {
        }

        public override async Task<RequestResult<bool>> Handle(ToggleFavoriteCommand request, CancellationToken cancelationToken)
        {
            // Check the user 
            var userId = _userInfo.ID;
            if (userId == "-1")
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "User Not Authorized");
            }

            //To Check The Existence
            // This Command In AddFavoriteApartCommand 
            var isApartExist = await _mediator.Send(new IsApartmentExist(request.id));
            if (isApartExist.isSuccess == false)
            {
                return RequestResult<bool>.Failure(isApartExist.errorCode, isApartExist.message);
            }

            // Check if the table favorite is exist 
            var isFavoriteExist =  _repository.Get(f => f.UserId == userId && f.ApartmentId == request.id).FirstOrDefault();
            if (isFavoriteExist != null)
            {
                //chagne the status 
                isFavoriteExist.Deleted = true;
                await _repository.DeleteAsync(isFavoriteExist);
                return RequestResult<bool>.Success(true, "The Apartment Deleted Succefully");
            }

            // if it is not found we just add it 
            var newFavorite = new FavoriteApartment
            {
                UserId = userId,
                ApartmentId = request.id,
                Deleted = false
            };
            await _repository.AddAsync(newFavorite);
            return RequestResult<bool>.Success(true, "The Apartment Added To Favorite");
        }
    }
}
