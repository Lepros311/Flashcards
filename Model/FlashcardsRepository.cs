using Flashcards.Model;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using System.Security.Principal;

namespace Flashcards.Model
{
    public class FlashcardsRepository
    {
        private readonly string _connectionString;

        public FlashcardsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateTable()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string createTableQuery = @"
                    IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Flashcards')
                    BEGIN
                        CREATE TABLE Flashcards(
                            FlashcardId INT IDENTITY(1,1) PRIMARY KEY,
                            StackId INT NOT NULL,
                            Question TEXT NOT NULL,
                            Answer TEXT NOT NULL,
                            FOREIGN KEY (StackID) REFERENCES Stacks(StackID) ON DELETE CASCADE
                        );
                    END;";

                using (SqlCommand command = new SqlCommand(createTableQuery, connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Flashcards table created successfully.");
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"An error occurred while creating the Flashcards table: {ex.Message}");
                    }
                }
            }
        }

        public void SeedFlashcards()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string insertFlashcardsQuery = "INSERT INTO Flashcards (Question, Answer, Id) VALUES (@Question, @Answer, @Id);";

                var flashcards = new List<(string Question, string Answer, int Id)>
                {
                    ("What is 2 + 2?", "4", 1),
                    ("What is the chemical symbol for water?", "H20", 2),
                    ("Who was the first president of the USA?", "George Washington", 3)
                };

                foreach (var (Question, Answer, Id) in flashcards)
                {
                    using (SqlCommand command = new SqlCommand(insertFlashcardsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Question", Question);
                        command.Parameters.AddWithValue("@Answer", Answer);
                        command.Parameters.AddWithValue("@Id", Id);
                        command.ExecuteNonQuery();
                        Console.WriteLine($"Inserted Flashcard: {Question} for StackId: {Id}");
                    }
                }
            }
        }

        public List<Flashcard> GetAllFlashcardsForStack(int stackId)
        {
            var flashcards = new List<Flashcard>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"SELECT * FROM Flashcards WHERE StackId = @stackId ORDER BY FlashcardID DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@stackId", stackId));

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var flashcard = new Flashcard
                            {
                                FlashcardId = reader.GetInt32(0),
                                StackId = reader.GetInt32(1),
                                Question = reader.GetString(2),
                                Answer = reader.GetString(3)
                            };
                            flashcards.Add(flashcard);
                        }
                    }
                }
            }

            return flashcards;
        }
    }
}