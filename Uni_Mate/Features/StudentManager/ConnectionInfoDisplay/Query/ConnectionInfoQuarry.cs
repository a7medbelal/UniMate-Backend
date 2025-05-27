using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.StudentManager.ConnectionInfoDisplay.Query
{
    public record ConnectionInfoQuarry() : IRequest<RequestResult<ConnectionInfoDTO>> ;
    public class ConnectionHandler : BaseWithoutRepositoryRequestHandler<ConnectionInfoQuarry,
        RequestResult<ConnectionInfoDTO>,
        Student>
    {
        public ConnectionHandler(BaseWithoutRepositoryRequestHandlerParameters<Student> parameters) : base(parameters)
        {
        }

        public override async Task<RequestResult<ConnectionInfoDTO>> Handle(ConnectionInfoQuarry request, CancellationToken cancellationToken)
        {
            var userId = _userInfo.ID;


            //7de44948-9c0c-4227-9c00-3f35a7ac4a56

            if (string.IsNullOrEmpty(userId) || userId == "-1")
            {
                return RequestResult<ConnectionInfoDTO>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.InvalidData, "the data is invalide");
            }

            var student = _repositoryIdentity.Get(st => st.Id == userId).FirstOrDefault();

            if(student == null)
            {
                return RequestResult<ConnectionInfoDTO>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.NotFound, "User Not Found");
            }

            var connectionInfo = new ConnectionInfoDTO(student.PhoneNumber,student.AnotherPhoneNum,student.WhatAppLink,student.FaceBookLink);

            return RequestResult<ConnectionInfoDTO>.Success(connectionInfo, "The data is returned successfully");
        }
    }
}
