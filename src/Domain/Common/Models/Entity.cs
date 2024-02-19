using ErrorOr;
using FluentValidation.Results;

namespace Domain.Common.Models;

public abstract class Entity<TId> : IEquatable<Entity<TId>>, IHasDomainEvents
    where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public TId Id { get; protected set; }

    protected Entity(TId id)
    {
        Id = id;
    }

    protected ErrorOr<bool> GetValidationErrors(ValidationResult validationResult)
    {
        if (validationResult.IsValid)
        {
            return true;
        }

        var errors = validationResult.Errors
            .ConvertAll(validationFailure => Error.Validation(
                validationFailure.PropertyName,
                validationFailure.ErrorMessage));

        return errors;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
            return false;

        var entity = (Entity<TId>)obj;

        return Id.Equals(entity.Id);
    }

    public bool Equals(Entity<TId>? other)
    {
        return Equals(other);
    }

    public static bool operator ==(Entity<TId>? a, Entity<TId>? b)
    {
        return Equals(a, b);
    }

    public static bool operator !=(Entity<TId>? a, Entity<TId>? b)
    {
        return !Equals(a, b);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

#pragma warning disable CS8618
    protected Entity()
    {
    }
#pragma warning restore CS8618
}
