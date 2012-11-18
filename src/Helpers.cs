using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAS.EG.Controls;
using SAS.Tasks.Toolkit.Data;
using SAS.Shared.AddIns;
using SAS.Tasks.Toolkit;

namespace RunningTotals
{
    internal class Helpers
    {
        /// <summary>
        /// In the special case where we have a local SAS data set file (sas7bdat),
        /// and a local SAS server, we have to make sure that there is a library
        /// assigned.  
        /// </summary>
        /// <param name="sd"></param>
        internal static void AssignLocalLibraryIfNeeded(ISASTaskConsumer3 consumer)
        {
            SAS.Tasks.Toolkit.Data.SasData sd = new SAS.Tasks.Toolkit.Data.SasData(consumer.ActiveData as ISASTaskData2);
            // get a SasServer object so we can see if it's the "Local" server
            SAS.Tasks.Toolkit.SasServer server = new SAS.Tasks.Toolkit.SasServer(sd.Server);
            // local server with local file, so we have to assign library
            if (server.IsLocal)
            {
                // see if the data reference is a file path ("c:\data\myfile.sas7bdat")
                if (!string.IsNullOrEmpty(consumer.ActiveData.File) &&
                    consumer.ActiveData.Source == SourceType.SasDataset &&
                    consumer.ActiveData.File.Contains("\\"))
                {
                    string path = System.IO.Path.GetDirectoryName(consumer.ActiveData.File);
                    SasSubmitter submitter = new SasSubmitter(sd.Server);
                    string log;
                    submitter.SubmitSasProgramAndWait(string.Format("libname {0} \"{1}\";\r\n", sd.Libref, path), out log);
                }
            }
        }

        /// <summary>
        /// Build a List of AddVariableParams that can be added
        /// to the SAS Variable Selector control
        /// </summary>
        /// <param name="data">A SasData object for the input data used
        /// in the variable selector</param>
        /// <returns>A list of AddVariableParams objects that
        /// can be added to the control</returns>
        public static List<SASVariableSelector.AddVariableParams> 
            BuildVariableParamsList(SasData data)
        {
            // Allocate the list
            List<SASVariableSelector.AddVariableParams> parmList = 
                new List<SASVariableSelector.AddVariableParams>();

            foreach (SasColumn col in data.GetColumns())
            {
                SASVariableSelector.AddVariableParams parms = 
                    new SASVariableSelector.AddVariableParams();

                // populate the column properties
                // from the SasColumn entry
                parms.Name = col.Name;
                parms.Label = col.Label;
                parms.Format = col.Format;
                parms.Informat = col.Informat;

                // map the column category
                // to the variable selector
                // version of this enumeration
                // Ensures the correct "type" icon is
                // shown in the variable selector
                switch (col.Category)
                {
                    case SasColumn.eCategory.Character:
                        parms.Type = VARTYPE.Character;
                        break;
                    case SasColumn.eCategory.Numeric:
                        parms.Type = VARTYPE.Numeric;
                        break;
                    case SasColumn.eCategory.Currency:
                        parms.Type = VARTYPE.Currency;
                        break;
                    case SasColumn.eCategory.Date:
                        parms.Type = VARTYPE.Date;
                        break;
                    case SasColumn.eCategory.DateTime:
                        parms.Type = VARTYPE.Time;
                        break;
                    case SasColumn.eCategory.Georef:
                        parms.Type = VARTYPE.GeoRef;
                        break;

                    default:
                        parms.Type = VARTYPE.Numeric;
                        break;
                }
                // add the complete object to the list
                // to return
                parmList.Add(parms);
            }
            return parmList;
        }
    }
}
