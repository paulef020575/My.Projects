using My.Projects.Classes.References;
using My.Projects.ViewModels.Base;

namespace My.Projects.ViewModels.ReferenceItems
{
    public class DepartmentViewModel : DataItemViewModel<Department>
    {
        #region Properties

        #region Title

        public override string Title => DataItem.Name;

        #endregion

        #region Name

        public string Name
        {
            get => DataItem.Name;
            set => SetProperty(value);
        }

        #endregion
        
        #endregion


    }
}
