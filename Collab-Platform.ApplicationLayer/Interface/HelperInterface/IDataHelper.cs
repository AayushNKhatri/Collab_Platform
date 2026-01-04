namespace Collab_Platform.ApplicationLayer.Interface.HelperInterface
{
    public interface IDataHelper
    {
        (string userId, string role) GetTokenDetails();
        public (Guid? ProjectId, Guid? TaskId, Guid? ChannelId) GetRouteData();
    }
}
