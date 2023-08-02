using EPV.Data.DataItems;
using My.Projects.ViewModels.Base;
using System.Collections.ObjectModel;

namespace My.Projects.ViewModels.TreeNodes
{
    /// <summary>
    ///     Класс модели представления для узлов дерева
    /// </summary>
    /// <typeparam name="TDataItem">Класс объекта узла</typeparam>
    /// <typeparam name="TChild">Класс для подчиненных узлов</typeparam>
    public abstract class TreeNodeViewModel<TDataItem, TChild> : ViewModel
        where TDataItem : DataItem
        where TChild : ViewModel
    {
        #region Properties

        #region DataItem

        /// <summary>
        ///     Объект данных, соответствующий узлу
        /// </summary>
        public TDataItem DataItem { get; }

        #endregion

        #region Title

        /// <summary>
        ///     Текст для описания узла
        /// </summary>
        public override string Title => DataItem.GetDescription();

        #endregion

        #region IsExpanded

        private bool _isExpanded;
        /// <summary>
        ///     Признак "Узел развернут"
        /// </summary>
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Children

        protected ObservableCollection<TChild> _children;
        /// <summary>
        ///     Подчиненные узлу объекты
        /// </summary>
        public ObservableCollection<TChild> Children
        {
            get
            {
                if (_children == null)
                    ReadChildren();
                return _children;
            }
        }

        #endregion

        #endregion

        #region Ctors

        private TreeNodeViewModel() { }

        public TreeNodeViewModel(TDataItem dataItem)
        {
            DataItem = dataItem;
            _children = new ObservableCollection<TChild>();
        }

        #endregion

        #region Methods

        #region ReadChildren

        /// <summary>
        ///     Перезаполняет коллекцию подчиненных объектов
        /// </summary>
        public abstract void ReadChildren();

        #endregion

        #endregion
    }
}
