using MediatR;
using System.Security.Claims;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Domain.FileServices;
using Uni_Mate.Models.ApartmentManagement.Enum;
using Uni_Mate.Models.UserManagment;


namespace Uni_Mate.Features.StudentManager.UpdateAcademicInfoSave.Command
{
    public record AcademicInfoSaveCommand(string? University, string? Faculty, string? AcademicYear, string? Department, IFormFile? KarnihImage) : IRequest<RequestResult<bool>>;

    public class AcademicInfoSaveHandler : BaseWithoutRepositoryRequestHandler<AcademicInfoSaveCommand, RequestResult<bool>, Student>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileService _fileService;
        public  AcademicInfoSaveHandler(BaseWithoutRepositoryRequestHandlerParameters<Student> parameters,IHttpContextAccessor httpContextAccessor,IFileService fileService) : base(parameters)
        {
            _httpContextAccessor =httpContextAccessor;
            _fileService = fileService;
        }
        public override async Task<RequestResult<bool>> Handle(AcademicInfoSaveCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "User Not Found");

            var student = _repositoryIdentity.GetInclude(s => s.Id == userId, new[] { "Images" });
            if (student == null)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Student not found");

            student.University = request.University;
            student.Faculty = request.Faculty;
            student.AcademicYear = request.AcademicYear;
            student.Department = request.Department;

            string im = "ziad";
            if (request.KarnihImage != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(request.KarnihImage);
                var existingImage = student.Images?.FirstOrDefault(i => i.ImageType == ImageType.KarnihImage);

                im = imageUrl;
                if (existingImage != null)
                    existingImage.ImageUrl = imageUrl;
                else
                {
                    //student.Images ??= new Lis<Image>();
                    //student.Images.Add(new Image
                    //{
                    //    ImageType = ImageType.KarnihImage,
                    //    ImageUrl = imageUrl
                    //    student.Images ??= new List<Image>();
                    //    student.Images.Add(new Image
                    //    {
                    //        ImageType = ImageType.KarnihImage,
                    //        ImageUrl = imageUrl,
                    //        StudentId = student.Id // Ensure the StudentId is set to associate the image with the student
                    //    });
                    //});
                }
            }

          //  await _unitOfWork.SaveChangesAsync();
            return RequestResult<bool>.Success(true, $"Academic Info Updated and this is the link {im}");
        }
    }

        
    

}
