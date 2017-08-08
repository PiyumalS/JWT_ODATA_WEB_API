using System.ComponentModel.DataAnnotations;

namespace JWT_ODATA_WEB_API.Infrastructure
{
    [TrackChanges]
    public class ApplicationUserRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}