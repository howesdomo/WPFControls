using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Client.Common
{
    class ConfigHandler
    {
        /// <summary>
        /// 根据配置文件名称，获取配置Configuration
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        private static Configuration GetConfiguration()
        {
            return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        private static Configuration GetConfiguration(string configName)
        {
            string m_curPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string m_ConfigFullName = System.IO.Path.Combine(m_curPath, configName.Trim() + ".config");
            ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
            configFile.ExeConfigFilename = m_ConfigFullName;

            return ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);
        }

        /// <summary>
        /// 根据配置文件名称，获取配置信息
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        private static KeyValueConfigurationCollection GetSettings(string configName)
        {
            string m_curPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string m_ConfigFullName = System.IO.Path.Combine(m_curPath, configName.Trim() + ".config");
            ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
            configFile.ExeConfigFilename = m_ConfigFullName;

            return ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None).AppSettings.Settings;
        }

        #region ServiceSetting - 服务连接配置

        public static KeyValueConfigurationCollection ServiceSettings
        {
            get;
            private set;
        }

        public const string ServiceSettingsFileName = "ServiceSettings";

        public const string WebServer = "WebServer";

        /// <summary>
        /// Set IP Port
        /// </summary>
        /// <param name="uri"></param>
        public static void SaveServiceSetting_WebServer(Uri uri)
        {
            Configuration cfa = ConfigHandler.GetConfiguration(ConfigHandler.ServiceSettingsFileName);
            
            if (cfa.AppSettings.Settings[ConfigHandler.WebServer] != null)
            {
                cfa.AppSettings.Settings[ConfigHandler.WebServer].Value = uri.ToString();
            }
            else
            {
                cfa.AppSettings.Settings.Add(key: ConfigHandler.WebServer, value: uri.ToString());                
            }
            
            cfa.Save(ConfigurationSaveMode.Full);
        }

        /// <summary>
        /// 获取SecurityService配置信息
        /// </summary>
        /// <returns></returns>
        public static Uri GetServiceSetting_WebServer()
        {
            try
            {
                string uriStr = ConfigHandler.GetServiceSetting(ConfigHandler.WebServer);
                return new Uri(uriStr);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据key从Service配置里获取Value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetServiceSetting(string key)
        {
            string value = string.Empty;
            ConfigHandler.ServiceSettings = ConfigHandler.GetSettings(ConfigHandler.ServiceSettingsFileName);
            if (ConfigHandler.ServiceSettings[key] != null)
            {
                value = ConfigHandler.ServiceSettings[key].Value;
            }
            return value;
        }

        #endregion

        #region NativeSettings - PC单机配置

        public static Configuration NativeSettings
        {
            get;
            private set;
        }

        public static string NativeSettingsFileName
        {
            get { return "NativeSettings"; }
        }

        public static string GetValueFromNativeSettings(string key)
        {
            ConfigHandler.NativeSettings = ConfigHandler.GetConfiguration(ConfigHandler.NativeSettingsFileName);
            return ConfigHandler.NativeSettings.AppSettings.Settings[key].Value;
        }

        public static void SetValueToNativeSettings(string key, string value)
        {
            ConfigHandler.NativeSettings = ConfigHandler.GetConfiguration(ConfigHandler.NativeSettingsFileName);

            if (ConfigHandler.NativeSettings.AppSettings.Settings[key] == null)
            {
                // 若一直用此方法, Value 的值会用逗号不断增加, 故需要用 if 区分好情况
                ConfigHandler.NativeSettings.AppSettings.Settings.Add(key, value);
            }
            else
            {
                ConfigHandler.NativeSettings.AppSettings.Settings[key].Value = value;
            }

            ConfigHandler.NativeSettings.Save();
        }

        public static int GetDebugMode()
        {
            try
            {
                string value = ConfigHandler.GetValueFromNativeSettings("DebugMode");
                int.TryParse(value, out int r);
                return r;
            }
            finally { }
        }

        #endregion
    }
}
