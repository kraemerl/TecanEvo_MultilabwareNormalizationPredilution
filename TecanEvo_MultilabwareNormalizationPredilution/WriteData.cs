using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace TecanEvo_MultilabwareNormalizationPredilution
{
    class WriteData
    {
        /// <summary>
        /// retVal: 0 - all OK
        /// retVal: 1 - all OK, load variables
        /// retVal: 2 - Errors, abort Evoware script
        /// 
        /// 
        /// write_data C:\Lars\Git\C#_projects\TecanEvo_MultilabwareNormalizationPredilution\Data\variables.csv C:\Lars\Git\C#_projects\TecanEvo_MultilabwareNormalizationPredilution\Data\lastJobInfo.txt C:\Lars\Git\C#_projects\TecanEvo_MultilabwareNormalizationPredilution\Data\worklist.gwl  C:\Lars\Git\C#_projects\TecanEvo_MultilabwareNormalizationPredilution\Data\Lunatic.txt C:\Lars\Git\C#_projects\TecanEvo_MultilabwareNormalizationPredilution\Data\micronic_volumes.txt C:\Lars\Git\C#_projects\TecanEvo_MultilabwareNormalizationPredilution\Data\Backup
        /// </summary>
        private int m_retVal = 2;

        private Int32 m_use_ibdbase = 1;
        private string m_mode = "Normalization";
        private string m_sampleSource = "";
        private string m_sampleDestination = "";
        private string m_destination_box = "";
        private Int32 m_destination_box_id = -1;
        private string m_destination_box_prefix = "";
        private Int32 m_container_id = -1;
        private Int32 m_targetConc = 0; // in ng/µl
        private Int32 m_targetVol = 0; // in µl
        private Int32 m_normalize = 0; // boolean: 0 or 1
        private Int32 m_measure_volume = 0; // boolean: 0 or 1
        private Int32 m_lunatic_measurement = 0; // boolean: 0 or 1
        private string errInfo = "";

        private Dictionary<string, CSampleDBEntry> newSampleDict = new Dictionary<string, CSampleDBEntry>();

        private string[] m_args;

        public WriteData(string[] args)
        {
            m_args = args;
            try
            {
                loadVariables(m_args[1]);
                backupFiles();
                loadLastJobFile(m_args[2], m_mode);
                if (m_mode == "Normalization")
                {
                    if (m_lunatic_measurement == 1)
                    {
                        loadLunaticFile(m_args[4]);
                    }
                    if (m_measure_volume == 1)
                    {
                        loadMicronicVolumeFile(m_args[5]);
                    }
                } else
                {
                    loadLunaticFile(m_args[4]);
                }
                if (m_use_ibdbase == 1)
                {
                    writeToIBDbase(m_mode);
                }
               
                m_retVal = 0;
                deleteFiles();
            }
            catch (Exception e)
            {
                m_retVal = 2;
                MessageBox.Show(e.Message);
                Application.Exit();
            }
        }

        public int returnValue()
        {
            return m_retVal;
        }

        private void backupFiles()
        {
            if (Directory.Exists(m_args[6]) == false)
            {
                DirectoryInfo di = Directory.CreateDirectory(m_args[6]);
            }

            DateTime localDate = DateTime.Now;
            string prefix = m_args[6] + "\\" + localDate.ToString("yyyyMMdd_HHmmssfff") + "_";
            File.Copy(m_args[1], prefix + "variables.txt");
            File.Copy(m_args[2], prefix + "jobInfo.txt");
            File.Copy(m_args[3], prefix + "worklist.txt");
            if (m_lunatic_measurement == 1 || m_mode == "Measurement")
            {
                File.Copy(m_args[4], prefix + "Lunatic.txt");
            }
            if (m_measure_volume == 1)
            {
                File.Copy(m_args[5], prefix + "micronic_volumes.txt");
            }
        }

        private void deleteFiles()
        {
            File.Delete(m_args[1]);
            File.Delete(m_args[2]);
            File.Delete(m_args[3]);
            if (m_normalize == 1 || m_mode == "Measurement")
            {
                File.Delete(m_args[4]);
            }
            if (m_measure_volume == 1)
            {
                File.Delete(m_args[5]);
            }
        }

        private void loadLastJobFile(string file, string mode)
        {
            try
            {
                var fileStream = new FileStream(@file, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream))
                {
                    string line;
                    if ((line = streamReader.ReadLine()) != null)
                    {
                        if (mode == "Normalization")
                        {
                            while ((line = streamReader.ReadLine()) != null)
                            {
                                String[] tokenized = line.Split('\t');
                                for (int i = 0; i < tokenized.Length; i++)
                                    tokenized[i] = tokenized[i].Trim(' ');
                                if (tokenized[0] != "source" && tokenized[0] != "")
                                {
                                    var category = -1;
                                    if (m_use_ibdbase == 1)
                                    {
                                        category = CSQL.getCategoryID(tokenized[4]);
                                    }
                                    if (m_normalize == 1)
                                    {
                                        newSampleDict[tokenized[3]] = new CSampleDBEntry(Convert.ToInt32(tokenized[1]), tokenized[7], tokenized[3], category, m_container_id, m_targetConc, m_targetVol, Convert.ToDouble(tokenized[9]));
                                    }
                                    else
                                    {
                                        newSampleDict[tokenized[3]] = new CSampleDBEntry(Convert.ToInt32(tokenized[1]), tokenized[7], tokenized[3], category, m_container_id, Convert.ToDouble(tokenized[6], CultureInfo.GetCultureInfo("us-EN").NumberFormat), m_targetVol, Convert.ToDouble(tokenized[9]));
                                    }
                                    if (m_sampleDestination == "PCR Plate" || m_sampleDestination == "DeepWell Plate" || m_sampleDestination == "IScan Plate" || m_sampleDestination == "96 Well LVL SX-300")
                                    {
                                        newSampleDict[tokenized[3]].setWell(newSampleDict[tokenized[3]].getBarcode().Replace(m_destination_box_prefix + "-", ""));
                                    }
                                }
                            }
                        }
                        else
                        {
                            while ((line = streamReader.ReadLine()) != null)
                            {
                                String[] tokenized = line.Split('\t');
                                for (int i = 0; i < tokenized.Length; i++)
                                    tokenized[i] = tokenized[i].Trim(' ');
                                if (tokenized[0] != "source" && tokenized[0] != "")
                                {
                                    var category = -1;
                                    if (m_use_ibdbase == 1)
                                    {
                                        category = CSQL.getCategoryID(tokenized[4]);
                                    }
                                    newSampleDict[tokenized[3]] = new CSampleDBEntry(Convert.ToInt32(tokenized[1]), tokenized[3], "", category, m_container_id, 0, 0, 3);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                m_retVal = 2;
                throw new Exception("Error importing last job file: \"" + file + "\"\r\n" + e.Message);
            }
        }

        private void loadLunaticFile(string file)
        {
            try
            {
                double concentration = -1;
                int errorCount = 0;
                var fileStream = new FileStream(@file, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream))
                {
                    string line;
                    if ((line = streamReader.ReadLine()) != null)
                    {
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            String[] tokenized = line.Split(',');
                            for (int i = 0; i < tokenized.Length; i++)
                                tokenized[i] = tokenized[i].Trim(' ');
                            foreach (string key in newSampleDict.Keys)
                            {
                                if (newSampleDict[key].getBarcode() == tokenized[2])
                                {
                                    if (tokenized[4] != "-")
                                    {
                                        concentration = Convert.ToDouble(tokenized[4], CultureInfo.GetCultureInfo("us-EN").NumberFormat);
                                    }
                                    else
                                    {
                                        concentration = -1;
                                    }
                                    newSampleDict[key].setConcentration(concentration);
                                }
                            }
                        }
                    }
                }
                if (errorCount > 0)
                {
                    //MessageBox.Show("Could not find concentration values of " + errorCount + " samples!");         
                }
            }
            catch (Exception e)
            {
                m_retVal = 2;
                throw new Exception("Error importing last Lunatic file: \"" + file + "\"\r\n" + e.Message);
            }
        }

        private void loadMicronicVolumeFile(string file)
        {
            try
            {
                double volume = -1;
                double currentVolume = -1;
                int errorCount = 0;
                var fileStream = new FileStream(@file, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        String[] tokenized = line.Split('\t');
                        for (int i = 0; i < tokenized.Length; i++)
                            tokenized[i] = tokenized[i].Trim(' ');
                        foreach (string key in newSampleDict.Keys)
                        {
                            if (newSampleDict[key].getBarcode() == tokenized[0])
                            {
                                if (tokenized[1] != "-")
                                {
                                    volume = Convert.ToDouble(tokenized[1], CultureInfo.GetCultureInfo("us-EN").NumberFormat);
                                }
                                else
                                {
                                    volume = -1;
                                }
                                currentVolume = newSampleDict[key].getVolume();

                                if (volume > 0 && volume < currentVolume)
                                {
                                    newSampleDict[key].setVolume(volume);
                                }
                            }
                        }
                    }
                }
                if (errorCount > 0)
                {
                    //MessageBox.Show("Could not find concentration values of " + errorCount + " samples!");         
                }
            }
            catch (Exception e)
            {
                m_retVal = 2;
                throw new Exception("Error importing last micronic volume file: \"" + file + "\"\r\n" + e.Message);
            }
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
                        if (line != "")
                        {
                            errInfo = line;
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
                                }
                                else if (tokenized[1] == "sample_source")
                                {
                                    m_sampleSource = tokenized[2].Trim();
                                }
                                else if (tokenized[1] == "sample_destination")
                                {
                                    m_sampleDestination = tokenized[2].Trim();
                                    if (m_sampleDestination == "PCR Plate" || m_sampleDestination == "DeepWell Plate" || m_sampleDestination == "IScan Plate" || m_sampleDestination == "96 Well LVL SX-300")
                                    {
                                        m_container_id = CSQL.getContainerID("plate_well");
                                    }
                                    else
                                    {
                                        m_container_id = CSQL.getContainerID(m_sampleDestination);
                                    }
                                }
                                else if (tokenized[1] == "destination_box_bc")
                                {
                                    m_destination_box = tokenized[2].Trim();
                                    m_destination_box_id = CSQL.getBoxID(m_destination_box);
                                }
                                else if (tokenized[1] == "destination_box_prefix")
                                {
                                    m_destination_box_prefix = tokenized[2].Trim();
                                }
                                else if (tokenized[1] == "target_volume")
                                {
                                    m_targetVol = int.Parse(tokenized[2].Trim());
                                }
                                else if (tokenized[1] == "target_concentration")
                                {
                                    m_targetConc = int.Parse(tokenized[2].Trim());
                                }
                                else if (tokenized[1] == "normalize")
                                {
                                    m_normalize = int.Parse(tokenized[2].Trim());
                                }
                                else if (tokenized[1] == "lunatic")
                                {
                                    m_lunatic_measurement = int.Parse(tokenized[2].Trim());
                                    m_targetVol = m_targetVol - 3;
                                }
                                else if (tokenized[1] == "measure_volume")
                                {
                                    m_measure_volume = int.Parse(tokenized[2].Trim());
                                }
                            }
                        }
                    }
                    fileStream.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error importing variables file: \"" + file + "\" (" + errInfo + ")\r\n" + e.Message);
            }
        }

        private void writeToIBDbase(string mode)
        {
            try
            {
                Int32 SampleID = -1;
                
                if (mode == "Normalization")
                {
                    foreach (string key in newSampleDict.Keys)
                    {
                        SampleID = CSQL.createSampleAliqout(
                            newSampleDict[key].getPatientID(),
                            newSampleDict[key].getParentBarcode(),
                            newSampleDict[key].getBarcode(),
                            false,
                            newSampleDict[key].getCategory(),
                            1,
                            2,
                            newSampleDict[key].getContainerID(),
                            newSampleDict[key].getVolume(),
                            newSampleDict[key].getUsedParentVolume(),
                            newSampleDict[key].getConcentration(),
                            "tecan",
                            ""
                        );
                        if (m_sampleDestination == "PCR Plate" || m_sampleDestination == "DeepWell Plate" || m_sampleDestination == "IScan Plate" || m_sampleDestination == "96 Well LVL SX-300")
                        {
                            string well = newSampleDict[key].getWell();
                            char row = well[0];
                            Int32 col = int.Parse(well.Substring(1));
                            CSQL.trackSampleToBox(SampleID, m_destination_box_id, row, col);
                        }
                    }
                } else
                {
                    foreach (string key in newSampleDict.Keys)
                    {
                        if (newSampleDict[key].getConcentration() > 0) {
                            CSQL.updateConcentration(
                                newSampleDict[key].getBarcode(),
                                newSampleDict[key].getConcentration()
                            );
                        }
                    }
                }
            }
            catch (Exception e)
            {
                m_retVal = 2;
                throw new Exception("Error while writing to IBDbase:" + e.Message);
            }
        }
    }
}
