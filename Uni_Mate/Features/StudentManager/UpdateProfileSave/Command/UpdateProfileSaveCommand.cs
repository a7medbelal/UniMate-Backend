using MediatR;
using System.Security.Claims;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Common.UserProfilePicture.Commands;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.StudentManager.UpdateProfileSave.Command
{
    public record UpdateProfileSaveCommand(string? FirstName, string? LastName, string? Governorate, string? Address, string? BriefOverView , IFormFile ImageProfile) : IRequest<RequestResult<bool>>;

    public class UpdateProfileSaveCommandHandler : BaseWithoutRepositoryRequestHandler<UpdateProfileSaveCommand, RequestResult<bool>, User>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UpdateProfileSaveCommandHandler(BaseWithoutRepositoryRequestHandlerParameters<User> parameters, IHttpContextAccessor httpContextAccessor) : base(parameters)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public override async Task<RequestResult<bool>> Handle(UpdateProfileSaveCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "User not found");
            }
            var student = await _repositoryIdentity.GetByIDAsync(userId);
            if (student == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Student not found");
            }
            if (student == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Student not found");
            }
            var imageProfile = await _mediator.Send(new UploadProfilePictureCommand(request.ImageProfile)); 

            student.Fname = request.FirstName;
            student.Lname = request.LastName;
            student.Governomet = request.Governorate;
            student.Address = request.Address;
            student.BriefOverView = request.BriefOverView;
            student.Image = imageProfile.data; // Assuming UploadProfilePictureCommand returns the image path or URL
            try
            {
                await _repositoryIdentity.UpdateAsync(student);
            }
            catch (Exception ex)
            {
                return RequestResult<bool>.Failure(ErrorCode.InternalServerError, $"An error occurred while updating the student: {ex.Message}");
            }


            return RequestResult<bool>.Success(true, "Profile updated successfully");
        }
    }
}