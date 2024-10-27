using Employee_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class EmployeeController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly IHolidayService _holidayService; 

    public EmployeeController(IEmployeeService employeeService, IHolidayService holidayService)
    {
        _employeeService = employeeService;
        _holidayService = holidayService;  
    }

    public async Task<IActionResult> Index()
    {
        var employees = await _employeeService.GetAllEmployeesCached();
        return View(employees);
    }

    
    public IActionResult AddEmployee()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddEmployee(Employee employee)
    {
        if (ModelState.IsValid)
        {
            await _employeeService.AddEmployee(employee);
            return RedirectToAction(nameof(Index));
        }
        return View(employee);
    }

    
    public async Task<IActionResult> UpdateEmployee(int id)
    {
        var employee = await _employeeService.GetEmployeeById(id);
        if (employee == null)
        {
            return NotFound();
        }
        return View(employee);
    }

    
    [HttpPost]
    public async Task<IActionResult> UpdateEmployee(Employee employee)
    {
        if (ModelState.IsValid)
        {
            await _employeeService.UpdateEmployee(employee);
            return RedirectToAction(nameof(Index));
        }
        return View(employee);
    }

    public class EmailAddressAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var email = value as string;

            if (string.IsNullOrWhiteSpace(email))
            {
                return new ValidationResult("Email is required.");
            }
            //adding validation for email adreess
            var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.IgnoreCase);

            if (!regex.IsMatch(email))
            {
                return new ValidationResult("Invalid email format. Please use a valid email address.");
            }

            return ValidationResult.Success;
        }
    }

    
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        await _employeeService.DeleteEmployee(id);
        return RedirectToAction(nameof(Index));
    }

    
    public IActionResult CalculateWorkingDays()
    {
        return View(new WorkingDaysModel());  
    }

    [HttpPost]
    public async Task<IActionResult> CalculateWorkingDays(WorkingDaysModel model) 
    {
        
        if (model.StartDate.DayOfWeek == DayOfWeek.Saturday || model.StartDate.DayOfWeek == DayOfWeek.Sunday)
        {
            //check if start day is a weekday
            ModelState.AddModelError(nameof(model.StartDate), "Start date must be a weekday.");
            return View(model); 
        }

        if (model.EndDate < model.StartDate)
        {
            //check if start date is befire the enddate
            ModelState.AddModelError(nameof(model.EndDate), "End date must be after start date.");
            return View(model);
        }

        model.WorkingDays = await _holidayService.CalculateWorkingDays(model.StartDate, model.EndDate);
        return View(model);  
    }

}
