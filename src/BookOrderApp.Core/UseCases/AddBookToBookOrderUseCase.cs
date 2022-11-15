using BookOrderApp.Core.Ports.Persistence;

namespace BookOrderApp.Core.UseCases;

public class AddBookToBookOrderUseCase
{
    private readonly IBookOrderRepository _bookOrderRepository;

    public AddBookToBookOrderUseCase(IBookOrderRepository bookOrderRepository)
    {
        if (bookOrderRepository == null) throw new ArgumentNullException(nameof(bookOrderRepository));
        _bookOrderRepository = bookOrderRepository;
    }

    public void Execute(Guid bookOrderId, string bookTitle)
    {
        var bookOrder = _bookOrderRepository.Get(bookOrderId);

        bookOrder.AddToOrder(bookTitle);

        _bookOrderRepository.Store(bookOrder);
    }
}