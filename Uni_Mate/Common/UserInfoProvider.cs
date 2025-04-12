using Uni_Mate.Common.Views;

namespace Uni_Mate.Common;

public class UserInfoProvider
{
    public UserInfo UserInfo { get; set; }

    public UserInfoProvider()
    {
        UserInfo = new UserInfo(); // Ensure it's initialized by default
    }
}