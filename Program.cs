using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;
using Microsoft.Extensions.Configuration;

Console.Title = "Flashcards";

string currentDirectory = Directory.GetCurrentDirectory();

string projectDirectory = Path.Combine(currentDirectory, @"..\..\..");
string appSettingsPath = Path.Combine(projectDirectory, "Properties");

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"{appSettingsPath}\\appsettings.json", optional: true, reloadOnChange: true)
    .Build();

string connectionString = configuration.GetConnectionString("connection");
