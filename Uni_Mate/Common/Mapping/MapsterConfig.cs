using Uni_Mate.Features.StudentManager.GetStudent.Quarry;
using Uni_Mate.Models.UserManagment;
using Mapster;
using Uni_Mate.Features.StudentManager.UpdateProfileDisplay.Quarry;
using Uni_Mate.Features.StudentManager.UpdateProfileSave;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.CategoryWithFaciltyCommand;
using Uni_Mate.Features.OwnerManager.GetOwner.Queries;
namespace Uni_Mate.Common.Mapping
{
    public static class MapsterConfig
    {
        public static TypeAdapterConfig Configure()
        {
            // Create a new TypeAdapterConfig instance
            var config = new TypeAdapterConfig();

            // Add mapping configuration for Student to GetStudentDTO
            #region mapping from Student to GetStudentDTO
            config.NewConfig<Student, GetStudentDTO>()
                .Map(dest => dest.FullName, src => src.Fname + " " + src.Lname)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.Address, src => src.Address)
                .Map(dest => dest.BriefOverView, src => src.BriefOverView)
                .Map(dest => dest.Faculty, src => src.Faculty)
                .Map(dest => dest.Image, src => src.Image)
                .Map(dest => dest.National_Id, src => src.National_Id)
                .Map(dest => dest.Phones, src => src.Phones.Select(phone => phone.PhoneNumber).ToList());
            #endregion


            #region mapping from Student to UpdateProfileDisplayDTO
            config.NewConfig<Student, UpdateProfileDisplayDTO>()
                .Map(dest => dest.FirstName, src => src.Fname)
                .Map(dest => dest.LastName, src => src.Lname)
                .Map(dest => dest.Governorate, src => src.Governorate)
                .Map(dest => dest.Address, src => src.Address)
                .Map(dest => dest.BriefOverView, src => src.BriefOverView);
            #endregion

            #region mapping From UpdateProfileSaveVM to Student
            config.NewConfig<UpdateProfileSaveVM, Student>()
                .Map(dest => dest.Fname, src => src.FirstName)
                .Map(dest => dest.Lname, src => src.LastName)
                .Map(dest => dest.Governorate, src => src.Governorate)
                .Map(dest => dest.Address, src => src.Address)
                .Map(dest => dest.BriefOverView, src => src.BriefOverView);
			#endregion


			#region mapping from Owner to GetOwnerDTO
			config.NewConfig<Owner, GetOwnerDTO>()
				.Map(dest => dest.Username, src => src.Fname + " " + src.Lname)
				.Map(dest => dest.Image, src => src.Image)
				.Map(dest => dest.Phones, src => src.Phones.Select(phone => phone.PhoneNumber).ToList())
				.Map(dest => dest.Email, src => src.Email)
				.Map(dest => dest.BriefOverView, src => src.BriefOverView);
			#endregion


			config.NewConfig<List<CategoryFacilityViewModel>, List<ApartmentFacility>>()
                 .MapWith((categories) => categories.
                    SelectMany(c => c.facilities).
                    Where(c => c.IsSelected == true).
                    Select(c => new ApartmentFacility { FacilityId = c.FacilityId }).ToList());

            return config;
        }
    }
}
