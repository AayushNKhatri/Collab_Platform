namespace Collab_Platform.ApplicationLayer.Interface.ServiceInterface
{
    public interface IHelperService
    {
        (string userId, string role) GetTokenDetails();
    }
}
