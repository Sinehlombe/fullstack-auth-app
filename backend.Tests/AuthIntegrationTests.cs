using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using backend.Data;
using backend.DTOs;

namespace backend.Tests
{
    public class AuthIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public AuthIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove real database
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    // Add in-memory database
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseInMemoryDatabase("SharedTestDb"));

                    // Reconfigure JWT to use same key as appsettings.json
                    services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                    {
                        var key = "your-super-secret-jwt-key-that-is-at-least-32-characters-long";
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = "fullstack-auth-app",
                            ValidAudience = "fullstack-auth-app-users",
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(key))
                        };
                    });
                });
            });

            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Register_ShouldReturn200_WhenValidData()
        {
            var request = new RegisterRequestDto
            {
                FirstName = "Onwaba",
                LastName = "Nobhitywana",
                Email = $"onwaba_{Guid.NewGuid()}@gmail.com",
                Password = "password123"
            };

            var response = await _client.PostAsJsonAsync("/api/auth/register", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Register_ShouldReturn400_WhenPasswordTooShort()
        {
            var request = new RegisterRequestDto
            {
                FirstName = "Onwaba",
                LastName = "Nobhitywana",
                Email = $"onwaba_{Guid.NewGuid()}@gmail.com",
                Password = "123"
            };

            var response = await _client.PostAsJsonAsync("/api/auth/register", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Login_ShouldReturn200_WhenValidCredentials()
        {
            var email = $"onwaba_{Guid.NewGuid()}@gmail.com";
            await _client.PostAsJsonAsync("/api/auth/register", new RegisterRequestDto
            {
                FirstName = "Onwaba",
                LastName = "Nobhitywana",
                Email = email,
                Password = "password123"
            });

            var response = await _client.PostAsJsonAsync("/api/auth/login", new LoginRequestDto
            {
                Email = email,
                Password = "password123"
            });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Login_ShouldReturn401_WhenInvalidCredentials()
        {
            var response = await _client.PostAsJsonAsync("/api/auth/login", new LoginRequestDto
            {
                Email = "nobody@test.com",
                Password = "wrongpassword"
            });

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetUserDetails_ShouldReturn401_WhenNoToken()
        {
            var response = await _client.GetAsync("/api/user/me");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        
        [Fact]
        public async Task GetUserDetails_ShouldReturn200_WhenValidToken()
        {
            var email = $"onwaba_{Guid.NewGuid()}@gmail.com";
            await _client.PostAsJsonAsync("/api/auth/register", new RegisterRequestDto
            {
                FirstName = "Onwaba",
                LastName = "Nobhitywana",
                Email = email,
                Password = "password123"
            });

            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new LoginRequestDto
            {
                Email = email,
                Password = "password123"
            });

            var loginData = await loginResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            var token = loginData!["token"];

            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/user/me");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
