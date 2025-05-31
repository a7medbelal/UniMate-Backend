namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentRoomDisplay.Queries
{
	public class UpdateApartmentRoomDisplayDTO
	{
		public string? Description { get; set; }
		public string? Image { get; set; }
		public bool IsAirConditioned { get; set; }

		public int BedCount { get; set; }

		public decimal BedPrice { get; set; }
		public int Capacity { get; set; }
	}
}