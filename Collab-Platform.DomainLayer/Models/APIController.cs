using System.ComponentModel.DataAnnotations;

namespace Collab_Platform.DomainLayer.Models
{
    public class APIController
    {
        public int ControllerID { get; set; }
        public string? RoutePattern { get; set; }
        public IReadOnlyList<string>? HttpMethod { get; set; }
        public string? Controller { get; set; }
        public string? Action { get; set; }

    }
}
