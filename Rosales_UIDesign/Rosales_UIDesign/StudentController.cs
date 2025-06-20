
using System;
using System.Collections;
using System.Windows.Forms;

namespace Rosales_UIDesign
{
    public class StudentController
    {
        IStudentRepository studentRepository;
        public StudentController(IStudentRepository repo)
        {
            studentRepository = repo;
        }

        public void SaveStudentRegistration(Student student)
        {
            try
            {
                if (student.FirstName == string.Empty || student.LastName == string.Empty || student.MiddleName == string.Empty || student.Address == string.Empty || student.ContactNumber == string.Empty)
                {
                    throw new Exception("Student Information are Required");
                }
                else if (student.Guardian.FirstName == string.Empty || student.Guardian.LastName == string.Empty || student.Guardian.MiddleName == string.Empty || student.Guardian.Address == string.Empty || student.Guardian.ContactNumber == string.Empty || student.Guardian.Relationship == string.Empty)
                {
                    throw new Exception("Guardian Information are Required");
                }
                else if (student.Requirements.BirthCertificate == false && student.Requirements.GoodMoral == false && student.Requirements.TOR == false)
                {
                    throw new Exception("Provide at least One Requirement");
                }
                else if (string.IsNullOrEmpty(student.Program.ProgramName) || string.IsNullOrEmpty(student.YearLevel))
                {
                    throw new Exception("Academic Information are Required");
                }
                else if (string.IsNullOrEmpty(student.Payment.AmountPaid.ToString()))
                {
                    throw new Exception("Payment Information are required");
                }
                studentRepository.SaveStudentRegistration(student);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateStudentRegistration(Student student)
        {
            try
            {
                if (student.FirstName == string.Empty || student.LastName == string.Empty || student.MiddleName == string.Empty || student.Address == string.Empty || student.ContactNumber == string.Empty)
                {
                    throw new Exception("Student Information are Required");
                }
                else if (student.Guardian.FirstName == string.Empty || student.Guardian.LastName == string.Empty || student.Guardian.MiddleName == string.Empty || student.Guardian.Address == string.Empty || student.Guardian.ContactNumber == string.Empty || student.Guardian.Relationship == string.Empty)
                {
                    throw new Exception("Guardian Information are Required");
                }
                else if (student.Requirements.BirthCertificate == false && student.Requirements.GoodMoral == false && student.Requirements.TOR == false)
                {
                    throw new Exception("Provide at least One Requirement");
                }
                studentRepository.UpdateStudentRegistration(student);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void DeleteStudent(string id)
        {
            try
            {
                studentRepository.DeleteStudent(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Student GetStudentById(string id)
        {
            try
            {
                return studentRepository.GetStudentById(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public ArrayList GetAllStudents()
        {
            try
            {
                return studentRepository.GetAllStudents();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ArrayList GetAllPrograms()
        {
            try
            {
                return studentRepository.GetAllPrograms();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string GenerateNewStudentId()
        {
            try
            {
                return studentRepository.GenerateNewStudentId();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}