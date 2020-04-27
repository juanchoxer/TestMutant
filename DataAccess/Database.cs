using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApi.Controllers;
using WebApi.Models;

namespace WebApi.DataAccess
{
    public class Database
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ToString();

        public void AddMutant(Mutant mutant)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();

                    if (IsNewDNA(mutant.dna, con))
                    {
                        InsertNewDNA(mutant, con);
                    }
                    
                    con.Close();
                }
                catch (Exception ex)
                {
                    var x = ex;
                }
            }
        }

        private bool IsNewDNA(string dna, SqlConnection con)
        {
            bool isNewDNA = true;

            string stringCommand = "SELECT top 1 dna FROM TestDNA where dna = @param1)";
            using (SqlCommand cmd = new SqlCommand(stringCommand, con))
            {
                cmd.Parameters.Add("@param1", SqlDbType.VarChar, 100).Value = dna;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isNewDNA = false;
                    }
                }
            }
            return isNewDNA;
        }

        private void InsertNewDNA(Mutant mutant, SqlConnection con)
        {
            string stringCommand = "INSERT INTO TestDNA(dna,isMutant) VALUES(@param1,@param2)";
            using (SqlCommand cmd = new SqlCommand(stringCommand, con))
            {
                cmd.Parameters.Add("@param1", SqlDbType.VarChar, 100).Value = mutant.dna;
                cmd.Parameters.Add("@param2", SqlDbType.Bit).Value = mutant.isMutant;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
        }

        public Response GetStats()
        {
            Response response = new Response();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();

                    response = GetStatsFromDB(con);
                    response.ratio = response.count_mutant_dna / response.count_human_dna;
                        
                    con.Close();
                }
                catch (Exception ex)
                {
                    var x = ex;
                }
            }
            return response;
        }

        private Response GetStatsFromDB(SqlConnection con)
        {
            Response response = new Response();

            string stringCommand = "SELECT isMutant FROM TestDNA";
            using (SqlCommand cmd = new SqlCommand(stringCommand, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if ((bool)reader[0])
                        {
                            response.count_mutant_dna++;
                        }
                        else
                        {
                            response.count_mutant_dna++;
                        }
                    }
                    
                }
            }
            return response;
        }
    }
}