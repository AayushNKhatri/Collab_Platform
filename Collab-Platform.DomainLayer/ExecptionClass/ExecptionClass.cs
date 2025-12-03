namespace Collab_Platform.PresentationLayer.Middleware
{
    public class ExecptionClass
    {
        public class InvalidRoleException : Exception
        {
            public InvalidRoleException() { }
            public InvalidRoleException(string message) : base(message) { }
            public InvalidRoleException(string message, Exception inner) : base(message, inner) { }
        }
    }
}
