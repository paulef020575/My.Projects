﻿using System;
using System.Collections.Generic;
using System.IO;
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

        public void CopyFrom(Reference other)
        {
            Id = other.Id;
            Description = other.Description;
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj as Reference);
        }

        public bool Equals(Reference other)
        {
            return other != null && Id.Equals(other.Id);
        }
    }
}
