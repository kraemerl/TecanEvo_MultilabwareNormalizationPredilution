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
using System.Globalization;

namespace TecanEvo_MultilabwareNormalizationPredilution
{
    public partial class ScanReport : Form
    {
        /// <summary>
        /// retVal: 0 - all OK
        /// retVal: 1 - all OK, load variables
        /// retVal: 2 - Errors, abort Evoware script
        /// retVal: 3
        /// retVal: 4
        /// retVal: 5
        /// 
        /// scan_report C:\Lars\Git\C#_projects\TecanEvo_MultilabwareNormalizationPredilution\Data\variables.csv C:\Lars\Git\C#_projects\TecanEvo_MultilabwareNormalizationPredilution\Data\4001124008.csv C:\Lars\Git\C#_projects\TecanEvo_MultilabwareNormalizationPredilution\Data\4001088725.csv C:\Lars\Git\C#_projects\TecanEvo_MultilabwareNormalizationPredilution\Data\lastJobInfo.txt C:\Lars\Git\C#_projects\TecanEvo_MultilabwareNormalizationPredilution\Data\worklist.gwl C:\Lars\Git\C#_projects\TecanEvo_MultilabwareNormalizationPredilution\Data\LunaticTemplate.csv
        /// 
        /// </summary>
        private int m_retVal = 2;

        private Int32 m_use_ibdbase = 1;
        private string m_mode = "Normalization";
        private double m_max_diluter_volume = 400.0;
        private double m_mix_diluter_volume = 0.0;
        private string m_sampleSource = "";
        private string m_sampleDestination = "";
        private Int32 m_sourceSamples = 0;
        private Int32 m_destinationSamples = 0;
        private Int32 m_targetConc = 0; // in ng/µl
        private Int32 m_targetVol = 0; // in µl
        private Int32 m_targetAmount = 0; // in ng
        private Int32 m_normalize = 0; // boolean: 0 or 1
        private Int32 m_normalizeConc = 0; // boolean: 0 or 1
        private Int32 m_zmax = 0; // boolean: 0 or 1
        private Int32 m_lunatic_measurement = 0; // boolean: 0 or 1
        private Int32 m_lunatic_mix = 0; // boolean: 0 or 1
        private string m_lunatic_program = "A260 dsDNA";
        private Int32 m_measure_volume = 0; // boolean: 0 or 1
        private string m_source_box = "";
        private string m_destination_box = "";
        private string m_destination_prefix = "";
        private string m_exportVolumesScript = "";
        private Dictionary<int, CSample> sourceBarcodesDict = new Dictionary<int, CSample>();
        private Dictionary<int, CSample> destinationBarcodesDict = new Dictionary<int, CSample>();

        private string[] m_args;

        public ScanReport(string[] args)
        {
            m_args = args;
            InitializeComponent();
        }

