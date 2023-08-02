using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using My.Projects.Classes.References;
using My.Projects.Data;
using My.Projects.ViewModels.Base;
using My.Projects.ViewModels.ReferenceItems;
using My.Projects.ViewModels.TreeNodes;

namespace My.Projects.ViewModels.References
{
    public class DepartmentListViewModel : DataListViewModel<Department>
    {
        public override string Title => "Подразделения холдинга";

        public ObservableCollection<DepartmentTreeNode> TreeItems { get; }

        public DepartmentListViewModel() : base()
        {
            TreeItems = new ObservableCollection<DepartmentTreeNode>();
        }

        protected override DataItemViewModel<Department> GetDataItemViewModel(Department dataItem = null)
        {
            return new DepartmentViewModel(dataItem);
        }


        protected override object GetData(LoaderArguments loaderArguments)
        {
            return loaderArguments.Connector.LoadChildren<Department>("IdParent", null);
        }

        protected override void SetData(object data)
        {
            IList<Department> departments = (IList<Department>)data;

            foreach (Department department in departments)
                TreeItems.Add(new DepartmentTreeNode(department));
        }

        public void LoadChildren(Guid id)
        {
            foreach (Department department in Items)
            {
                
            }
        }
    }
}
