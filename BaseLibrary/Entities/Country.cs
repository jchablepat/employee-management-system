using System.Text.Json.Serialization;

namespace BaseLibrary.Entities
{
    public class Country : BaseEntity
    {
        // Relation one-to-many with City
        [JsonIgnore]
        List<City>? Cities { get; set; }
    }
}
