using TechTalk.SpecFlow.Generator.Plugins;
using TechTalk.SpecFlow.Generator.UnitTestProvider;
using TechTalk.SpecFlow.UnitTestProvider;

namespace SpecValidator.SpecFlowPlugin
{
    public class SpecValidatorPlugin : IGeneratorPlugin
    {
        public void Initialize(GeneratorPluginEvents generatorPluginEvents, GeneratorPluginParameters generatorPluginParameters)
        {
            generatorPluginEvents.CustomizeDependencies += GeneratorPluginEvents_CustomizeDependencies;
        }

        private void GeneratorPluginEvents_CustomizeDependencies(object sender, CustomizeDependenciesEventArgs e)
        {
            e.ObjectContainer.RegisterTypeAs<FuncValidatorFromConfig, IFuncValidator>();
            e.ObjectContainer.RegisterTypeAs<CustomNUnit3TestGeneratorProvider, IUnitTestGeneratorProvider>();
            e.ObjectContainer.RegisterTypeAs<NUnit3RuntimeProvider, IUnitTestRuntimeProvider>();

        }
    }
}