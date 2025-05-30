using MediatR;
using Uni_Mate.Models.BookingManagement;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Features.BookingManagement.GetStudenetBookingRequests.Queries;

namespace Uni_Mate.Features.BookingManagement.GetStudenetBookingRequests.Queries
{
	public record GetStudentBookingHistoryQuery() : IRequest<RequestResult<List<GetStudentBookingHistoryDTO>>>;

	public class GetStudentBookingHistoryHandler
		: BaseRequestHandler<GetStudentBookingHistoryQuery, RequestResult<List<GetStudentBookingHistoryDTO>>, Booking>
	{
		public GetStudentBookingHistoryHandler(BaseRequestHandlerParameter<Booking> parameters)
			: base(parameters)
		{
		}

		public override async Task<RequestResult<List<GetStudentBookingHistoryDTO>>> Handle(GetStudentBookingHistoryQuery request, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(_userInfo?.ID))
			{
				return RequestResult<List<GetStudentBookingHistoryDTO>>.Failure(ErrorCode.NotFound, "User not found");
			}

			var requests = await _repository.GetAll()
				.Where(b => b.StudentId == _userInfo.ID)
				.OrderByDescending(b => b.CreatedDate)
				.Select(b => new GetStudentBookingHistoryDTO
				{
					RequestId = b.Id,
					ApartmentName = b.Apartment.Location + " - "
						+ b.Apartment.Gender.ToString() + " - "
						+ b.Apartment.NumberOfRooms + " غرف - "
						+ b.Apartment.Capecity + " - أشخاص "
						+ b.Apartment.Floor + "الدور الـ",
					RequestDate = b.CreatedDate,
					Status = b.Status.ToString(),
				    booking = b.Type.ToString(),
				})
				.ToListAsync(cancellationToken);

			return RequestResult<List<GetStudentBookingHistoryDTO>>.Success(requests, "Booking history retrieved successfully");
		}
	}
}