using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.MapGet("/generate-guid", () =>
{
    return Results.Ok(new GuidResponse { Guid = Guid.NewGuid() });
})
.WithName("GenerateGuid").Produces<GuidResponse>(StatusCodes.Status200OK);

app.MapPost("/hash-md5", (MD5HashRequest request) =>
{
    if (request.Text == null) return Results.BadRequest();
    var response = new MD5HashResponse();
    using (var md5Hash = MD5.Create())
    {
        var hashBytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(request.Text));
        response.MD5Hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
    }
    return Results.Ok(response);
})
.WithName("GetMD5Hash")
.Produces<MD5HashResponse>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest);

app.MapPost("/hash-sha512", (SHA512HashRequest request) =>
{
    if (request.Text == null) return Results.BadRequest();
    var response = new SHA512HashResponse();
    using (var sha512Hash = SHA512.Create())
    {
        var hashBytes = sha512Hash.ComputeHash(Encoding.UTF8.GetBytes(request.Text));
        response.SHA512Hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
    }
    return Results.Ok(response);
})
.WithName("GetSHA512Hash")
.Produces<SHA512HashResponse>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest);

app.Run();