using GlamBook.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ClientaDAL>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("GlamBookDB")
        ?? throw new InvalidOperationException("Connection string 'GlamBookDB' no configurada.");

    return new ClientaDAL(connectionString);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
