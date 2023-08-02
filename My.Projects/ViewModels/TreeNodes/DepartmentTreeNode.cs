using My.Projects.Classes.References;

namespace My.Projects.ViewModels.TreeNodes
{
    public class DepartmentTreeNode : TreeNodeViewModel<Department, DepartmentTreeNode>
    {
        public DepartmentTreeNode(Department department) : base(department) { }

        public override void ReadChildren()
        {
            DataItem.Children.Load();
            Children.Clear();

            foreach (Department child in DataItem.Children)
            {
                Children.Add(new DepartmentTreeNode(child));    
            }

            OnPropertyChanged(nameof(Children));
        }
    }
}
