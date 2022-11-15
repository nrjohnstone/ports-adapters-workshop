using Adapter.Persistence.InMemory.Repositories;
using BookOrderApp.Core.Entities;
using BookOrderApp.Core.Exceptions;
using BookOrderApp.Core.UseCases;

namespace BookOrderApp.Core.Tests.Unit;

[TestFixture]
public class AddBookToBookOrderUseCaseTests
{
    private BookOrderRepositoryInMemory _bookOrderRepository;
    
    [SetUp]
    public void SetUp()
    {
        _bookOrderRepository = new BookOrderRepositoryInMemory();
    }
    
    private AddBookToBookOrderUseCase CreateSut()
    {
        return new AddBookToBookOrderUseCase(_bookOrderRepository);
    }

    [Test]
    public void WhenBookOrderIsInOpenState_CanAddBook()
    {
        var sut = CreateSut();
        var bookOrderId = Guid.NewGuid();
        
        _bookOrderRepository.Store(new CustomerBookOrder(bookOrderId, "Nathan Johnstone", new[] { "The Real McCaw" },
            BookOrderState.Open));

        // act
        sut.Execute(bookOrderId, "The Jersey");
        
        // assert
        var actualBookOrder = _bookOrderRepository.Get(bookOrderId);
        actualBookOrder.Items.Count().Should().Be(2);
    }
    
    [Test]
    public void WhenBookOrderIsNotInOpenState_ShouldThrowDomainException()
    {
        var sut = CreateSut();
        var bookOrderId = Guid.NewGuid();

        var customerBookOrder = new CustomerBookOrder(bookOrderId, "Nathan Johnstone", 
            new[] { "The Real McCaw" }, BookOrderState.Closed);
        
        _bookOrderRepository.Store(customerBookOrder);

        // act
        Action addBookToClosedOrder = () => sut.Execute(bookOrderId, "The Jersey");
        
        // assert
        addBookToClosedOrder.Should().Throw<DomainException>();
    }
}