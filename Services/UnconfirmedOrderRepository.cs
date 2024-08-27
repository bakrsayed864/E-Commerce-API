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
        public int Add(UnconfirmedOrderDTO unConfirmedOrderdto, string userId)
        {
            int customerId = getCustomerId(userId);

            if (!checkProductAvilability(unConfirmedOrderdto.productId, unConfirmedOrderdto.Quantity))
            {
                return -1; //product not found or quantity required exceeded available
            }

            var unconfirmedOrder = _commerceContext.UnConfirmedOrders.FirstOrDefault(u => u.customerId == customerId && u.PoductId == unConfirmedOrderdto.productId);
            if (unconfirmedOrder != null)
            {
                //this mean the customer add order on product which exist in unconfirmed orders for same caustomer
                //so we will edite the quantity only
                var result = Edite(unConfirmedOrderdto.Quantity + unconfirmedOrder.Quantity, unconfirmedOrder.Id);
                return result == null ? -2 : result.Id; //-2 in case quantity is not available
            }
            return CreateNewOrder(customerId, unConfirmedOrderdto);
        }
      
        public int Delete(string userId, int orderId)//user only delete unconfirmed order he made
        {
            int customerId = getCustomerId(userId);
            var order = _commerceContext.UnConfirmedOrders.FirstOrDefault(u => u.customerId == customerId && u.Id == orderId);
            if (order == null)
                return 0;
            _commerceContext.UnConfirmedOrders.Remove(order);
            return _commerceContext.SaveChanges();
        }
        public int Delete(int orderId)//will be for admin (admin can delete any unconfirmed order)
        {
            var order = _commerceContext.UnConfirmedOrders.FirstOrDefault(u => u.Id == orderId);
            if (order == null)
                return 0;
            _commerceContext.UnConfirmedOrders.Remove(order);
            return _commerceContext.SaveChanges();
        }

        public UnconfirmedOrderDTO Edite(int quantity, int orderId)
        {
            var OldunconfOrder = _commerceContext.UnConfirmedOrders.Find(orderId);
            var prodQuantity = _commerceContext.Products.Find(OldunconfOrder.PoductId).Quantity;
            if(OldunconfOrder == null||prodQuantity<quantity)
                return null;
            OldunconfOrder.Quantity=quantity;
            //OldunconfOrder.PoductId = unConfirmedOrder.productId;
            //OldunconfOrder.Id = unConfirmedOrder.Id;
            _commerceContext.SaveChanges();
            return new UnconfirmedOrderDTO { Id=OldunconfOrder.Id,Quantity=OldunconfOrder.Quantity,productId=OldunconfOrder.PoductId};
        }

        public List<UnconfirmedOrderWithProductNameDTO> getAll(string userId)//for specific user
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

        public List<UnconfirmedOrderWithProductNameDTO> getAll()//for all users(Admin)
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
        private int getProductQuantity(int productId)
        {
            int quantity = _commerceContext.Products.Find(productId).Quantity;
            return quantity;
        }
        public int getCustomerId(string userId)
        {
            return _commerceContext.Customers.AsNoTracking().FirstOrDefault(c => c.UserId == userId).Id;
        }

        private bool checkProductAvilability(int productId, int quantity)
        {
            var product = _commerceContext.Products.Find(productId);
            return product != null && product.Quantity >= quantity;
        }
        private int CreateNewOrder(int customerId, UnconfirmedOrderDTO unConfirmedOrderdto)
        {
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
            catch (Exception ex)
            {
                return -3;
            }
        }
    }
}
