using EPV.Data.Attributes;
using EPV.Data.DataItems;

namespace My.Projects.Classes.References
{
    [DataTable("refDepartment")]
    [Description(nameof(Department.Name))]
    public class Department : DataItem
    {
        [DataColumn]
        public string Name { get; set; }

        [DataColumn]
        public Reference<Department> IdParent { get; } = new Reference<Department>();

    }
}
