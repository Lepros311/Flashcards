using Flashcards.Model;
using Spectre.Console;

namespace Flashcards.View
{
    internal class UI
    {
        public static void ReturnToMainMenu()
        {
            Console.Write("\nPress any key to return to the Main Menu...");
            Console.ReadKey();
        }

        public static string PromptForAlphaNumericInput(string message, bool forEdit = false)
        {
            string? input;
            bool isValidInput = false;
            do
            {
                Console.Write(message);
                input = Console.ReadLine();

                if (!forEdit)
                {
                    if (Validation.ValidateAlphaNumericInput(input) == false)
                    {
                        Console.WriteLine("Invalid input.");
                    }
                    else
                    {
                        isValidInput = true;
                    }
                }
                else
                    if (Validation.ValidateAlphaNumericInput(input, true) == false)
                    {
                        Console.WriteLine("Invalid input.");
                    }
                    else
                    {
                        isValidInput = true;
                    }
            } while (isValidInput == false);

            return input;
        }

        public static int PromptForId(string message, string tableName)
        {
            string? input;
            bool isValidInput = false;
            do
            {
                Console.Write(message);
                input = Console.ReadLine();
                if (Validation.ValidateNumericInput(input!) == false)
                {
                    Console.WriteLine("Invalid input.");
                }
                else if (DatabaseUtility.CheckIfIdExists(DatabaseUtility.GetConnectionString(), tableName, Convert.ToInt32(input)))
                {
                    isValidInput = true;
                }
                else
                {
                    Console.WriteLine("Record not found. Please enter a valid ID.");
                }
            } while (isValidInput == false);

            return Convert.ToInt32(input);
        }

        public static string PromptForDeleteConfirmation(int recordId, string recordType)
        {
            string? confirmation;
            bool isValidConfirmation = false;
            do
            {
                Console.Write($"Are you sure you want to delete the {recordType} with ID {recordId}? (y/n): ");
                confirmation = Console.ReadLine();
                isValidConfirmation = Validation.ValidateDeleteConfirmation(confirmation);
                if (isValidConfirmation)
                {
                    if (confirmation == "n")
                    {
                        Console.WriteLine("Deletion canceled.");
                        return "n";
                    }
                    else
                    {
                        return "y";
                    }
                }
                else 
                {
                    Console.WriteLine("Invalid response.\n");
                }
            } while (isValidConfirmation == false);

            return "";
        }
    }
}
