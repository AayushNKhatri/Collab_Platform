namespace Collab_Platform.PresentationLayer.Middleware
{
    public class ExecptionClass
    {
        public class ForbiddenException : Exception
        {
            public ForbiddenException(string message = "Forbidden access") : base(message) { }
        }
    }
}
