using System;

namespace EPV.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DataTableAttribute : DataNameAttribute
    {
        public DataTableAttribute(string name = "") : base(name) { }
    }
}
