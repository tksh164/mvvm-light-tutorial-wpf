using System.Collections.ObjectModel;
using System.Linq;
using WpfMvvmLightTutorial.Model;

namespace WpfMvvmLightTutorial.Service
{
    public interface IDataAccessService
    {
        ObservableCollection<Employee> GetEmployees();
        int CreateEmployee(Employee employee);
    }

    public class DataAccessService : IDataAccessService
    {
        private CompanyContext context;

        public DataAccessService()
        {
            context = new CompanyContext();
        }

        public int CreateEmployee(Employee employee)
        {
            context.Employees.Add(employee);
            context.SaveChanges();
            return employee.EmployeeID;
        }

        public ObservableCollection<Employee> GetEmployees()
        {
            return new ObservableCollection<Employee>(context.Employees.AsEnumerable());
        }
    }
}
