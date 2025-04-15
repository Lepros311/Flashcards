namespace Flashcards.Model
{
    public class StudySession
    {
        public int Id { get; set; }

        public int StackId { get; set; }

        public DateTime SessionStartTime { get; set; }

        public decimal PercentageCorrect { get; set; }

        public StudySession() { }

        public StudySession(int id, int stackId, DateTime sessionStartTime, decimal percentageCorrect)
        {
            Id = id;
            StackId = stackId;
            SessionStartTime = sessionStartTime;
            PercentageCorrect = percentageCorrect;
        }
    }
}
