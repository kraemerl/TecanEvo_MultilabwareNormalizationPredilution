using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Text;
using System.Threading;

namespace TecanEvo_MultilabwareNormalizationPredilution
{
    class CSQL
    {
        private static String connString = "User Id=tecan;Password=tecan;Data Source=ukshikmb-sw054.i-kmb.de;Initial Catalog=ibdbase;";

        public static SqlConnection getConnection()
        {
            SqlConnection myConnection = new SqlConnection(connString);
            myConnection.Open();
            return myConnection;
        }


        public static void setPatients(Dictionary<int, CSample> tubeBarcodes)
        {
            string lst = "";
            for (int i = 1; i <= 96; i++)
            {
                if (tubeBarcodes.ContainsKey(i))
                {
                    lst += ", '" + tubeBarcodes[i].getBarcode() + "'";
                }
            }
            lst = "(" + lst.Substring(1) + ")";

            SqlConnection myConnection = new SqlConnection(connString);
            myConnection.Open();

            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand("SELECT bar_code, patient_id, sample_id, sc.categ_name_id " +
                                                " FROM sample s" +
                                                " JOIN sample_category sc ON s.category_id = sc.category_id" +
                                                " WHERE bar_code IN " + lst
                                                , myConnection);

            myReader = myCommand.ExecuteReader();
            if (myReader.HasRows)
                while (myReader.Read())
                {
                    for (int i = 1; i <= 96; i++)
                    {
                        if (tubeBarcodes.ContainsKey(i))
                        {
                            if (tubeBarcodes[i].getBarcode() == myReader["bar_code"].ToString())
                            {
                                tubeBarcodes[i].setPatientAndSample(int.Parse(myReader["patient_id"].ToString()), int.Parse(myReader["sample_id"].ToString()));
                                tubeBarcodes[i].setCategory(myReader["categ_name_id"].ToString());
                            }
                        }
                    }
                }
            myReader.Close();
        }    

        /// <summary>
        ///  updates List<CDBSample> dbEntries with concentration and volume and returns a list of sample_ids that are in the DB layout but have no entry
        ///  for concentration and/or value
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="dbEntries"></param>
        /// <returns></returns>
        public static void addVolAndConc(Dictionary<int, CSample> tubeBarcodes)
        {
            string lst = "";
            foreach (int key in tubeBarcodes.Keys)
            {
                lst += ", " + tubeBarcodes[key].getSample();
            }
            lst = "(" + lst.Substring(1) + ")";

            SqlConnection myConnection = new SqlConnection(connString);
            myConnection.Open();

            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand("SELECT case when sc.sample_id is null then sv.sample_id else sc.sample_id end as sample_id, " +
                                                " isnull(volume,-1) as volume, " +
                                                " isnull(concentration,-1) as concentration " +
                                                " from ( " +
                                                "	select s.sample_id, s.property_value as volume " +
                                                "	from sample_prop_dec s " +
                                                "	JOIN ( " +
                                                "		select sample_id, max(date_entered) as date_entered " +
                                                "		from sample_prop_dec " +
                                                "       where property_id = 2 " +
                                                "		and property_value != -1 " +
                                                "		and sample_id in " + lst + " " +
                                                "		group by sample_id " +
                                                "	) dd on dd.sample_id = s.sample_id and property_id = 2 and dd.date_entered = s.date_entered " +
                                                ") sv " +
                                                "FULL OUTER JOIN( " +
                                                "	select s.sample_id, s.property_value as concentration " +
                                                "	from sample_prop_dec s " +
                                                "	join ( " +
                                                "		select sample_id, max(date_entered) as date_entered " +
                                                "		from sample_prop_dec " +
                                                "       where property_id = 1 " +
                                                "		and property_value != -1 " +
                                                "		and sample_id in " + lst + " " +
                                                "		group by sample_id " +
                                                "	) dd on dd.sample_id = s.sample_id and property_id = 1 and dd.date_entered = s.date_entered " +
                                                ") sc on sc.sample_id = sv.sample_id"
                                                , myConnection);

            myReader = myCommand.ExecuteReader();
            if (myReader.HasRows)
                while (myReader.Read())
                {
                    foreach (int key in tubeBarcodes.Keys)
                    {
                        if (tubeBarcodes[key].getSample() == int.Parse(myReader["sample_id"].ToString()))
                        {
                            tubeBarcodes[key].setVolumeAndConcentration(Convert.ToDouble(myReader["volume"].ToString()), Convert.ToDouble(myReader["concentration"].ToString()));
                        }
                    }
                }
            myReader.Close();
        }

