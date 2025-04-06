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
        studySessionRepository.SeedStudySessionStats();
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
        case "Manage Stacks":
            menuChoice = Display.PrintStacksMenu();

            switch (menuChoice)
            {
                case "Return to Main Menu":
                    break;
                case "View All Stacks":
                    Display.PrintAllStacks(connectionString!, "View All Stacks");
                    UI.ReturnToMainMenu();
                    break;
                case "Add Stack":
                    RecordsController.AddStack();
                    break;
                case "Edit Stack":
                    RecordsController.EditStack();
                    break;
                case "Delete Stack":
                    RecordsController.DeleteStack();
                    break;
            }
            break;
        //case "Manage Flashcards":
        //    Display.PrintFlashcardsMenu();
        //    RecordsController.AddRecord();
        //    break;
        //case "Study":
        //    RecordsController.EditRecord();
        //    UI.ReturnToMainMenu();
        //    break;
        //case "View Study Session Data":
        //    RecordsController.DeleteRecord();
        //    UI.ReturnToMainMenu();
        //    break;
        //case "View Full Report":
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