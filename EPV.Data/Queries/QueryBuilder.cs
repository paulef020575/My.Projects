using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using EPV.Data.Attributes;
using EPV.Data.Conditions;
using EPV.Data.DataItems;
using EPV.Database;

namespace EPV.Data.Queries
{
    /// <summary>
    ///     Класс "Генератор запросов"
    /// </summary>
    public static class QueryBuilder
    {
        #region Queries

        #region GetLoadQuery

        /// <summary>
        ///     Возвращает запрос для загрузки строки по идентификатору
        /// </summary>
        /// <typeparam name="TDataItem">тип объекта</typeparam>
        /// <param name="dataItem">объект</param>
        /// <returns>строка запроса</returns>
        public static string GetLoadQuery<TDataItem>(TDataItem dataItem = null) 
            where TDataItem : DataItem
        {
            string result = "SELECT {0} FROM {1} AS T WHERE {2}";

            string columnList = GetSelectColumnList<TDataItem>(),
                tableName = GetTableName<TDataItem>(),
                conditionList = GetIdCondition<TDataItem>();

            return string.Format(result, columnList, tableName, conditionList);
        }

        #endregion

        #region GetLoadListQuery

        /// <summary>
        ///     Возвращает запрос для загрузки всех строк таблицы
        /// </summary>
        /// <typeparam name="TDataItem">тип объектов</typeparam>
        /// <returns>текст запроса</returns>
        public static string GetLoadListQuery<TDataItem>(ICondition conditions = null)
            where TDataItem : DataItem
        {
            string result = "SELECT {0} FROM {1} AS T WHERE {2}",
                conditionsString = "1 = 1";
            if (conditions != null)
                conditionsString = conditions.ToString();

            return string.Format(result, 
                GetSelectColumnList<TDataItem>(), 
                GetTableName<TDataItem>(),
                conditionsString);
        }

        #endregion

        #region GetInsertQuery

        /// <summary>
        ///     Возвращает запрос INSERT для объекта
        /// </summary>
        /// <typeparam name="TDataItem">тип объекта</typeparam>
        /// <param name="dataItem">объект данных</param>
        /// <returns>текст запроса INSERT</returns>
        public static string GetInsertQuery<TDataItem>(TDataItem dataItem = null)
            where TDataItem : DataItem
        {
            string result = "INSERT INTO {0} ({1}) VALUES ({2})";

            List<string> columns = GetColumnList<TDataItem>();
            string columnList = columns[0], parameterList = $"@{columns[0]}";
            for (int i = 1; i < columns.Count; i++)
            {
                columnList += $", {columns[i]}";
                parameterList += $", @{columns[i]}";
            }

            return string.Format(result, GetTableName<TDataItem>(), columnList, parameterList);

        }

        #endregion

        #region GetUpdateQuery

        /// <summary>
        ///     Возвращает текст запроса UPDATE для объекта
        /// </summary>
        /// <typeparam name="TDataItem">тип объекта</typeparam>
        /// <param name="dataItem">объект данных</param>
        /// <returns>текст запроса UPDATE</returns>
        public static string GetUpdateQuery<TDataItem>(TDataItem dataItem = null)
            where TDataItem : DataItem
        {
            string result = "UPDATE {0} SET {1} WHERE {2}";

            List<string> columns = GetColumnList<TDataItem>(false);
            string columnList = $"{columns[0]} = @{columns[0]}";
            for (int i = 1; i < columns.Count; i++)
                columnList += $", {columns[i]} = @{columns[i]}";

            return string.Format(result, GetTableName<TDataItem>(), columnList, GetIdCondition<TDataItem>());
        }

        #endregion

        #region GetUpdateOrInsertQuery

        /// <summary>
        ///     Возвращает текст запроса UPDATE OR INSERT для объекта данных (Firebird)
        /// </summary>
        /// <typeparam name="TDataItem">тип объекта</typeparam>
        /// <param name="dataItem">объект данных</param>
        /// <returns>текст запроса UPDATE OR INSERT</returns>
        public static string GetUpdateOrInsertQuery<TDataItem>(TDataItem dataItem = null)
            where TDataItem : DataItem
        {
            string query = "UPDATE OR INSERT INTO {0} ({1}) VALUES ({2}) MATCHING ({3})";

            List<string> columns = GetColumnList<TDataItem>();
            string columnList = columns[0],
                parameterList = $"@{columns[0]}";

            for (int i = 1; i < columns.Count; i++)
            {
                columnList += $", {columns[i]}";
                parameterList += $", @{columns[i]}";
            }

            return string.Format(query, 
                GetTableName<TDataItem>(), 
                columnList, 
                parameterList, 
                GetIdColumn<TDataItem>());
        }

