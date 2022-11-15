using BookOrderApp.Core.Entities;

namespace BookOrderApp.Core.Ports.Persistence;

public interface IBookOrderRepository
{
    bool Store(CustomerBookOrder customerBookOrder);
    bool Exists(Guid bookOrderId);
    bool ExistsForCustomer(string customerName);
    CustomerBookOrder Get(Guid bookOrderId);
}