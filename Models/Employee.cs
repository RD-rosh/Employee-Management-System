using System;
using System.Collections.Generic;
using static EmployeeController;

namespace Employee_Management_System.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    [EmailAddress(ErrorMessage = "Enter a valid Email Address")]
    public string Email { get; set; } = null!;

    public string JobPosition { get; set; } = null!;
}
