using Adapter.Persistence.InMemory;
using Adapter.Persistence.InMemory.Repositories;
using BookOrderApp.Core.Entities;
using BookOrderApp.Core.Exceptions;
using BookOrderApp.Core.UseCases;

namespace BookOrderApp.Core.Tests.Unit;

[TestFixture]
public class CreateBookOrderUseCaseTests
{
    private BookOrderRepositoryInMemory _bookOrderRepository;

    private CreateBookOrderUseCase CreateSut()
    {
        return new CreateBookOrderUseCase(_bookOrderRepository);
    }

    [SetUp]
    public void SetUp()
    {
        _bookOrderRepository = new BookOrderRepositoryInMemory();
    }
    
    [Test]
    public void CanCreateUseCase()
    {
        CreateSut();
    }

    [Test]
    public void CanCreateNewBookOrder()
    {
        var sut = CreateSut();
        var bookOrderId = Guid.NewGuid();
        
        // act
        var result = sut.Execute(bookOrderId, "Nathan Johnstone", new[] { "The Real McCaw" });
        
        // assert
        result.Should().BeTrue();
        CustomerBookOrder customerBookOrder = _bookOrderRepository.Get(bookOrderId);
        customerBookOrder.Should().NotBeNull();
        customerBookOrder.Id.Should().Be(bookOrderId);
        customerBookOrder.CustomerName.Should().Be("Nathan Johnstone");
        customerBookOrder.Items.Count().Should().Be(1);
        customerBookOrder.Items.First().Title.Should().Be("The Real McCaw");
    }

    [Test]
    public void WhenBookOrderAlreadyExists_ShouldThrowDomainException()
    {
        var sut = CreateSut();
        var bookOrderId = Guid.NewGuid();

        _bookOrderRepository.Store(new CustomerBookOrder(bookOrderId, "Nathan Johnstone", new[] { "The Real McCaw" }));
        
        // act
        Action createBookOrder =  () => sut.Execute(bookOrderId, "Nathan Johnstone", new[] { "The Real McCaw" });
        
        // assert
        createBookOrder.Should().Throw<DomainException>();
    }
    
    [Test]
    public void WhenCustomerAlreadyHasOpenBookOrder_ShouldThrowDomainException()
    {
        var sut = CreateSut();
        var bookOrderId = Guid.NewGuid();

        _bookOrderRepository.Store(new CustomerBookOrder(bookOrderId, "Nathan Johnstone", new[] { "The Real McCaw" }));
        
        // act
        Action createBookOrder =  () => sut.Execute(Guid.NewGuid(), "Nathan Johnstone", new[] { "The Real McCaw" });
        
        // assert
        createBookOrder.Should().Throw<DomainException>();
    }

    [Test]
    public void CanCreateBookOrder_WhenDifferentCustomerAlreadyHasOpenBookOrder()
    {
        var sut = CreateSut();
        var existingBookOrderId = Guid.NewGuid();

        _bookOrderRepository.Store(new CustomerBookOrder(existingBookOrderId, "Nathan Johnstone", new[] { "The Real McCaw" }));
        
        var bookOrderId = Guid.NewGuid();
        
        // act
        bool result = sut.Execute(bookOrderId, "Dan Carter", new[] { "The Color of Magic" });
        
        // assert
        result.Should().BeTrue();
        CustomerBookOrder customerBookOrder = _bookOrderRepository.Get(bookOrderId);
        customerBookOrder.Should().NotBeNull();
        customerBookOrder.Id.Should().Be(bookOrderId);
        customerBookOrder.CustomerName.Should().Be("Dan Carter");
        customerBookOrder.Items.Count().Should().Be(1);
        customerBookOrder.Items.First().Title.Should().Be("The Color of Magic");
    }
}