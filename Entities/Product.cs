using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductApiAdvanced.Entities;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string Name { get; set; } = ""!;

    public decimal Price { get; set; }
}

/*
public class Product
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Name { get; set; } = "";

    public decimal Price { get; set; }
}
*/

