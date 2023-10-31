using SoftUni.Data;
using SoftUni.Models;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();
            string output = string.Empty;

            //03.Employees Full Information
            //output = GetEmployeesFullInformation(context);

            //04.Employees with Salary Over 50 000
            //output = GetEmployeesWithSalaryOver50000(context);

            //05.Employees from Research and Development
            //output = GetEmployeesFromResearchAndDevelopment(context);

            //06.Adding a New Address and Updating Employee
            //output = AddNewAddressToEmployee(context);

            //07.Employees and Projects
            //output = GetEmployeesInPeriod(context);

            //08.Addresses by Town
            //output = GetAddressesByTown(context);

            //09.Employee 147
            //output = GetEmployee147(context);

            //10.Departments with More Than 5 Employees
            //output = GetDepartmentsWithMoreThan5Employees(context);

            //11.Find Latest 10 Projects
            //output = GetLatestProjects(context);

            //12.Increase Salaries
            //output = IncreaseSalaries(context);

            //13.Find Employees by First Name Starting with "Sa"
            //output = GetEmployeesByFirstNameStartingWithSa(context);

            //14.Delete Project by Id
            //output = DeleteProjectById(context);

            //15.Remove Town
            //output = RemoveTown(context);

            Console.WriteLine(output);
        }
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary,
                    e.EmployeeId
                }).OrderBy(e => e.EmployeeId)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .Where(e => e.Salary > 50_000)
                .OrderBy(e => e.FirstName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Department.Name,
                    e.Salary
                }).Where(e => e.Name == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.Name} - ${employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Address address = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var employee = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");

            employee.Address = address;

            context.SaveChanges();

            var employees = context.Employees
                .Select(e => new
                {
                    e.Address.AddressText,
                    e.AddressId
                }).OrderByDescending(e => e.AddressId)
                .Take(10)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine(e.AddressText);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Projects = e.EmployeesProjects.Where(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003)
                        .Select(p => new
                        {
                            ProjectName = p.Project.Name,
                            StartDate = p.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                            EndDate = p.Project.EndDate != null
                                ? p.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                                : "not finished"
                        })
                }).Take(10).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

                if (e.Projects.Any())
                {
                    sb.AppendLine(string.Join(Environment.NewLine, e.Projects
                        .Select(p => $"--{p.ProjectName} - {p.StartDate} - {p.EndDate}")));
                }
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                .Select(a => new
                {
                    TownName = a.Town.Name,
                    a.AddressText,
                    EmployeesCount = a.Employees.Count
                }).OrderByDescending(a => a.EmployeesCount)
                .ThenBy(a => a.TownName)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var a in addresses)
            {
                sb.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeesCount} employees");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetEmployee147(SoftUniContext context)
        {
            var employeeInfo = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects
                        .Select(p => new
                        {
                            ProjectName = p.Project.Name
                        }).OrderBy(p => p.ProjectName).ToList()
                }).FirstOrDefault();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{employeeInfo.FirstName} {employeeInfo.LastName} - {employeeInfo.JobTitle}");

            foreach (var project in employeeInfo.Projects)
            {
                sb.AppendLine(project.ProjectName);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departmentInfo = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    d.Name,
                    ManagerInfo = context.Employees.Where(m => m.EmployeeId == d.ManagerId)
                        .Select(m => new
                        {
                            m.FirstName,
                            m.LastName
                        }).FirstOrDefault(),
                    EmployeesInfo = d.Employees
                        .Select(e => new
                        {
                            e.FirstName,
                            e.LastName,
                            e.JobTitle
                        }).OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList()
                }).ToList();


            StringBuilder sb = new StringBuilder();

            foreach (var department in departmentInfo)
            {
                sb.AppendLine($"{department.Name} - {department.ManagerInfo.FirstName} {department.ManagerInfo.LastName}");

                foreach (var employee in department.EmployeesInfo)
                {
                    sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetLatestProjects(SoftUniContext context)
        {
            var projectsInfo = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(d => d.Name)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                }).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var project in projectsInfo)
            {
                sb.AppendLine($"{project.Name}");
                sb.AppendLine($"{project.Description}");
                sb.AppendLine($"{project.StartDate}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string IncreaseSalaries(SoftUniContext context)
        {
            string[] allowedDepartments = new string[] { "Engineering", "Tool Design", "Marketing", "Information Services" };

            var employeesWithIncreasedSalaries = context.Employees
                .Where(e => allowedDepartments.Contains(e.Department.Name))                
                .ToList();

            foreach(var employee in employeesWithIncreasedSalaries)
            {
                employee.Salary *= 1.12m;
            }

            context.SaveChanges();

            var employeesInfo = context.Employees
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Where(e => allowedDepartments.Contains(e.Department.Name))
                .Select(e => $"{e.FirstName} {e.LastName} (${e.Salary:f2})")
                .ToList();

            return string.Join(Environment.NewLine, employeesInfo).TrimEnd();
        }
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employeesInfoText = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .Where(e => e.FirstName.ToLower().Substring(0, 2) == "sa")
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => $"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})")
                .ToList();

            return string.Join(Environment.NewLine, employeesInfoText).TrimEnd();
        }
        public static string DeleteProjectById(SoftUniContext context)
        {
            var projectToDeleteFromEmployeesProjects = context.EmployeesProjects.Where(ep => ep.ProjectId == 2);
            context.EmployeesProjects.RemoveRange(projectToDeleteFromEmployeesProjects);

            var projectToDeleteFromProjects = context.Projects.Where(p => p.ProjectId == 2);
            context.Projects.RemoveRange(projectToDeleteFromProjects);

            context.SaveChanges();

            var projectsText = context.Projects
                .Take(10)
                .Select(p => $"{p.Name}")
                .ToList();

            return string.Join(Environment.NewLine, projectsText);
        }
        public static string RemoveTown(SoftUniContext context)
        {
            Town townToDelete = context.Towns.Where(t => t.Name == "Seattle").FirstOrDefault();

            Address[] addressesToDelete = context.Addresses.Where(a => a.TownId == townToDelete.TownId).ToArray();

            Employee[] employeesToRemoveAddressFrom = context.Employees.Where(e => addressesToDelete.Contains(e.Address)).ToArray();

            foreach(var employee in employeesToRemoveAddressFrom)
            {
                employee.AddressId = null;
            }

            context.Addresses.RemoveRange(addressesToDelete);
            context.Towns.RemoveRange(townToDelete);

            context.SaveChanges();

            return $"{addressesToDelete.Count()} addresses in Seattle were deleted";
        }
    }
}