using System.Runtime.InteropServices.ComTypes;

namespace BookOrderApp.Core.Entities;

public class CustomerBookOrder
{
    public Guid Id { get; private set; }
    public string CustomerName { get; private set; }
    public IEnumerable<BookOrderItem> Items { get; private set; }

    public CustomerBookOrder(Guid id, string customerName, string[] books)
    {
        if (books == null) throw new ArgumentNullException(nameof(books));
        if (id == default)
        {
            throw new ArgumentException("BookOrderId must not be default guid");
        }

        if (string.IsNullOrWhiteSpace(customerName))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(customerName));

        Id = id;
        CustomerName = customerName;
        var items = new List<BookOrderItem>();
        foreach (var book in books)
        {
            items.Add(new BookOrderItem(book));
        }

        Items = items;
    }
}