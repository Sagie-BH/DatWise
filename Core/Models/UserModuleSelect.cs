using System.Reflection;

namespace Core.Models
{
    public class UserModuleSelect
    {
        public int UserID { get; set; }
        public List<AppModule> SelectedModules { get; set; }
    }


}
