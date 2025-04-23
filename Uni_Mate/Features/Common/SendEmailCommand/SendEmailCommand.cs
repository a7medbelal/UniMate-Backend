using MailKit.Net.Smtp;
using MailKit.Security;
using MediatR;
using Microsoft.Extensions.Options;
using MimeKit;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.Common.SendEmailCommand
{
    public record SendEmailQuery(string email, string name, string confirmationLink) : IRequest<RequestResult<bool>>;

    public class SendEmailQueryHandler : BaseWithoutRepositoryRequestHandler<SendEmailQuery, RequestResult<bool>>
    {

        readonly MailSettings mailSettings;
        public SendEmailQueryHandler(BaseWithoutRepositoryRequestHandlerParameters parameters, IOptions<MailSettings> _mailSettings) : base(parameters)
        {
            mailSettings = _mailSettings.Value;
        }

        public override async Task<RequestResult<bool>> Handle(SendEmailQuery request, CancellationToken cancellationToken)
        {

            var Email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(mailSettings.Email),
                Subject = request.name
            };

            Email.To.Add(MailboxAddress.Parse(request.email));


            var builder = new BodyBuilder();

            builder.HtmlBody = request.confirmationLink;
            Email.Body = builder.ToMessageBody();
            Email.From.Add(new MailboxAddress(mailSettings.DisplayNAme, mailSettings.Email));


            using var smtp = new SmtpClient();
            smtp.Timeout = 10000;
            smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(mailSettings.Email, mailSettings.Password);


            await smtp.SendAsync(Email);
            smtp.Disconnect(true);
            return RequestResult<bool>.Success(true);
        }
    }
}
