using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecanEvo_MultilabwareNormalizationPredilution
{
    class CSample
    {
        private string m_sampleSource = "";
        private string m_barcode = "***";
        private string m_rackWell = "";
        private string m_category = "UKN";
        private double m_concentration = 0.0;
        private double m_volume = 0.0;
        private double m_dw_sample_vol = 0.0;
        private double m_dw_buffer_vol = 0.0;
        private double m_sample_vol = 0.0;
        private double m_buffer_vol = 0.0;

        private Int32 m_sourceIdx = -1;

        private Int32 m_eppieRack = -1;
        private Int32 m_eppieRackMod = 0;
        private Int32 m_rackPos = 0;

        private Int32 m_patientId = -1;
        private Int32 m_sampleId = -1;


        public Int32 getEppieRack()
        {
            return m_eppieRack;
        }

        public Int32 getRackPos()
        {
            return m_rackPos;
        }

        public Int32 getEppiRackPos()
        {
            return (16*(m_eppieRack - m_eppieRackMod - 1) + m_rackPos);
        }

        public string getMicronicRackWell()
        {
            return m_rackWell;
        }

        public string getRackPosDetailsAsString()
        {
            String strRet = "";

            strRet += Convert.ToString(m_eppieRack).PadRight(5);
            strRet += Convert.ToString(m_rackPos).PadRight(5);
            strRet += m_barcode.PadRight(15);

            return strRet;
        }

        public CSample(string source, string barcode, Int32 eppieRack, Int32 rackPos, string micronicRackWell, Int32 eppieRackMod = 0)
        {
            m_sampleSource = source;
            m_barcode = barcode;
            m_eppieRack = eppieRack;
            m_rackPos = rackPos;
            m_rackWell = micronicRackWell;
            m_eppieRackMod = eppieRackMod;
        }

        public CSample(string source, string barcode, string concentration, string volume)
        {
            m_sampleSource = source;
            m_barcode = barcode;
            m_concentration = Convert.ToDouble(concentration);
            m_volume = Convert.ToDouble(volume);
        }

        public CSample(string source, string barcode, double concentration, double volume)
        {
            m_sampleSource = source;
            m_barcode = barcode;
            m_concentration = concentration;
            m_volume = volume;
        }

        public int getSourceIdx()
        {
            return m_sourceIdx;
        }

        public void setSourceIdx(int sourceIdx)
        {
            m_sourceIdx = sourceIdx;
        }

        public string getSource()
        {
            return m_sampleSource;
        }

        public double getVolume()
        {
            return m_volume;
        }

        public double getConcentration()
        {
            return m_concentration;
        }

        public string getCategory()
        {
            return m_category;
        }

        public void setVolume(double volume)
        {
            m_volume = volume;
        }

        public void setConcentration(double concentration)
        {
            m_concentration = concentration;
        }

        public void setVolumeAndConcentration(double volume, double concentration)
        {
            m_volume = volume;
            m_concentration = concentration;
        }

        public void setPatientAndSample(int patient_id, int sample_id)
        {
            m_patientId = patient_id;
            m_sampleId = sample_id;
        }

        public void setCategory(string category)
        {
            m_category = category;
        }

        public string getBarcode()
        {
            return m_barcode;
        }

        public int getPatient()
        {
            return m_patientId;
        }

        public int getSample()
        {
            return m_sampleId;
        }

        public double getDWSampleVol()
        {
            return m_dw_sample_vol;
        }

        public double getDWBufferVol()
        {
            return m_dw_buffer_vol;
        }

        public void setDWSampleVol(double sample_vol)
        {
            m_dw_sample_vol = sample_vol;
        }

        public void setDWBufferVol(double buffer_vol)
        {
            m_dw_buffer_vol = buffer_vol;
        }

        public void setSampleVol(double sample_vol)
        {
            m_sample_vol = sample_vol;
        }

        public void setBufferVol(double buffer_vol)
        {
            m_buffer_vol = buffer_vol;
        }

        public double getSampleVol()
        {
            return m_sample_vol;
        }

        public double getBufferVol()
        {
            return m_buffer_vol;
        }
    }
}
