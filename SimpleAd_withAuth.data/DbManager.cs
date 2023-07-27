using System.Collections.Generic;
using System.Data.SqlClient;

namespace SimpleAd_withAuth.data
{
    public class DbManager
    {
        private string _connectionString;

        public DbManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Ad> GetAllAds()
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM Ad a
                            JOIN Lister l
                            ON l.Id = a.ListerId
                            ORDER BY DATE DESC";

            connection.Open();
            var ads = new List<Ad>();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ads.Add(new Ad
                {
                    Id = (int)reader["Id"],
                    ListerId = (int)reader["ListerId"],
                    Name = (string)reader["Name"],
                    ListingDate = (DateTime)reader["Date"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Listing = (string)reader["Listing"]
                    
                });
            }
           

            return ads;

        }

        public List<Ad> GetAllAdsForMyAccount(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM Ad a
                            JOIN Lister l
                            ON l.Id = a.ListerId
                            WHERE ListerId = @id
                            ORDER BY DATE DESC";
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            var ads = new List<Ad>();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ads.Add(new Ad
                {
                    Id = (int)reader["Id"],
                    ListerId = (int)reader["ListerId"],
                    Name = (string)reader["Name"],
                    ListingDate = (DateTime)reader["Date"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Listing = (string)reader["Listing"]

                });
            }


            return ads;

        }

        public void NewLister(Lister lister, string password)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Lister (Name, Email, Password)
                            VALUES (@name, @email, @password)";
            command.Parameters.AddWithValue("@name", lister.Name);
            command.Parameters.AddWithValue("@email", lister.Email);
            command.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.HashPassword(password));
            connection.Open();

            command.ExecuteNonQuery();
        }

        public Lister GetByEmail(string email)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM Lister
                            WHERE Email = @email";
            command.Parameters.AddWithValue("@email", email);
            connection.Open();

            var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            return new Lister
            {
                Id = (int)reader["Id"],
                Name = (string)reader["name"],
                Email = (string)reader["email"],
                Password = (string)reader["password"]
            };
        }

        public Lister Login(string email, string password)
        {
            var lister = GetByEmail(email);
            if (lister == null)
            {
                return null;
            }

            var isValid = BCrypt.Net.BCrypt.Verify(password, lister.Password);
            if (!isValid)
            {
                return null;
            }

            return lister;

        }

        public void NewAd(Ad ad)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Ad (Name, Date, PhoneNumber, Listing, ListerId)
                            VALUES (@name, @date, @cell, @listing, @ListerId)";
            command.Parameters.AddWithValue("@name", ad.Name);
            command.Parameters.AddWithValue("@date", ad.ListingDate);
            command.Parameters.AddWithValue("@cell", ad.PhoneNumber);
            command.Parameters.AddWithValue("@listing", ad.Listing);
            command.Parameters.AddWithValue("@listerId", ad.ListerId);

            connection.Open();

            command.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM Ad WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();

            command.ExecuteNonQuery();
        }
    }
}