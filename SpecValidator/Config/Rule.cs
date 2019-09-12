using System;
using System.Configuration;

namespace SpecValidator.SpecFlowPlugin.Config
{
    public class Rule : ConfigurationElement
    {
        // Create a "Name" attribute.
        [ConfigurationProperty(Constants.Name, IsRequired = true)]
        public String Name
        {
            get => this[Constants.Name] as string;
            set => this[Constants.Name] = value;
        }

        // Create a "ApplyTo" attribute.
        [ConfigurationProperty(Constants.ApplyTo, IsRequired = true)]
        public RuleApplicableTo ApplyTo
        {
            get => (RuleApplicableTo)Enum.Parse(typeof(RuleApplicableTo), this[Constants.ApplyTo].ToString(), true);
            set => this[Constants.ApplyTo] = value;
        }

        // Create a "IsError" attribute.
        [ConfigurationProperty(Constants.IsError, DefaultValue = true)]
        public Boolean IsError
        {
            get => (bool)this[Constants.IsError];
            set => this[Constants.IsError] = value;
        }

        // Create a "Message" attribute.
        [ConfigurationProperty(Constants.Message, IsRequired = true)]
        public String Message
        {
            get => (string)this[Constants.Message];
            set => this[Constants.Message] = value;
        }

        // Create a "RegEx" attribute.
        [ConfigurationProperty(Constants.RegEx, IsRequired = true)]
        public string RegEx
        {
            get => (string)this[Constants.RegEx];
            set => this[Constants.RegEx] = value;
        }

    }
}