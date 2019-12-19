using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OA.Data.Entities
{
    public class DepartmentEmployee : BaseEntity
    {
        public DateTime ToDate 
        {
            get; set; 
        }
    }
}
