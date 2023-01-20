using System;
using System.IO;
using System.Text;
using System.Net.NetworkInformation;

namespace PrinterInstall
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                //Paths of batch file location and driver location for Brother HL-6250dw
                string path = @"\\filesrv2\Admin\software\Vision\VisionScripts\PrinterInstall\AddPrinter.bat";
                string driver = "pnputil.exe -i -a \"\\\\filesrv2\\admin\\Software\\Printers_Scanners\\Brother HL-6250dw\\*.inf\"";

                //Prompts the user to enter Printer Host Name
                Console.Write("Enter Printer Name:  ");
                string printerName = Console.ReadLine();

                //Paths to scripts for port creation and printer installation
                string setPort = "cscript \"C:\\Windows\\System32\\Printing_Admin_Scripts\\en - US\\Prnport.vbs\" -a -r " + printerName + " -h " + printerName + " -o raw -n 9100";
                string setName = "cscript \"C:\\Windows\\System32\\Printing_Admin_Scripts\\en - US\\prnmngr.vbs\" -a -p \"" + printerName + "\" -m \"Brother HL-L6250DW series\" -r \"" + printerName + "\"";
                string setDefault = "cscript \"C:\\Windows\\System32\\Printing_Admin_Scripts\\en - US\\prnmngr.vbs\" -t -p \"" + printerName + "\"";

                string[] lines = { driver, setPort, setName, setDefault };

                Ping myPing = new Ping();

                //Attempts to ping printer by host name and re create batch file after successful ping
                try
                {
                    PingReply reply = myPing.Send(printerName, 1000);
                    if (reply.Status == IPStatus.Success)
                    {
                        File.WriteAllLines(path, lines);
                        break;
                    }
                }

                //If unable to ping printer, the program ask for input of name again until it can successfully ping or program is closed 
                catch (PingException e)
                {
                    Console.WriteLine("Check Printer Name. Unable to locate\n");
                }
            }



   


        }
    }
}
