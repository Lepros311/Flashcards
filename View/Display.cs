using Spectre.Console;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Flashcards.Model;

namespace Flashcards.View
{
    public class Display
    {
        public static string PrintMainMenu()
        {
            Console.Clear();

            var menuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("MAIN MENU")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Close Application", "Manage Stacks", "Manage Flashcards", "Study", "View Study Session Data", "View Full Report"
                }));

            return menuChoice;
        }

        public static string PrintStacksMenu()
        {
            Console.Clear();

            var menuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("STACKS MENU")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Return to Main Menu", "View All Stacks", "Add Stack", "Edit Stack", "Delete Stack"
                }));

            return menuChoice;
        }

        public static string PrintFlashcardsMenu()
        {
            Console.Clear();

            var menuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("FLASHCARDS MENU")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Return to Main Menu", "View All Flashcards", "Add Flashcard", "Edit Flashcard", "Delete Flashcard"
                }));

            return menuChoice;
        }

        public static void PrintAllStacks(string connectionString, string heading)
        {
            Console.WriteLine("PrintAllStacks.");

            var repository = new StacksRepository(connectionString);
            var stacks = repository.GetAllStacks();

            Console.Clear();

            var rule = new Rule($"[green]{heading}[/]");
            rule.Justification = Justify.Left;
            AnsiConsole.Write(rule);

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[dodgerblue1]ID[/]").Centered())
                .AddColumn(new TableColumn("[dodgerblue1]Stack Name[/]").Centered());

            if (stacks.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No records found.[/]");
                return;
            }

            foreach (var stack in stacks)
            {
                table.AddRow(
                    stack.Id.ToString(),
                    stack.Name!
                );
            }

            AnsiConsole.Write(table);
        }
    }


        
    
}
