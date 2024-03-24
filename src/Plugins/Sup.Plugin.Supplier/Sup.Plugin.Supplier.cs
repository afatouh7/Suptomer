using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.ScheduleTasks;
using Nop.Core.Domain.Security;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.ScheduleTasks;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Menu;

namespace Sup.Plugin.Supplier
{
    public class Sup_plugin_Supplier : BasePlugin, IAdminMenuPlugin
    {

        private readonly IPermissionService _permissionService;
        private readonly IWebHelper _webHelper;
        private readonly ILocalizationService _localizationService;



      

        public Sup_plugin_Supplier(IPermissionService permissionService,
            IWebHelper webHelper, ILocalizationService localizationService)
        {
            _permissionService = permissionService;
            _webHelper = webHelper;
            _localizationService = localizationService;
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}SuplierContract/List";
        }

        public virtual async Task InstallAsync()
        {
            await base.InstallAsync();
        }

        public virtual Task UninstallAsync()
        {
            return Task.CompletedTask;
        }
        public async Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return;

            var config = rootNode.ChildNodes.FirstOrDefault(node => node.SystemName.Equals("Configuration"));
            if (config == null)
                return;

            var plugins = config.ChildNodes.FirstOrDefault(node => node.SystemName.Equals("Local plugins"));

            if (plugins == null)
                return;

            var index = config.ChildNodes.IndexOf(plugins);

            if (index < 0)
                return;

            config.ChildNodes.Insert(index, new SiteMapNode
            {
                SystemName = "nopCommerce Suplier plugin",
                Title = "Suplier",
                ControllerName = "SuplierContract",
                ActionName = "List",
                IconClass = "far fa-dot-circle",
                Visible = true,
                RouteValues = new RouteValueDictionary { { "area", AreaNames.Admin } }
            });
        }
    }
    
}