        public static Int32 getContainerID(string container_type)
        {
            SqlConnection myConnection = new SqlConnection(connString);
            myConnection.Open();

            SqlCommand myCommand = new SqlCommand("SELECT container_type_id " +
                                                " FROM sample_container_type" +
                                                " WHERE container_type_name = '" + container_type + "'"
                                                , myConnection);

            return int.Parse(myCommand.ExecuteScalar().ToString());
        }

        public static Int32 getCategoryID(string categ_name_id)
        {
            SqlConnection myConnection = new SqlConnection(connString);
            myConnection.Open();

            SqlCommand myCommand = new SqlCommand("SELECT category_id " +
                                                " FROM sample_category" +
                                                " WHERE categ_name_id = '" + categ_name_id + "'"
                                                , myConnection);

            return int.Parse(myCommand.ExecuteScalar().ToString());
        }

        public static Int32 createSampleAliqout(Int32 patient_id,
                                              string parent_barcode,
                                              string child_barcode,
                                              bool parent_empty,
                                              Int32 category_id,
                                              Int32 sample_status_id,
                                              Int32 sample_buffer_id,
                                              Int32 container_type_id,
                                              double vol,
                                              double used_parent_vol,
                                              double conc,
                                              string received_by,
                                              string comment
       )
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("spu_sample_add_aliquot", myConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@patient_id", SqlDbType.Int).Value = patient_id;
                    cmd.Parameters.Add("@parent_barcode", SqlDbType.VarChar, 25).Value = parent_barcode;
                    cmd.Parameters.Add("@parent_used_volume", SqlDbType.Decimal, 10).Value = used_parent_vol;
                    cmd.Parameters.Add("@child_barcode", SqlDbType.VarChar, 25).Value = child_barcode;
                    cmd.Parameters.Add("@parent_empty", SqlDbType.Bit).Value = parent_empty;
                    cmd.Parameters.Add("@category_id", SqlDbType.Int).Value = category_id;
                    cmd.Parameters.Add("@sample_status_id", SqlDbType.Int).Value = sample_status_id;
                    cmd.Parameters.Add("@sample_buffer_id", SqlDbType.Int).Value = sample_buffer_id;
                    cmd.Parameters.Add("@container_type_id", SqlDbType.Int).Value = container_type_id;
                    cmd.Parameters.Add("@vol", SqlDbType.Decimal, 10).Value = vol;
                    cmd.Parameters.Add("@conc", SqlDbType.Decimal, 10).Value = conc;
                    cmd.Parameters.Add("@received_by", SqlDbType.VarChar, 20).Value = received_by;
                    cmd.Parameters.Add("@comment", SqlDbType.VarChar, 255).Value = comment;
                    cmd.Parameters.Add("@sampleID", SqlDbType.Int).Direction = ParameterDirection.Output;

                    myConnection.Open();
                    cmd.ExecuteNonQuery();
                    return int.Parse(cmd.Parameters["@sampleID"].Value.ToString());
                }
            }
        }

        public static Boolean updateConcentration(string barcode, double conc)
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("spu_sample_add_property", myConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@barcode", SqlDbType.VarChar, 25).Value = barcode;
                    cmd.Parameters.Add("@property_name_id", SqlDbType.VarChar, 25).Value = "Conc";
                    cmd.Parameters.Add("@value", SqlDbType.Decimal, 10).Value = conc;

                    myConnection.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
        }

        public static void trackSampleToBox(Int32 sample_id,
                                            Int32 box_id,
                                            Char row,
                                            Int32 col
        )
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("spu_sample_box_sample_track", myConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@sample_id", SqlDbType.Int).Value = sample_id;
                    cmd.Parameters.Add("@box_id", SqlDbType.Int).Value = box_id;
                    cmd.Parameters.Add("@row", SqlDbType.Char).Value = row;
                    cmd.Parameters.Add("@col", SqlDbType.Int).Value = col;
                    cmd.Parameters.Add("@user", SqlDbType.VarChar, 20).Value = "tecan";

                    myConnection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static bool checkBoxExist(string box_barcode)
        {
            SqlConnection myConnection = new SqlConnection(connString);
            myConnection.Open();

            SqlCommand myCommand = new SqlCommand("SELECT count(*) as box_count " +
                                                " FROM sample_box" +
                                                " WHERE sample_box_barcode = '" + box_barcode + "'" +
                                                " AND storage_container_id = (" +
                                                "   SELECT storage_container_id" +
                                                "   FROM storage_container" +
                                                "   WHERE storage_container_name = '96well Plate'" +
                                                ")"
                                                , myConnection);

            if (int.Parse(myCommand.ExecuteScalar().ToString()) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool checkBoxIsEmpty(string box_barcode)
        {
            SqlConnection myConnection = new SqlConnection(connString);
            myConnection.Open();

            SqlCommand myCommand = new SqlCommand("SELECT count(*) as sample_count " +
                                                " FROM sample_box_sample" +
                                                " WHERE sample_box_id = (" +
                                                "   SELECT sample_box_id" +
                                                "   FROM sample_box" +
                                                "   WHERE sample_box_barcode = '" + box_barcode + "'" +
                                                ")" +
                                                " AND sample_id > 0 AND ukn_barcode IS NULL;"
                                                , myConnection);

            if (int.Parse(myCommand.ExecuteScalar().ToString()) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool checkSampleBarcodesExist(string prefix)
        {
            string lst = "";
            for (int i = 1; i <= 96; i++)
            {
                lst += ", '" + prefix + "-" + ScanReport.numberTo96wellRackPos(i) + "'";
            }
            lst = "(" + lst.Substring(1) + ")";

            SqlConnection myConnection = new SqlConnection(connString);
            myConnection.Open();

            SqlCommand myCommand = new SqlCommand("SELECT COUNT(*) as num" +
                                                " FROM sample" +
                                                " WHERE bar_code IN " + lst
                                                , myConnection);

            if (int.Parse(myCommand.ExecuteScalar().ToString()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Int32 getBoxID(string box_barcode)
        {
            SqlConnection myConnection = new SqlConnection(connString);
            myConnection.Open();

            SqlCommand myCommand = new SqlCommand("SELECT sample_box_id " +
                                                " FROM sample_box" +
                                                " WHERE sample_box_barcode = '" + box_barcode + "'"
                                                , myConnection);

            return int.Parse(myCommand.ExecuteScalar().ToString());

        }

        public static void getBoxSamples(string box_barcode, Dictionary<int, CSample> tubeBarcodes)
        {
            SqlConnection myConnection = new SqlConnection(connString);
            myConnection.Open();

            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand("SELECT bar_code, patient_id, s.sample_id, sc.categ_name_id, row, col" +
                                                " FROM sample s" +
                                                " JOIN sample_box_sample sbs ON s.sample_id = sbs.sample_id" +
                                                " JOIN sample_category sc ON s.category_id = sc.category_id" +
                                                " WHERE sbs.sample_box_id = (" +
                                                "   SELECT sample_box_id" +
                                                "   FROM sample_box" +
                                                "   WHERE sample_box_barcode = '" + box_barcode + "'" +
                                                ")"
                                                , myConnection);

            myReader = myCommand.ExecuteReader();
            Int32 idx = 0;
            if (myReader.HasRows)
                while (myReader.Read())
                {
                    idx = ScanReport.wellToNumber(char.Parse(myReader["row"].ToString()), int.Parse(myReader["col"].ToString()));
                    tubeBarcodes[idx] = new CSample("plate_well",myReader["bar_code"].ToString(), -1, idx, ScanReport.getWell(char.Parse(myReader["row"].ToString()), int.Parse(myReader["col"].ToString())));
                    tubeBarcodes[idx].setPatientAndSample(int.Parse(myReader["patient_id"].ToString()), int.Parse(myReader["sample_id"].ToString()));
                    tubeBarcodes[idx].setCategory(myReader["categ_name_id"].ToString());
                }
            myReader.Close();
        }
    }
}
