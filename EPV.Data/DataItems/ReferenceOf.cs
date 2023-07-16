using System;

namespace EPV.Data.DataItems
{
    public class Reference<TDataItem> : Reference
        where TDataItem : DataItem
    {
        #region Properties


        public static Type ObjectType => typeof(TDataItem);

        #endregion

 
    }
}
