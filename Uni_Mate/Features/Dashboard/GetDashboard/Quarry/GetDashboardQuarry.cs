using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Common.Views;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.BookingManagement;
using Uni_Mate.Models.Comment_Review;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.Dashboard.GetDashboard.Quarry
{
    public record GetDashboardQuarry() : IRequest<RequestResult<GetDashboardDTO>>;

    public class GetDashboardHandler : IRequestHandler<GetDashboardQuarry, RequestResult<GetDashboardDTO>>
    {
        private readonly IRepository<Apartment> _apartmentRepo;
        private readonly IRepositoryIdentity<Student> _studentRepo;
        private readonly IRepositoryIdentity<Owner> _ownerRepo;
        private readonly IRepository<Booking> _bookingRepo;
        private readonly IRepository<Comment> _commentRepo;

        public GetDashboardHandler(
            IRepository<Apartment> apartmentRepo,
            IRepositoryIdentity<Student> studentRepo,
            IRepositoryIdentity<Owner> ownerRepo,
            IRepository<Booking> bookingRepo,
            IRepository<Comment> commentRepo)
        {
            _apartmentRepo = apartmentRepo;
            _studentRepo = studentRepo;
            _ownerRepo = ownerRepo;
            _bookingRepo = bookingRepo;
            _commentRepo = commentRepo;
        }

        public async Task<RequestResult<GetDashboardDTO>> Handle(GetDashboardQuarry request, CancellationToken cancellationToken)
        {
            var totalApartments = await _apartmentRepo.CountAsync();
            var availableApartments = await _apartmentRepo.CountAsync(x => x.IsAvailable);
            var bookedApartments = totalApartments - availableApartments;

            var totalStudents = await _studentRepo.CountAsync();
            var studentsBooked = await _bookingRepo.CountAsync(b => b.Status == BookingStatus.Approved);
            var studentsNotBooked = totalStudents - studentsBooked;

            var totalOwners = await _ownerRepo.CountAsync();
            var totalBookings = await _bookingRepo.CountAsync();

            var approvedBookings = await _bookingRepo.CountAsync(b => b.Status == BookingStatus.Approved);
            var rejectedBookings = await _bookingRepo.CountAsync(b => b.Status == BookingStatus.Rejected);
            var pendingBookings = await _bookingRepo.CountAsync(b => b.Status == BookingStatus.Pending);

            var totalComments = await _commentRepo.CountAsync();

            var dto = new GetDashboardDTO
            {
                TotalApartments = totalApartments,
                BookedApartments = bookedApartments,
                AvailableApartments = availableApartments,
                TotalStudents = totalStudents,
                StudentsBooked = studentsBooked,
                StudentsNotBooked = studentsNotBooked,
                TotalOwners = totalOwners,
                TotalBookings = totalBookings,
                ApprovedBookings = approvedBookings,
                RejectedBookings = rejectedBookings,
                PendingBookings = pendingBookings,
                TotalComments = totalComments
            };

            return RequestResult<GetDashboardDTO>.Success(dto, "Dashboard data loaded successfully");
        }
    }
}
