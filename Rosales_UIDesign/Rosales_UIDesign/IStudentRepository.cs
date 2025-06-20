using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosales_UIDesign
{
    public interface IStudentRepository
    {
        void SaveStudentRegistration(Student student);
        void UpdateStudentRegistration(Student student);
        void DeleteStudent(string id);
        Student GetStudentById(string id);
        ArrayList GetAllStudents();
        ArrayList GetAllPrograms();
        string GenerateNewStudentId();
    }
}