        #endregion

        #region GetDeleteQuery

        public static string GetDeleteQuery<TDataItem>(TDataItem dataItem = null)
            where TDataItem : DataItem, new()
        {
            string query = "DELETE FROM {0} WHERE {1}";

            return string.Format(query, GetTableName<TDataItem>(), GetIdCondition<TDataItem>());
        }

        #endregion

        #region GetReferencesQuery

        public static string GetReferencesList<TDataItem>(ICondition conditions = null)
            where TDataItem : DataItem, new()
        {
            string query = "SELECT {0} AS Id, {1} AS Description FROM {2}";
            return string.Format(query, GetIdColumn<TDataItem>(), GetDescriptionColumn<TDataItem>(),
                GetTableName<TDataItem>());
        }

        #endregion

        #endregion

        #region Names

        #region GetTableName

        /// <summary>
        ///     Возвращает имя таблицы, определенное для типа
        /// </summary>
        /// <typeparam name="TDataItem">тип</typeparam>
        /// <returns>Имя таблицы в БД</returns>
        public static string GetTableName<TDataItem>()
            where TDataItem : DataItem
        {
            return GetTableName(typeof(TDataItem));
        }

        /// <summary>
        ///     Возвращает имя таблицы, определенное для типа
        /// </summary>
        /// <param name="type">тип данных</param>
        /// <returns>Имя таблицы в БД</returns>
        public static string GetTableName(Type type)
        {
            DataTableAttribute dataTableAttribute 
                = type.GetCustomAttribute<DataTableAttribute>();

            if (dataTableAttribute == null)
                throw new NotImplementedException($"table name for {type.Name} class");

            if (String.IsNullOrEmpty(dataTableAttribute.Name))
                return type.Name;

            return dataTableAttribute.Name;
        }

        #endregion

        #region GetColumnList

        /// <summary>
        ///     Возвращает список колонок для запроса типа
        /// </summary>
        /// <typeparam name="TDataItem">тип</typeparam>
        /// <returns>список колонок</returns>
        public static List<string> GetColumnList<TDataItem>(bool withId = true)
            where TDataItem : DataItem
        {
            List<string> result = new List<string>();

            PropertyInfo[] properties = typeof(TDataItem).GetProperties();
            string idColumn = GetIdColumn<TDataItem>();
            foreach (PropertyInfo property in properties)
            {
                DataColumnAttribute attribute = property.GetCustomAttribute<DataColumnAttribute>();

                if (attribute != null)
                {
                    string columnName = attribute.Name;
                    if (string.IsNullOrEmpty(columnName)) columnName = property.Name;

                    if (withId || !string.Equals(idColumn, columnName))
                        result.Add(columnName);
                }
            }

            if (result.Count == 0)
                throw new NotImplementedException($"columns for {typeof(TDataItem).Name} class");
            return result;
        }

        #endregion
        
        #region GetIdColumn

        /// <summary>
        ///     Возвращает имя идентификатора для объекта данных
        /// </summary>
        /// <typeparam name="TDataItem">тип объекта</typeparam>
        /// <returns>имя поля идентификатора</returns>
        public static string GetIdColumn<TDataItem>()
            where TDataItem : DataItem
        {
            return GetIdColumn(typeof(TDataItem));
        }

        /// <summary>
        ///     Возвращает имя идентификатора для объекта данных
        /// </summary>
        /// <param name="type">тип данных</param>
        /// <returns>имя поля идентификатора</returns>
        public static string GetIdColumn(Type type)
        {

            IdentityAttribute identityAttribute = type.GetCustomAttribute<IdentityAttribute>();
            if (identityAttribute != null)
            {
                return identityAttribute.Name;
            }

            throw new NotImplementedException($"ID for {type.Name} class");
        }

        #endregion

        #region GetDescriptionColumn

