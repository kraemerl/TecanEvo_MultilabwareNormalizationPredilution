namespace TecanEvo_MultilabwareNormalizationPredilution
{
    partial class InitializeForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitializeForm));
            this.butCancel = new System.Windows.Forms.Button();
            this.butOK = new System.Windows.Forms.Button();
            this.lblAmount = new System.Windows.Forms.Label();
            this.lblConcentration = new System.Windows.Forms.Label();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.chkNormalize = new System.Windows.Forms.CheckBox();
            this.txtConcentration = new System.Windows.Forms.TextBox();
            this.txtVolume = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDestinationBoxPrefix = new System.Windows.Forms.TextBox();
            this.lblDestinationBoxPrefix = new System.Windows.Forms.Label();
            this.txtDestinationBoxBarcode = new System.Windows.Forms.TextBox();
            this.txtSourceBoxBarcode = new System.Windows.Forms.TextBox();
            this.lblDestinationBoxBarcode = new System.Windows.Forms.Label();
            this.lblSourceBoxBarcode = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbDestination = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbSource = new System.Windows.Forms.ComboBox();
            this.chkLunatic = new System.Windows.Forms.CheckBox();
            this.chkMeasureVolume = new System.Windows.Forms.CheckBox();
            this.txtExportVolumeScriptPath = new System.Windows.Forms.TextBox();
            this.lblExportVolumeScriptPath = new System.Windows.Forms.Label();
            this.butExportVolumeScriptPath = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.chkAspirateZMax = new System.Windows.Forms.CheckBox();
            this.chkMix = new System.Windows.Forms.CheckBox();
            this.rbNormalization = new System.Windows.Forms.RadioButton();
            this.rbMeasurement = new System.Windows.Forms.RadioButton();
            this.pNormalization = new System.Windows.Forms.Panel();
            this.rbNormalizeAmount = new System.Windows.Forms.RadioButton();
            this.rbNormalizeConc = new System.Windows.Forms.RadioButton();
            this.cbLunaticProgram = new System.Windows.Forms.ComboBox();
            this.chkLunaticProgram = new System.Windows.Forms.CheckBox();
            this.chkIBDbase = new System.Windows.Forms.CheckBox();
            this.pNormalization.SuspendLayout();
            this.SuspendLayout();
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.Location = new System.Drawing.Point(289, 540);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 23);
            this.butCancel.TabIndex = 5;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // butOK
            // 
            this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butOK.Location = new System.Drawing.Point(208, 540);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(75, 23);
            this.butOK.TabIndex = 9;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // lblAmount
            // 
            this.lblAmount.AutoSize = true;
            this.lblAmount.Enabled = false;
            this.lblAmount.Location = new System.Drawing.Point(29, 311);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(127, 13);
            this.lblAmount.TabIndex = 35;
            this.lblAmount.Text = "Target DNA Amount (ng):";
            // 
            // lblConcentration
            // 
            this.lblConcentration.AutoSize = true;
            this.lblConcentration.Enabled = false;
            this.lblConcentration.Location = new System.Drawing.Point(29, 285);
            this.lblConcentration.Name = "lblConcentration";
            this.lblConcentration.Size = new System.Drawing.Size(144, 13);
            this.lblConcentration.TabIndex = 34;
            this.lblConcentration.Text = "Target Concentration (ng/µl):";
            // 
            // txtAmount
            // 
            this.txtAmount.Enabled = false;
            this.txtAmount.Location = new System.Drawing.Point(179, 308);
            this.txtAmount.MaxLength = 5;
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(54, 20);
            this.txtAmount.TabIndex = 33;
            this.txtAmount.Text = "0";
            this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtAmount.TextChanged += new System.EventHandler(this.txtAmount_TextChanged);
            // 
            // chkNormalize
            // 
            this.chkNormalize.AutoSize = true;
            this.chkNormalize.Location = new System.Drawing.Point(11, 230);
            this.chkNormalize.Name = "chkNormalize";
            this.chkNormalize.Size = new System.Drawing.Size(282, 17);
            this.chkNormalize.TabIndex = 30;
            this.chkNormalize.Text = "normalize (will automatically add 3µl to Target Volume):";
            this.chkNormalize.UseVisualStyleBackColor = true;
            this.chkNormalize.CheckedChanged += new System.EventHandler(this.chkNormalize_CheckedChanged_1);
            // 
            // txtConcentration
            // 
            this.txtConcentration.Enabled = false;
            this.txtConcentration.Location = new System.Drawing.Point(179, 282);
            this.txtConcentration.MaxLength = 5;
            this.txtConcentration.Name = "txtConcentration";
            this.txtConcentration.Size = new System.Drawing.Size(54, 20);
            this.txtConcentration.TabIndex = 31;
            this.txtConcentration.Text = "0";
            this.txtConcentration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtConcentration.TextChanged += new System.EventHandler(this.txtConcentration_TextChanged);
            // 
            // txtVolume
            // 
            this.txtVolume.Location = new System.Drawing.Point(160, 165);
            this.txtVolume.MaxLength = 4;
            this.txtVolume.Name = "txtVolume";
            this.txtVolume.Size = new System.Drawing.Size(54, 20);
            this.txtVolume.TabIndex = 29;
            this.txtVolume.Text = "0";
            this.txtVolume.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtVolume.TextChanged += new System.EventHandler(this.txtVolume_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 168);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 13);
            this.label5.TabIndex = 32;
            this.label5.Text = "Target Volume (µl):";
            // 
            // txtDestinationBoxPrefix
            // 
            this.txtDestinationBoxPrefix.Enabled = false;
            this.txtDestinationBoxPrefix.Location = new System.Drawing.Point(112, 122);
            this.txtDestinationBoxPrefix.MaxLength = 12;
            this.txtDestinationBoxPrefix.Name = "txtDestinationBoxPrefix";
            this.txtDestinationBoxPrefix.Size = new System.Drawing.Size(80, 20);
            this.txtDestinationBoxPrefix.TabIndex = 40;
            // 
            // lblDestinationBoxPrefix
            // 
            this.lblDestinationBoxPrefix.AutoSize = true;
            this.lblDestinationBoxPrefix.Enabled = false;
            this.lblDestinationBoxPrefix.Location = new System.Drawing.Point(8, 125);
            this.lblDestinationBoxPrefix.Name = "lblDestinationBoxPrefix";
            this.lblDestinationBoxPrefix.Size = new System.Drawing.Size(79, 13);
            this.lblDestinationBoxPrefix.TabIndex = 45;
            this.lblDestinationBoxPrefix.Text = "Barcode Prefix:";
            // 
            // txtDestinationBoxBarcode
            // 
            this.txtDestinationBoxBarcode.Enabled = false;
            this.txtDestinationBoxBarcode.Location = new System.Drawing.Point(112, 96);
            this.txtDestinationBoxBarcode.Name = "txtDestinationBoxBarcode";
            this.txtDestinationBoxBarcode.Size = new System.Drawing.Size(100, 20);
            this.txtDestinationBoxBarcode.TabIndex = 39;
            // 
            // txtSourceBoxBarcode
            // 
            this.txtSourceBoxBarcode.Enabled = false;
            this.txtSourceBoxBarcode.Location = new System.Drawing.Point(116, 33);
            this.txtSourceBoxBarcode.Name = "txtSourceBoxBarcode";
            this.txtSourceBoxBarcode.Size = new System.Drawing.Size(100, 20);
            this.txtSourceBoxBarcode.TabIndex = 38;
            // 
            // lblDestinationBoxBarcode
            // 
            this.lblDestinationBoxBarcode.AutoSize = true;
            this.lblDestinationBoxBarcode.Enabled = false;
            this.lblDestinationBoxBarcode.Location = new System.Drawing.Point(8, 99);
            this.lblDestinationBoxBarcode.Name = "lblDestinationBoxBarcode";
            this.lblDestinationBoxBarcode.Size = new System.Drawing.Size(84, 13);
            this.lblDestinationBoxBarcode.TabIndex = 44;
            this.lblDestinationBoxBarcode.Text = "Destination Box:";
            // 
            // lblSourceBoxBarcode
            // 
            this.lblSourceBoxBarcode.AutoSize = true;
            this.lblSourceBoxBarcode.Enabled = false;
            this.lblSourceBoxBarcode.Location = new System.Drawing.Point(12, 36);
            this.lblSourceBoxBarcode.Name = "lblSourceBoxBarcode";
            this.lblSourceBoxBarcode.Size = new System.Drawing.Size(65, 13);
            this.lblSourceBoxBarcode.TabIndex = 43;
            this.lblSourceBoxBarcode.Text = "Source Box:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 42;
            this.label2.Text = "Sample Destination:";
            // 
            // cbDestination
            // 
            this.cbDestination.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDestination.FormattingEnabled = true;
            this.cbDestination.Items.AddRange(new object[] {
            "Eppi 2ml",
            "Micronic Tube 1.4ml",
            "Micronic Tube 0.7ml",
            "PCR Plate",
            "DeepWell Plate",
            "IScan Plate",
            "96 Well LVL SX-300"});
            this.cbDestination.Location = new System.Drawing.Point(112, 9);
            this.cbDestination.Name = "cbDestination";
            this.cbDestination.Size = new System.Drawing.Size(204, 21);
            this.cbDestination.TabIndex = 37;
            this.cbDestination.SelectedIndexChanged += new System.EventHandler(this.cbDestination_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 41;
            this.label1.Text = "Sample Source:";
            // 
            // cbSource
            // 
            this.cbSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSource.FormattingEnabled = true;
            this.cbSource.Items.AddRange(new object[] {
            "Eppi 2ml",
            "Micronic Tube 1.4ml",
            "Micronic Tube 0.7ml",
            "PCR Plate",
            "DeepWell Plate",
            "IScan Plate"});
            this.cbSource.Location = new System.Drawing.Point(116, 6);
            this.cbSource.Name = "cbSource";
            this.cbSource.Size = new System.Drawing.Size(204, 21);
            this.cbSource.TabIndex = 36;
            this.cbSource.SelectedIndexChanged += new System.EventHandler(this.cbSource_SelectedIndexChanged);
            // 
            // chkLunatic
            // 
            this.chkLunatic.AutoSize = true;
            this.chkLunatic.Checked = true;
            this.chkLunatic.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLunatic.Enabled = false;
            this.chkLunatic.Location = new System.Drawing.Point(32, 369);
            this.chkLunatic.Name = "chkLunatic";
            this.chkLunatic.Size = new System.Drawing.Size(127, 17);
            this.chkLunatic.TabIndex = 46;
            this.chkLunatic.Text = "Lunatic measurement";
            this.chkLunatic.UseVisualStyleBackColor = true;
            this.chkLunatic.CheckedChanged += new System.EventHandler(this.chkLunatic_CheckedChanged);
            // 
            // chkMeasureVolume
            // 
            this.chkMeasureVolume.AutoSize = true;
            this.chkMeasureVolume.Enabled = false;
            this.chkMeasureVolume.Location = new System.Drawing.Point(112, 36);
            this.chkMeasureVolume.Name = "chkMeasureVolume";
            this.chkMeasureVolume.Size = new System.Drawing.Size(103, 17);
            this.chkMeasureVolume.TabIndex = 47;
            this.chkMeasureVolume.Text = "measure volume";
            this.chkMeasureVolume.UseVisualStyleBackColor = true;
            this.chkMeasureVolume.CheckedChanged += new System.EventHandler(this.chkMeasureVolume_CheckedChanged);
            // 
            // txtExportVolumeScriptPath
            // 
            this.txtExportVolumeScriptPath.Enabled = false;
            this.txtExportVolumeScriptPath.Location = new System.Drawing.Point(112, 60);
            this.txtExportVolumeScriptPath.Name = "txtExportVolumeScriptPath";
            this.txtExportVolumeScriptPath.Size = new System.Drawing.Size(181, 20);
            this.txtExportVolumeScriptPath.TabIndex = 53;
            // 
            // lblExportVolumeScriptPath
            // 
            this.lblExportVolumeScriptPath.Enabled = false;
            this.lblExportVolumeScriptPath.Location = new System.Drawing.Point(8, 63);
            this.lblExportVolumeScriptPath.Name = "lblExportVolumeScriptPath";
            this.lblExportVolumeScriptPath.Size = new System.Drawing.Size(100, 22);
            this.lblExportVolumeScriptPath.TabIndex = 0;
            this.lblExportVolumeScriptPath.Text = "ExportVolumeScript:";
            // 
            // butExportVolumeScriptPath
            // 
            this.butExportVolumeScriptPath.Enabled = false;
            this.butExportVolumeScriptPath.Location = new System.Drawing.Point(296, 58);
            this.butExportVolumeScriptPath.Name = "butExportVolumeScriptPath";
            this.butExportVolumeScriptPath.Size = new System.Drawing.Size(27, 23);
            this.butExportVolumeScriptPath.TabIndex = 50;
            this.butExportVolumeScriptPath.Text = "...";
            this.butExportVolumeScriptPath.UseVisualStyleBackColor = true;
            this.butExportVolumeScriptPath.Click += new System.EventHandler(this.butExportVolumeScriptPath_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // chkAspirateZMax
            // 
            this.chkAspirateZMax.AutoSize = true;
            this.chkAspirateZMax.Location = new System.Drawing.Point(11, 193);
            this.chkAspirateZMax.Name = "chkAspirateZMax";
            this.chkAspirateZMax.Size = new System.Drawing.Size(117, 17);
            this.chkAspirateZMax.TabIndex = 51;
            this.chkAspirateZMax.Text = "aspirate from z-Max";
            this.chkAspirateZMax.UseVisualStyleBackColor = true;
            this.chkAspirateZMax.CheckedChanged += new System.EventHandler(this.chkAspirateZMax_CheckedChanged);
            // 
            // chkMix
            // 
            this.chkMix.AutoSize = true;
            this.chkMix.Checked = true;
            this.chkMix.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMix.Enabled = false;
            this.chkMix.Location = new System.Drawing.Point(32, 392);
            this.chkMix.Name = "chkMix";
            this.chkMix.Size = new System.Drawing.Size(186, 17);
            this.chkMix.TabIndex = 52;
            this.chkMix.Text = "Mix liquid before Lunatic pipetting ";
            this.chkMix.UseVisualStyleBackColor = true;
            this.chkMix.CheckedChanged += new System.EventHandler(this.chkMix_CheckedChanged);
            // 
            // rbNormalization
            // 
            this.rbNormalization.AutoSize = true;
            this.rbNormalization.Checked = true;
            this.rbNormalization.Location = new System.Drawing.Point(15, 62);
            this.rbNormalization.Name = "rbNormalization";
            this.rbNormalization.Size = new System.Drawing.Size(88, 17);
            this.rbNormalization.TabIndex = 53;
            this.rbNormalization.TabStop = true;
            this.rbNormalization.Text = "Normalization";
            this.rbNormalization.UseVisualStyleBackColor = true;
            this.rbNormalization.CheckedChanged += new System.EventHandler(this.rbNormalization_CheckedChanged);
            // 
            // rbMeasurement
            // 
            this.rbMeasurement.AutoSize = true;
            this.rbMeasurement.Location = new System.Drawing.Point(109, 62);
            this.rbMeasurement.Name = "rbMeasurement";
            this.rbMeasurement.Size = new System.Drawing.Size(89, 17);
            this.rbMeasurement.TabIndex = 54;
            this.rbMeasurement.Text = "Measurement";
            this.rbMeasurement.UseVisualStyleBackColor = true;
            // 
            // pNormalization
            // 
            this.pNormalization.Controls.Add(this.rbNormalizeAmount);
            this.pNormalization.Controls.Add(this.rbNormalizeConc);
            this.pNormalization.Controls.Add(this.cbLunaticProgram);
            this.pNormalization.Controls.Add(this.chkLunaticProgram);
            this.pNormalization.Controls.Add(this.lblExportVolumeScriptPath);
            this.pNormalization.Controls.Add(this.label5);
            this.pNormalization.Controls.Add(this.txtVolume);
            this.pNormalization.Controls.Add(this.butExportVolumeScriptPath);
            this.pNormalization.Controls.Add(this.chkMix);
            this.pNormalization.Controls.Add(this.txtConcentration);
            this.pNormalization.Controls.Add(this.chkAspirateZMax);
            this.pNormalization.Controls.Add(this.chkNormalize);
            this.pNormalization.Controls.Add(this.txtAmount);
            this.pNormalization.Controls.Add(this.txtExportVolumeScriptPath);
            this.pNormalization.Controls.Add(this.lblConcentration);
            this.pNormalization.Controls.Add(this.lblAmount);
            this.pNormalization.Controls.Add(this.chkMeasureVolume);
            this.pNormalization.Controls.Add(this.cbDestination);
            this.pNormalization.Controls.Add(this.chkLunatic);
            this.pNormalization.Controls.Add(this.label2);
            this.pNormalization.Controls.Add(this.txtDestinationBoxPrefix);
            this.pNormalization.Controls.Add(this.lblDestinationBoxBarcode);
            this.pNormalization.Controls.Add(this.lblDestinationBoxPrefix);
            this.pNormalization.Controls.Add(this.txtDestinationBoxBarcode);
            this.pNormalization.Location = new System.Drawing.Point(5, 84);
            this.pNormalization.Name = "pNormalization";
            this.pNormalization.Size = new System.Drawing.Size(367, 450);
            this.pNormalization.TabIndex = 55;
            // 
            // rbNormalizeAmount
            // 
            this.rbNormalizeAmount.AutoSize = true;
            this.rbNormalizeAmount.Enabled = false;
            this.rbNormalizeAmount.Location = new System.Drawing.Point(142, 253);
            this.rbNormalizeAmount.Name = "rbNormalizeAmount";
            this.rbNormalizeAmount.Size = new System.Drawing.Size(74, 17);
            this.rbNormalizeAmount.TabIndex = 57;
            this.rbNormalizeAmount.Text = "by amount";
            this.rbNormalizeAmount.UseVisualStyleBackColor = true;
            this.rbNormalizeAmount.CheckedChanged += new System.EventHandler(this.rbNormalizeAmount_CheckedChanged);
            // 
            // rbNormalizeConc
            // 
            this.rbNormalizeConc.AutoSize = true;
            this.rbNormalizeConc.Checked = true;
            this.rbNormalizeConc.Enabled = false;
            this.rbNormalizeConc.Location = new System.Drawing.Point(32, 253);
            this.rbNormalizeConc.Name = "rbNormalizeConc";
            this.rbNormalizeConc.Size = new System.Drawing.Size(104, 17);
            this.rbNormalizeConc.TabIndex = 56;
            this.rbNormalizeConc.TabStop = true;
            this.rbNormalizeConc.Text = "by concentration";
            this.rbNormalizeConc.UseVisualStyleBackColor = true;
            this.rbNormalizeConc.CheckedChanged += new System.EventHandler(this.rbNormalizeConc_CheckedChanged);
            // 
            // cbLunaticProgram
            // 
            this.cbLunaticProgram.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLunaticProgram.Enabled = false;
            this.cbLunaticProgram.FormattingEnabled = true;
            this.cbLunaticProgram.Items.AddRange(new object[] {
            "660nm Protein Assay",
            "A260 dsDNA",
            "A260 Oligo molar E",
            "A260 RNA",
            "A260 ssDNA",
            "A260 DNA Oligo Sequence",
            "A260 RNA Oligo Sequence",
            "A280 Protein",
            "BCA",
            "Bradford",
            "Culture OD600",
            "DNA Mamm.",
            "DNA Plant",
            "dsDNA -bg(340)",
            "dsDNA Labeled",
            "IgG quant",
            "Lowry",
            "MS peptide quant",
            "Nucleic Acids (DNA equiv.)",
            "Nucleic Acids (RNA equiv.)",
            "OD check",
            "Plasma QC",
            "Protein A280 -bg(340)",
            "Protein Labeled",
            "Purified IgG + imidazole",
            "Purified PCR",
            "RNA",
            "RNA A260 -bg(340)",
            "RNA Cy3 Cy5",
            "RNA FFPE",
            "RNA Labeled",
            "ssDNA A260 -bg(340)",
            "ssDNA Cy3 Cy5",
            "ssDNA Labeled",
            "Standard Curve",
            "Total Protein",
            "Troubleshooting App",
            "UV/Vis"});
            this.cbLunaticProgram.Location = new System.Drawing.Point(166, 413);
            this.cbLunaticProgram.Name = "cbLunaticProgram";
            this.cbLunaticProgram.Size = new System.Drawing.Size(179, 21);
            this.cbLunaticProgram.TabIndex = 55;
            this.cbLunaticProgram.SelectedIndexChanged += new System.EventHandler(this.cbLunaticProgram_SelectedIndexChanged);
            // 
            // chkLunaticProgram
            // 
            this.chkLunaticProgram.AutoSize = true;
            this.chkLunaticProgram.Enabled = false;
            this.chkLunaticProgram.Location = new System.Drawing.Point(32, 415);
            this.chkLunaticProgram.Name = "chkLunaticProgram";
            this.chkLunaticProgram.Size = new System.Drawing.Size(128, 17);
            this.chkLunaticProgram.TabIndex = 54;
            this.chkLunaticProgram.Text = "Custom Measurement";
            this.chkLunaticProgram.UseVisualStyleBackColor = true;
            this.chkLunaticProgram.CheckedChanged += new System.EventHandler(this.chkLunaticProgram_CheckedChanged);
            // 
            // chkIBDbase
            // 
            this.chkIBDbase.AutoSize = true;
            this.chkIBDbase.Checked = true;
            this.chkIBDbase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIBDbase.Location = new System.Drawing.Point(235, 63);
            this.chkIBDbase.Name = "chkIBDbase";
            this.chkIBDbase.Size = new System.Drawing.Size(89, 17);
            this.chkIBDbase.TabIndex = 56;
            this.chkIBDbase.Text = "Use IBDbase";
            this.chkIBDbase.UseVisualStyleBackColor = true;
            this.chkIBDbase.CheckedChanged += new System.EventHandler(this.chkIBDbase_CheckedChanged);
            // 
            // InitializeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 575);
            this.Controls.Add(this.chkIBDbase);
            this.Controls.Add(this.pNormalization);
            this.Controls.Add(this.rbMeasurement);
            this.Controls.Add(this.rbNormalization);
            this.Controls.Add(this.txtSourceBoxBarcode);
            this.Controls.Add(this.lblSourceBoxBarcode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbSource);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.butCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InitializeForm";
            this.Text = "EVO: Multilabware Normalization";
            this.Load += new System.EventHandler(this.InitializeForm_Load);
            this.pNormalization.ResumeLayout(false);
            this.pNormalization.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.Label lblConcentration;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.CheckBox chkNormalize;
        private System.Windows.Forms.TextBox txtConcentration;
        private System.Windows.Forms.TextBox txtVolume;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDestinationBoxPrefix;
        private System.Windows.Forms.Label lblDestinationBoxPrefix;
        private System.Windows.Forms.TextBox txtDestinationBoxBarcode;
        private System.Windows.Forms.TextBox txtSourceBoxBarcode;
        private System.Windows.Forms.Label lblDestinationBoxBarcode;
        private System.Windows.Forms.Label lblSourceBoxBarcode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbDestination;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbSource;
        private System.Windows.Forms.CheckBox chkLunatic;
        private System.Windows.Forms.CheckBox chkMeasureVolume;
        private System.Windows.Forms.TextBox txtExportVolumeScriptPath;
        private System.Windows.Forms.Label lblExportVolumeScriptPath;
        private System.Windows.Forms.Button butExportVolumeScriptPath;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox chkAspirateZMax;
        private System.Windows.Forms.CheckBox chkMix;
        private System.Windows.Forms.RadioButton rbNormalization;
        private System.Windows.Forms.RadioButton rbMeasurement;
        private System.Windows.Forms.Panel pNormalization;
        private System.Windows.Forms.CheckBox chkIBDbase;
        private System.Windows.Forms.ComboBox cbLunaticProgram;
        private System.Windows.Forms.CheckBox chkLunaticProgram;
        private System.Windows.Forms.RadioButton rbNormalizeAmount;
        private System.Windows.Forms.RadioButton rbNormalizeConc;
    }
}

