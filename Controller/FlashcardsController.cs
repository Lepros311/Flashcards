using Flashcards.Model;
using Flashcards.View;
using Microsoft.Data.SqlClient;

namespace Flashcards.Controller
{
    public class FlashcardsController
    {
        public static void AddFlashcard()
        {
            Display.PrintAllStacks("Add Flashcard");

            int stackId = UI.PromptForId("Enter the ID of the stack you want to add a flashcard to: ", "Stacks");
            string? stackName = null;

            using (var connection = new SqlConnection(DatabaseUtility.GetConnectionString()))
            {
                connection.Open();

                string queryForStackName = "SELECT StackName FROM Stacks WHERE StackId = @stackId";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = queryForStackName;
                    command.Parameters.AddWithValue("@stackId", stackId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stackName = reader.GetString(0);
                        }
                    }
                }

            }

            string question = UI.PromptForAlphaNumericInput("Enter the flashcard's question: ");
            string answer = UI.PromptForAlphaNumericInput("Enter the flashcard's answer: ");

            using (var connection = new SqlConnection(DatabaseUtility.GetConnectionString()))
            {
                connection.Open();

                string insertQuery = @"
                    INSERT INTO Flashcards (StackID, Question, Answer)
                    VALUES (@stackId, @question, @answer)";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = insertQuery;
                    command.Parameters.AddWithValue("@stackId", stackId);
                    command.Parameters.AddWithValue("@question", question);
                    command.Parameters.AddWithValue("@answer", answer);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Display.PrintAllFlashcardsForStack("Add Flashcard", stackId);
                        Console.WriteLine("\nFlashcard added successfully!");
                    }
                    else
                    {
                        Console.WriteLine("\nFailed to add flashcard. Please try again.");
                    }
                }
            }
        }

        public static void EditFlashcard()
        {
            Display.PrintAllStacks("Edit Flashcard");

            int stackId = UI.PromptForId("Enter the ID of the flashcard's stack: ", "Stacks");

            Display.PrintAllFlashcardsForStack("Edit Flashcard", stackId);

            int flashcardId = UI.PromptForId("Enter the ID of the flashcard you want to edit: ", "Flashcards");

            string? currentQuestion = null;
            string? currentAnswer = null;

            using (var connection = new SqlConnection(DatabaseUtility.GetConnectionString()))
            {
                connection.Open();

                string flashcardQuery = "SELECT * FROM Flashcards WHERE flashcardId = @flashcardId";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = flashcardQuery;
                    command.Parameters.AddWithValue("@flashcardId", flashcardId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentQuestion = reader["Question"].ToString();
                            currentAnswer = reader["Answer"].ToString();
                        }
                    }
                }

                Console.WriteLine($"\nSelected flashcard ID: {flashcardId}");
                Console.WriteLine($"Question: {currentQuestion}");
                Console.WriteLine($"Answer: {currentAnswer}");

                string newQuestion = UI.PromptForAlphaNumericInput($"\nEnter new question (leave blank to keep current): ", true);

                if (string.IsNullOrEmpty(newQuestion))
                {
                    newQuestion = currentQuestion!;
                }

                string newAnswer = UI.PromptForAlphaNumericInput($"\nEnter new answer (leave blank to keep current): ", true);

                if (string.IsNullOrEmpty(newAnswer))
                {
                    newAnswer = currentAnswer!;
                }

                if (newQuestion == currentQuestion && newAnswer == currentAnswer)
                {
                    Console.WriteLine("\nNo changes were made.");
                    return;
                }

                string updateFlashcardQuery = "UPDATE Flashcards SET Question = @newQuestion, Answer = @newAnswer WHERE FlashcardId = @flashcardId";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = updateFlashcardQuery;
                    command.Parameters.AddWithValue("@flashcardId", flashcardId);
                    command.Parameters.AddWithValue("@newQuestion", newQuestion);
                    command.Parameters.AddWithValue("@newAnswer", newAnswer);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Display.PrintAllFlashcardsForStack("Edit Flashcard", stackId);
                        Console.WriteLine("\nFlashcard updated successfully!");
                    }
                }
            }
        }

        public static void DeleteFlashcard()
        {
            Display.PrintAllStacks("Delete Flashcard");

            int stackId = UI.PromptForId("Enter the ID of the flashcard's stack: ", "Stacks");

            Display.PrintAllFlashcardsForStack("Delete Flashcard", stackId);

            int flashcardId = UI.PromptForId("Enter the ID of the flashcard you want to delete: ", "Flashcards");

            if (UI.PromptForDeleteConfirmation(flashcardId, "flashcard") == "n")
            {
                return;
            }

            using (var connection = new SqlConnection(DatabaseUtility.GetConnectionString()))
            {
                connection.Open();

                string deleteQuery = @"
                    DELETE FROM Flashcards
                    WHERE FlashcardId = @flashcardId";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = deleteQuery;
                    command.Parameters.AddWithValue("@flashcardId", flashcardId);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Display.PrintAllFlashcardsForStack("Delete Flashcard", stackId);
                        Console.WriteLine("\nFlashcard deleted successfully!");
                    }
                    else
                    {
                        Console.WriteLine("\nNo flashcard found with that ID. Deletion failed.");
                    }
                }
            }

        }
    }
}
