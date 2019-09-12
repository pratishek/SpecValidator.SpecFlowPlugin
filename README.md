# SpecValidator.SpecFlowPlugin

This plugin provides a mechanism to validate Spec flow Background,Scenario,Feature tags, Scenario tags as per the rule defined in Configuration.


Sample Configuration:
```html
  <configSections>
    <section name="specFlow" type="TechTalk.SpecFlow.Configuration.ConfigurationSectionHandler, TechTalk.SpecFlow" />
    <section name="SpecValidationConfiguration" type="SpecValidator.SpecFlowPlugin.Config.SpecValidationConfiguration, SpecValidator.SpecFlowPlugin" />
  </configSections>


  <SpecValidationConfiguration>
    <Rules>
      <Rule Name="ScenarioNumberValidator1" RegEx="^Scenario_(\\d+\\.){2,}\\d+\\.$" ApplyTo="ScenarioTags" Message="Has no Scenario Number. Format must be @Scenario_1.2.3. Minimum of 3 points, maximum unbounded."/>
    </Rules>
  </SpecValidationConfiguration>
```
