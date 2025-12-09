namespace Collab_Platform.ApplicationLayer.DTO.PermissionDto
{
    public class EndpointDTO
    {
        public string? RoutePattern { get; set; }
        public IReadOnlyList<string>? HttpMethod { get; set; }
        public string? Controller { get; set; }
        public string? Action { get; set; }
    }
}
