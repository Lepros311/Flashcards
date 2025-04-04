using Microsoft.Data.SqlClient;

namespace Flashcards.Model
{
    public static class DatabaseUtility
    {
        // Method to count rows in a specified table
        public static int CountRows(string connectionString, string tableName)
        {
            // Validate the table name to prevent SQL injection
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentException("Table name cannot be null or empty.", nameof(tableName));
            }

            // Optionally, maintain a list of valid table names to check against
            var validTables = new[] { "Flashcards", "StudySessionStats" }; // Add your valid table names here
            if (!Array.Exists(validTables, t => t.Equals(tableName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("Invalid table name.", nameof(tableName));
            }

            // Prepare the SQL query
            string query = $"SELECT COUNT(1) FROM {tableName}";

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
        }

        // Method to check if a row with a specific ID exists in a specified table
        public static bool CheckIfIdExists(string connectionString, string tableName, int id)
        {
            // Validate the table name to prevent SQL injection
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentException("Table name cannot be null or empty.", nameof(tableName));
            }

            // Optionally, maintain a list of valid table names to check against
            var validTables = new[] { "Flashcards", "StudySessionStats" }; // Add your valid table names here
            if (!Array.Exists(validTables, t => t.Equals(tableName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("Invalid table name.", nameof(tableName));
            }

            // Prepare the SQL query
            string query = $"SELECT COUNT(1) FROM {tableName} WHERE Id = @Id";

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@Id", id));
                    connection.Open();
                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }
    }
}
