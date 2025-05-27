using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.BookingManagement;
using Uni_Mate.Domain.Repository;

namespace Uni_Mate.Features.BookingManagement.GetOwnerBookingRequests.Queries
{
	public record GetOwnerBookingRequestsQuery() : IRequest<RequestResult<List<GetOwnerBookingRequestsDTO>>>;

	public class GetOwnerBookingRequestsHandler : BaseRequestHandler<GetOwnerBookingRequestsQuery, RequestResult<List<GetOwnerBookingRequestsDTO>>, Booking>
	{
		public GetOwnerBookingRequestsHandler(BaseRequestHandlerParameter<Booking> parameters)
			: base(parameters)
		{
		}

		public override async Task<RequestResult<List<GetOwnerBookingRequestsDTO>>> Handle(GetOwnerBookingRequestsQuery request, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(_userInfo?.ID))
			{
				return RequestResult<List<GetOwnerBookingRequestsDTO>>.Failure(ErrorCode.NotFound, "User not found");
			}
			
			var requests = await _repository.GetAll()
			.Where(b => b.Apartment.OwnerID == _userInfo.ID && b.Status == BookingStatus.Pending) 
			.OrderByDescending(b => b.CreatedDate)
			.Select(b => new GetOwnerBookingRequestsDTO
				{
					RequestId = b.Id,
					ApartmentName = b.Apartment.Location + " - "
						+ b.Apartment.Gender.ToString() + " - "
						+ b.Apartment.NumberOfRooms + " غرف - "
						+ b.Apartment.Capecity + " - أشخاص "
						+ b.Apartment.Floor + "الدور الـ",
					StudentName = b.Student.Fname + " " + b.Student.Lname,
					RequestStatus = b.Status.ToString(),
					RequestDate = b.CreatedDate
				})
				.ToListAsync(cancellationToken);

			return RequestResult<List<GetOwnerBookingRequestsDTO>>.Success(requests, "Requests retrieved successfully");
		}
	}
}