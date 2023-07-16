using System.Collections.Generic;

namespace EPV.Data.Conditions
{
    public interface ICondition
    {
        Dictionary<string, object> GetParameters();

        int SetNum(int startNum);
    }
}