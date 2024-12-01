using GraphQL.Types;
using UserService.Models;
using UserService.Services;

namespace UserService.Helper
{
    public class DynamicGraphQLSchema : Schema
    {
        public DynamicGraphQLSchema(IServiceProvider provider) : base(provider)
        {
            try
            {
                Query = provider.GetRequiredService<DynamicGraphQLQuery>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting up schema: {ex.Message}");
                throw;
            }
        }
    }
    public class UserType : ObjectGraphType<ViewUserModel>
    {
        public UserType()
        {
            Field(x => x.Id).Description("The ID of the user.");
            Field(x => x.FirstName).Description("The name of the user.");
            Field(x => x.EmailId).Description("The email of the user.");
        }
    }
}
