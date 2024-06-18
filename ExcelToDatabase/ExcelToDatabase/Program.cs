Console.WriteLine("Starting application...");

// Database connection string
string connectionString = "Server=LAPTOP-46NPMGS0\\SQLEXPRESS;Database=ATSDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";

string excelFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExcelFile", "Salary Sheet Sample.xlsx");
