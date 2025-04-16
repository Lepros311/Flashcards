using Flashcards.Model;
using Flashcards.View;
using Microsoft.Data.SqlClient;

namespace Flashcards.Controller
{
    public class StacksController
    {
        public static void AddStack()
        {
            Display.PrintAllStacks("Add Stack");

            string name = UI.PromptForAlphaNumericInput("Enter a name for the new stack: ");

            using (var connection = new SqlConnection(DatabaseUtility.GetConnectionString()))
            {
                connection.Open();

                string insertQuery = @"
                    INSERT INTO Stacks (StackName)
                    VALUES (@name)";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = insertQuery;
                    command.Parameters.AddWithValue("@name", name);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Display.PrintAllStacks("Add Stack");
                        Console.WriteLine("\nStack added successfully!");
                    }
                    else
                    {
                        Console.WriteLine("\nFailed to add stack. Please try again.");
                    }
                }
            }
        }

        public static void EditStack()
        {
            //Display.PrintAllStacks("Edit Stack");

            //int stackId = UI.PromptForId("Enter the ID of the stack you want to edit: ", "Stacks");

            int stackId = Display.PrintStackSelectionMenu("Edit Stack", "Select a stack to edit...").Id;

            Display.PrintAllStacks("Edit Stack");

            string? currentStackName = null;

            using (var connection = new SqlConnection(DatabaseUtility.GetConnectionString()))
            {
                connection.Open();

                string stackQuery = "SELECT * FROM Stacks WHERE StackId = @stackId";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = stackQuery;
                    command.Parameters.AddWithValue("@stackId", stackId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentStackName = reader["StackName"].ToString();
                        }
                    }
                }

                Console.WriteLine($"\nSelected stack ID: {stackId}");
                Console.WriteLine($"Stack Name: {currentStackName}");

                string newStackName = UI.PromptForAlphaNumericInput($"\nEnter new stack name (leave blank to keep current): ", true); 
                
                if (string.IsNullOrEmpty(newStackName))
                {
                    newStackName = currentStackName!;
                }

                if (newStackName == currentStackName)
                {
                    Console.WriteLine("\nNo changes were made.");
                    return;
                }

                string updateStackQuery = "UPDATE Stacks SET StackName = @newStackName WHERE StackId = @stackId";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = updateStackQuery;
                    command.Parameters.AddWithValue("@stackId", stackId);
                    command.Parameters.AddWithValue("@newStackName", newStackName);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Display.PrintAllStacks("Edit Stack");
                        Console.WriteLine("\nStack updated successfully!");
                    }
                }
            }

        }

        public static void DeleteStack()
        {
            //Display.PrintAllStacks("Delete Stack");

            //int stackId = UI.PromptForId("Enter the ID of the stack you want to delete: ", "Stacks");

            int stackId = Display.PrintStackSelectionMenu("Delete Stack", "Select a stack to delete...").Id;

            Display.PrintAllStacks("Delete Stack");

            if (UI.PromptForDeleteConfirmation(stackId, "stack") == "n")
            {
                return;
            }

            using (var connection = new SqlConnection(DatabaseUtility.GetConnectionString()))
            {
                connection.Open();

                string deleteQuery = @"
                    DELETE FROM Stacks
                    WHERE StackId = @stackId";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = deleteQuery;
                    command.Parameters.AddWithValue("@stackId", stackId);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Display.PrintAllStacks("Delete Stack");
                        Console.WriteLine("\nStack deleted successfully!");
                    }
                    else
                    {
                        Console.WriteLine("\nNo stack found with that ID. Deletion failed.");
                    }
                }
            }
        }
    }
}
