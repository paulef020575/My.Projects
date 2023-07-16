using System;

namespace EPV.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DescriptionAttribute : DataNameAttribute
    {
        public DescriptionAttribute(string name = "") : base(name) { }
    }
}
