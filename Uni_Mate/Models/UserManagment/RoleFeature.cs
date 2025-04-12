using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Models.UserManagment.Enum;

namespace Uni_Mate.Models.UserManagment
{
    public class RoleFeature : BaseEntity
    {
        public Role role { get; set; }

        public Feature feature { get; set; }
    }
}
