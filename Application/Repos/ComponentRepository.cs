using Core.Models;
using Infrastructure.DAL;
using Infrastructure.Loggers;

namespace Application.Repos
{
    public class AppComponentRepository(IDataAccess dataAccess, IAppLogger _logger) : BaseRepository<AppComponent>(dataAccess, _logger)
    {

        // Here you can add methods specific to AppComponent
        // For example, methods to retrieve components based on certain criteria.
    }
}
