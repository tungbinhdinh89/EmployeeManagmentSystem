
namespace BaseLibrary.Entities
{
    public class GeneralDepartment : BaseEntity
    {
        // One to many relationship with department
        public List<Department>? Departments { get; set; }
    }
}
