    using System.Data.SqlClient;
    using Dapper;
    using UXComex_challenge.Application.Interfaces;
    using UXComex_challenge.Domain.Entities;

    namespace UXComex_challenge.Application.Repository
    {
        public class ProductRepository : IRepository<Product>
        {
            private readonly string _connectionString;

            public ProductRepository(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("DefaultConnection");
            }

            public List<Product> GetAll()
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string sql = "SELECT Id, Name, Description, Price, Quantity FROM Products";

                    var products = connection.Query<Product>(sql).ToList();

                    return products;
                }
            }
            public Product GetById(int id)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string sql = "SELECT Id, Name, Description, Price, Quantity FROM Products WHERE Id = @Id";

                    var product = connection.QueryFirstOrDefault<Product>(sql, new { Id = id });

                    return product;
                }
            }


        public int insert(Product product)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var sql = @"
                        INSERT INTO Products (Name, Description, Price, Quantity)
                        VALUES (@Name, @Description, @Price, @Quantity);
                        SELECT CAST(SCOPE_IDENTITY() as int);
                    ";

                    int newId = connection.ExecuteScalar<int>(sql, new
                    {
                        product.Name,
                        product.Description,
                        product.Price,
                        product.Quantity
                    });

                    return newId;
                }
            }

            public bool Update(Product product)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var sql = @"
                        UPDATE Products
                        SET Name = @Name,
                            Description = @Description,
                            Price = @Price,
                            Quantity = @Quantity
                        WHERE Id = @Id;
                    ";

                    int rowsAffected = connection.Execute(sql, new
                    {
                        product.Id,
                        product.Name,
                        product.Description,
                        product.Price,
                        product.Quantity
                    });

                    return rowsAffected > 0;
                }
            }

            public bool Delete(int id)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var sql = "DELETE FROM Products WHERE Id = @Id;";

                    int rowsAffected = connection.Execute(sql, new { Id = id });

                    return rowsAffected > 0;
                }
            }
        }
    }
