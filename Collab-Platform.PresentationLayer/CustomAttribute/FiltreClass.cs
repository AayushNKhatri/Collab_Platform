using Microsoft.AspNetCore.Mvc;

namespace Collab_Platform.PresentationLayer.CustomAttribute;

public class PermissionValidation : TypeFilterAttribute
{
    public PermissionValidation(string accessType) : base(typeof(ProjectAcessExtention))
    {
        Arguments = [accessType];
    }
}
