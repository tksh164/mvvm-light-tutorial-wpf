using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Linq;
using WpfMvvmLightTutorial.MessageInfrastructure;
using WpfMvvmLightTutorial.Model;
using WpfMvvmLightTutorial.Service;

namespace WpfMvvmLightTutorial.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private IDataAccessService serviceProxy;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataAccessService serviceProxy)
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            this.serviceProxy = serviceProxy;

            // Data binding to View
            Employees = new ObservableCollection<Employee>();

            // Command
            ReadAllCommand = new RelayCommand(GetEmployees);

            // Command wih parameter
            Employee = new Employee();
            SaveCommand = new RelayCommand<Employee>(SaveEmployee);

            // EventToCommand
            SearchCommand = new RelayCommand(SearchEmployee);

            // MVVM Light Messenger
            SendEmployeeCommand = new RelayCommand<Employee>(SendEmployee);
            ReceiveEmployee();
        }

        private ObservableCollection<Employee> employees;
        public ObservableCollection<Employee> Employees
        {
            get
            {
                return employees;
            }
            set
            {
                employees = value;
                RaisePropertyChanged(nameof(Employees));
            }
        }

        private void GetEmployees()
        {
            Employees.Clear();
            foreach (var e in serviceProxy.GetEmployees())
            {
                Employees.Add(e);
            }
        }

        public RelayCommand ReadAllCommand { get; set; }

        private Employee employee;
        public Employee Employee
        {
            get
            {
                return employee;
            }
            set
            {
                employee = value;
                RaisePropertyChanged(nameof(Employee));
            }
        }

        void SaveEmployee(Employee employee)
        {
            Employee.EmployeeID = serviceProxy.CreateEmployee(employee);
            if (Employee.EmployeeID != 0)
            {
                // The notification is unnecessary because the Employees property type is ObservableCollection.
                // The ObservableCollection notifies when added/removed items in it.
                Employees.Add(Employee);

                // This notifies to updated the EmployeeID of Employee.
                RaisePropertyChanged(nameof(Employee));
            }
        }

        /// <summary>
        /// The RelayCommand generic type is declared where T represents the input parameter.
        /// </summary>
        public RelayCommand<Employee> SaveCommand { get; set; }

        private string employeeName;
        public string EmployeeName
        {
            get
            {
                return employeeName;
            }
            set
            {
                employeeName = value;
                RaisePropertyChanged(nameof(EmployeeName));
            }
        }

        private void SearchEmployee()
        {
            Employees.Clear();
            var result = from e in serviceProxy.GetEmployees()
                         where e.EmployeeName.StartsWith(EmployeeName)
                         select e;
            foreach (var e in result)
            {
                Employees.Add(e);
            }
        }

        public RelayCommand SearchCommand { get; set; }

        private void SendEmployee(Employee employee)
        {
            if (employee != null)
            {
                Messenger.Default.Send<MessageCommunicator>(new MessageCommunicator()
                {
                    Employee = employee,
                });
            }
        }

        public RelayCommand<Employee> SendEmployeeCommand { get; set; }

        private void ReceiveEmployee()
        {
            if (Employee != null)
            {
                Messenger.Default.Register<MessageCommunicator>(this, (mc) => {
                    this.Employee = mc.Employee;
                });
            }
        }
    }
}