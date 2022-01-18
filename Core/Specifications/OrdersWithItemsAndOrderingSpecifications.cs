using System.Linq.Expressions;
using Core.Entities.OrderAggregate;

namespace Core.Specifications
{
  public class OrdersWithItemsAndOrderingSpecifications : BaseSpecification<Order>
  {
    public OrdersWithItemsAndOrderingSpecifications(string email): base(o => o.BuyerEmail == email)
    {
        AddInclude(o => o.OrderItem);
        AddInclude(o => o.DeliveryMethod);
        AddOrderByDescending(o => o.OrderDate);
    }

    public OrdersWithItemsAndOrderingSpecifications(int id, string email) 
    : base(o => o.Id == id && o.BuyerEmail == email)
    {
        AddInclude(o => o.OrderItem);
        AddInclude(o => o.DeliveryMethod);
    }
  }
}