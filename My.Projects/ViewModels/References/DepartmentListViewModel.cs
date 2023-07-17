using EPV.Database;
using My.Projects.Classes.References;
using My.Projects.ViewModels.Base;

namespace My.Projects.ViewModels.References
{
    public class DepartmentListViewModel : DataListViewModel<Department>
    {
        public override string Title => "Подразделения холдинга";

        public DepartmentListViewModel(IConnector connector) : base(connector) { }

    }
}
