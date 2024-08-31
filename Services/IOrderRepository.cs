namespace Own_Service.Services
{
    public interface IOrderRepository
    {
        int ConfirmOrder(string userId,string Address);
    }
}
