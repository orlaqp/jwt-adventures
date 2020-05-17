using System.Collections.Generic;
using System.Linq;
using api.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace api.Controllers
{
    [Route("[controller]")]
    public class ConfigurationController : ControllerBase
    {
        IConfiguration config;
        IConfigurationRoot configRoot;

        public ConfigurationController(IConfiguration config)
        {
            this.config = config;
            this.configRoot = (IConfigurationRoot)config;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return configRoot.Providers.Select(p => p.ToString());
        }

        [HttpGet("keys")]
        public IDictionary<string, string> DumpConfig() {
            var logging = new Logging();
            config.GetSection("Logging").Bind(logging);

            var jwt = new JwtOptions();
            config.GetSection("Jwt").Bind(jwt);

            var configKeys = new Dictionary<string, string>();

            configKeys["logging"] = config["Logging"];
            configKeys["loggingLevel"] = logging.Level;
            configKeys["jwt"] = JsonConvert.SerializeObject(jwt);

            return configKeys;
        }

    }
}