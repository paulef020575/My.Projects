using System;
using System.Collections.Generic;
using System.Reflection;
using EPV.Database;

namespace EPV.Data.Conditions
{
    public class QueryCondition : ICondition
    {
        public string Column { get; private set; }

        public Comparison Comparison { get; private set; }

        public object Value { get; private set; }

        public int? CondOrder { get; set; } = null;

        public QueryCondition(string column, Comparison comparison, object value = null)
        {
            Column = column;
            Comparison = comparison;
            Value = value;
        }

        public override string ToString()
        {
            if (Comparison == Comparison.InList || Comparison == Comparison.NotInList)
                throw new NotImplementedException("InList and NotInList comparisons");

            string result = Column + Comparison.GetExpression() + ParameterName;

            return result;
        }

        private string ParameterName
        {
            get
            {
                string result = "";

                if (Comparison.HasParameter())
                {
                    result += "@";
                    if (CondOrder != null)
                        result += $"cond{CondOrder}_";

                    result += Column;
                }

                return result;
            }
        }

        public CommandParameters GetParameters()
        {
            CommandParameters result = new CommandParameters();
            if (Comparison.HasParameter()) result.Add(ParameterName, Value);
            return result;
        }

        public int SetNum(int startNum)
        {
            CondOrder = startNum++;

            return startNum;
        }
    }
}
