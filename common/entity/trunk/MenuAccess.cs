using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EXE_IT.Common.Entity
{
    public class MenuAccess : EntityBase
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public MenuAccess()
        {
            Css = string.Empty;
            DisplayName = string.Empty;
            Name = string.Empty;
            PageUrl = string.Empty;
            ContentType = 0;
            MenuType = 0;
            MenuAccessLevel = 0;
            SiteAccessLevel = 0;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reader">Data reader containing a row holding data on this MenuAccess item</param>
        public MenuAccess(IDataReader reader) : base(reader)
        {
            Css = string.Empty;
            DisplayName = string.Empty;
            Name = string.Empty;
            PageUrl = string.Empty;
            ContentType = 0;
            MenuType = 0;
            MenuAccessLevel = 0;
            SiteAccessLevel = 0;
            int column = reader.GetOrdinal("SiteAccess");
            if (!reader.IsDBNull(column))
                SiteAccessLevel = reader.GetInt32(column);

            column = reader.GetOrdinal("MenuAccess");
            if (!reader.IsDBNull(column))
                MenuAccessLevel = reader.GetInt32(column);

            column = reader.GetOrdinal("MenuType");
            if (!reader.IsDBNull(column))
                MenuType = reader.GetInt32(column);

            column = reader.GetOrdinal("ContentType");
            if (!reader.IsDBNull(column))
                ContentType = reader.GetInt32(column);

            column = reader.GetOrdinal("Name");
            if (!reader.IsDBNull(column))
                Name = reader.GetString(column);

            column = reader.GetOrdinal("DisplayName");
            if (!reader.IsDBNull(column))
                DisplayName = reader.GetString(column);

            column = reader.GetOrdinal("PageURL");
            if (!reader.IsDBNull(column))
                PageUrl = reader.GetString(column);
        }

        #endregion

        #region Properties

        public int SiteAccessLevel { get; set; }

        public int MenuAccessLevel { get; set; }

        public int MenuType { get; set; }

        public int ContentType { get; set; }

        public string PageUrl { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Css { get; set; }

        #endregion

        #region Static methods

        /// <summary>
        /// Loads all menu items appropriate to the rights of the user
        /// </summary>
        /// <returns>Dictionary keyed on ID</returns>
        public static Dictionary<int, MenuAccess> LoadAll(int siteAccess, int menuAccess, int menuType)
        {
            var result = new Dictionary<int, MenuAccess>();

            // define command
            SqlCommand command = GetSpCommand("CP_Menu_Load");

            SqlParameter param = command.Parameters.Add("UserSiteAccess", SqlDbType.Int);
            param.Value = siteAccess;
            param = command.Parameters.Add("UserMenuAccess", SqlDbType.Int);
            param.Value = menuAccess;
            param = command.Parameters.Add("MenuType", SqlDbType.Int);
            param.Value = menuType;

            // execute
            try
            {
                SqlDataReader reader = command.ExecuteReader();

                if (reader == null) return result;

                while (reader.Read())
                {
                    var obj = new MenuAccess(reader);
                    result.Add(obj.Id, obj);
                }
            }
            catch (SqlException)
            {
                throw;
            }
            finally
            {
                command.Dispose();
                command.Connection.Close();
            }

            return result;
        }

        #endregion
    }
}
