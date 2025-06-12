using System.Text.Json.Serialization;

namespace BaseLibrary.Entities
{
    public class Department : BaseEntity
    {
        // Relation many-to-one with General Department
        public GeneralDepartment? GeneralDepartment { get; set; }
        public int GeneralDepartmentId { get; set; }

        // Relation one-to-many with Branch
        [JsonIgnore]
        public List<Branch>? Branches { get; set; }
    }
}
