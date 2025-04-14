using Flashcards.Controller;
using Flashcards.Model;
using Flashcards.View;
using Microsoft.Data.SqlClient;

Console.Title = "Flashcards";

var stacksRepository = new StacksRepository(DatabaseUtility.GetConnectionString());
var flashcardsRepository = new FlashcardsRepository(DatabaseUtility.GetConnectionString());
var studySessionRepository = new StudySessionRepository(DatabaseUtility.GetConnectionString());

using (SqlConnection connection = new SqlConnection(DatabaseUtility.GetConnectionString()))
{
    connection.Open();

    stacksRepository.CreateTable();
    flashcardsRepository.CreateTable();
    studySessionRepository.CreateTable();

    if (DatabaseUtility.CountRows(DatabaseUtility.GetConnectionString(), "Stacks") == 0)
        stacksRepository.SeedStacks();
    if (DatabaseUtility.CountRows(DatabaseUtility.GetConnectionString(), "Flashcards") == 0)
        flashcardsRepository.SeedFlashcards();
    if (DatabaseUtility.CountRows(DatabaseUtility.GetConnectionString(), "StudySessionStats") == 0)
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
                    Display.PrintAllStacks("View All Stacks");
                    UI.ReturnToMainMenu();
                    break;
                case "Add Stack":
                    StacksController.AddStack();
                    UI.ReturnToMainMenu();
                    break;
                case "Edit Stack":
                    StacksController.EditStack();
                    UI.ReturnToMainMenu();
                    break;
                case "Delete Stack":
                    StacksController.DeleteStack();
                    UI.ReturnToMainMenu();
                    break;
            }
            break;
        case "Manage Flashcards":
            menuChoice = Display.PrintFlashcardsMenu();

            switch (menuChoice)
            {
                case "Return to Main Menu":
                    break;
                case "View Flashcards":
                    Display.PrintAllStacks("View All Flashcards");
                    int stackId = UI.PromptForId("Enter the stack ID of the flashcards you want to view: ", "Stacks");
                    Display.PrintAllFlashcardsForStack("View All Flashcards", stackId);
                    UI.ReturnToMainMenu();
                    break;
                case "Add Flashcard":
                    FlashcardsController.AddFlashcard();
                    UI.ReturnToMainMenu();
                    break;
                case "Edit Flashcard":
                    FlashcardsController.EditFlashcard();
                    UI.ReturnToMainMenu();
                    break;
                    //case "Delete Flashcard"
                    //    FlashcardsController.DeleteFlashcard();
                    //    UI.ReturnToMainMenu();
                    //    break;
            }
            break;
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