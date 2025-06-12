using System.Text.Json.Serialization;

namespace BaseLibrary.Entities
{
    public class City : BaseEntity
    {
        // Relation many-to-one with Country
        public Country? Country { get; set; }
        public int CountryId { get; set; }

        [JsonIgnore]
        // Relation one-to-many with Town
        public List<Town>? Towns { get; set; }
    }
}