        /// <summary>
        ///     Возвращает имя колонки-описания для объекта данных
        /// </summary>
        /// <typeparam name="TDataItem">тип объекта</typeparam>
        /// <returns>имя поля описания</returns>
        public static string GetDescriptionColumn<TDataType>()
            where TDataType : DataItem, new()
        {
            return GetDescriptionColumn(typeof(TDataType));
        }

        /// <summary>
        ///     Возвращает имя колонки описания для объекта данных
        /// </summary>
        /// <param name="type">тип данных</param>
        /// <returns>имя поля описания</returns>
        public static string GetDescriptionColumn(Type type)
        {
            if (type.IsSubclassOf(typeof(DataItem)))
            {
                DescriptionAttribute descriptionAttribute
                    = type.GetCustomAttribute<DescriptionAttribute>();
                if (descriptionAttribute != null)
                {
                    return descriptionAttribute.Name;
                }
            }

            throw new NotImplementedException($"Description column for {type.Name} class");
        }

        #endregion

        #endregion

        #region Query parts

        #region GetIdCondition

        /// <summary>
        ///     Возвращает условие запроса для строки по идентификатору
        /// </summary>
        /// <typeparam name="TDataItem">тип</typeparam>
        /// <returns>текст условия</returns>
        public static string GetIdCondition<TDataItem>()
            where TDataItem : DataItem
        {
            string idColumn = GetIdColumn<TDataItem>();

            return $"{idColumn} = @{idColumn}";
        }

        #endregion

        #region GetQueryExpressionForReference

        /// <summary>
        ///     Возвращает выражение для запроса на описание ссылочного поля
        /// </summary>
        /// <param name="type">тип ссылки</param>
        /// <param name="columnName">имя поля ссылки</param>
        /// <returns>выражение запроса для получения описания</returns>
        public static string GetQueryExpressionForReference(Type type, string columnName)
        {
            return string.Format(
                "(SELECT R.{0} FROM {1} AS R WHERE R.{2} = T.{3}) AS {4}Description",
                GetDescriptionColumn(type), GetTableName(type),
                GetIdColumn(type), columnName, columnName);
        }

        #endregion

        #region GetSelectColumnList

        /// <summary>
        ///     Возвращает выражение списка колонок для запроса SELECT
        /// </summary>
        /// <typeparam name="TDataItem">тип загружаемого объекта</typeparam>
        /// <returns>выражение списка колонок</returns>
        public static string GetSelectColumnList<TDataItem>()
            where TDataItem : DataItem
        {
            string result = "";

            PropertyInfo[] properties = typeof(TDataItem).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                DataColumnAttribute attribute = property.GetCustomAttribute<DataColumnAttribute>();
                if (attribute != null)
                {
                    string columnName = attribute.Name;
                    if (string.IsNullOrEmpty(columnName)) columnName = property.Name;

                    result += (result.Length > 0 ? ", " : "") + columnName;

                    if (property.PropertyType.IsSubclassOf(typeof(Reference)))
                    {
                        Type referenceType = property.PropertyType.GetGenericArguments()[0];
                        result += ", " + GetQueryExpressionForReference(referenceType, columnName);
                    }
                }
            }

            return result;
        }

        #endregion

        #region GetQueryParameters

        /// <summary>
        ///     Врзвращает набор параметров для запросов UPDATE или INSERT
        /// </summary>
        /// <typeparam name="TDataItem">тип данных объекта</typeparam>
        /// <param name="dataItem">объект данных</param>
        /// <returns>набор параметров для запроса</returns>
        public static CommandParameters GetQueryParameters<TDataItem>(TDataItem dataItem)
            where TDataItem : DataItem
        {
            CommandParameters result = new CommandParameters();

            PropertyInfo[] properties = typeof(TDataItem).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                DataColumnAttribute attribute = property.GetCustomAttribute<DataColumnAttribute>();
                if (attribute != null)
                {
                    string columnName = (string.IsNullOrEmpty(attribute.Name) ? property.Name : attribute.Name);
                    object value = property.GetValue(dataItem);
                    if (value is Reference reference)
                    {
                        if (reference.Id.Equals(Guid.Empty))
                            value = DBNull.Value;
                        else
                            value = reference.Id;
                    }

                    result.Add(columnName, value);
                }
            }

            return result;
        }

        #endregion
        

        #endregion
    }
}
