using ASSISTENTE.Language.Identifiers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ASSISTENTE.Persistence.Configuration.Converters.Identifiers;

internal sealed class ResourceIdConverter()
    : ValueConverter<ResourceId, Guid>(
        v => v.Value,
        v => new ResourceId(v)
    );