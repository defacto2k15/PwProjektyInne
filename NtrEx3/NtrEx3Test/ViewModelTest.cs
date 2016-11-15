using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using NtrEx3.ViewModel;
using NtrEx3.View;
using NtrEx3.Model;
using System.Collections.Generic;
using NtrEx3;
using System.Linq;

namespace NtrEx3Test
{
    [TestClass]
    public class ViewModelTest
    {
        MockRepository mocks;
        MainWindowViewModel viewModel;
        IDialogWindowsGenerator dialogWindowGenerator;
        IStorage storage;
        IValidationEnforcer validationEnfoncer;

        static List<Group> groups = new List<Group>(new Group[] { new Group { IDGroup = 1, Name = "GrupaA" }, new Group { IDGroup = 2, Name = "GrupaB" }, new Group { IDGroup = 3, Name = "GrupaC" } });
        List<Student> students = new List<Student>(new Student[] {
                new Student{ StudentId=0, FirstName="bartek", LastName="Czerwinski", IndexNo="226564", BirthDate=DateTime.Parse("2000-10-10"), BirthPlace="Warszawa", IDGroup=1, Group=groups[0]},
                new Student{ StudentId=1, FirstName="Marek", LastName="Nowak", IndexNo="12265641", BirthDate=DateTime.Now, BirthPlace="Gdansk", IDGroup=1, Group=groups[0]},
                new Student{ StudentId=2, FirstName="Darek", LastName="Jackowski", IndexNo="22265364", BirthDate=DateTime.Now, BirthPlace="Lodz", IDGroup=2, Group=groups[1]},
                new Student{ StudentId=1, FirstName="Marek", LastName="Nowak", IndexNo="32265641", BirthDate=DateTime.Now, BirthPlace="lXGdansk", IDGroup=1, Group=groups[0]},
                new Student{ StudentId=2, FirstName="Darek", LastName="Jackowski", IndexNo="42265364", BirthDate=DateTime.Now, BirthPlace="Lodz", IDGroup=2, Group=groups[1]},
                new Student{ StudentId=1, FirstName="Marek", LastName="Nowak", IndexNo="52265641", BirthDate=DateTime.Now, BirthPlace="Regdansk", IDGroup=1, Group=groups[0]},
                new Student{ StudentId=2, FirstName="Darek", LastName="Jackowski", IndexNo="62265364", BirthDate=DateTime.Now, BirthPlace="Lodz", IDGroup=2, Group=groups[1]},
                new Student{ StudentId=1, FirstName="Marek", LastName="Nowak", IndexNo="72265641", BirthDate=DateTime.Now, BirthPlace="Gdansk", IDGroup=1, Group=groups[0]},
                new Student{ StudentId=2, FirstName="Darek", LastName="Jackowski", IndexNo="82265364", BirthDate=DateTime.Now, BirthPlace="Lodz", IDGroup=2, Group=groups[1]},
                new Student{ StudentId=1, FirstName="Marek", LastName="Nowak", IndexNo="92265641", BirthDate=DateTime.Now, BirthPlace="Gdansk", IDGroup=2, Group=groups[1]},
                new Student{ StudentId=2, FirstName="Darek", LastName="Jackowski", IndexNo="102265364", BirthDate=DateTime.Now, BirthPlace="Lodz", IDGroup=2, Group=groups[1]},
                new Student{ StudentId=1, FirstName="Marek", LastName="Nowak", IndexNo="112265641", BirthDate=DateTime.Now, BirthPlace="Gdansk", IDGroup=2, Group=groups[1]},
                new Student{ StudentId=2, FirstName="Darek", LastName="Jackowski", IndexNo="122265364", BirthDate=DateTime.Now, BirthPlace="Lodz", IDGroup=2, Group=groups[1]},
                new Student{ StudentId=1, FirstName="Marek", LastName="Nowak", IndexNo="132265641", BirthDate=DateTime.Now, BirthPlace="Gdansk", IDGroup=1, Group=groups[0]},
                new Student{ StudentId=2, FirstName="Darek", LastName="Jackowski", IndexNo="142265364", BirthDate=DateTime.Now, BirthPlace="Lodz", IDGroup=2, Group=groups[1]}
            });

        [TestInitialize]
        public void setUpMocks()
        {
            mocks = new MockRepository();
            dialogWindowGenerator = (IDialogWindowsGenerator)mocks.StrictMock(typeof(IDialogWindowsGenerator));
            storage = (IStorage)mocks.StrictMock(typeof(IStorage));
            validationEnfoncer = (IValidationEnforcer)mocks.StrictMock(typeof(IValidationEnforcer));
           
        }

        private void setStandartInputMocks()
        {
            storage.Stub(c => c.getStudents()).Return(students);
            storage.Stub(c => c.getGroups()).Return(groups);
            validationEnfoncer.Stub(v => v.validateInputFields());
        }

