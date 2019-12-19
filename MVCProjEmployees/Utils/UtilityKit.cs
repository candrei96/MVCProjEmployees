using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MVCProjEmployees.Utils
{
    public static class UtilityKit
    {
        public static string ConstructDepartmentNumber(string numericalString)
        {
            int.TryParse(numericalString, out int number);
            number++;

            string numberString = Convert.ToString(number);
            if (numberString.Length != 3)
            {
                for (int i = numberString.Length; i < 3; i++)
                {
                    numberString = string.Concat("0", numberString);
                }
            }
            else
            {
                throw new Exception("Invalid department string length.");
            }

            string departmentNumber = string.Concat("d", numberString);
            return departmentNumber;
        }
    }
}
