using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Uni_Mate.Common;
using Uni_Mate.Common.helper;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Common.BaseHandlers
{
    public class BaseWithoutRepositoryRequestHandlerParameters
    {
        private readonly IMediator _mediator;
        private readonly TokenHelper _tokenHelper;
        private readonly UserInfo _userInfo;
        private readonly UserManager<User> _userManager;  

        public IMediator Mediator => _mediator;
        public TokenHelper TokenHelper => _tokenHelper;
        public UserInfo UserInfo => _userInfo;
        public UserManager<User> UserManager => _userManager;


        //Constructor accepts the generic repository type for flexibility
        public BaseWithoutRepositoryRequestHandlerParameters(IMediator mediator, UserInfoProvider userInfoProvider , TokenHelper tokenHelper , UserManager<User> userManager)
        {
            _mediator = mediator;
            _userInfo = userInfoProvider.UserInfo;
            _tokenHelper = tokenHelper;
            _userManager = userManager;
        }
    }
}
