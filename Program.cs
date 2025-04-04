using Flashcards.Controller;
using Flashcards.Model;
using Flashcards.View;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

Console.Title = "Flashcards";

string currentDirectory = Directory.GetCurrentDirectory();

string projectDirectory = Path.Combine(currentDirectory, @"..\..\..");

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"{projectDirectory}\\app.json", optional: true, reloadOnChange: true)
    .Build();

string? connectionString = configuration.GetConnectionString("connection");

var stacksRepository = new StacksRepository(connectionString!);
var flashcardsRepository = new FlashcardsRepository(connectionString!);
var studySessionRepository = new StudySessionRepository(connectionString!);

using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();

    stacksRepository.CreateTable();
    flashcardsRepository.CreateTable();
    studySessionRepository.CreateTable();

    if (DatabaseUtility.CountRows(connectionString!, "Stacks") == 0)
        stacksRepository.SeedStacks();
    if (DatabaseUtility.CountRows(connectionString!, "Flashcards") == 0)
        flashcardsRepository.SeedFlashcards();
    if (DatabaseUtility.CountRows(connectionString!, "StudySessionStats") == 0)
        studySessionRepository = new StudySessionRepository(connectionString!);
}

string? menuChoice;

do
{
    menuChoice = Display.PrintMainMenu();

    switch (menuChoice)
    {
        case "Close Application":
            Console.WriteLine("\nGoodbye!");
            Thread.Sleep(2000);
            Environment.Exit(0);
            break;
        //case "View All Records":
        //    Display.PrintAllRecords("View All Records");
        //    UI.ReturnToMainMenu();
        //    break;
        //case "Add Record":
        //    RecordsController.AddRecord();
        //    UI.ReturnToMainMenu();
        //    break;
        //case "Edit Record":
        //    RecordsController.EditRecord();
        //    UI.ReturnToMainMenu();
        //    break;
        //case "Delete Record":
        //    RecordsController.DeleteRecord();
        //    UI.ReturnToMainMenu();
        //    break;
        //case "View Report":
        //    repository = new CodingSessionRepository(connection);
        //    var reportData = repository.GetReportData();
        //    Display.PrintReport(reportData);
        //    UI.ReturnToMainMenu();
        //    break;
        default:
            Console.WriteLine("Invalid choice.");
            break;
    }
} while (menuChoice != "Close Application");