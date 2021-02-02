using System;
using System.Linq;
using API.Models;
using NLog;

namespace API.Helpers
{
    public class PasswordHelper
    {
        private EmployeesContext employeesContext;
        private readonly ILogger logger;

        public PasswordHelper(EmployeesContext _context,ILogger _logger)
        {
            employeesContext = _context;
            logger = _logger;
        }

        // This will check the password, and since the encryption key is also the password in
        // clear text, we don't pass a seperate key
        public bool CheckPassword(string userName, string password)
        {
            try
            {
                var employee = employeesContext.Employees
                    .Where<Employee>(o => o.UserName == userName).FirstOrDefault<Employee>();

                if(employee.Password == password)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                logger.Log(LogLevel.Error,ex);
                return false;
            }
        }

        // This will take the password, presented in clear-text,
        //and store it encrypted, using the pasword itself as the key
        public bool SetPassword(string userName, string password)
        {
            try
            {
                var emp = employeesContext.Employees
                    .Where<Employee>(o => o.UserName == userName).FirstOrDefault<Employee>();

                emp.Password = password;
                return true;
            }
            catch(Exception ex)
            {
                logger.Log(LogLevel.Error,ex);
                return false;
            }
        }
    }
}