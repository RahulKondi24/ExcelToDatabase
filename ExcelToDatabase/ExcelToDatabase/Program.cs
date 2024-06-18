using ExcelToDatabase.Service;

Console.WriteLine("Starting application...");

// Database connection string
string connectionString = "Server=LAPTOP-46NPMGS0\\SQLEXPRESS;Database=ATSDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";

// Construct the relative path to the Excel file
string excelFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExcelFile", "Salary Sheet Sample.xlsx");
Console.WriteLine($"Excel file path: {excelFilePath}");

// Ensure the file path is correct
if (!File.Exists(excelFilePath))
{
    Console.WriteLine($"The specified Excel file does not exist at: {Path.GetFullPath(excelFilePath)}");
    return;
}

// Read data from Excel file
var excelDataService = new ExcelDataService(excelFilePath);
var data = await excelDataService.ReadDataFromExcelAsync();

// Insert data into the database
var databaseService = new DatabaseService(connectionString);
await databaseService.InsertDataIntoDatabaseAsync(data);

Console.WriteLine("Data has been inserted into the database.");
Console.WriteLine("Application finished.");
Console.ReadLine();