using FirebirdSql.Data.FirebirdClient;

namespace pots.Models
{
    public class PotModel
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public float DIAMETER { get; set; }
        public float VOLUME { get; set; }
        public string IMAGE { get; set; }
        private ImageModel _imageModel;


        private string connectionString = string.Empty;
        private IConfiguration _configuration;

        public PotModel() { }
        public PotModel(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            _imageModel = new ImageModel();
        }


        // Returns a list of all pots in the database
        public List<PotModel> GetPots()
        {
            var pots = new List<PotModel>();

            using (var connection = new FbConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (var command = new FbCommand("SELECT ID, NAME, DIAMETER, VOLUME, IMAGE FROM POT", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var pot = new PotModel(_configuration)
                                {
                                    ID = reader.GetInt32(0),
                                    NAME = reader.GetString(1),
                                    DIAMETER = reader.GetFloat(2),
                                    VOLUME = reader.GetFloat(3),
                                    IMAGE = reader.IsDBNull(4) ? null : reader.GetString(4)
                                };

                                pots.Add(pot);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return pots;
        }


        // Adds a new pot to the database. If an image is provided, it is saved and its path is stored in the IMAGE property of the pot
        public void AddPot(PotModel newPot, IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                newPot.IMAGE = _imageModel.SaveImage(image);
            }

            using (var connection = new FbConnection(connectionString))
            {
                connection.Open();

                string insertSql = "INSERT INTO POT (NAME, DIAMETER, VOLUME, IMAGE) VALUES (@name, @diameter, @volume, @image)";

                using (var command = new FbCommand(insertSql, connection))
                {
                    command.Parameters.AddWithValue("@name", newPot.NAME);
                    command.Parameters.AddWithValue("@diameter", newPot.DIAMETER);
                    command.Parameters.AddWithValue("@volume", newPot.VOLUME);
                    command.Parameters.AddWithValue("@image", newPot.IMAGE);

                    command.ExecuteNonQuery();
                }
            }
        }


        // Returns a PotModel object representing the pot with the given ID from the database
        public PotModel GetPot(int id)
        {
            PotModel pot = null;

            using (var connection = new FbConnection(connectionString))
            {
                connection.Open();

                string selectSql = "SELECT ID, NAME, DIAMETER, VOLUME, IMAGE FROM POT WHERE ID = @id";

                using (var command = new FbCommand(selectSql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            pot = new PotModel(_configuration)
                            {
                                ID = reader.GetInt32(0),
                                NAME = reader.GetString(1),
                                DIAMETER = reader.GetFloat(2),
                                VOLUME = reader.GetFloat(3),
                                IMAGE = reader.IsDBNull(4) ? null : reader.GetString(4)
                            };
                        }
                    }
                }
            }

            return pot;
        }


        // Updates the details of a pot in the database. If an image is provided, it is saved and its path is stored in the IMAGE property of the pot
        public void UpdatePot(PotModel pot, IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                pot.IMAGE = _imageModel.SaveImage(image);
            }

            using (var connection = new FbConnection(connectionString))
            {
                connection.Open();

                string updateSql;

                if (!string.IsNullOrEmpty(pot.IMAGE))
                {
                    updateSql = "UPDATE POT SET NAME = @name, DIAMETER = @diameter, VOLUME = @volume, IMAGE = @image WHERE ID = @id";
                }
                else
                {
                    updateSql = "UPDATE POT SET NAME = @name, DIAMETER = @diameter, VOLUME = @volume WHERE ID = @id";
                }

                using (var command = new FbCommand(updateSql, connection))
                {
                    command.Parameters.AddWithValue("@name", pot.NAME);
                    command.Parameters.AddWithValue("@diameter", pot.DIAMETER);
                    command.Parameters.AddWithValue("@volume", pot.VOLUME);
                    if (!string.IsNullOrEmpty(pot.IMAGE))
                    {
                        command.Parameters.AddWithValue("@image", pot.IMAGE);
                    }
                    command.Parameters.AddWithValue("@id", pot.ID);

                    command.ExecuteNonQuery();
                }
            }
        }


        // Deletes the pot with the given ID from the database. If the pot has an image, the image is also deleted
        public void DeletePot(int id)
        {
            PotModel pot = GetPot(id);

            if (!string.IsNullOrEmpty(pot.IMAGE))
            {
                _imageModel.DeleteImage(pot.IMAGE);
            }

            using (var connection = new FbConnection(connectionString))
            {
                connection.Open();

                string deleteSql = "DELETE FROM POT WHERE ID = @id";

                using (var command = new FbCommand(deleteSql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
