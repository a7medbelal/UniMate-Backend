using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.ExtraEndpoints.GetOwnerById.GetOwnerByIdQuarry
{
    public record GetOwnerByIdQuarry(string? OwnerId) : IRequest<RequestResult<GetOwnerByIdDTO>>;

    public class GetHandler : BaseWithoutRepositoryRequestHandler<GetOwnerByIdQuarry, RequestResult<GetOwnerByIdDTO>,Owner>
    {
        public GetHandler(BaseWithoutRepositoryRequestHandlerParameters<Owner> parameters) : base(parameters)
        {
        }

        public override async Task<RequestResult<GetOwnerByIdDTO>> Handle(GetOwnerByIdQuarry request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.OwnerId) || request.OwnerId =="-1")
            {
                return RequestResult<GetOwnerByIdDTO>.Failure(ErrorCode.InvalidData, "Owner ID cannot be null or empty.");
            }

            var owner = await _repositoryIdentity.GetByIDAsync(request.OwnerId);

            if (owner == null)
            {
                return RequestResult<GetOwnerByIdDTO>.Failure(ErrorCode.NotFound, "Owner not found.");
            }

            var ownerDto = new GetOwnerByIdDTO
            {
                Id = owner.Id,
                FName = owner.Fname,
                LName = owner.Lname,
                Email = owner.Email,
                PhoneNumber = owner.PhoneNumber,
                NationalId = owner.National_Id,
                Address = owner.Address,
                Governomet = owner.Governomet
            };
            return RequestResult<GetOwnerByIdDTO>.Success(ownerDto, "Owner retrieved successfully.");
        }
    }
}
