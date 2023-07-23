using System;
using System.Collections.ObjectModel;
using My.Projects.Classes.References;
using My.Projects.Data;
using My.Projects.ViewModels.Base;
using My.Projects.ViewModels.ReferenceItems;

namespace My.Projects.ViewModels.References
{
    public class DepartmentListViewModel : DataListViewModel<Department>
    {
        public override string Title => "Подразделения холдинга";

        public ObservableCollection<DepartmentViewModel> TreeItems { get; }

        public DepartmentListViewModel() : base()
        {
            TreeItems = new ObservableCollection<DepartmentViewModel>();
        }

        protected override DataItemViewModel<Department> GetDataItemViewModel(Department dataItem = null)
        {
            return new DepartmentViewModel(dataItem);
        }


        protected override object GetData(LoaderArguments loaderArguments)
        {
            return loaderArguments.Connector.LoadChildren<Department>("IdParent", null);
        }

        public void LoadChildren(Guid id)
        {
            foreach (Department department in Items)
            {
            }
        }
    }
}
