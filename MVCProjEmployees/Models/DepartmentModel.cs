using MVCProjEmployees.Exceptions;

namespace MVCProjEmployees.Models
{
    public class DepartmentModel
    {
        public string DepartmentNumber
        {
            get; set;
        }
        public string DepartmentName
        {
            get; set;
        }

        public override string ToString()
        {
            return $"Department {DepartmentNumber}, name: {DepartmentName}";
        }
    }
}
