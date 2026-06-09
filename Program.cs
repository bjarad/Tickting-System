using System.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TicketingSystem.Data;
using TicketingSystem.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options=>options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<TokenService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                                  .AddJwtBearer(options=>
                                  {
                                      options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                                      {
                                          ValidateIssuer = true,
                                          ValidateAudience = true,
                                          ValidateLifetime = true,
                                          ValidateIssuerSigningKey = true,
                                          ValidIssuer = builder.Configuration["JWT:Issuer"],
                                          ValidAudience = builder.Configuration["JWT:Audience"],
                                          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:key"]))
                                      };
                                  });
var app = builder.Build();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();