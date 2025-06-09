using System.Data.SqlClient;
using Dapper;
using UXComex_challenge.Application.Interfaces;
using UXComex_challenge.Domain.Entities;
using UXComex_challenge.Domain.ObjectValues;

namespace UXComex_challenge.Application.Repository
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Order> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "SELECT Id, ClientId, CreatedAt, Total, Status FROM Orders";

                var orders = connection.Query<Order>(sql).ToList();

                return orders;
            }
        }

        public Order GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlOrder = @"
                    SELECT Id, ClientId, CreatedAt, Total, Status
                    FROM Orders
                    WHERE Id = @Id";

                var order = connection.QueryFirstOrDefault<Order>(sqlOrder, new { Id = id });

                if (order == null)
                    return null;

                string sqlItems = @"
                    SELECT OrderId, ProductId, Quantity, UnitPrice
                    FROM OrderItems
                    WHERE OrderId = @OrderId";

                var items = connection.Query<OrderItem>(sqlItems, new { OrderId = order.Id }).ToList();

                order.Items = items;

                string sqlClient = "SELECT Id, Name, Email, PhoneNumber FROM Clients WHERE Id = @Id";

                var client = connection.Query<Client>(sqlClient, new { Id = order.ClientId }).FirstOrDefault();

                order.Client = client;

                return order;
            }
        }


        public int insert(Order order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = @"
                    INSERT INTO Orders (ClientId, CreatedAt, Total, Status)
                    VALUES (@ClientId, @CreatedAt, @Total, @Status);
                    SELECT CAST(SCOPE_IDENTITY() as int);
                ";

                int newId = connection.ExecuteScalar<int>(sql, new
                {
                    order.ClientId,
                    order.CreatedAt,
                    order.Total,
                    order.Status
                });

                return newId;
            }
        }

        public bool Update(Order order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = @"
                    UPDATE Orders
                    SET ClientId = @ClientId,
                        CreatedAt = @CreatedAt,
                        Total = @Total,
                        Status = @Status
                    WHERE Id = @Id;
                ";

                int rowsAffected = connection.Execute(sql, new
                {
                    order.Id,
                    order.ClientId,
                    order.CreatedAt,
                    order.Total,
                    order.Status
                });

                return rowsAffected > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = "DELETE FROM Orders WHERE Id = @Id;";

                int rowsAffected = connection.Execute(sql, new { Id = id });

                return rowsAffected > 0;
            }
        }
    }
}
