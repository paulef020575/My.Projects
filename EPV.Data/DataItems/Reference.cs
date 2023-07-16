using System;
using System.Collections.Generic;
using System.Text;

namespace EPV.Data.DataItems
{
    public abstract class Reference
    {
        #region Id

        public Guid Id { get; set; } = Guid.Empty;

        #endregion

        #region Description

        public string Description { get; set; }

        #endregion

        public override string ToString() => Description;
    }
}
