var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => 
{
    x.CustomSchemaIds(n => n.FullName);
});
builder.Services.AddTransient<Handler>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost(
        "/v1/transactions", 
        (Request request, Handler handler) 
            => handler.Handle(request))
    .WithName("Transactions: Create")
    .WithSummary("Cria uma nova transação")
    .Produces<Response>();

app.Run();

// Request
public class Request
{
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int Type { get; set; }
    public decimal Amount { get; set; }
    public long CategoryId { get; set; }
    public string UserId { get; set; } = string.Empty;
}

// Response

public class Response 
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
}

// Handler
public class Handler 
{
    public Response Handle (Request request)
    {
        return new Response 
        {
            Id = 4,
            Title = request.Title,
        };
    }
}