namespace Collab_Platform.DomainLayer.Models
{
    public class APIResponse
    {
        public bool Success { get; set; }
        public string? Messege { get; set; }
        public string? Error { get; set; }
        public string ? TraceID { get; set; }

    }
    public class APIResponse<T> : APIResponse { 
        public T? Data { get; set; }
    }
}
