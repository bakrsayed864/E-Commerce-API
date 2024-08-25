using Own_Service.DTO;
using Own_Service.Models;
using System.Collections.Generic;

namespace Own_Service.Services
{
    public interface IUnconfirmedOrderRepository
    {
        int Add(UnconfirmedOrderDTO unconfirmedOrderdto,string userId);
        int Delete(int id);
        UnconfirmedOrderDTO Edite (UnconfirmedOrderDTO unconfirmedOrderdto,int id);
        List<UnconfirmedOrderWithProductNameDTO> getAll(string userId);
        List<UnconfirmedOrderWithProductNameDTO> getAll();
        UnconfirmedOrderWithProductNameDTO getById(int unconfOrdId);
    }
}