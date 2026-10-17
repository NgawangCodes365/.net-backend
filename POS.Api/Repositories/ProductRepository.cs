//Models describe what a product looks like
//Repository explains how we retrieve product data.

//so here we will implement the database access logic using Dapper.


using Dapper;
using Microsoft.Data.SqlClient;
using POS.Api.Models;

namespace POS.Api.Repositories
{
    public class ProductRepository
    {
        private readonly string _connectionString;
        //readonly means This field can be assigned to a constructor but cannot be reassigned afterward.

        //so we will create a constructor.
        public ProductRepository(IConfiguration configuration)
        //so what is Iconfiguration configuration: Remember we stored our connectionString in User secrets.
        // in order for us to get that connectionString from the UserSecret; IConfiguration is a interface
        //that allows us to access this app configuration.
        {
            _connectionString = configuration.GetConnectionString("POSDatabase")
            ?? throw new InvalidOperationException(
                "The POSDATABASE connection wasnot succesfully configured."
            );
        }
       //so since this is a constructor: when instance of this class will be created it will run.
        //for example : var repository = new ProductRepository(connectionString)
        //so when this occurs: the _connectionString will be initialized.
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            const string sql = @"
            SELECT
                ProductId as Id,
                ProductName as Name,
                Price
            FROM dbo.Products;
            ";
            using var connection = new SqlConnection(_connectionString);
            //here using var means: if exception occurs here --> this connection object is to be disposed when it leaves its intended scope.
            //This helps resource release properly.
            var products = await connection.QueryAsync<Product>(sql);
            return products;
        }

    }
}