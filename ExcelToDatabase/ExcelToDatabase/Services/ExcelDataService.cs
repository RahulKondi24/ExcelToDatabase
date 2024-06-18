using ExcelToDatabase.Models;
using OfficeOpenXml;
using System.Data;

namespace ExcelToDatabase.Service
{
    public class ExcelDataService
    {
        private readonly string _filePath;

        public ExcelDataService(string filePath)
        {
            _filePath = filePath;
        }
        public async Task<List<SalaryMetadata>> ReadDataFromExcelAsync()
        {
            // Set the license context
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            Console.WriteLine($"Starting to read data from Excel file at: {_filePath}");

            List<SalaryMetadata> data = new List<SalaryMetadata>();
            if (!File.Exists(_filePath))
            {
                Console.WriteLine($"The specified Excel file does not exist at: {Path.GetFullPath(_filePath)}");
                return data;
            }
            return await Task.Run(() =>
            {
                using (var package = new ExcelPackage(new FileInfo(_filePath)))
                {
                    if (package.Workbook.Worksheets.Count == 0)
                    {
                        Console.WriteLine("The Excel file does not contain any worksheets.");
                        return data;
                    }

                    var worksheet = package.Workbook.Worksheets[0]; // Assuming the data is in the first worksheet
                    if (worksheet == null)
                    {
                        Console.WriteLine("The specified worksheet does not exist.");
                        return data;
                    }

                    var rowCount = worksheet.Dimension?.Rows ?? 0;
                    if (rowCount == 0)
                    {
                        Console.WriteLine("The worksheet is empty.");
                        return data;
                    }
                    Console.WriteLine($"Total rows found in Excel: {rowCount - 2}");

                    for (int row = 3; row <= rowCount; row++) // Start from row 3, assuming row 1 is the header
                    {
                        SalaryMetadata rowData = new SalaryMetadata
                        {
                            Emp_Code = worksheet.Cells[row, 3].Value?.ToString(),
                            PaySlipForMonth = DateTime.Now.ToString("yyyy-MM"),
                            DaysPaid = decimal.TryParse(worksheet.Cells[row, 8].Value?.ToString(), out var daysPaid) ? daysPaid : 0,
                            AbsentDays = decimal.TryParse(worksheet.Cells[row, 6].Value?.ToString(), out var absentDays) ? absentDays : 0,
                            EarnedBasic =0,
                            HRA = 0,
                            SpecialAllowance = 0,
                            PFEmployeeShare = decimal.TryParse(worksheet.Cells[row, 12].Value?.ToString(), out var pfEmployeeShare) ? pfEmployeeShare : 0,
                            ProfessionalTax = decimal.TryParse(worksheet.Cells[row, 10].Value?.ToString(), out var professionalTax) ? professionalTax : 0,
                            TDS = decimal.TryParse(worksheet.Cells[row, 11].Value?.ToString(), out var tds) ? tds : 0,
                            EarningTotal = decimal.TryParse(worksheet.Cells[row, 15].Value?.ToString(), out var earningTotal) ? earningTotal : 0,
                            TotalDeductions = 0,
                            NetPay = decimal.TryParse(worksheet.Cells[row, 15].Value?.ToString(), out var netPay) ? netPay : 0,
                        };
                        Console.WriteLine($"Reading row {row - 2}");
                        data.Add(rowData);
                        Console.WriteLine($"Added employee {rowData.Emp_Code} to the list.");
                    }
                }
                Console.WriteLine("Finished reading data from Excel.");
                return data;
            });
        }
    }
}