namespace Own_Service.Services
{
    public interface IOrderRepository
    {
        int ConfirmOrder(int UnconfirmedOrderId);

    }
}
