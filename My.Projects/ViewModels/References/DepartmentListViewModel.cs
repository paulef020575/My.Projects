using EPV.Database;
using My.Projects.Classes.References;
using My.Projects.ViewModels.Base;
using My.Projects.ViewModels.ReferenceItems;

namespace My.Projects.ViewModels.References
{
    public class DepartmentListViewModel : DataListViewModel<Department>
    {
        public override string Title => "Подразделения холдинга";

        public DepartmentListViewModel(IConnector connector) : base(connector) { }

        protected override DataItemViewModel<Department> GetDataItemViewModel(Department dataItem = null)
        {
            return new DepartmentViewModel(Connector, dataItem);
        }
    }
}
