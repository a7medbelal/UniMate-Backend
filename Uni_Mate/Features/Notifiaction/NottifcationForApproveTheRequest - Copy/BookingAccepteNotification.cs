using MediatR;
using System.Drawing;
using Uni_Mate.Features.Common.SendEmailCommand;
using Uni_Mate.Models.BookingManagment;

namespace Uni_Mate.Features.Notifiaction.NottifcationForApproveTheRequest
{
    public record BookingAccepteNotification(string StudentEmail,
        string StudentName,
        string Bookingtype,
        DateTime AccepteDate

    ) : INotification;
    
    public class BookingAccepteNotificationHandler : INotificationHandler<BookingAccepteNotification>
    {
        private readonly IMediator _mediator;
        public BookingAccepteNotificationHandler(IMediator mediator ) {
          
            _mediator = mediator;
          
        }

        public async Task Handle(BookingAccepteNotification notification, CancellationToken cancellationToken)
        {
            // Logic to handle the booking created notification
            // For example, send an email or log the event


            var massege = $"""
                              Hi {notification.StudentName},

               Great news! Your booking request for the apartment "{notification.Bookingtype}" has been accepted by the owner.

               You can now proceed with the next steps to confirm your stay.

               If you have any questions, feel free to reach out to the apartment owner.

               Best regards,  
               UniMate Team
      
               """;

            var sendEmail = await _mediator.Send(new SendEmailQuery(notification.StudentEmail, "Notfiy the Owner ", massege));

        }
    }

}
