using System.ComponentModel.DataAnnotations;

namespace JWT_ODATA_WEB_API.Infrastructure
{
    [TrackChanges]
    public class UserRoleMap
    {
        [Key]
        public int Id { get; set; }

        public string RoleId { get; set; }
        public string UserId { get; set; }
    }
}