var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "RemotePC Add-on API is running.");

// Google Meet Add-on expects a POST to this endpoint
app.MapPost("/createConference", async (HttpContext context) =>
{
    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
    context.Response.ContentType = "application/json";

    using var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync();

    var response = new
    {
        conferenceSolution = new
        {
            key = new { type = "REMOTE_PC" },
            name = "RemotePC Session",
            iconUri = "https://png.pngtree.com/element_our/png/20181229/vector-chat-icon-png_302635.jpg"
        },
        conferenceId = Guid.NewGuid().ToString().Substring(0, 8),
        entryPoints = new[]
        {
            new {
                entryPointType = "video",
                uri = "remotepc://launch?id=123456",
                label = "Join RemotePC Session"
            }
        }
    };

    await context.Response.WriteAsJsonAsync(response);
});

app.Run();
