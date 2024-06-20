using ExcelToDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelToDatabase.Services.Interface
{
    public interface IDatabaseService
    {
        Task InsertDataIntoDatabaseAsync(List<SalaryMetadata> data);
    }
}
