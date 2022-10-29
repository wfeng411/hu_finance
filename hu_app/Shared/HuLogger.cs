using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace hu_app.Shared
{
    public class HuLogger
    {
        private static string _connectionString;

        public static void Setup(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static void LogInfo(string info)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Logs (LogTime, Source, Message) VALUES (@logTime, @source , @message)";
                command.Parameters.Add(new SqlParameter("@logTime", DateTime.Now));
                command.Parameters.Add(new SqlParameter("@source", HuConstants.LogSource.Info));
                command.Parameters.Add(new SqlParameter("@message", info));
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public static void LogException(Exception exception)
        {
            List<string> errors = exception.GetErrors();
            var position = exception.GetPosition() ?? string.Empty;
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Logs (LogTime, Source, Message, Error) VALUES (@logTime, @source, @position, @error)";
                command.Parameters.Add(new SqlParameter("@logTime", DateTime.Now));
                command.Parameters.Add(new SqlParameter("@source", HuConstants.LogSource.Exception));
                command.Parameters.Add(new SqlParameter("@position", position));
                command.Parameters.Add(new SqlParameter("@error", String.Join("; ", errors)));
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public static void LogMediatorPerformance(string mediator, int elapsedMilliseconds, long? dataSize)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Logs (LogTime, Source, TimeElapsed, Message, DataSize) VALUES (@logTime, @source, @timeElapsed, @message, @dataSize)";
                command.Parameters.Add(new SqlParameter("@logTime", DateTime.Now));
                command.Parameters.Add(new SqlParameter("@source", HuConstants.LogSource.Mediator));
                command.Parameters.Add(new SqlParameter("@message", mediator));
                command.Parameters.Add(new SqlParameter("@timeElapsed", elapsedMilliseconds));
                command.Parameters.Add(new SqlParameter("@dataSize", dataSize));
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public static void LogMediatorValidationError(string mediator, List<string> errors)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Logs (LogTime, Source, Message, Error) VALUES (@logTime, @source, @message, @error)";
                command.Parameters.Add(new SqlParameter("@logTime", DateTime.Now));
                command.Parameters.Add(new SqlParameter("@source", HuConstants.LogSource.MediatorValidator));
                command.Parameters.Add(new SqlParameter("@message", mediator));
                command.Parameters.Add(new SqlParameter("@error", String.Join("; ", errors)));
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
}
