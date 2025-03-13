using System;
namespace Someren.Models
{
    public class Student
    {
        public int StudentN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneN { get; set; }
        public string ClassN { get; set; }
        //public string Age { get; set; }
        //public string IsDeleted { get; set; }

        public Student(int studentN, string firstName, string lastName, string phoneN, string classN) // string age, string isDeleted)
        {
            StudentN = studentN;
            FirstName = firstName;
            LastName = lastName;
            PhoneN = phoneN;
            ClassN = classN;
            //Age = age;
            //IsDeleted = isDeleted;
        }
        public Student() { }
    }
}

