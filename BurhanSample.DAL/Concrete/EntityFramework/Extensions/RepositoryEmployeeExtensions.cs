using BurhanSample.Entities.Concrete;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;
using BurhanSample.DataAccess.Concrete.EntityFramework.Extensions.Utility;

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

        /// <summary>
        /// Sort based on the field name(s). Querystring can be like ...employees?orderBy=name,age desc
        /// Author=Burhan Sözer 
        /// </summary>
        /// <param name="employees"></param>
        /// <param name="orderByQueryString"></param>
        /// <returns></returns>
        public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return employees.OrderBy(e => e.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Employee>(orderByQueryString);

            0if (string.IsNullOrWhiteSpace(orderQuery))
                return employees.OrderBy(e => e.Name);          /// System.Linq (normal)

            return employees.OrderBy(orderQuery);               /// System.Linq.Dynamic.Core; (extra package)
        }


    }
}
