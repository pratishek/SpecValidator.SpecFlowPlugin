using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace SpecValidator.SpecFlowPlugin.Config
{
    public class Utility
    {

        public static SpecValidationConfiguration GetConfig()
        {
            return System.Configuration.ConfigurationManager.GetSection(nameof(SpecValidationConfiguration)) as SpecValidationConfiguration;
        }

        public static SpecValidationConfiguration GetConfig(string path)
        {
            try
            {
                var configFileContent = File.ReadAllText(path);
                var configDocument = new XmlDocument();
                configDocument.LoadXml(configFileContent);
                var xmlNode = configDocument.SelectSingleNode("/configuration/SpecValidationConfiguration");
                return SpecValidationConfiguration.CreateFromXml(xmlNode?.OuterXml.Trim());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "Config load error");
                return new SpecValidationConfiguration();
            }
        }
    }
}
