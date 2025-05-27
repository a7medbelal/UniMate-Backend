using MediatR;
using System.Security.Claims;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Common.UploadPhotoCommand;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.OwnerManager.UpdateOwnerProfileSave.Command
{
	public record UpdateOwnerProfileSaveCommand(string? Fname, string? Lname, String?  adderess, string? BriefOverview , string?Governmnet, IFormFile ProfileImage ) : IRequest<RequestResult<bool>>;

	public class UpdateOwnerProfileSaveCommandHandler : BaseWithoutRepositoryRequestHandler<UpdateOwnerProfileSaveCommand, RequestResult<bool>, Owner>
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UpdateOwnerProfileSaveCommandHandler(BaseWithoutRepositoryRequestHandlerParameters<Owner> parameters, IHttpContextAccessor httpContextAccessor) : base(parameters)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public override async Task<RequestResult<bool>> Handle(UpdateOwnerProfileSaveCommand request, CancellationToken cancellationToken)
		{
			var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
			{
				return RequestResult<bool>.Failure(ErrorCode.NotFound, "User not found");
			}

			Owner owner = await _repositoryIdentity.GetByIDAsync(userId);
			if (owner == null)
			{
				return RequestResult<bool>.Failure(ErrorCode.NotFound, "Owner not found");
			}

		     RequestResult<string> frontImageUrl;	
              string image = string.Empty;	
            if (string.IsNullOrEmpty(owner.Image))
			{
				frontImageUrl = await _mediator.Send(new UploadPhotoCommand(request.ProfileImage));
			    image = frontImageUrl.data; 
			}

            owner.Fname = request.Fname;
			owner.Lname = request.Lname;
			owner.Address = request.adderess ;
			owner.BriefOverView = request.BriefOverview;
			owner.Governomet = request.Governmnet;
			owner.Image = image;



  		   await _repositoryIdentity.UpdateAsync(owner);

	

			return RequestResult<bool>.Success(true, "Owner profile updated successfully");
		}
	}
}