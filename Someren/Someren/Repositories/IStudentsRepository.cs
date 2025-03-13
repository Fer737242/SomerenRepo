using System;
using Someren.Models;

namespace Someren.Repositories
{
    public interface IStudentsRepository
    {
        List<Student> GetAll();
        Student? GetByStudentN(int studentN);
        void Add(Student Student);
        void Update(Student Student);
        void Delete(Student Student);
    }
}

