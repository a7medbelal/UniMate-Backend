using MediatR;
using System.Security.Claims;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.StudentManager.LoginInfoSave.Command
{
    public record LoginInfoSaveCommand(string? Email, string? National_Id, string FrontImageUrl, string BackImageUrl) : IRequest<RequestResult<bool>>;

    public class LoginInfoSaveHandler : BaseWithoutRepositoryRequestHandler<LoginInfoSaveCommand, RequestResult<bool>, Student>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginInfoSaveHandler(BaseWithoutRepositoryRequestHandlerParameters<Student> parameters,IHttpContextAccessor httpContextAccessor): base(parameters)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task<RequestResult<bool>> Handle(LoginInfoSaveCommand request, CancellationToken cancellationToken)
        {
           // var userId = _userInfo.ID;
           var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.UserNotFound, "User not found");
            }

            var student = await _repositoryIdentity.GetByIDAsync(userId);
            if (student == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.UserNotFound, "User not found");
            }

            student.Email = request.Email;
            student.National_Id = request.National_Id;
            student.FrontPersonalImage = request.FrontImageUrl;
            student.BackPersonalImage = request.BackImageUrl;

            var result = await _userManager.UpdateAsync(student);
            if (result.Succeeded)
            {
                return RequestResult<bool>.Success(true,"Updated Successfuly");
            }
            else
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return RequestResult<bool>.Failure(ErrorCode.UpdateFailed, errorMessage);
            }
        }
    }
}
