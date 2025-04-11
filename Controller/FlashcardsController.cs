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
    }
}
