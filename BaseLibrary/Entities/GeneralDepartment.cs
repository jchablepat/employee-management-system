using System.Text.Json.Serialization;

namespace BaseLibrary.Entities
{
    public class GeneralDepartment : BaseEntity
    {
        // Relation one-to-many with Department
        [JsonIgnore]
        public List<Department>? Departments { get; set; }
    }
}
