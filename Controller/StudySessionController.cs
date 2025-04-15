using Flashcards.Model;
using Flashcards.View;
using Microsoft.Data.SqlClient;

namespace Flashcards.Controller
{
    public class StudySessionController
    {
        public static void Study()
        {
            Display.PrintAllStacks("Study");

            int stackId = UI.PromptForId("Enter the ID of the stack you want to study: ", "Stacks");

            int numberOfFlashcardsToStudy = UI.PromptForNumberOfFlashcards("How many flashcards do you want to study during this session? ", stackId);

            Console.Clear();

            var flashcardsRepo = new FlashcardsRepository(DatabaseUtility.GetConnectionString());
            var flashcards = flashcardsRepo.GetAllFlashcardsForStack(stackId);

            Random random = new Random();
            int correctAnswers = 0;

            List<int> shownIndices = new List<int>();

            Console.WriteLine("Studying Flashcards:");

            for (int i = 0; i < numberOfFlashcardsToStudy; i++)
            {
                int randomIndex;

                do
                {
                    randomIndex = random.Next(flashcards.Count);
                } while (shownIndices.Contains(randomIndex));

                Flashcard flashcard = flashcards[randomIndex];

                Console.WriteLine($"{flashcard.Question}");

                Console.WriteLine("User Ansewr:");

                string? userAnswer = Console.ReadLine();

                if (userAnswer.Equals(flashcard.Answer, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Correct!");
                    correctAnswers++;
                }
                else
                {
                    Console.WriteLine($"Incorrect! The correct answer is: {flashcard.Answer}");
                }

                shownIndices.Add(randomIndex);
            }

            decimal percentageCorrect = (decimal)correctAnswers / numberOfFlashcardsToStudy * 100;

            var session = new StudySession
            {
                StackId = stackId,
                SessionStartTime = DateTime.Now,
                PercentageCorrect = percentageCorrect
            };

            var statsRepo = new StudySessionRepository(DatabaseUtility.GetConnectionString());
            statsRepo.SaveStudySessionStats(session);
        }
    }
}
