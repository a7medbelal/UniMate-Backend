namespace Uni_Mate.Features.StudentManager.GetStudent.Quarry
{
    public class GetStudentDTO
    {
        public string? FullName { get; set; }    
        public string? Image { get; set; }
        public string? National_Id { get; set; }
        public string? University { get; set; }
        public string? Faculty { get; set; }
        public ICollection<string>? Phones { get; set; }
        public string? BriefOverView { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
    }
}
