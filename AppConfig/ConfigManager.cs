using Microsoft.Extensions.Configuration;
using System;

namespace AppConfig
{
    public class ConfigManager : IConfigManager
    {
        private readonly IConfiguration _configuration;
        public ConfigManager(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string GetConnectionString(string connectionName)
        {
            return this._configuration.GetConnectionString(connectionName);
        }

        public string AppKey(string Key)
        {
            return this._configuration["AppSeettings:"+ Key + ""];
            
        }

        public IConfigurationSection GetConfigurationSection(string key)
        {
            return this._configuration.GetSection(key);
        }
    }
}
