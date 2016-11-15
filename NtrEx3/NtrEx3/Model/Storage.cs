using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtrEx3.Model
{
    public class Storage : IStorage
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public  List<Student> getStudents()
        {
            using (var db = new StorageContext())
            {
                var students =  db.Students.Include("Group").ToList();
                log.Info("Wczytywanie studentow. Wczytano sztuk: " + students.Count);
                return students;
            }

        }

        public  void createStudent(string firstName, string lastName, string indexNo, int groupId, DateTime? birthDate, String birthPlace)
        {
            using (var db = new StorageContext())
            {
                var group = db.Groups.Find(groupId);
                var student = new Student
                {
                    FirstName = firstName,
                    LastName = lastName,
                    IndexNo = indexNo,
                    Group = group,
                    BirthDate = birthDate,
                    BirthPlace = birthPlace
                };
                db.Students.Add(student);
                log.Info("Tworzenie studenta. Nr. Indeksu: " + student.IndexNo);
                db.SaveChanges();
            }
        }
        public  void updateStudent(Student st)
        {
            using (var db = new StorageContext())
            {
                var original = db.Students.Find(st.StudentId);
                if (original.Stamp != st.Stamp)
                {
                    throw new StorageException("Dane studenta zostaly zmodyfikowane przez kogos innego");
                } else if (original != null)
                {
                    original.FirstName = st.FirstName;
                    original.LastName = st.LastName;
                    original.IndexNo = st.IndexNo;
                    original.IDGroup = st.IDGroup;
                    original.BirthPlace = st.BirthPlace;
                    original.BirthDate = st.BirthDate;
                    log.Info("Aktualizacja studenta. Identyfikator: " + st.StudentId);
                    db.SaveChanges();
                }
                else
                {
                    throw new StorageException("Nie znaleziono pierwotnego studenta - mogl byc skasowany wczesniej");
                }
            }
        }
        public  void deleteStudent(Student st)
        {
            using (var db = new StorageContext())
            {
                var original = db.Students.Find(st.StudentId);
                if (original != null)
                {
                    db.Students.Remove(original);
                    log.Info("Kasowanie studenta. Identyfikator: " + st.StudentId);
                    db.SaveChanges();
                }
                else
                {
                    throw new StorageException("Nie znaleziono studenta do usuniecia - mozliwe ze wykasowal go ktos inny");
                }
            }
        }

        public  List<Group> getGroups()
        {
            using (var db = new StorageContext())
            {
                var groups =  db.Groups.ToList();
                log.Info("Wczytywanie grup. Wczytano sztuk: "+groups.Count);
                return groups;
            }
        }
    }
}
