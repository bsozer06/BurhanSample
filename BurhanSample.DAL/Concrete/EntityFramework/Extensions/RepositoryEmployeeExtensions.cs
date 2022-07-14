using BurhanSample.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurhanSample.DataAccess.Concrete.EntityFramework.Extensions
{
    public static class RepositoryEmployeeExtensions
    {
        /// <summary>
        /// Filter the ages of employees.
        /// Author=Burhan Sözer
        /// </summary>
        /// <param name="employees"></param>
        /// <param name="minAge"></param>
        /// <param name="maxAge"></param>
        /// <returns></returns>
        public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees, uint minAge, uint maxAge)
        {
            return employees.Where(emp => emp.Age >= minAge && emp.Age <= maxAge);
        }


        /// <summary>
        /// Search the employees' names.
        /// Author=Burhan Sözer
        /// </summary>
        /// <param name="employees"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public static IQueryable<Employee> Search(this IQueryable<Employee> employees, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return employees;
            }

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return employees.Where(emp => emp.Name.ToLower().Contains(lowerCaseTerm));
        }


    }
}
