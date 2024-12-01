namespace UserService.Middlewere
{
    public class GraphQLMetricsMiddleware
    {
        private readonly RequestDelegate _next;
        public GraphQLMetricsMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var startTime = DateTime.UtcNow;

                await _next(context);

                var elapsedTime = DateTime.UtcNow - startTime;

                // Log execution time (you can replace this with a custom logging library)
                Console.WriteLine($"GraphQL request executed in {elapsedTime.TotalMilliseconds} ms");
            }catch(Exception ex)
            {

            }
        }
    }
}
