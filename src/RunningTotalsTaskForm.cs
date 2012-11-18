using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using SAS.Shared.AddIns;
using SAS.Tasks.Toolkit.Controls;
using SAS.EG.Controls;
using System.Collections.Generic;
using SAS.Tasks.Toolkit.Data;
using System.Text.RegularExpressions;

namespace RunningTotals
{
    /// <summary>
    /// This windows form inherits from the TaskForm class, which
    /// includes a bit of special handling for SAS Enterprise Guide.
    /// </summary>
    public partial class RunningTotalsTaskForm : SAS.Tasks.Toolkit.Controls.TaskForm
    {
        public RunningTotalsTaskSettings Settings { get; set; }

        public RunningTotalsTaskForm(SAS.Shared.AddIns.ISASTaskConsumer3 consumer)
        {
            InitializeComponent();

            // provide a handle to the SAS Enterprise Guide application
            this.Consumer = consumer;
        }

        // save any values from the dialog into the settings class
        protected override void OnClosing(CancelEventArgs e)
        {
            // if clicked OK, then save our settings
            if (this.DialogResult == DialogResult.OK)
            {
                Settings.DataOut = txtOutput.Text;

                Settings.VariableMeasure = 
                    varSelCtl.GetAssignedVariable(Translate.MeasureValueRole, 0);

                Settings.VariableGroups.Clear();
                int groupers = varSelCtl.GetNumberOfAssignedVariables(Translate.GroupingRole);
                if (groupers > 0)
                {
                    for (int i = 0; i < groupers; i++)
                    {
                        Settings.VariableGroups.Add(varSelCtl.GetAssignedVariable(Translate.GroupingRole, i));
                    }
                }

                Settings.VariableTotal = txtTotalsCol.Text;
            }

            base.OnClosing(e);
        }

        // initialize the form with the values from the settings
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // initialize the data selection control with a
            // handle to the Consumer application
            // and the active data
            selDataCtl.Consumer = Consumer;
            selDataCtl.TaskData = Consumer.InputData[0] as ISASTaskData2;
            selDataCtl.FilterSettings = Settings.FilterSettings;

            // add the Roles to the variable selector
            // Best practice is to rely on names/titles from 
            // resource files, as these will be consistent with the
            // UI if the resources are localized
            varSelCtl.ColumnsToAssignTitle = Translate.ColumnsTitle;
            varSelCtl.ColumnRolesTitle = Translate.RolesTitleBar;

            // add the roles to the variable selector
            SetupVariableRoles();

            // call the routine to add the variables from the input data
            // and initialize values from saved settings
            SetupDataForVarSelector();

            txtOutput.Text = Settings.DataOut;

            txtTotalsCol.Text = Settings.VariableTotal;

            UpdateStatus();
        }

        private void SetupVariableRoles()
        {
            // define the two roles that we're going to use for this task

            // this one is the "Measure" role, which requires a numeric variable
            SASVariableSelector.AddRoleParams parms = new SASVariableSelector.AddRoleParams();
            parms.Name = Translate.MeasureValueRole;
            parms.MinNumVars = 1; // Min of 1 says required
            parms.MaxNumVars = 1; // one and only one
            // not going to allow "macro"-style prompts
            parms.AcceptsMacroVars = false;
            parms.Type = SASVariableSelector.ROLETYPE.Numeric;
            varSelCtl.AddRole(parms);

            // and this one is the "grouping" role, optional
            SASVariableSelector.AddRoleParams labelParm = new SASVariableSelector.AddRoleParams();
            labelParm.Name = Translate.GroupingRole;
            labelParm.MinNumVars = 0; // optional
            // No labelParm.MaxNumVars to allow as many as you want
            // not going to allow "macro"-style prompts
            labelParm.AcceptsMacroVars = false;
            labelParm.Type = SASVariableSelector.ROLETYPE.All;
            varSelCtl.AddRole(labelParm);
        }


