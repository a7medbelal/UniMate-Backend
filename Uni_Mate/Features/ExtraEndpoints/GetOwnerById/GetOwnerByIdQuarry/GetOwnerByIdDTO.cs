namespace Uni_Mate.Features.ExtraEndpoints.GetOwnerById.GetOwnerByIdQuarry
{
    public class GetOwnerByIdDTO
    {
        public string Id { get; set; } = string.Empty;
        public string? FName { get; set; } = string.Empty;
        public string? LName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Governomet { get; set; }
    }
}
