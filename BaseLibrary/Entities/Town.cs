using System.Text.Json.Serialization;

namespace BaseLibrary.Entities
{
    /// <summary>
    /// Town es una entidad que representa una localidad
    /// </summary>
    public class Town : BaseEntity
    {
        // Relation one-to-many with Employee
        [JsonIgnore]
        public List<Employee>? Employees { get; set; }

        // Relation many-to-one with City
        public City? City { get; set; }
        public int CityId { get; set; }
    }
}
