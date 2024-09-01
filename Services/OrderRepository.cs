using Microsoft.EntityFrameworkCore;
using Own_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Own_Service.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CommerceDbContext _commerceDbContext;

        public OrderRepository(CommerceDbContext commerceDbContext)
        {
            this._commerceDbContext = commerceDbContext;
        }
        public int ConfirmOrder(string userId,string Address)
        {
            int customerId = getCustomerId(userId);
            //get user unconfirmed orders 
            var unConfirmedOrdersList = _commerceDbContext.UnConfirmedOrders.Where(u => u.customerId == customerId).ToList();
            //check if all unconfirmed orders quantities is available
            bool isAvailabe=checkQuantityAvailability(unConfirmedOrdersList);
            if(!isAvailabe)
                return 0;
            //confirm order and remove them from unconfirmed orders
            int changes = orderDone(unConfirmedOrdersList, Address, customerId);
            deleteCustomerUnconfirmedOrders(customerId);
            return changes;
        }
        private int orderDone(List<UnConfirmedOrder> unConfirmedOrders,string Address, int customerId)
        {
            Order order = new Order { Date = DateTime.Now, Address = Address, CustomerId = customerId };
            int orderId=CreatOrder(order);
            OrderDetails orderDetails;
            foreach (var unconfirmedOrder in unConfirmedOrders)
            {
                orderDetails = new OrderDetails();
                var product = _commerceDbContext.Products.Find(unconfirmedOrder.PoductId);
                orderDetails.OrderId = orderId;
                orderDetails.Quantity = unconfirmedOrder.Quantity;
                orderDetails.ProductId = unconfirmedOrder.PoductId;
                product.Quantity = product.Quantity-unconfirmedOrder.Quantity;
                _commerceDbContext.Add(orderDetails);
            }
            return _commerceDbContext.SaveChanges();
        }
        private int CreatOrder(Order order)
        {
            _commerceDbContext.Orders.Add(order);
            _commerceDbContext.SaveChanges();
            return order.Id;
        }
        private bool checkQuantityAvailability(List<UnConfirmedOrder> unConfirmedOrders)
        {
            Product product;
            foreach(var unConfirmedOrder in unConfirmedOrders)
            {
                product = _commerceDbContext.Products.Find(unConfirmedOrder.PoductId);
                if (product == null || product.Quantity < unConfirmedOrder.Quantity)
                    return false;
            }
            return true;
        }
        private int deleteCustomerUnconfirmedOrders(int customerId)
        {
            //get user unconfirmed orders
            var ordersList = _commerceDbContext.UnConfirmedOrders.Where(u => u.customerId == customerId);
            //remove
            _commerceDbContext.UnConfirmedOrders.RemoveRange(ordersList);
            return _commerceDbContext.SaveChanges();
        }
        public int getCustomerId(string userId)
        {
            return _commerceDbContext.Customers.AsNoTracking().FirstOrDefault(c => c.UserId == userId).Id;
        }
    }
}
