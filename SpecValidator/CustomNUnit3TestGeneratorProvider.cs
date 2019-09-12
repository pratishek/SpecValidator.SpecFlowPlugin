using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Gherkin.Ast;
using TechTalk.SpecFlow.Generator;
using TechTalk.SpecFlow.Generator.UnitTestProvider;
using TechTalk.SpecFlow.Parser;
using TechTalk.SpecFlow.Tracing;
using TechTalk.SpecFlow.Utils;

namespace SpecValidator.SpecFlowPlugin
{
    public class CustomNUnit3TestGeneratorProvider : NUnit3TestGeneratorProvider, IUnitTestGeneratorProvider
    {
        private const string TestNameFormat = "{0}";
        private readonly IFuncValidator _funcValidator;
        private readonly Dictionary<string, SpecsWarningAndErrors> _dictTestCaseWarningAndErrors = new Dictionary<string, SpecsWarningAndErrors>();

        public CustomNUnit3TestGeneratorProvider(CodeDomHelper codeDomHelper, IFuncValidator funcValidator) : base(codeDomHelper)
        {
            _funcValidator = funcValidator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generationContext"></param>
        public override void SetTestClassInitializeMethod(TestClassGenerationContext generationContext)
        {
            var feature = generationContext.Feature;
            if (null != feature)
            {
                SpecsWarningAndErrors testCaseWarningAndErrorsFeature = new SpecsWarningAndErrors();
                _funcValidator.FeatureValidator(feature, feature.Location, testCaseWarningAndErrorsFeature);
                AddWarningAndErrorStatement(generationContext.FeatureBackgroundMethod, testCaseWarningAndErrorsFeature);

                if (feature.Tags!=null && feature.HasTags())
                {
                    SpecsWarningAndErrors testCaseWarningAndErrorsFeaturetags = new SpecsWarningAndErrors();
                    var properties = feature.Tags.Select(x => x.Name.TrimStart("@".ToCharArray())).ToArray();
                    _funcValidator.FeatureTagValidator(properties, feature.Location, testCaseWarningAndErrorsFeaturetags);
                    AddWarningAndErrorStatement(generationContext.FeatureBackgroundMethod, testCaseWarningAndErrorsFeaturetags);
                }

                if (null != feature.Background)
                {
                    SpecsWarningAndErrors testCaseWarningAndErrorsBackground = new SpecsWarningAndErrors();
                    _funcValidator.BackgroundValidator(feature.Background, feature.Background.Location, testCaseWarningAndErrorsBackground);
                    AddWarningAndErrorStatement(generationContext.FeatureBackgroundMethod, testCaseWarningAndErrorsBackground);
                }

                foreach (var featureScenarioDefinition in feature.ScenarioDefinitions)
                {
                    var scenario = featureScenarioDefinition;
                    SpecsWarningAndErrors testCaseWarningAndErrorsScenario = new SpecsWarningAndErrors();
                    _funcValidator.ScenarioValidator(featureScenarioDefinition, featureScenarioDefinition.Location, testCaseWarningAndErrorsScenario);
                    if (((IHasTags)scenario).Tags != null && scenario.HasTags())
                    {
                        var properties = scenario.GetTags().Select(x => x.Name.TrimStart("@".ToCharArray())).ToArray();
                        _funcValidator.ScenarioTagValidator(properties, scenario.Location, testCaseWarningAndErrorsScenario);
                    }
                    if (testCaseWarningAndErrorsScenario.HavingErrors || testCaseWarningAndErrorsScenario.HavingWarnings)
                    {
                        _dictTestCaseWarningAndErrors.Add(string.Format(TestNameFormat, scenario.Name.ToIdentifier()), testCaseWarningAndErrorsScenario);
                    }
                }
            }
            base.SetTestClassInitializeMethod(generationContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generationContext"></param>
        /// <param name="testMethod"></param>
        /// <param name="friendlyTestName"></param>
        public new void SetTestMethod(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, string friendlyTestName)
        {

            if (_dictTestCaseWarningAndErrors.ContainsKey(testMethod.Name))
            {
                SpecsWarningAndErrors testCaseWarningAndErrors = _dictTestCaseWarningAndErrors[testMethod.Name];
                AddWarningAndErrorStatement(testMethod, testCaseWarningAndErrors);
            }
            base.SetTestMethod(generationContext, testMethod, friendlyTestName);
        }

        private void AddWarningAndErrorStatement(CodeMemberMethod testMethod, SpecsWarningAndErrors testCaseWarningAndErrors)
        {
            testCaseWarningAndErrors.Errors.ForEach(x => testMethod.Statements.Add(new CodeSnippetStatement(CodeDomHelper.GetErrorStatementString($" ({x.Location.Line}:{x.Location.Column}): {x.Message}"))));
            if (testCaseWarningAndErrors.HavingWarnings)
            {
                testMethod.Statements.Add(CodeDomHelper.GetEnableWarningsPragma());
                testCaseWarningAndErrors.Warnings.ForEach(x => testMethod.Statements.Add(new CodeSnippetStatement($"#warning ({x.Location.Line}:{x.Location.Column}): {x.Message} ")));
                testMethod.Statements.Add(CodeDomHelper.GetDisableWarningsPragma());
            }
        }
    }
}