        [TestMethod]
        public void AfterStartupThereIsNineFirstStudentsInListView()
        {
            setStandartInputMocks();
            mocks.ReplayAll();
            viewModel = new MainWindowViewModel(storage, dialogWindowGenerator, validationEnfoncer);

            CollectionAssert.AreEqual(students.GetRange(0, Constants.MAX_STUDENTS_PER_PAGE), viewModel.Students);
        }

        [TestMethod]
        public void AfterStartupThereIsTwoPagesAndButtonClickingChangesPage()
        {
            setStandartInputMocks();
            mocks.ReplayAll();
            viewModel = new MainWindowViewModel(storage, dialogWindowGenerator, validationEnfoncer);

            Assert.IsFalse(viewModel.PreviousPageCommand.CanExecute(null));
            Assert.IsTrue(viewModel.NextPageCommand.CanExecute(null));
            Assert.AreEqual(1, viewModel.CurrentPageNumber);
            Assert.AreEqual(2, viewModel.AllPagesCount);

            viewModel.NextPageCommand.Execute(null);

            Assert.IsTrue(viewModel.PreviousPageCommand.CanExecute(null));
            Assert.IsFalse(viewModel.NextPageCommand.CanExecute(null));
            Assert.AreEqual(2, viewModel.CurrentPageNumber);
            Assert.AreEqual(2, viewModel.AllPagesCount);
        }

        [TestMethod]
        public void AfterSelectingOneStudentItsDataIsDisplayedInInputFields()
        {
            setStandartInputMocks();
            mocks.ReplayAll();
            viewModel = new MainWindowViewModel(storage, dialogWindowGenerator, validationEnfoncer);
            Student studentToSelect = students[2];

            viewModel.SelectedStudent = studentToSelect;

            Assert.AreEqual(studentToSelect.FirstName, viewModel.InputName);
            Assert.AreEqual(studentToSelect.LastName, viewModel.InputSurname);
            Assert.AreEqual(studentToSelect.BirthPlace, viewModel.InputBirthplace);
            Assert.AreEqual(studentToSelect.BirthDate, viewModel.InputBirthDate);
            Assert.AreEqual(studentToSelect.IndexNo, viewModel.InputIndexNumber);
            Assert.AreEqual(studentToSelect.Group, viewModel.SelectedInputGroup);
        }

        [TestMethod]
        public void FilteringStudentsByCityNameWorks()
        {
            setStandartInputMocks();
            mocks.ReplayAll();
            viewModel = new MainWindowViewModel(storage, dialogWindowGenerator, validationEnfoncer);

            AssertFilteredStudentsAreEqual(viewModel, students.Where(s => s.BirthPlace.Contains("nsk")).ToList(), filterText: "Gdansk");
            AssertFilteredStudentsAreEqual(viewModel, students.Where(s => s.BirthPlace.Contains("nsk")).ToList(), filterText: "gdansk");
            AssertFilteredStudentsAreEqual(viewModel, students.Where(s => s.BirthPlace.Contains("nsk")).ToList(), filterText: "AnS");
        }

        [TestMethod]
        public void FilteringStudentsByGroupWorks()
        {
            setStandartInputMocks();
            mocks.ReplayAll();
            viewModel = new MainWindowViewModel(storage, dialogWindowGenerator, validationEnfoncer);

            Group filterGroup = groups[1];
            AssertFilteredStudentsAreEqual(viewModel, students.Where(s => s.Group.Equals(filterGroup)).ToList(), filterGroup: filterGroup);
        }

        [TestMethod]
        public void FilteringStudentsByGroupAndCityWorks()
        {
            setStandartInputMocks();
            mocks.ReplayAll();
            viewModel = new MainWindowViewModel(storage, dialogWindowGenerator, validationEnfoncer);

            Group filterGroup = groups[1];
            String filterCity = "GdAnSk";
            AssertFilteredStudentsAreEqual(viewModel, students
                .Where(s => s.Group.Equals(filterGroup))
                .Where(s => s.BirthPlace.ToLower().Contains(filterCity.ToLower()))
                .ToList(), 
                    filterGroup: filterGroup, filterText: filterCity);
        }

        [TestMethod]
        public void FilteringStudentsAndThenResetingFilterMakesResultWindowAsBefore()
        {
            setStandartInputMocks();
            mocks.ReplayAll();
            viewModel = new MainWindowViewModel(storage, dialogWindowGenerator, validationEnfoncer);

            viewModel.SelectedSearchCities = "GdaNSk";
            viewModel.SelectedSearchGroup = groups[1];
            viewModel.FilterButtonCommand.Execute(null);
            viewModel.ClearButtonCommand.Execute(null);

            CollectionAssert.AreEqual(students.GetRange(0, Constants.MAX_STUDENTS_PER_PAGE), viewModel.Students);

            Assert.IsFalse(viewModel.PreviousPageCommand.CanExecute(null));
            Assert.IsTrue(viewModel.NextPageCommand.CanExecute(null));
            Assert.AreEqual(1, viewModel.CurrentPageNumber);
            Assert.AreEqual(2, viewModel.AllPagesCount);

            Assert.AreEqual("", viewModel.SelectedSearchCities);
            Assert.AreEqual(Constants.EMPTY_GROUP, viewModel.SelectedSearchGroup);
        }

