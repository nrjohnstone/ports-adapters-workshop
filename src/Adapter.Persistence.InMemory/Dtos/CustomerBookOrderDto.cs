namespace Adapter.Persistence.InMemory.Dtos;

internal class CustomerBookOrderDto
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; }
    public IEnumerable<BookOrderItemDto> Items { get; set; }
}