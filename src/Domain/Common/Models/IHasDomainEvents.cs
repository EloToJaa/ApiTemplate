namespace Domain.Common.Models;

public interface IHasDomainEvents
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }

    public void ClearDomainEvents();
}
