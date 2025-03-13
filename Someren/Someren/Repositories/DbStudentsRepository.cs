using Microsoft.Data.SqlClient;
using Someren.Models;

namespace Someren.Repositories
{
    public class DbStudentsRepository : IStudentsRepository
    {
        private readonly string? _connectionString;

        public DbStudentsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("projectdatabase");
        }

        private Student ReadStudent(SqlDataReader reader)
        {
            int StudentN = (int)reader["Student_n"];
            string first_name = (string)reader["first_name"];
            string last_name = (string)reader["last_name"];
            string phone_n = (string)reader["phone_n"];
            string classN = (string)reader["classN"];
            //string isDeleted = (string)reader["IsDeleted"];

            return new Student(StudentN, first_name, last_name, phone_n, classN); //isDeleted);
        }

        public List<Student> GetAll()
        {
            List<Student> Students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Student_n, first_name, last_name, phone_n, classN FROM Student";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Student Student = ReadStudent(reader);
                    Students.Add(Student);
                }
                reader.Close();
            }
            return Students;
        }

        public void Add(Student Student)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Student (first_name, last_name, phone_n, age) VALUES (@Student_n, @first_name, @last_name, @phone_n, @age)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@first_name", Student.FirstName);
                command.Parameters.AddWithValue("@last_name", Student.LastName);
                command.Parameters.AddWithValue("@phone_n", Student.PhoneN);
               
                //command.Parameters.AddWithValue("@IsDeleted", user.IsDeleted);

                command.Connection.Open();
                command.ExecuteNonQuery();
                Student.StudentN = Convert.ToInt32(command.ExecuteScalar()); //I added this manually
            }
        }

        public void Update(Student Student)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Student SET FirstName = @FirstName, LastName = @LastName, PhoneN = @PhoneN, Age = @Age WHERE StudentN = @StudentN";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StudentId", Student.StudentN);
                command.Parameters.AddWithValue("@first_name", Student.FirstName);
                command.Parameters.AddWithValue("@last_name", Student.LastName);
                command.Parameters.AddWithValue("@phone_n", Student.PhoneN);
                //command.Parameters.AddWithValue("@IsDeleted", user.IsDeleted);

                command.Connection.Open();
                command.ExecuteNonQuery();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records updated!");
            }
        }

        public Student GetByStudentN(int StudentN)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT StudentN, FirstName, LastName, PhoneN, ClassN FROM Student WHERE StudentN = @StudentN";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", StudentN);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return ReadStudent(reader);
                }
                else
                {
                    return null;
                }
            }
        }

        public void Delete(Student Student)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"DELETE FROM Student WHERE StudentN= @StudentN";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", Student.StudentN);

                command.Connection.Open();
                command.ExecuteNonQuery();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records deleted!");
            }
        }

        Student? IStudentsRepository.GetByStudentN(int studentN)
        {
            throw new NotImplementedException();
        }
    }
}
