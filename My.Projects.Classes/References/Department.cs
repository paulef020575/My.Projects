using System;
using System.Runtime.CompilerServices;
using EPV.Data.Attributes;
using EPV.Data.DataItems;
using Newtonsoft.Json;

namespace My.Projects.Classes.References
{
    [DataTable("refDepartment")]
    [Description(nameof(Department.Name))]
    public class Department : DataItem
    {
        [DataColumn] public string Name { get; set; } = "Новое подразделение";

        [DataColumn]
        public Reference<Department> IdParent { get; set; } = new Reference<Department>();

        private Children<Department, Department> _children;

        [JsonIgnore]
        public Children<Department, Department> Children
        {
            get
            {
                if (_children == null)
                    _children = new Children<Department, Department>(this, nameof(IdParent));

                if (!_children.IsLoaded)
                    _children.Load();

                return _children;
            }
        }
    }
}
