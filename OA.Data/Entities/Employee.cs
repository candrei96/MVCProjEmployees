using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OA.Data.Entities
{
    public class Employee : BaseEntity
    {
        public DateTime BirthDate
        {
            get; set;
        }

        public string FirstName 
        { 
            get; set; 
        }
        public string LastName 
        {
            get; set; 
        }
        public string Gender 
        { 
            get; set; 
        }
        public DateTime HireDate 
        { 
            get; set; 
        }
    }
}
