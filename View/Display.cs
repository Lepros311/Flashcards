﻿using Spectre.Console;
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
                    "Return to Main Menu", "View Flashcards", "Add Flashcard", "Edit Flashcard", "Delete Flashcard"
                }));

            return menuChoice;
        }

        public static Stacks PrintStackSelectionMenu(string heading, string title)
        {
            Console.Clear();

            var repository = new StacksRepository(DatabaseUtility.GetConnectionString());
            var stacks = repository.GetAllStacks();

            if (stacks == null || stacks.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No stacks available.[/]");
                return null;
            }

            var displayOptions = stacks.Select((stack, index) => new
            {
                DisplayText = $"{index + 1}: {stack.Name}",
                Stack = stack
            }).ToList();

            var rule = new Rule($"[green]{heading}[/]");
            rule.Justification = Justify.Left;
            AnsiConsole.Write(rule);

            var selectedOption = AnsiConsole.Prompt(
                new SelectionPrompt<dynamic>()
                .Title($"\n{title}")
                .PageSize(10)
                .AddChoices(displayOptions.Select(option => option.DisplayText).ToArray()));

            var selectedStack = displayOptions.First(option => option.DisplayText.StartsWith(selectedOption.Split(':')[0]));

            return selectedStack.Stack;
        }

        public static void PrintAllStacks(string heading)
        {
            var repository = new StacksRepository(DatabaseUtility.GetConnectionString());
            var stacks = repository.GetAllStacks();

            Console.Clear();

            var rule = new Rule($"[green]{heading}[/]");
            rule.Justification = Justify.Left;
            AnsiConsole.Write(rule);

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[dodgerblue1]ID[/]").Centered())
                .AddColumn(new TableColumn("[dodgerblue1]Stack Name[/]").Centered());

            if (stacks == null || stacks.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No records found.[/]");
                return;
            }

            var sortedStacks = stacks.OrderBy(stack => stack.Id).ToList();

            int displayId = 1;

            foreach (var stack in sortedStacks)
            {
                //table.AddRow(
                //    stack.Id.ToString(),
                //    stack.Name!
                //);
                table.AddRow(displayId.ToString(), stack.Name);
                displayId++;
            }

            AnsiConsole.Write(table);
        }

        public static void PrintAllFlashcardsForStack(string heading, int stackId)
        {
            var repository = new FlashcardsRepository(DatabaseUtility.GetConnectionString());
            var flashcards = repository.GetAllFlashcardsForStack(stackId);

            string? stackName = null;

            using (var connection = new SqlConnection(DatabaseUtility.GetConnectionString()))
            {
                connection.Open();

                string stackQuery = "SELECT StackName FROM Stacks WHERE StackId = @stackId";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = stackQuery;
                    command.Parameters.AddWithValue("@stackId", stackId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stackName = reader["StackName"].ToString();
                        }
                    }
                }
            }

            Console.Clear();

            var rule = new Rule($"[green]{heading}[/]");
            rule.Justification = Justify.Left;
            AnsiConsole.Write(rule);

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[dodgerblue1]ID[/]").Centered())
                .AddColumn(new TableColumn("[dodgerblue1]Question[/]").Centered())
                .AddColumn(new TableColumn("[dodgerblue1]Answer[/]").Centered());

            if (flashcards.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No records found.[/]");
                return;
            }

            foreach (var flashcard in flashcards)
            {
                table.AddRow(
                    flashcard.FlashcardId.ToString(),
                    flashcard.Question!,
                    flashcard.Answer!
                );
            }

            Console.WriteLine();
            AnsiConsole.Write(new Markup($" [dodgerblue1]Stack:[/] [white]{stackName}[/]"));
            Console.WriteLine();
            AnsiConsole.Write(table);
        }

        public static void PrintAllStudySessionData(string heading)
        {
            var sessionRepo = new StudySessionRepository(DatabaseUtility.GetConnectionString());
            var sessions = sessionRepo.GetAllStudySessions();

            Console.Clear();

            var rule = new Rule($"[green]{heading}[/]");
            rule.Justification = Justify.Left;
            AnsiConsole.Write(rule);

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[dodgerblue1]ID[/]").Centered())
                .AddColumn(new TableColumn("[dodgerblue1]Stack Name[/]").Centered())
                .AddColumn(new TableColumn("[dodgerblue1]Session Start Time[/]").Centered())
                .AddColumn(new TableColumn("[dodgerblue1]Percentage Correct[/]").Centered());

            if (sessions.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No records found.[/]");
                return;
            }

            foreach (var session in sessions)
            {
                table.AddRow(
                    session.Id.ToString(),
                    session.StackName.ToString(),
                    session.SessionStartTime.ToString("MMMM dd, yyyy h:mm tt"),
                    session.PercentageCorrect.ToString("0") + "%"
                );
            }

            AnsiConsole.Write(table);
        }
    }


        
    
}
