using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using MediaDevices;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using ChoETL;
using System.Windows.Media.Animation;
using System.ComponentModel;
using BauhofWMSDLL.ListDefinitions;
using System.Configuration;
using System.Net.Mail;

namespace BauhofOffline
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BackgroundWorker bw = new BackgroundWorker();
        List<ListOfShopRelations> lstShopRelations = new List<ListOfShopRelations>();
        List<ListOfSettings> lstSettings = new List<ListOfSettings>();
        public MainWindow()
        {
            InitializeComponent();
            prgRing.Visibility = Visibility.Hidden;
            GetConfiguration();
            CheckVersion();
            ConvertFilesStart();
        }

        private void GetConfiguration()
        {
            try
            {
                WriteLog("GetConfiguration started", 1);
                lstSettings = new List<ListOfSettings>();
                var row = new ListOfSettings();

                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;
                WriteLog("GetConfiguration iterating through configuration", 2);
                if (confCollection["adminEmail"] != null)
                {
                    row.adminEmail = confCollection["adminEmail"].Value.ToString();
                    WriteLog("GetConfiguration adminEmail = " + row.adminEmail, 2);
                }
                if (confCollection["csvFolder"] != null)
                {
                    row.csvFolder = confCollection["csvFolder"].Value.ToString();
                    WriteLog("GetConfiguration csvFolder = " + row.csvFolder, 2);
                }
                if (confCollection["jsonFolder"] != null)
                {
                    row.jsonFolder = confCollection["jsonFolder"].Value.ToString();
                    WriteLog("GetConfiguration jsonFolder = " + row.jsonFolder, 2);
                }
                if (confCollection["logFolder"] != null)
                {
                    row.logFolder = confCollection["logFolder"].Value.ToString();
                    WriteLog("GetConfiguration logFolder = " + row.logFolder, 2);
                }
                if (confCollection["debugLevel"] != null)
                {
                    row.debugLevel = Convert.ToInt32(confCollection["debugLevel"].Value.ToString());
                    WriteLog("GetConfiguration debugLevel = " + row.debugLevel, 2);
                }
                lstSettings.Add(row);
                WriteLog("GetConfiguration complete", 1);
                WriteError("GetConfiguration " + "TEST" + " " + "test2");
            }
            catch (Exception ex)
            {
                WriteError("GetConfiguration " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
                MessageBox.Show("GetConfiguration  " + ex.Message);
            }
        }


        public async void WriteLog(string message, int logLevel)
        {
            try
            {
                if (lstSettings.Any())
                {
                    if (logLevel >= lstSettings.First().debugLevel)
                    {
                        string logToWrite = "" + "\r\n" +
                        String.Format("{0:dd.MM.yyyy HH:mm:ss}", DateTime.Now) + "\r\n" +
                        "Hostname: " + Environment.MachineName + " Username:" + Environment.UserName + "\r\n" +
                        "" + "\r\n" +
                        message;

                        if (!Directory.Exists(lstSettings.First().logFolder))
                        {
                            Directory.CreateDirectory(lstSettings.First().logFolder);
                        }
                        var str = new StreamWriter(lstSettings.First().logFolder + Environment.MachineName + "_Log.txt", true);
                        str.WriteLine(logToWrite);
                        str.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("WriteLog " + lstSettings.First().logFolder + " " + "\r\n"  + ex.Message);
            }
        }

        public async void WriteError(string message)
        {
            try
            {
                string errroToWrite = "" + "\r\n" +
                    String.Format("{0:dd.MM.yyyy HH:mm:ss}", DateTime.Now) + "\r\n" +
                    "Hostname: " + Environment.MachineName + " Username:" + Environment.UserName + "\r\n" +
                    "" + "\r\n" +
                    message + "\r\n" +
                    "================"; 

                var str = new StreamWriter(lstSettings.First().logFolder + Environment.MachineName + "_Error.txt", true);
                str.WriteLine(errroToWrite);
                str.Close();

                if (!string.IsNullOrEmpty(lstSettings.First().adminEmail))
                {
                    var mail = new MailMessage();
                    var SmtpServer = new SmtpClient("mail.neti.ee");
                    mail.From = new MailAddress("bauhofoffline@bauhof.ee");
                    mail.To.Add(lstSettings.First().adminEmail);

                    mail.Subject = "BauhofOffline error from " + Environment.MachineName;
                    mail.Body = errroToWrite;
                    //SmtpServer.Send(mail);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void CheckVersion()
        {
            try
            {
                WriteLog("CheckVersion started", 1);
                txtBkLatestVersionValue.Text = "";
                txtBkScannerVersionValue.Text = "";
                btnUpdate.Visibility = Visibility.Collapsed;
                string DeviceNameAsSeenInMyComputer = "";
                MediaDevice device = null;
                WriteLog("trying to connect...", 2);
                var devices = MediaDevice.GetDevices();
                if (devices.Any())
                {
                    if (devices.Count() == 1)
                    {
                        device = devices.First();
                        DeviceNameAsSeenInMyComputer = devices.First().Description;
                        WriteLog("CheckVersion DeviceNameAsSeenInMyComputer " + DeviceNameAsSeenInMyComputer, 2);
                    }
                    else
                    {
                        
                        txtBkStatus.Text = "LEITI ROHKEM KUI 1 ÜHENDATUD VÄLINE SEADE!";
                        WriteLog("CheckVersion " + txtBkStatus.Text, 2);
                        MessageBox.Show("LEITI ROHKEM KUI 1 ÜHENDATUD VÄLINE SEADE!");
                    }
                }
                else
                {
                    txtBkStatus.Text = "VÄLIST SEADET EI LEITUD!";
                    WriteLog("CheckVersion " + txtBkStatus.Text, 2);
                    MessageBox.Show("VÄLIST SEADET EI LEITUD!");
                }
                Debug.WriteLine("DeviceNameAsSeenInMyComputer " + DeviceNameAsSeenInMyComputer);
                if (!string.IsNullOrEmpty(DeviceNameAsSeenInMyComputer))
                {
                    bool proceed = true;
                    int count = 0;
                    try
                    {
                        device.Connect();
                        WriteLog("CheckVersion device connected", 2);
                        var photoDir = device.GetDirectoryInfo(@"\Internal shared storage\Download");
                        var files = photoDir.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly);
                        foreach (var file in files)
                        {
                           
                            if (file.Name.ToUpper() == "VERSION.TXT")
                            {
                                WriteLog(@"CheckVersion VERSION.TXT found in device Internal shared storage\Download", 2);
                                count = count + 1;
                                string destinationFileName = $@"c:\windows\temp\{file.Name}";
                                if (File.Exists(destinationFileName))
                                {
                                    File.Delete(destinationFileName);
                                    WriteLog(@"CheckVersion VERSION.TXT deleted from c:\windows\temp\", 2);
                                }

                                if (!File.Exists(destinationFileName))
                                {
                                    using (FileStream fs = new FileStream(destinationFileName, FileMode.Create, System.IO.FileAccess.Write))
                                    {
                                        device.DownloadFile(file.FullName, fs);
                                        WriteLog(@"CheckVersion VERSION.TXT downloaded from the device to c:\windows\temp\", 2);
                                    }
                                }
                                else
                                {
                                    proceed = false;
                                    WriteLog(@"CheckVersion SKÄNNERI VERSIOONI EI ÕNNESTUNUD KONTROLLIDA: " + @"c:\windows\temp\version.txt EI LEITUD", 2);
                                    MessageBox.Show("SKÄNNERI VERSIOONI EI ÕNNESTUNUD KONTROLLIDA: " + @"c:\windows\temp\version.txt EI LEITUD");
                                    
                                }
                                if (proceed)
                                {
                                    if (File.Exists(destinationFileName))
                                    {
                                        string text = System.IO.File.ReadAllText(destinationFileName);
                                        if (!string.IsNullOrEmpty(text))
                                        {
                                            txtBkScannerVersionValue.Text = text;
                                            WriteLog(@"CheckVersion SKÄNNERI VERSIOON: " + txtBkScannerVersionValue.Text, 2);
                                        }
                                        else
                                        {
                                            proceed = false;
                                            WriteLog(@"CheckVersion SKÄNNERI VERSIOONI EI ÕNNESTUNUD KONTROLLIDA: " + @"c:\windows\temp\version.txt ON TÜHI!", 2);
                                            MessageBox.Show("SKÄNNERI VERSIOONI EI ÕNNESTUNUD KONTROLLIDA: " + @"c:\windows\temp\version.txt ON TÜHI!");
                                        }
                                    }
                                    else
                                    {
                                        proceed = false;
                                        WriteLog(@"CheckVersion SKÄNNERI VERSIOONI EI ÕNNESTUNUD KONTROLLIDA: " + @"c:\windows\temp\version.txt EI LEITUD", 2);
                                        MessageBox.Show("SKÄNNERI VERSIOONI EI ÕNNESTUNUD KONTROLLIDA: " + @"c:\windows\temp\version.txt EI LEITUD");
                                    }
                                }
                            }
                        }

                        if (proceed)
                        {
                            string destinationLatestFileName = $@"c:\windows\temp\BauhofWMSVersion.txt";
                            if (File.Exists(destinationLatestFileName))
                            {
                                File.Delete(destinationLatestFileName);
                                WriteLog(@"CheckVersion deleted c:\windows\temp\BauhofWMSVersion.txt", 2);
                            }

                            try
                            {
                                WebClient webClient = new WebClient();
                                webClient.DownloadFile("http://www.develok.ee/BauhofWMS/Install/BauhofWMSVersion.txt", destinationLatestFileName);
                                WriteLog(@"CheckVersion file downloaded from http://www.develok.ee/BauhofWMS/Install/BauhofWMSVersion.txt", 2);
                            }
                            catch (Exception ex)
                            {
                                proceed = false;
                                WriteLog(@"CheckVersion VERSIOONIUUENDUST EI ÕNNESTUNUD KONTROLLIDA.FAILI EI LEITUD!", 2);
                                MessageBox.Show("VERSIOONIUUENDUST EI ÕNNESTUNUD KONTROLLIDA. FAILI EI LEITUD!" + "\r\n" + ex.Message);
                            }
                            
                            if (proceed)
                            {
                                if (File.Exists(destinationLatestFileName))
                                {
                                    string text = System.IO.File.ReadAllText(destinationLatestFileName);
                                    if (!string.IsNullOrEmpty(text))
                                    {
                                        WriteLog(@"CheckVersion " + destinationLatestFileName + " loetud: " + "\r\n" + text, 2);
                                        var textSplit = text.Split(new[] { "#_#" }, StringSplitOptions.None);
                                        if (textSplit.Any())
                                        {
                                            foreach (var s in textSplit)
                                            {
                                                if (s.StartsWith("MAJOR:"))
                                                {
                                                    txtBkLatestVersionValue.Text = s.Replace("MAJOR:", "");
                                                    WriteLog(@"CheckVersion version:" + txtBkLatestVersionValue.Text, 2);
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        proceed = false;
                                        WriteLog(@"CheckVersion VERSIOONIUUENDUST EI ÕNNESTUNUD KONTROLLIDA: " + @"c:\windows\temp\BauhofWMSVersion.txt ON TÜHI!", 2);
                                        MessageBox.Show("VERSIOONIUUENDUST EI ÕNNESTUNUD KONTROLLIDA: " + @"c:\windows\temp\BauhofWMSVersion.txt ON TÜHI!");
                                    }
                                }
                                else
                                {
                                    proceed = false;
                                    WriteLog(@"CheckVersion VERSIOONIUUENDUST EI ÕNNESTUNUD KONTROLLIDA: " + @"c:\windows\temp\BauhofWMSVersion.txt EI LEITUD", 2);
                                    MessageBox.Show("VERSIOONIUUENDUST EI ÕNNESTUNUD KONTROLLIDA: " + @"c:\windows\temp\BauhofWMSVersion.txt EI LEITUD");
                                }
                            }
                        }
                        if (proceed)
                        {
                            Debug.WriteLine(txtBkLatestVersionValue.Text + "  " + txtBkScannerVersionValue.Text);
                            if (!string.IsNullOrEmpty(txtBkLatestVersionValue.Text) && !string.IsNullOrEmpty(txtBkScannerVersionValue.Text))
                            {
                                var splitversionValueLatest = txtBkLatestVersionValue.Text.Split(new[] { "." }, StringSplitOptions.None);
                                var versionValueLatest = Convert.ToInt32(splitversionValueLatest[2]);

                                var splitversionValueScanner = txtBkScannerVersionValue.Text.Split(new[] { "." }, StringSplitOptions.None);
                                var versionValueScanner = Convert.ToInt32(splitversionValueScanner[2]);

                                if (versionValueLatest > versionValueScanner)
                                {
                                    btnUpdate.Visibility = Visibility.Visible;
                                    txtBkStatus.Text = "LEITI UUS VERSIOON!";
                                    WriteLog(@"CheckVersion " + txtBkStatus.Text, 2);
                                }
                            }
                        }
                        if (proceed)
                        {
                            device.Connect();
                            WriteLog("CheckVersion device connected", 2);
                            var DCIMDir = device.GetDirectoryInfo(@"\Internal shared storage\DCIM\");
                            var folders = DCIMDir.EnumerateDirectories("*.*", SearchOption.TopDirectoryOnly);
                            {
                                bool exists = false;
                                foreach (var s in folders)
                                {
                                    if (s.Name == "Export")
                                    {
                                        exists = true;
                                    }
                                }
                                if (!exists)
                                {
                                    device.CreateDirectory(@"\Internal shared storage\DCIM\Export");
                                    WriteLog(@"CheckVersion device \Internal shared storage\DCIM\Export folder created", 2);
                                }
                            }
                            
                            var exportDir = device.GetDirectoryInfo(@"\Internal shared storage\DCIM\Export");
                            var exportFiles = exportDir.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly);
                            if (exportFiles.Count() < 3)
                            {
                                foreach (var file in exportFiles)
                                {
                                    txtBkDownloadFiles.Text = txtBkDownloadFiles.Text + file.Name + "\r\n";
                                    WriteLog(@"CheckVersion found " + txtBkDownloadFiles.Text, 2);
                                }
                            }
                        }
                        device.Disconnect();
                        WriteLog(@"CheckVersion device disconnect", 2);
                    }
                    catch (Exception ex)
                    {
                        proceed = false;
                        if (ex.Message.Contains(@"\Internal shared storage\Download") && ex.Message.Contains("not found"))
                        {
                            WriteError("GetConfiguration ÜHENDATUD SEADMELT EI LEITUD DOWNLOAD KATALOOGI!" + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
                            MessageBox.Show("GetConfiguration  ÜHENDATUD SEADMELT EI LEITUD DOWNLOAD KATALOOGI!");
                        }
                        else
                        {
                            WriteError("GetConfiguration " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
                            MessageBox.Show("GetConfiguration  " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError("CheckVersion " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
                MessageBox.Show("CheckVersion  " + ex.Message);
            }
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WriteLog(@"btnDownload_Click started", 2);
                string DeviceNameAsSeenInMyComputer = "";
                MediaDevice device = null;
                var devices = MediaDevice.GetDevices();
                if (devices.Any())
                {
                    if (devices.Count() == 1)
                    {
                        device = devices.First();
                        DeviceNameAsSeenInMyComputer = devices.First().Description;
                        WriteLog(@"btnDownload_Click DeviceNameAsSeenInMyComputer:" + DeviceNameAsSeenInMyComputer, 2);
                    }
                    else
                    {
                        WriteLog(@"btnDownload_Click LEITI ROHKEM KUI 1 ÜHENDATUD VÄLINE SEADE!", 2);
                        MessageBox.Show("LEITI ROHKEM KUI 1 ÜHENDATUD VÄLINE SEADE!");
                    }

                }
                else
                {
                    WriteLog(@"btnDownload_Click VÄLIST SEADET EI LEITUD!", 2);
                    MessageBox.Show("VÄLIST SEADET EI LEITUD!");
                }
                if (!string.IsNullOrEmpty(DeviceNameAsSeenInMyComputer))
                {
                    bool proceed = true;
                    int count = 0;

                    try
                    {


                        var photoDir = device.GetDirectoryInfo(@"\Internal shared storage\DCIM\Export");
                        var files = photoDir.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly);

                        foreach (var file in files)
                        {
                            count = count + 1;
                            string destinationFileName = lstSettings.First().jsonFolder + file.Name;
                            if (!File.Exists(destinationFileName))
                            {
                                using (FileStream fs = new FileStream(destinationFileName, FileMode.Create, System.IO.FileAccess.Write))
                                {
                                    device.DownloadFile(file.FullName, fs);
                                    WriteLog(@"btnDownload_Click downloaded " + file.FullName, 2);
                                }
                            }
                        }
                        device.Disconnect();
                        WriteLog(@"btnDownload_Click device disconnected", 2);
                    }
                    catch (Exception ex)
                    {
                        proceed = false;
                        if (ex.Message.Contains(@"\Internal shared storage\DCIM") && ex.Message.Contains("not found"))
                        {
                            WriteError("btnDownload_Click ÜHENDATUD SEADMELT EI LEITUD DOWNLOAD KATALOOGI!" + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
                            MessageBox.Show("btnDownload_Click  ÜHENDATUD SEADMELT EI LEITUD DOWNLOAD KATALOOGI!");
                        }
                        else
                        {
                            WriteError("btnDownload_Click " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
                            MessageBox.Show("btnDownload_Click  " + ex.Message);
                        }
                    }
                    if (proceed)
                    {
                        WriteLog(@"btnDownload_Click  " + count.ToString() + (count == 1 ? " FAIL SKÄNNERIST LAETUD" : " FAILI SKÄNNERIST LAETUD!"), 2);
                        MessageBox.Show(count.ToString() + (count == 1 ? " FAIL SKÄNNERIST LAETUD" : " FAILI SKÄNNERIST LAETUD!"));
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError("btnDownload_Click " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
                MessageBox.Show("btnDownload_Click  " + ex.Message);
            }
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            WriteLog(@"btnUpload_Click started", 2);
            UploadFilesStart();
        }

        public void UploadFilesStart()
        {
            try
            {
                var backgroundWorker = new BackgroundWorker
                {

                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };
                backgroundWorker.DoWork += bw_DoWork_UploadFiles;
                backgroundWorker.RunWorkerCompleted += bw_RunWorkerCompleted_UploadFiles;
                backgroundWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                WriteError("UploadFilesStart " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
                MessageBox.Show("UploadFilesStart  " + ex.Message);
            }
        }
        
        private void bw_DoWork_UploadFiles(object sender, DoWorkEventArgs e)
        {
            try
            {
                WriteLog(@"bw_DoWork_UploadFiles started", 2);
                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
                {
                    txtBkStatus.Text = "Kopeerin faile skännerisse!";
                    prgRing.Visibility = Visibility.Visible;
                }));

                string DeviceNameAsSeenInMyComputer = "";
                MediaDevice device = null;
                var devices = MediaDevice.GetDevices();
                if (devices.Any())
                {
                    if (devices.Count() == 1)
                    {
                        device = devices.First();
                        DeviceNameAsSeenInMyComputer = devices.First().Description;
                    }
                    else
                    {
                        MessageBox.Show("LEITI ROHKEM KUI 1 ÜHENDATUD VÄLINE SEADE!");
                    }

                }
                else
                {
                    MessageBox.Show("VÄLIST SEADET EI LEITUD!");
                }
                if (!string.IsNullOrEmpty(DeviceNameAsSeenInMyComputer))
                {
                    bool proceed = true;
                    int count = 0;
                    try
                    {
                        device.Connect();

                        var photoDir = device.GetDirectoryInfo(@"\Internal shared storage\DCIM\");
                        string[] dirs = Directory.GetFiles(lstSettings.First().jsonFolder);
                        foreach (string str in dirs)
                        {

                            string sourceFileName = str;
                            int index = str.LastIndexOf("\\");
                            string fileName = str.Substring(index + 1);
                            if (fileName.EndsWith(".txt"))
                            {
                                try
                                {
                                    device.DeleteFile(photoDir.FullName + @"\" + fileName);
                                }
                                catch (Exception es)
                                {

                                }
                                count = count + 1;
                                device.UploadFile(sourceFileName, photoDir.FullName + @"\" + fileName);
                            }
                        }

                        device.Disconnect();
                    }
                    catch (Exception ex)
                    {
                        proceed = false;
                        if (ex.Message.Contains("already exists"))
                        {
                            MessageBox.Show("FAIL ON JUBA SKÄNNERIL OLEMAS!");
                        }
                        else
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    if (proceed)
                    {
                        Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
                        {
                            MessageBox.Show(count.ToString() + (count == 1 ? " FAIL SKÄNNERISSE LAETUD" : " FAILI SKÄNNERISSE LAETUD!"));
                        }));
                        
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError("bw_DoWork_UploadFiles " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
                MessageBox.Show("bw_DoWork_UploadFiles  " + ex.Message);
            }
        }

        private void bw_RunWorkerCompleted_UploadFiles(object sender, RunWorkerCompletedEventArgs e)
        {
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
            {
                txtBkStatus.Text = "";
                prgRing.Visibility = Visibility.Hidden;
            }));
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool proceed = true;
                string androidVersion = "";
                var splitlatest = txtBkLatestVersionValue.Text.Split(new[] { "." }, StringSplitOptions.None);
                if (splitlatest.Any())
                {
                    foreach (var r in splitlatest)
                    {
                        androidVersion = androidVersion + ".x" + r;
                    }
                }

                string destinationFile = @"c:\windows\temp\BauhofWMS." + androidVersion + ".apk";

                try
                {
                    if (File.Exists(destinationFile))
                    {
                        File.Delete(destinationFile);
                    }
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile("http://www.develok.ee/BauhofWMS/Install/BauhofWMS" + androidVersion + ".apk", destinationFile);
                }
                catch (Exception ex)
                {
                    proceed = false;
                    MessageBox.Show("UUENDUSFAILI LAADIMINE EBAÕNNESTUS" + "\r\n" + "http://www.develok.ee/BauhofWMS/Install/BauhofWMS" + androidVersion + ".apk" + "\r\n" + ex.Message);
                }
                if (proceed)
                {
                    if (File.Exists(destinationFile))
                    {
                        string DeviceNameAsSeenInMyComputer = "";
                        MediaDevice device = null;
                        var devices = MediaDevice.GetDevices();
                        if (devices.Any())
                        {
                            if (devices.Count() == 1)
                            {
                                device = devices.First();
                                DeviceNameAsSeenInMyComputer = devices.First().Description;
                            }
                            else
                            {
                                txtBkStatus.Text = "LEITI ROHKEM KUI 1 ÜHENDATUD VÄLINE SEADE!";
                            }

                        }
                        else
                        {
                            txtBkStatus.Text = "VÄLIST SEADET EI LEITUD!";
                        }
                        if (!string.IsNullOrEmpty(DeviceNameAsSeenInMyComputer))
                        {
                            device.Connect();
                            var dcimDownload = device.GetDirectoryInfo(@"\Internal shared storage\Download");
                            var files = dcimDownload.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly);
                            foreach (var file in files)
                            {
                                if (file.Name.ToUpper().EndsWith(".APK"))
                                {
                                    try
                                    {
                                        device.DeleteFile(dcimDownload.FullName + @"\" + file.Name);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("SKÄNNERIL EI ÕNNESTUNUD KUSTUTADA FAILI:" + "\r\n" + dcimDownload.FullName + @"\" + "BauhofWMS" + androidVersion + ".apk" + "\r\n" + ex.Message);
                                    }
                                }
                            }
                            device.UploadFile(destinationFile, dcimDownload.FullName + @"\" + "BauhofWMS" + androidVersion + ".apk");
                            device.Disconnect();
                            btnUpdate.Visibility = Visibility.Collapsed;
                            txtBkStatus.Text = "";
                            MessageBox.Show("UUENDUS ON SKÄNNERISSE LAETUD!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("SKÄNNERISSE SAADETAVAT FAILI EI LEITUD:" + "\r\n" + destinationFile);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError("btnUpdate_Click " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
                MessageBox.Show("btnUpdate_Click  " + ex.Message);
            }
           
        }

        public string ConvertCsvFileToJsonObject(string folderPath, string inputFileName, DateTime inputFileDate)
        {
            try
            {
                Debug.WriteLine(folderPath);
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                string filename = folderPath + inputFileName;
                if (!Directory.Exists(folderPath + @"\Archive\"))
                {
                    Directory.CreateDirectory(folderPath + @"\Archive\");
                }
                if (!File.Exists(folderPath + @"\Archive\" + inputFileName.Replace(".csv", ".convert")))
                {
                    File.WriteAllText(folderPath + @"\Archive\" + inputFileName.Replace(".csv", ".convert"), DateTime.Now.ToString());
                    List<ListOfdbRecordsImport> values = File.ReadAllLines(folderPath + inputFileName).Skip(1).Select(v => FromCsv(v, inputFileName, inputFileDate, lstSettings.First().logFolder, lstSettings.First().adminEmail)).ToList();

                    values.Where(x => string.IsNullOrEmpty(x.SKUqty)).ToList().ForEach(x => x.SKUqty = "0");
                    values.Where(x => string.IsNullOrEmpty(x.profiklubihind)).ToList().ForEach(x => x.profiklubihind = "0");
                    values.Where(x => string.IsNullOrEmpty(x.price)).ToList().ForEach(x => x.price = "0");
                    values.Where(x => string.IsNullOrEmpty(x.meistriklubihind)).ToList().ForEach(x => x.meistriklubihind = "0");
                    values.Where(x => string.IsNullOrEmpty(x.soodushind)).ToList().ForEach(x => x.soodushind = "0");

                    string outputFile = inputFileName.Replace(".csv", ".txt");
                    string json = JsonConvert.SerializeObject(values);
                    File.WriteAllText(lstSettings.First().jsonFolder + outputFile, json);
                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                    Debug.WriteLine("elapsed: " + elapsedTime + " rows:" + values.Count());
                    //MessageBox.Show("elapsed: " + elapsedTime + " rows:" + values.Count());
                    File.Move(filename, folderPath + @"\Archive\" + inputFileName);
                }

                return "";
            }
            catch (Exception ex)
            {
                WriteError("ConvertCsvFileToJsonObject " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
                MessageBox.Show("ConvertCsvFileToJsonObject  " + ex.Message);
                return "";
            }
        }

        public void ConvertFilesStart()
        {
            try
            {
                var backgroundWorker = new BackgroundWorker
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };
                backgroundWorker.DoWork += bw_DoWork_ConvertFiles;
                backgroundWorker.RunWorkerCompleted += bw_RunWorkerCompleted_ConvertFiles;
                backgroundWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                WriteError("ConvertFilesStart " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
                MessageBox.Show("ConvertFilesStart  " + ex.Message);
            }
        }

        private void bw_DoWork_ConvertFiles(object sender, DoWorkEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
                {
                    txtBkStatus.Text = "Konverteerin leitud andmebaasi skänneri andmebaasiks!";
                    prgRing.Visibility = Visibility.Visible;
                }));

                string csvFolderPath = lstSettings.First().csvFolder;
                string jsonFolderPath = lstSettings.First().jsonFolder;
                string jsonArchiveFolder = lstSettings.First().jsonFolder + @"\Archive\";
                if (!Directory.Exists(jsonArchiveFolder))
                {
                    Directory.CreateDirectory(jsonArchiveFolder);
                }
                string relationFileName = "ShopRelations.csv";
                if (!Directory.Exists(csvFolderPath))
                {
                    Directory.CreateDirectory(csvFolderPath);
                }

                if (File.Exists(csvFolderPath + relationFileName))
                {
                    lstShopRelations = File.ReadAllLines(csvFolderPath + relationFileName).Skip(1).Select(v => FromShopRelationCsv(v, relationFileName, lstSettings.First().logFolder, lstSettings.First().adminEmail)).ToList();
                    string[] dirs = Directory.GetFiles(csvFolderPath);
                    if (dirs.Any())
                    {
                        foreach (string str in dirs)
                        {
                            string sourceFileName = str;
                            int index = str.LastIndexOf("\\");
                            string fileName = str.Substring(index + 1);
                            if (fileName.EndsWith(".csv"))
                            {
                                if (fileName.ToUpper() == relationFileName.ToUpper())
                                {
                                    string json = JsonConvert.SerializeObject(lstShopRelations);
                                    File.WriteAllText(jsonFolderPath + relationFileName.Replace(".csv", ".txt"), json);
                                }
                                else
                                {
                                    var fileNameSplitPrefix = fileName.Split(new[] { "_" }, StringSplitOptions.None);
                                    var prefixToSearch = fileNameSplitPrefix[0];
                                    Debug.WriteLine("Prefix is " + prefixToSearch);
                                    string[] dirsPrefixSearch = Directory.GetFiles(jsonFolderPath);
                                    foreach (string str2 in dirsPrefixSearch)
                                    {
                                        Debug.WriteLine("fileName is " + str2);
                                        int index2 = str2.LastIndexOf("\\");
                                        string fileName2 = str2.Substring(index2 + 1);
                                        if (fileName2.StartsWith(prefixToSearch))
                                        {
                                            Debug.WriteLine("fileName is " + fileName);
                                            Debug.WriteLine("fileName2 is " + fileName2);
                                            if (fileName2 != fileName)
                                            {
                                                File.Move(jsonFolderPath + fileName2, jsonArchiveFolder + fileName2);
                                            }
                                        }
                                    }

                                    if (!File.Exists(jsonFolderPath + fileName))
                                    {
                                        var fileNameSplit = fileName.Split(new[] { "_PDA_Products_" }, StringSplitOptions.None);
                                        string datePart = fileNameSplit[1].Replace(".csv", "").Replace("-", "");
                                        string formatstring = "yyyyMMddHHmmss";
                                        DateTime fileDate = DateTime.ParseExact(datePart, formatstring, null);
                                        ConvertCsvFileToJsonObject(csvFolderPath, fileName, fileDate);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("bw_DoWork_ConvertFiles: csv faile ei leitud kataloogist " + csvFolderPath);
                    }
                    
                }
                else
                {
                    MessageBox.Show("bw_DoWork_ConvertFiles: ShopRelations.csv nimeline fail puudub csv kataloogist " + csvFolderPath + "!");
                }

              
            }
            catch (Exception ex)
            {
                WriteError("bw_DoWork_ConvertFiles: " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
                MessageBox.Show("bw_DoWork_ConvertFiles: " + ex.Message);
            }
        }

        private void bw_RunWorkerCompleted_ConvertFiles(object sender, RunWorkerCompletedEventArgs e)
        {
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
            {
                txtBkStatus.Text = "";
                prgRing.Visibility = Visibility.Hidden;
            }));
        }

        public static ListOfShopRelations FromShopRelationCsv(string csvLine, string fileName, string logFolder, string adminEmail)
        {
            ListOfShopRelations lst = new ListOfShopRelations();
            try
            {
                string[] values = csvLine.Split(',');
                lst.shopID = values[0];
                lst.shopName = values[1];
                return lst;
            }
            catch (Exception ex)
            {
                string message = ("FromShopRelationCsv " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
                string errroToWrite = "" + "\r\n" +
                    String.Format("{0:dd.MM.yyyy HH:mm:ss}", DateTime.Now) + "\r\n" +
                    "Hostname: " + Environment.MachineName + " Username:" + Environment.UserName + "\r\n" +
                    "" + "\r\n" +
                    message + "\r\n" +
                    "================";

                var str = new StreamWriter(logFolder + Environment.MachineName + "_Error.txt", true);
                str.WriteLine(errroToWrite);
                str.Close();

                if (!string.IsNullOrEmpty(adminEmail))
                {
                    var mail = new MailMessage();
                    var SmtpServer = new SmtpClient("mail.neti.ee");
                    mail.From = new MailAddress("bauhofoffline@bauhof.ee");
                    mail.To.Add(adminEmail);

                    mail.Subject = "BauhofOffline error from " + Environment.MachineName;
                    mail.Body = errroToWrite;
                    //SmtpServer.Send(mail);
                }

                MessageBox.Show("FromShopRelationCsv  " + ex.Message);
                return lst;
            }
        }

        public static ListOfdbRecordsImport FromCsv(string csvLine, string fileName, DateTime fileDate, string logFolder, string adminEmail)
        {
            ListOfdbRecordsImport lst = new ListOfdbRecordsImport();
            try
            {
                string[] values = csvLine.Split("\",");
                lst.fileName = fileName;
                lst.fileDate = fileDate;
                lst.itemCode = values[0];
                lst.itemDesc = values[1];
                lst.itemMagnitude = values[2];
                lst.price = !string.IsNullOrEmpty(values[3]) ? "0" : values[3];
                lst.SKU = values[4];
                lst.SKUqty = !string.IsNullOrEmpty(values[6]) ? "0" : values[6];
                lst.meistriklubihind = !string.IsNullOrEmpty(values[7]) ? "0" : values[7];
                lst.soodushind = !string.IsNullOrEmpty(values[8]) ? "0" : values[8];
                lst.profiklubihind = !string.IsNullOrEmpty(values[9]) ? "0" : values[9];
                lst.sortiment = values[10];
                
                lst.SKUBin = values[12];
                lst.barCode = values[13];

                
                return lst;
            }
            catch (Exception ex)
            {
                string message = ("FromCsv " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
                string errroToWrite = "" + "\r\n" +
                    String.Format("{0:dd.MM.yyyy HH:mm:ss}", DateTime.Now) + "\r\n" +
                    "Hostname: " + Environment.MachineName + " Username:" + Environment.UserName + "\r\n" +
                    "" + "\r\n" +
                    message + "\r\n" +
                    "================";

                var str = new StreamWriter(logFolder + Environment.MachineName + "_Error.txt", true);
                str.WriteLine(errroToWrite);
                str.Close();

                if (!string.IsNullOrEmpty(adminEmail))
                {
                    var mail = new MailMessage();
                    var SmtpServer = new SmtpClient("mail.neti.ee");
                    mail.From = new MailAddress("bauhofoffline@bauhof.ee");
                    mail.To.Add(adminEmail);

                    mail.Subject = "BauhofOffline error from " + Environment.MachineName;
                    mail.Body = errroToWrite;
                    //SmtpServer.Send(mail);
                }
                MessageBox.Show("FromCsv  " + ex.Message);
                return lst;
            }
        }

       
    }
}
