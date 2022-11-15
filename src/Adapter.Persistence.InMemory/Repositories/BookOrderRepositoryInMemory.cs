using Adapter.Persistence.InMemory.Dtos;
using Adapter.Persistence.InMemory.Mappers;
using BookOrderApp.Core.Entities;
using BookOrderApp.Core.Exceptions;
using BookOrderApp.Core.Ports.Persistence;
using BookOrderApp.Core.UseCases;
using BookOrderState = BookOrderApp.Core.Entities.BookOrderState;

namespace Adapter.Persistence.InMemory.Repositories;

public class BookOrderRepositoryInMemory : IBookOrderRepository
{
    private readonly Dictionary<Guid, CustomerBookOrderDto> _values = new Dictionary<Guid, CustomerBookOrderDto>();
    
    public bool Store(CustomerBookOrder customerBookOrder)
    {
        List<BookOrderItemDto> books = new List<BookOrderItemDto>();
        foreach (var bookOrderItem in customerBookOrder.Items)
        {
            books.Add(new BookOrderItemDto()
            {
                Title = bookOrderItem.Title
            });
        }
        
        var customerBookOrderDto = new CustomerBookOrderDto()
        {
            Id = customerBookOrder.Id,
            CustomerName = customerBookOrder.CustomerName,
            Items = books,
            BookOrderState = customerBookOrder.BookOrderState.MapToDto()
        };
        
        
        _values[customerBookOrder.Id] = customerBookOrderDto;
        return false;
    }

    public bool Exists(Guid bookOrderId)
    {
        return _values.ContainsKey(bookOrderId);
    }

    public bool ExistsForCustomer(string customerName)
    {
        foreach (var customerBookOrderDto in _values)
        {
            if (customerBookOrderDto.Value.CustomerName.Equals(customerName))
            {
                return true;
            }
        }
        return false;
    }

    public CustomerBookOrder Get(Guid id)
    {
        if (!_values.ContainsKey(id))
        {
            throw new DomainException($"Book order with id {id} does not exist");
        }

        var customerBookOrderDto = _values[id];

        BookOrderState bookOrderState = customerBookOrderDto.BookOrderState.MapToEntity();
        
        return new CustomerBookOrder(customerBookOrderDto.Id, customerBookOrderDto.CustomerName, customerBookOrderDto.Items.Select(x => x.Title).ToArray(),
            bookOrderState);
    }
}