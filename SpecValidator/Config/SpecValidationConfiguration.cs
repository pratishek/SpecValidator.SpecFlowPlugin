using System.Configuration;
using System.IO;
using System.Xml;

namespace SpecValidator.SpecFlowPlugin.Config
{
    public class SpecValidationConfiguration : ConfigurationSection
    {
        [ConfigurationProperty(Constants.Rules, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(RuleCollection), AddItemName = Constants.Rule)]
        public RuleCollection Rules
        {
            get
            {
                return (RuleCollection)base[Constants.Rules];
            }
        }

        public static SpecValidationConfiguration CreateFromXml(string xmlContent)
        {
            SpecValidationConfiguration section = new SpecValidationConfiguration();
            section.Init();
            // ReSharper disable once AssignNullToNotNullAttribute
            section.Reset(null);
            using (var reader = new XmlTextReader(new StringReader(xmlContent.Trim())))
            {
                section.DeserializeSection(reader);
            }
            section.ResetModified();
            return section;
        }
    }
}