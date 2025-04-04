namespace Flashcards.Model
{
    public class Flashcards
    {
        public int Id { get; set; }

        public int StackId { get; set; }

        public string? Question { get; set; }

        public string? Answer { get; set; }

        public Flashcards() { }

        public Flashcards(int id, int stackId, string? question, string? answer)
        {
            Id = id;
            StackId = stackId;
            Question = question;
            Answer = answer;
        }
    }
}
