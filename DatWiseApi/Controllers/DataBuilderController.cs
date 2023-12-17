using Application.Controllers;
using Application.Services;
using Infrastructure.DAL;
using Infrastructure.Loggers;
using Infrastructure.MockData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;

namespace DatWiseApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataBuilderController : BaseController 
    {
        private readonly IDataBuilderService _dataBuilderService;

        public DataBuilderController(IAppLogger logger, IDataBuilderService dataBuilderService) : base(logger)
        {
            _dataBuilderService = dataBuilderService;
        }

        [HttpGet(Name = "CreateSqlData")]
        public async Task<IActionResult> Get()
        {
            _logger.LogMessage("Stating to create sql data", Core.LogLevel.INFO);
            var responseResult = await _dataBuilderService.CreateSqlData();

            _logger.LogMessage(responseResult.Message, Core.LogLevel.INFO);
            return ResponseResult(responseResult);
        }
    }

}
