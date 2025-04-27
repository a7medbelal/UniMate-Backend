using Uni_Mate.Features.StudentManager.GetStudent.Quarry;
using Uni_Mate.Models.UserManagment;
using Mapster;
namespace Uni_Mate.Common.Mapping
{
    public static class MapsterConfig
    {
        public static TypeAdapterConfig Configure()
        {
            // Create a new TypeAdapterConfig instance
            var config = new TypeAdapterConfig();

            // Add mapping configuration for Student to GetStudentDTO
            config.NewConfig<Student, GetStudentDTO>()
                .Map(dest => dest.FullName, src => src.Fname + " " + src.Lname)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.Address, src => src.Address)
                .Map(dest => dest.BriefOverView, src => src.BriefOverView)
                .Map(dest => dest.Faculty, src => src.Faculty)
                .Map(dest => dest.Image, src => src.Image)
                .Map(dest => dest.National_Id, src => src.National_Id)
                .Map(dest => dest.Phones, src => src.Phones.Select(phone => phone.PhoneNumber).ToList());

            return config;
        }
    }
}
