using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OA.Data.Entities
{
    public class Title : BaseEntity
    {
        public DateTime ToDate
        {
            get; set;
        }
        public Employee Employee
        {
            get; set;
        }
    }
}
