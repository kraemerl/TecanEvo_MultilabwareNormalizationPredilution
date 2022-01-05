namespace TecanEvo_MultilabwareNormalizationPredilution
{
    partial class ScanReport
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScanReport));
            this.butContinue = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.lblDestinationSamples = new System.Windows.Forms.Label();
            this.lblSourceSamples = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblMethod = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.grdTransferOverview = new System.Windows.Forms.DataGridView();
            this.source = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.patient_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sample_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.source_barcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.source_volume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.source_concentration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.destination_barcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.destination_useable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sample_needed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buffer_needed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTargetVolume = new System.Windows.Forms.Label();
            this.lblTargetConcentration = new System.Windows.Forms.Label();
            this.lblSampleDestination = new System.Windows.Forms.Label();
            this.lblTargetConcentrationText = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblLunaticMeasurement = new System.Windows.Forms.Label();
            this.pNormalize = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSampleSource = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblMode = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblTargetAmount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdTransferOverview)).BeginInit();
            this.pNormalize.SuspendLayout();
            this.SuspendLayout();
            // 
            // butContinue
            // 
            this.butContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butContinue.Enabled = false;
            this.butContinue.Location = new System.Drawing.Point(798, 492);
            this.butContinue.Name = "butContinue";
            this.butContinue.Size = new System.Drawing.Size(75, 23);
            this.butContinue.TabIndex = 51;
            this.butContinue.Text = "Continue";
            this.butContinue.UseVisualStyleBackColor = true;
            this.butContinue.Click += new System.EventHandler(this.butContinue_Click);
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.Location = new System.Drawing.Point(879, 492);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 23);
            this.butCancel.TabIndex = 50;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // lblDestinationSamples
            // 
            this.lblDestinationSamples.AutoSize = true;
            this.lblDestinationSamples.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDestinationSamples.Location = new System.Drawing.Point(726, 32);
            this.lblDestinationSamples.Name = "lblDestinationSamples";
            this.lblDestinationSamples.Size = new System.Drawing.Size(28, 13);
            this.lblDestinationSamples.TabIndex = 66;
            this.lblDestinationSamples.Text = "???";
            // 
            // lblSourceSamples
            // 
            this.lblSourceSamples.AutoSize = true;
            this.lblSourceSamples.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSourceSamples.Location = new System.Drawing.Point(726, 15);
            this.lblSourceSamples.Name = "lblSourceSamples";
            this.lblSourceSamples.Size = new System.Drawing.Size(28, 13);
            this.lblSourceSamples.TabIndex = 65;
            this.lblSourceSamples.Text = "???";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(566, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(154, 13);
            this.label6.TabIndex = 64;
            this.label6.Text = "Number of destination samples:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(566, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 13);
            this.label4.TabIndex = 63;
            this.label4.Text = "Number of source samples:";
            // 
            // lblMethod
            // 
            this.lblMethod.AutoSize = true;
            this.lblMethod.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMethod.Location = new System.Drawing.Point(112, 48);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Size = new System.Drawing.Size(28, 13);
            this.lblMethod.TabIndex = 62;
            this.lblMethod.Text = "???";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(4, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 61;
            this.label5.Text = "Method:";
            // 
            // grdTransferOverview
            // 
            this.grdTransferOverview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdTransferOverview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.grdTransferOverview.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.source,
            this.patient_id,
            this.sample_id,
            this.source_barcode,
            this.category,
            this.source_volume,
            this.source_concentration,
            this.destination_barcode,
            this.destination_useable,
            this.sample_needed,
            this.buffer_needed});
            this.grdTransferOverview.Location = new System.Drawing.Point(12, 132);
            this.grdTransferOverview.Name = "grdTransferOverview";
            this.grdTransferOverview.ReadOnly = true;
            this.grdTransferOverview.Size = new System.Drawing.Size(942, 354);
            this.grdTransferOverview.TabIndex = 60;
            // 
            // source
            // 
            this.source.HeaderText = "source";
            this.source.Name = "source";
            this.source.ReadOnly = true;
            this.source.Width = 64;
            // 
            // patient_id
            // 
            this.patient_id.HeaderText = "patient_id";
            this.patient_id.Name = "patient_id";
            this.patient_id.ReadOnly = true;
            this.patient_id.Width = 78;
            // 
            // sample_id
            // 
            this.sample_id.HeaderText = "sample_id";
            this.sample_id.Name = "sample_id";
            this.sample_id.ReadOnly = true;
            this.sample_id.Width = 79;
            // 
            // source_barcode
            // 
            this.source_barcode.HeaderText = "source BC";
            this.source_barcode.Name = "source_barcode";
            this.source_barcode.ReadOnly = true;
            this.source_barcode.Width = 81;
            // 
            // category
            // 
            this.category.HeaderText = "category";
            this.category.Name = "category";
            this.category.ReadOnly = true;
            this.category.Width = 73;
            // 
            // source_volume
            // 
            this.source_volume.HeaderText = "volume (µl)";
            this.source_volume.Name = "source_volume";
            this.source_volume.ReadOnly = true;
            this.source_volume.Width = 83;
            // 
            // source_concentration
            // 
            this.source_concentration.HeaderText = "concentration (ng/µl)";
            this.source_concentration.Name = "source_concentration";
            this.source_concentration.ReadOnly = true;
            this.source_concentration.Width = 131;
            // 
            // destination_barcode
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.destination_barcode.DefaultCellStyle = dataGridViewCellStyle1;
            this.destination_barcode.HeaderText = "destination BC";
            this.destination_barcode.Name = "destination_barcode";
            this.destination_barcode.ReadOnly = true;
            // 
            // destination_useable
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.destination_useable.DefaultCellStyle = dataGridViewCellStyle2;
            this.destination_useable.HeaderText = "is useable";
            this.destination_useable.Name = "destination_useable";
            this.destination_useable.ReadOnly = true;
            this.destination_useable.Width = 79;
            // 
            // sample_needed
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.sample_needed.DefaultCellStyle = dataGridViewCellStyle3;
            this.sample_needed.HeaderText = "sample (µl)";
            this.sample_needed.Name = "sample_needed";
            this.sample_needed.ReadOnly = true;
            this.sample_needed.Width = 82;
            // 
            // buffer_needed
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.buffer_needed.DefaultCellStyle = dataGridViewCellStyle4;
            this.buffer_needed.HeaderText = "TE (µl)";
            this.buffer_needed.Name = "buffer_needed";
            this.buffer_needed.ReadOnly = true;
            this.buffer_needed.Width = 63;
            // 
            // lblTargetVolume
            // 
            this.lblTargetVolume.AutoSize = true;
            this.lblTargetVolume.Location = new System.Drawing.Point(433, 15);
            this.lblTargetVolume.Name = "lblTargetVolume";
            this.lblTargetVolume.Size = new System.Drawing.Size(25, 13);
            this.lblTargetVolume.TabIndex = 59;
            this.lblTargetVolume.Text = "???";
            // 
            // lblTargetConcentration
            // 
            this.lblTargetConcentration.AutoSize = true;
            this.lblTargetConcentration.Location = new System.Drawing.Point(433, 32);
            this.lblTargetConcentration.Name = "lblTargetConcentration";
            this.lblTargetConcentration.Size = new System.Drawing.Size(25, 13);
            this.lblTargetConcentration.TabIndex = 58;
            this.lblTargetConcentration.Text = "???";
            // 
            // lblSampleDestination
            // 
            this.lblSampleDestination.AutoSize = true;
            this.lblSampleDestination.Location = new System.Drawing.Point(111, 11);
            this.lblSampleDestination.Name = "lblSampleDestination";
            this.lblSampleDestination.Size = new System.Drawing.Size(25, 13);
            this.lblSampleDestination.TabIndex = 57;
            this.lblSampleDestination.Text = "???";
            // 
            // lblTargetConcentrationText
            // 
            this.lblTargetConcentrationText.AutoSize = true;
            this.lblTargetConcentrationText.Location = new System.Drawing.Point(283, 32);
            this.lblTargetConcentrationText.Name = "lblTargetConcentrationText";
            this.lblTargetConcentrationText.Size = new System.Drawing.Size(144, 13);
            this.lblTargetConcentrationText.TabIndex = 55;
            this.lblTargetConcentrationText.Text = "Target Concentration (ng/µl):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(283, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 54;
            this.label3.Text = "Target Volume (µl):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 53;
            this.label2.Text = "Sample Destination:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(566, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 13);
            this.label7.TabIndex = 67;
            this.label7.Text = "Lunatic Measurement:";
            // 
            // lblLunaticMeasurement
            // 
            this.lblLunaticMeasurement.AutoSize = true;
            this.lblLunaticMeasurement.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLunaticMeasurement.Location = new System.Drawing.Point(726, 48);
            this.lblLunaticMeasurement.Name = "lblLunaticMeasurement";
            this.lblLunaticMeasurement.Size = new System.Drawing.Size(23, 13);
            this.lblLunaticMeasurement.TabIndex = 68;
            this.lblLunaticMeasurement.Text = "No";
            // 
            // pNormalize
            // 
            this.pNormalize.Controls.Add(this.label9);
            this.pNormalize.Controls.Add(this.lblTargetAmount);
            this.pNormalize.Controls.Add(this.label5);
            this.pNormalize.Controls.Add(this.lblLunaticMeasurement);
            this.pNormalize.Controls.Add(this.lblSampleDestination);
            this.pNormalize.Controls.Add(this.label3);
            this.pNormalize.Controls.Add(this.label7);
            this.pNormalize.Controls.Add(this.label2);
            this.pNormalize.Controls.Add(this.lblTargetConcentrationText);
            this.pNormalize.Controls.Add(this.lblDestinationSamples);
            this.pNormalize.Controls.Add(this.lblTargetConcentration);
            this.pNormalize.Controls.Add(this.lblSourceSamples);
            this.pNormalize.Controls.Add(this.lblTargetVolume);
            this.pNormalize.Controls.Add(this.label6);
            this.pNormalize.Controls.Add(this.lblMethod);
            this.pNormalize.Controls.Add(this.label4);
            this.pNormalize.Location = new System.Drawing.Point(8, 55);
            this.pNormalize.Name = "pNormalize";
            this.pNormalize.Size = new System.Drawing.Size(946, 71);
            this.pNormalize.TabIndex = 69;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 52;
            this.label1.Text = "Sample Source:";
            // 
            // lblSampleSource
            // 
            this.lblSampleSource.AutoSize = true;
            this.lblSampleSource.Location = new System.Drawing.Point(120, 39);
            this.lblSampleSource.Name = "lblSampleSource";
            this.lblSampleSource.Size = new System.Drawing.Size(25, 13);
            this.lblSampleSource.TabIndex = 56;
            this.lblSampleSource.Text = "???";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(12, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 16);
            this.label8.TabIndex = 70;
            this.label8.Text = "Mode:";
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMode.Location = new System.Drawing.Point(69, 9);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(32, 16);
            this.lblMode.TabIndex = 71;
            this.lblMode.Text = "???";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(283, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 13);
            this.label9.TabIndex = 69;
            this.label9.Text = "Target Amount (ng):";
            // 
            // lblTargetAmount
            // 
            this.lblTargetAmount.AutoSize = true;
            this.lblTargetAmount.Location = new System.Drawing.Point(433, 48);
            this.lblTargetAmount.Name = "lblTargetAmount";
            this.lblTargetAmount.Size = new System.Drawing.Size(25, 13);
            this.lblTargetAmount.TabIndex = 70;
            this.lblTargetAmount.Text = "???";
            // 
            // ScanReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(966, 527);
            this.Controls.Add(this.lblMode);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.pNormalize);
            this.Controls.Add(this.grdTransferOverview);
            this.Controls.Add(this.lblSampleSource);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.butContinue);
            this.Controls.Add(this.butCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ScanReport";
            this.Text = "EVO MCA: ScanReport";
            this.Load += new System.EventHandler(this.ScanReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdTransferOverview)).EndInit();
            this.pNormalize.ResumeLayout(false);
            this.pNormalize.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butContinue;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Label lblDestinationSamples;
        private System.Windows.Forms.Label lblSourceSamples;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblMethod;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView grdTransferOverview;
        private System.Windows.Forms.DataGridViewTextBoxColumn source;
        private System.Windows.Forms.DataGridViewTextBoxColumn patient_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn sample_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn source_barcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn category;
        private System.Windows.Forms.DataGridViewTextBoxColumn source_volume;
        private System.Windows.Forms.DataGridViewTextBoxColumn source_concentration;
        private System.Windows.Forms.DataGridViewTextBoxColumn destination_barcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn destination_useable;
        private System.Windows.Forms.DataGridViewTextBoxColumn sample_needed;
        private System.Windows.Forms.DataGridViewTextBoxColumn buffer_needed;
        private System.Windows.Forms.Label lblTargetVolume;
        private System.Windows.Forms.Label lblTargetConcentration;
        private System.Windows.Forms.Label lblSampleDestination;
        private System.Windows.Forms.Label lblTargetConcentrationText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblLunaticMeasurement;
        private System.Windows.Forms.Panel pNormalize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSampleSource;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblTargetAmount;
    }
}