using Dapper;
using System.Data.SqlClient;
using UXComex_challenge.Application.Interfaces;
using UXComex_challenge.Domain.Entities;
using Microsoft.Extensions.Configuration;


namespace UXComex_challenge.Application.Repository
{
    public class ClientRepository : IRepository<Client>
    {
        private readonly string _connectionString;

        public ClientRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public List<Client> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "SELECT Id, Name, Email, PhoneNumber FROM Clients";

                var clients = connection.Query<Client>(sql).ToList();

                return clients;
            }
        }
        public Client GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "SELECT Id, Name, Email, PhoneNumber FROM Clients WHERE Id = @Id";

                var client = connection.QueryFirstOrDefault<Client>(sql, new { Id = id });

                return client;
            }
        }

        public int insert(Client client)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = @"
                    INSERT INTO Clients (Name, Email, PhoneNumber)
                    VALUES (@Name, @Email, @PhoneNumber);
                    SELECT CAST(SCOPE_IDENTITY() as int);
                ";

                int newId = connection.ExecuteScalar<int>(sql, new
                {
                    client.Name,
                    client.Email,
                    client.PhoneNumber
                });

                return newId;
            }
        }
        public bool Update(Client client)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = @"
                    UPDATE Clients
                    SET Name = @Name,
                        Email = @Email,
                        PhoneNumber = @PhoneNumber
                    WHERE Id = @Id;
                ";

                int rowsAffected = connection.Execute(sql, new
                {
                    client.Name,
                    client.Email,
                    client.PhoneNumber,
                    client.Id
                });

                return rowsAffected > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = "DELETE FROM Clients WHERE Id = @Id;";

                int rowsAffected = connection.Execute(sql, new { Id = id });

                return rowsAffected > 0;
            }
        }
    }
}

