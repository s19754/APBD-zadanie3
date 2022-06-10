using Cw4.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw4.Services;
using System.Data.SqlClient;

namespace Cw4.Services
{
    public class DbServices : IDbServices
    {
        static string connectionPath = @"Data Source=db-mssql16;Initial Catalog=s19754;Integrated Security=True";

        public IEnumerable<Animal> GetAnimals(string orderBy = "name")
        {
           
                var res = new List<Animal>();
                string queryString = $"SELECT * FROM Animal ORDER BY {orderBy} ASC";

                using (SqlConnection con = new SqlConnection(connectionPath))
                {
                    con.Open();
                    SqlCommand com = new SqlCommand(queryString, con);

                    SqlDataReader reader = com.ExecuteReader();

                    while (reader.Read())
                    {
                        res.Add(new Animal
                        {
                            IdAnimal = int.Parse(reader["idAnimal"].ToString()),
                            Name = reader["name"].ToString(),
                            Description = reader["description"].ToString(),
                            Category = reader["category"].ToString(),
                            Area = reader["area"].ToString(),
                        }); ;
                    }
                    reader.Close();
                }
                return res;
            
        }


        string[] IDbServices.AddAnimal(Animal animal)
        {
            
            string queryString = "INSERT INTO Animal (name, description, category, area)" +
                                 "VALUES (" +
                                 "'" + animal.Name + "'" + "," +
                                 "'" + animal.Description + "'" + "," +
                                 "'" + animal.Category + "'" + "," +
                                 "'" + animal.Area + "'" +
                                 ")";

            using (SqlConnection con = new SqlConnection(connectionPath))
            {
                string[] res = { "Ok", "New animal is added" };
                try
                {
                    SqlCommand com = new SqlCommand(queryString, con);
                    con.Open();
                    com.ExecuteNonQuery();
                    return res;
                }
                catch (SqlException x)
                {
                    res[0] = "Error";
                    res[1] = "Error during query execution" + x.Message;
                    return res;
                }
                catch (Exception xs)
                {
                    res[0] = "Error";
                    res[1] = "Connection Error" + xs.Message;
                    return res;
                }
                finally
                {
                    con.Close();
                }


                }
        }

        string[] IDbServices.EditAnimal(Animal animal, int idAnimal)
        {
            string queryString = "UPDATE Animal " +
                                 "SET " +
                                 "name = '" + animal.Name + "'" + "," +
                                 "description = '" + animal.Description + "'" + "," +
                                 "category = '" + animal.Category + "'" + "," +
                                 "area = '" + animal.Area + "' " +
                                 "WHERE idAnimal = " + idAnimal;

            using (SqlConnection con = new SqlConnection(connectionPath))
            {
                string[] res = { "Ok", $"Animal {idAnimal} is edited" };
                try
                {
                    SqlCommand com = new SqlCommand(queryString, con);
                    con.Open();
                    com.ExecuteNonQuery();
                }
                catch (SqlException x)
                {
                    res[0] = "Error";
                    res[1] = "Error during query execution" + x.Message;
                    
                }
                catch (Exception xs)
                {
                    res[0] = "Error";
                    res[1] = "Connection Error" + xs.Message;
                   
                }
                finally
                {
                    con.Close();
                }
                return res;


            }
        }

        string[] IDbServices.DeleteAnimal(int idAnimal)
        {
            using (SqlConnection con = new SqlConnection(connectionPath))
            {
                string queryString = "DELETE FROM Animal " +
                                 "WHERE " +
                                 "idAnimal = " + idAnimal;
                int rowAffected = 0;

                string[] res = { "Ok", $"Animal {idAnimal} is deleted" };
                try
                {
                    SqlCommand com = new SqlCommand(queryString, con);
                    con.Open();
                    rowAffected = com.ExecuteNonQuery();
                    if (!Convert.ToBoolean(rowAffected))
                    {
                        res[0] = "File Not Found";
                        res[1] = $"Animal with id { Convert.ToString(idAnimal)} does not exist";
                    }

                    return res;
                }
                catch (SqlException x)
                {
                    res[0] = "Error";
                    res[1] = "Error during query execution" + x.Message;
                    return res;
                }
                catch (Exception xs)
                {
                    res[0] = "Error";
                    res[1] = "Connection Error" + xs.Message;

                }
                finally
                {
                    con.Close();
                }
                return res;
            }
        }
    }
}
