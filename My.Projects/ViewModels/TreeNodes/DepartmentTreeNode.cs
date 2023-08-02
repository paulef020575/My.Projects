using My.Projects.Classes.References;

namespace My.Projects.ViewModels.TreeNodes
{
    public class DepartmentTreeNode : TreeNodeViewModel<Department, DepartmentTreeNode>
    {
        public DepartmentTreeNode(Department department) : base(department) { }

        public override void ReadChildren()
        {
            _children.Clear();

            foreach (Department child in DataItem.Children)
            {
                _children.Add(new DepartmentTreeNode(child));    
            }

            //OnPropertyChanged(nameof(Children));
        }

        public override void RefreshChildren()
        {
            DataItem.Children.Load();

            ReadChildren();
        }
    }
}
