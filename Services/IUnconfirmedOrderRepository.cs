using Own_Service.DTO;
using Own_Service.Models;
using System.Collections.Generic;

namespace Own_Service.Services
{
    public interface IUnconfirmedOrderRepository
    {
        int Add(UnconfirmedOrderDTO unconfirmedOrderdto,string userId);
        int Delete(string userId, int id);
        int Delete(int id);

        UnconfirmedOrderDTO Edite (int quantity, int id);
        UnconfirmedOrderDTO Edite(int quantity, UnConfirmedOrder OldunconfOrder);
        List<UnconfirmedOrderWithProductNameDTO> getAll(string userId);
        List<UnconfirmedOrderWithProductNameDTO> getAll();
        UnConfirmedOrder getById(int unconfOrdId);
        double getTotalPrice(string userId);
    }
}