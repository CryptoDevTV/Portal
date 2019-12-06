using Portal.Shared.Model.Entities;
using System.Collections.Generic;

namespace Portal.Academia.Web.ViewModels
{
    public class ModulesViewModel : BaseViewModel
    {
        public ModulesViewModel()
        {
            Modules = new HashSet<Module>();
        }
        public IEnumerable<Module> Modules { get; set; }
    }
}