        private void SetupDataForVarSelector()
        {
            if (this.Consumer.InputData.Length > 0)
            {
                SasData data = new SasData(
                    this.Consumer.InputData[0] as ISASTaskData2
                    );

                // this helps to "realize" the data if it
                // is a local data set
                // we need it to be in an assigned library
                // in order to read the columns from the SasData object
                Helpers.AssignLocalLibraryIfNeeded(Consumer);

                // this builds the variable list
                // in the format that the variable selector needs
                List<SASVariableSelector.AddVariableParams> parmList = 
                    Helpers.BuildVariableParamsList(data);
                varSelCtl.AddVariables(parmList);

                // assign the selected measure, if any
                if (!string.IsNullOrEmpty(Settings.VariableMeasure))
                {
                    if (data.ContainsColumn(Settings.VariableMeasure))
                        varSelCtl.AssignVariable(Translate.MeasureValueRole, 
                            Settings.VariableMeasure);
                }

                // assign the selected group vars, if any
                foreach (string var in Settings.VariableGroups)
                {
                    if (data.ContainsColumn(var))
                        varSelCtl.AssignVariable(Translate.GroupingRole, var);
                }

                // listen on assigned/unassigned events 
                varSelCtl.VariableAssigned += 
                    new SASVariableSelector.VariableAssignedEventHandler(varSelCtl_VariableAssigned);
                varSelCtl.VariableDeassigned += 
                    new SASVariableSelector.VariableDeassignedEventHandler(varSelCtl_VariableDeassigned);
            }
        }

        // handle events from the variable selector
        // each time there is a change (variable assigned or unassigned)
        // update the status to indicate whether we have
        // the assignments that we need
        private bool varSelectorValid = false;
        void varSelCtl_VariableAssigned(object sender, VariableAssignedEventArgs ea)
        {
            varSelectorValid = ea.MeetsRequirements;

            // if measure var is assigned
            // generate a default output totals name
            if (ea.RoleName == Translate.MeasureValueRole)
                txtTotalsCol.Text = string.Format("totals_{0}",
                    SAS.Tasks.Toolkit.Helpers.UtilityFunctions.GetValidSasName(ea.VarName,24));

            UpdateStatus();
        }

        void varSelCtl_VariableDeassigned(object sender, VariableDeassignedEventArgs ea)
        {
            varSelectorValid = ea.MeetsRequirements;
            UpdateStatus();
        }

        // Allow user to browse to output data location
        private void btnBrowseOut_Click(object sender, EventArgs e)
        {
            // this calls back into application
            // and tells it to display browse dialog
            string cookie = "";
            ISASTaskDataName od = Consumer.ShowOutputDataSelector(this,
                ServerAccessMode.OneServer,
                Consumer.AssignedServer,
                "", "", ref cookie);

            // the ShowOutputDataSelector doesn't return
            // a cancel status.  So if it produces
            // a null value, that tells us it was
            // Cancel'd
            if (od != null)
            {
                txtOutput.Text = 
                    string.Format("{0}.{1}", od.Library, od.Member);
            }
        }

        /// <summary>
        /// User changed the data selection - active data or filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDataSelectionChanged(object sender, EventArgs e)
        {
            FilterSettings fs = selDataCtl.FilterSettings;
            Settings.FilterSettings = fs;
        }

        private void UpdateStatus()
        {
            bool bValid =
             varSelCtl.GetNumberOfAssignedVariables(Translate.MeasureValueRole)>0 &&
             isValidVarName(txtTotalsCol.Text) &&
             isValidOutput(txtOutput.Text);

            btnOK.Enabled = bValid;

        }

        private bool isValidVarName(string name)
        {
            // a regular expression to match a variable name
            // 1-32 chars, begin with alpha, then alphanumeric
            // or underscores
            Regex regex = new Regex(
                "^(?=.{1,32}$)([a-zA-Z_][a-zA-Z0-9_]*)$", 
                RegexOptions.Compiled);
            return (regex.IsMatch(txtTotalsCol.Text));
        }

        private bool isValidOutput(string output)
        {
            // validate dataset name - some crude validation
            // a regular expression would be better
            string[] parts = output.Trim().Split(new char[] { '.' });
            if (output.Trim().Contains(" ") ||
                parts.Length != 2 ||
                parts[0].IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) > -1 ||
                parts[1].IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) > -1)
            {
                return false;
            }
            return true;
        }

        private void OnOutputDataName_Changed(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void OnTotalsVariable_Changed(object sender, EventArgs e)
        {
            UpdateStatus();
        }

    }
}
