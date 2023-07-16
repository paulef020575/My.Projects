using System;

namespace EPV.Data.Attributes
{
    /// <summary>
    ///     Базовый класс для атрибутов, возвращающих имя
    /// </summary>
    public abstract class DataNameAttribute : Attribute
    {
        #region Properties

        #region Name

        /// <summary>
        ///     Имя объекта
        /// </summary>
        public string Name { get; }

        #endregion

        #endregion

        #region ctor

        public DataNameAttribute(string name = "")
        {
            Name = name;
        }

        #endregion
    }
}
