using System;

namespace EPV.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class IdentityAttribute : DataNameAttribute
    {
        public IdentityAttribute(string name = "Id") : base(name) { }
    }
}
