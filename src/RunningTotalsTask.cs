using System;
using System.Text;
using SAS.Shared.AddIns;
using SAS.Tasks.Toolkit;
using System.Drawing;

namespace RunningTotals
{
    // unique identifier for this task
    [ClassId("b9f68fbb-da5d-4e09-968d-7b71690bb09c")]
    // location of the task icon to show in the menu and process flow
    [IconLocation("RunningTotals.task.ico")]
    // does this task require input data? 
    // InputResourceType.Data for data set, or 
    // InputResourceType.None for none required
    [InputRequired(InputResourceType.Data)]
    public class RunningTotalsTask : SAS.Tasks.Toolkit.SasTask
    {
        #region private members

        private RunningTotalsTaskSettings settings;

        #endregion

        #region Initialization
        public RunningTotalsTask()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // 
            // RunningTotalsTask
            // 
            this.ProcsUsed = "DATA step";
            this.ProductsRequired = "BASE";
            this.TaskCategory = "SAS Press Examples";
            this.TaskDescription = "Calculate running totals for a measure";
            this.TaskName = "Calculate Running Totals";

        }
        #endregion

        #region overrides
        public override bool Initialize()
        {
            settings = new RunningTotalsTaskSettings();
            return true;
        }

        public override string GetXmlState()
        {
            return settings.ToXml();
        }

        public override void RestoreStateFromXml(string xmlState)
        {
            settings = new RunningTotalsTaskSettings();
            settings.FromXml(xmlState);
        }

        /// <summary>
        /// Show the task user interface
        /// </summary>
        /// <param name="Owner"></param>
        /// <returns>whether to cancel the task, or run now</returns>
        public override ShowResult Show(System.Windows.Forms.IWin32Window Owner)
        {
            RunningTotalsTaskForm dlg = new RunningTotalsTaskForm(this.Consumer);
            dlg.Icon = new Icon(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(this.IconName));
            dlg.Text = this.Label;
            dlg.Settings = settings;
            if (System.Windows.Forms.DialogResult.OK == dlg.ShowDialog(Owner))
            {
                // gather settings values from the dialog
                settings = dlg.Settings;
                return ShowResult.RunNow;
            }
            else
                return ShowResult.Canceled;
        }

        /// <summary>
        /// Get the SAS program that this task should generate
        /// based on the options specified.
        /// </summary>
        /// <returns>a valid SAS program to run</returns>
        public override string GetSasCode()
        {
            return settings.GetSasProgram(Consumer);
        }

        /// <summary>
        /// Always going to return exactly one data set
        /// </summary>
        public override int OutputDataCount
        {
            get { return 1; }
        }

        /// <summary>
        /// Return a list of output data expected
        /// Just one, in this case
        /// </summary>
        public override System.Collections.Generic.List<SAS.Shared.AddIns.ISASTaskDataDescriptor> OutputDataDescriptorList
        {
            get
            {
                System.Collections.Generic.List<ISASTaskDataDescriptor> outList =
                    new System.Collections.Generic.List<ISASTaskDataDescriptor>();

                string[] parts = settings.DataOut.Split(new char[] { '.' });
                if (parts.Length == 2)
                {
                    outList.Add(
                        // use this helper method to build the output descriptor
                        SAS.Shared.AddIns.SASTaskDataDescriptor.CreateLibrefDataDescriptor(
                            Consumer.AssignedServer, parts[0], parts[1], "")
                        );
                }
                return outList;
            }
        }
        #endregion

    }
}
