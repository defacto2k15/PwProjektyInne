using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtrEx3.Model
{
    public interface IStorage
    {
         List<Student> getStudents();

         void createStudent(string firstName, string lastName, string indexNo, int groupId, DateTime? birthDate, String birthPlace);

         void updateStudent(Student st);

         void deleteStudent(Student st);

         List<Group> getGroups();
    }
}
