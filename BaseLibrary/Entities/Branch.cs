using System.Text.Json.Serialization;

namespace BaseLibrary.Entities
{
    /// <summary>
    /// Branch es una entidad que representa una sucursal de una empresa o razón social
    /// </summary>
    public class Branch : BaseEntity
    {
        // Relation many-to-one with Department
        public Department? Department { get; set; }
        public int DepartmentId { get; set; }

        // Relation one-to-many with Employee
        [JsonIgnore]
        public List<Employee>? Employees { get; set; }
    }
}
