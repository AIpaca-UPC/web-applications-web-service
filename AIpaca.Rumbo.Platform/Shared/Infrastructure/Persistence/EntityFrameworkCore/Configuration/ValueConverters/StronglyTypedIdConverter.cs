using AIpaca.Rumbo.Platform.Shared.Domain.Model;
using AIpaca.Rumbo.Platform.Shared.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;

/// <summary>
///     Value converter for strongly-typed identifiers to and from their primitive representation.
/// </summary>
/// <typeparam name="TId">The strongly-typed identifier type.</typeparam>
/// <typeparam name="TValue">The underlying primitive type.</typeparam>
public class StronglyTypedIdConverter<TId, TValue>(
    Func<TId, TValue> convertToProvider,
    Func<TValue, TId> convertFromProvider)
    : ValueConverter<TId, TValue>(
        id => convertToProvider(id),
        val => convertFromProvider(val))
    where TValue : struct
{
    public StronglyTypedIdConverter(Func<TValue, TId> factory)
        : this(id => ((IEntityId<TValue>)id!).Value, factory)
    {
    }
}

/// <summary>
///     Stores a calendar date as the type the MySQL provider reads back.
/// </summary>
/// <remarks>
///     The provider accepts a <see cref="DateOnly" /> when writing but returns a
///     <see cref="DateTime" /> when reading, and Entity Framework cannot cast between the two.
///     Declaring the conversion makes the round trip explicit; the column stays a SQL DATE, so
///     no time component is stored and nothing about the schema changes.
/// </remarks>
public class DateOnlyConverter() : ValueConverter<DateOnly, DateTime>(
    date => date.ToDateTime(TimeOnly.MinValue),
    dateTime => DateOnly.FromDateTime(dateTime));

/// <summary>
///     Stores an instant in UTC, as the type the MySQL provider reads back.
/// </summary>
/// <remarks>
///     Same limitation as <see cref="DateOnlyConverter" />, one type over. A SQL DATETIME carries
///     no offset, so the instant is written as UTC and read back as an offset of zero rather than
///     as a local time that would depend on where the service happens to run.
/// </remarks>
public class InstantConverter() : ValueConverter<DateTimeOffset, DateTime>(
    instant => instant.UtcDateTime,
    dateTime => new DateTimeOffset(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)));