using Adapter.Persistence.InMemory.Repositories;
using BookOrderApp.Core.Entities;
using BookOrderApp.Core.Exceptions;
using BookOrderApp.Core.UseCases;

namespace BookOrderApp.Core.Tests.Unit;

[TestFixture]
public class GetBookOrderUseCaseTests
{
    private BookOrderRepositoryInMemory _bookOrderRepository;
    
    [SetUp]
    public void SetUp()
    {
        _bookOrderRepository = new BookOrderRepositoryInMemory();
    }
    
    private GetBookOrderUseCase CreateSut()
    {
        return new GetBookOrderUseCase(_bookOrderRepository);
    }

    [Test]
    public void WhenBookOrderDoesNotExist_ShouldThrowDomainException()
    {
        var sut = CreateSut();
        var bookOrderId = Guid.NewGuid();
        _bookOrderRepository.Store(new CustomerBookOrder(bookOrderId, "Nathan Johnstone", new[] { "The Real McCaw" }));
        var bookOrderIdThatDoesNotExist = Guid.NewGuid();
        
        // act
        Action getBookOrder = () => sut.Execute(bookOrderIdThatDoesNotExist);
        
        // assert
        getBookOrder.Should().Throw<DomainException>();
    }
    
    [Test]
    public void CanRetrieveCustomerBookOrder()
    {
        var sut = CreateSut();
        var bookOrderId = Guid.NewGuid();
        _bookOrderRepository.Store(new CustomerBookOrder(bookOrderId, "Nathan Johnstone", new[] { "The Real McCaw" }));
        
        // act
        var customerBookOrder = sut.Execute(bookOrderId);
        
        // assert
        customerBookOrder.Should().NotBeNull();
        customerBookOrder.Id.Should().Be(bookOrderId);
        customerBookOrder.CustomerName.Should().Be("Nathan Johnstone");
        customerBookOrder.Items.Count().Should().Be(1);
        customerBookOrder.Items.First().Title.Should().Be("The Real McCaw");
    }
}