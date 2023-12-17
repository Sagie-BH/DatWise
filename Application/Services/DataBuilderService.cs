using Core.Models;
using Infrastructure.DAL;
using Infrastructure.Loggers;
using Infrastructure.MockData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IDataBuilderService
    {
        Task<ResponseResultObj> CreateSqlData();
    }

    public class DataBuilderService : IDataBuilderService
    {
        private readonly IAppLogger _logger;
        private readonly IMockStartup _mockStartup;
        private readonly ISqlDbConnFactory _sqlDbConnFactory;

        public DataBuilderService(IAppLogger logger, IMockStartup mockStartup, ISqlDbConnFactory sqlDbConnFactory)
        {
            _logger = logger;
            _mockStartup = mockStartup;
            _sqlDbConnFactory = sqlDbConnFactory;
        }
        public async Task<ResponseResultObj> CreateSqlData()
        {
            ResponseResultObj response = new()
            {
                StringBuilder = new StringBuilder()
            };

            var canConnect = await _sqlDbConnFactory.CheckConnectionAsync();
            if (canConnect)
            {
                response.StringBuilder.AppendLine("Successfully connected to the database.");
                var tablesCreated = await _mockStartup.CreateTables();
                if (tablesCreated)
                {
                    response.StringBuilder.AppendLine("Tables created successfully.");
                    var dataInserted = await _mockStartup.InsertMockData();
                    if (dataInserted)
                    {
                        response.StringBuilder.AppendLine("Data Inserted successfully.");
                    }
                    else
                    {
                        response.StringBuilder.AppendLine("Failed to insert data");
                        response.IsSuccess = false;
                    }
                }
                else
                {
                    response.StringBuilder.AppendLine("Failed to create tables");
                    response.IsSuccess = false;
                }
            }
            else
            {
                response.StringBuilder.AppendLine("Failed to connect to the database.");
                response.IsSuccess = false;
            }

            return response;
        }


    }
}
