﻿using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;
using Uni_Mate.Features.Authoraztion.RoleAcess.Querys;
using Uni_Mate.Models.UserManagment.Enum;


namespace Uni_Mate.Filters
{
    public class CustomizeAuthorizeAttribute : ActionFilterAttribute
    {
        
        Feature _feature;
        IMediator _mediator;

        public CustomizeAuthorizeAttribute(Feature feature, IMediator mediator)
        {
            _feature = feature;
            _mediator = mediator;
        }

        public override  void OnActionExecuting(ActionExecutingContext context)
        {
            var claims = context.HttpContext.User;

            var RoleID = claims.FindFirst("roleType");

            if (RoleID == null || string.IsNullOrEmpty(RoleID.Value))
            {

                throw new UnauthorizedAccessException();
            }

            var role = (Role)int.Parse(RoleID.Value);

            var hasAccess =  _mediator.Send(new HasAccessQuery(role, _feature)).Result;

            if (!hasAccess.isSuccess)
            {
                throw new UnauthorizedAccessException();
            }

            base.OnActionExecuting(context);
        }

        //public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        //{
        //    var claims = context.HttpContext.User;

        //    var RoleID = claims.FindFirst("roleType");

        //    if (RoleID == null || string.IsNullOrEmpty(RoleID.Value))
        //    {

        //        throw new UnauthorizedAccessException();
        //    }

        //    var role = (Role)int.Parse(RoleID.Value);

        //    var hasAccess = await _mediator.Send(new HasAccessQuery(role, _feature));

        //    if (!hasAccess.isSuccess)
        //    {
        //        throw new UnauthorizedAccessException();
        //    }
        }
    }

