using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TecanEvo_MultilabwareNormalizationPredilution
{
    public partial class InitializeForm : Form
    {
        /// <summary>
        /// retVal: 0 - all OK
        /// retVal: 1 - all OK, load variables
        /// retVal: 2 - Errors, abort Evoware script
        /// retVal: 3
        /// retVal: 4
        /// retVal: 5
        /// 
        /// </summary>
        private int m_retVal = 2;

        private Int32 m_use_ibdbase = 1;
        private string m_mode = "Normalization";
        private Int32 m_maxTargetConc = 10000; // in ng/µl
        private Int32 m_maxTargetVol = 0; // in µl
        private Int32 m_targetConc = 0; // in ng/µl
        private Int32 m_targetVol = 0; // in µl
        private Int32 m_targetAmount = 0; // in ng
        private Int32 m_normalize = 0; // boolean: 0 or 1
        private Int32 m_normalizeConc = 1; // boolean: 0 or 1
        private Int32 m_zmax = 0; // boolean: 0 or 1
        private Int32 m_lunatic_measurement = 1; // boolean: 0 or 1
        private Int32 m_lunatic_mix = 1; // boolean: 0 or 1
        private Int32 m_lunatic_custom_program = 1; // boolean: 0 or 1
        private string m_lunatic_program = "A260 dsDNA";
        private Int32 m_measure_volume = 0; // boolean: 0 or 1
        private string m_exportVolumesScript = Properties.Settings.Default.ExportVolumeScriptPath;
        private string m_source_box = ""; //box barcode
        private string m_destination_box = ""; //box barcode
        private string m_destination_prefix = ""; //prefix from sample barcodes of new box

        private string[] m_args;

        public InitializeForm(string[] args)
        {
            m_args = args;
            InitializeComponent();
            txtExportVolumeScriptPath.Text = m_exportVolumesScript;          
        }

        private void InitializeForm_Load(object sender, EventArgs e)
        {
            if (m_args[1] == "config1")
            {
                cbSource.Items.Clear();
                cbSource.Items.Add("Eppi 2ml");
                cbSource.Items.Add("Micronic Tube 1.4ml");
                cbSource.Items.Add("Micronic Tube 0.7ml");
                cbSource.Items.Add("PCR Plate");
                cbSource.Items.Add("DeepWell Plate");
                cbSource.Items.Add("IScan Plate");

                cbDestination.Items.Clear();
                cbDestination.Items.Add("Eppi 2ml");
                cbDestination.Items.Add("Micronic Tube 1.4ml");
                cbDestination.Items.Add("Micronic Tube 0.7ml");
                cbDestination.Items.Add("PCR Plate");
                cbDestination.Items.Add("DeepWell Plate");
                cbDestination.Items.Add("IScan Plate");
                cbDestination.Items.Add("96 Well LVL SX - 300");
            } else if (m_args[1] == "config2")
            {
                cbSource.Items.Clear();
                cbSource.Items.Add("Eppi 2ml");
                cbSource.Items.Add("Matrix Tube 0.4ml");
                cbSource.Items.Add("Matrix Tube 0.7ml");
                cbSource.Items.Add("PCR Plate");
                cbSource.Items.Add("DeepWell Plate");
                cbSource.Items.Add("IScan Plate");

                cbDestination.Items.Clear();
                cbDestination.Items.Add("Eppi 2ml");
                cbDestination.Items.Add("Matrix Tube 0.4ml");
                cbDestination.Items.Add("Matrix Tube 0.7ml");
                cbDestination.Items.Add("PCR Plate");
                cbDestination.Items.Add("DeepWell Plate");
                cbDestination.Items.Add("IScan Plate");
                cbDestination.Items.Add("96 Well LVL SX - 300");
            }

            cbDestination.SelectedIndex = 0;
            cbSource.SelectedIndex = 0;
        }

        public int returnValue()
        {
            return m_retVal;
        }

        private void writeVariables(string file)
        {
            try
            {
                var fileStream = new FileStream(@file, FileMode.Create, FileAccess.ReadWrite);
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write("I,use_ibdbase," + m_use_ibdbase + "\r\n");
                    streamWriter.Write("S,mode," + m_mode + "\r\n");
                    streamWriter.Write("S,sample_source," + cbSource.Text + "\r\n");
                    streamWriter.Write("S,sample_destination," + cbDestination.Text + "\r\n");
                    streamWriter.Write("I,target_volume," + m_targetVol + "\r\n");
                    streamWriter.Write("I,target_concentration," + m_targetConc + "\r\n");
                    streamWriter.Write("I,target_amount," + m_targetAmount + "\r\n");
                    streamWriter.Write("I,normalize," + m_normalize + "\r\n");
                    streamWriter.Write("I,normalize_conc," + m_normalizeConc + "\r\n");
                    streamWriter.Write("I,aspirate_zmax," + m_zmax + "\r\n");
                    streamWriter.Write("I,measure_volume," + m_measure_volume + "\r\n");
                    streamWriter.Write("S,export_volume_script," + m_exportVolumesScript + "\r\n");
                    if (m_source_box  != "")
                    {
                        streamWriter.Write("S,source_box_bc," + m_source_box + "\r\n");
                    }
                    if (m_destination_box != "")
                    {
                        streamWriter.Write("S,destination_box_bc," + m_destination_box + "\r\n");
                        if (m_destination_prefix != "")
                        {
                            streamWriter.Write("S,destination_box_prefix," + m_destination_prefix + "\r\n");
                        }
                    }

                    if (m_mode == "Normalization")
                    {
                        if (m_normalize == 1)
                        {
                            streamWriter.Write("I,lunatic," + m_lunatic_measurement + "\r\n");
                            streamWriter.Write("I,lunatic_mix," + m_lunatic_mix + "\r\n");
                            if (m_lunatic_measurement == 1)
                            {
                                streamWriter.Write("S,lunatic_program," + m_lunatic_program + "\r\n");
                            }
                        }
                    }
                    else
                    {
                        streamWriter.Write("I,lunatic," + m_lunatic_measurement + "\r\n");
                        if (m_lunatic_measurement == 1)
                        {
                            streamWriter.Write("S,lunatic_program," + m_lunatic_program + "\r\n");
                        }
                    }

                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error writing variables file: \"" + file + "\"\r\n" + e.Message);
                throw new Exception("Error writing variables file: \"" + file + "\"\r\n" + e.Message);
            }
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            m_targetConc = int.Parse(txtConcentration.Text);
            m_targetVol = int.Parse(txtVolume.Text);

            if (m_use_ibdbase == 1)
            {
                if (cbSource.Text == "PCR Plate" || cbSource.Text == "DeepWell Plate" || cbSource.Text == "IScan Plate" || cbSource.Text == "96 Well LVL SX-300")
                {
                    if (txtSourceBoxBarcode.Text.Trim() == "")
                    {
                        MessageBox.Show("Please enter a source box barcode.");
                        txtSourceBoxBarcode.Focus();
                        return;
                    }
                    else
                    {
                        m_source_box = txtSourceBoxBarcode.Text.Trim();
                        if (CSQL.checkBoxExist(m_source_box) == false)
                        {
                            MessageBox.Show("Unknown source box barcode, or not of type '96well Plate'.");
                            txtSourceBoxBarcode.Focus();
                            return;
                        }
                        if (CSQL.checkBoxIsEmpty(m_source_box) == true)
                        {
                            MessageBox.Show("Source box is empty.");
                            txtSourceBoxBarcode.Focus();
                            return;
                        }
                    }
                }
            }

            if (m_mode == "Normalization")
            {
                if (m_use_ibdbase == 1)
                {
                    if (cbDestination.Text == "PCR Plate" || cbDestination.Text == "DeepWell Plate" || cbDestination.Text == "IScan Plate" || cbDestination.Text == "96 Well LVL SX-300")
                    {
                        if (txtDestinationBoxBarcode.Text.Trim() == "")
                        {
                            MessageBox.Show("Please enter a destination box barcode.");
                            txtDestinationBoxBarcode.Focus();
                            return;
                        }
                        else
                        {
                            m_destination_box = txtDestinationBoxBarcode.Text.Trim();
                            if (CSQL.checkBoxExist(m_destination_box) == false)
                            {
                                MessageBox.Show("Unknown destination box barcode, or not of type '96well Plate'.");
                                txtSourceBoxBarcode.Focus();
                                return;
                            }
                            if (CSQL.checkBoxIsEmpty(m_destination_box) == false)
                            {
                                MessageBox.Show("Destination box is not empty.");
                                txtSourceBoxBarcode.Focus();
                                return;
                            }
                        }
                        if (txtDestinationBoxPrefix.Text.Trim() == "")
                        {
                            MessageBox.Show("Please enter a prefix for the destination barcodes.");
                            txtDestinationBoxPrefix.Focus();
                            return;
                        }
                        else
                        {
                            m_destination_prefix = txtDestinationBoxPrefix.Text.Trim();
                            if (CSQL.checkSampleBarcodesExist(m_destination_prefix))
                            {
                                MessageBox.Show("Cannot use given prefix, barcodes already used.");
                                txtDestinationBoxPrefix.Focus();
                                return;
                            }
                        }
                    }

                    if (cbSource.Text == "PCR Plate" && cbDestination.Text == "PCR Plate")
                    {
                        if (m_destination_box == m_source_box)
                        {
                            MessageBox.Show("Cannot use the same box as source and destination");
                            txtSourceBoxBarcode.Focus();
                            return;
                        }
                    }

                    if (cbSource.Text == "DeepWell Plate" && cbDestination.Text == "DeepWell Plate")
                    {
                        if (m_destination_box == m_source_box)
                        {
                            MessageBox.Show("Cannot use the same box as source and destination");
                            txtSourceBoxBarcode.Focus();
                            return;
                        }
                    }

                    if (cbSource.Text == "IScan Plate" && cbDestination.Text == "IScan Plate")
                    {
                        if (m_destination_box == m_source_box)
                        {
                            MessageBox.Show("Cannot use the same box as source and destination");
                            txtSourceBoxBarcode.Focus();
                            return;
                        }
                    }

                    if (cbSource.Text == "96 Well LVL SX-300" && cbDestination.Text == "96 Well LVL SX-300")
                    {
                        if (m_destination_box == m_source_box)
                        {
                            MessageBox.Show("Cannot use the same box as source and destination");
                            txtSourceBoxBarcode.Focus();
                            return;
                        }
                    }
                }

                if (m_targetVol <= 0)
                {
                    if (m_normalize == 0 || (m_normalize == 1 && m_normalizeConc == 1))
                    {
                        MessageBox.Show("Please enter a volume > 0");
                        txtVolume.Focus();
                        return;
                    }
                }

                if (m_targetConc <= 0 && m_normalize == 1 && m_normalizeConc == 1)
                {
                    MessageBox.Show("Please enter a concentration > 0");
                    txtConcentration.Focus();
                    return;
                }
            }

            writeVariables(m_args[2]);
            m_retVal = 1;
            Application.Exit();
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            m_retVal = 2;
            Application.Exit();
        }

        private void txtVolume_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)'0':
                    e.KeyChar = e.KeyChar;
                    break;
                case (char)'1':
                    e.KeyChar = e.KeyChar;
                    break;
                case (char)'2':
                    e.KeyChar = e.KeyChar;
                    break;
                case (char)'3':
                    e.KeyChar = e.KeyChar;
                    break;
                case (char)'4':
                    e.KeyChar = e.KeyChar;
                    break;
                case (char)'5':
                    e.KeyChar = e.KeyChar;
                    break;
                case (char)'6':
                    e.KeyChar = e.KeyChar;
                    break;
                case (char)'7':
                    e.KeyChar = e.KeyChar;
                    break;
                case (char)'8':
                    e.KeyChar = e.KeyChar;
                    break;
                case (char)'9':
                    e.KeyChar = e.KeyChar;
                    break;
                case (char)Keys.Back:
                    e.KeyChar = e.KeyChar;
                    break;
                default:
                    e.KeyChar = (char)Keys.None;
                    break;
            }
        }

        private void txtConcentration_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)'0':
                    e.KeyChar = e.KeyChar;
                    break;
                case (char)'1':
                    e.KeyChar = e.KeyChar;
                    break;
                case (char)'2':
                    e.KeyChar = e.KeyChar;
                    break;
                case (char)'3':
                    e.KeyChar = e.KeyChar;
                    break;
                case (char)'4':
                    e.KeyChar = e.KeyChar;
                    break;
                case (char)'5':
                    e.KeyChar = e.KeyChar;
                    break;
                case (char)'6':
                    e.KeyChar = e.KeyChar;
                    break;
                case (char)'7':
                    e.KeyChar = e.KeyChar;
                    break;
                case (char)'8':
                    e.KeyChar = e.KeyChar;
                    break;
                case (char)'9':
                    e.KeyChar = e.KeyChar;
                    break;
                case (char)Keys.Back:
                    e.KeyChar = e.KeyChar;
                    break;
                default:
                    e.KeyChar = (char)Keys.None;
                    break;
            }
        }

        private void cbDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkMeasureVolume.Enabled = false;
            m_measure_volume = 0;
            chkAspirateZMax.Enabled = true;
            if (cbDestination.Text == "Micronic Tube 1.4ml")
            {
                m_maxTargetVol = 1000;
                chkMeasureVolume.Enabled = true;
            }
            else if (cbDestination.Text == "Micronic Tube 0.7ml")
            {
                m_maxTargetVol = 400;
                chkMeasureVolume.Enabled = true;
            }
            else if (cbDestination.Text == "Matrix Tube 0.7ml")
            {
                m_maxTargetVol = 700;
                chkMeasureVolume.Enabled = true;
            }
            else if (cbDestination.Text == "Matrix Tube 0.4ml")
            {
                m_maxTargetVol = 400;
                chkMeasureVolume.Enabled = true;
            }
            else if (cbDestination.Text == "Eppi 2ml")
            {
                m_maxTargetVol = 1800;
            }
            else if (cbDestination.Text == "PCR Plate")
            {
                m_maxTargetVol = 210;
                chkAspirateZMax.Enabled = false;
                chkAspirateZMax.Checked = false;
            }
            else if (cbDestination.Text == "DeepWell Plate")
            {
                m_maxTargetVol = 800;
            }
            else if (cbDestination.Text == "IScan Plate")
            {
                m_maxTargetVol = 210;
                chkAspirateZMax.Enabled = false;
                chkAspirateZMax.Checked = false;
            }
            else if (cbDestination.Text == "96 Well LVL SX-300")
            {
                m_maxTargetVol = 285;
            }


            if (cbDestination.Text == "PCR Plate" || cbDestination.Text == "DeepWell Plate" || cbDestination.Text == "IScan Plate" || cbDestination.Text == "96 Well LVL SX-300")
            {
                txtDestinationBoxBarcode.Enabled = true;
                lblDestinationBoxBarcode.Enabled = true;
                txtDestinationBoxPrefix.Enabled = true;
                lblDestinationBoxPrefix.Enabled = true;
                txtDestinationBoxBarcode.Focus();
            }
            else
            {
                txtDestinationBoxBarcode.Enabled = false;
                lblDestinationBoxBarcode.Enabled = false;
                txtDestinationBoxPrefix.Enabled = false;
                lblDestinationBoxPrefix.Enabled = false;
            }

            if (int.Parse(txtConcentration.Text) > m_maxTargetConc)
            {
                txtConcentration.Text = m_maxTargetConc.ToString();
            }
            if (int.Parse(txtVolume.Text) > m_maxTargetVol)
            {
                txtVolume.Text = m_maxTargetVol.ToString();
            }
        }

        private void cbSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSource.Text == "PCR Plate" || cbSource.Text == "DeepWell Plate" || cbSource.Text == "IScan Plate" || cbSource.Text == "96 Well LVL SX-300")
            {
                txtSourceBoxBarcode.Enabled = true;
                lblSourceBoxBarcode.Enabled = true;
                txtSourceBoxBarcode.Focus();
            }
            else
            {
                txtSourceBoxBarcode.Enabled = false;
                lblSourceBoxBarcode.Enabled = false;
            }
        }

        private void chkNormalize_CheckedChanged_1(object sender, EventArgs e)
        {
            if (chkNormalize.Checked)
            {
                rbNormalizeAmount.Enabled = true;
                rbNormalizeConc.Enabled = true;
                lblConcentration.Enabled = true;
                txtConcentration.Enabled = true;
                lblAmount.Enabled = true;
                txtAmount.Enabled = true;
                chkLunatic.Enabled = true;
                chkMix.Enabled = true;
                chkLunaticProgram.Enabled = true;
                cbLunaticProgram.Enabled = true;
                m_normalize = 1;
            }
            else
            {
                rbNormalizeAmount.Enabled = false;
                rbNormalizeConc.Enabled = false;
                lblConcentration.Enabled = false;
                txtConcentration.Enabled = false;
                lblAmount.Enabled = false;
                txtAmount.Enabled = false;
                chkLunatic.Enabled = false;
                chkMix.Enabled = false;
                chkLunaticProgram.Enabled = false;
                cbLunaticProgram.Enabled = false;
                m_normalize = 0;
            }
        }

        private void txtConcentration_TextChanged(object sender, EventArgs e)
        {
            if (rbNormalizeConc.Checked)
            {
                if (txtConcentration.Text == "")
                {
                    txtConcentration.Text = "0";
                    txtAmount.Text = "0";
                }
                else if (int.Parse(txtConcentration.Text) > m_maxTargetConc)
                {
                    txtConcentration.Text = m_maxTargetConc.ToString();
                }

                m_targetConc = int.Parse(txtConcentration.Text);
                m_targetAmount = m_targetConc * m_targetVol;
                txtAmount.Text = m_targetAmount.ToString();
            }
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            if (rbNormalizeConc.Checked)
            {
                if (txtAmount.Text == "" || m_targetVol == 0)
                {
                    txtConcentration.Text = "0";
                    txtAmount.Text = "0";
                }
                else if (int.Parse(txtAmount.Text) / m_targetVol > m_maxTargetConc)
                {
                    m_targetAmount = int.Parse(txtAmount.Text) / m_targetVol;
                    txtAmount.Text = m_targetAmount.ToString();
                }

                m_targetAmount = int.Parse(txtAmount.Text);
                if (m_targetVol > 0)
                {
                    m_targetConc = m_targetAmount / m_targetVol;
                    txtConcentration.Text = m_targetConc.ToString();
                }
            }
            else if(rbNormalizeAmount.Checked)
            {
                txtVolume.Text = "0";
                m_targetVol = 0;
                txtConcentration.Text = "0";
                m_targetAmount = int.Parse(txtAmount.Text);
            }
        }

        private void txtVolume_TextChanged(object sender, EventArgs e)
        {
            if (txtVolume.Text == "")
            {
                txtVolume.Text = "0";
            }
            else if (int.Parse(txtVolume.Text) > m_maxTargetVol)
            {
                txtVolume.Text = m_maxTargetVol.ToString();
            }
            m_targetVol = int.Parse(txtVolume.Text);
            m_targetAmount = m_targetConc * m_targetVol;
            txtAmount.Text = m_targetAmount.ToString();
        }

        private void chkLunatic_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLunatic.Checked)
            {
                m_lunatic_measurement = 1;
            }
            else 
            {
                m_lunatic_measurement = 0;
            }
            chkMix.Checked = chkLunatic.Checked;
        }

        private void chkMeasureVolume_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMeasureVolume.Checked)
            {
                m_measure_volume = 1;
                txtExportVolumeScriptPath.Enabled = true;
                lblExportVolumeScriptPath.Enabled = true;
                butExportVolumeScriptPath.Enabled = true;     
            }
            else
            {
                m_measure_volume = 0;
                txtExportVolumeScriptPath.Enabled = false;
                lblExportVolumeScriptPath.Enabled = false;
                butExportVolumeScriptPath.Enabled = false;
            }
        }

        private void butExportVolumeScriptPath_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.ExportVolumeScriptPath != "")
            {
                openFileDialog1.InitialDirectory = Properties.Settings.Default.ExportVolumeScriptPath;
            }
            
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                txtExportVolumeScriptPath.Text = openFileDialog1.FileName;
                Properties.Settings.Default.ExportVolumeScriptPath = openFileDialog1.FileName;
                Properties.Settings.Default.Save();
                m_exportVolumesScript = Properties.Settings.Default.ExportVolumeScriptPath;
            }       
        }

        private void chkAspirateZMax_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAspirateZMax.Checked) {
                m_zmax = 1;
            }
            else
            {
                m_zmax = 0;
            }        
        }

        private void chkMix_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMix.Checked)
            {
                m_lunatic_mix = 1;
            }
            else
            {
                m_lunatic_mix = 0;
            }
        }

        private void rbNormalization_CheckedChanged(object sender, EventArgs e)
        {
            pNormalization.Enabled = rbNormalization.Checked;
            if (rbNormalization.Checked)
            {
                m_mode = "Normalization";
            }
            else
            {
                m_mode = "Measurement";
            }
        }

        private void chkIBDbase_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIBDbase.Checked)
            {
                m_use_ibdbase = 1;
            } else
            {
                m_use_ibdbase = 0;
            }
        }

        private void chkLunaticProgram_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLunaticProgram.Checked)
            {
                m_lunatic_custom_program = 1;
                m_lunatic_program = cbLunaticProgram.Text.Trim();
            } else
            {
                m_lunatic_custom_program = 0;
                m_lunatic_program = "A260 dsDNA";
            }
        }

        private void cbLunaticProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkLunaticProgram.Checked)
            {
                m_lunatic_program = cbLunaticProgram.Text.Trim();
            }
        }

        private void rbNormalizeAmount_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNormalizeAmount.Checked)
            {
                txtConcentration.Text = "0";
                txtConcentration.Enabled = false;
                m_normalizeConc = 0;
            }
        }

        private void rbNormalizeConc_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNormalizeConc.Checked)
            {
                txtConcentration.Text = "0";
                txtConcentration.Enabled = true;
                m_normalizeConc = 1;
            }
        }
    }
}
