using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Uni_Mate.Common;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.UserManagment;

public class UserInfoFilter : IActionFilter
{
    private readonly UserInfoProvider _userInfoProvider;

    public UserInfoFilter(UserInfoProvider userInfoProvider) 
    {
        _userInfoProvider = userInfoProvider;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.User;
        if (user.Identity is ClaimsIdentity identity && identity.IsAuthenticated)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null) userId = "-1";   
            _userInfoProvider.UserInfo = new UserInfo { ID = userId };
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}   