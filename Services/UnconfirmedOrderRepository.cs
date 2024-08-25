using Microsoft.EntityFrameworkCore;
using Own_Service.DTO;
using Own_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Own_Service.Services
{
    public class UnconfirmedOrderRepository : IUnconfirmedOrderRepository
    {
        private readonly CommerceDbContext _commerceContext;

        public UnconfirmedOrderRepository(CommerceDbContext commerceContext)
        {
            this._commerceContext = commerceContext;
        }
        public int Add(UnconfirmedOrderDTO unConfirmedOrderdto,string userId)
        {
            int customerId=getCustomerId(userId);
            var product=_commerceContext.Products.Find(unConfirmedOrderdto.productId);
            if (product==null)
                return -1;//product not found

            int productQuantity = product.Quantity; //getProductQuantity(unConfirmedOrderdto.productId);
            if (unConfirmedOrderdto.Quantity > productQuantity)
                return -2;//in case quantity is not available

            try
            {
                UnConfirmedOrder unConfirmedOrder = new UnConfirmedOrder
                {
                    customerId = customerId,
                    PoductId = unConfirmedOrderdto.productId,
                    Quantity = unConfirmedOrderdto.Quantity
                };
                _commerceContext.UnConfirmedOrders.Add(unConfirmedOrder);
                _commerceContext.SaveChanges();
                return unConfirmedOrder.Id;//return id of the unconfirmedOrder Added
            }
            catch(Exception ex)
            {
                return -3;
            }
        }

        public int Delete(int id)
        {
            var order = _commerceContext.UnConfirmedOrders.Find(id);
            if (order == null)
                return 0;
            _commerceContext.UnConfirmedOrders.Remove(order);
            return _commerceContext.SaveChanges();
        }

        public UnconfirmedOrderDTO Edite(UnconfirmedOrderDTO oldunConfirmedOrder, int id)
        {
            throw new System.NotImplementedException();
        }

        public List<UnconfirmedOrderWithProductNameDTO> getAll(string userId)
        {
            int customerId = getCustomerId(userId);
            var unconfirmedOrders =_commerceContext.UnConfirmedOrders.Where(o => o.customerId == customerId).Include(u=>u.product).ToList();
            if(unconfirmedOrders.Count == 0)
                return null;
            List<UnconfirmedOrderWithProductNameDTO> DTOlist=new List<UnconfirmedOrderWithProductNameDTO>();
            foreach (var unconfOrder in unconfirmedOrders)
            {
                DTOlist.Add(new UnconfirmedOrderWithProductNameDTO
                {
                    Id = unconfOrder.Id,
                    ProductName = unconfOrder.product.Name,
                    Quantity = unconfOrder.Quantity,
                });
            }
            return DTOlist;
        }

        public List<UnconfirmedOrderWithProductNameDTO> getAll()
        {
            var unconfirmedOrders = _commerceContext.UnConfirmedOrders.Include(u => u.product).ToList();
            if (unconfirmedOrders.Count == 0)
                return null;
            List<UnconfirmedOrderWithProductNameDTO> DTOlist = new List<UnconfirmedOrderWithProductNameDTO>();
            foreach (var unconfOrder in unconfirmedOrders)
            {
                DTOlist.Add(new UnconfirmedOrderWithProductNameDTO
                {
                    Id = unconfOrder.Id,
                    ProductName = unconfOrder.product.Name,
                    Quantity = unconfOrder.Quantity,
                });
            }
            return DTOlist;
        }

        public UnconfirmedOrderWithProductNameDTO getById(int unconfOrdId)
        {
            var unconfOrder = _commerceContext.UnConfirmedOrders.Find(unconfOrdId);
            if (unconfOrder == null)
                return null;
            return new UnconfirmedOrderWithProductNameDTO { Id=unconfOrder.Id,ProductName=unconfOrder.product.Name,Quantity=unconfOrder.Quantity};
        }
        public int getProductQuantity(int productId)
        {
            int quantity = _commerceContext.Products.Find(productId).Quantity;
            return quantity;
        }
        public int getCustomerId(string userId)
        {
            return _commerceContext.Customers.AsNoTracking().FirstOrDefault(c => c.UserId == userId).Id;
        }
    }
}
