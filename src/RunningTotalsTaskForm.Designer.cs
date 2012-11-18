namespace RunningTotals
{
    partial class RunningTotalsTaskForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SAS.Tasks.Toolkit.Controls.FilterSettings filterSettings1 = new SAS.Tasks.Toolkit.Controls.FilterSettings();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.selDataCtl = new SAS.Tasks.Toolkit.Controls.TaskSelectedDataControl();
            this.varSelCtl = new SAS.EG.Controls.SASVariableSelector();
            this.btnBrowseOut = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.lblOutData = new System.Windows.Forms.Label();
            this.lblOutCol = new System.Windows.Forms.Label();
            this.txtTotalsCol = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(277, 336);
            this.btnOK.Margin = new System.Windows.Forms.Padding(2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(67, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(348, 336);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // selDataCtl
            // 
            this.selDataCtl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.selDataCtl.Consumer = null;
            this.selDataCtl.FilterSettings = filterSettings1;
            this.selDataCtl.HideEditButton = false;
            this.selDataCtl.Location = new System.Drawing.Point(13, 13);
            this.selDataCtl.Name = "selDataCtl";
            this.selDataCtl.Size = new System.Drawing.Size(402, 46);
            this.selDataCtl.TabIndex = 0;
            this.selDataCtl.TaskData = null;
            this.selDataCtl.UseLabelsForVarNames = false;
            this.selDataCtl.DataSelectionChanged += new System.EventHandler(this.OnDataSelectionChanged);
            // 
            // varSelCtl
            // 
            this.varSelCtl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.varSelCtl.CacheDirtyFlag = false;
            this.varSelCtl.CachePath = null;
            this.varSelCtl.ColumnRolesTitle = "";
            this.varSelCtl.ColumnsToAssignTitle = "";
            this.varSelCtl.Location = new System.Drawing.Point(13, 66);
            this.varSelCtl.Name = "varSelCtl";
            this.varSelCtl.Size = new System.Drawing.Size(402, 195);
            this.varSelCtl.TabIndex = 1;
            // 
            // btnBrowseOut
            // 
            this.btnBrowseOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseOut.Location = new System.Drawing.Point(340, 299);
            this.btnBrowseOut.Name = "btnBrowseOut";
            this.btnBrowseOut.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseOut.TabIndex = 6;
            this.btnBrowseOut.Text = "Browse...";
            this.btnBrowseOut.UseVisualStyleBackColor = true;
            this.btnBrowseOut.Click += new System.EventHandler(this.btnBrowseOut_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(95, 299);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(239, 20);
            this.txtOutput.TabIndex = 5;
            this.txtOutput.TextChanged += new System.EventHandler(this.OnOutputDataName_Changed);
            // 
            // lblOutData
            // 
            this.lblOutData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblOutData.AutoSize = true;
            this.lblOutData.Location = new System.Drawing.Point(14, 303);
            this.lblOutData.Name = "lblOutData";
            this.lblOutData.Size = new System.Drawing.Size(66, 13);
            this.lblOutData.TabIndex = 4;
            this.lblOutData.Text = "Output data:";
            // 
            // lblOutCol
            // 
            this.lblOutCol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblOutCol.AutoSize = true;
            this.lblOutCol.Location = new System.Drawing.Point(14, 277);
            this.lblOutCol.Name = "lblOutCol";
            this.lblOutCol.Size = new System.Drawing.Size(69, 13);
            this.lblOutCol.TabIndex = 2;
            this.lblOutCol.Text = "New column:";
            // 
            // txtTotalsCol
            // 
            this.txtTotalsCol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotalsCol.Location = new System.Drawing.Point(95, 273);
            this.txtTotalsCol.Name = "txtTotalsCol";
            this.txtTotalsCol.Size = new System.Drawing.Size(239, 20);
            this.txtTotalsCol.TabIndex = 3;
            this.txtTotalsCol.TextChanged += new System.EventHandler(this.OnTotalsVariable_Changed);
            // 
            // RunningTotalsTaskForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(427, 368);
            this.Controls.Add(this.lblOutCol);
            this.Controls.Add(this.txtTotalsCol);
            this.Controls.Add(this.lblOutData);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnBrowseOut);
            this.Controls.Add(this.varSelCtl);
            this.Controls.Add(this.selDataCtl);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "RunningTotalsTaskForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Your task title here";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private SAS.Tasks.Toolkit.Controls.TaskSelectedDataControl selDataCtl;
        private SAS.EG.Controls.SASVariableSelector varSelCtl;
        private System.Windows.Forms.Button btnBrowseOut;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Label lblOutData;
        private System.Windows.Forms.Label lblOutCol;
        private System.Windows.Forms.TextBox txtTotalsCol;
    }
}