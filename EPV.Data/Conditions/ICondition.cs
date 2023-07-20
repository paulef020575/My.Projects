using System.Collections.Generic;
using EPV.Database;

namespace EPV.Data.Conditions
{
    public interface ICondition
    {
        CommandParameters GetParameters();

        int SetNum(int startNum);
    }
}