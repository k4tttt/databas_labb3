using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Labb3.Models
{
	public class GenreMethods
	{
		public GenreMethods()
		{
		}

        public IConfigurationRoot GetConnection()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();
            return builder;
        }

        public List<GenreDetail> GetGenreList(out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);
            String sqlstring = "SELECT * FROM [Tbl_Genre]";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            SqlDataAdapter adapter = new SqlDataAdapter(dbCommand);
            DataSet dataSet = new DataSet();

            List<GenreDetail> GenreList = new List<GenreDetail>();

            try
            {
                dbConnection.Open();

                adapter.Fill(dataSet, "genre");
                int count = 0;
                int i = 0;
                count = dataSet.Tables["genre"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        GenreDetail gd = new GenreDetail();
                        gd.Genre = dataSet.Tables["genre"].Rows[i]["Ge_Name"].ToString();
                        gd.Id = Convert.ToInt16(dataSet.Tables["genre"].Rows[i]["Ge_Id"]);
                        i++;
                        GenreList.Add(gd);
                    }
                    errormsg = "";
                    return GenreList;
                }
                else
                {
                    errormsg = "No person is selected";
                    return null;
                }
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }
    }
}

