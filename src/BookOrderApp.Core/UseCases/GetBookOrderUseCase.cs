using BookOrderApp.Core.Entities;
using BookOrderApp.Core.Ports.Persistence;

namespace BookOrderApp.Core.UseCases;

public class GetBookOrderUseCase
{
    private readonly IBookOrderRepository _bookOrderRepository;

    public GetBookOrderUseCase(IBookOrderRepository bookOrderRepository)
    {
        if (bookOrderRepository == null) throw new ArgumentNullException(nameof(bookOrderRepository));
        _bookOrderRepository = bookOrderRepository;
    }

    public CustomerBookOrder Execute(Guid bookOrderId)
    {
        return _bookOrderRepository.Get(bookOrderId);
    }
}