namespace Uni_Mate.Features.BookingManagement.GetStudenetBookingRequests.Queries
{
	public class GetStudentBookingHistoryDTO
	{
		public int RequestId { get; set; } 
		public string ApartmentName { get; set; } = string.Empty;
		public DateTime RequestDate { get; set; }
		public string Status { get; set; } = string.Empty;          
	}
}
