using System;

namespace EPV.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataColumnAttribute : DataNameAttribute
    {
        public DataColumnAttribute(string name = "") : base(name) { }
    }
}
