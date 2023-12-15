using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Labb3.Models
{
	public class PersonMethods
	{
		public PersonMethods()
		{
		}

		public IConfigurationRoot GetConnection()
		{
			var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();
			return builder;
		}

		public int InsertPerson(PersonDetail pd, out string errormsg)
		{
			SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);
			String sqlstring = "INSERT INTO [Tbl_Person]([Pe_Name], [Pe_Age], [Pe_FavSong]) VALUES (@name, @age, @favsong)";
			SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("name", System.Data.SqlDbType.NVarChar, 30).Value = pd.Name;
            dbCommand.Parameters.Add("age", System.Data.SqlDbType.Int).Value = pd.Age;
            dbCommand.Parameters.Add("favsong", System.Data.SqlDbType.Int).Value = pd.FavSongId;

			errormsg = "";

			try
			{
				dbConnection.Open();
				int i = 0;
				i = dbCommand.ExecuteNonQuery();
				if (i == 1) { errormsg = ""; }
				else { errormsg = "The person is not created in the database"; }
				return (i);
			}
			catch (Exception e)
			{
				errormsg = e.Message;
				return 0;
			}
			finally
			{
				dbConnection.Close();
			}
        }

        public List<PersonDetail> GetPersonWithDataSet(out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);
            String sqlstring = "SELECT * FROM [Tbl_Person] INNER JOIN [Tbl_Song] ON [Tbl_Person].[Pe_FavSong] = [Tbl_Song].[So_Id]";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

			SqlDataAdapter adapter = new SqlDataAdapter(dbCommand);
			DataSet dataSet = new DataSet();

            List<PersonDetail> PersonList = new List<PersonDetail>();

            try
            {
                dbConnection.Open();

				adapter.Fill(dataSet, "person");
				int count = 0;
				int i = 0;
				count = dataSet.Tables["person"].Rows.Count;

                if (count > 0)
				{
					while (i < count)
					{
						PersonDetail pd = new PersonDetail();
						pd.Id = Convert.ToInt16(dataSet.Tables["person"].Rows[i]["Pe_Id"]);
						pd.Name = dataSet.Tables["person"].Rows[i]["Pe_Name"].ToString();
                        pd.Age = Convert.ToInt16(dataSet.Tables["person"].Rows[i]["Pe_Age"]);
                        pd.FavSongId = Convert.ToInt16(dataSet.Tables["person"].Rows[i]["Pe_FavSong"]);
                        pd.FavSong = dataSet.Tables["person"].Rows[i]["So_Name"].ToString();
                        pd.Artist = dataSet.Tables["person"].Rows[i]["So_Artist"].ToString();
                        i++;
						PersonList.Add(pd);
                    }
					errormsg = "";
					return PersonList;
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

        public int EditPerson(PersonDetail pd, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);
            String sqlstring = "UPDATE [Tbl_Person] SET [Pe_Name] = @name, [Pe_Age] = @age, [Pe_FavSong] = @favsong WHERE [Pe_Id] = @id";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("name", System.Data.SqlDbType.NVarChar, 30).Value = pd.Name;
            dbCommand.Parameters.Add("age", System.Data.SqlDbType.Int).Value = pd.Age;
            dbCommand.Parameters.Add("favsong", System.Data.SqlDbType.Int).Value = pd.FavSongId;
            dbCommand.Parameters.Add("id", System.Data.SqlDbType.Int).Value = pd.Id;

            errormsg = "";

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "The person is not edited in the database"; }
                return (i);
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public int DeletePerson(PersonDetail pd, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);
            String sqlstring = "DELETE FROM [Tbl_Person] WHERE [Pe_Id] = @id";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("id", System.Data.SqlDbType.Int).Value = pd.Id;

            errormsg = "";

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "The person is not deleted from the database"; }
                return (i);
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public List<PersonGenres> GetGenresWithDataSet(out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);
            String sqlstring = "SELECT * FROM [Tbl_Person] INNER JOIN [Tbl_Song] ON [Tbl_Person].[Pe_FavSong] = [Tbl_Song].[So_Id] INNER JOIN [Tbl_PersonGenre] ON [Tbl_Person].[Pe_Id] = [Tbl_PersonGenre].[Pe_Id] INNER JOIN [Tbl_Genre] ON [Tbl_PersonGenre].[Ge_Id] = [Tbl_Genre].[Ge_Id]";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            SqlDataAdapter adapter = new SqlDataAdapter(dbCommand);
            DataSet dataSet = new DataSet();

            List<PersonGenres> GenreList = new List<PersonGenres>();

            try
            {
                dbConnection.Open();

                adapter.Fill(dataSet, "person");
                int count = 0;
                int i = 0;
                count = dataSet.Tables["person"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        PersonGenres pg = new PersonGenres();
                        pg.Id = Convert.ToInt16(dataSet.Tables["person"].Rows[i]["Pe_Id"]);
                        pg.Name = dataSet.Tables["person"].Rows[i]["Pe_Name"].ToString();
                        pg.Age = Convert.ToInt16(dataSet.Tables["person"].Rows[i]["Pe_Age"]);
                        pg.Genre = dataSet.Tables["person"].Rows[i]["Ge_Name"].ToString();
                        i++;
                        GenreList.Add(pg);
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

        public List<PersonGenres> GetGenresWithDataSet(out string errormsg, int filterId)
        {
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);
            String sqlstring = "SELECT * FROM [Tbl_Person] INNER JOIN [Tbl_Song] ON [Tbl_Person].[Pe_FavSong] = [Tbl_Song].[So_Id] INNER JOIN [Tbl_PersonGenre] ON [Tbl_Person].[Pe_Id] = [Tbl_PersonGenre].[Pe_Id] INNER JOIN [Tbl_Genre] ON [Tbl_PersonGenre].[Ge_Id] = [Tbl_Genre].[Ge_Id] WHERE [Tbl_Genre].[Ge_Id] = @filterId";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("filterId", System.Data.SqlDbType.Int).Value = filterId;

            SqlDataAdapter adapter = new SqlDataAdapter(dbCommand);
            DataSet dataSet = new DataSet();

            List<PersonGenres> GenreList = new List<PersonGenres>();

            try
            {
                dbConnection.Open();

                adapter.Fill(dataSet, "person");
                int count = 0;
                int i = 0;
                count = dataSet.Tables["person"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        PersonGenres pg = new PersonGenres();
                        pg.Id = Convert.ToInt16(dataSet.Tables["person"].Rows[i]["Pe_Id"]);
                        pg.Name = dataSet.Tables["person"].Rows[i]["Pe_Name"].ToString();
                        pg.Age = Convert.ToInt16(dataSet.Tables["person"].Rows[i]["Pe_Age"]);
                        pg.Genre = dataSet.Tables["person"].Rows[i]["Ge_Name"].ToString();
                        i++;
                        GenreList.Add(pg);
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

        public PersonDetail GetPerson(int id, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);
            String sqlstring = "SELECT * FROM [Tbl_Person] INNER JOIN [Tbl_Song] ON [Tbl_Person].[Pe_FavSong] = [Tbl_Song].[So_Id] INNER JOIN [Tbl_PersonGenre] ON [Tbl_Person].[Pe_Id] = [Tbl_PersonGenre].[Pe_Id] INNER JOIN [Tbl_Genre] ON [Tbl_PersonGenre].[Ge_Id] = [Tbl_Genre].[Ge_Id] WHERE [Tbl_Person].[Pe_Id] = @id";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("id", System.Data.SqlDbType.Int).Value = id;

            SqlDataAdapter adapter = new SqlDataAdapter(dbCommand);
            DataSet dataSet = new DataSet();

            PersonDetail person = new PersonDetail();

            try
            {
                dbConnection.Open();

                adapter.Fill(dataSet, "person");
                int count = 0;
                int i = 0;
                count = dataSet.Tables["person"].Rows.Count;

                if (count > 0)
                {
                    person.Id = Convert.ToInt16(dataSet.Tables["person"].Rows[i]["Pe_Id"]);
                    person.Name = dataSet.Tables["person"].Rows[i]["Pe_Name"].ToString();
                    person.Age = Convert.ToInt16(dataSet.Tables["person"].Rows[i]["Pe_Age"]);
                    person.FavSongId = Convert.ToInt16(dataSet.Tables["person"].Rows[i]["Pe_FavSong"]);
                    person.FavSong = dataSet.Tables["person"].Rows[i]["So_Name"].ToString();
                    person.Artist = dataSet.Tables["person"].Rows[i]["So_Artist"].ToString();
                    errormsg = "";
                    return person;
                }
                else
                {
                    errormsg = "No person is fetched";
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

