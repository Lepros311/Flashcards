namespace Flashcards.Model
{
    public class StudySession
    {
        public int Id { get; set; }

        public int StackId { get; set; }

        public int FlashcardId { get; set; }

        public string? Question { get; set; }

        public string? CorrectAnswer { get; set; }

        public string? UserAnswer { get; set; }

        public bool AnsweredCorrectly { get; set; }

        public DateTime SessionStartTime { get; set; }

        public decimal PercentageCorrect { get; set; }

        public StudySession() { }

        public StudySession(int id, int stackId, int flashcardId, string question, string correctAnswer, string userAnswer, bool answeredCorrectly, DateTime sessionStartTime, decimal percentageCorrect)
        {
            Id = id;
            StackId = stackId;
            FlashcardId = flashcardId;
            Question = question;
            CorrectAnswer = correctAnswer;
            UserAnswer = userAnswer;
            AnsweredCorrectly = answeredCorrectly;
            SessionStartTime = sessionStartTime;
            PercentageCorrect = percentageCorrect;
        }
    }
}
