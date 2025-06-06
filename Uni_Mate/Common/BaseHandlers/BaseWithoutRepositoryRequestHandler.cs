﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using Uni_Mate.Common.helper;
using Uni_Mate.Common.Views;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Common.BaseHandlers
{
    public abstract class BaseWithoutRepositoryRequestHandler<TRequest, TResponse ,Entity> : IRequestHandler<TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
        where Entity : User
    {
        protected readonly IMediator _mediator;
        protected readonly TokenHelper _tokenHelper;
        protected readonly UserInfo _userInfo;
        protected readonly UserManager<User> _userManager;
        protected readonly SignInManager<User> _signInManager;
        protected readonly IRepositoryIdentity<Entity> _repositoryIdentity;
        public BaseWithoutRepositoryRequestHandler(BaseWithoutRepositoryRequestHandlerParameters<Entity> parameters)
        {
            _mediator = parameters.Mediator;
            _userInfo = parameters.UserInfo;
            _tokenHelper = parameters.TokenHelper;
            _userManager = parameters.UserManager;
            _repositoryIdentity = parameters.RepositoryIdentity;
        }
        
        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}