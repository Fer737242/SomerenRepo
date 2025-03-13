using Microsoft.Data.SqlClient;
using Someren.Models;

namespace Someren.Repositories
{
    public class DbLecturersRepository : ILecturersRepository
    {
        private readonly string? _connectionString;

        public DbLecturersRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("projectdatabase");
        }

        private Lecturer ReadLecturer(SqlDataReader reader)
        {
            int employeeN = (int)reader["employee_n"];
            string first_name = (string)reader["first_name"];
            string last_name = (string)reader["last_name"];
            string phone_n = (string)reader["phone_n"];
            string age = (string)reader["age"];
            //string isDeleted = (string)reader["IsDeleted"];

            return new Lecturer(employeeN, first_name, last_name, phone_n, age); //isDeleted);
        }

        public List<Lecturer> GetAll()
        {
            List<Lecturer> lecturers = new List<Lecturer>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT employee_n, first_name, last_name, phone_n, age FROM LECTURER";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Lecturer lecturer = ReadLecturer(reader);
                    lecturers.Add(lecturer);
                }
                reader.Close();
            }
            return lecturers;
        }

        public void Add(Lecturer lecturer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO LECTURER (first_name, last_name, phone_n, age) VALUES (@employee_n, @first_name, @last_name, @phone_n, @age)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@first_name", lecturer.FirstName);
                command.Parameters.AddWithValue("@last_name", lecturer.LastName);
                command.Parameters.AddWithValue("@phone_n", lecturer.PhoneN);
                command.Parameters.AddWithValue("@age", lecturer.Age);
                //command.Parameters.AddWithValue("@IsDeleted", user.IsDeleted);

                command.Connection.Open();
                command.ExecuteNonQuery();
                lecturer.EmployeeN = Convert.ToInt32(command.ExecuteScalar()); //I added this manually
            }
        }

        public void Update(Lecturer lecturer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE LECTURER SET FirstName = @FirstName, LastName = @LastName, PhoneN = @PhoneN, Age = @Age WHERE EmployeeN = @EmployeeN";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeId", lecturer.EmployeeN);
                command.Parameters.AddWithValue("@first_name", lecturer.FirstName);
                command.Parameters.AddWithValue("@last_name", lecturer.LastName);
                command.Parameters.AddWithValue("@phone_n", lecturer.PhoneN);
                command.Parameters.AddWithValue("@age", lecturer.Age);
                //command.Parameters.AddWithValue("@IsDeleted", user.IsDeleted);

                command.Connection.Open();
                command.ExecuteNonQuery();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records updated!");
            }
        }

        public Lecturer GetByEmployeeN(int employeeN)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT EmployeeN, FirstName, LastName, PhoneN, Age FROM LECTURER WHERE EmployeeN = @EmployeeN";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", employeeN);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return ReadLecturer(reader);
                }
                else
                {
                    return null;
                }
            }
        }

        public void Delete(Lecturer lecturer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"DELETE FROM LECTURER WHERE LecturerN= @LecturerN";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", lecturer.EmployeeN);

                command.Connection.Open();
                command.ExecuteNonQuery();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records deleted!");
            }
        }
    }
}
