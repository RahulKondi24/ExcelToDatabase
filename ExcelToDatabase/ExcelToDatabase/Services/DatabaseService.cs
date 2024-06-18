using System.Data.SqlClient;
using ExcelToDatabase.Models;

namespace ExcelToDatabase.Service
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task InsertDataIntoDatabaseAsync(List<ExcelData> data)
        {
            Console.WriteLine("Starting to insert data into the database...");
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                Console.WriteLine("Database connection opened.");

                foreach (var row in data)
                {
                    string insertQuery = "INSERT INTO SalaryDetails (EmployeeCode, Leaves, Gross, WorkingDays, Salary, EarnedBasic, HRA, SpecialAllowance, PT, IT_TDS, PF, Advance, CompanyPF, NetAmount) " +
                                        "VALUES (@EmployeeCode, @Leaves, @Gross, @WorkingDays, @Salary, @EarnedBasic, @HRA, @SpecialAllowance, @PT, @IT_TDS, @PF, @Advance, @CompanyPF, @NetAmount)";

                    await using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeCode", row.EmployeeCode);
                        command.Parameters.AddWithValue("@Leaves", row.Leaves);
                        command.Parameters.AddWithValue("@Gross", row.Gross);
                        command.Parameters.AddWithValue("@WorkingDays", row.WorkingDays);
                        command.Parameters.AddWithValue("@Salary", row.Salary);
                        command.Parameters.AddWithValue("@EarnedBasic", row.EarnedBasic);
                        command.Parameters.AddWithValue("@HRA", row.HRA);
                        command.Parameters.AddWithValue("@SpecialAllowance", row.SpecialAllowance);
                        command.Parameters.AddWithValue("@PT", row.PT);
                        command.Parameters.AddWithValue("@IT_TDS", row.IT_TDS);
                        command.Parameters.AddWithValue("@PF", row.PF);
                        command.Parameters.AddWithValue("@Advance", row.Advance == null ? 0 : row.Advance);
                        command.Parameters.AddWithValue("@CompanyPF", row.CompanyPF);
                        command.Parameters.AddWithValue("@NetAmount", row.NetAmount);
                        await command.ExecuteNonQueryAsync();
                        Console.WriteLine($"Inserted employee {row.EmployeeCode} into the database.");
                    }
                    Console.WriteLine("Finished inserting data into the database.");
                }
            }
        }
    }
}
