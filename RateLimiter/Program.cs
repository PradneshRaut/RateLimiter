using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

const int REQUEST_LIMIT_PER_MINUTE = 5;

builder.Services.AddRateLimiter(options =>
{ 
    options.AddPolicy("per-user-policy", context =>
    { 
        var userKey =
            context.Request.Headers["X-Api-Key"].FirstOrDefault()
            ?? context.Connection.RemoteIpAddress?.ToString()
            ?? "anonymous";

        // Sliding window - rolling 1 minute per user
        return RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: userKey,
            factory: _ => new SlidingWindowRateLimiterOptions
            {
                PermitLimit = REQUEST_LIMIT_PER_MINUTE,
                Window = TimeSpan.FromMinutes(1),
                SegmentsPerWindow = 6,
                QueueLimit = 0,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            });
    });

    // Blocked response
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            allowed = false,
            message = "Too many requests. Please try again later."
        }, token);
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRateLimiter();    
app.UseAuthorization();

app.MapControllers();
app.Run();
