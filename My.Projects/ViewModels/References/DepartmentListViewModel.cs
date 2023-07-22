using System;
using My.Projects.Classes.References;
using My.Projects.Data;
using My.Projects.ViewModels.Base;
using My.Projects.ViewModels.ReferenceItems;

namespace My.Projects.ViewModels.References
{
    public class DepartmentListViewModel : DataListViewModel<Department>
    {
        public override string Title => "Подразделения холдинга";

        public DepartmentListViewModel() : base() { }

        protected override DataItemViewModel<Department> GetDataItemViewModel(Department dataItem = null)
        {
            return new DepartmentViewModel(dataItem);
        }

        protected override object GetData(LoaderArguments loaderArguments)
        {
            return loaderArguments.Connector.LoadChildren<Department>("IdParent", null);
        }
    }
}
