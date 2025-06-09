using System.Data.SqlClient;
using Dapper;
using UXComex_challenge.Application.Interfaces;
using UXComex_challenge.Domain.Entities;

namespace UXComex_challenge.Application.Repository
{
    public class OrderNotificationRepository : IRepository<OrderNotification>
    {
        private readonly string _connectionString;

        public OrderNotificationRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<OrderNotification> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = @"
                    SELECT Id, OrderId, OldStatus, NewStatus, ChangedAt
                    FROM OrderNotifications";

                var notifications = connection.Query<OrderNotification>(sql).ToList();

                return notifications;
            }
        }

        public OrderNotification GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = @"
                    SELECT Id, OrderId, OldStatus, NewStatus, ChangedAt
                    FROM OrderNotifications
                    WHERE Id = @Id";

                var notification = connection.QueryFirstOrDefault<OrderNotification>(sql, new { Id = id });

                return notification;
            }
        }

        public int insert(OrderNotification notification)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = @"
                    INSERT INTO OrderNotifications (OrderId, OldStatus, NewStatus, ChangedAt)
                    VALUES (@OrderId, @OldStatus, @NewStatus, @ChangedAt);
                    SELECT CAST(SCOPE_IDENTITY() as int);
                ";

                int newId = connection.ExecuteScalar<int>(sql, notification);

                return newId;
            }
        }

        public bool Update(OrderNotification notification)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = @"
                    UPDATE OrderNotifications
                    SET OrderId = @OrderId,
                        OldStatus = @OldStatus,
                        NewStatus = @NewStatus,
                        ChangedAt = @ChangedAt
                    WHERE Id = @Id;
                ";

                int rowsAffected = connection.Execute(sql, notification);

                return rowsAffected > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "DELETE FROM OrderNotifications WHERE Id = @Id;";

                int rowsAffected = connection.Execute(sql, new { Id = id });

                return rowsAffected > 0;
            }
        }
    }
}
