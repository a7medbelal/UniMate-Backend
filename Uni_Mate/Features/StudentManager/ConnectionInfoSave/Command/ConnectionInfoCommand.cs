using MediatR;
using System.Data.Entity;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.StudentManager.ConnectionInfoSave.Command
{
    public record ConnectionInfoCommand(string? PhoneNum, string? AnotherPhoneNum
        , string? WhatAppLink, string? FaceBookLink): IRequest<RequestResult<bool>>;

    public class ConnectionInfoHandler : BaseWithoutRepositoryRequestHandler<ConnectionInfoCommand, RequestResult<bool>,User>
    {
        public ConnectionInfoHandler(BaseWithoutRepositoryRequestHandlerParameters<User> parameters) : base(parameters)
        {

        }

        public override async Task<RequestResult<bool>> Handle(ConnectionInfoCommand request, CancellationToken cancellationToken)
        {
            var userId = _userInfo.ID;
            if(string.IsNullOrEmpty(userId) || userId == "-1")
            {
                return RequestResult<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.InvalidData, "the data is not valide");
            }

            var student = await _repositoryIdentity.GetByIDAsync(userId);

            if(student == null)
            {
                return RequestResult<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.NotFound, "the Student not found");
            }

            student.PhoneNumber = request.PhoneNum;
            student.AnotherPhoneNum = request.AnotherPhoneNum;
            student.WhatAppLink = request.WhatAppLink;
            student.FaceBookLink = request.FaceBookLink;

            await _repositoryIdentity.UpdateAsync(student);

            return RequestResult<bool>.Success(true, "The data updated Successfully");
        }
    }
}
