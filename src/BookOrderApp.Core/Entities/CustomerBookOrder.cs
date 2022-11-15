using System.ComponentModel;
using System.Runtime.InteropServices.ComTypes;
using BookOrderApp.Core.Exceptions;

namespace BookOrderApp.Core.Entities;

public class CustomerBookOrder
{
    public BookOrderState BookOrderState { get; private set; }
    public Guid Id { get; private set; }
    public string CustomerName { get; private set; }
    public IEnumerable<BookOrderItem> Items { get; private set; }

    public CustomerBookOrder(Guid id, string customerName, string[] books, BookOrderState bookOrderState)
    {
        if (books == null) throw new ArgumentNullException(nameof(books));
        if (id == default)
        {
            throw new ArgumentException("BookOrderId must not be default guid");
        }

        if (string.IsNullOrWhiteSpace(customerName))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(customerName));
        if (!Enum.IsDefined(typeof(BookOrderState), bookOrderState))
            throw new InvalidEnumArgumentException(nameof(bookOrderState), (int)bookOrderState, typeof(BookOrderState));
        
        BookOrderState = bookOrderState;

        Id = id;
        CustomerName = customerName;
        var items = new List<BookOrderItem>();
        foreach (var book in books)
        {
            items.Add(new BookOrderItem(book));
        }

        Items = items;
    }

    public void AddToOrder(string bookTitle)
    {
        if (BookOrderState != BookOrderState.Open)
        {
            throw new DomainException("Unable to add book. Book order is not in open state");
        }
        Items = Items.Append(new BookOrderItem(bookTitle));
    }
}