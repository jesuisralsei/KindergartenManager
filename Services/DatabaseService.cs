
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;
using KindergartenCRUD.Models;

namespace KindergartenCRUD.Services
{
    /// <summary>
    /// All SQLite access lives here.
    ///
    /// Two tables:
    /// Students  -> one row per child
    /// Payments  -> one row per student + month
    ///
    /// A payment is always tied to a specific student and month.
    /// </summary>
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            string folder = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData),
                "KindergartenCRUD");

            Directory.CreateDirectory(folder);

            string dbPath = Path.Combine(
                folder,
                "kindergarten.db");

            _connectionString = $"Data Source={dbPath}";

            Initialize();
        }

        private void Initialize()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Students (
                    Id           TEXT PRIMARY KEY,
                    Name         TEXT NOT NULL,
                    Age          INTEGER NOT NULL,
                    DateOfBirth  TEXT NOT NULL,
                    ClassName    TEXT NOT NULL,
                    MonthlyFee   REAL NOT NULL DEFAULT 0,
                    Notes        TEXT NOT NULL DEFAULT ''
                );

                CREATE TABLE IF NOT EXISTS Payments (
                    StudentId TEXT NOT NULL,
                    Month     INTEGER NOT NULL,
                    Amount    REAL NOT NULL DEFAULT 0,
                    PRIMARY KEY (StudentId, Month)
                );";

            cmd.ExecuteNonQuery();

            // Makes sure older databases also get the MonthlyFee column.
            MigrateStudentsTable(conn);
        }

        /// <summary>
        /// Adds MonthlyFee to older Students tables that don't have it yet.
        /// </summary>
        private static void MigrateStudentsTable(SqliteConnection conn)
        {
            var columns = new HashSet<string>();

            var pragma = conn.CreateCommand();
            pragma.CommandText = "PRAGMA table_info(Students);";

            using (var reader = pragma.ExecuteReader())
            {
                while (reader.Read())
                {
                    columns.Add(reader.GetString(1));
                }
            }

            // Add MonthlyFee if it doesn't exist
            if (!columns.Contains("MonthlyFee"))
            {
                var alter = conn.CreateCommand();

                alter.CommandText =
                    "ALTER TABLE Students " +
                    "ADD COLUMN MonthlyFee REAL NOT NULL DEFAULT 0;";

                alter.ExecuteNonQuery();
            }

            // Add Notes if it doesn't exist
            if (!columns.Contains("Notes"))
            {
                var alter = conn.CreateCommand();

                alter.CommandText =
                    "ALTER TABLE Students " +
                    "ADD COLUMN Notes TEXT NOT NULL DEFAULT '';";

                alter.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Gets all students and their payment information.
        /// </summary>
        public List<Student> GetAll()
        {
            var students = new List<Student>();
            var byId = new Dictionary<string, Student>();

            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            // -----------------------------
            // Load students
            // -----------------------------

            var studentCmd = conn.CreateCommand();

            studentCmd.CommandText = @"
                SELECT
                    Id,
                    Name,
                    Age,
                    DateOfBirth,
                    ClassName,
                    MonthlyFee,
                    Notes
                FROM Students
                ORDER BY Name;";

            using (var reader = studentCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var student = new Student
                    {
                        Id = reader.GetString(0),
                        Name = reader.GetString(1),
                        Age = reader.GetInt32(2),
                        DateOfBirth =
                            DateTime.Parse(reader.GetString(3)),
                        ClassName = reader.GetString(4),
                        MonthlyFee =
                            (decimal)reader.GetDouble(5),
                        Notes = reader.GetString(6)
                    };

                    students.Add(student);

                    byId[student.Id] = student;
                }
            }

            // -----------------------------
            // Load payments
            // -----------------------------

            var paymentCmd = conn.CreateCommand();

            paymentCmd.CommandText =
                "SELECT StudentId, Month, Amount FROM Payments;";

            using (var reader = paymentCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string studentId = reader.GetString(0);

                    int month = reader.GetInt32(1);

                    decimal amount =
                        (decimal)reader.GetDouble(2);

                    if (byId.TryGetValue(
                        studentId,
                        out var student))
                    {
                        student.SetMonthPaid(
                            month,
                            amount);
                    }
                }
            }

            return students;
        }

        /// <summary>
        /// Inserts a new student.
        /// </summary>
        public void Insert(Student s)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText = @"
                INSERT INTO Students
                (
                    Id,
                    Name,
                    Age,
                    DateOfBirth,
                    ClassName,
                    MonthlyFee,
                    Notes
                )
                VALUES
                (
                    $id,
                    $name,
                    $age,
                    $dob,
                    $class,
                    $fee,
                    $notes
                );";

            AddStudentParams(cmd, s);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates an existing student.
        /// </summary>
        public void Update(Student s)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText = @"
                UPDATE Students
                SET
                    Name = $name,
                    Age = $age,
                    DateOfBirth = $dob,
                    ClassName = $class,
                    MonthlyFee = $fee,
                    Notes = $notes
                WHERE Id = $id;";

            AddStudentParams(cmd, s);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Deletes a student and all of their payments.
        /// </summary>
        public void Delete(string id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            using var transaction = conn.BeginTransaction();

            // Delete payments first.
            var deletePayments = conn.CreateCommand();

            deletePayments.Transaction = transaction;

            deletePayments.CommandText =
                "DELETE FROM Payments WHERE StudentId = $id;";

            deletePayments.Parameters.AddWithValue(
                "$id",
                id);

            deletePayments.ExecuteNonQuery();

            // Then delete the student.
            var deleteStudent = conn.CreateCommand();

            deleteStudent.Transaction = transaction;

            deleteStudent.CommandText =
                "DELETE FROM Students WHERE Id = $id;";

            deleteStudent.Parameters.AddWithValue(
                "$id",
                id);

            deleteStudent.ExecuteNonQuery();

            transaction.Commit();
        }

        /// <summary>
        /// Records or updates the payment for one
        /// student for one specific month.
        /// </summary>
        public void UpsertPayment(
            string studentId,
            int month,
            decimal amount)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText = @"
                INSERT INTO Payments
                (
                    StudentId,
                    Month,
                    Amount
                )
                VALUES
                (
                    $sid,
                    $month,
                    $amount
                )
                ON CONFLICT(StudentId, Month)
                DO UPDATE SET
                    Amount = excluded.Amount;";

            cmd.Parameters.AddWithValue(
                "$sid",
                studentId);

            cmd.Parameters.AddWithValue(
                "$month",
                month);

            cmd.Parameters.AddWithValue(
                "$amount",
                (double)amount);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Adds the parameters used when inserting
        /// or updating a student.
        /// </summary>
        private static void AddStudentParams(
            SqliteCommand cmd,
            Student s)
        {
            cmd.Parameters.AddWithValue(
                "$id",
                s.Id);

            cmd.Parameters.AddWithValue(
                "$name",
                s.Name);

            cmd.Parameters.AddWithValue(
                "$age",
                s.Age);

            cmd.Parameters.AddWithValue(
                "$dob",
                s.DateOfBirth.ToString("yyyy-MM-dd"));

            cmd.Parameters.AddWithValue(
                "$class",
                s.ClassName);

            cmd.Parameters.AddWithValue(
                "$fee",
                (double)s.MonthlyFee);

            cmd.Parameters.AddWithValue(
                "$notes",
                s.Notes ?? string.Empty);
        }
    }
}
