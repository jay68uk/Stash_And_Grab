using System.Text.Json;

namespace Stash_And_Grab.Application.ApplicationServices;

internal static class StashConverterService
{
    public static dynamic? DeserialiseReturn(string itemData, string entityType)
    {
        return entityType switch
        {
            "String" => itemData,
            "Integer" => int.TryParse(itemData, out var parsedInt) ? parsedInt : null,
            "List" => JsonSerializer.Deserialize<List<object>>(itemData),
            "Object" => JsonSerializer.Deserialize<object>(itemData),
            _ => itemData
        };
    }
}