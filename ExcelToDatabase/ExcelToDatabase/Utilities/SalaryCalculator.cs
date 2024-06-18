using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelToDatabase.Models;

namespace ExcelToDatabase.Utilities
{
    public class SalaryCalculator
    {
        public static Salary CalculateSalary(decimal amountTotal)
        {
            Salary salaryModel = new Salary();

            // Calculate Earned Basic
            decimal earnedBasicDecimal = amountTotal * 0.4m;
            salaryModel.EarnedBasic = earnedBasicDecimal.ToString("F2");

            // Calculate HRA
            decimal hraDecimal = earnedBasicDecimal * 0.4m;
            salaryModel.HRA = hraDecimal.ToString("F2");

            // Calculate Special Allowance
            decimal specialAllowanceDecimal = amountTotal - earnedBasicDecimal - hraDecimal;
            salaryModel.SpecialAllowance = specialAllowanceDecimal.ToString("F2");

            return salaryModel;
        }
    }
}
