using Stash_And_Grab.Application.Interfaces;

namespace Stash_And_Grab.Data.InMemory;

internal static class InMemoryData
{
    public static List<IDataModel> Records = new()
    {
        new DataModelInMemory
        {
            Id = new Guid("6f9619ff-8b86-d011-b42d-00cf4fc964ff"),
            EntityType = "List",
            JsonContent =
                "[{\"street\":\"123 Elm St\",\"city\":\"SampleCity1\"},{\"street\":\"456 Oak St\",\"city\":\"SampleCity2\"}]",
            LastModified = DateTime.UtcNow.AddDays(-1)
        },
        new DataModelInMemory
        {
            Id = new Guid("3cf8b2a7-c4f0-4fa8-a7b7-71c237dd7a2c"),
            EntityType = "Object",
            JsonContent = "{\"name\":\"John Doe\",\"age\":30}",
            LastModified = DateTime.UtcNow.AddDays(-2)
        },
        new DataModelInMemory
        {
            Id = new Guid("0dab9c2b-0f7c-4669-92d3-8d1dd4e3086c"),
            EntityType = "List",
            JsonContent = "[{\"name\":\"Laptop\",\"price\":1000},{\"name\":\"Mouse\",\"price\":20}]",
            LastModified = DateTime.UtcNow.AddDays(-3)
        },
        new DataModelInMemory
        {
            Id = new Guid("0dab9c2b-0f7c-4669-92d3-8d1dd4e30869"),
            EntityType = "List",
            JsonContent = """["Laptop","Mouse"]""",
            LastModified = DateTime.UtcNow.AddDays(-3)
        }
    };
}