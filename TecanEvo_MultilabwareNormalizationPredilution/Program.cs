using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TecanEvo_MultilabwareNormalizationPredilution
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                MessageBox.Show("Error, no parameters provided!");
                return 2;
            }
            if (args[0] == "initialize")
            {
                /// args[1] = file name and path for variable export
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                InitializeForm mf = new InitializeForm(args);
                Application.Run(mf);
                return mf.returnValue();
            }
            else if (args[0] == "scan_report")
            {
                // args[1] = file name and path for variable import
                // args[2] = file name and path to source barcodes
                // args[3] = file name and path to destination barcodes
                // args[4] = file name and path to job info (output)
                // args[5] = file name and path to worklist (output)
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ScanReport sr = new ScanReport(args);
                Application.Run(sr);
                return sr.returnValue();
            }
            else if (args[0] == "write_data")
            {
                // args[1] = file name and path for variable import
                // args[2] = file name and path to job info (input)
                // args[3] = file name and path to worklist (input)
                // args[4] = file name and path to Lunatic output (input)
                // args[5] = file name and path to micronic volume output (input)
                // args[6] = path to backup folder
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                WriteData wd = new WriteData(args);
                return wd.returnValue();
            }
            else
            {
                MessageBox.Show("Error, unknown mode!");
                return 2;
            }
        }
    }
}
