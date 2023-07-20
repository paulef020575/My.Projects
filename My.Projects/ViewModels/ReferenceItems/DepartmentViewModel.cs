using System.Collections.Generic;
using System.Collections.ObjectModel;
using EPV.Data.DataItems;
using My.Projects.Classes.References;
using My.Projects.Data;
using My.Projects.ViewModels.Base;

namespace My.Projects.ViewModels.ReferenceItems
{
    public class DepartmentViewModel : DataItemViewModel<Department>
    {
        public DepartmentViewModel(IMyConnector connector, Department department = null) : base(connector, department) { }

        #region Properties

        #region Title

        public override string Title => DataItem?.Name;

        #endregion

        #region Name

        public string Name
        {
            get => DataItem.Name;
            set => SetProperty(value);
        }

        #endregion

        #region IdParent

        public Reference<Department> IdParent
        {
            get => DataItem.IdParent;
            set
            {
                if (value == null || !value.Equals(DataItem.IdParent))
                {
                    DataItem.IdParent.CopyFrom(value);
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region DepartmentList

        private ObservableCollection<Reference<Department>> _departmentList;

        public ObservableCollection<Reference<Department>> DepartmentList
        {
            get
            {
                if (_departmentList == null)
                    _departmentList = GetDepartmentList();
                return _departmentList;
            }
        }

        private ObservableCollection<Reference<Department>> GetDepartmentList()
        {
            IList<Reference<Department>> references = Connector.LoaReferences<Department>();

            ObservableCollection<Reference<Department>> result = new ObservableCollection<Reference<Department>>();
            foreach (Reference<Department> reference in references)
                if (reference.Id != DataItem.Id)
                    result.Add(reference);

            return result;
        }

        #endregion

        #endregion

        protected override object GetData(LoaderArguments loaderArguments)
        {
            return base.GetData(loaderArguments);
        }
    }
}
