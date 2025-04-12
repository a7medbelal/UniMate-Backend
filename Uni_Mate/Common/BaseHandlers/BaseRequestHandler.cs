using MediatR;
using TrelloCopy.Common.BaseHandlers;
using Uni_Mate.Common.helper;
using Uni_Mate.Common.Views;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Models;

namespace Uni_Mate.Common.BaseHandlers
{
    public abstract class BaseRequestHandler<TRequest, TResponse, TEntity> : IRequestHandler<TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
        where TEntity : BaseEntity
    {
        protected readonly IMediator _mediator;
        protected readonly IRepository<TEntity> _repository;
        protected readonly TokenHelper _tokenHelper;
        protected readonly UserInfo _userInfo;
        
        public BaseRequestHandler(BaseWithoutRepositoryRequestHandlerParameter<TEntity> parameters)
        {
            _mediator = parameters.Mediator;
            _repository = parameters.Repository;
            _userInfo = parameters.UserInfo;
            _tokenHelper = parameters.TokenHelper;

            
        }
        
        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}