using Dapper;
using GraphQL;
using GraphQL.Types;
using System.Data.SqlClient;
using UserService.Models;
using UserService.Services;
namespace UserService.Helper
{
    public class DynamicGraphQLQuery : ObjectGraphType
    {
        public DynamicGraphQLQuery(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            FieldAsync<ListGraphType<UserType>>(
                        "users",
                        arguments: new QueryArguments(
                            new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "sql" },
                             new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "Name" }
                        ),
                        resolve: async context =>
                        {
                            var query = context.GetArgument<string>("sql");

                            // Allow only SELECT queries for security
                            if (!query.TrimStart().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                            {
                                throw new ExecutionError("Only SELECT queries are allowed.");
                            }

                            using var connection = new SqlConnection(connectionString);
                            return await connection.QueryAsync<ViewUserModel>(query);
                        }
                    );
        }
    }
}
