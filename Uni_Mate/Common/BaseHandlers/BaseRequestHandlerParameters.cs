using FluentValidation;
using MediatR;
using Uni_Mate.Common;
using Uni_Mate.Common.helper;
using Uni_Mate.Common.Views;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Models;

namespace Uni_Mate.Common.BaseHandlers
{
    public class BaseRequestHandlerParameter<TEntity> where TEntity : BaseEntity
    {
        private readonly IMediator _mediator;
        private readonly IRepository<TEntity> _repository;
        private readonly TokenHelper _tokenHelper;
        private readonly UserInfo _userInfo;

        public IMediator Mediator => _mediator;
        public IRepository<TEntity> Repository => _repository;
        public TokenHelper TokenHelper => _tokenHelper;
        public UserInfo UserInfo => _userInfo;


        // Constructor accepts the generic repository type for flexibility
        public BaseRequestHandlerParameter(IMediator mediator, IRepository<TEntity> repository, UserInfoProvider userInfoProvider, TokenHelper tokenHelper)
        {
            _mediator = mediator;
            _repository = repository;
            _userInfo = userInfoProvider.UserInfo;
            _tokenHelper = tokenHelper;
            
        }
    }
}
