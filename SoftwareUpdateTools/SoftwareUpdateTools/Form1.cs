using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftwareUpdateTools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
            Process.GetCurrentProcess().Kill();
        }

        private void TroyeeVatSolutionsUpdate_Click(object sender, EventArgs e)
        {
            if (textBoxApplicationPath.Text == "")
            {
                MessageBox.Show("Please Write the Path");
            }
            else
            {
                try
                {
                    SaveSatting();
                    //var versionInfo = FileVersionInfo.GetVersionInfo(@"\\192.168.1.4\Share Folder\Land_Cant_Fund.exe");
                    //string versionServer = versionInfo.ProductVersion;
                    String currentDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                    //MessageBox.Show("" + currentDirectory);
                    /*Assembly assembly = Assembly.GetExecutingAssembly();
                    FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                    string version1 = fileVersionInfo.ProductVersion;)*/

                    //var versionInfoCurrentDirectory = FileVersionInfo.GetVersionInfo(currentDirectory + "\\Land_Cant_Fund.exe");
                    //string versionCurrent = versionInfoCurrentDirectory.ProductVersion;
                    //MessageBox.Show("Server V " + versionServer + " Current P V" + versionCurrent);

                    var macAddr = (from nic in NetworkInterface.GetAllNetworkInterfaces()
                                   where nic.OperationalStatus == OperationalStatus.Up
                                   select nic.GetPhysicalAddress().ToString()
                                        ).FirstOrDefault();

                    string Ipaddress = localIPAddress();

                    string name = System.Environment.MachineName;
                    DateTime pcDate = DateTime.Now;

                    //if (versionServer != versionCurrent)
                    //{
                    string sourceDirectory = textBoxApplicationPath.Text;
                    //string sourceDirectory = @"\\192.168.1.4\Share Folder";
                    //string sourceDirectory = @"\\3BL-9\Software\Practise";
                    string targetDirectory = currentDirectory;
                    Copy(sourceDirectory, targetDirectory);
                    MessageBox.Show("Software has been Updated");
                    getSatting();
                    string lines = "Mac Address : " + macAddr + " \r\nIp Address: " + Ipaddress + "\r\nPc Name: " + name + "\r\nDate: " + pcDate + " ";

                    // Write the string to a file.
                    System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\VatUpdateInfo.txt");
                    file.WriteLine(lines);

                    file.Close();
                    //saveInformation();
                    //}


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void getSatting()
        {
            textBoxApplicationPath.Text = Properties.Settings.Default.PathName;
        }

        private void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }
        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        private string localIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
            {
                localIP = ip.ToString();

                string[] temp = localIP.Split('.');

                if (ip.AddressFamily == AddressFamily.InterNetwork && temp[0] == "192")
                {
                    break;
                }
                else
                {
                    localIP = null;
                }
            }

            return localIP;
        }

        private void SaveSatting()
        {
            Properties.Settings.Default.PathName = textBoxApplicationPath.Text;
            Properties.Settings.Default.Save();
        }
    }
}
