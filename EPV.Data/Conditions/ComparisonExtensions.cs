using System;
using System.Collections.Generic;
using System.Text;

namespace EPV.Data.Conditions
{
    public static class ComparisonExtensions
    {
        public static string GetExpression(this Comparison comparison)
        {
            switch (comparison)
            {
                case Comparison.Equal:
                    return " = ";

                case Comparison.NotEqual:
                    return " <> ";

                case Comparison.Less:
                    return " < ";

                case Comparison.LessOrEqual:
                    return " <= ";

                case Comparison.Greater:
                    return " > ";

                case Comparison.GreaterOrEqual:
                    return " >= ";

                case Comparison.IsNull:
                    return " IS NULL";

                case Comparison.IsNotNull:
                    return " IS NOT NULL";

                case Comparison.InList:
                    return " IN ";

                case Comparison.NotInList:
                    return " NOT IN ";

                case Comparison.Like:
                    return " LIKE ";

                case Comparison.NotLike:
                    return " NOT LIKE ";


                default:
                    throw new ArgumentException("Unknown comparison");
            }
        }

        public static bool HasParameter(this Comparison comparison)
        {
            return comparison != Comparison.IsNull && comparison != Comparison.IsNotNull;
        }
    }
}
