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
        public int sKUCounter = 0;
        private readonly BackgroundWorker bw = new BackgroundWorker();

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
        #endregion

        #region Main
        public MainWindow()
        {
            InitializeComponent();
            prgRing.Visibility = Visibility.Hidden;
            GetConfiguration();
            CheckVersion();
            ConvertFilesStart();            
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
                        bool fileExists = false;
                        device.Connect();
                        WriteLog("CheckVersion device connected", 2);
                        var photoDir = device.GetDirectoryInfo(@"\Internal shared storage\Download");
                        var files = photoDir.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly);
                        foreach (var file in files)
                        {
                           if (file.Name.ToUpper() == "VERSION.TXT")
                           {
                                fileExists = true;
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
                        if (!fileExists)
                        {
                            txtBkScannerVersionValue.Text = "PUUDUB";
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
                        var photoDir = device.GetDirectoryInfo(@"\Internal shared storage\DCIM\Export");
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

                        var photoDir = device.GetDirectoryInfo(@"\Internal shared storage\DCIM\");
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
                            else
                            {
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
                            var dcimDownload = device.GetDirectoryInfo(@"\IPSM card\Download");
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

        public string ConvertCsvFileToJsonObjectToLarge(string folderPath, string inputFileName, DateTime inputFileDate, int fileCounter)
        {
            try
            {
                Debug.WriteLine("folderPath " + fileCounter);
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                string filename = folderPath + inputFileName;
                Debug.WriteLine("filename:" + filename);
                if (!Directory.Exists(folderPath + @"\Archive\"))
                {
                    Directory.CreateDirectory(folderPath + @"\Archive\");
                }
                if (!File.Exists(folderPath + @"\Archive\" + inputFileName.Replace(".csv", ".convert")))
                {
                    
                    File.WriteAllText(folderPath + @"\Archive\" + inputFileName.Replace(".csv", ".convert"), DateTime.Now.ToString());
                    List<ListOfdbRecordsImport> values = File.ReadAllLines(folderPath + inputFileName).Skip(1).Select(v => FromCsv(v, inputFileName, inputFileDate, lstSettings.First().logFolder, lstSettings.First().adminEmail)).ToList();
                    if (fileCounter == 1)
                    {
                        lstDB01 = values;
                        Debug.WriteLine("lstDB01 values count:" + values.Count());
                    }
                    if (fileCounter == 2)
                    {
                        lstDB02 = values;
                        Debug.WriteLine("lstDB02 values count:" + values.Count());
                    }
                    if (fileCounter == 3)
                    {
                        lstDB03 = values;
                        Debug.WriteLine("lstDB03 values count:" + values.Count());
                    }
                    if (fileCounter == 4)
                    {
                        lstDB04 = values;
                        Debug.WriteLine("lstDB04 values count:" + values.Count());
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
                File.Move(filename, folderPath + @"\Archive\" + inputFileName);
                return "";
            }
            catch (Exception ex)
            {
                WriteError("ConvertCsvFileToJsonObject " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
                MessageBox.Show("ConvertCsvFileToJsonObject  " + ex.Message);
                return "";
            }
        }

        public string ConvertCsvFileToJsonObject(string folderPath, string inputFileName, DateTime inputFileDate)
        {
            try
            {
                string filename = folderPath + inputFileName;
               
                File.WriteAllText(folderPath + @"\Archive\" + inputFileName.Replace(".csv", ".convert"), DateTime.Now.ToString());
                List<ListOfShopRelations> values = File.ReadAllLines(folderPath + inputFileName).Skip(1).Select(v => FromShopRelationCsv(v, inputFileName, lstSettings.First().logFolder, lstSettings.First().adminEmail)).ToList();
                lstShopRelations = values;
                Debug.WriteLine("lstShopRelations.Count() " + lstShopRelations.Count());
                string outputFile = inputFileName.Replace(".csv", ".txt").ToUpper();
                string json = JsonConvert.SerializeObject(values);
                File.WriteAllText(lstSettings.First().jsonFolder + outputFile, json);
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

        public void GetLatestDBFile()
        {
            string[] dirs = Directory.GetFiles(lstSettings.First().jsonFolder);
            if (dirs.Any())
            {
                foreach (string str in dirs)
                {
                    
                    string sourceFileName = str;
                    int index = str.LastIndexOf("\\");
                    string fileName = str.Substring(index + 1);
                    Debug.WriteLine(fileName);
                    if (fileName.ToUpper().StartsWith("DBRECORDS_") && fileName.ToUpper().EndsWith(".TXT"))
                    {
                        string fileVersionFull = fileName.Replace("DBRECORDS_", "").Replace(".TXT", "");
                        string fileVersion = fileVersionFull.Substring(6, 2) + "." + fileVersionFull.Substring(4, 2) + "." + fileVersionFull.Substring(0, 4) + " " + fileVersionFull.Substring(9, 2) + ":" + fileVersionFull.Substring(11, 2) + ":" + fileVersionFull.Substring(13, 2);
                        Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
                        {
                            txtBkLatestFile.Text = "ANDMEBAASI VERSIOON: " + fileVersion;
                        }));
                    }
                }
            }
            
        }

        private void bw_DoWork_ConvertFiles(object sender, DoWorkEventArgs e)
        {
            GetLatestDBFile();
            bool proceed = true;
            string csvFolderPath = "";
            IEnumerable<ListOfdbRecordsImport> dbconcat = null;
            try
            {
                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
                {
                    txtBkStatus.Text = "Konverteerin leitud andmebaasi skänneri andmebaasiks!";
                    prgRing.Visibility = Visibility.Visible;
                }));

                csvFolderPath = lstSettings.First().csvFolder;
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

                
                if (proceed)
                {
                    if (File.Exists(csvFolderPath + relationFileName))
                    {
                        var l = ConvertCsvFileToJsonObject(csvFolderPath, relationFileName, DateTime.Now);
                        //lstShopRelations = File.ReadAllLines(csvFolderPath + relationFileName).Select(v => FromShopRelationCsv(v, relationFileName, lstSettings.First().logFolder, lstSettings.First().adminEmail)).ToList();
                        string[] dirs = Directory.GetFiles(csvFolderPath);
                        int fileCounter = 0;
                        if (dirs.Any())
                        {
                            bool dbfilesExist = false;
                            foreach (string str in dirs)
                            {
                                string sourceFileName = str;
                                int index = str.LastIndexOf("\\");
                                string fileName = str.Substring(index + 1);
                                if (fileName.EndsWith(".lock"))
                                {
                                    proceed = false;
                                }
                                if (fileName.EndsWith(".csv") && fileName.Contains("_PDA_Products"))
                                {
                                    dbfilesExist = true;
                                }
                            }
                            
                            if (proceed)
                            {
                                if (dbfilesExist)
                                {
                                    Debug.WriteLine("A1");
                                    var file = new StreamWriter(csvFolderPath + Environment.MachineName + ".lock", true);
                                    Debug.WriteLine("A2");
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
                            MessageBox.Show("bw_DoWork_ConvertFiles: csv faile ei leitud kataloogist " + csvFolderPath);
                        }
                    }
                    else
                    {
                        MessageBox.Show("bw_DoWork_ConvertFiles: ShopRelations.csv nimeline fail puudub csv kataloogist " + csvFolderPath + "!");
                    }
                }
                if (proceed)
                {
                    Debug.WriteLine("started concat");
                    if (lstDB01.Any())
                    {
                        Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
                        {
                            txtBkStatus.Text = "Konverteerin leitud andmebaasi skänneri andmebaasiks! " + "\r\n" + "Liidan loetud failide sisu";
                        }));

                        Debug.WriteLine("started concat 1");
                       
                        Debug.WriteLine("started concat 2");
                        if (lstDB02.Any())
                        {
                            Debug.WriteLine("started concat 3");
                            Debug.Write("started lstDB02");
                            var d1 = lstDB01.Concat(lstDB02);
                            dbconcat = d1;
                            Debug.WriteLine("CONCAT1 " + d1.Count());
                        }
                        if (lstDB03.Any())
                        {
                            var d2 = dbconcat.Concat(lstDB03);
                            dbconcat = d2;
                            Debug.WriteLine("CONCAT2 " + d2.Count());
                        }

                        if (lstDB04.Any())
                        {
                            var d3 = dbconcat.Concat(lstDB04);
                            dbconcat = d3;
                            Debug.WriteLine("CONCAT3 " + d3.Count());
                        }

                        if (lstDB05.Any())
                        {
                            var d4 = dbconcat.Concat(lstDB05);
                            dbconcat = d4;
                            Debug.WriteLine("CONCAT4 " + d4.Count());
                        }

                        if (lstDB06.Any())
                        {
                            var d5 = dbconcat.Concat(lstDB06);
                            dbconcat = d5;
                            Debug.WriteLine("CONCAT5 " + d5.Count());
                        }

                        if (lstDB07.Any())
                        {
                            var d6 = dbconcat.Concat(lstDB07);
                            dbconcat = d6;
                            Debug.WriteLine("CONCAT6 " + d6.Count());
                        }

                        if (lstDB08.Any())
                        {
                            var d7 = dbconcat.Concat(lstDB08);
                            dbconcat = d7;
                            Debug.WriteLine("CONCAT7 " + d7.Count());
                        }

                        if (lstDB09.Any())
                        {
                            var d8 = dbconcat.Concat(lstDB09);
                            dbconcat = d8;
                            Debug.WriteLine("CONCAT8 " + d8.Count());
                        }

                        if (lstDB10.Any())
                        {
                            var d9 = dbconcat.Concat(lstDB10);
                            dbconcat = d9;
                            Debug.WriteLine("CONCAT9 " + d9.Count());
                        }

                        if (lstDB11.Any())
                        {
                            var d10 = dbconcat.Concat(lstDB11);
                            dbconcat = d10;
                            Debug.WriteLine("CONCAT10 " + d10.Count());
                        }

                        if (lstDB12.Any())
                        {
                            var d11 = dbconcat.Concat(lstDB12);
                            dbconcat = d11;
                            Debug.WriteLine("CONCAT11 " + d11.Count());
                        }

                        if (lstDB13.Any())
                        {
                            var d12 = dbconcat.Concat(lstDB13);
                            dbconcat = d12;
                            Debug.WriteLine("CONCAT12 " + d12.Count());
                        }

                        if (lstDB14.Any())
                        {
                            var d13 = dbconcat.Concat(lstDB14);
                            dbconcat = d13;
                            Debug.WriteLine("CONCAT13 " + d13.Count());
                        }

                        if (lstDB15.Any())
                        {
                            var d14 = dbconcat.Concat(lstDB15);
                            dbconcat = d14;
                            Debug.WriteLine("CONCAT14 " + d14.Count());
                        }

                        if (lstDB16.Any())
                        {
                            var d15 = dbconcat.Concat(lstDB16);
                            dbconcat = d15;
                            Debug.WriteLine("CONCAT15 " + d15.Count());
                        }

                        if (lstDB17.Any())
                        {
                            var d16 = dbconcat.Concat(lstDB17);
                            dbconcat = d16;
                            Debug.WriteLine("CONCAT16 " + d16.Count());
                        }

                        if (lstDB18.Any())
                        {
                            var d17 = dbconcat.Concat(lstDB18);
                            dbconcat = d17;
                            Debug.WriteLine("CONCAT17 " + d17.Count());
                        }

                        if (lstDB19.Any())
                        {
                            var d18 = dbconcat.Concat(lstDB19);
                            dbconcat = d18;
                            Debug.WriteLine("CONCAT18 " + d18.Count());
                        }

                        if (lstDB20.Any())
                        {
                            var d19 = dbconcat.Concat(lstDB20);
                            dbconcat = d19;
                            Debug.WriteLine("CONCAT19 " + d19.Count());
                        }


                        if (lstDB21.Any())
                        {
                            var d20 = dbconcat.Concat(lstDB21);
                            dbconcat = d20;
                            Debug.WriteLine("CONCAT20 " + d20.Count());
                        }

                        if (lstDB22.Any())
                        {
                            var d21 = dbconcat.Concat(lstDB22);
                            dbconcat = d21;
                            Debug.WriteLine("CONCAT21 " + d21.Count());
                        }

                        if (lstDB23.Any())
                        {
                            var d22 = dbconcat.Concat(lstDB23);
                            dbconcat = d22;
                            Debug.WriteLine("CONCAT22 " + d22.Count());
                        }

                        if (lstDB24.Any())
                        {
                            var d23 = dbconcat.Concat(lstDB24);
                            dbconcat = d23;
                            Debug.WriteLine("CONCAT23 " + d23.Count());
                        }

                        if (lstDB25.Any())
                        {
                            var d24 = dbconcat.Concat(lstDB25);
                            dbconcat = d24;
                            Debug.WriteLine("CONCAT24 " + d24.Count());
                        }

                        if (lstDB26.Any())
                        {
                            var d25 = dbconcat.Concat(lstDB26);
                            dbconcat = d25;
                            Debug.WriteLine("CONCAT25 " + d25.Count());
                        }

                        if (lstDB27.Any())
                        {
                            var d26 = dbconcat.Concat(lstDB27);
                            dbconcat = d26;
                            Debug.WriteLine("CONCAT26 " + d26.Count());
                        }

                        if (lstDB28.Any())
                        {
                            var d27 = dbconcat.Concat(lstDB28);
                            dbconcat = d27;
                            Debug.WriteLine("CONCAT27 " + d27.Count());
                        }

                        if (lstDB29.Any())
                        {
                            var d28 = dbconcat.Concat(lstDB29);
                            dbconcat = d28;
                            Debug.WriteLine("CONCAT28 " + d28.Count());
                        }

                        if (lstDB30.Any())
                        {
                            var d29 = dbconcat.Concat(lstDB30);
                            dbconcat = d29;
                            Debug.WriteLine("CONCAT29 " + d29.Count());
                        }

                        
                        
                    }
                }
                if (proceed)
                {
                    var lstOfConcat = dbconcat.ToList();
                    List<ListOfdbRecords> finalDB = null;
                    sKUCounter = 0;
                    Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
                    {
                        txtBkStatus.Text = "Konverteerin leitud andmebaasi skänneri andmebaasiks! " + "\r\n" + "Grupeerin failide sisu";
                    }));
                    var countOfConcat = lstOfConcat.GroupBy(x => x.itemCode).ToList().Count();
                        
                    finalDB = FillSKUData(lstOfConcat, countOfConcat);

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
                                File.Move(lstSettings.First().jsonFolder + @"\" + fileName, lstSettings.First().jsonFolder + @"\Archive\" + fileName);
                            }
                        }
                    }


                    File.WriteAllText(lstSettings.First().jsonFolder + outputFile.ToUpper(), jsonFinal);
                    Debug.WriteLine("CONCAT final " + dbconcat.Count() + " "  + csvFolderPath);
                    File.Delete(csvFolderPath + Environment.MachineName + ".lock");
                    Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
                    {
                        DisplayPostedMessage("ANDMEBAAS VALMIS!");
                    }));



                }
            }
            catch (Exception ex)
            {
                WriteError("bw_DoWork_ConvertFiles: " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
                MessageBox.Show("bw_DoWork_ConvertFiles: " + ex.Message);
            }
            
        }

        public List<ListOfdbRecords> FillSKUData(List<ListOfdbRecordsImport> lstOfConcat, int countOfConcat)
        {
            var finalDB = lstOfConcat.GroupBy(x => x.itemCode).Select(s => new ListOfdbRecords
            {
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
            return finalDB;
        }

        public string GetSKUString(IGrouping<string, ListOfdbRecordsImport> s, int countOfConcat)
        {
            sKUCounter = sKUCounter + 1;
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new System.Threading.ThreadStart(delegate
            {
                txtBkStatus.Text = "Konverteerin leitud andmebaasi skänneri andmebaasiks! " + "\r\n" + "Grupeerin failide sisu. Kirje " + sKUCounter + "/" + countOfConcat;
            }));
            var f = s.ToList();
            var re = "";
            foreach (var o in f)
            {
                re = re + o.SKU + "###" + (string.IsNullOrEmpty(o.SKUBin) ? "-" : o.SKUBin) + "###" + (o.SKUqty == 0 ? "0" : o.SKUqty.ToString("#.###")) + "%%%";
            }
            Debug.WriteLine(sKUCounter + "Ree: " + re);
            return re;
           
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

        public static ListOfShopRelations FromShopRelationCsv(string csvLine, string fileName, string logFolder, string adminEmail)
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
                decimal temp = 0;
                string[] values = csvLine.Split("\"" + "," + "\"");
                lst.fileName = fileName;
                lst.fileDate = fileDate;

                
                lst.itemCode = values[0].Replace("\"", "");
                lst.itemDesc = values[1].Replace("\"", "");
                lst.itemMagnitude = values[2].Replace("\"", "");

                values[3] = string.IsNullOrEmpty(values[3]) ? "0" : values[6].Replace(",", ".").Replace("\"", "");
                lst.price = Decimal.TryParse(values[3], out temp) ? Convert.ToDecimal(values[3]) : 0;

                lst.SKU = values[4].Replace("\"", "");

                values[6] = string.IsNullOrEmpty(values[6]) ? "0" : values[6].Replace(",", ".").Replace("\"", "");
                lst.SKUqty = Decimal.TryParse(values[6], out temp) ? Convert.ToDecimal(values[6]) : 0;

                values[7] = string.IsNullOrEmpty(values[7]) ? "0" : values[6].Replace(",", ".").Replace("\"", "");
                lst.meistriklubihind = Decimal.TryParse(values[7], out temp) ? Convert.ToDecimal(values[7]) : 0;

                values[8] = string.IsNullOrEmpty(values[8]) ? "0" : values[6].Replace(",", ".").Replace("\"", "");
                lst.soodushind = Decimal.TryParse(values[8], out temp) ? Convert.ToDecimal(values[8]) : 0;

                values[9] = string.IsNullOrEmpty(values[9]) ? "0" : values[6].Replace(",", ".").Replace("\"", "");
                lst.profiklubihind = Decimal.TryParse(values[9], out temp) ? Convert.ToDecimal(values[9]) : 0;

                lst.sortiment = values[10].Replace("\"", "");
                lst.SKUBin = values[12].Replace("\"", "");
                lst.barCode = values[13].Replace("\"", "");

                //if (values[0].Contains("000181"))
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

                    
                //}
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

    }
}
