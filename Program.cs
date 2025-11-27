using MedViaApi.Services;
using Mscc.GenerativeAI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            // Allow requests from any origin (for local development)
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<IGenerativeAI>(sp =>
{
    var apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY")
          ?? builder.Configuration["Gemini:ApiKey"];

    if (string.IsNullOrEmpty(apiKey))
        throw new Exception("Gemini API key is not configured.");

    return new GoogleAI(apiKey);
});




builder.Services.AddScoped<MedService>();

var app = builder.Build();
app.UseCors();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
