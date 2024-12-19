using Azure.Storage.Blobs;
using EventManagement.Data.Configuration;
using EventManagement.Hubs;
using EventManagement.Middleware;
using EventManagement.Middleware.AuthorizationSetUp;
using EventManagement.Middleware.Identity;
using EventManagement.Service.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterContextDb(builder.Configuration);
//System
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddSignalR();
builder.Services.AddOptions();
builder.Services.AddSetupAuthorization();
builder.Services.AddHttpContextAccessor();

//Identity
builder.Services.SetUpIdentity();
builder.Services.AddSingleton(u => new BlobServiceClient(builder.Configuration.GetConnectionString("StorageAccount")));
//Mail
builder.Services.RegisterMailService(builder.Configuration);
//Auto mapper để map các object
builder.Services.AddAutoMapper(typeof(MappingConfig));
//Register DI
builder.Services.RegisterDIBussiness();
builder.Services.RegisterDIRepository();
//Configure JWT
builder.Services.SetUpJWT(builder.Configuration);
//Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Chỉ cho phép nguồn này
              .AllowCredentials()  // Cho phép gửi cookies và credentials
              .AllowAnyHeader()
              .AllowAnyMethod()
                .WithExposedHeaders("X-Pagination"); 
    });
});
//Use Swagger
builder.Services.AddSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("AllowLocalhost");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CustomExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<SupportChatHub>("/hubs/supportchat");

app.Run();