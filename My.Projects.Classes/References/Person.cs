using System;
using EPV.Data.Attributes;
using EPV.Data.DataItems;

namespace My.Projects.Classes.References
{
    [DataTable("refPerson")]
    [Description(nameof(Person.Lastname))]
    public class Person : DataItem
    {
        #region Properties

        #region Lastname

        [DataColumn]
        public string Lastname { get; set; }

        #endregion

        #region Firstname

        [DataColumn]
        public string Firstname { get; set; }

        #endregion

        #region Secondname

        [DataColumn]
        public string Secondname { get; set; }

        #endregion

        #region Birthday

        [DataColumn]
        public DateTime? Birthday { get; set; }

        #endregion

        #region Fullname

        public string Fullname => string.Join(Lastname, Firstname, Secondname);

        #endregion

        #region Shortname

        public string Shortname
        {
            get
            {
                string result = Lastname;
                if (Firstname.Length > 0)
                {
                    result += $" {Firstname[0]}.";
                    if (Secondname.Length > 0)
                        result += $" {Secondname[0]}.";
                }

                return result;
            }
        }

        #endregion

        #endregion
    }
}
