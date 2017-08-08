using System.ComponentModel.DataAnnotations;

namespace JWT_ODATA_WEB_API.Models
{
    public class Permission
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}