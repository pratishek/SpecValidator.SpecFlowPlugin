using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Gherkin.Ast;
using SpecValidator.SpecFlowPlugin.Config;
using TechTalk.SpecFlow.Generator.Interfaces;
using TechTalk.SpecFlow.Parser;

namespace SpecValidator.SpecFlowPlugin
{
    public class FuncValidatorFromConfig : IFuncValidator
    {
        private readonly SpecValidationConfiguration _specValidationConfiguration;

        public FuncValidatorFromConfig(ProjectSettings projectSettings)
        {
            _specValidationConfiguration = Utility.GetConfig(Path.Combine(projectSettings.ProjectFolder, "App.Config"));
            if (null == _specValidationConfiguration)
            {
                throw new ArgumentNullException($"SpecValidator Issue:There is no configuration defined for the specs");
            }
        }

        /// <summary>
        /// Validator for background description
        /// </summary>
        /// <param name="scenarioDefinition"></param>
        /// <param name="location"></param>
        /// <param name="testCaseWarningAndErrors"></param>
        public void BackgroundValidator(ScenarioDefinition scenarioDefinition, Location location, SpecsWarningAndErrors testCaseWarningAndErrors)
        {
            ScenarioDefinitionValidator(scenarioDefinition, location, testCaseWarningAndErrors, RuleApplicableTo.Background);
        }

        /// <summary>
        /// Validator for feature tags
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="location"></param>
        /// <param name="testCaseWarningAndErrors"></param>
        public void FeatureTagValidator(IEnumerable<string> tags, Location location, SpecsWarningAndErrors testCaseWarningAndErrors)
        {
            TagsValidator(tags, location, testCaseWarningAndErrors, RuleApplicableTo.FeatureTags);
        }

        /// <summary>
        /// Validator for scenario tags
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="location"></param>
        /// <param name="testCaseWarningAndErrors"></param>
        public void ScenarioTagValidator(IEnumerable<string> tags, Location location, SpecsWarningAndErrors testCaseWarningAndErrors)
        {
            TagsValidator(tags, location, testCaseWarningAndErrors, RuleApplicableTo.ScenarioTags);
        }

        public void FeatureValidator(SpecFlowFeature specFlowFeature, Location location,SpecsWarningAndErrors testCaseWarningAndErrors)
        {
           
        }

        /// <summary>
        /// Validator for scenario description
        /// </summary>
        /// <param name="scenarioDefinition"></param>
        /// <param name="location"></param>
        /// <param name="testCaseWarningAndErrors"></param>
        public void ScenarioValidator(ScenarioDefinition scenarioDefinition, Location location, SpecsWarningAndErrors testCaseWarningAndErrors)
        {
            ScenarioDefinitionValidator(scenarioDefinition, location, testCaseWarningAndErrors, RuleApplicableTo.Scenario);

        }

        /// <summary>
        /// Validator for scenario definition
        /// </summary>
        /// <param name="scenarioDefinition"></param>
        /// <param name="location"></param>
        /// <param name="testCaseWarningAndErrors"></param>
        /// <param name="ruleApplicabeTo"></param>
        private void ScenarioDefinitionValidator(ScenarioDefinition scenarioDefinition, Location location, SpecsWarningAndErrors testCaseWarningAndErrors, RuleApplicableTo ruleApplicabeTo)
        {
            var backgroundRules = _specValidationConfiguration.Rules.Cast<Rule>().Where(x => x.ApplyTo == ruleApplicabeTo);
            var enumerableRules = backgroundRules as IList<Rule> ?? backgroundRules.ToList();
            if (enumerableRules.Any())
            {
                var lstSteps = scenarioDefinition.Steps.ToList();
                StringBuilder steps = new StringBuilder();
                foreach (var step in lstSteps)
                {
                    steps.Append(step.Text);
                    steps.Append(Environment.NewLine);
                }
                foreach (var backgroundRule in enumerableRules)
                {
                    var matches = Regex.Match(steps.ToString(), backgroundRule.RegEx, RegexOptions.IgnoreCase);

                    if (!matches.Success)
                    {
                        MessageDetails messagedetails = new MessageDetails() { Message = backgroundRule.Message, Location = location };
                        if (backgroundRule.IsError)
                        {
                            testCaseWarningAndErrors.Errors.Add(messagedetails);
                        }
                        else
                        {
                            testCaseWarningAndErrors.Warnings.Add(messagedetails);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Validator for tags
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="location"></param>
        /// <param name="testCaseWarningAndErrors"></param>
        /// <param name="ruleApplicabeTo"></param>
        private void TagsValidator(IEnumerable<string> tags, Location location, SpecsWarningAndErrors testCaseWarningAndErrors, RuleApplicableTo ruleApplicabeTo)
        {
            var backgroundRules = _specValidationConfiguration.Rules.Cast<Rule>().Where(x => x.ApplyTo == ruleApplicabeTo);
            var enumerableRules = backgroundRules as IList<Rule> ?? backgroundRules.ToList();
            if (enumerableRules.Any())
            {

                var propertiesList = tags as IList<string> ?? tags.ToList();
                if (propertiesList.Any())
                {
                    foreach (var backgroundRule in enumerableRules)
                    {
                        var matches = propertiesList
                            .Select(p => Regex.Match(p, backgroundRule.RegEx, RegexOptions.IgnoreCase))
                            .Where(m => m.Success);
                        var matchList = matches as IList<Match> ?? matches.ToList();
                        if (!matchList.Any())
                        {
                            MessageDetails messagedetails = new MessageDetails() { Message = backgroundRule.Message, Location = location };
                            if (backgroundRule.IsError)
                            {
                                testCaseWarningAndErrors.Errors.Add(messagedetails);
                            }
                            else
                            {
                                testCaseWarningAndErrors.Warnings.Add(messagedetails);
                            }
                        }
                    }
                }
            }
        }

    }
}