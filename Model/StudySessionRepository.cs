using Microsoft.Data.SqlClient;

namespace Flashcards.Model
{
    internal class StudySessionRepository
    {
        private readonly string _connectionString;

        public StudySessionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateTable()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string createTableQuery = @"
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'StudySessionStats')
                    BEGIN
                        CREATE TABLE StudySessionStats (
                        SessionID INT IDENTITY(1,1) PRIMARY KEY,
                        StackID INT NOT NULL,
                        FlashcardID INT NOT NULL,
                        Question NVARCHAR(MAX) NOT NULL,
                        CorrectAnswer NVARCHAR(MAX) NOT NULL,
                        UserAnswer NVARCHAR(MAX) NOT NULL,
                        AnsweredCorrectly BIT NOT NULL,
                        SessionStartTime DATETIME NOT NULL,
                        PercentageCorrect DECIMAL(5,2) NOT NULL,
                        FOREIGN KEY (StackID) REFERENCES Stacks(StackID),
                        FOREIGN KEY (FlashcardID) REFERENCES Flashcards(FlashcardID)
                        );
                    END;";

                using (SqlCommand command = new SqlCommand(createTableQuery, connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("StudySessionStats table created successfully.");
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"An error occurred while creating the StudySessionStats table: {ex.Message}");
                    }
                }
            }
        }

        public void SeedStudySessionStats()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string insertStudySessionStatsQuery = @"
                    INSERT INTO StudySessionStats (StackID, FlashcardID, Question, CorrectAnswer, UserAnswer, AnsweredCorrectly, SessionStartTime, PercentageCorrect)
                      VALUES (@StackID, @FlashcardID, @Question, @CorrectAnswer, @UserAnswer, @AnsweredCorrectly, @SessionStartTime, @PercentageCorrect);";

                var studySessions = new List<(int StackID, int FlashcardID, string Question, string CorrectAnswer, string UserAnswer, bool AnsweredCorrectly, DateTime SessionStartTime, decimal PercentageCorrect)>
                {
                    (1, 1, "What is 2 + 2?", "4", "4", true, DateTime.Now, 100.00m),
                    (2, 2, "What is the chemical symbol for water?", "H20", "H20", true, DateTime.Now.AddMinutes(-30), 100.00m),
                    (3, 3, "Who was the first president of the USA?", "George Washington", "Thomas Jefferson", false, DateTime.Now.AddMinutes(-60), 0.00m)
                };

                foreach (var (StackID, FlashcardID, Question, CorrectAnswer, UserAnswer, AnsweredCorrectly, SessionStartTime, PercentageCorrect) in studySessions)
                {
                    using (SqlCommand command = new SqlCommand(insertStudySessionStatsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@StackID", StackID);
                        command.Parameters.AddWithValue("@FlashcardID", FlashcardID);
                        command.Parameters.AddWithValue("@Question", Question);
                        command.Parameters.AddWithValue("@CorrectAnswer", CorrectAnswer);
                        command.Parameters.AddWithValue("@UserAnswer", UserAnswer);
                        command.Parameters.AddWithValue("@AnsweredCorrectly", AnsweredCorrectly);
                        command.Parameters.AddWithValue("@SessionStartTime", SessionStartTime);
                        command.Parameters.AddWithValue("@PercentageCorrect", PercentageCorrect);
                        command.ExecuteNonQuery();
                        Console.WriteLine($"Inserted Study Session for StackID: {StackID}, FlashcardID: {FlashcardID}, Question: '{Question}', Answered Correctly: {AnsweredCorrectly}, Percentage Correct: {PercentageCorrect}%");
                    }
                }
            }
        }
    }
}
