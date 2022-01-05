using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecanEvo_MultilabwareNormalizationPredilution
{
    class CSampleDBEntry
    {
        private Int32 m_patient_id = -1;
        private string m_barcode = "";
        private string m_parentBarcode = "";
        private Int32 m_category = 7;
        private double m_concentration = 0.0;
        private double m_volume = 0.0;
        private double m_parent_used_volume = 0.0;
        private string m_well = "";
        private Int32 m_container = -1;

        public CSampleDBEntry(Int32 patient_id, string barcode, string parentBarcode, Int32 category, Int32 container, double volume, double parent_used_volume)
        {
            m_patient_id = patient_id;
            m_barcode = barcode;
            m_parentBarcode = parentBarcode;
            m_category = category;
            m_container = container;
            m_volume = Convert.ToDouble(volume);
            m_parent_used_volume = Convert.ToDouble(parent_used_volume);
        }

        public CSampleDBEntry(Int32 patient_id, string barcode, string parentBarcode, Int32 category, Int32 container, double concentration, double volume, double parent_used_volume)
        {
            m_patient_id = patient_id;
            m_barcode = barcode;
            m_parentBarcode = parentBarcode;
            m_category = category;
            m_container = container;
            m_concentration = Convert.ToDouble(concentration);
            m_volume = Convert.ToDouble(volume);
            m_parent_used_volume = Convert.ToDouble(parent_used_volume);
        }

        public Int32 getPatientID()
        {
            return m_patient_id;
        }

        public string getBarcode()
        {
            return m_barcode;
        }

        public string getParentBarcode()
        {
            return m_parentBarcode;
        }

        public Int32 getCategory()
        {
            return m_category;
        }

        public double getVolume()
        {
            return m_volume;
        }

        public double getConcentration()
        {
            return m_concentration;
        }

        public double getUsedParentVolume()
        {
            return m_parent_used_volume;
        }

        public Int32 getContainerID()
        {
            return m_container;
        }

        public string getWell()
        {
            return m_well;
        }

        public void setWell(string well)
        {
            m_well = well;
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

    }
}