        private void ScanReport_Load(object sender, EventArgs e)
        {
            try
            { 
                if (m_args.Length != 7)
                {
                    throw new Exception("Invalid argument list. Need exactly 5 parameters: \r\n" +
                                     "1. Variable file path,\r\n" +
                                     "2. Source Barcode file path,\r\n" +
                                     "3. Destination Barcode file path,\r\n" +
                                     "4. JobInfo output file path,\r\n" +
                                     "5. Worklist output file path,\r\n" +
                                     "6. Lunatic file path,\r\n");
                }
                loadVariables(m_args[1]);
                loadSourceBarcodes(m_args[2]);
                if (m_mode == "Normalization")
                {
                    loadDestinationBarcodes(m_args[3]); 
                }

                getTransferVolumes();
                writeLunaticTemplate(m_args[6], m_mode);
                writeLunaticExpDefs(m_args[6]);
                bool all_okay = showTransferOverview(m_mode);

                if (all_okay)
                {
                    butContinue.Enabled = true;
                }
            }
            catch (Exception exc)
            {
                m_retVal = 2;
                MessageBox.Show("Error initializing dialog:\r\n" + exc.Message + "\r\n\r\nClosing application ...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        public int returnValue()
        {
            return m_retVal;
        }

        private void loadVariables(string file)
        {
            try
            {
                var fileStream = new FileStream(@file, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        String[] tokenized = line.Split(',');
                        if (tokenized.Length != 3)
                        {
                            throw new Exception("Invalid entries in import variables file: \"" + file + "\" with entries:\r\n" + line);
                        }
                        else
                        {

                            if (tokenized[1] == "use_ibdbase")
                            {
                                m_use_ibdbase = int.Parse(tokenized[2].Trim());
                            }
                            else if(tokenized[1] == "mode")
                            {
                                m_mode = tokenized[2].Trim();
                                lblMode.Text = m_mode;
                                if (m_mode == "Normalization")
                                {
                                    pNormalize.Visible = true;
                                }
                                else
                                {
                                    pNormalize.Visible = false;
                                }
                            }
                            else if (tokenized[1] == "sample_source")
                            {
                                m_sampleSource = tokenized[2].Trim();
                                lblSampleSource.Text = m_sampleSource;
                            }
                            else if (tokenized[1] == "sample_destination")
                            {
                                m_sampleDestination = tokenized[2].Trim();
                                lblSampleDestination.Text = m_sampleDestination;
                            }
                            else if (tokenized[1] == "target_volume")
                            {
                                m_targetVol = int.Parse(tokenized[2].Trim());
                                lblTargetVolume.Text = m_targetVol.ToString();
                                m_mix_diluter_volume = m_targetVol / 2;
                                if (m_mix_diluter_volume > 80)
                                {
                                    m_mix_diluter_volume = 80;
                                }
                            }
                            else if (tokenized[1] == "target_concentration")
                            {
                                m_targetConc = int.Parse(tokenized[2].Trim());
                                lblTargetConcentration.Text = m_targetConc.ToString();
                            }
                            else if (tokenized[1] == "target_amount")
                            {
                                m_targetAmount = int.Parse(tokenized[2].Trim());
                                lblTargetAmount.Text = m_targetAmount.ToString();
                            }
                            else if (tokenized[1] == "source_box_bc")
                            {
                                m_source_box = tokenized[2].Trim();
                            }
                            else if (tokenized[1] == "destination_box_bc")
                            {
                                m_destination_box = tokenized[2].Trim();
                            }
                            else if (tokenized[1] == "destination_box_prefix")
                            {
                                m_destination_prefix = tokenized[2].Trim();
                            }
                            else if (tokenized[1] == "aspirate_zmax")
                            {
                                m_zmax = int.Parse(tokenized[2].Trim());
                            }
                            else if (tokenized[1] == "normalize")
                            {
                                m_normalize = int.Parse(tokenized[2].Trim());
                                if (m_normalize == 1)
                                {
                                    lblMethod.Text = "Normalize";
                                }
                                else
                                {
                                    lblMethod.Text = "Transfer";
                                    lblTargetConcentration.Enabled = false;
                                    lblTargetConcentrationText.Enabled = false;
                                }
                            }
                            else if (tokenized[1] == "normalize_conc")
                            {
                                m_normalizeConc = int.Parse(tokenized[2].Trim());
                            }
                            else if (tokenized[1] == "measure_volume")
                            {
                                m_measure_volume = int.Parse(tokenized[2].Trim());
                            }
                            else if (tokenized[1] == "export_volume_script")
                            {
                                m_exportVolumesScript = tokenized[2].Trim();
                            }
                            else if (tokenized[1] == "lunatic")
                            {
                                m_lunatic_measurement = int.Parse(tokenized[2].Trim());
                                if (m_lunatic_measurement == 1)
                                {
                                    m_targetVol = m_targetVol + 3;
                                    lblLunaticMeasurement.Text = "Yes";
                                }                            
                            }
                            else if (tokenized[1] == "lunatic_mix")
                            {
                                m_lunatic_mix = int.Parse(tokenized[2].Trim());
                            }
                            else if (tokenized[1] == "lunatic_program")
                            {
                                m_lunatic_program = tokenized[2].Trim();
                            }
                        }
                    }
                    fileStream.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error importing variables file: \"" + file + "\"\r\n" + e.Message);
                throw new Exception("Error importing variables file: \"" + file + "\"\r\n" + e.Message);
            }
        }

        private void loadSourceBarcodes(string file)
        {
            if (m_sampleSource == "Micronic Tube 1.4ml" || m_sampleSource == "Micronic Tube 0.7ml")
            {
                readMicronicScanFile(file, sourceBarcodesDict, m_sampleSource);
            }
            else if(m_sampleSource == "Matrix Tube 0.4ml" || m_sampleSource == "Matrix Tube 0.7ml")
            {
                readMicronicScanFile(file, sourceBarcodesDict, m_sampleSource);
            }
            else if (m_sampleSource == "Eppi 2ml")
            {
                readEppendorfScanFile("source", file, sourceBarcodesDict);
            }
            else if (m_sampleSource == "PCR Plate" || m_sampleSource == "DeepWell Plate" || m_sampleSource == "IScan Plate" || m_sampleSource == "96 Well LVL SX-300")
            {
                if (m_use_ibdbase == 1)
                {
                    CSQL.getBoxSamples(m_source_box, sourceBarcodesDict);
                }
            }
            else
            {
                m_retVal = 2;
                throw new Exception("Unknown sample source file: \"" + file + "\"");
            }

            if (m_use_ibdbase == 1)
            {
                CSQL.setPatients(sourceBarcodesDict);
                CSQL.addVolAndConc(sourceBarcodesDict);
            }
        }

        private void loadDestinationBarcodes(string file)
        {
            if (m_sampleDestination == "Eppi 2ml")
            {
                readEppendorfScanFile("destination", file, destinationBarcodesDict);
            }
            else if (m_sampleDestination == "Micronic Tube 1.4ml" || m_sampleDestination == "Micronic Tube 0.7ml")
            {
                readMicronicScanFile(file, destinationBarcodesDict, m_sampleDestination);
            }
            else if (m_sampleDestination == "Matrix Tube 0.4ml" || m_sampleDestination == "Matrix Tube 0.7ml")
            {
                readMicronicScanFile(file, destinationBarcodesDict, m_sampleDestination);
            }
            else if (m_sampleDestination == "PCR Plate" || m_sampleDestination == "DeepWell Plate" || m_sampleDestination == "IScan Plate" || m_sampleDestination == "96 Well LVL SX-300")
            {
                createBoxSamples(destinationBarcodesDict);
            }
            else
            {
                m_retVal = 2;
                throw new Exception("Unknown sample destination file: \"" + file + "\"");
            }

            if (m_use_ibdbase == 1)
            {
                CSQL.setPatients(destinationBarcodesDict);
            }
        }
        
        private void getTransferVolumes()
        {
            double sampleVol = 0;
            double bufferVol = 0;
            double min_sample_vol = 0;
            double DWVol = m_targetVol;
            double DWConc = 0;
            double bufferDWVol = 0;
            double sampleDWVol = 0;
            Int32 DWWell = 0;
            Int32 DWIdx = -1;
            for (int i = 1; i <= 96; i++)
            {
                if (sourceBarcodesDict.ContainsKey(i) && destinationBarcodesDict.ContainsKey(i))
                {
                    if (m_normalize == 1)
                    {
                        if (m_normalizeConc == 1)
                        {
                            //if (m_targetConc >= sourceBarcodesDict[i].getConcentration())
                            //{
                            //    destinationBarcodesDict[i].setSampleVol(m_targetVol);
                            //    destinationBarcodesDict[i].setBufferVol(0);
                            //}
                            //else
                            //{
                            //    sampleVol = Math.Round(m_targetVol / sourceBarcodesDict[i].getConcentration() * m_targetConc);
                            //    buferVol = m_targetVol - sampleVol;
                            //    destinationBarcodesDict[i].setSampleVol(sampleVol);
                            //    destinationBarcodesDict[i].setBufferVol(buferVol);
                            //}

                            min_sample_vol = Math.Round(m_targetConc * m_targetVol / sourceBarcodesDict[i].getConcentration());

                            if (m_targetConc >= sourceBarcodesDict[i].getConcentration())
                            {
                                destinationBarcodesDict[i].setSampleVol(m_targetVol);
                                destinationBarcodesDict[i].setBufferVol(0);
                                destinationBarcodesDict[i].setSourceIdx(i);
                            }
                            else if (min_sample_vol >= 5 && min_sample_vol <= m_targetVol)
                            {
                                destinationBarcodesDict[i].setSampleVol(min_sample_vol);
                                destinationBarcodesDict[i].setBufferVol(m_targetVol - min_sample_vol);
                                destinationBarcodesDict[i].setSourceIdx(i);
                            }
                            else
                            {
                                DWIdx = 100 + i;
                                DWWell += 1;
                                DWConc = m_targetConc * m_targetVol / 5;
                                DWVol = m_targetVol;

                                if (!sourceBarcodesDict.ContainsKey(DWIdx))
                                {
                                    sourceBarcodesDict[DWIdx] = new CSample("Predilution", sourceBarcodesDict[i].getBarcode(), i, DWWell, numberTo96wellRackPos(DWWell));
                                    sourceBarcodesDict[DWIdx].setPatientAndSample(sourceBarcodesDict[i].getPatient(), sourceBarcodesDict[i].getSample());
                                    sourceBarcodesDict[DWIdx].setCategory(sourceBarcodesDict[i].getCategory());
                                    sourceBarcodesDict[DWIdx].setConcentration(DWConc);
                                    sourceBarcodesDict[DWIdx].setVolume(0);
                                    DWVol += 5;
                                }

                                sampleDWVol = Math.Round(DWVol / sourceBarcodesDict[i].getConcentration() * DWConc);
                                if (sampleDWVol < 5)
                                {
                                    sampleDWVol = 5;
                                    DWVol = Math.Round(sampleDWVol * sourceBarcodesDict[i].getConcentration() / DWConc);
                                }

                                bufferDWVol = DWVol - sampleDWVol;
                                sourceBarcodesDict[DWIdx].setVolume(sourceBarcodesDict[DWIdx].getVolume() + DWVol);

                                sourceBarcodesDict[DWIdx].setDWSampleVol(sourceBarcodesDict[DWIdx].getDWSampleVol() + sampleDWVol);
                                sourceBarcodesDict[DWIdx].setDWBufferVol(sourceBarcodesDict[DWIdx].getDWBufferVol() + bufferDWVol);

                                //destinationBarcodesDict[i].setSampleBarcode(DWIdx);


                                sampleVol = Math.Round(m_targetVol / DWConc * m_targetConc);
                                bufferVol = m_targetVol - sampleVol;
                                destinationBarcodesDict[i].setSampleVol(sampleVol);
                                destinationBarcodesDict[i].setBufferVol(bufferVol);
                                destinationBarcodesDict[i].setSourceIdx(DWIdx);
                            }
                        } 
                        else
                        {
                            min_sample_vol = Math.Round(m_targetAmount / sourceBarcodesDict[i].getConcentration());

                            destinationBarcodesDict[i].setSampleVol(min_sample_vol);
                            destinationBarcodesDict[i].setBufferVol(0);
                            destinationBarcodesDict[i].setSourceIdx(i);
                        }
                    }
                    else
                    {
                        destinationBarcodesDict[i].setSampleVol(m_targetVol);
                        destinationBarcodesDict[i].setBufferVol(0);
                        destinationBarcodesDict[i].setSourceIdx(i);
                    }
                }
            }
        }

        private void getMeasurementVolumes()
        {
            for (int i = 1; i <= 96; i++)
            {
                if (sourceBarcodesDict.ContainsKey(i))
                {
                    destinationBarcodesDict[i].setSampleVol(3);
                    destinationBarcodesDict[i].setBufferVol(0);
                    destinationBarcodesDict[i].setSourceIdx(i);
                }
            }
        }

        private bool showTransferOverview(string mode)
        {
            bool all_okay = true;

            if (mode == "Normalization")
            {
                int sourceIdx = -1;
                for (int i = 1; i <= 96; i++)
                {
                    if (destinationBarcodesDict.ContainsKey(i))
                    {
                        sourceIdx = destinationBarcodesDict[i].getSourceIdx();
                        if (sourceBarcodesDict.ContainsKey(sourceIdx))
                        {
                            DataGridViewRow row = (DataGridViewRow)grdTransferOverview.Rows[0].Clone();
                            row.Cells[0].Value = sourceBarcodesDict[sourceIdx].getSource();
                            row.Cells[1].Value = sourceBarcodesDict[sourceIdx].getPatient();
                            if (sourceBarcodesDict[sourceIdx].getPatient() == -1 && m_use_ibdbase == 1)
                            {
                                row.Cells[0].Style.BackColor = Color.Red;
                                all_okay = false;
                            }
                            row.Cells[2].Value = sourceBarcodesDict[sourceIdx].getSample();
                            if (sourceBarcodesDict[sourceIdx].getSample() == -1 && m_use_ibdbase == 1)
                            {
                                row.Cells[1].Style.BackColor = Color.Red;
                                all_okay = false;
                            }
                            row.Cells[3].Value = sourceBarcodesDict[sourceIdx].getBarcode();
                            if (sourceBarcodesDict[sourceIdx].getBarcode() == "***" || sourceBarcodesDict[sourceIdx].getBarcode() == "$$$")
                            {
                                row.Cells[3].Style.BackColor = Color.Red;
                                all_okay = false;
                            }
                            row.Cells[4].Value = sourceBarcodesDict[sourceIdx].getCategory();
                            row.Cells[5].Value = sourceBarcodesDict[sourceIdx].getVolume();
                            if (sourceBarcodesDict[sourceIdx].getVolume() <= 0 && m_use_ibdbase == 1)
                            {
                                row.Cells[5].Style.BackColor = Color.Red;
                                all_okay = false;
                            }
                            row.Cells[6].Value = sourceBarcodesDict[sourceIdx].getConcentration();
                            if (sourceBarcodesDict[sourceIdx].getConcentration() <= 0 && m_use_ibdbase == 1)
                            {
                                row.Cells[6].Style.BackColor = Color.Red;
                                if (m_normalize == 1)
                                {
                                    all_okay = false;
                                }
                            }
                            else if (sourceBarcodesDict[sourceIdx].getConcentration() < m_targetConc)
                            {
                                row.Cells[6].Style.BackColor = Color.Orange;
                            }

                            row.Cells[7].Value = destinationBarcodesDict[i].getBarcode();
                            if (destinationBarcodesDict[i].getPatient() == -1)
                            {
                                if (destinationBarcodesDict[i].getBarcode() == "***" || destinationBarcodesDict[i].getBarcode() == "$$$")
                                {
                                    row.Cells[7].Style.BackColor = Color.Red;
                                    row.Cells[8].Style.BackColor = Color.Red;
                                    all_okay = false;
                                    row.Cells[8].Value = "no barcode";
                                }
                                else
                                {
                                    row.Cells[8].Value = "yes";
                                }
                            }
                            else
                            {
                                row.Cells[8].Value = "existing barcode";
                                row.Cells[8].Style.BackColor = Color.Red;
                                all_okay = false;
                            }
                            row.Cells[9].Value = destinationBarcodesDict[i].getSampleVol();
                            if (destinationBarcodesDict[i].getSampleVol() < 4)
                            {
                                row.Cells[9].Style.BackColor = Color.Red;
                                all_okay = false;
                            }
                            row.Cells[10].Value = destinationBarcodesDict[i].getBufferVol();

                            grdTransferOverview.Rows.Add(row);
                        }
                    }
                }

                m_destinationSamples = destinationBarcodesDict.Count();
                lblDestinationSamples.Text = m_destinationSamples.ToString();
            }
            else
            {
                for (int i = 1; i <= 96; i++)
                {
                    if (sourceBarcodesDict.ContainsKey(i))
                    {
                        DataGridViewRow row = (DataGridViewRow)grdTransferOverview.Rows[0].Clone();
                        row.Cells[0].Value = sourceBarcodesDict[i].getSource();
                        row.Cells[1].Value = sourceBarcodesDict[i].getPatient();
                        if (sourceBarcodesDict[i].getPatient() == -1)
                        {
                            row.Cells[0].Style.BackColor = Color.Red;
                            if (m_use_ibdbase == 1)
                            {
                                all_okay = false;
                            }
                        }
                        row.Cells[2].Value = sourceBarcodesDict[i].getSample();
                        if (sourceBarcodesDict[i].getSample() == -1)
                        {
                            row.Cells[1].Style.BackColor = Color.Red;
                            if (m_use_ibdbase == 1)
                            {
                                all_okay = false;
                            }
                        }
                        row.Cells[3].Value = sourceBarcodesDict[i].getBarcode();
                        if (sourceBarcodesDict[i].getBarcode() == "***" || sourceBarcodesDict[i].getBarcode() == "$$$")
                        {
                            row.Cells[3].Style.BackColor = Color.Red;
                            all_okay = false;
                        }
                        row.Cells[4].Value = sourceBarcodesDict[i].getCategory();
                        row.Cells[5].Value = sourceBarcodesDict[i].getVolume();
                        if (sourceBarcodesDict[i].getVolume() <= 0)
                        {
                            row.Cells[5].Style.BackColor = Color.Red;
                        }
                        row.Cells[6].Value = sourceBarcodesDict[i].getConcentration();
                        if (sourceBarcodesDict[i].getConcentration() <= 0)
                        {
                            row.Cells[6].Style.BackColor = Color.Red;
                        }

                        row.Cells[7].Style.BackColor = Color.Gray;
                        row.Cells[8].Style.BackColor = Color.Gray;

                        row.Cells[9].Value = 3;
                        row.Cells[10].Value = 0;

                        grdTransferOverview.Rows.Add(row);
                    }
                }
            }

            m_sourceSamples = sourceBarcodesDict.Count();
            lblSourceSamples.Text = m_sourceSamples.ToString();

            grdTransferOverview.AllowUserToAddRows = false;

            return all_okay;
        }

        private void readEppendorfScanFile(string mode, string file, Dictionary<int, CSample> tubeBarcodes)
        {
            try
            {
                Dictionary<String, String> duplicateEppendorfBarcodes = new Dictionary<string, string>();
                Dictionary<string, string> origEntry = new Dictionary<string, string>();

                int idx_mod = 0;
                var fileStream = new FileStream(@file, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream))
                {
                    string line;
                    if ((line = streamReader.ReadLine()) != null)
                    {
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            String[] tokenized = line.Split(';');
                            for (int i = 0; i < tokenized.Length; i++)
                                tokenized[i] = tokenized[i].Trim(' ');
                            if (tokenized.Length == 7)
                            {
                                string barcode = tokenized[6];
                                if (barcode.CompareTo("***") == 0)
                                    continue;
                                if (mode == "destination")
                                {
                                    idx_mod = 12;
                                }
                                int rack = Convert.ToInt32(tokenized[0]);
                                int well = Convert.ToInt32(tokenized[2]);
                                int idx = (rack - idx_mod - 1) * 16 + well;

                                if (duplicateEppendorfBarcodes.Keys.Contains(barcode))
                                    throw new Exception("Duplicate barcode in scan file.\r\nOld entry: " + duplicateEppendorfBarcodes[barcode] + "\r\nNew entry: " + line);
                                else
                                    duplicateEppendorfBarcodes.Add(barcode, line);
                                tubeBarcodes[idx] = new CSample("Eppi 2ml",barcode, rack, well, "", idx_mod);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                m_retVal = 2;
                throw new Exception("Error importing eppi barcode file: \"" + file + "\"\r\n" + e.Message);
            }
        }

        private void readMicronicScanFile(string file, Dictionary<int, CSample> tubeBarcodes, string source)
        {
            try
            {
                var fileStream = new FileStream(@file, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    int idx = -1;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        String[] tokenized = line.Split(',');
                        if (tokenized.Length > 1)
                        {
                            string barcode = tokenized[1];
                            if (barcode.CompareTo("***") == 0)
                                continue;
                            idx = RackPosTo96wellNumber(tokenized[0]);
                            tubeBarcodes[idx] = new CSample(source, tokenized[1], -1, idx, tokenized[0]);
                        }  
                    }
                }
            }
            catch (Exception e)
            {
                m_retVal = 2;
                throw new Exception("Error importing micronic barcode file: \"" + file + "\"\r\n" + e.Message);
            }
        }

        private void createBoxSamples(Dictionary<int, CSample> tubeBarcodes)
        {
            for (int i = 1; i <= 96; i++)
            {
                tubeBarcodes[i] = new CSample("plate_well",m_destination_prefix + "-" + numberTo96wellRackPos(i), -1, i, numberTo96wellRackPos(i));
            }
        }

        private void writeJobFile(string file)
        {
            try
            {
                var fileStream = new FileStream(@file, FileMode.Create, FileAccess.ReadWrite);
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write("source\tpatient_id\tsample_id\tsource BC\tcategory\tvolume (µl)\tconcentration (ng/µl)\tdestination BC\tis useable\tsample (µl)\tTE (µl)\t\r\n");
                    foreach (DataGridViewRow row in grdTransferOverview.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            streamWriter.Write(cell.Value + "\t");
                        }
                        streamWriter.Write("\r\n");
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error writing variables file: \"" + file + "\"\r\n" + e.Message);
                throw new Exception("Error writing variables file: \"" + file + "\"\r\n" + e.Message);
            }
        }

        private void writeWorklistFile(string file, string mode)
        {
            string worklistFileLunatic = file.Replace(".gwl", "_DS.gwl");
            string variableExportCommands = "";
            string liquidDetectionCommands = "";

            string sourceLabwareName = "";
            string sourceLabwareType = "";

            string liquidClassName = "DNA Transfer Job";
            if (m_zmax == 1)
            {
                liquidClassName = "DNA Transfer Job zMax";
            }

            if (mode == "Normalization")
            {
                string destinationLabwareName = getLabwareName("destination", m_sampleDestination);
                string destinationLabwareType = getLabwareType(m_sampleDestination);

                try
                {
                    var fileStream = new FileStream(@file, FileMode.Create, FileAccess.ReadWrite);
                    using (var streamWriter = new StreamWriter(fileStream))
                    {
                        DateTime localDate = DateTime.Now;
                        int tip = 0;
                        int sourceIdx = -1;
                        double volume = 0.0;
                        double mixVolume = 0.0;
                        double maxAspirate = m_max_diluter_volume;
                        bool bExportVolume = false;
                        bool decon_wash = false;

                        streamWriter.WriteLine("C; This GWL was generated by TecanEvo_MultilabwareNormalization at " + localDate);
                        streamWriter.WriteLine("B;");
                        streamWriter.WriteLine("C; Pipetting scheme for job type: " + lblMethod.Text);
                        streamWriter.WriteLine("B;");
                        streamWriter.WriteLine("C; DNA Source: " + m_sampleSource + ", DNA Destination: " + m_sampleDestination);
                        streamWriter.WriteLine("B;");

                        if (m_normalize == 1)
                        {
                            streamWriter.WriteLine("C; pipetting DW Buffer");
                            streamWriter.WriteLine("B;");
                            for (int i = 1; i <= 96; i++)
                            {
                                if (destinationBarcodesDict.ContainsKey(i))
                                {
                                    sourceIdx = destinationBarcodesDict[i].getSourceIdx();

                                    if (sourceBarcodesDict.ContainsKey(sourceIdx))
                                    {
                                        if (sourceBarcodesDict[sourceIdx].getSource() == "Predilution")
                                        {
                                            sourceLabwareName = getLabwareName("source", sourceBarcodesDict[sourceIdx].getSource());
                                            sourceLabwareType = getLabwareType(sourceBarcodesDict[sourceIdx].getSource());
                                            volume = sourceBarcodesDict[sourceIdx].getDWBufferVol();

                                            if (volume > 0)
                                            {
                                                if (volume <= maxAspirate)
                                                {
                                                    streamWriter.WriteLine("A;Buffer;;Trough 100ml;" + (tip + 1) + ";;" + volume.ToString("R", CultureInfo.InvariantCulture) + ";DNA Transfer Job Buffer;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                    streamWriter.WriteLine("D;" + sourceLabwareName + ";;" + sourceLabwareType + ";" + sourceBarcodesDict[sourceIdx].getRackPos() + ";;" + volume.ToString("R", CultureInfo.InvariantCulture) + ";DNA Transfer Job;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                    streamWriter.WriteLine("W3;");
                                                }
                                                else
                                                {
                                                    double readwriteVolume = volume;
                                                    double actPipetVolume = maxAspirate;

                                                    while (readwriteVolume > 0)
                                                    {
                                                        streamWriter.WriteLine("A;Buffer;;Trough 100ml;" + (tip + 1) + ";;" + actPipetVolume.ToString("R", CultureInfo.InvariantCulture) + ";DNA Transfer Job Buffer;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                        streamWriter.WriteLine("D;" + sourceLabwareName + ";;" + sourceLabwareType + ";" + sourceBarcodesDict[sourceIdx].getRackPos() + ";;" + actPipetVolume.ToString("R", CultureInfo.InvariantCulture) + ";DNA Transfer Job;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                        readwriteVolume -= actPipetVolume;
                                                        if (readwriteVolume > maxAspirate)
                                                            actPipetVolume = maxAspirate;
                                                        else
                                                            actPipetVolume = readwriteVolume;
                                                    }
                                                    streamWriter.WriteLine("W3;");
                                                }
                                            }
                                            if (++tip == 8)
                                            {
                                                tip = 0;
                                                streamWriter.WriteLine("B;");
                                            }
                                        }
                                    }
                                }
                            }

                            tip = 0;
                            streamWriter.WriteLine("B;");
                            for (int tipWash = 0; tipWash < 8; tipWash++)
                            {
                                streamWriter.WriteLine("W3;");
                            }
                            streamWriter.WriteLine("B;");

                            streamWriter.WriteLine("C; pipetting Buffer");
                            streamWriter.WriteLine("B;");
                            for (int i = 1; i <= 96; i++)
                            {
                                if (destinationBarcodesDict.ContainsKey(i))
                                {
                                    sourceIdx = destinationBarcodesDict[i].getSourceIdx();

                                    if (sourceBarcodesDict.ContainsKey(sourceIdx))
                                    {
                                        sourceLabwareName = getLabwareName("source", sourceBarcodesDict[sourceIdx].getSource());
                                        sourceLabwareType = getLabwareType(sourceBarcodesDict[sourceIdx].getSource());
                                        volume = destinationBarcodesDict[i].getBufferVol();

                                        if (volume > 0)
                                        {
                                            if (volume <= maxAspirate)
                                            {
                                                streamWriter.WriteLine("A;Buffer;;Trough 100ml;" + (tip + 1) + ";;" + volume.ToString("R", CultureInfo.InvariantCulture) + ";DNA Transfer Job Buffer;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                if (destinationLabwareName == "Eppi")
                                                {
                                                    streamWriter.WriteLine("D;" + destinationBarcodesDict[i].getEppieRack() + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;" + volume.ToString("R", CultureInfo.InvariantCulture) + ";DNA Transfer Job;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                }
                                                else
                                                {
                                                    streamWriter.WriteLine("D;" + destinationLabwareName + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;" + volume.ToString("R", CultureInfo.InvariantCulture) + ";DNA Transfer Job;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                }
                                                streamWriter.WriteLine("W3;");
                                            }
                                            else
                                            {
                                                double readwriteVolume = volume;
                                                double actPipetVolume = maxAspirate;

                                                while (readwriteVolume > 0)
                                                {
                                                    streamWriter.WriteLine("A;Buffer;;Trough 100ml;" + (tip + 1) + ";;" + actPipetVolume.ToString("R", CultureInfo.InvariantCulture) + ";DNA Transfer Job Buffer;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                    if (destinationLabwareName == "Eppi")
                                                    {
                                                        streamWriter.WriteLine("D;" + destinationBarcodesDict[i].getEppieRack() + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;" + actPipetVolume.ToString("R", CultureInfo.InvariantCulture) + ";DNA Transfer Job;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                    }
                                                    else
                                                    {
                                                        streamWriter.WriteLine("D;" + destinationLabwareName + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;" + actPipetVolume.ToString("R", CultureInfo.InvariantCulture) + ";DNA Transfer Job;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                    }
                                                    readwriteVolume -= actPipetVolume;
                                                    if (readwriteVolume > maxAspirate)
                                                        actPipetVolume = maxAspirate;
                                                    else
                                                        actPipetVolume = readwriteVolume;
                                                }
                                                streamWriter.WriteLine("W3;");
                                            }
                                        }
                                        if (++tip == 8)
                                        {
                                            tip = 0;
                                            //streamWriter.WriteLine("B;");
                                        }
                                    }
                                }
                            }
                            //for (int tipWash = 0; tipWash < 8; tipWash++)
                            //{
                            //    streamWriter.WriteLine("W3;");
                            //}
                            streamWriter.WriteLine("B;");
                        }

                        tip = 0;
                        streamWriter.WriteLine("B;");
                        for (int tipWash = 0; tipWash < 8; tipWash++)
                        {
                            streamWriter.WriteLine("A;NaClO;;Trough 100ml;" + (tipWash + 1) + ";;" + maxAspirate.ToString("R", CultureInfo.InvariantCulture) + ";Decon Mix In Trough Detect;;" + Convert.ToInt16(Math.Pow(2, tipWash)) + ";;;");
                            streamWriter.WriteLine("D;NaClO;;Trough 100ml;" + (tipWash + 1) + ";;" + maxAspirate.ToString("R", CultureInfo.InvariantCulture) + ";Decon Mix In Trough Detect;;" + Convert.ToInt16(Math.Pow(2, tipWash)) + ";;;");
                            streamWriter.WriteLine("W2;");
                        }
                        streamWriter.WriteLine("B;");

                        streamWriter.WriteLine("C; pipetting DW DNA");
                        streamWriter.WriteLine("B;");
                        for (int i = 1; i <= 96; i++)
                        {
                            if (destinationBarcodesDict.ContainsKey(i))
                            {
                                sourceIdx = destinationBarcodesDict[i].getSourceIdx();

                                if (sourceBarcodesDict.ContainsKey(sourceIdx))
                                {
                                    if (sourceBarcodesDict[sourceIdx].getSource() == "Predilution")
                                    {
                                        sourceLabwareName = getLabwareName("source", sourceBarcodesDict[sourceIdx].getSource());
                                        sourceLabwareType = getLabwareType(sourceBarcodesDict[sourceIdx].getSource());
                                        volume = sourceBarcodesDict[sourceIdx].getDWSampleVol();
                                        decon_wash = true;

                                        if (volume <= maxAspirate)
                                        {
                                            if (getLabwareName("source", sourceBarcodesDict[sourceBarcodesDict[sourceIdx].getEppieRack()].getSource()) == "Eppi")
                                            {
                                                streamWriter.WriteLine("A;" + sourceBarcodesDict[sourceBarcodesDict[sourceIdx].getEppieRack()].getEppieRack() + ";;" + getLabwareType(sourceBarcodesDict[sourceBarcodesDict[sourceIdx].getEppieRack()].getSource()) + ";" + sourceBarcodesDict[sourceBarcodesDict[sourceIdx].getEppieRack()].getRackPos() + ";;" + volume.ToString("R", CultureInfo.InvariantCulture) + ";" + liquidClassName + ";;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                streamWriter.WriteLine("D;" + sourceLabwareName + ";;" + sourceLabwareType + ";" + sourceBarcodesDict[sourceIdx].getRackPos() + ";;" + volume.ToString("R", CultureInfo.InvariantCulture) + ";" + liquidClassName + ";;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                            }
                                            else
                                            {
                                                streamWriter.WriteLine("A;" + getLabwareName("source", sourceBarcodesDict[sourceBarcodesDict[sourceIdx].getEppieRack()].getSource()) + ";;" + getLabwareType(sourceBarcodesDict[sourceBarcodesDict[sourceIdx].getEppieRack()].getSource()) + ";" + sourceBarcodesDict[sourceBarcodesDict[sourceIdx].getEppieRack()].getRackPos() + ";;" + volume.ToString("R", CultureInfo.InvariantCulture) + ";" + liquidClassName + ";;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                streamWriter.WriteLine("D;" + sourceLabwareName + ";;" + sourceLabwareType + ";" + sourceBarcodesDict[sourceIdx].getRackPos() + ";;" + volume.ToString("R", CultureInfo.InvariantCulture) + ";" + liquidClassName + ";;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                            }
                                        }
                                        else
                                        {
                                            double readwriteVolume = volume;
                                            double actPipetVolume = maxAspirate;

                                            while (readwriteVolume > 0)
                                            {
                                                if (getLabwareName("source", sourceBarcodesDict[sourceBarcodesDict[sourceIdx].getEppieRack()].getSource()) == "Eppi")
                                                {
                                                    streamWriter.WriteLine("A;" + sourceBarcodesDict[sourceBarcodesDict[sourceIdx].getEppieRack()].getEppieRack() + ";;" + getLabwareType(sourceBarcodesDict[sourceBarcodesDict[sourceIdx].getEppieRack()].getSource()) + ";" + sourceBarcodesDict[sourceBarcodesDict[sourceIdx].getEppieRack()].getRackPos() + ";;" + actPipetVolume.ToString("R", CultureInfo.InvariantCulture) + ";" + liquidClassName + ";;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                    streamWriter.WriteLine("D;" + sourceLabwareName + ";;" + sourceLabwareType + ";" + sourceBarcodesDict[sourceIdx].getEppiRackPos() + ";;" + actPipetVolume.ToString("R", CultureInfo.InvariantCulture) + ";" + liquidClassName + ";;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                }
                                                else
                                                {
                                                    streamWriter.WriteLine("A;" + getLabwareName("source", sourceBarcodesDict[sourceBarcodesDict[sourceIdx].getEppieRack()].getSource()) + ";;" + getLabwareType(sourceBarcodesDict[sourceBarcodesDict[sourceIdx].getEppieRack()].getSource()) + ";" + sourceBarcodesDict[sourceBarcodesDict[sourceIdx].getEppieRack()].getRackPos() + ";;" + actPipetVolume.ToString("R", CultureInfo.InvariantCulture) + ";" + liquidClassName + ";;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                    streamWriter.WriteLine("D;" + sourceLabwareName + ";;" + sourceLabwareType + ";" + sourceBarcodesDict[sourceIdx].getRackPos() + ";;" + actPipetVolume.ToString("R", CultureInfo.InvariantCulture) + ";" + liquidClassName + ";;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                }
                                                readwriteVolume -= actPipetVolume;
                                                if (readwriteVolume > maxAspirate)
                                                    actPipetVolume = maxAspirate;
                                                else
                                                    actPipetVolume = readwriteVolume;
                                            }
                                        }
                                        // Mix
                                        mixVolume = sourceBarcodesDict[sourceIdx].getVolume();
                                        if (mixVolume > m_max_diluter_volume)
                                        {
                                            mixVolume = m_max_diluter_volume;
                                        }
                                        streamWriter.WriteLine("A;" + sourceLabwareName + ";;" + sourceLabwareType + ";" + sourceBarcodesDict[sourceIdx].getRackPos() + ";;" + mixVolume + ";DNA Transfer Job;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                        streamWriter.WriteLine("D;" + sourceLabwareName + ";;" + sourceLabwareType + ";" + sourceBarcodesDict[sourceIdx].getRackPos() + ";;" + mixVolume + ";DNA Transfer Job;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                        streamWriter.WriteLine("W;");

                                        if (++tip == 8)
                                        {
                                            tip = 0;
                                            streamWriter.WriteLine("B;");
                                            for (int tipWash = 0; tipWash < 8; tipWash++)
                                            {
                                                streamWriter.WriteLine("A;NaClO;;Trough 100ml;" + (tipWash + 1) + ";;" + maxAspirate.ToString("R", CultureInfo.InvariantCulture) + ";Decon Mix In Trough Detect;;" + Convert.ToInt16(Math.Pow(2, tipWash)) + ";;;");
                                                streamWriter.WriteLine("D;NaClO;;Trough 100ml;" + (tipWash + 1) + ";;" + maxAspirate.ToString("R", CultureInfo.InvariantCulture) + ";Decon Mix In Trough Detect;;" + Convert.ToInt16(Math.Pow(2, tipWash)) + ";;;");
                                                streamWriter.WriteLine("W2;");
                                            }
                                            streamWriter.WriteLine("B;");
                                            decon_wash = false;
                                        }
                                    }
                                }
                            }
                        }

                        tip = 0;
                        streamWriter.WriteLine("B;");
                        if (decon_wash)
                        {
                            for (int tipWash = 0; tipWash < 8; tipWash++)
                            {
                                streamWriter.WriteLine("A;NaClO;;Trough 100ml;" + (tipWash + 1) + ";;" + maxAspirate.ToString("R", CultureInfo.InvariantCulture) + ";Decon Mix In Trough Detect;;" + Convert.ToInt16(Math.Pow(2, tipWash)) + ";;;");
                                streamWriter.WriteLine("D;NaClO;;Trough 100ml;" + (tipWash + 1) + ";;" + maxAspirate.ToString("R", CultureInfo.InvariantCulture) + ";Decon Mix In Trough Detect;;" + Convert.ToInt16(Math.Pow(2, tipWash)) + ";;;");
                                streamWriter.WriteLine("W2;");
                            }
                            streamWriter.WriteLine("B;");
                        }
                        streamWriter.WriteLine("C; pipetting DNA");
                        streamWriter.WriteLine("B;");

                        for (int i = 1; i <= 96; i++)
                        {
                            if (destinationBarcodesDict.ContainsKey(i))
                            {
                                sourceIdx = destinationBarcodesDict[i].getSourceIdx();

                                if (sourceBarcodesDict.ContainsKey(sourceIdx))
                                {
                                    sourceLabwareName = getLabwareName("source", sourceBarcodesDict[sourceIdx].getSource());
                                    sourceLabwareType = getLabwareType(sourceBarcodesDict[sourceIdx].getSource());
                                    volume = destinationBarcodesDict[i].getSampleVol();
                                    decon_wash = true;

                                    if (volume <= maxAspirate)
                                    {
                                        if (sourceLabwareName == "Eppi")
                                        {
                                            streamWriter.WriteLine("A;" + sourceBarcodesDict[sourceIdx].getEppieRack() + ";;" + sourceLabwareType + ";" + sourceBarcodesDict[sourceIdx].getRackPos() + ";;" + volume.ToString("R", CultureInfo.InvariantCulture) + ";" + liquidClassName + ";;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                            if (destinationLabwareName == "Eppi")
                                            {
                                                streamWriter.WriteLine("D;" + destinationBarcodesDict[i].getEppieRack() + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;" + volume.ToString("R", CultureInfo.InvariantCulture) + ";" + liquidClassName + ";;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                            }
                                            else
                                            {
                                                streamWriter.WriteLine("D;" + destinationLabwareName + ";;" + destinationLabwareType + ";" + sourceBarcodesDict[sourceIdx].getEppiRackPos() + ";;" + volume.ToString("R", CultureInfo.InvariantCulture) + ";" + liquidClassName + ";;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                            }
                                        }
                                        else
                                        {
                                            streamWriter.WriteLine("A;" + sourceLabwareName + ";;" + sourceLabwareType + ";" + sourceBarcodesDict[sourceIdx].getRackPos() + ";;" + volume.ToString("R", CultureInfo.InvariantCulture) + ";" + liquidClassName + ";;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                            if (destinationLabwareName == "Eppi")
                                            {
                                                streamWriter.WriteLine("D;" + destinationBarcodesDict[i].getEppieRack() + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;" + volume.ToString("R", CultureInfo.InvariantCulture) + ";" + liquidClassName + ";;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                            }
                                            else
                                            {
                                                streamWriter.WriteLine("D;" + destinationLabwareName + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;" + volume.ToString("R", CultureInfo.InvariantCulture) + ";" + liquidClassName + ";;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        double readwriteVolume = volume;
                                        double actPipetVolume = maxAspirate;

                                        while (readwriteVolume > 0)
                                        {
                                            if (sourceLabwareName == "Eppi")
                                            {
                                                streamWriter.WriteLine("A;" + sourceBarcodesDict[sourceIdx].getEppieRack() + ";;" + sourceLabwareType + ";" + sourceBarcodesDict[sourceIdx].getRackPos() + ";;" + actPipetVolume.ToString("R", CultureInfo.InvariantCulture) + ";" + liquidClassName + ";;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                if (destinationLabwareName == "Eppi")
                                                {
                                                    streamWriter.WriteLine("D;" + destinationBarcodesDict[i].getEppieRack() + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;" + actPipetVolume.ToString("R", CultureInfo.InvariantCulture) + ";" + liquidClassName + ";;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                }
                                                else
                                                {
                                                    streamWriter.WriteLine("D;" + destinationLabwareName + ";;" + destinationLabwareType + ";" + sourceBarcodesDict[sourceIdx].getEppiRackPos() + ";;" + actPipetVolume.ToString("R", CultureInfo.InvariantCulture) + ";" + liquidClassName + ";;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                }
                                            }
                                            else
                                            {
                                                streamWriter.WriteLine("A;" + sourceLabwareName + ";;" + sourceLabwareType + ";" + sourceBarcodesDict[sourceIdx].getRackPos() + ";;" + actPipetVolume.ToString("R", CultureInfo.InvariantCulture) + ";" + liquidClassName + ";;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                if (destinationLabwareName == "Eppi")
                                                {
                                                    streamWriter.WriteLine("D;" + destinationBarcodesDict[i].getEppieRack() + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;" + actPipetVolume.ToString("R", CultureInfo.InvariantCulture) + ";" + liquidClassName + ";;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                }
                                                else
                                                {
                                                    streamWriter.WriteLine("D;" + destinationLabwareName + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;" + actPipetVolume.ToString("R", CultureInfo.InvariantCulture) + ";" + liquidClassName + ";;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                }
                                            }
                                            readwriteVolume -= actPipetVolume;
                                            if (readwriteVolume > maxAspirate)
                                                actPipetVolume = maxAspirate;
                                            else
                                                actPipetVolume = readwriteVolume;
                                        }
                                    }
                                    if (m_measure_volume == 1 && destinationLabwareType.StartsWith("Micronic"))
                                    {
                                        //Measure Volume
                                        liquidDetectionCommands += ("A;" + destinationLabwareName + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;5;DNA Transfer Job;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;\r\n");
                                        liquidDetectionCommands += ("D;" + destinationLabwareName + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;5;DNA Transfer Job Detect Volume;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;\r\n");
                                        liquidDetectionCommands += ("W;\r\n");

                                        variableExportCommands += ("B;Variable(MicronicVolumeBarcode" + (tip + 1) + ",\"" + destinationBarcodesDict[i].getBarcode() + "\",0,,0,,,1,2,0,0);\r\n");

                                        //streamWriter.WriteLine("A;" + destinationLabwareName + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;5;DNA Transfer Job;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                        //streamWriter.WriteLine("D;" + destinationLabwareName + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;5;DNA Transfer Job Detect Volume;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                        //streamWriter.WriteLine("B;Variable(MicronicVolumeBarcode" + (tip + 1) + ",\"" + destinationBarcodesDict[i].getBarcode() + "\",0,,0,,,1,2,0,0);");
                                        bExportVolume = true;
                                    }
                                    if (m_lunatic_measurement == 1)
                                    {
                                        //Mixing
                                        if (m_lunatic_mix == 1)
                                        {
                                            if (destinationLabwareName == "Eppi")
                                            {
                                                streamWriter.WriteLine("A;" + destinationBarcodesDict[i].getEppieRack() + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;" + (m_mix_diluter_volume).ToString("R", CultureInfo.InvariantCulture) + ";DNA Transfer Job;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                streamWriter.WriteLine("D;" + destinationBarcodesDict[i].getEppieRack() + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;" + (m_mix_diluter_volume).ToString("R", CultureInfo.InvariantCulture) + ";DNA Transfer Job;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                streamWriter.WriteLine("A;" + destinationBarcodesDict[i].getEppieRack() + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;" + (m_mix_diluter_volume).ToString("R", CultureInfo.InvariantCulture) + ";DNA Transfer Job;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                streamWriter.WriteLine("D;" + destinationBarcodesDict[i].getEppieRack() + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;" + (m_mix_diluter_volume).ToString("R", CultureInfo.InvariantCulture) + ";DNA Transfer Job;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                            }
                                            else
                                            {
                                                streamWriter.WriteLine("A;" + destinationLabwareName + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;" + (m_mix_diluter_volume).ToString("R", CultureInfo.InvariantCulture) + ";DNA Transfer Job;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                streamWriter.WriteLine("D;" + destinationLabwareName + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;" + (m_mix_diluter_volume).ToString("R", CultureInfo.InvariantCulture) + ";DNA Transfer Job;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                streamWriter.WriteLine("A;" + destinationLabwareName + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;" + (m_mix_diluter_volume).ToString("R", CultureInfo.InvariantCulture) + ";DNA Transfer Job;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                                streamWriter.WriteLine("D;" + destinationLabwareName + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;" + (m_mix_diluter_volume).ToString("R", CultureInfo.InvariantCulture) + ";DNA Transfer Job;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                            }
                                        }
                                        //Pipette LunaticPlate
                                        if (destinationLabwareName == "Eppi")
                                        {
                                            streamWriter.WriteLine("A;" + destinationBarcodesDict[i].getEppieRack() + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;" + (3.0).ToString("R", CultureInfo.InvariantCulture) + ";DropSense;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                            streamWriter.WriteLine("D;NSense1;;;" + destinationBarcodesDict[i].getEppiRackPos() + ";;" + (2.0).ToString("R", CultureInfo.InvariantCulture) + ";DropSense;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                        }
                                        else
                                        {
                                            streamWriter.WriteLine("A;" + destinationLabwareName + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;" + (3.0).ToString("R", CultureInfo.InvariantCulture) + ";DropSense;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                            streamWriter.WriteLine("D;NSense1;;;" + destinationBarcodesDict[i].getRackPos() + ";;" + (2.0).ToString("R", CultureInfo.InvariantCulture) + ";DropSense;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                        }
                                    }

                                    if (m_measure_volume == 1 && destinationLabwareType.StartsWith("Micronic"))
                                    {
                                        streamWriter.WriteLine("W3;");
                                    }
                                    else
                                    {
                                        streamWriter.WriteLine("W;");
                                    }

                                    if (++tip == 8)
                                    {
                                        tip = 0;
                                        streamWriter.WriteLine("B;");
                                        if (bExportVolume)
                                        {
                                            streamWriter.Write(liquidDetectionCommands);
                                            streamWriter.Write(variableExportCommands);
                                            streamWriter.WriteLine("B;Execute_VBscript(\"" + m_exportVolumesScript + "\",0);");
                                            bExportVolume = false;
                                            variableExportCommands = "";
                                            liquidDetectionCommands = "";
                                        }
                                        for (int tipWash = 0; tipWash < 8; tipWash++)
                                        {
                                            streamWriter.WriteLine("A;NaClO;;Trough 100ml;" + (tipWash + 1) + ";;" + maxAspirate.ToString("R", CultureInfo.InvariantCulture) + ";Decon Mix In Trough Detect;;" + Convert.ToInt16(Math.Pow(2, tipWash)) + ";;;");
                                            streamWriter.WriteLine("D;NaClO;;Trough 100ml;" + (tipWash + 1) + ";;" + maxAspirate.ToString("R", CultureInfo.InvariantCulture) + ";Decon Mix In Trough Detect;;" + Convert.ToInt16(Math.Pow(2, tipWash)) + ";;;");
                                            streamWriter.WriteLine("W2;");
                                        }
                                        streamWriter.WriteLine("B;");
                                        decon_wash = false;
                                    }
                                }
                            }
                        }
                        streamWriter.WriteLine("C; pipetting done - wash tips");
                        tip = 0;
                        streamWriter.WriteLine("B;");
                        if (bExportVolume)
                        {
                            streamWriter.Write(liquidDetectionCommands);
                            streamWriter.Write(variableExportCommands);
                            streamWriter.WriteLine("B;Execute_VBscript(\"" + m_exportVolumesScript + "\",0);");
                            bExportVolume = false;
                            variableExportCommands = "";
                            liquidDetectionCommands = "";
                        }
                        if (decon_wash)
                        {
                            for (int tipWash = 0; tipWash < 8; tipWash++)
                            {
                                streamWriter.WriteLine("A;NaClO;;Trough 100ml;" + (tipWash + 1) + ";;" + maxAspirate.ToString("R", CultureInfo.InvariantCulture) + ";Decon Mix In Trough Detect;;" + Convert.ToInt16(Math.Pow(2, tipWash)) + ";;;");
                                streamWriter.WriteLine("D;NaClO;;Trough 100ml;" + (tipWash + 1) + ";;" + maxAspirate.ToString("R", CultureInfo.InvariantCulture) + ";Decon Mix In Trough Detect;;" + Convert.ToInt16(Math.Pow(2, tipWash)) + ";;;");
                                streamWriter.WriteLine("W2;");
                            }
                            streamWriter.WriteLine("B;");
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error writing worklist file: \"" + file + "\"\r\n" + e.Message);
                    throw new Exception("Error writing worklist file: \"" + file + "\"\r\n" + e.Message);
                }
            }
            else
            {
                try
                {
                    var fileStream = new FileStream(@file, FileMode.Create, FileAccess.ReadWrite);
                    using (var streamWriter = new StreamWriter(fileStream))
                    {
                        DateTime localDate = DateTime.Now;
                        int tip = 0;
                        double maxAspirate = m_max_diluter_volume;
                        bool decon_wash = false;

                        streamWriter.WriteLine("C; This GWL was generated by TecanEvo_MultilabwareNormalization at " + localDate);
                        streamWriter.WriteLine("B;");
                        streamWriter.WriteLine("C; Pipetting scheme for job type: Measurement");
                        streamWriter.WriteLine("B;");
                        streamWriter.WriteLine("C; DNA Source: " + m_sampleSource);
                        streamWriter.WriteLine("B;");
                        
                        tip = 0;
                        streamWriter.WriteLine("B;");
                        for (int tipWash = 0; tipWash < 8; tipWash++)
                        {
                            streamWriter.WriteLine("A;NaClO;;Trough 100ml;" + (tipWash + 1) + ";;" + maxAspirate.ToString("R", CultureInfo.InvariantCulture) + ";Decon Mix In Trough Detect;;" + Convert.ToInt16(Math.Pow(2, tipWash)) + ";;;");
                            streamWriter.WriteLine("D;NaClO;;Trough 100ml;" + (tipWash + 1) + ";;" + maxAspirate.ToString("R", CultureInfo.InvariantCulture) + ";Decon Mix In Trough Detect;;" + Convert.ToInt16(Math.Pow(2, tipWash)) + ";;;");
                            streamWriter.WriteLine("W2;");
                        }
                        streamWriter.WriteLine("B;");
                        streamWriter.WriteLine("C; pipetting DNA");

                        for (int i = 1; i <= 96; i++)
                        {
                            if (sourceBarcodesDict.ContainsKey(i))
                            {
                                sourceLabwareName = getLabwareName("source", sourceBarcodesDict[i].getSource());
                                sourceLabwareType = getLabwareType(sourceBarcodesDict[i].getSource());
                                decon_wash = true;

                                //Pipette LunaticPlate
                                if (sourceLabwareName == "Eppi")
                                {
                                    streamWriter.WriteLine("A;" + sourceBarcodesDict[i].getEppieRack() + ";;" + sourceLabwareType + ";" + sourceBarcodesDict[i].getRackPos() + ";;" + (3.0).ToString("R", CultureInfo.InvariantCulture) + ";DropSense;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                    streamWriter.WriteLine("D;NSense1;;;" + sourceBarcodesDict[i].getEppiRackPos() + ";;" + (2.0).ToString("R", CultureInfo.InvariantCulture) + ";DropSense;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                }
                                else
                                {
                                    streamWriter.WriteLine("A;" + sourceLabwareName + ";;" + sourceLabwareType + ";" + sourceBarcodesDict[i].getRackPos() + ";;" + (3.0).ToString("R", CultureInfo.InvariantCulture) + ";DropSense;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                    streamWriter.WriteLine("D;NSense1;;;" + sourceBarcodesDict[i].getRackPos() + ";;" + (2.0).ToString("R", CultureInfo.InvariantCulture) + ";DropSense;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
                                }
                                streamWriter.WriteLine("W;");

                                if (++tip == 8)
                                {
                                    tip = 0;
                                    streamWriter.WriteLine("B;");
                                    for (int tipWash = 0; tipWash < 8; tipWash++)
                                    {
                                        streamWriter.WriteLine("A;NaClO;;Trough 100ml;" + (tipWash + 1) + ";;" + maxAspirate.ToString("R", CultureInfo.InvariantCulture) + ";Decon Mix In Trough Detect;;" + Convert.ToInt16(Math.Pow(2, tipWash)) + ";;;");
                                        streamWriter.WriteLine("D;NaClO;;Trough 100ml;" + (tipWash + 1) + ";;" + maxAspirate.ToString("R", CultureInfo.InvariantCulture) + ";Decon Mix In Trough Detect;;" + Convert.ToInt16(Math.Pow(2, tipWash)) + ";;;");
                                        streamWriter.WriteLine("W2;");
                                    }
                                    streamWriter.WriteLine("B;");
                                    decon_wash = false;
                                }
                            }
                        }
                        streamWriter.WriteLine("C; pipetting done - wash tips");
                        tip = 0;
                        streamWriter.WriteLine("B;");
                        if (decon_wash)
                        {
                            for (int tipWash = 0; tipWash < 8; tipWash++)
                            {
                                streamWriter.WriteLine("A;NaClO;;Trough 100ml;" + (tipWash + 1) + ";;" + maxAspirate.ToString("R", CultureInfo.InvariantCulture) + ";Decon Mix In Trough Detect;;" + Convert.ToInt16(Math.Pow(2, tipWash)) + ";;;");
                                streamWriter.WriteLine("D;NaClO;;Trough 100ml;" + (tipWash + 1) + ";;" + maxAspirate.ToString("R", CultureInfo.InvariantCulture) + ";Decon Mix In Trough Detect;;" + Convert.ToInt16(Math.Pow(2, tipWash)) + ";;;");
                                streamWriter.WriteLine("W2;");
                            }
                            streamWriter.WriteLine("B;");
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error writing worklist file: \"" + file + "\"\r\n" + e.Message);
                    throw new Exception("Error writing worklist file: \"" + file + "\"\r\n" + e.Message);
                }
            }

            //if (m_lunatic_measurement == 1)
            //{
            //    //Mixing
            //    if (m_lunatic_mix == 0)
            //    {
            //        try
            //        {
            //            var fileStream = new FileStream(@worklistFileLunatic, FileMode.Create, FileAccess.ReadWrite);
            //            using (var streamWriter = new StreamWriter(fileStream))
            //            {
            //                DateTime localDate = DateTime.Now;
            //                int tip = 0;
            //                int sourceIdx = -1;
            //                double maxAspirate = m_max_diluter_volume;
            //                bool decon_wash = false;

            //                streamWriter.WriteLine("C; This GWL was generated by TecanEvo_MultilabwareNormalization at " + localDate);
            //                streamWriter.WriteLine("B;");
            //                streamWriter.WriteLine("C; Pipetting scheme for Lunatic plate");
            //                streamWriter.WriteLine("B;");
            //                streamWriter.WriteLine("C; DNA Source: " + m_sampleSource + ", DNA Destination: " + m_sampleDestination);
            //                streamWriter.WriteLine("B;");

            //                streamWriter.WriteLine("C; pipetting DNA to Lunatic");
            //                streamWriter.WriteLine("B;");

            //                for (int i = 1; i <= 96; i++)
            //                {
            //                    if (destinationBarcodesDict.ContainsKey(i))
            //                    {
            //                        sourceIdx = destinationBarcodesDict[i].getSourceIdx();

            //                        if (sourceBarcodesDict.ContainsKey(sourceIdx))
            //                        {
            //                            sourceLabwareName = getLabwareName("source", sourceBarcodesDict[sourceIdx].getSource());
            //                            sourceLabwareType = getLabwareType(sourceBarcodesDict[sourceIdx].getSource());
            //                            decon_wash = true;

            //                            //Pipette LunaticPlate
            //                            if (destinationLabwareName == "Eppi")
            //                            {
            //                                streamWriter.WriteLine("A;" + destinationBarcodesDict[i].getEppieRack() + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;" + (3.0).ToString("R", CultureInfo.InvariantCulture) + ";DropSense;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
            //                            }
            //                            else
            //                            {
            //                                streamWriter.WriteLine("A;" + destinationLabwareName + ";;" + destinationLabwareType + ";" + destinationBarcodesDict[i].getRackPos() + ";;" + (3.0).ToString("R", CultureInfo.InvariantCulture) + ";DropSense;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
            //                            }
            //                            streamWriter.WriteLine("D;NSense1;;;" + destinationBarcodesDict[i].getRackPos() + ";;" + (2.5).ToString("R", CultureInfo.InvariantCulture) + ";DropSense;;" + Convert.ToInt16(Math.Pow(2, tip)) + ";;;");
            //                            streamWriter.WriteLine("W;");

            //                            if (++tip == 8)
            //                            {
            //                                tip = 0;
            //                                streamWriter.WriteLine("B;");
            //                                for (int tipWash = 0; tipWash < 8; tipWash++)
            //                                {
            //                                    streamWriter.WriteLine("A;NaClO;;Trough 100ml;" + (tipWash + 1) + ";;" + maxAspirate.ToString("R", CultureInfo.InvariantCulture) + ";Decon Mix In Trough Detect;;" + Convert.ToInt16(Math.Pow(2, tipWash)) + ";;;");
            //                                    streamWriter.WriteLine("D;NaClO;;Trough 100ml;" + (tipWash + 1) + ";;" + maxAspirate.ToString("R", CultureInfo.InvariantCulture) + ";Decon Mix In Trough Detect;;" + Convert.ToInt16(Math.Pow(2, tipWash)) + ";;;");
            //                                    streamWriter.WriteLine("W2;");
            //                                }
            //                                streamWriter.WriteLine("B;");
            //                                decon_wash = false;
            //                            }
            //                        }
            //                    }
            //                }
            //                streamWriter.WriteLine("C; pipetting done - wash tips");
            //                tip = 0;
            //                streamWriter.WriteLine("B;");
            //                if (decon_wash)
            //                {
            //                    for (int tipWash = 0; tipWash < 8; tipWash++)
            //                    {
            //                        streamWriter.WriteLine("A;NaClO;;Trough 100ml;" + (tipWash + 1) + ";;" + maxAspirate.ToString("R", CultureInfo.InvariantCulture) + ";Decon Mix In Trough Detect;;" + Convert.ToInt16(Math.Pow(2, tipWash)) + ";;;");
            //                        streamWriter.WriteLine("D;NaClO;;Trough 100ml;" + (tipWash + 1) + ";;" + maxAspirate.ToString("R", CultureInfo.InvariantCulture) + ";Decon Mix In Trough Detect;;" + Convert.ToInt16(Math.Pow(2, tipWash)) + ";;;");
            //                        streamWriter.WriteLine("W2;");
            //                    }
            //                    streamWriter.WriteLine("B;");
            //                }
            //            }
            //        }
            //        catch (Exception e)
            //        {
            //            MessageBox.Show("Error writing worklist for Lunatic file: \"" + worklistFileLunatic + "\"\r\n" + e.Message);
            //            throw new Exception("Error writing worklist for Lunatic file: \"" + worklistFileLunatic + "\"\r\n" + e.Message);
            //        }
            //    }
            //}
        }

        public string getLabwareName(string mode, string labwareName)
        {
            string prefix = "S ";
            if (mode == "destination") {
                prefix = "D ";
            }

            if (labwareName == "Eppi 2ml")
            {
                return "Eppi";
            }
            else if (labwareName == "Micronic Tube 1.4ml")
            {
                return prefix + "Micronic14";
            }
            else if (labwareName == "Micronic Tube 0.7ml")
            {
                return prefix + "Micronic07";
            }
            else if (labwareName == "Matrix Tube 0.4ml")
            {
                return prefix + "Matrix04";
            }
            else if (labwareName == "Matrix Tube 0.7ml")
            {
                return prefix + "Matrix07";
            }
            else if (labwareName == "PCR Plate")
            {
                return prefix + "PCR Plate";
            }
            else if (labwareName == "DeepWell Plate")
            {
                return prefix + "DeepWell";
            }
            else if (labwareName == "IScan Plate")
            {
                return prefix + "IScan";
            }
            else if (labwareName == "96 Well LVL SX-300")
            {
                return prefix + "LVL300";
            }
            else if (labwareName == "Predilution")
            {
                return "Predilution";
            }
            else if (labwareName == "plate_well")
            {
                if (m_sampleSource == "PCR Plate")
                {
                    return prefix + "PCR Plate";
                }
                else if (m_sampleSource == "DeepWell Plate")
                {
                    return prefix + "DeepWell";
                }
                else if (m_sampleSource == "IScan Plate")
                {
                    return prefix + "IScan";
                }
                else if (m_sampleSource == "96 Well LVL SX-300")
                {
                    return prefix + "LVL300";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public static string getLiquidClass(string labwareName)
        {
            if (labwareName == "PCR Plate")
            {
                return "IKMB Water detect Volume";
            }
            else
            {
                return "";
            }
        }

        public string getLabwareType(string labwareName)
        {
            if (labwareName == "Eppi 2ml")
            {
                return "Tube Eppendorf 16 Pos";
            }
            else if (labwareName == "Micronic Tube 1.4ml")
            {
                return "Micronic 1.4 mL";
            }
            else if (labwareName == "Micronic Tube 0.7ml")
            {
                return "Micronic 0.7 mL";
            }
            else if (labwareName == "Matrix Tube 0.4ml")
            {
                return "Matrix 0.4 mL";
            }
            else if (labwareName == "Matrix Tube 0.7ml")
            {
                return "Matrix 0.7 mL";
            }
            else if (labwareName == "PCR Plate")
            {
                return "96 well PCR in Metalladapter";
            }
            else if (labwareName == "DeepWell Plate")
            {
                return "Deepwell 0.8 mL";
            }
            else if (labwareName == "IScan Plate")
            {
                return "4titudePCR96wellSkirted";
            }
            else if (labwareName == "96 Well LVL SX-300")
            {
                return "96 Well LVL SX-300";
            }
            else if (labwareName == "Predilution")
            {
                return "Deepwell 0.8 mL";
            }
            else if (labwareName == "plate_well")
            {
                if (m_sampleSource == "PCR Plate")
                {
                    return "96 well PCR in Metalladapter";
                }
                else if (m_sampleSource == "DeepWell Plate")
                {
                    return "Deepwell 0.8 mL";
                }
                else if (m_sampleSource == "IScan Plate")
                {
                    return "4titudePCR96wellSkirted";
                }
                else if (m_sampleSource == "96 Well LVL SX-300")
                {
                    return "96 Well LVL SX-300";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        private void writeLunaticTemplate(string path, string mode)
        {
            var file = path + "/LunaticBarcodes1.txt";
            try
            {
                var fileStream = new FileStream(@file, FileMode.Create, FileAccess.ReadWrite);
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    if (mode == "Normalization")
                    {
                        for (int i = 1; i <= 96; i++)
                        {
                            if (destinationBarcodesDict.ContainsKey(i))
                            {
                                streamWriter.WriteLine("TransferJobNDropPlate1," + numberTo96wellRackPosLunatic(i) + "," + destinationBarcodesDict[i].getBarcode());
                            }
                            else
                            {
                                streamWriter.WriteLine("TransferJobNDropPlate1," + numberTo96wellRackPosLunatic(i) + "," + numberTo96wellRackPos(i));
                            }
                        }

                    }
                    else
                    {
                        for (int i = 1; i <= 96; i++)
                        {
                            if (sourceBarcodesDict.ContainsKey(i))
                            {
                                streamWriter.WriteLine("TransferJobNDropPlate1," + numberTo96wellRackPosLunatic(i) + "," + sourceBarcodesDict[i].getBarcode());
                            }
                            else
                            {
                                streamWriter.WriteLine("TransferJobNDropPlate1," + numberTo96wellRackPosLunatic(i) + "," + numberTo96wellRackPos(i));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error writing lunatic template file: \"" + file + "\"\r\n" + e.Message);
                throw new Exception("Error writing lunatic template file: \"" + file + "\"\r\n" + e.Message);
            }
        }

        private void writeLunaticExpDefs(string path)
        {
            var file = path + "/expdef_Multilabware_DB.txt";
            try
            {
                var fileStream = new FileStream(@file, FileMode.Create, FileAccess.ReadWrite);
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.WriteLine("[Experiment definition]");
                    streamWriter.WriteLine("application_name=\"" + m_lunatic_program + "\"");
                    streamWriter.WriteLine("dropplate_type=DropPlate-S");
                    streamWriter.WriteLine("background_wavelength=340");
                    streamWriter.WriteLine("[Import samples]");
                    streamWriter.WriteLine("column_source_plate=-1");
                    streamWriter.WriteLine("column_source_position=-1");
                    streamWriter.WriteLine("column_dropplate_ID=0");
                    streamWriter.WriteLine("column_dropplate_position=1");
                    streamWriter.WriteLine("column_sample_name=2");
                    streamWriter.WriteLine("column_blank_dropplate_ID=-1");
                    streamWriter.WriteLine("column_blank_dropplate_position=-1");
                    streamWriter.WriteLine("column_dna_rna=-1");
                    streamWriter.WriteLine("column_sequence=-1");
                    streamWriter.WriteLine("blanking_information=0");
                    streamWriter.WriteLine("blank_dropplate_ID=\"\"");
                    streamWriter.WriteLine("blank_dropplate_position=\"\"");
                    streamWriter.WriteLine("blank_name_used=\"\"");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error writing lunatic parameter file: \"" + file + "\"\r\n" + e.Message);
                throw new Exception("Error writing lunatic parameter file: \"" + file + "\"\r\n" + e.Message);
            }
        }

        public static string numberTo96wellRackPos(int idx)
        {
            switch (idx)
            {
                case 1: return "A01";
                case 2: return "B01";
                case 3: return "C01";
                case 4: return "D01";
                case 5: return "E01";
                case 6: return "F01";
                case 7: return "G01";
                case 8: return "H01";
                case 9: return "A02";
                case 10: return "B02";
                case 11: return "C02";
                case 12: return "D02";
                case 13: return "E02";
                case 14: return "F02";
                case 15: return "G02";
                case 16: return "H02";
                case 17: return "A03";
                case 18: return "B03";
                case 19: return "C03";
                case 20: return "D03";
                case 21: return "E03";
                case 22: return "F03";
                case 23: return "G03";
                case 24: return "H03";
                case 25: return "A04";
                case 26: return "B04";
                case 27: return "C04";
                case 28: return "D04";
                case 29: return "E04";
                case 30: return "F04";
                case 31: return "G04";
                case 32: return "H04";
                case 33: return "A05";
                case 34: return "B05";
                case 35: return "C05";
                case 36: return "D05";
                case 37: return "E05";
                case 38: return "F05";
                case 39: return "G05";
                case 40: return "H05";
                case 41: return "A06";
                case 42: return "B06";
                case 43: return "C06";
                case 44: return "D06";
                case 45: return "E06";
                case 46: return "F06";
                case 47: return "G06";
                case 48: return "H06";
                case 49: return "A07";
                case 50: return "B07";
                case 51: return "C07";
                case 52: return "D07";
                case 53: return "E07";
                case 54: return "F07";
                case 55: return "G07";
                case 56: return "H07";
                case 57: return "A08";
                case 58: return "B08";
                case 59: return "C08";
                case 60: return "D08";
                case 61: return "E08";
                case 62: return "F08";
                case 63: return "G08";
                case 64: return "H08";
                case 65: return "A09";
                case 66: return "B09";
                case 67: return "C09";
                case 68: return "D09";
                case 69: return "E09";
                case 70: return "F09";
                case 71: return "G09";
                case 72: return "H09";
                case 73: return "A10";
                case 74: return "B10";
                case 75: return "C10";
                case 76: return "D10";
                case 77: return "E10";
                case 78: return "F10";
                case 79: return "G10";
                case 80: return "H10";
                case 81: return "A11";
                case 82: return "B11";
                case 83: return "C11";
                case 84: return "D11";
                case 85: return "E11";
                case 86: return "F11";
                case 87: return "G11";
                case 88: return "H11";
                case 89: return "A12";
                case 90: return "B12";
                case 91: return "C12";
                case 92: return "D12";
                case 93: return "E12";
                case 94: return "F12";
                case 95: return "G12";
                case 96: return "H12";
                default: return "";
            }
        }

        public static string numberTo96wellRackPosLunatic(int idx)
        {
            switch (idx)
            {
                case 1: return "A1";
                case 2: return "B1";
                case 3: return "C1";
                case 4: return "D1";
                case 5: return "E1";
                case 6: return "F1";
                case 7: return "G1";
                case 8: return "H1";
                case 9: return "A2";
                case 10: return "B2";
                case 11: return "C2";
                case 12: return "D2";
                case 13: return "E2";
                case 14: return "F2";
                case 15: return "G2";
                case 16: return "H2";
                case 17: return "A3";
                case 18: return "B3";
                case 19: return "C3";
                case 20: return "D3";
                case 21: return "E3";
                case 22: return "F3";
                case 23: return "G3";
                case 24: return "H3";
                case 25: return "A4";
                case 26: return "B4";
                case 27: return "C4";
                case 28: return "D4";
                case 29: return "E4";
                case 30: return "F4";
                case 31: return "G4";
                case 32: return "H4";
                case 33: return "A5";
                case 34: return "B5";
                case 35: return "C5";
                case 36: return "D5";
                case 37: return "E5";
                case 38: return "F5";
                case 39: return "G5";
                case 40: return "H5";
                case 41: return "A6";
                case 42: return "B6";
                case 43: return "C6";
                case 44: return "D6";
                case 45: return "E6";
                case 46: return "F6";
                case 47: return "G6";
                case 48: return "H6";
                case 49: return "A7";
                case 50: return "B7";
                case 51: return "C7";
                case 52: return "D7";
                case 53: return "E7";
                case 54: return "F7";
                case 55: return "G7";
                case 56: return "H7";
                case 57: return "A8";
                case 58: return "B8";
                case 59: return "C8";
                case 60: return "D8";
                case 61: return "E8";
                case 62: return "F8";
                case 63: return "G8";
                case 64: return "H8";
                case 65: return "A9";
                case 66: return "B9";
                case 67: return "C9";
                case 68: return "D9";
                case 69: return "E9";
                case 70: return "F9";
                case 71: return "G9";
                case 72: return "H9";
                case 73: return "A10";
                case 74: return "B10";
                case 75: return "C10";
                case 76: return "D10";
                case 77: return "E10";
                case 78: return "F10";
                case 79: return "G10";
                case 80: return "H10";
                case 81: return "A11";
                case 82: return "B11";
                case 83: return "C11";
                case 84: return "D11";
                case 85: return "E11";
                case 86: return "F11";
                case 87: return "G11";
                case 88: return "H11";
                case 89: return "A12";
                case 90: return "B12";
                case 91: return "C12";
                case 92: return "D12";
                case 93: return "E12";
                case 94: return "F12";
                case 95: return "G12";
                case 96: return "H12";
                default: return "";
            }
        }

        public int RackPosTo96wellNumber(string well)
        {
            switch (well)
            {
                case "A01": return 1;
                case "B01": return 2;
                case "C01": return 3;
                case "D01": return 4;
                case "E01": return 5;
                case "F01": return 6;
                case "G01": return 7;
                case "H01": return 8;
                case "A02": return 9;
                case "B02": return 10;
                case "C02": return 11;
                case "D02": return 12;
                case "E02": return 13;
                case "F02": return 14;
                case "G02": return 15;
                case "H02": return 16;
                case "A03": return 17;
                case "B03": return 18;
                case "C03": return 19;
                case "D03": return 20;
                case "E03": return 21;
                case "F03": return 22;
                case "G03": return 23;
                case "H03": return 24;
                case "A04": return 25;
                case "B04": return 26;
                case "C04": return 27;
                case "D04": return 28;
                case "E04": return 29;
                case "F04": return 30;
                case "G04": return 31;
                case "H04": return 32;
                case "A05": return 33;
                case "B05": return 34;
                case "C05": return 35;
                case "D05": return 36;
                case "E05": return 37;
                case "F05": return 38;
                case "G05": return 39;
                case "H05": return 40;
                case "A06": return 41;
                case "B06": return 42;
                case "C06": return 43;
                case "D06": return 44;
                case "E06": return 45;
                case "F06": return 46;
                case "G06": return 47;
                case "H06": return 48;
                case "A07": return 49;
                case "B07": return 50;
                case "C07": return 51;
                case "D07": return 52;
                case "E07": return 53;
                case "F07": return 54;
                case "G07": return 55;
                case "H07": return 56;
                case "A08": return 57;
                case "B08": return 58;
                case "C08": return 59;
                case "D08": return 60;
                case "E08": return 61;
                case "F08": return 62;
                case "G08": return 63;
                case "H08": return 64;
                case "A09": return 65;
                case "B09": return 66;
                case "C09": return 67;
                case "D09": return 68;
                case "E09": return 69;
                case "F09": return 70;
                case "G09": return 71;
                case "H09": return 72;
                case "A10": return 73;
                case "B10": return 74;
                case "C10": return 75;
                case "D10": return 76;
                case "E10": return 77;
                case "F10": return 78;
                case "G10": return 79;
                case "H10": return 80;
                case "A11": return 81;
                case "B11": return 82;
                case "C11": return 83;
                case "D11": return 84;
                case "E11": return 85;
                case "F11": return 86;
                case "G11": return 87;
                case "H11": return 88;
                case "A12": return 89;
                case "B12": return 90;
                case "C12": return 91;
                case "D12": return 92;
                case "E12": return 93;
                case "F12": return 94;
                case "G12": return 95;
                case "H12": return 96;
                default: return -1;
            }
        }



        public static Int32 wellToNumber(char row, int col)
        {
            int i_row = 0;
            switch (row)
            {
                case 'A':
                    i_row = 1;
                    break;
                case 'B':
                    i_row = 2;
                    break;
                case 'C':
                    i_row = 3;
                    break;
                case 'D':
                    i_row = 4;
                    break;
                case 'E':
                    i_row = 5;
                    break;
                case 'F':
                    i_row = 6;
                    break;
                case 'G':
                    i_row = 7;
                    break;
                case 'H':
                    i_row = 8;
                    break;
                default:
                    i_row = 0;
                    break;
            }
            if (i_row > 0)
            {
                return i_row + (col - 1) * 8;
            }
            else
            {
                return -1;
            }
        }

        public static string getWell(char row, int col)
        {
            if (col < 10)
            {
                return row + "0" + col.ToString();
            }
            else
            {
                return row + col.ToString();
            }
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            m_retVal = 2;
            Application.Exit();
        }

        private void butContinue_Click(object sender, EventArgs e)
        {
            writeJobFile(m_args[4]);
            writeWorklistFile(m_args[5], m_mode);
            m_retVal = 0;
            Application.Exit();
        }
    }
}
