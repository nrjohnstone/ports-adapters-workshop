using BookOrderApp.Core.Entities;
using BookOrderApp.Core.Exceptions;
using BookOrderApp.Core.Ports.Persistence;

namespace BookOrderApp.Core.UseCases;

public class CreateBookOrderUseCase
{
    private readonly IBookOrderRepository _bookOrderRepository;

    public CreateBookOrderUseCase(IBookOrderRepository bookOrderRepository)
    {
        if (bookOrderRepository == null) throw new ArgumentNullException(nameof(bookOrderRepository));
        _bookOrderRepository = bookOrderRepository;
    }
    
    public bool Execute(Guid bookOrderId, string customerName, string[] books)
    {
        if (_bookOrderRepository.Exists(bookOrderId))
        {
            throw new DomainException("An open book order with this id already exists");
        }

        if (_bookOrderRepository.ExistsForCustomer(customerName))
        {
            throw new DomainException("Customer already has an open book order");
        }
        
        _bookOrderRepository.Store(new CustomerBookOrder(bookOrderId, customerName, books, BookOrderState.Open));
        
        return true;
    }
}