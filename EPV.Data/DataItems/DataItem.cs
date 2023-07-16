using System;
using System.Data.Common;
using System.Reflection;
using EPV.Data.Attributes;
using EPV.Data.Queries;

namespace EPV.Data.DataItems
{
    /// <summary>
    ///     Базовый класс для элементов данных
    /// </summary>
    [Identity]
    [Description(nameof(DataItem.Id))]
    public abstract class DataItem
    {
        #region Properties

        #region Id

        [DataColumn]
        public virtual Guid Id { get; set; }

        #endregion

        #endregion

        #region Constructors

        public DataItem()
        {
            Id = Guid.Empty;
        }

        #endregion

        #region Methods

        #region ReadProperties

        /// <summary>
        ///     Заполняет свойства объекта из прочитанных данных
        /// </summary>
        /// <param name="reader">Объект, считанный из БД</param>
        public virtual void ReadProperties(DbDataReader reader)
        {
            PropertyInfo[] properties = GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                DataColumnAttribute dataColumnAttribute = property.GetCustomAttribute<DataColumnAttribute>();
                if (dataColumnAttribute != null)
                {
                    string columnName = dataColumnAttribute.Name;
                    if (string.IsNullOrEmpty(columnName))
                        columnName = property.Name;

                    if (property.PropertyType.IsSubclassOf(typeof(Reference)))
                    {
                        if (!DBNull.Value.Equals(reader[columnName]))
                        {
                            Reference itemReference = (Reference) property.GetValue(this);
                            itemReference.Id = (Guid) reader[columnName];
                            itemReference.Description = (string) reader[$"{columnName}Description"];
                        }
                    }
                    else
                        property.SetValue(this, reader[columnName]);
                }
            }
        }

        #endregion

        #region Equals

        public override bool Equals(object? obj)
        {
            DataItem other = obj as DataItem;

            return (other != null && Equals(other));
        }

        protected bool Equals(DataItem other)
        {
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion

        #region GetDescription

        public virtual string GetDescription()
        {
            DescriptionAttribute attribute = GetType().GetCustomAttribute<DescriptionAttribute>();

            PropertyInfo property = GetType().GetProperty(attribute.Name);
            if (property != null)
                return property.GetValue(this).ToString();

            throw new NotImplementedException("Description");
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            return GetDescription();
        }

        #endregion

        #endregion
    }
}
