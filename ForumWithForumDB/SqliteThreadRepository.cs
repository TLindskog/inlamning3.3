using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForumWithForumDB
{

    public class Thread
    {
        public int ThreadId { get; set; }
        public string Topic { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public int PostCount { get; set; }
    }

    class SqliteThreadRepository
    {

        private const string _connectionString = "Data Source=.\\Forum.db";

        public List<Thread> GetThreads()
        {
            using var connection = new SqliteConnection(_connectionString);
            var output = connection.Query<Thread>("SELECT * FROM Threads");// Lägga till parameter substitution
            return output.ToList();
        }

        public void AddThread(Thread thread)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = "INSERT INTO threads (Topic,Text,PostCount) VALUES " +
                $"(@Topic, @Text, @PostCount)";
            var result = connection.Execute(sql, thread);
        }

        public void UpdateThreadPostCount(Thread thread)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = $"UPDATE Threads SET PostCount = @PostCount WHERE ThreadId = @ThreadId";
            connection.Execute(sql, thread);
        }

        public Thread GetThreadWithId(int id) //Parameter substitution
        {
            using var connection = new SqliteConnection(_connectionString);
            var query = "SELECT * FROM Threads WHERE ThreadId = @Id";
            return connection.QuerySingle<Thread>(query, new { Id = id });
        }
    }
}
