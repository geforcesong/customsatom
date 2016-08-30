using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace CustomsAtomMobileSite.Models
{
    public class CustomUser : ModelBase
    {
        public int ID { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string UserRole { get; set; }

        public static CustomUser Login(string alias, string pwd)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand selectCMD = connection.CreateCommand();
            selectCMD.CommandText = "select ID, Alias, Name from [User] Where Alias=@alias and Password=@password and IsActived=1";

            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@alias", System.Data.SqlDbType.NVarChar);
            param[0].Value = alias;
            param[1] = new SqlParameter("@password", System.Data.SqlDbType.NVarChar);
            param[1].Value = pwd;

            for (int i = 0; i < param.Length; i++)
                selectCMD.Parameters.Add(param[i]);

            SqlDataReader sdr = null;
            try
            {
                connection.Open();
                sdr = selectCMD.ExecuteReader();
                CustomUser user = null;
                if (sdr.Read())
                {
                    user = new CustomUser();
                    user.ID = Convert.ToInt32(sdr["ID"]);
                    user.Alias = sdr["Alias"] == DBNull.Value ? "" : sdr["Alias"].ToString();
                    user.Name = sdr["Name"] == DBNull.Value ? "" : sdr["Name"].ToString();
                    user.UserRole = "";
                }
                return user;
            }
            catch {
                return null;
            }
            finally
            {
                if (sdr != null && !sdr.IsClosed)
                    sdr.Close();
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }
        }
    }
}