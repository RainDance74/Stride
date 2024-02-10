using Stride.Application;
using Stride.Infrastructure;
using Stride.Infrastructure.Data;
using Stride.Infrastructure.Identity;
using Stride.Web;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapFallbackToFile("index.html");

app.UseExceptionHandler(opt => { });

app.Map("/", () => Results.Redirect("/api"));

app.UseCors();

app.MapControllers();

app.MapGroup("/api/Users")
   .WithTags("Users")
   .MapIdentityApi<ApplicationUser>();

app.Run();

public partial class Program { }