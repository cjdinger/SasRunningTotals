using System;
using System.Text;
using System.Xml;
using SAS.Tasks.Toolkit.Controls;
using System.Xml.Linq;
using System.Collections.Generic;
using SAS.Shared.AddIns;
using SAS.Tasks.Toolkit.Helpers;

namespace RunningTotals
{
    /// <summary>
    /// Use this class to keep track of the 
    /// options that are set within your task.
    /// You must save and restore these settings when the user
    /// interacts with your task user interface,
    /// and when the task runs within the process flow.
    /// </summary>
    public class RunningTotalsTaskSettings
    {
  
        // initialize settings to defaults
        public RunningTotalsTaskSettings()
        {
            VariableGroups = new List<string>();
            FilterSettings = new FilterSettings();
            DataOut = "WORK.OUT_TOTALS";
            VariableMeasure = "";
            VariableTotal = "";
        }

        #region Properties, or task settings

        /// <summary>
        /// Store the filter settings, if any, for WHERE= subsetting
        /// options
        /// </summary>
        public FilterSettings FilterSettings { get; set; }

        /// <summary>
        /// Name of the variable measure to sum
        /// </summary>
        public string VariableMeasure { get; set; }

        /// <summary>
        /// Name of any grouping variables,
        /// to segment running totals
        /// </summary>
        public List<string> VariableGroups { get; set; }

        /// <summary>
        /// Name of the output data set
        /// </summary>
        public string DataOut { get; set; }

        /// <summary>
        /// Name of the new variable to hold totals
        /// </summary>
        public string VariableTotal { get; set; }

        #endregion

        #region Code to save/restore task settings
        int version = 1;
        public string ToXml()
        {
            // using LINQ to save to Xml
            // create an "inner" element to hold
            // the list of Grouping variables
            // could be 0 or more
            XElement groups = new XElement("GroupVariables");
            foreach (string var in VariableGroups)
            {
                groups.Add(new XElement("GroupVariable",var));
            }

            XDocument doc = new XDocument(
                new XDeclaration("1.0", "utf-8", string.Empty),
                new XElement("RunningTotalsTask",
                    new XElement("Version", version),
                    new XElement("FilterSettings", FilterSettings.ToXml()),
                    new XElement("DataOut", DataOut),
                    new XElement("MeasureVar", VariableMeasure),
                    new XElement("TotalsVar", VariableTotal),
                    groups
                        )
                );

            return doc.ToString();
        }

        public void FromXml(string xml)
        {
            try
            {
                // and use LINQ to parse it back out
                XDocument doc = XDocument.Parse(xml);

                XElement filter = doc
                    .Element("RunningTotalsTask")
                    .Element("FilterSettings");
                FilterSettings = new FilterSettings(filter.Value);

                XElement outdata = doc
                    .Element("RunningTotalsTask")
                    .Element("DataOut");
                DataOut = outdata.Value;

                XElement measure = doc
                    .Element("RunningTotalsTask")
                    .Element("MeasureVar");

                VariableMeasure = measure.Value;

                XElement totals = doc
                    .Element("RunningTotalsTask")
                    .Element("TotalsVar");

                VariableTotal = totals.Value;

                XElement groups = doc
                    .Element("RunningTotalsTask")
                    .Element("GroupVariables");

                var g = groups.Elements("GroupVariable");
                foreach (XElement e in g)
                {
                    VariableGroups.Add(e.Value);
                }

            }
            catch (XmlException)
            {
                // couldn't read the XML content
            }
        }
        #endregion

        #region Routine to build a SAS program

        public string GetSasProgram(ISASTaskConsumer3 consumer)
        {
            StringBuilder program = new StringBuilder();

            string filter = "";
            if (!string.IsNullOrEmpty(FilterSettings.FilterValue))
                filter = string.Format("(where=({0}))", FilterSettings.FilterValue);

            // this handy function creates an
            // easy-to-read comment block
            // in the generated SAS program
            program.Append(
                SAS.Tasks.Toolkit.Helpers.UtilityFunctions.BuildSasTaskCodeHeader(
                "Calculate Running Totals",
                string.Format("{0}.{1}",consumer.ActiveData.Library, 
                    consumer.ActiveData.Member),
                consumer.AssignedServer));

            // if no groups, then easy 1-line calculation
            if (VariableGroups.Count == 0)
            {
                program.AppendFormat("data {0};\n", DataOut);
                program.AppendFormat("  set {0}.{1}{2};\n",
                       consumer.ActiveData.Library, consumer.ActiveData.Member,filter);
                program.AppendFormat("  {0} + {1}; \n",
                    UtilityFunctions.SASValidLiteral(VariableTotal), 
                    UtilityFunctions.SASValidLiteral(VariableMeasure));
                program.AppendLine("run;");
            }
            // if there are groups, then create a program
            // with a BY statement and IF FIRST construct
            else
            {
                program.AppendFormat("data {0};\n", DataOut);
                program.AppendFormat("  set {0}.{1}{2};\n",
                       consumer.ActiveData.Library, consumer.ActiveData.Member, filter);

                // add BY variables 
                // and build IF FIRST logic
                program.AppendLine("  by ");
                StringBuilder first = new StringBuilder();
                for (int i = 0; i < VariableGroups.Count; i++)
                {
                    program.AppendLine(string.Format("   {0}", 
                        SAS.Tasks.Toolkit.Helpers.UtilityFunctions.SASValidLiteral(VariableGroups[i])));

                    // build 1 or more FIRST checks
                    if (i == 0) 
                        first.AppendFormat("FIRST.{0} ", 
                            SAS.Tasks.Toolkit.Helpers.UtilityFunctions.SASValidLiteral(VariableGroups[i]));
                    else first.AppendFormat("or FIRST.{0} ", 
                        SAS.Tasks.Toolkit.Helpers.UtilityFunctions.SASValidLiteral(VariableGroups[i]));
                }
                program.AppendLine("  ;");

                // add FIRST logic line
                program.AppendFormat("  if {0} then \n   {1}={2};\n", first, 
                    SAS.Tasks.Toolkit.Helpers.UtilityFunctions.SASValidLiteral(VariableTotal), 
                    SAS.Tasks.Toolkit.Helpers.UtilityFunctions.SASValidLiteral(VariableMeasure));
                program.AppendFormat("  else {0} + {1}; \n",
                    SAS.Tasks.Toolkit.Helpers.UtilityFunctions.SASValidLiteral(VariableTotal), 
                    SAS.Tasks.Toolkit.Helpers.UtilityFunctions.SASValidLiteral(VariableMeasure));
                program.AppendLine("run;");
            }

            // return the built program
            return program.ToString();
        }
        #endregion
    }
}
