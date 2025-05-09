using MediatR;
using System.Security.Claims;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.StudentManager.UpdateAcademicInfoDisplay.Quarry
{
    public record AcademicInfoDisplayQuarry() : IRequest<RequestResult<AcademicInfoDisplayDTO>>;

    public class AcademicInfoDisplayQuarryHandler : BaseWithoutRepositoryRequestHandler<AcademicInfoDisplayQuarry, RequestResult<AcademicInfoDisplayDTO>, Student>
    {
       private readonly IHttpContextAccessor _httpContextAccessor;
        public AcademicInfoDisplayQuarryHandler(BaseWithoutRepositoryRequestHandlerParameters<Student> parameters, IHttpContextAccessor httpContextAccessor) : base(parameters)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task<RequestResult<AcademicInfoDisplayDTO>> Handle(AcademicInfoDisplayQuarry request, CancellationToken cancellationToken)
        {

            //var userId = _userInfo.ID;
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RequestResult<AcademicInfoDisplayDTO>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.NotFound, "The Student Not Found");
            }

            Student student =await _repositoryIdentity.GetByIDAsync(userId);
            //(string? University, string? Faculty, string? AcademicYear, string Apartment, string? KarnihImage
            if (student != null)
            {
                AcademicInfoDisplayDTO academicInfoDisplay = new AcademicInfoDisplayDTO(
                    student.University,
                    student.Faculty,
                    student.AcademicYear,
                    student.Department,
                    student.KarnihImage
                );
                return RequestResult<AcademicInfoDisplayDTO>.Success(academicInfoDisplay, "Sent Succefully");
            }
            else
            {
                return RequestResult<AcademicInfoDisplayDTO>.Failure(ErrorCode.NotFound, "There Is No Studen ");
            }
        }
    }
}
