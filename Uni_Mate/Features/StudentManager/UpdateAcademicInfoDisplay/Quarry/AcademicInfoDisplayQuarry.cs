using MediatR;
using System.Security.Claims;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement.Enum;
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

            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RequestResult<AcademicInfoDisplayDTO>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.NotFound, "The Student Not Found");
            }

            Student student = _repositoryIdentity.GetInclude(st => st.Id == userId, new string[] {"Images"});

            var image = student.Images?.FirstOrDefault(i => i.ImageType == ImageType.KarnihImage);
            //(string? University, string? Faculty, string? AcademicYear, string Apartment, string? KarnihImage
            AcademicInfoDisplayDTO academicInfoDisplay = new AcademicInfoDisplayDTO(
                student.University,
                student.Faculty,
                student.AcademicYear,
                student.Department,
                image?.ImageUrl
            );

            return RequestResult<AcademicInfoDisplayDTO>.Success(academicInfoDisplay, "Sent Succefully");
        }
    }
}
