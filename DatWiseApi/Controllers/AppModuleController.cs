using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatWiseApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppModuleController : ControllerBase
    {
        private readonly IAppModuleService _appModuleService;

        public AppModuleController(IAppModuleService appModuleService)
        {
            _appModuleService = appModuleService;
        }

        [HttpGet("modules")]
        public async Task<IActionResult> GetModulesWithComponents(int userId)
        {
            var modules = await _appModuleService.GetModulesWithComponentsAsync(userId);
            return Ok(modules);
        }
        [HttpPost("updateusermodules")]
        public async Task<IActionResult> UpdateUserModules([FromBody] UserModuleUpdateModel model)
        {
            var modules = await _appModuleService.UpdateUserModules(model.UserId, model.SelectedModuleIds);
            return Ok(modules);
        }
    }
    public class UserModuleUpdateModel
    {
        public int UserId { get; set; }
        public List<int> SelectedModuleIds { get; set; }
    }
}
