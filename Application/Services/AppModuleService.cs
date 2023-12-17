using Application.Repos;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IAppModuleService
    {
        Task<IEnumerable<AppModule>> GetModulesWithComponentsAsync(int userId);
        Task<bool> UpdateUserModules(int userId, List<int> selectedModuleIds);
    }

    public class AppModuleService : IAppModuleService
    {
        private readonly IAppModuleRepository _appModuleRepository;

        public AppModuleService(IAppModuleRepository appModuleRepository)
        {
            _appModuleRepository = appModuleRepository;
        }

        public async Task<IEnumerable<AppModule>> GetModulesWithComponentsAsync(int userId)
        {
            return await _appModuleRepository.GetModulesByUserId(userId);
        }
        public async Task<bool> UpdateUserModules(int userId, List<int> selectedModuleIds)
        {
            return await _appModuleRepository.UpdateUserModules(userId, selectedModuleIds); 
        }
    }
}
