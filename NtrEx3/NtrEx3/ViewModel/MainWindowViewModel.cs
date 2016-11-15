using NtrEx3.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NtrEx3;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data.Entity.Core;
using NtrEx3.View;

namespace NtrEx3.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MainWindowViewModel(IStorage storage, IDialogWindowsGenerator dialogWindowsGenerator, IValidationEnforcer validationEnforcer)
        {
            try
            {
                this.storage = storage;
                this.dialogWindowsGenerator = dialogWindowsGenerator;
                this.validationEnforcer = validationEnforcer;
                this._Groups = storage.getGroups();
                SelectedSearchGroup = Constants.EMPTY_GROUP;
                InputBirthDate = null;
                FilterButtonCommand = new RelayCommand(() => { doFilterAction(); });
                ClearButtonCommand = new RelayCommand(
                    () => { SelectedSearchCities = ""; SelectedSearchGroup = Constants.EMPTY_GROUP; doFilterAction(); },
                    () => { return !((SelectedSearchGroup == null || SelectedSearchGroup == Constants.EMPTY_GROUP) && (SelectedSearchCities == null || SelectedSearchCities == "")); });

                PropertyChanged += (NOT_USED, propertyNamedEventArgs) =>
                {
                    var propertyName = propertyNamedEventArgs.PropertyName;
                    if (propertyName == "SelectedSearchGroup" || propertyName == "SelectedSearchCities")
                    {
                        ClearButtonCommand.RaiseCanExecuteChanged();
                    }
                };

                DeleteButtonCommand = new RelayCommand(
                    () => doRemoveStudentAction(),
                    () => SelectedStudent != null && isUnchangedUser);
                PropertyChanged += (NOT_USED, propertyNamedEventArgs) =>
                {
                    var propertyName = propertyNamedEventArgs.PropertyName;
                    if (propertyName == "SelectedStudent" || propertyName == "IsUnchangedUser")
                    {
                        DeleteButtonCommand.RaiseCanExecuteChanged();
                    }
                };

                NewButtonCommand = new RelayCommand(() => doCreateStudentAction(), () => !isUnchangedUser);
                SaveButtonCommand = new RelayCommand(() => doSaveStudentAction(), () => !isUnchangedUser && SelectedStudent != null);
                PropertyChanged += (NOT_USED, propertyNamedEventArgs) =>
                {
                    var propertyName = propertyNamedEventArgs.PropertyName;
                    List<String> avalibleNames = new List<String>(new String[] { "IsDateTimeEnabled", "SelectedStudent", "InputName", "InputSurname", "SelectedInputGroup", "InputBirthplace", "InputBirthDate", "InputIndexNumber" });
                    if (avalibleNames.Contains(propertyName))
                    {
                        updateIsUnchangedStudent();
                        SaveButtonCommand.RaiseCanExecuteChanged();
                        NewButtonCommand.RaiseCanExecuteChanged();
                    }
                };

                PreviousPageCommand = new RelayCommand(() => CurrentPageNumber--, () => CurrentPageNumber > 1);
                NextPageCommand = new RelayCommand(() => CurrentPageNumber++, () => CurrentPageNumber < AllPagesCount);

                PropertyChanged += (NOT_USED, propertyNamedEventArgs) =>
                {
                    var propertyName = propertyNamedEventArgs.PropertyName;
                    if (propertyName == "AllPagesCount")
                    {
                        NextPageCommand.RaiseCanExecuteChanged();
                        PreviousPageCommand.RaiseCanExecuteChanged();
                    }
                    if (propertyName == "CurrentPageNumber")
                    {
                        NextPageCommand.RaiseCanExecuteChanged();
                        PreviousPageCommand.RaiseCanExecuteChanged();
                        regenerateStudentsListWithPagination();
                    }
                };

                resetInputFields();
                doFilterAction();
            }
            catch (Exception e)
            {
                Console.WriteLine("Wystapil wyjatek: " + e.ToString());
            }
        }

        private IStorage storage;
        private IDialogWindowsGenerator dialogWindowsGenerator;
        private IValidationEnforcer validationEnforcer;

        public RelayCommand FilterButtonCommand { get; private set; }
        public RelayCommand ClearButtonCommand { get; private set; }
        public RelayCommand NewButtonCommand { get; private set; }
        public RelayCommand SaveButtonCommand { get; private set; }
        public RelayCommand DeleteButtonCommand { get; private set; }

        bool isUnchangedUser = false;

        void updateIsUnchangedStudent()
        {
            bool newValue = false;
            if ( SelectedStudent == null)
            {
                newValue =  false;
            }
            else
            {
                var newCreatedStudent = createStudentFromInputFields();
                newValue =
                    SelectedStudent.FirstName == newCreatedStudent.FirstName &&
                    SelectedStudent.LastName == newCreatedStudent.LastName &&
                    SelectedStudent.IDGroup == newCreatedStudent.IDGroup &&
                    SelectedStudent.BirthDate == newCreatedStudent.BirthDate &&
                    SelectedStudent.BirthPlace == newCreatedStudent.BirthPlace &&
                    SelectedStudent.IndexNo == newCreatedStudent.IndexNo;
            }
            if (newValue != isUnchangedUser)
            {
                isUnchangedUser = newValue;
                OnPropertyChanged("IsUnchangedUser");
            }
            else
            {
                isUnchangedUser = newValue;
            }
        }

        private List<Student> allStudents = new List<Student>();
        
        void doFilterAction()
        {
            var filteredStudents = from student in storage.getStudents()
                                   where (SelectedSearchGroup == null) || SelectedSearchGroup == Constants.EMPTY_GROUP || student.Group.Equals( SelectedSearchGroup)
                                    where (SelectedSearchCities == null || SelectedSearchCities.Trim() == "" ||  student.BirthPlace.ToLower().Contains( SelectedSearchCities.Trim().ToLower()))
                                    select student;
            allStudents = filteredStudents.ToList();

            
            AllPagesCount = ((int)Math.Ceiling(((double)(allStudents.Count ) / Constants.MAX_STUDENTS_PER_PAGE))) ;
            if (AllPagesCount == 0)
            {
                CurrentPageNumber = 0;
            }
            else
            {
                CurrentPageNumber = 1;
            }

            SelectedStudent = null;
            resetInputFields();
        }

        private void regenerateStudentsListWithPagination(){
            int startIndex = Math.Max(0, (CurrentPageNumber - 1) * Constants.MAX_STUDENTS_PER_PAGE);
            int endIndex = Math.Min(allStudents.Count, startIndex + Constants.MAX_STUDENTS_PER_PAGE);
            Students = allStudents.GetRange(startIndex, endIndex - startIndex);
            Console.WriteLine("Getting from " + startIndex + " to " + endIndex+" got "+Students.Count);
        }

        void resetInputFields()
        {
            _InputName = "";
            _InputSurname = "";
            _InputIndexNumber = "";
            _InputBirthDate = null;
            _IsDateTimeEnabled = false;
            _InputBirthplace = "";
            _SelectedInputGroup = Groups[0];
            OnPropertyChanged("InputName");
            OnPropertyChanged("InputSurname");
            OnPropertyChanged("InputIndexNumber");
            OnPropertyChanged("InputBirthDate");
            OnPropertyChanged("InputBirthplace");
            OnPropertyChanged("SelectedInputGroup");
            validationEnforcer.validateInputFields();
        }

        void doSelectedStudentAction()
        {
            if (SelectedStudent == null)
            {
                
            }
            else
            {
                InputName = SelectedStudent.FirstName;
                InputSurname = SelectedStudent.LastName;
                InputIndexNumber = SelectedStudent.IndexNo;
                InputBirthDate = SelectedStudent.BirthDate;
                InputBirthplace = SelectedStudent.BirthPlace;
                SelectedInputGroup = SelectedStudent.Group;
            }
        }

        void doRemoveStudentAction()
        {
            try
            {
                storage.deleteStudent(SelectedStudent);
            }
            catch (StorageException e)
            {
                log.Error("Nieudane kasowanie studenta: ", e);
                dialogWindowsGenerator.generateWarningWindow("Nieudane kasowanie: " + e.ToString());
            }
            doFilterAction();
        }

        void doCreateStudentAction()
        {
            if (!isUnchangedUser)
            {
                try
                {
                    storage.createStudent(InputName, InputSurname, InputIndexNumber, SelectedInputGroup.IDGroup, InputBirthDate, InputBirthplace);
                    doFilterAction();
                }
                catch (DbUpdateException e)
                {
                    log.Error("Nieudane tworzenie studenta: ", e);
                    if (e.InnerException is UpdateException
                        && e.InnerException.InnerException is SqlException
                        && DBHelpers.IsUniqueKeyViolation((SqlException)e.InnerException.InnerException))
                    {
                        dialogWindowsGenerator.generateWarningWindow("Problem: Istnieje juz student o indeksie " + InputIndexNumber);
                    }
                    else
                    {
                        dialogWindowsGenerator.generateWarningWindow("Nieudany zapis studenta: " + e.ToString());
                        doFilterAction();
                    }
                }
                
            }
        }

        private Student createStudentFromInputFields()
        {
            Student student = new Student();
            if( SelectedStudent != null ){
                student.StudentId = SelectedStudent.StudentId;
                student.Stamp = SelectedStudent.Stamp;
            }
            student.FirstName = InputName;
            student.LastName = InputSurname;
            student.IDGroup = SelectedInputGroup == null ? -22 : SelectedInputGroup.IDGroup; // todo
            student.BirthDate = InputBirthDate;
            student.BirthPlace = InputBirthplace;
            student.IndexNo = InputIndexNumber;
            return student;
        }

        void doSaveStudentAction()
        {
            if (!isUnchangedUser && SelectedStudent != null)
            {
                var student = createStudentFromInputFields();
                try
                {
                    storage.updateStudent(student);
                    doFilterAction();
                }
                catch (DbUpdateException e)
                {
                    log.Error("Nieudane zapisywanie studenta: ", e);
                    if (e.InnerException is UpdateException
                        && e.InnerException.InnerException is SqlException
                        && DBHelpers.IsUniqueKeyViolation((SqlException)e.InnerException.InnerException))
                    {
                        dialogWindowsGenerator.generateWarningWindow("Problem: Istnieje juz student o indeksie " + student.IndexNo);
                    }
                    else
                    {
                        dialogWindowsGenerator.generateWarningWindow("Nieudany zapis studenta: " + e.ToString());
                        doFilterAction();
                    }
                }
                catch (StorageException e)
                {
                    log.Error("Nieudane zapisywanie studenta: ", e);
                    dialogWindowsGenerator.generateWarningWindow("Nieudany zapis : " + e.ToString());
                    doFilterAction();
                }
            }
        }

        private List<Student> _Students;
        public List<Student> Students
        {
            get
            {
                return _Students;
            }
            set
            {
                _Students = value;
                OnPropertyChanged("Students");
            }
        }

        public List<Group> FilterGroups
        {
            get
            {
                List<Group> allGroups = Groups;
                allGroups.Add(Constants.EMPTY_GROUP);
                return allGroups;
            }
        }

        public List<Group> _Groups;
        public List<Group> Groups
        {
            get
            {
                return _Groups;
            }
        }

        private Group _SelectedSearchGroup;
        public Group SelectedSearchGroup
        {
            get { return _SelectedSearchGroup; }
            set {
                if (value == Constants.EMPTY_GROUP)
                {
                    _SelectedSearchGroup = value;
                }
                else
                {
                    _SelectedSearchGroup = Groups.Where(g => g.IDGroup == value.IDGroup).First(); 
                }
                OnPropertyChanged("SelectedSearchGroup"); 
            } 
        }

        private String _SelectedSearchCities;
        public String SelectedSearchCities
        {
            get { return _SelectedSearchCities; }
            set { _SelectedSearchCities = value; OnPropertyChanged("SelectedSearchCities"); }
        }

        private Group _SelectedInputGroup;
        public Group SelectedInputGroup
        {
            get { return _SelectedInputGroup; }
            set {
                _SelectedInputGroup = Groups.Where( g => g.IDGroup == value.IDGroup).First(); 
                OnPropertyChanged("SelectedInputGroup"); 
            }
        }

        private String _InputName;
        public String InputName
        {
            get { return _InputName; }
            set { if (value == "" || value == null) { throw new ArgumentException("Imie jest polem wymaganym"); } _InputName = value; OnPropertyChanged("InputName"); }
        }

        private String _InputSurname;
        public String InputSurname
        {
            get { return _InputSurname; }
            set { if (value == "" || value == null) { throw new ArgumentException("Nazwisko jest polem wymaganym"); } _InputSurname = value; OnPropertyChanged("InputSurname"); }
        }

        private String _InputBirthplace;
        public String InputBirthplace
        {
            get { return _InputBirthplace; }
            set { _InputBirthplace = value;  OnPropertyChanged("InputBirthplace"); }
        }

        private DateTime? _InputBirthDate;
        public DateTime? InputBirthDate
        {
            get {
                if (IsDateTimeEnabled)
                {
                    return _InputBirthDate;
                }
                else
                {
                    return null;
                }
            }
            set {
                if (value == null)
                {
                    IsDateTimeEnabled = false;
                }
                else
                {
                    IsDateTimeEnabled = true;
                }
                _InputBirthDate = value;  
                OnPropertyChanged("InputBirthDate"); 
            }
        }

        private bool _IsDateTimeEnabled;
        public bool IsDateTimeEnabled
        {
            get { return _IsDateTimeEnabled; }
            set { _IsDateTimeEnabled = value; OnPropertyChanged("IsDateTimeEnabled"); }
        }

        private String _InputIndexNumber;
        public String InputIndexNumber
        {
            get { return _InputIndexNumber; }
            set {
                if (value == null || value == String.Empty)
                {
                    throw new ArgumentException("Numer indeksu jest obowiazkowy");
                }
                else if (System.Text.RegularExpressions.Regex.Matches(value, "^[0-9]+$", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Count == 0)
                {
                    throw new ArgumentException("W numerze indeku moga znajdowac sie jedynie cyfry");
                }
                _InputIndexNumber = value; 
                OnPropertyChanged("InputIndexNumber"); 
            }
        }


        public RelayCommand PreviousPageCommand { get; private set; }
        public RelayCommand NextPageCommand { get; private set; }

        private int _CurrentPageNumber;
        public int CurrentPageNumber
        {
            get { return _CurrentPageNumber; }
            private set
            {
                _CurrentPageNumber = value;
                OnPropertyChanged("CurrentPageNumber");
            }
        }

        private int _AllPagesCount;
        public int AllPagesCount
        {
            get { return _AllPagesCount; }
            private set
            {
                _AllPagesCount = value;
                OnPropertyChanged("AllPagesCount");
            }
        }


        private Student _SelectedStudent;
        public Student SelectedStudent
        {
            get { return _SelectedStudent; }
            set { _SelectedStudent = value; doSelectedStudentAction(); OnPropertyChanged("SelectedStudent"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null) this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
