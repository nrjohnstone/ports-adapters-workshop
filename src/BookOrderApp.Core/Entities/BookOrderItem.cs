namespace BookOrderApp.Core.Entities;

public class BookOrderItem
{
    public string Title { get; }

    public BookOrderItem(string title)
    {
        Title = title;
    }
}