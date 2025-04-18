using MediatR;
using Microsoft.AspNetCore.Identity;
using Uni_Mate.Common.helper;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Common.BaseHandlers
{
    public abstract class BaseWithoutRepositoryRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
    {
        protected readonly IMediator _mediator;
        protected readonly TokenHelper _tokenHelper;
        protected readonly UserInfo _userInfo;
        protected readonly UserManager<User> _userManager;
        public BaseWithoutRepositoryRequestHandler(BaseWithoutRepositoryRequestHandlerParameters parameters)
        {
            _mediator = parameters.Mediator;
            _userInfo = parameters.UserInfo;
            _tokenHelper = parameters.TokenHelper;
            _userManager = parameters.UserManager;
        }
        
        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}