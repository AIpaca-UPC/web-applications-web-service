using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;

/// <summary>
///     Value converter for <see cref="DateOnly" /> values to and from their <see cref="DateTime" /> representation.
/// </summary>
/// <remarks>
///     The MySQL provider reads DATE columns as <see cref="DateTime" />, so every <see cref="DateOnly" /> property
///     is persisted and materialized through this converter.
/// </remarks>
public class DateOnlyConverter()
    : ValueConverter<DateOnly, DateTime>(
        dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue),
        dateTime => DateOnly.FromDateTime(dateTime));
