using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EPV.Data.DataItems;
using My.Projects.Classes.References;
using My.Projects.Data;
using My.Projects.MyEventArgs;
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
            IList<Department> departments = (IList<Department>) data;
            TreeItems.Clear();
            if (departments != null)
                foreach (Department department in departments)
                    TreeItems.Add(new DepartmentTreeNode(department));
        }

        public void LoadChildren(Guid? id)
        {
            if (id.HasValue)
            {
                DepartmentTreeNode node = FindNode(TreeItems, id.Value);
                node.RefreshChildren();
                node.IsExpanded = true;
            }
            else
            {
                LoadData();
            }
        }

        public DepartmentTreeNode FindNode(ICollection<DepartmentTreeNode> nodes, Guid id)
        {
            foreach (DepartmentTreeNode node in nodes)
            {
                if (node.DataItem.Id == id)
                    return node;

                DepartmentTreeNode child = FindNode(node.Children, id);

                if (child != null)
                {
                    node.IsExpanded = true;
                    return child;
                }
            }

            return null;
        }

        private void AddChild(Department department)
        {
            DepartmentViewModel viewModel = new DepartmentViewModel();
            viewModel.IdParent = new Reference<Department>(department);
            _onSwitchToViewModel(this, new ViewModelEventArgs(viewModel, false));
        }

        private RelayCommand _addChildCommand;

        public RelayCommand AddChildCommand
        {
            get
            {
                if (_addChildCommand == null)
                    _addChildCommand = new RelayCommand(
                        x => AddChild(x as Department), 
                        x => x is Department);

                return _addChildCommand;
            }
        }
    }
}
