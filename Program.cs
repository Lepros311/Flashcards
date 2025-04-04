using Microsoft.Data.SqlClient;
using System.Configuration;

Console.Title = "Flashcards";

string connection = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

var repository = new CodingSessionRepository(connection);

repository.CreateTable();
repository.SeedDatabase();
