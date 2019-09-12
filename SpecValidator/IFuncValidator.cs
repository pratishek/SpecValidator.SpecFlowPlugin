using System.Collections.Generic;
using Gherkin.Ast;
using TechTalk.SpecFlow.Parser;

namespace SpecValidator.SpecFlowPlugin
{
    public interface IFuncValidator
    {
        void FeatureValidator(SpecFlowFeature specFlowFeature, Location location, SpecsWarningAndErrors testCaseWarningAndErrors);
        void ScenarioValidator(ScenarioDefinition scenarioDefinition, Location location, SpecsWarningAndErrors testCaseWarningAndErrors);
        void BackgroundValidator(ScenarioDefinition scenarioDefinition, Location location, SpecsWarningAndErrors testCaseWarningAndErrors);
        void ScenarioTagValidator(IEnumerable<string> tags, Location location, SpecsWarningAndErrors testCaseWarningAndErrors);
        void FeatureTagValidator(IEnumerable<string> tags, Location location, SpecsWarningAndErrors testCaseWarningAndErrors);
    }
}
