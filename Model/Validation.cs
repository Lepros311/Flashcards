namespace Flashcards.Model
{
    internal class Validation
    {
        public static bool ValidateAlphaNumericInput(string input, bool forEdit = false)
        {
            if (!forEdit)
            {
                if (!string.IsNullOrWhiteSpace(input) && (input.Any(char.IsLetter) || input.Any(char.IsDigit)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(input) || input.Any(char.IsLetter) || input.Any(char.IsDigit))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool ValidateNumericInput(string input)
        {
            if (!string.IsNullOrEmpty(input) && input.Any(char.IsDigit))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ValidateDeleteConfirmation(string input)
        {
            if (input == "n" || input == "y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
