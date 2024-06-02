using Microsoft.EntityFrameworkCore;
using FirebirdSql.EntityFrameworkCore.Firebird;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace pots.Models
{
    public class LidModel
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public float DIAMETER { get; set; }
        public string IMAGE { get; set; }

        private string connectionString = string.Empty;
        private IConfiguration _configuration;

        public LidModel() { }

        public LidModel(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration["ConnectionStrings:DefaultConnection"];
        }

        public List<LidModel> GetLids()
        {
            var lids = new List<LidModel>();

            using (var connection = new FbConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Execute a sample query
                    using (var command = new FbCommand("SELECT ID, NAME, DIAMETER, IMAGE FROM LID", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var lid = new LidModel(_configuration)
                                {
                                    ID = reader.GetInt32(0),
                                    NAME = reader.GetString(1),
                                    DIAMETER = reader.GetFloat(2),
                                    IMAGE = reader.IsDBNull(3) ? null : reader.GetString(3)
                                };

                                lids.Add(lid);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return lids;
        }

        public void AddLid(LidModel newLid)
        {
            using (var connection = new FbConnection(connectionString))
            {
                connection.Open();

                string insertSql = "INSERT INTO LID (NAME, DIAMETER) VALUES (@name, @diameter)";

                using (var command = new FbCommand(insertSql, connection))
                {
                    command.Parameters.AddWithValue("@name", newLid.NAME);
                    command.Parameters.AddWithValue("@diameter", newLid.DIAMETER);

                    command.ExecuteNonQuery();
                }
            }
        }

        public LidModel GetLid(int id)
        {
            LidModel lid = null;

            using (var connection = new FbConnection(connectionString))
            {
                connection.Open();

                string selectSql = "SELECT ID, NAME, DIAMETER, IMAGE FROM LID WHERE ID = @id";

                using (var command = new FbCommand(selectSql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lid = new LidModel(_configuration)
                            {
                                ID = reader.GetInt32(0),
                                NAME = reader.GetString(1),
                                DIAMETER = reader.GetFloat(2),
                                IMAGE = reader.IsDBNull(3) ? null : reader.GetString(3)
                            };
                        }
                    }
                }
            }

            return lid;
        }

        public void UpdateLid(LidModel lid)
        {
            using (var connection = new FbConnection(connectionString))
            {
                connection.Open();

                string updateSql = "UPDATE LID SET NAME = @name, DIAMETER = @diameter WHERE ID = @id";

                using (var command = new FbCommand(updateSql, connection))
                {
                    command.Parameters.AddWithValue("@name", lid.NAME);
                    command.Parameters.AddWithValue("@diameter", lid.DIAMETER);
                    command.Parameters.AddWithValue("@id", lid.ID);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteLid(int id)
        {
            using (var connection = new FbConnection(connectionString))
            {
                connection.Open();

                string deleteSql = "DELETE FROM LID WHERE ID = @id";

                using (var command = new FbCommand(deleteSql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<LidModel> GetCompatibleLids(float diameter)
        {
            var lids = new List<LidModel>();

            using (var connection = new FbConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Execute a sample query
                    using (var command = new FbCommand("SELECT ID, NAME, DIAMETER, IMAGE FROM LID WHERE DIAMETER = @diameter", connection))
                    {
                        command.Parameters.AddWithValue("@diameter", diameter);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var lid = new LidModel(_configuration)
                                {
                                    ID = reader.GetInt32(0),
                                    NAME = reader.GetString(1),
                                    DIAMETER = reader.GetFloat(2),
                                    IMAGE = reader.IsDBNull(3) ? null : reader.GetString(3)
                                };

                                lids.Add(lid);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return lids;
        }


    }
}
