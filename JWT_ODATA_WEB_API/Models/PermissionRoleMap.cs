using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JWT_ODATA_WEB_API.Models
{
    [TrackChanges]
    public class PermissionRoleMap
    {
        public int Id { get; set; }

        [Required]
        public string RoleId { get; set; }

        [Required]
        public int PermissionId { get; set; }


        [ForeignKey("PermissionId")]
        public Permission Permission { get; set; }

        [ForeignKey("RoleId")]
        public IdentityRole Role { get; set; }
    }
}