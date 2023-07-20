using System.Collections.Generic;

namespace EPV.Data.DataItems
{
    public class Children<TParent, TChild> : List<TChild>
        where TParent : DataItem, new()
        where TChild : DataItem, new()
    {
        #region Properties

        #region Parent

        public Reference<TParent> Parent { get; }

        #endregion

        #region IdParent

        public string IdParent { get; }

        #endregion

        #region IsLoaded

        public bool IsLoaded { get; private set; }

        #endregion

        #endregion

        #region Constructors

        private Children() { }

        public Children(TParent parent, string idParent)
        {
            Parent = new Reference<TParent>(parent);
            IdParent = idParent;
        }

        #endregion

        #region Methods

        


        #endregion
    }
}
