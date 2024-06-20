using System.Data.SqlClient;
using ExcelToDatabase.Models;
using ExcelToDatabase.Services.Interface;

namespace ExcelToDatabase.Service
{
    public class DatabaseService : IDatabaseService
    {
        private readonly string _connectionString;
        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task InsertDataIntoDatabaseAsync(List<SalaryMetadata> data)
        {
            Console.WriteLine("Starting to insert data into the database...");
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                Console.WriteLine("Database connection opened.");
                foreach (var row in data)
                {
                    string insertQuery = @"INSERT INTO SalaryMetadata (Emp_Code,PaySlipForMonth,DaysPaid,AbsentDays,EarnedBasic,HRA,SpecialAllowance,PFEmployeeShare,ProfessionalTax,TDS,EarningTotal,TotalDeductions,NetPay) 
                                           VALUES (@Emp_Code,@PaySlipForMonth,@DaysPaid,@AbsentDays,@EarnedBasic,@HRA,@SpecialAllowance,@PFEmployeeShare,@ProfessionalTax,@TDS,@EarningTotal,@TotalDeductions,@NetPay)";
                    await using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Emp_Code", row.Emp_Code);
                        command.Parameters.AddWithValue("@PaySlipForMonth", row.PaySlipForMonth);
                        command.Parameters.AddWithValue("@DaysPaid", Convert.ToDecimal(row.DaysPaid));
                        command.Parameters.AddWithValue("@AbsentDays", Convert.ToDecimal(row.AbsentDays));

                        if (Convert.ToDecimal(row.NetPay)!=null)
                        {
                            decimal earnedBasic = Convert.ToDecimal(row.NetPay) * 0.4m;
                            decimal hra = earnedBasic * 0.4m;
                            decimal specialAllowance = Convert.ToDecimal(row.NetPay) - earnedBasic - hra;

                            command.Parameters.AddWithValue("@EarnedBasic", earnedBasic);
                            command.Parameters.AddWithValue("@HRA", hra);
                            command.Parameters.AddWithValue("@SpecialAllowance", specialAllowance);
                            command.Parameters.AddWithValue("@EarningTotal", Convert.ToDecimal(row.NetPay));
                            decimal totalDeductions = (Convert.ToDecimal(row.ProfessionalTax))+(Convert.ToDecimal(row.PFEmployeeShare))+(Convert.ToDecimal(row.TDS));
                            command.Parameters.AddWithValue("@TotalDeductions", totalDeductions);
                            command.Parameters.AddWithValue("@NetPay", Convert.ToDecimal(row.NetPay));
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@EarnedBasic", 0);
                            command.Parameters.AddWithValue("@HRA", 0);
                            command.Parameters.AddWithValue("@SpecialAllowance", 0);
                            command.Parameters.AddWithValue("@EarningTotal", 0);
                            command.Parameters.AddWithValue("@TotalDeductions", 0);
                            command.Parameters.AddWithValue("@NetPay", 0);
                        }

                        command.Parameters.AddWithValue("@PFEmployeeShare", Convert.ToDecimal(row.PFEmployeeShare));
                        command.Parameters.AddWithValue("@ProfessionalTax", Convert.ToDecimal(row.ProfessionalTax));
                        command.Parameters.AddWithValue("@TDS", Convert.ToDecimal(row.TDS));
                        await command.ExecuteNonQueryAsync();
                        Console.WriteLine($"Inserted employee {row.Emp_Code} into the database.");
                    }
                }
                Console.WriteLine("Finished inserting data into the database.");
            }
        }
    }
}
