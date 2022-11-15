using Adapter.Persistence.InMemory.Dtos;
using BookOrderApp.Core.Entities;
using BookOrderApp.Core.Exceptions;

namespace Adapter.Persistence.InMemory.Mappers;

internal static class BookOrderStateMapper
{
    public static BookOrderState MapToEntity(this BookOrderStateDto bookOrderState)
    {
        switch (bookOrderState)
        {
            case BookOrderStateDto.Unknown:
                return BookOrderState.Unknown;
            
            case BookOrderStateDto.Closed:
                return BookOrderState.Closed;
            
            case BookOrderStateDto.Open:
                return BookOrderState.Open;
            
            default:
                throw new DomainException("Unmapped BookOrderState enum value");
        }
    }
    
    public static BookOrderStateDto MapToDto(this BookOrderState bookOrderState)
    {
        switch (bookOrderState)
        {
            case BookOrderState.Unknown:
                return BookOrderStateDto.Unknown;
            
            case BookOrderState.Closed:
                return BookOrderStateDto.Closed;
            
            case BookOrderState.Open:
                return BookOrderStateDto.Open;
            
            default:
                throw new DomainException("Unmapped BookOrderState enum value");
        }
    }
}