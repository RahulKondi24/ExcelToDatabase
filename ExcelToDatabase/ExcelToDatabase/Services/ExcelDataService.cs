using ExcelToDatabase.Models;
using OfficeOpenXml;

namespace ExcelToDatabase.Service
{
    public class ExcelDataService
    {
        private readonly string _filePath;

        public ExcelDataService(string filePath)
        {
            _filePath = filePath;
        }
        public async Task<List<ExcelData>> ReadDataFromExcelAsync()
        {
            // Set the license context
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            Console.WriteLine($"Starting to read data from Excel file at: {_filePath}");

            List<ExcelData> data = new List<ExcelData>();
            if (!File.Exists(_filePath))
            {
                Console.WriteLine($"The specified Excel file does not exist at: {Path.GetFullPath(_filePath)}");
                return data;
            }

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
                    ExcelData rowData = new ExcelData
                    {
                        EmployeeCode = worksheet.Cells[row, 3].Value?.ToString(),
                        Leaves = worksheet.Cells[row, 6].Value?.ToString(),
                        Gross = worksheet.Cells[row, 7].Value?.ToString(),
                        WorkingDays = worksheet.Cells[row, 8].Value?.ToString(),
                        Salary = worksheet.Cells[row, 9].Value?.ToString(),
                        PT = worksheet.Cells[row, 10].Value?.ToString(),
                        IT_TDS = worksheet.Cells[row, 11].Value?.ToString(),
                        PF = worksheet.Cells[row, 12].Value?.ToString(),
                        Advance = worksheet.Cells[row, 13].Value?.ToString(),
                        CompanyPF = worksheet.Cells[row, 14].Value?.ToString(),
                        NetAmount = worksheet.Cells[row, 15].Value?.ToString(),
                    };
                    Console.WriteLine($"Reading row {row - 2}");
                    data.Add(rowData);
                    Console.WriteLine($"Added employee {rowData.EmployeeCode} to the list.");
                }
            }
            Console.WriteLine("Finished reading data from Excel.");
            return data;
        }
    }
}