        private void AssertFilteredStudentsAreEqual(MainWindowViewModel viewModel, List<Student> expectedStudents, String filterText = null, Group filterGroup=null)
        {
            if (filterText != null)
            {
                viewModel.SelectedSearchCities = filterText;
            }
            if (filterGroup != null)
            {
                viewModel.SelectedSearchGroup = filterGroup;
            }
            viewModel.FilterButtonCommand.Execute(null);
            CollectionAssert.AreEqual(expectedStudents, viewModel.Students);
        }

        [TestMethod]
        public void ButtonsAreDisabledAndEnabled()
        {
            setStandartInputMocks();
            mocks.ReplayAll();
            viewModel = new MainWindowViewModel(storage, dialogWindowGenerator, validationEnfoncer);

            Assert.IsNull(viewModel.SelectedStudent);
            buttonsAreInSelectedModifiedUserState();

            Student student = students[1];
            String originalStudentName = student.FirstName;
            viewModel.SelectedStudent = student;

            buttonsAreInSelectedUnchangedUserState();

            viewModel.InputName = "JakiesInneImie";

            buttonsAreInSelectedModifiedUserState();

            viewModel.InputName = originalStudentName;

            buttonsAreInSelectedUnchangedUserState();
        }

        void buttonsAreInSelectedUnchangedUserState()
        {
            Assert.IsFalse(viewModel.SaveButtonCommand.CanExecute(null));
            Assert.IsFalse(viewModel.NewButtonCommand.CanExecute(null));
            Assert.IsTrue(viewModel.DeleteButtonCommand.CanExecute(null));
        }

        void buttonsAreInSelectedModifiedUserState()
        {
            Assert.IsTrue(viewModel.SaveButtonCommand.CanExecute(null));
            Assert.IsTrue(viewModel.NewButtonCommand.CanExecute(null));
            Assert.IsFalse(viewModel.DeleteButtonCommand.CanExecute(null));
        }

        [TestMethod]
        public void deletingStudentsWorksAsExpected()
        {
            setStandartInputMocks();
            mocks.ReplayAll();
            Student studentToDelete = students[1];
            storage.Expect(c => c.deleteStudent(studentToDelete));
            viewModel = new MainWindowViewModel(storage, dialogWindowGenerator, validationEnfoncer);

            viewModel.SelectedStudent = studentToDelete;

            viewModel.DeleteButtonCommand.Execute(null);
            Assert.IsNull(viewModel.SelectedStudent);
        }

        [TestMethod]
        public void savingStudentsWorksAsExpected()
        {
            setStandartInputMocks();
            mocks.ReplayAll();
            Student initialStudent = students[1];
            String newName = "JakiesNoweImie";
            Student studentAfterModification = new Student{
                FirstName = initialStudent.FirstName,
                LastName = initialStudent.LastName,
                BirthDate = initialStudent.BirthDate,
                BirthPlace = initialStudent.BirthPlace,
                Group = initialStudent.Group,
                IDGroup = initialStudent.IDGroup,
                IndexNo = initialStudent.IndexNo
            };
            storage.Expect(c => c.updateStudent(studentAfterModification));
            viewModel = new MainWindowViewModel(storage, dialogWindowGenerator, validationEnfoncer);

            viewModel.SelectedStudent = initialStudent;

            viewModel.SaveButtonCommand.Execute(null);
        }

        [TestMethod]
        public void creatingStudentWorksAsExpected()
        {
            setStandartInputMocks();
            mocks.ReplayAll();
            String firstName = "FirstName";
            String lastName = "LastName";
            String indexNo = "1231231";
            Group selectedGroup = groups[0];
            DateTime birthDate = DateTime.Now;
            String birthPlace = "Warszawa";
            storage.Expect(c => c.createStudent(firstName, lastName, indexNo, selectedGroup.IDGroup, birthDate, birthPlace));
            viewModel = new MainWindowViewModel(storage, dialogWindowGenerator, validationEnfoncer);

            viewModel.InputName = firstName;
            viewModel.InputSurname = lastName;
            viewModel.InputIndexNumber = indexNo;
            viewModel.SelectedInputGroup = selectedGroup;
            viewModel.InputBirthDate = birthDate;
            viewModel.InputBirthplace = birthPlace;

            viewModel.NewButtonCommand.Execute(null);
        }
    }
}
