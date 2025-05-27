namespace Uni_Mate.Features.BookingManagement.GetOwnerBookingRequests.Queries
{
	public class GetOwnerBookingRequestsDTO
	{
		public Guid RequestId { get; set; }
		public string ApartmentName { get; set; }
		public string StudentName { get; set; }
		public double Rating { get; set; }
		public DateTime RequestDate { get; set; }
		public string RequestStatus { get; set; }
	}
}
