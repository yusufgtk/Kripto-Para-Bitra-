using Bitra.Extensions;
using Bitra.Hubs;
using Bitra.Services;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});
builder.Services.ConfigureAddDbContext(builder.Configuration);
builder.Services.ConfigureAddRepository();
builder.Services.ConfigureAddBusiness();

builder.Services.AddSignalR();
builder.Services.AddHostedService<PeerClientService>();
builder.Services.AddHostedService<PriceUpdateService>();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//hubs
app.MapHub<BlockchainHub>("/blockchainhub");

app.Run();


