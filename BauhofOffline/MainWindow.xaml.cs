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
using BauhofWMSDLL.ListDefinitions;
using System.Configuration;
using System.Net.Mail;
using BauhofOffline.Utils;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BauhofOffline
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int sKUCounter = 0;
        private readonly BackgroundWorker bw = new BackgroundWorker();
        private string startupArgs = App.mArgs;
        public bool ui = true;
        public ParseArguments ParseArguments = new ParseArguments();
        public string convertProcessLog = "";
        public bool convertProcess = true;
        public string language = "EN";

        #region lists
        public List<ListOfShopRelations> lstShopRelations = new List<ListOfShopRelations>();
        public List<ListOfSettings> lstSettings = new List<ListOfSettings>();

        public List<ListOfdbRecordsImport> lstDB01 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB02 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB03 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB04 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB05 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB06 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB07 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB08 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB09 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB10 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB11 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB12 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB13 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB14 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB15 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB16 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB17 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB18 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB19 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB20 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB21 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB22 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB23 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB24 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB25 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB26 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB27 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB28 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB29 = new List<ListOfdbRecordsImport>();
        public List<ListOfdbRecordsImport> lstDB30 = new List<ListOfdbRecordsImport>();

        public List<ListOfPurchaseReceive> lstPurchaseReceive = new List<ListOfPurchaseReceive>();
        public List<ListOfTransferReceive> lstTransferReceive = new List<ListOfTransferReceive>();
        #endregion

        #region Main
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                WriteLog("InitializeComponent done", 1);
                prgRing.Visibility = Visibility.Hidden;
                WriteLog("prgRing visible", 1);


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
                    Debug.WriteLine("Sii");
                    device.Connect();
                    if (device.DirectoryExists(@"\Internal shared storage"))
                    {
                        language = "EN";
                    }
                    else
                    {
                        language = "ET";
                    }
                }
                if (startupArgs != null)
                {
                    WriteLog("Started with arguments: " + startupArgs, 1);
                    convertProcessLog = "Started with arguments: " + startupArgs;
                    convertProcessLog = convertProcessLog + "\r\n" + "Parsing startupArgs";
                    var parseArgs = ParseArguments.Parse(this, startupArgs);
                    if (parseArgs.Item1)
                    {
                        var lstStartupArguments = parseArgs.Item3;
                        if (lstStartupArguments.Any())
                        {
                            ui = lstStartupArguments.First().showUI;
                            WriteLog("UI:" + ui, 1);
                            WriteLog("Arguments parsed", 1);
                            convertProcessLog = convertProcessLog + "\r\n" + "Arguments parsed";
                            if (lstStartupArguments.First().ConvertFiles && lstStartupArguments.First().showUI == false)
                            {
                                GetConfiguration();
                                WriteLog("GetConfiguration done", 1);
                                convertProcessLog = convertProcessLog + "\r\n" + "GetConfiguration done";
                                //CheckVersion();
                                //WriteLog("CheckVersion done", 1);
                                WriteLog("Converting files", 1);
                                convertProcessLog = convertProcessLog + "\r\n" + "Converting files";
                                ConvertFiles();
                                CloseWindow();
                            }
                            else
                            {
                                if (lstStartupArguments.First().showUI)
                                {
                                    GetConfiguration();
                                    WriteLog("GetConfiguration done", 1);
                                    CheckVersion();
                                    WriteLog("CheckVersion done", 1);
                                    ConvertFilesStart();
                                }
                                else
                                {
                                    CloseWindow();
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(parseArgs.Item2);
                    }
                }
                else
                {
                    GetConfiguration();
                    WriteLog("GetConfiguration done", 1);
                    CheckVersion();
                    WriteLog("CheckVersion done", 1);
                    GetLatestDBFile();
                }
                WriteLog("ConvertFilesStart done", 1);
            }
            catch (Exception ex)
            {
                string error = "MainWindow " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null);
                WriteError(error);
                if (!string.IsNullOrEmpty(convertProcessLog))
                {
                    SendMail(convertProcessLog + "\r\n" + "\r\n" + "ERRROR: " + "\r\n" + error);
                }
                MessageBox.Show(error);
            }

            //MessageBox.Show(CultureInfo.InstalledUICulture.DisplayName);
        }

        public void SendMail(string errroToWrite)
        {
            try
            {
                
                //lstSettings.First().smtpServer = "mail.konesko.ee";
                Debug.WriteLine("using " + lstSettings.First().smtpServer);
                Debug.WriteLine("using " + lstSettings.First().adminEmail);
                Debug.WriteLine("BauhofOffline error from " + Environment.MachineName);
                Debug.WriteLine(errroToWrite);
                var mail = new MailMessage();
                var SmtpServer = new SmtpClient(lstSettings.First().smtpServer);
                //SmtpServer.Credentials = new System.Net.NetworkCredential("dan.sool@konesko.ee", "kl0ngn11 ");
                mail.From = new MailAddress(lstSettings.First().senderEmail);
                mail.To.Add(lstSettings.First().adminEmail);

                mail.Subject = "BauhofOffline error from " + Environment.MachineName;
                mail.Body = errroToWrite;
                SmtpServer.Send(mail);
                Debug.WriteLine("mail sent");
            }
            catch(SmtpException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void CloseWindow()
        {
            Environment.Exit(0);
        }

        private void CheckVersion()
        {
            try
            {
                if (ui)
                {
                    WriteLog("CheckVersion started", 1);
                    txtBkLatestVersionValue.Text = "";
                    txtBkScannerVersionValue.Text = "";
                    btnUpdate.Visibility = Visibility.Collapsed;
                    string DeviceNameAsSeenInMyComputer = "";
                    MediaDevice device = null;
                    string destinationLatestFileNameText = "";

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
                            bool fileExists = false;
                            device.Connect();
                            WriteLog("CheckVersion device connected", 2);

                            WriteLog(@"CheckVersion looking for files in " + (language == "EN" ? "Internal shared storage" : "Sisemine jagatud mäluruum") + @"\Download", 2);
                            var photoDir = device.GetDirectoryInfo(@"\" + (language == "EN" ? "Internal shared storage" : "Sisemine jagatud mäluruum") + @"\Download");
                            var files = photoDir.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly);
                            foreach (var file in files)
                            {
                                WriteLog(@"CheckVersion found " + file.Name + @" in " + (language == "EN" ? "Internal shared storage" : "Sisemine jagatud mäluruum") + @"\Download", 2);
                                if (file.Name.ToUpper().StartsWith("VERSION_") && file.Name.ToUpper().EndsWith("_.TXT"))
                                {
                                    var textSplit = file.Name.ToUpper().Split(new[] { "_" }, StringSplitOptions.None);
                                    txtBkScannerVersionValue.Text = textSplit[1];
                                    fileExists = true;
                                }
                            }


                            if (!fileExists)
                            {
                                txtBkScannerVersionValue.Text = "PUUDUB";
                            }

                            if (proceed)
                            {
                                //string destinationLatestFileName = $@"c:\windows\temp\BauhofWMSVersion.txt";
                                //if (File.Exists(destinationLatestFileName))
                                //{
                                //    File.Delete(destinationLatestFileName);
                                //    WriteLog(@"CheckVersion deleted c:\windows\temp\BauhofWMSVersion.txt", 2);
                                //}

                                try
                                {
                                    WebClient webClient = new WebClient();
                                    byte[] myDataBuffer = webClient.DownloadData((new Uri("http://www.develok.ee/BauhofWMS/Install/BauhofWMSVersion.txt")));
                                    string download = Encoding.ASCII.GetString(myDataBuffer);
                                    destinationLatestFileNameText = download;

                                    //webClient.DownloadFile("http://www.develok.ee/BauhofWMS/Install/BauhofWMSVersion.txt", destinationLatestFileName);
                                    //WriteLog(@"CheckVersion file downloaded from http://www.develok.ee/BauhofWMS/Install/BauhofWMSVersion.txt", 2);
                                }
                                catch (Exception ex)
                                {
                                    proceed = false;
                                    WriteLog(@"CheckVersion VERSIOONIUUENDUST EI ÕNNESTUNUD KONTROLLIDA.FAILI EI LEITUD!", 2);
                                    MessageBox.Show("VERSIOONIUUENDUST EI ÕNNESTUNUD KONTROLLIDA. FAILI EI LEITUD!" + "\r\n" + ex.Message);
                                }

                                if (proceed)
                                {
                                    if (!string.IsNullOrEmpty(destinationLatestFileNameText))
                                    {
                                        
                                            var textSplit = destinationLatestFileNameText.Split(new[] { "#_#" }, StringSplitOptions.None);
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
                                        WriteLog(@"CheckVersion VERSIOONIUUENDUST EI ÕNNESTUNUD KONTROLLIDA: " + @"c:\windows\temp\BauhofWMSVersion.txt EI LEITUD", 2);
                                        MessageBox.Show("VERSIOONIUUENDUST EI ÕNNESTUNUD KONTROLLIDA: " + @"c:\windows\temp\BauhofWMSVersion.txt EI LEITUD");
                                    }
                                }
                            }
                            if (proceed)
                            {
                                Debug.WriteLine(txtBkLatestVersionValue.Text + "  " + txtBkScannerVersionValue.Text);
                                if (txtBkScannerVersionValue.Text == "PUUDUB")
                                {
                                    if (MessageBox.Show("NÄIB, ET SKÄNNERIL EI OLE VEEL TARKVARA INSALLEERITUD. KAS KOPEERIN INSTALLATSIOONIFAILI " + txtBkLatestVersionValue.Text + " SKÄNNERISSE ? ", "SKÄNNERITARKVARA", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                    {
                                        DownloadAPK();
                                    }
                                }
                                else
                                {
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
                            }
                            if (proceed)
                            {
                                device.Connect();
                                WriteLog("CheckVersion device connected", 2);
                                var DCIMDir = device.GetDirectoryInfo(@"\" + (language == "EN" ? "Internal shared storage" : "Sisemine jagatud mäluruum") + @"\DCIM\");
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
                                        device.CreateDirectory(@"\" + (language == "EN" ? "Internal shared storage" : "Sisemine jagatud mäluruum") + @"\DCIM\Export");
                                        WriteLog(@"CheckVersion device \" + (language == "EN" ? "Internal shared storage" : "Sisemine jagatud mäluruum") + @"\DCIM\Export folder created", 2);
                                    }
                                }

                                var exportDir = device.GetDirectoryInfo(@"\" + (language == "EN" ? "Internal shared storage" : "Sisemine jagatud mäluruum") + @"\DCIM\Export");
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
                            if (ex.Message.Contains(@"\" + (language == "EN" ? "Internal shared storage" : "Sisemine jagatud mäluruum") + @"\Download") && ex.Message.Contains("not found"))
                            {
                                WriteError("CheckVersion ÜHENDATUD SEADMELT EI LEITUD DOWNLOAD KATALOOGI!" + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
                                MessageBox.Show("CheckVersion  ÜHENDATUD SEADMELT EI LEITUD DOWNLOAD KATALOOGI!");
                            }
                            else
                            {
                                WriteError("CheckVersion " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
                                MessageBox.Show("CheckVersion  " + ex.Message);
                            }
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

        #endregion

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WriteLog(@"btnDownload_Click started", 2);
                string DeviceNameAsSeenInMyComputer = "";
                Debug.WriteLine("1");
                MediaDevice device = null;
                var devices = MediaDevice.GetDevices();
                if (devices.Any())
                {
                    Debug.WriteLine("2");
                    if (devices.Count() == 1)
                    {
                        Debug.WriteLine("3");
                        device = devices.First();

                        DeviceNameAsSeenInMyComputer = devices.First().Description;
                        Debug.WriteLine(DeviceNameAsSeenInMyComputer);
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
                        device.Connect();
                        var photoDir = device.GetDirectoryInfo(@"\" + (language == "EN" ? "Internal shared storage" : "Sisemine jagatud mäluruum") + @"\DCIM\Export");
                        Debug.WriteLine("5");
                        var files = photoDir.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly);
                        foreach (var file in files)
                        {
                            count = count + 1;
                            string destinationFileName = lstSettings.First().exportFolder + file.Name;
                            if (!File.Exists(destinationFileName))
                            {
                                using (FileStream fs = new FileStream(destinationFileName, FileMode.Create, System.IO.FileAccess.Write))
                                {
                                    device.DownloadFile(file.FullName, fs);
                                    WriteLog(@"btnDownload_Click downloaded " + file.FullName, 2);
                                }
                            }
                            if (File.Exists(destinationFileName))
                            {
                                device.DeleteFile(file.FullName);
                            }
                        }
                        device.Disconnect();
                        WriteLog(@"btnDownload_Click device disconnected", 2);
                    }
                    catch (Exception ex)
                    {
                        proceed = false;
                        if (ex.Message.Contains(@"\" + (language == "EN" ? "Internal shared storage" : "Sisemine jagatud mäluruum") + @"\DCIM") && ex.Message.Contains("not found"))
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
                    txtBkStatus.Text = "Kopeerin faili skännerisse!";
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

                        var photoDir = device.GetDirectoryInfo(@"\" + (language == "EN" ? "Internal shared storage" : "Sisemine jagatud mäluruum") + @"\DCIM\");
                        var files = photoDir.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly);

                        foreach (var file in files)
                        {
                            if (file.Name.ToUpper().StartsWith("DBRECORD") && file.Name.ToUpper().EndsWith(".TXT"))
                            {
                                device.DeleteFile(photoDir.FullName + @"\" + file.Name);
                            }

                            if (file.Name.ToUpper().StartsWith("SHOPRELATIONS") && file.Name.ToUpper().EndsWith(".TXT"))
                            {
                                device.DeleteFile(photoDir.FullName + @"\" + file.Name);
                            }

                            if (file.Name.ToUpper().StartsWith("SHRCVDBRECORDS") && file.Name.ToUpper().EndsWith(".TXT"))
                            {
                                device.DeleteFile(photoDir.FullName + @"\" + file.Name);
                            }

                            if (file.Name.ToUpper().StartsWith("TRFRCVDBRECORDS") && file.Name.ToUpper().EndsWith(".TXT"))
                            {
                                device.DeleteFile(photoDir.FullName + @"\" + file.Name);
                            }
                        }

                        string[] dirs = Directory.GetFiles(lstSettings.First().jsonFolder);
                        foreach (string str in dirs)
                        {
                            string sourceFileName = str;
                            int index = str.LastIndexOf("\\");
                            string fileName = str.Substring(index + 1);
                            if (fileName.ToUpper().StartsWith("DBRECORD") && fileName.ToUpper().EndsWith(".TXT"))
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
                            if (fileName.ToUpper().StartsWith("SHOPRELATIONS") && fileName.ToUpper().EndsWith(".TXT"))
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

                            if (fileName.ToUpper().StartsWith("SHRCVDBRECORDS") && fileName.ToUpper().EndsWith(".TXT"))
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

                            if (fileName.ToUpper().StartsWith("TRFRCVDBRECORDS") && fileName.ToUpper().EndsWith(".TXT"))
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

            DownloadAPK();
        }

        private void DownloadAPK()
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
                            try
                            {
                                device.CreateDirectory(@"\IPSM card\Download");
                            }
                            catch (Exception ex)
                            {

                            }

                            if (proceed)
                            {
                                var dcimDownload = device.GetDirectoryInfo(@"\IPSM card\Download");

                                var files = dcimDownload.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly);
                                foreach (var file in files)
                                {
                                    if (file.Name.ToUpper().EndsWith(".APK"))
                                    {
                                        //MessageBox.Show(file.Name);
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
                                try
                                {
                                    device.UploadFile(destinationFile, dcimDownload.FullName + @"\" + "BauhofWMS" + androidVersion + ".apk");
                                }
                                catch (System.IO.IOException ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }


                            //device.UploadFile(destinationFile, dcimDownload.FullName + @"\" + "BauhofWMS" + androidVersion + ".apk");
                            //MessageBox.Show(dcimDownload.FullName + @"\" + "BauhofWMS" + androidVersion + ".apk");
                            if (proceed)
                            {
                                var photoDir = device.GetDirectoryInfo(@"\" + (language == "EN" ? "Internal shared storage" : "Sisemine jagatud mäluruum") + @"\Download\");
                                var files2 = photoDir.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly);
                                foreach (var file in files2)
                                {
                                    if (file.Name.ToUpper().EndsWith(".APK"))
                                    {
                                        try
                                        {
                                            device.DeleteFile(photoDir.FullName + @"\" + file.Name);
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("SKÄNNERIL EI ÕNNESTUNUD KUSTUTADA FAILI:" + "\r\n" + photoDir.FullName + @"\" + "BauhofWMS" + androidVersion + ".apk" + "\r\n" + ex.Message);
                                        }
                                    }
                                }
                                device.UploadFile(destinationFile, photoDir.FullName + @"\" + "BauhofWMS" + androidVersion + ".apk");
                                device.Disconnect();
                                btnUpdate.Visibility = Visibility.Collapsed;
                                txtBkStatus.Text = "";
                                MessageBox.Show("UUENDUS ON SKÄNNERISSE LAETUD!");
                            }
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

        public string ConvertCsvFileToJsonObjectToLarge(string folderPath, string inputFileName, DateTime inputFileDate, int fileCounter)
        {
            try
            {
                convertProcessLog = convertProcessLog + "\r\n" + "Processing " + inputFileName;
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                string filename = folderPath + inputFileName;
                if (!Directory.Exists(lstSettings.First().csvArchiveFolder))
                {                    
                    Directory.CreateDirectory(lstSettings.First().csvArchiveFolder);
                }
                if (!File.Exists(lstSettings.First().csvArchiveFolder + @"\" + inputFileName.Replace(".csv", ".convert")))
                {
                    convertProcessLog = convertProcessLog + "\r\n" + "Locking file " + inputFileName;
                    File.WriteAllText(lstSettings.First().csvArchiveFolder + @"\" + inputFileName.Replace(".csv", ".convert"), DateTime.Now.ToString());
                    convertProcessLog = convertProcessLog + "\r\n" + "Reading file " + inputFileName;
                    WriteLog("Inputfile read started: " + inputFileName, 1);
                    List<ListOfdbRecordsImport> values = File.ReadAllLines(folderPath + inputFileName).Skip(1).Select(v => FromCsv(v, inputFileName, inputFileDate, lstSettings.First().logFolder, lstSettings.First().adminEmail, lstSettings.First().senderEmail, lstSettings.First().smtpServer)).ToList();
                    convertProcessLog = convertProcessLog + "\r\n" + "Inputfile " + inputFileName + " " + values.Count() + " lines readed";
                    WriteLog("Inputfile " + inputFileName + " " + values.Count() + " lines readed", 1);
                    if (fileCounter == 1)
                    {
                        lstDB01 = values;
                    }
                    if (fileCounter == 2)
                    {
                        lstDB02 = values;
                    }
                    if (fileCounter == 3)
                    {
                        lstDB03 = values;
                    }
                    if (fileCounter == 4)
                    {
                        lstDB04 = values;
                    }
                    if (fileCounter == 5)
                    {
                        lstDB05 = values;
                    }
                    if (fileCounter == 6)
                    {
                        lstDB06 = values;
                    }
                    if (fileCounter == 7)
                    {
                        lstDB07 = values;
                    }
                    if (fileCounter == 8)
                    {
                        lstDB08 = values;
                    }
                    if (fileCounter == 9)
                    {
                        lstDB09 = values;
                    }
                    if (fileCounter == 10)
                    {
                        lstDB10 = values;
                    }
                    if (fileCounter == 11)
                    {
                        lstDB11 = values;
                    }
                    if (fileCounter == 12)
                    {
                        lstDB12 = values;
                    }

                    if (fileCounter == 13)
                    {
                        lstDB13 = values;
                    }

                    if (fileCounter == 14)
                    {
                        lstDB14 = values;
                    }

                    if (fileCounter == 15)
                    {
                        lstDB15 = values;
                    }

                    if (fileCounter == 16)
                    {
                        lstDB16 = values;
                    }

                    if (fileCounter == 17)
                    {
                        lstDB17 = values;
                    }

                    if (fileCounter == 18)
                    {
                        lstDB18 = values;
                    }

                    if (fileCounter == 19)
                    {
                        lstDB19 = values;
                    }

                    if (fileCounter == 20)
                    {
                        lstDB20 = values;
                    }

                    if (fileCounter == 21)
                    {
                        lstDB21 = values;
                    }
                    if (fileCounter == 22)
                    {
                        lstDB22 = values;
                    }

                    if (fileCounter == 23)
                    {
                        lstDB23 = values;
                    }

                    if (fileCounter == 24)
                    {
                        lstDB24 = values;
                    }

                    if (fileCounter == 25)
                    {
                        lstDB25 = values;
                    }

                    if (fileCounter == 26)
                    {
                        lstDB26 = values;
                    }

                    if (fileCounter == 27)
                    {
                        lstDB27 = values;
                    }

                    if (fileCounter == 28)
                    {
                        lstDB28 = values;
                    }

                    if (fileCounter == 29)
                    {
                        lstDB29 = values;
                    }

                    if (fileCounter == 30)
                    {
                        lstDB30 = values;
                    }
                }
                WriteLog("Moving inputfile " + inputFileName + " to " + lstSettings.First().csvArchiveFolder, 1);
                convertProcessLog = convertProcessLog + "\r\n" + "Moving inputfile " + inputFileName + " to " + lstSettings.First().csvArchiveFolder;
                File.Move(filename, lstSettings.First().csvArchiveFolder + @"\" + inputFileName);
                return "";
            }
            catch (Exception ex)
            {
                string error = "ConvertCsvFileToJsonObject " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null);
                WriteError(error);
                if (!string.IsNullOrEmpty(convertProcessLog))
                {
                    SendMail(convertProcessLog + "\r\n" + "\r\n" + "ERRROR: " + "\r\n" + error);
                }
                MessageBox.Show(error);
                return "";
            }
        }

        public string ConvertCsvFileToJsonObject(string folderPath, string inputFileName, DateTime inputFileDate)
        {
            try
            {
                convertProcessLog = convertProcessLog + "\r\n" + "Convert of shopfile started";
                string filename = folderPath + inputFileName;
                convertProcessLog = convertProcessLog + "\r\n" + "Shopfile read started";
                List<ListOfShopRelations> values = File.ReadAllLines(folderPath + inputFileName).Skip(1).Select(v => FromShopRelationCsv(v, inputFileName, lstSettings.First().logFolder, lstSettings.First().adminEmail, lstSettings.First().senderEmail, lstSettings.First().smtpServer)).ToList();
                lstShopRelations = values;
                convertProcessLog = convertProcessLog + "\r\n" + "Shopfile read done. Total records " + lstShopRelations.Count();
                Debug.WriteLine("lstShopRelations.Count() " + lstShopRelations.Count());
                string outputFile = inputFileName.Replace(".csv", ".txt").ToUpper();
                string json = JsonConvert.SerializeObject(values);
                if (File.Exists(lstSettings.First().jsonFolder + outputFile))
                {
                    convertProcessLog = convertProcessLog + "\r\n" + "Deleting existing shopfile";
                    File.Delete(lstSettings.First().jsonFolder + outputFile);
                }
                File.WriteAllText(lstSettings.First().jsonFolder + outputFile, json);
                convertProcessLog = convertProcessLog + "\r\n" + "Shopfile created!";
                return "";
            }
            catch (Exception ex)
            {
                string error = "ConvertCsvFileToJsonObject " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null);
                WriteError(error);
                if (!string.IsNullOrEmpty(convertProcessLog))
                {
                    SendMail(convertProcessLog + "\r\n" + "\r\n" + "ERRROR: " + "\r\n" + error);
                }
                MessageBox.Show(error);
                return "";
            }
        }

        public string ConvertPurchaseReceiveCsvFileToJsonObject(string folderPath, string inputFileName, DateTime inputFileDate)
        {
            try
            {
                convertProcessLog = convertProcessLog + "\r\n" + "Convert of purchase receive started";
                string filename = folderPath + inputFileName;
                convertProcessLog = convertProcessLog + "\r\n" + "Purchase receive read started";
                List<ListOfPurchaseReceive> values = File.ReadAllLines(folderPath + inputFileName).Skip(1).Select(v => FromPurchaseReceiveCsv(v, inputFileName, lstSettings.First().logFolder, lstSettings.First().adminEmail, lstSettings.First().senderEmail, lstSettings.First().smtpServer)).ToList();
                lstPurchaseReceive = values;
                convertProcessLog = convertProcessLog + "\r\n" + "Purchase receive read done. Total records " + lstPurchaseReceive.Count();
                Debug.WriteLine("lstShopRelations.Count() " + lstPurchaseReceive.Count());

                string[] dirs = Directory.GetFiles(lstSettings.First().jsonFolder);
                foreach (string str in dirs)
                {
                   
                    string sourceFileName = str;
                    int index = str.LastIndexOf("\\");
                    string fileName = str.Substring(index + 1);
                    
                    if (fileName.ToUpper().StartsWith("SHRCVDBRECORDS") && fileName.ToUpper().EndsWith(".TXT"))
                    {
                        File.Delete(str);
                    }
                }
                string json = JsonConvert.SerializeObject(values);
                File.WriteAllText(lstSettings.First().jsonFolder + "SHRCVDBRECORDS_" + String.Format("{0:yyyyMMdd_HHmmss}", DateTime.Now) + ".TXT", json);
                convertProcessLog = convertProcessLog + "\r\n" + "Purchase receive created!";

                File.Move(lstSettings.First().csvFolder + inputFileName, lstSettings.First().csvArchiveFolder + inputFileName);
                convertProcessLog = convertProcessLog + "\r\n" + "Purchase receive csv file archived!";
                return "";
            }
            catch (Exception ex)
            {
                string error = "ConvertPurchaseReceiveCsvFileToJsonObject " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null);
                WriteError(error);
                if (!string.IsNullOrEmpty(convertProcessLog))
                {
                    SendMail(convertProcessLog + "\r\n" + "\r\n" + "ERRROR: " + "\r\n" + error);
                }
                MessageBox.Show(error);
                return "";
            }
        }

        public string ConvertTransferReceiveCsvFileToJsonObject(string folderPath, string inputFileName, DateTime inputFileDate)
        {
            try
            {
                convertProcessLog = convertProcessLog + "\r\n" + "Convert of transfer receive started";
                string filename = folderPath + inputFileName;
                convertProcessLog = convertProcessLog + "\r\n" + "transfer receive read started";
                List<ListOfTransferReceive> values = File.ReadAllLines(folderPath + inputFileName).Skip(1).Select(v => FromTransferReceiveCsv(v, inputFileName, lstSettings.First().logFolder, lstSettings.First().adminEmail, lstSettings.First().senderEmail, lstSettings.First().smtpServer)).ToList();
                lstTransferReceive = values;
                convertProcessLog = convertProcessLog + "\r\n" + "Transfer receive read done. Total records " + lstPurchaseReceive.Count();

                string[] dirs = Directory.GetFiles(lstSettings.First().jsonFolder);
                foreach (string str in dirs)
                {

                    string sourceFileName = str;
                    int index = str.LastIndexOf("\\");
                    string fileName = str.Substring(index + 1);

                    if (fileName.ToUpper().StartsWith("TRFRCVDBRECORDS") && fileName.ToUpper().EndsWith(".TXT"))
                    {
                        File.Delete(str);
                    }
                }
                string json = JsonConvert.SerializeObject(values);
                File.WriteAllText(lstSettings.First().jsonFolder + "TRFRCVDBRECORDS_" + String.Format("{0:yyyyMMdd_HHmmss}", DateTime.Now) + ".TXT", json);
                convertProcessLog = convertProcessLog + "\r\n" + "Transfer receive created!";

                File.Move(lstSettings.First().csvFolder + inputFileName, lstSettings.First().csvArchiveFolder + inputFileName);
                convertProcessLog = convertProcessLog + "\r\n" + "Transfer receive csv file archived!";
                return "";
            }
            catch (Exception ex)
            {
                string error = "ConvertTransferReceiveCsvFileToJsonObject " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null);
                WriteError(error);
                if (!string.IsNullOrEmpty(convertProcessLog))
                {
                    SendMail(convertProcessLog + "\r\n" + "\r\n" + "ERRROR: " + "\r\n" + error);
                }
                MessageBox.Show(error);
                return "";
            }
        }



        public void ConvertFilesStart()
        {
            try
            {
                Debug.WriteLine("ConvertFilesStart start");
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

        public void GetLatestDBFile()
        {
            try
            {
                convertProcessLog = convertProcessLog + "\r\n" + "Getting files from " + lstSettings.First().jsonFolder;
                string[] dirs = Directory.GetFiles(lstSettings.First().jsonFolder);
                if (dirs.Any())
                {
                    //convertProcessLog = convertProcessLog + "\r\n" + "Dir exists";
                    foreach (string str in dirs)
                    {
                        //convertProcessLog = convertProcessLog + "\r\n" + "File:" + str;
                        string sourceFileName = str;
                        int index = str.LastIndexOf("\\");
                        string fileName = str.Substring(index + 1);
                        if (fileName.ToUpper().StartsWith("DBRECORDS_") && fileName.ToUpper().EndsWith(".TXT"))
                        {
                            string fileVersionFull = fileName.Replace("DBRECORDS_", "").Replace(".TXT", "");
                            string fileVersion = fileVersionFull.Substring(6, 2) + "." + fileVersionFull.Substring(4, 2) + "." + fileVersionFull.Substring(0, 4) + " " + fileVersionFull.Substring(9, 2) + ":" + fileVersionFull.Substring(11, 2) + ":" + fileVersionFull.Substring(13, 2);
                            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
                            {
                                convertProcessLog = convertProcessLog + "\r\n" + "Current database version: " + fileVersion;
                                txtBkLatestFile.Text = "ANDMEBAASI VERSIOON: " + fileVersion;
                            }));
                        }
                    }
                }
                //convertProcessLog = convertProcessLog + "\r\n" + "Getting files from " + lstSettings.First().jsonFolder + " complete";
            }
            catch (Exception ex)
            {
                string error = "GetLatestDBFile " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null);
                WriteError(error);
                if (!string.IsNullOrEmpty(convertProcessLog))
                {
                    SendMail(error);
                }
                MessageBox.Show(error);
            }
        }

        private void bw_DoWork_ConvertFiles(object sender, DoWorkEventArgs e)
        {
            ConvertFiles();
        }

        private void ConvertFiles()
        {
            try
            {                
                GetLatestDBFile();
                bool proceed = true;
                string csvFolderPath = "";
                string shopFileFolder = "";
                string csvArchiveFolder = "";
                IEnumerable<ListOfdbRecordsImport> dbconcat = null;
                try
                {
                    if (ui)
                    {
                        Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
                        {
                            txtBkStatus.Text = "Konverteerin leitud andmebaasi skänneri andmebaasiks!";
                            prgRing.Visibility = Visibility.Visible;
                        }));
                    }

                    csvFolderPath = lstSettings.First().csvFolder;
                    shopFileFolder = lstSettings.First().shopFileFolder;
                    csvArchiveFolder = lstSettings.First().csvArchiveFolder;
                    string jsonFolderPath = lstSettings.First().jsonFolder;
                    string jsonArchiveFolder = lstSettings.First().jsonFolder + @"\Archive\";
                    if (!Directory.Exists(jsonArchiveFolder))
                    {
                        convertProcessLog = convertProcessLog + "\r\n" + "Creating " + jsonArchiveFolder;
                        Directory.CreateDirectory(jsonArchiveFolder);
                    }
                    string relationFileName = "ShopRelations.csv";
                    if (!Directory.Exists(csvFolderPath))
                    {
                        convertProcessLog = convertProcessLog + "\r\n" + "Creating " + csvFolderPath;
                        Directory.CreateDirectory(csvFolderPath);
                    }
                    if (proceed)
                    {
                        if (File.Exists(shopFileFolder + relationFileName))
                        {
                            var l = ConvertCsvFileToJsonObject(shopFileFolder, relationFileName, DateTime.Now);
                        }
                    }

                    if (proceed)
                    {
                        bool receiveReceiveFileExists = false;
                        bool transferReceiveFileExists = false;
                        if (lstShopRelations.Any())
                        {
                            string[] dirs = Directory.GetFiles(csvFolderPath);
                            foreach (var r in lstShopRelations)
                            {
                                bool isInFolder = false;
                                foreach (string str in dirs)
                                {
                                    string sourceFileName = str;
                                    int index = str.LastIndexOf("\\");
                                    string fileName = str.Substring(index + 1);
                                    if (fileName.StartsWith(r.shopID + "_PDA_Products"))
                                    {
                                        isInFolder = true;
                                    }
                                    //ETAPP 2
                                    if (fileName.ToUpper().StartsWith("SHRCV_") && fileName.ToUpper().EndsWith(".CSV"))
                                    {
                                        receiveReceiveFileExists = true;
                                    }
                                    if (fileName.ToUpper().StartsWith("TRFRCV_") && fileName.ToUpper().EndsWith(".CSV"))
                                    {
                                        transferReceiveFileExists = true;
                                    }

                                }
                                if (!isInFolder)
                                {
                                    convertProcessLog = convertProcessLog + "\r\n" + "PDA csv file for " + r.shopID + " " + r.shopName + " was not found";                                    
                                    proceed = false;
                                }
                            }
                        }
                        if (receiveReceiveFileExists)
                        {
                            ConvertPurchaseReceiveFiles();
                        }

                        if (transferReceiveFileExists)
                        {
                            ConvertTransferReceiveFiles();
                        }

                    }
                    if (proceed)
                    {
                       
                        string[] dirs = Directory.GetFiles(csvFolderPath);
                        int fileCounter = 0;
                        if (dirs.Any())
                        {
                            bool dbfilesExist = false;
                            bool receivefilesExist = false;
                            foreach (string str in dirs)
                            {
                                string sourceFileName = str;
                                int index = str.LastIndexOf("\\");
                                string fileName = str.Substring(index + 1);
                                if (fileName.EndsWith(".lock"))
                                {
                                    WriteLog("Lock exists, convert operations skipped", 1);
                                    proceed = false;
                                }
                                if (fileName.ToUpper().EndsWith(".CSV") && fileName.Contains("_PDA_Products"))
                                {
                                    dbfilesExist = true;
                                }                                
                            }

                            if (proceed)
                            {                                
                                if (dbfilesExist)
                                {
                                    var file = new StreamWriter(csvArchiveFolder + Environment.MachineName + ".lock", true);
                                    WriteLog("Lock file created", 1);
                                    file.WriteLine("");
                                    file.Close();
                                    {
                                        foreach (string str in dirs)
                                        {
                                            fileCounter = fileCounter + 1;
                                            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
                                            {
                                                txtBkStatus.Text = "Konverteerin leitud andmebaasi skänneri andmebaasiks! " + "\r\n" + "Loen faili " + fileCounter + "/" + (dirs.Count() - 1);
                                            }));

                                            string sourceFileName = str;
                                            int index = str.LastIndexOf("\\");
                                            string fileName = str.Substring(index + 1);
                                            if (fileName.EndsWith(".lock"))
                                            {
                                                proceed = false;
                                            }

                                            if (proceed)
                                            {
                                                if (fileName.ToUpper().EndsWith(".CSV"))
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
                                                                ConvertCsvFileToJsonObjectToLarge(csvFolderPath, fileName, fileDate, fileCounter);
                                                            }
                                                        
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    proceed = false;
                                }
                            }
                            fileCounter = fileCounter + 1;
                        }
                        else
                        {
                            proceed = false;
                            //MessageBox.Show("bw_DoWork_ConvertFiles: csv faile ei leitud kataloogist " + csvFolderPath);
                        }
                    }
                    if (proceed)
                    {
                        convertProcessLog = convertProcessLog + "\r\n" + "Inputfiles merge started";
                        WriteLog("Inputfiles merge started", 1);
                        if (lstDB01.Any())
                        {
                            dbconcat = lstDB01;
                            if (ui)
                            {
                                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
                                {
                                    txtBkStatus.Text = "Konverteerin leitud andmebaasi skänneri andmebaasiks! " + "\r\n" + "Liidan loetud failide sisu";
                                }));
                            }

                            if (lstDB02.Any())
                            {
                                var d1 = dbconcat.Concat(lstDB02);
                                dbconcat = d1;
                            }

                            if (lstDB03.Any())
                            {
                                var d2 = dbconcat.Concat(lstDB03);
                                dbconcat = d2;
                            }

                            if (lstDB04.Any())
                            {
                                var d3 = dbconcat.Concat(lstDB04);
                                dbconcat = d3;
                            }

                            if (lstDB05.Any())
                            {
                                var d4 = dbconcat.Concat(lstDB05);
                                dbconcat = d4;
                            }

                            if (lstDB06.Any())
                            {
                                var d5 = dbconcat.Concat(lstDB06);
                                dbconcat = d5;
                            }

                            if (lstDB07.Any())
                            {
                                var d6 = dbconcat.Concat(lstDB07);
                                dbconcat = d6;
                            }

                            if (lstDB08.Any())
                            {
                                var d7 = dbconcat.Concat(lstDB08);
                                dbconcat = d7;
                            }

                            if (lstDB09.Any())
                            {
                                var d8 = dbconcat.Concat(lstDB09);
                                dbconcat = d8;
                            }

                            if (lstDB10.Any())
                            {
                                var d9 = dbconcat.Concat(lstDB10);
                                dbconcat = d9;
                            }

                            if (lstDB11.Any())
                            {
                                var d10 = dbconcat.Concat(lstDB11);
                                dbconcat = d10;
                            }

                            if (lstDB12.Any())
                            {
                                var d11 = dbconcat.Concat(lstDB12);
                                dbconcat = d11;
                            }

                            if (lstDB13.Any())
                            {
                                var d12 = dbconcat.Concat(lstDB13);
                                dbconcat = d12;
                            }

                            if (lstDB14.Any())
                            {
                                var d13 = dbconcat.Concat(lstDB14);
                                dbconcat = d13;
                            }

                            if (lstDB15.Any())
                            {
                                var d14 = dbconcat.Concat(lstDB15);
                                dbconcat = d14;
                            }

                            if (lstDB16.Any())
                            {
                                var d15 = dbconcat.Concat(lstDB16);
                                dbconcat = d15;
                            }

                            if (lstDB17.Any())
                            {
                                var d16 = dbconcat.Concat(lstDB17);
                                dbconcat = d16;
                            }

                            if (lstDB18.Any())
                            {
                                var d17 = dbconcat.Concat(lstDB18);
                                dbconcat = d17;
                            }

                            if (lstDB19.Any())
                            {
                                var d18 = dbconcat.Concat(lstDB19);
                                dbconcat = d18;
                            }

                            if (lstDB20.Any())
                            {
                                var d19 = dbconcat.Concat(lstDB20);
                                dbconcat = d19;
                            }


                            if (lstDB21.Any())
                            {
                                var d20 = dbconcat.Concat(lstDB21);
                                dbconcat = d20;
                            }

                            if (lstDB22.Any())
                            {
                                var d21 = dbconcat.Concat(lstDB22);
                                dbconcat = d21;
                            }

                            if (lstDB23.Any())
                            {
                                var d22 = dbconcat.Concat(lstDB23);
                                dbconcat = d22;
                            }

                            if (lstDB24.Any())
                            {
                                var d23 = dbconcat.Concat(lstDB24);
                                dbconcat = d23;
                            }

                            if (lstDB25.Any())
                            {
                                var d24 = dbconcat.Concat(lstDB25);
                                dbconcat = d24;
                            }

                            if (lstDB26.Any())
                            {
                                var d25 = dbconcat.Concat(lstDB26);
                                dbconcat = d25;
                            }

                            if (lstDB27.Any())
                            {
                                var d26 = dbconcat.Concat(lstDB27);
                                dbconcat = d26;
                            }

                            if (lstDB28.Any())
                            {
                                var d27 = dbconcat.Concat(lstDB28);
                                dbconcat = d27;
                            }

                            if (lstDB29.Any())
                            {
                                var d28 = dbconcat.Concat(lstDB29);
                                dbconcat = d28;
                            }

                            if (lstDB30.Any())
                            {
                                var d29 = dbconcat.Concat(lstDB30);
                                dbconcat = d29;
                            }
                        }
                        else
                        {
                            WriteLog("Inputfiles merge failed: no lstDB01 available", 1);
                            proceed = false;
                        }
                    }
                    if (proceed)
                    {

                        var lstOfConcat = dbconcat.ToList();
                        convertProcessLog = convertProcessLog + "\r\n" + "Inputfiles merge complete. Total records: " + lstOfConcat.Count();
                        WriteLog("Inputfiles merge complete. Total records: " + lstOfConcat.Count(), 1);
                        List<ListOfdbRecords> finalDB = null;
                        sKUCounter = 0;
                        Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
                        {
                            txtBkStatus.Text = "Konverteerin leitud andmebaasi skänneri andmebaasiks! " + "\r\n" + "Grupeerin failide sisu";
                        }));
                        var countOfConcat = lstOfConcat.GroupBy(x => x.itemCode).ToList().Count();
                        convertProcessLog = convertProcessLog + "\r\n" + "dbconcat grouping complete. Total records: " +  countOfConcat;
                        WriteLog("dbconcat grouping complete. Total records: " + countOfConcat, 1);
                        finalDB = FillSKUData(lstOfConcat, countOfConcat);
                        convertProcessLog = convertProcessLog + "\r\n" + "finalDB done. Total records: " + finalDB.Count();
                        WriteLog("finalDB done. Total records: " + finalDB.Count(), 1);


                        string outputFile = "dbRecords_" + String.Format("{0:yyyyMMdd_HHmmss}", DateTime.Now) + ".txt";
                        Debug.WriteLine(lstSettings.First().jsonFolder + outputFile);

                        Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
                        {
                            txtBkStatus.Text = "Konverteerin leitud andmebaasi skänneri andmebaasiks! " + "\r\n" + "Kirjutan impordifaili";
                        }));
                        string jsonFinal = JsonConvert.SerializeObject(finalDB);
                        string[] dirs = Directory.GetFiles(lstSettings.First().jsonFolder);
                        if (dirs.Any())
                        {
                            foreach (string str in dirs)
                            {
                                string sourceFileName = str;
                                int index = str.LastIndexOf("\\");
                                string fileName = str.Substring(index + 1);
                                if (fileName.ToUpper().StartsWith("DBRECORDS") && fileName.ToUpper().EndsWith(".TXT"))
                                {
                                    if (!Directory.Exists(lstSettings.First().jsonFolder + @"\Archive\"))
                                    {
                                        Directory.CreateDirectory(lstSettings.First().jsonFolder + @"\Archive\");
                                    }
                                    WriteLog("Moving existing file " + fileName + " to archive", 1);
                                    convertProcessLog = convertProcessLog + "\r\n" + "Moving existing file " + fileName + " to archive";
                                    File.Move(lstSettings.First().jsonFolder + @"\" + fileName, lstSettings.First().jsonFolder + @"\Archive\" + fileName);
                                }
                            }
                        }


                        File.WriteAllText(lstSettings.First().jsonFolder + outputFile.ToUpper(), jsonFinal);
                        convertProcessLog = convertProcessLog + "\r\n" + "New db file created: " + outputFile;
                        WriteLog("New db file created: " + outputFile, 1);
                        File.Delete(lstSettings.First().csvArchiveFolder + Environment.MachineName + ".lock");

                        convertProcessLog = convertProcessLog + "\r\n" + "lockfile deleted in " + lstSettings.First().csvArchiveFolder;
                        WriteLog("lockfile deleted in " + lstSettings.First().csvArchiveFolder, 1);

                        
                        Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
                        {
                            DisplayPostedMessage("ANDMEBAAS VALMIS!");
                        }));
                    }
                    if (proceed)
                    {
                        convertProcessLog = convertProcessLog + "\r\n" + "Conversion of db is complete " + lstSettings.First().csvArchiveFolder;
                        SendMail(convertProcessLog);
                        WriteLog("Conversion of db is complete " + lstSettings.First().csvArchiveFolder, 1);
                    }
                    if (!proceed)
                    {
                        if (!string.IsNullOrEmpty(convertProcessLog))
                        {
                            SendMail(convertProcessLog + "\r\n" + "Process stopped!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    string error = "ConvertFiles " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null);
                    WriteError(error);
                    if (!string.IsNullOrEmpty(convertProcessLog))
                    {
                        SendMail(error);
                    }
                    MessageBox.Show(error);
                }
            }
            catch(Exception ex)
            {
                string error = "ConvertFiles " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null);
                WriteError(error);
                if (!string.IsNullOrEmpty(convertProcessLog))
                {
                    SendMail(convertProcessLog + "\r\n" + "\r\n" + "ERRROR: " + "\r\n" + error);
                }
                MessageBox.Show(error);
            }
        }

        private void ConvertPurchaseReceiveFiles()
        {
            try
            {
                bool proceed = true;
                string csvFolderPath = "";
                string shopFileFolder = "";
                string csvArchiveFolder = "";
                IEnumerable<ListOfdbRecordsImport> dbconcat = null;
                try
                {
                    if (ui)
                    {
                        Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
                        {
                            txtBkStatus.Text = "Konverteerin leitud tarnete andmebaasi skänneri tarnete andmebaasiks!";
                            prgRing.Visibility = Visibility.Visible;
                        }));
                    }

                    csvFolderPath = lstSettings.First().csvFolder;
                    shopFileFolder = lstSettings.First().shopFileFolder;
                    csvArchiveFolder = lstSettings.First().csvArchiveFolder;
                    string jsonFolderPath = lstSettings.First().jsonFolder;
                    string jsonArchiveFolder = lstSettings.First().jsonFolder + @"\Archive\";
                    

                    if (proceed)
                    {
                        if (lstShopRelations.Any())
                        {
                            string[] dirs = Directory.GetFiles(csvFolderPath); 
                            bool isInFolder = false;
                            foreach (string str in dirs)
                            {
                                string sourceFileName = str;
                                int index = str.LastIndexOf("\\");
                                string fileName = str.Substring(index + 1);
                                if (fileName.StartsWith("SHRCV_"))
                                {
                                    isInFolder = true;
                                }
                            }
                            if (!isInFolder)
                            {
                                convertProcessLog = convertProcessLog + "\r\n" + "SHRCV csv file was not found";                                
                                proceed = false;
                            }
                        }
                    }
                    if (proceed)
                    {

                        string[] dirs = Directory.GetFiles(csvFolderPath);
                        int fileCounter = 0;
                        if (dirs.Any())
                        {
                            bool shrcvfilesExist = false;
                            foreach (string str in dirs)
                            {
                                string sourceFileName = str;
                                int index = str.LastIndexOf("\\");
                                string fileName = str.Substring(index + 1);
                                if (fileName.EndsWith(".lock"))
                                {
                                    WriteLog("Lock exists, convert operations skipped", 1);
                                    proceed = false;
                                }
                                if (fileName.ToUpper().EndsWith(".CSV") && fileName.Contains("SHRCV_"))
                                {
                                    shrcvfilesExist = true;
                                }
                            }

                            if (proceed)
                            {
                                if (shrcvfilesExist)
                                {
                                    var file = new StreamWriter(csvArchiveFolder + Environment.MachineName + ".lock", true);
                                    WriteLog("Lock file created", 1);
                                    file.WriteLine("");
                                    file.Close();
                                    {
                                        foreach (string str in dirs)
                                        {
                                            fileCounter = fileCounter + 1;
                                            if (ui)
                                            {
                                                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
                                                {
                                                    txtBkStatus.Text = "Konverteerin leitud andmebaasi skänneri andmebaasiks! " + "\r\n" + "Loen faili " + fileCounter + "/" + (dirs.Count() - 1);
                                                }));
                                            }
                                            string sourceFileName = str;
                                            int index = str.LastIndexOf("\\");
                                            string fileName = str.Substring(index + 1);
                                            if (fileName.EndsWith(".lock"))
                                            {
                                                proceed = false;
                                            }

                                            if (proceed)
                                            {
                                                if (fileName.ToUpper().EndsWith(".CSV") && fileName.Contains("SHRCV_"))
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
                                                        if (fileName2.ToUpper().EndsWith(".CSV") && fileName2.Contains("SHRCV_"))
                                                        {
                                                            if (fileName2 != fileName)
                                                            {
                                                                File.Move(jsonFolderPath + fileName2, jsonArchiveFolder + fileName2);
                                                            }
                                                        }
                                                    }
                                                    if (!File.Exists(jsonFolderPath + fileName))
                                                    {
                                                        var fileNameSplit = fileName.Split(new[] { "SHRCV_" }, StringSplitOptions.None);
                                                        string datePart = fileNameSplit[1].ToUpper().Replace(".CSV", "").Replace("-", "");
                                                        string formatstring = "yyyyMMddHHmmss";
                                                        DateTime fileDate = DateTime.ParseExact(datePart, formatstring, null);
                                                        ConvertPurchaseReceiveCsvFileToJsonObject(csvFolderPath, fileName, fileDate);
                                                        proceed = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    proceed = false;
                                }
                            }
                            fileCounter = fileCounter + 1;
                        }
                        else
                        {
                            proceed = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string error = "ConvertFiles " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null);
                    WriteError(error);
                    if (!string.IsNullOrEmpty(convertProcessLog))
                    {
                        SendMail(error);
                    }
                    MessageBox.Show(error);
                }
            }
            catch (Exception ex)
            {
                string error = "ConvertFiles " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null);
                WriteError(error);
                if (!string.IsNullOrEmpty(convertProcessLog))
                {
                    SendMail(convertProcessLog + "\r\n" + "\r\n" + "ERRROR: " + "\r\n" + error);
                }
                MessageBox.Show(error);
            }
        }

        private void ConvertTransferReceiveFiles()
        {
            try
            {
                bool proceed = true;
                string csvFolderPath = "";
                string shopFileFolder = "";
                string csvArchiveFolder = "";
                IEnumerable<ListOfdbRecordsImport> dbconcat = null;
                try
                {
                    if (ui)
                    {
                        Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
                        {
                            txtBkStatus.Text = "Konverteerin leitud tarnete andmebaasi skänneri tarnete andmebaasiks!";
                            prgRing.Visibility = Visibility.Visible;
                        }));
                    }

                    csvFolderPath = lstSettings.First().csvFolder;
                    shopFileFolder = lstSettings.First().shopFileFolder;
                    csvArchiveFolder = lstSettings.First().csvArchiveFolder;
                    string jsonFolderPath = lstSettings.First().jsonFolder;
                    string jsonArchiveFolder = lstSettings.First().jsonFolder + @"\Archive\";


                    if (proceed)
                    {
                        if (lstShopRelations.Any())
                        {
                            string[] dirs = Directory.GetFiles(csvFolderPath);
                            bool isInFolder = false;
                            foreach (string str in dirs)
                            {
                                string sourceFileName = str;
                                int index = str.LastIndexOf("\\");
                                string fileName = str.Substring(index + 1);
                                if (fileName.StartsWith("TRFRCV_"))
                                {
                                    isInFolder = true;
                                }
                            }
                            if (!isInFolder)
                            {
                                convertProcessLog = convertProcessLog + "\r\n" + "TRFRCV csv file was not found";
                                proceed = false;
                            }
                        }
                    }
                    if (proceed)
                    {

                        string[] dirs = Directory.GetFiles(csvFolderPath);
                        int fileCounter = 0;
                        if (dirs.Any())
                        {
                            bool shrcvfilesExist = false;
                            foreach (string str in dirs)
                            {
                                string sourceFileName = str;
                                int index = str.LastIndexOf("\\");
                                string fileName = str.Substring(index + 1);
                                if (fileName.EndsWith(".lock"))
                                {
                                    WriteLog("Lock exists, convert operations skipped", 1);
                                    proceed = false;
                                }
                                if (fileName.ToUpper().EndsWith(".CSV") && fileName.Contains("TRFRCV_"))
                                {
                                    shrcvfilesExist = true;
                                }
                            }

                            if (proceed)
                            {
                                if (shrcvfilesExist)
                                {
                                    var file = new StreamWriter(csvArchiveFolder + Environment.MachineName + ".lock", true);
                                    WriteLog("Lock file created", 1);
                                    file.WriteLine("");
                                    file.Close();
                                    {
                                        foreach (string str in dirs)
                                        {
                                            fileCounter = fileCounter + 1;
                                            if (ui)
                                            {
                                                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
                                                {
                                                    txtBkStatus.Text = "Konverteerin leitud andmebaasi skänneri andmebaasiks! " + "\r\n" + "Loen faili " + fileCounter + "/" + (dirs.Count() - 1);
                                                }));
                                            }
                                            string sourceFileName = str;
                                            int index = str.LastIndexOf("\\");
                                            string fileName = str.Substring(index + 1);
                                            if (fileName.EndsWith(".lock"))
                                            {
                                                proceed = false;
                                            }

                                            if (proceed)
                                            {
                                                if (fileName.ToUpper().EndsWith(".CSV") && fileName.Contains("TRFRCV_"))
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
                                                        if (fileName2.ToUpper().EndsWith(".CSV") && fileName2.Contains("TRFRCV_"))
                                                        {
                                                            if (fileName2 != fileName)
                                                            {
                                                                File.Move(jsonFolderPath + fileName2, jsonArchiveFolder + fileName2);
                                                            }
                                                        }
                                                    }
                                                    if (!File.Exists(jsonFolderPath + fileName))
                                                    {
                                                        var fileNameSplit = fileName.Split(new[] { "TRFRCV_" }, StringSplitOptions.None);
                                                        string datePart = fileNameSplit[1].ToUpper().Replace(".CSV", "").Replace("-", "");
                                                        string formatstring = "yyyyMMddHHmmss";
                                                        DateTime fileDate = DateTime.ParseExact(datePart, formatstring, null);
                                                        ConvertTransferReceiveCsvFileToJsonObject(csvFolderPath, fileName, fileDate);
                                                        proceed = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    proceed = false;
                                }
                            }
                            fileCounter = fileCounter + 1;
                        }
                        else
                        {
                            proceed = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string error = "ConvertFiles " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null);
                    WriteError(error);
                    if (!string.IsNullOrEmpty(convertProcessLog))
                    {
                        SendMail(error);
                    }
                    MessageBox.Show(error);
                }
            }
            catch (Exception ex)
            {
                string error = "ConvertFiles " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null);
                WriteError(error);
                if (!string.IsNullOrEmpty(convertProcessLog))
                {
                    SendMail(convertProcessLog + "\r\n" + "\r\n" + "ERRROR: " + "\r\n" + error);
                }
                MessageBox.Show(error);
            }
        }

        public List<ListOfdbRecords> FillSKUData(List<ListOfdbRecordsImport> lstOfConcat, int countOfConcat)
        {
            try
            {
                //foreach (var r in lstOfConcat)
                //{
                //    if (r.itemCode == "701222")
                //    {
                //        Debug.WriteLine("r.item             " + r.itemCode + "   r.price            " + r.price);
                //        Debug.WriteLine("r.profiklubihind   " + r.itemCode + "   r.profiklubihind   " + r.profiklubihind);
                //        Debug.WriteLine("r.soodushind       " + r.itemCode + "   r.soodushind       " + r.profiklubihind);
                //        Debug.WriteLine("r.meistriklubihind " + r.itemCode + "   r.meistriklubihind " + r.profiklubihind);
                //    }
                //}
                DateTime stamp = DateTime.Now;
                var finalDB = lstOfConcat.GroupBy(x => x.itemCode).Select(s => new ListOfdbRecords
                {
                    fileDate = stamp,
                    itemCode = s.First().itemCode,
                    SKU = GetSKUString(s, countOfConcat),
                    barCode = s.First().barCode,
                    itemDesc = s.First().itemDesc,
                    itemMagnitude = s.First().itemMagnitude,
                    meistriklubihind = s.First().meistriklubihind,
                    price = s.First().price,
                    profiklubihind = s.First().profiklubihind,
                    soodushind = s.First().soodushind,
                    sortiment = s.First().sortiment,
                }).ToList();

                //var p = finalDB.Where(x => x.itemCode == "701222").ToList();
                //if (p.Any())
                //{
                //    if (ui)
                //    {
                //        MessageBox.Show(p.First().price.ToString());
                //    }
                //    else
                //    {
                //        Debug.WriteLine("701222 " + p.First().price.ToString());
                //    }
                //}
                
                return finalDB;
            }
            catch (Exception ex)
            {
                string error = "FillSKUData " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null);
                WriteError(error);
                if (!string.IsNullOrEmpty(convertProcessLog))
                {
                    SendMail(convertProcessLog + "\r\n" + "\r\n" + "ERRROR: " + "\r\n" + error);
                }
                MessageBox.Show(error);
                return new List<ListOfdbRecords>();
            }

            //foreach (var r in finalDB)
            //{
            //    if (r.itemCode == "701222")
            //    {
            //        Debug.WriteLine("r.item             " + r.itemCode + "   r.price            " + r.price);
            //        Debug.WriteLine("r.profiklubihind   " + r.itemCode + "   r.profiklubihind   " + r.profiklubihind);
            //        Debug.WriteLine("r.soodushind       " + r.itemCode + "   r.soodushind       " + r.profiklubihind);
            //        Debug.WriteLine("r.meistriklubihind " + r.itemCode + "   r.meistriklubihind " + r.profiklubihind);
            //    }
            //}
            
        }

        public string GetSKUString(IGrouping<string, ListOfdbRecordsImport> s, int countOfConcat)
        {
            try
            {
                sKUCounter = sKUCounter + 1;
               
                var f = s.ToList();
                var re = "";
                foreach (var o in f)
                {
                    re = re + o.SKU + "###" + (string.IsNullOrEmpty(o.SKUBin) ? "-" : o.SKUBin) + "###" + (o.SKUqty == 0 ? "0" : o.SKUqty.ToString("#.###")) + "%%%";
                }
                if (sKUCounter.ToString().EndsWith("0000") || sKUCounter.ToString().EndsWith("00000") || sKUCounter.ToString().EndsWith("000000"))
                {
                    Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
                    {
                        txtBkStatus.Text = "Konverteerin leitud andmebaasi skänneri andmebaasiks! " + "\r\n" + "Grupeerin failide sisu. Kirje " + sKUCounter + "/" + countOfConcat;
                    }));
                    
                    //WriteLog("sKUCounter: " + sKUCounter, 1);
                    Debug.WriteLine(sKUCounter + ": " + re);
                }
                return re;
            }
            catch (Exception ex)
            {
                string error = "GetSKUString " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null);
                WriteError(error);
                if (!string.IsNullOrEmpty(convertProcessLog))
                {
                    SendMail(convertProcessLog + "\r\n" + "\r\n" + "ERRROR: " + "\r\n" + error);
                }
                MessageBox.Show(error);

                return "";
            }
        }

        private void bw_RunWorkerCompleted_ConvertFiles(object sender, RunWorkerCompletedEventArgs e)
        {
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
            {
                txtBkStatus.Text = "";
                prgRing.Visibility = Visibility.Hidden;
                GetLatestDBFile();
                //DisplayPostedMessage("ANDMEBAAS VALMIS!");
            }));
        }

        public static ListOfShopRelations FromShopRelationCsv(string csvLine, string fileName, string logFolder, string adminEmail, string senderEmail, string smtpServer)
        {
            ListOfShopRelations lst = new ListOfShopRelations();
            try
            {
                
                string[] values = csvLine.Split(',');
                string val = values[0].Replace("\"", "");
                lst.shopID = val.Length  == 1 ? ("0" + val.ToString()) : val;
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
                    var SmtpServer = new SmtpClient(smtpServer);
                    mail.From = new MailAddress(senderEmail);
                    mail.To.Add(adminEmail);

                    mail.Subject = "BauhofOffline error from " + Environment.MachineName;
                    mail.Body = errroToWrite;
                    SmtpServer.Send(mail);
                }

                MessageBox.Show("FromShopRelationCsv  " + ex.Message);
                return lst;
            }
        }

        public static ListOfPurchaseReceive FromPurchaseReceiveCsv(string csvLine, string fileName, string logFolder, string adminEmail, string senderEmail, string smtpServer)
        {
            ListOfPurchaseReceive lst = new ListOfPurchaseReceive();
            if (!string.IsNullOrEmpty(csvLine))
            {
               
                int step = 0;
                try
                {
                    step = 1;
                    decimal temp = 0;
                    
                    string[] values = csvLine.Split("\"" + "," + "\"");
                    string x = "pood " + values[0];
                    x = x + "\r\n" + "dokno " + values[1];
                    x = x + "\r\n" + "dokreanr " + values[2];
                    x = x + "\r\n" + "hankijakood " + values[3];
                    x = x + "\r\n" + "hankijanimi " + values[4];
                    x = x + "\r\n" + "hankijaviide " + values[5];
                    x = x + "\r\n" + "tarnekp " + values[6];
                    x = x + "\r\n" + "kaup " + values[7];
                    x = x + "\r\n" + "kogus " + values[8];
                    x = x + "\r\n" + "magnitude " + values[9];

                    //MessageBox.Show(x);
                    step = 2;
                    lst.pood = string.IsNullOrEmpty(values[0]) ? "" : values[0].Replace("\"", "");

                    step = 3;
                    lst.dokno = string.IsNullOrEmpty(values[1]) ? "" : values[1].Replace("\"", "");

                    step = 4;
                    lst.dokreanr = string.IsNullOrEmpty(values[2]) ? "" : values[2].Replace("\"", "");

                    step = 5;
                    lst.hankijakood = string.IsNullOrEmpty(values[3]) ? "" : values[3].Replace("\"", "");

                    step = 6;
                    lst.hankijanimi = string.IsNullOrEmpty(values[4]) ? "" : values[4].Replace("\"", "");

                    step = 7;
                    lst.hankijaviide = string.IsNullOrEmpty(values[5]) ? "" : values[5].Replace("\"", "");

                    step = 8;
                    lst.tarnekp = string.IsNullOrEmpty(values[6]) ? Convert.ToDateTime("1753-01-01 00:00:00") : Convert.ToDateTime(values[6].Replace("\"", ""));

                    step = 9;
                    lst.kaup = string.IsNullOrEmpty(values[7]) ? "" : values[7].Replace("\"", "");


                    step = 10;
                    var cultureInfo = CultureInfo.InvariantCulture;
                    NumberStyles styles = NumberStyles.AllowDecimalPoint;

                    values[8] = string.IsNullOrEmpty(values[8]) ? "0" : values[8].Replace(",", ".").Replace("\"", "");
                    try
                    {
                        lst.kogus = decimal.Parse(values[8], cultureInfo);
                    }
                    catch (Exception ex)
                    {
                        lst.kogus = 0;
                        Debug.WriteLine("kogus " + ex.Message);
                    }
                    step = 11;
                    lst.magnitude = string.IsNullOrEmpty(values[9]) ? "" : values[9].Replace("\"", "");
                    
                    return lst;
                }
                catch (Exception ex)
                {
                    string message = ("FromCsv " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
                    string errroToWrite = "" + "\r\n" +
                        String.Format("{0:dd.MM.yyyy HH:mm:ss}", DateTime.Now) + "\r\n" +
                        "Hostname: " + Environment.MachineName + " Username:" + Environment.UserName + "\r\n" +
                        "" + "\r\n" +
                        "step: " + step + " line:  " + csvLine + "\r\n" + message + "\r\n" +
                        "================";

                    var str = new StreamWriter(logFolder + Environment.MachineName + "_Error.txt", true);
                    str.WriteLine(errroToWrite);
                    str.Close();

                    if (!string.IsNullOrEmpty(adminEmail))
                    {
                        var mail = new MailMessage();
                        var SmtpServer = new SmtpClient(smtpServer);
                        mail.From = new MailAddress(senderEmail);
                        mail.To.Add(adminEmail);

                        mail.Subject = "BauhofOffline error from " + Environment.MachineName;
                        mail.Body = errroToWrite;
                        //SmtpServer.Send(mail);
                    }
                    MessageBox.Show("FromCsv  " + ex.Message);
                    return lst;
                }
            }
            else
            {
                return lst;
            }
        }

        public static ListOfTransferReceive FromTransferReceiveCsv(string csvLine, string fileName, string logFolder, string adminEmail, string senderEmail, string smtpServer)
        {
            ListOfTransferReceive lst = new ListOfTransferReceive();
            if (!string.IsNullOrEmpty(csvLine))
            {

                int step = 0;
                try
                {
                    step = 1;
                    decimal temp = 0;

                    string[] values = csvLine.Split("\"" + "," + "\"");
                    string x = "pood " + values[0];
                    x = x + "\r\n" + "dokno " + values[1];
                    x = x + "\r\n" + "dokreanr " + values[2];
                    x = x + "\r\n" + "lähetaja " + values[3];
                    x = x + "\r\n" + "tarnekp " + values[4];
                    x = x + "\r\n" + "kaup " + values[5];
                    x = x + "\r\n" + "kogus " + values[6];
                    x = x + "\r\n" + "magnitude " + values[7];

                    //MessageBox.Show(x);
                    step = 2;
                    lst.pood = string.IsNullOrEmpty(values[0]) ? "" : values[0].Replace("\"", "");

                    step = 3;
                    lst.dokno = string.IsNullOrEmpty(values[1]) ? "" : values[1].Replace("\"", "");

                    step = 4;
                    lst.dokreanr = string.IsNullOrEmpty(values[2]) ? "" : values[2].Replace("\"", "");

                    step = 5;
                    lst.lähetaja = string.IsNullOrEmpty(values[3]) ? "" : values[3].Replace("\"", "");

                    
                    step = 6;
                    lst.tarnekp = string.IsNullOrEmpty(values[4]) ? Convert.ToDateTime("1753-01-01 00:00:00") : Convert.ToDateTime(values[4].Replace("\"", ""));

                    step = 7;
                    lst.kaup = string.IsNullOrEmpty(values[5]) ? "" : values[5].Replace("\"", "");


                    step = 8;
                    var cultureInfo = CultureInfo.InvariantCulture;
                    NumberStyles styles = NumberStyles.AllowDecimalPoint;

                    values[6] = string.IsNullOrEmpty(values[6]) ? "0" : values[6].Replace(",", ".").Replace("\"", "");
                    try
                    {
                        lst.kogus = decimal.Parse(values[6], cultureInfo);
                    }
                    catch (Exception ex)
                    {
                        lst.kogus = 0;
                        Debug.WriteLine("kogus " + ex.Message);
                    }
                    step = 9;
                    lst.magnitude = string.IsNullOrEmpty(values[7]) ? "" : values[7].Replace("\"", "");

                    return lst;
                }
                catch (Exception ex)
                {
                    string message = ("FromCsv " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
                    string errroToWrite = "" + "\r\n" +
                        String.Format("{0:dd.MM.yyyy HH:mm:ss}", DateTime.Now) + "\r\n" +
                        "Hostname: " + Environment.MachineName + " Username:" + Environment.UserName + "\r\n" +
                        "" + "\r\n" +
                        "step: " + step + " line:  " + csvLine + "\r\n" + message + "\r\n" +
                        "================";

                    var str = new StreamWriter(logFolder + Environment.MachineName + "_Error.txt", true);
                    str.WriteLine(errroToWrite);
                    str.Close();

                    if (!string.IsNullOrEmpty(adminEmail))
                    {
                        var mail = new MailMessage();
                        var SmtpServer = new SmtpClient(smtpServer);
                        mail.From = new MailAddress(senderEmail);
                        mail.To.Add(adminEmail);

                        mail.Subject = "BauhofOffline error from " + Environment.MachineName;
                        mail.Body = errroToWrite;
                        //SmtpServer.Send(mail);
                    }
                    MessageBox.Show("FromCsv  " + ex.Message);
                    return lst;
                }
            }
            else
            {
                return lst;
            }
        }


        public static ListOfdbRecordsImport FromCsv(string csvLine, string fileName, DateTime fileDate, string logFolder, string adminEmail, string senderEmail, string smtpServer)
        {
            ListOfdbRecordsImport lst = new ListOfdbRecordsImport();
            if (!string.IsNullOrEmpty(csvLine))
            {
                int step = 0;
                try
                {
                    step = 1;
                    decimal temp = 0;
                    string[] values = csvLine.Split("\"" + "," + "\"");
                    lst.fileName = fileName;
                    lst.fileDate = fileDate;

                    step = 2;
                    lst.itemCode = string.IsNullOrEmpty(values[0]) ? "" : values[0].Replace("\"", "");

                    step = 3;
                    lst.itemDesc = string.IsNullOrEmpty(values[1]) ? "" : values[1].Replace("\"", "");

                    step = 4;
                    lst.itemMagnitude = string.IsNullOrEmpty(values[2]) ? "" : values[2].Replace("\"", "");

                    step = 5;
                    var cultureInfo = CultureInfo.InvariantCulture;
                    NumberStyles styles = NumberStyles.AllowDecimalPoint;

                    step = 6;
                    values[3] = string.IsNullOrEmpty(values[3]) ? "0" : values[3].Replace(",", ".").Replace("\"", "");
                    try
                    {
                        lst.price = decimal.Parse(values[3], cultureInfo);
                    }
                    catch (Exception ex)
                    {
                        lst.price = 0;
                        Debug.WriteLine("price " + ex.Message);
                    }

                    step = 7;
                    lst.SKU = string.IsNullOrEmpty(values[4]) ? "" : values[4].Replace("\"", "");

                    step = 8;
                    values[6] = string.IsNullOrEmpty(values[6]) ? "0" : values[6].Replace(",", ".").Replace("\"", "");
                    try
                    {
                        lst.SKUqty = decimal.Parse(values[6], cultureInfo);
                    }
                    catch (Exception ex)
                    {
                        lst.SKUqty = 0;
                        Debug.WriteLine("SKUqty " + ex.Message);
                    }

                    step = 9;
                    values[7] = string.IsNullOrEmpty(values[7]) ? "0" : values[7].Replace(",", ".").Replace("\"", "");
                    try
                    {
                        lst.meistriklubihind = decimal.Parse(values[7], cultureInfo);
                    }
                    catch (Exception ex)
                    {
                        lst.meistriklubihind = 0;
                        Debug.WriteLine("meistriklubihind "  + values[7]  + " "  + ex.Message);
                    }

                    step = 10;
                    values[8] = string.IsNullOrEmpty(values[8]) ? "0" : values[8].Replace(",", ".").Replace("\"", "");
                    try
                    {
                        lst.soodushind = decimal.Parse(values[8], cultureInfo);
                    }
                    catch (Exception ex)
                    {
                        lst.soodushind = 0;
                        Debug.WriteLine("soodushind " + ex.Message);
                    }

                    step = 11;
                    values[9] = string.IsNullOrEmpty(values[9]) ? "0" : values[9].Replace(",", ".").Replace("\"", "");
                    try
                    {
                        lst.profiklubihind = decimal.Parse(values[9], cultureInfo);
                    }
                    catch (Exception ex)
                    {
                        lst.profiklubihind = 0;
                        Debug.WriteLine("profiklubihind " + ex.Message);
                    }

                    step = 12;
                    lst.sortiment = string.IsNullOrEmpty(values[10]) ? "" : values[10].Replace("\"", "");

                    step = 13;
                    lst.SKUBin = string.IsNullOrEmpty(values[12]) ? "" : values[12].Replace("\"", "");

                    step = 14;
                    lst.barCode = string.IsNullOrEmpty(values[13]) ? "" : values[13].Replace("\"", "");

                    //if (values[0].Contains("701222"))
                    //{
                    //    Debug.WriteLine("values[0] " + values[0] + "  lst.itemCode:" + lst.itemCode);
                    //    Debug.WriteLine("values[1] " + values[1] + "  lst.itemDesc:" + lst.itemDesc);
                    //    Debug.WriteLine("values[2] " + values[2] + "  lst.itemMagnitude:" + lst.itemMagnitude);
                    //    Debug.WriteLine("values[3] " + values[3] + "  lst.price:" + lst.price);
                    //    Debug.WriteLine("values[4] " + values[4] + "  lst.SKU:" + lst.SKU);
                    //    Debug.WriteLine("values[5] " + values[5] + "  config");
                    //    Debug.WriteLine("values[6] " + values[6] + "  lst.SKUqty:" + lst.SKUqty);
                    //    Debug.WriteLine("values[7] " + values[7] + "  lst.soodushind:" + lst.meistriklubihind);
                    //    Debug.WriteLine("values[8] " + values[8] + "  lst.soodushind:" + lst.soodushind);
                    //    Debug.WriteLine("values[9] " + values[9] + "  lst.profiklubihind:" + lst.profiklubihind);
                    //    Debug.WriteLine("values[10] " + values[10] + "  lst.sortiment:" + lst.sortiment);
                    //    Debug.WriteLine("values[11] " + values[11] + "  lst.product");
                    //    Debug.WriteLine("values[12] " + values[12] + "  lst.SKUBin:" + lst.SKUBin);
                    //    Debug.WriteLine("values[13] " + values[13] + "  lst.barCode:" + lst.barCode);

                    step = 15;
                    //    }
                    return lst;
                }
                catch (Exception ex)
                {
                    string message = ("FromCsv " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
                    string errroToWrite = "" + "\r\n" +
                        String.Format("{0:dd.MM.yyyy HH:mm:ss}", DateTime.Now) + "\r\n" +
                        "Hostname: " + Environment.MachineName + " Username:" + Environment.UserName + "\r\n" +
                        "" + "\r\n" +
                        "step: " + step + " line:  " + csvLine + "\r\n" + message + "\r\n" +
                        "================";

                    var str = new StreamWriter(logFolder + Environment.MachineName + "_Error.txt", true);
                    str.WriteLine(errroToWrite);
                    str.Close();

                    if (!string.IsNullOrEmpty(adminEmail))
                    {
                        var mail = new MailMessage();
                        var SmtpServer = new SmtpClient(smtpServer);
                        mail.From = new MailAddress(senderEmail);
                        mail.To.Add(adminEmail);

                        mail.Subject = "BauhofOffline error from " + Environment.MachineName;
                        mail.Body = errroToWrite;
                        //SmtpServer.Send(mail);
                    }
                    MessageBox.Show("FromCsv  " + ex.Message);
                    return lst;
                }
            }
            else
            {
                return lst;
            }
        }

        private async Task DisplayPostedMessage(string message)
        {
            try
            {
               

                    grdMessagePosted.Visibility = Visibility.Visible;
                    txtbkMessagePosted.Text = message;
                    grdMessagePosted.Opacity = 1.0;
                    for (double opacity = 1.0; opacity >= 0.0; opacity = opacity - .03)
                    {
                        if (opacity >= 0.1)
                        {
                            await Task.Delay(40);
                            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
                            {
                                grdMessagePosted.Opacity = opacity;
                            }));
                        }
                        else
                        {
                            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
                            {
                                grdMessagePosted.Opacity = 0;
                                grdMessagePosted.Visibility = Visibility.Collapsed;
                                txtbkMessagePosted.Text = "";
                            }));
                            break;
                        }
                    }
                
            }
            catch (Exception ex)
            {
                //SendDebugErrorMessage(this.GetType().Name, "DisplayPostedMessage", ex);
            }
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
                if (confCollection["exportFolder"] != null)
                {
                    row.exportFolder = confCollection["exportFolder"].Value.ToString();
                    WriteLog("GetConfiguration exportFolder = " + row.exportFolder, 2);
                }
                if (confCollection["smtpServer"] != null)
                {
                    row.smtpServer = confCollection["smtpServer"].Value.ToString();
                    WriteLog("GetConfiguration smtpServer = " + row.smtpServer, 2);
                }
                if (confCollection["debugLevel"] != null)
                {
                    row.debugLevel = Convert.ToInt32(confCollection["debugLevel"].Value.ToString());
                    WriteLog("GetConfiguration debugLevel = " + row.debugLevel, 2);
                }
                if (confCollection["shopFileFolder"] != null)
                {
                    row.shopFileFolder = confCollection["shopFileFolder"].Value.ToString();
                    WriteLog("GetConfiguration shopFileFolder = " + row.debugLevel, 2);
                }
                if (confCollection["csvArchiveFolder"] != null)
                {
                    row.csvArchiveFolder = confCollection["csvArchiveFolder"].Value.ToString();
                    WriteLog("GetConfiguration csvArchiveFolder = " + row.debugLevel, 2);
                }
                if (confCollection["senderEmail"] != null)
                {
                    row.senderEmail = confCollection["senderEmail"].Value.ToString();
                    WriteLog("GetConfiguration senderEmail = " + row.debugLevel, 2);
                }

                
                lstSettings.Add(row);
                WriteLog("GetConfiguration complete", 1);
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
                        message + "\r\n" + 
                        "================";

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
                MessageBox.Show("WriteLog " + lstSettings.First().logFolder + " " + "\r\n" + ex.Message);
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
                    var SmtpServer = new SmtpClient(lstSettings.First().smtpServer);
                    mail.From = new MailAddress(lstSettings.First().senderEmail);
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

        private void BtnOpenUpload_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(lstSettings.First().jsonFolder);
        }

        private void BtnOpenDownload_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(lstSettings.First().exportFolder);
        }
    }
}
