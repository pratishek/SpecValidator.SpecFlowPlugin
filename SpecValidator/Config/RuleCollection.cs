using System.Configuration;

namespace SpecValidator.SpecFlowPlugin.Config
{
    public class RuleCollection : ConfigurationElementCollection
    {
        public Rule this[int index]
        {
            get { return (Rule)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new Rule();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Rule)element).Name;
        }
    }
}