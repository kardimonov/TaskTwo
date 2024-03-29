﻿using System.Collections.Generic;
using TaskTwo.Data.Models;

namespace TaskTwo.Web.ViewModels.SubjectVM
{
    public class SubjectDetails
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Employee> Employees { get; set; }
    }
}
