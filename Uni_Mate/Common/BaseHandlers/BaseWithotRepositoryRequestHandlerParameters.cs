using MediatR;
using Microsoft.AspNetCore.Identity;
using Uni_Mate.Common.helper;
using Uni_Mate.Common.Views;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Common.BaseHandlers
{
    public class BaseWithoutRepositoryRequestHandlerParameters<Entity>  where Entity : User
    {
        private readonly IMediator _mediator;
        private readonly TokenHelper _tokenHelper;
        private readonly UserInfo _userInfo;
        private readonly UserManager<User> _userManager;
        private readonly IRepositoryIdentity<Entity> _repositoryIdentity;

        public IMediator Mediator => _mediator;
        public TokenHelper TokenHelper => _tokenHelper;
        public UserInfo UserInfo => _userInfo;
        public UserManager<User> UserManager => _userManager;
        public IRepositoryIdentity<Entity> RepositoryIdentity => _repositoryIdentity;



        //Constructor accepts the generic repository type for flexibility
        public BaseWithoutRepositoryRequestHandlerParameters(IMediator mediator, UserInfoProvider userInfoProvider , TokenHelper tokenHelper , UserManager<User> userManager, IRepositoryIdentity<Entity> repositoryIdentity)
        {
            _mediator = mediator;
            _userInfo = userInfoProvider.UserInfo;
            _tokenHelper = tokenHelper;
            _userManager = userManager;
            _repositoryIdentity = repositoryIdentity;
        }
    }
}
