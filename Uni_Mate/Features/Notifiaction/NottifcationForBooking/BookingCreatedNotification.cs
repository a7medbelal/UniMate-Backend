using MediatR;
using System.Drawing;
using Uni_Mate.Features.Common.SendEmailCommand;
using Uni_Mate.Models.BookingManagment;

namespace Uni_Mate.Features.Notifiaction.NottifcationForBooking
{
    public record BookingAccepteNotification(int BookingId,
    string StudentEmail,
    BookingType BookingType,
    string OwnerEmail , 
    DateTime CreatedDate 
    ): INotification;
    
    public class BookingCreatedNotificationHandler : INotificationHandler<BookingAccepteNotification>
    {
        private readonly IMediator _mediator;
        public BookingCreatedNotificationHandler(IMediator mediator ) {
          
            _mediator = mediator;
          
        }

        public async Task Handle(BookingAccepteNotification notification, CancellationToken cancellationToken)
        {
            // Logic to handle the booking created notification
            // For example, send an email or log the event


            var massege = $"""
               Hello,

               Great news! A new student, **{notification.StudentEmail}**, has just booked your apartment: **"{notification.BookingId} and booked {notification.BookingType.ToString()}"**.

               🗓️ Booking Dates: {notification.CreatedDate:MMMM dd, yyyy}

               📧 Student Contact: {notification.StudentEmail}

               Please get in touch with the student if you need to coordinate further.

               Thanks for using UniMate!

               — The UniMate Team
               """;

            var sendEmail = await _mediator.Send(new SendEmailQuery(notification.OwnerEmail, "Notfiy the Owner ", massege));

        }
    }

}
