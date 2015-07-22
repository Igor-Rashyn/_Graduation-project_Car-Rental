using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarRental.DataContexts;
using CarRental.Models;

namespace CarRental.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        private AppDb context;

        public OrderRepository(AppDb context)
        {
            this.context = context;
        }
        public void Create(Order item)
        {
            context.Orders.Add(item);
        }

        public void Delete(Order item)
        {
            Order order = context.Orders.Find(item);
            if (order != null)
                context.Orders.Remove(order);
        }

        public Order Find(object id)
        {
            return context.Orders.Find(id);
        }

        public IEnumerable<Order> GetAll()
        {
            return context.Orders;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Order item)
        {
            context.Entry(item).State = System.Data.Entity.EntityState.Modified;
        }
    }
}