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

namespace BauhofOffline
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CheckVersion();
        }

        private void CheckVersion()
        {
            txtBkLatestVersionValue.Text = "";
            txtBkScannerVersionValue.Text = "";
            btnUpdate.Visibility = Visibility.Collapsed;
            string DeviceNameAsSeenInMyComputer = "";
            MediaDevice device = null;
            Debug.WriteLine("trying to connect...");
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
                    txtBkStatus.Text =  "LEITI ROHKEM KUI 1 ÜHENDATUD VÄLINE SEADE!";
                }

            }
            else
            {
                txtBkStatus.Text = "VÄLIST SEADET EI LEITUD!";
            }
            Debug.WriteLine("DeviceNameAsSeenInMyComputer " + DeviceNameAsSeenInMyComputer);
            if (!string.IsNullOrEmpty(DeviceNameAsSeenInMyComputer))
            {
                bool proceed = true;
                int count = 0;

                try
                {
                    device.Connect();
                    
                    var photoDir = device.GetDirectoryInfo(@"\Internal shared storage\Download");
                    var files = photoDir.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly);
                    Debug.WriteLine("===1");
                    foreach (var file in files)
                    {
                        
                        if (file.Name.ToUpper() == "VERSION.TXT")
                        {
                            count = count + 1;
                            string destinationFileName = $@"c:\windows\temp\{file.Name}";
                            if (File.Exists(destinationFileName))
                            {
                                File.Delete(destinationFileName);
                            }

                            if (!File.Exists(destinationFileName))
                            {
                                using (FileStream fs = new FileStream(destinationFileName, FileMode.Create, System.IO.FileAccess.Write))
                                {
                                    device.DownloadFile(file.FullName, fs);
                                }
                            }
                            else
                            {
                                proceed = false;
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
                                    }
                                    else
                                    {
                                        proceed = false;
                                        MessageBox.Show("SKÄNNERI VERSIOONI EI ÕNNESTUNUD KONTROLLIDA: " + @"c:\windows\temp\version.txt ON TÜHI!");
                                    }
                                }
                                else
                                {
                                    proceed = false;
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
                        }

                        try
                        {
                            WebClient webClient = new WebClient();
                            webClient.DownloadFile("http://www.develok.ee/BauhofWMS/Install/BauhofWMSVersion.txt", destinationLatestFileName);
                        }
                        catch (Exception ex)
                        {
                            proceed = false;
                            MessageBox.Show("VERSIOONIUUENDUST EI ÕNNESTUNUD KONTROLLIDA. FAILI EI LEITUD!" + "\r\n" + ex.Message);
                        }
                        
                        if (proceed)
                        {
                           
                            if (File.Exists(destinationLatestFileName))
                            {
                                string text = System.IO.File.ReadAllText(destinationLatestFileName);
                                if (!string.IsNullOrEmpty(text))
                                {
                                    var textSplit = text.Split(new[] { "#_#" }, StringSplitOptions.None);
                                    if (textSplit.Any())
                                    {
                                        foreach (var s in textSplit)
                                        {
                                            if (s.StartsWith("MAJOR:"))
                                            {
                                                txtBkLatestVersionValue.Text = s.Replace("MAJOR:", "");
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    proceed = false;
                                    MessageBox.Show("VERSIOONIUUENDUST EI ÕNNESTUNUD KONTROLLIDA: " + @"c:\windows\temp\BauhofWMSVersion.txt ON TÜHI!");
                                }
                            }
                            else
                            {
                                proceed = false;
                                MessageBox.Show("VERSIOONIUUENDUST EI ÕNNESTUNUD KONTROLLIDA: " + @"c:\windows\temp\BauhofWMSVersion.txt EI LEITUD");
                            }
                        }
                    }
                    if (proceed)
                    {
                       
                        if (!string.IsNullOrEmpty(txtBkLatestVersionValue.Text) && !string.IsNullOrEmpty(txtBkScannerVersionValue.Text))
                        {
                            if (Convert.ToDecimal(txtBkLatestVersionValue.Text.Remove(0, 4)) > Convert.ToDecimal(txtBkScannerVersionValue.Text.Remove(0, 4)))
                            {
                                btnUpdate.Visibility = Visibility.Visible;
                                txtBkStatus.Text = "LEITI UUS VERSIOON!";
                            }
                        }
                    }

                    if (proceed)
                    {
                        device.Connect();
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
                            }
                        }

                        Debug.WriteLine("===2");
                        var exportDir = device.GetDirectoryInfo(@"\Internal shared storage\DCIM\Export");
                        var exportFiles = exportDir.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly);
                        if (exportFiles.Count() < 3);
                        foreach (var file in exportFiles)
                        {
                            txtBkDownloadFiles.Text = txtBkDownloadFiles.Text + file.Name + "\r\n";
                        }
                    }
                    device.Disconnect();
                }
                catch (Exception ex)
                {
                    proceed = false;
                    if (ex.Message.Contains(@"\Internal shared storage\Download") && ex.Message.Contains("not found"))
                    {
                        MessageBox.Show("ÜHENDATUD SEADMELT EI LEITUD DOWNLOAD KATALOOGI!");
                    }
                    else
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
           
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
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
                    

                    var photoDir = device.GetDirectoryInfo(@"\Internal shared storage\DCIM\Export");
                    var files = photoDir.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly);
                    
                    foreach (var file in files)
                    {
                        count = count + 1;
                        string destinationFileName = $@"c:\e\{file.Name}";
                        if (!File.Exists(destinationFileName))
                        {
                            using (FileStream fs = new FileStream(destinationFileName, FileMode.Create, System.IO.FileAccess.Write))
                            {
                                device.DownloadFile(file.FullName, fs);
                            }
                        }
                    }
                    device.Disconnect();
                }
                catch(Exception ex)
                {
                    proceed = false;
                    if (ex.Message.Contains(@"\Internal shared storage\DCIM") && ex.Message.Contains("not found"))
                    {
                        MessageBox.Show("ÜHENDATUD SEADMELT EI LEITUD DCIM KATALOOGI!");
                    }
                    else
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                if (proceed)
                {
                    MessageBox.Show(count.ToString() + (count == 1 ? " FAIL SKÄNNERIST LAETUD" : " FAILI SKÄNNERIST LAETUD!"));
                }
            }

            Console.WriteLine("Done...");
            Console.ReadLine();
            

        }

        private async void btnUpload_Click(object sender, RoutedEventArgs e)
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
                    string[] dirs = Directory.GetFiles(@"c:\e\");
                    foreach (string str in dirs)
                    {
                        
                        string sourceFileName = str;
                        int index = str.LastIndexOf("\\");
                        string fileName = str.Substring(index + 1);
                        if (fileName.EndsWith(".jpeg"))
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
                    MessageBox.Show(count.ToString() + (count == 1 ? " FAIL SKÄNNERISSE LAETUD" : " FAILI SKÄNNERISSE LAETUD!"));
                }
            }

            Console.WriteLine("Done...");
            Console.ReadLine();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            bool proceed = true;
            string androidVersion = "";
            var splitlatest = txtBkLatestVersionValue.Text.Split(new[] { "." }, StringSplitOptions.None);
            if (splitlatest.Any())
            {
                foreach(var r in splitlatest)
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

            //bool proceed = true;
            //string destinationFile = @"c:\windows\temp\BauhofWMS.UWP_" + txtBkLatestVersionValue.Text + "_x64_arm.appxbundle";

            //try
            //{
            //    if (File.Exists(destinationFile))
            //    {
            //        File.Delete(destinationFile);
            //    }
            //    WebClient webClient = new WebClient();
            //    Uri source = new Uri("http://www.develok.ee/KoneskoWMS/Install/BauhofWMS.UWP_" + txtBkLatestVersionValue.Text + "_x64_arm.appxbundle", UriKind.Absolute);
            //    webClient.DownloadFile("http://www.develok.ee/BauhofWMS/Install/BauhofWMS.UWP_" + txtBkLatestVersionValue.Text + "_x64_arm.appxbundle", destinationFile);
            //}
            //catch (Exception ex)
            //{
            //    proceed = false;
            //    MessageBox.Show("UUENDUSFAILI LAADIMINE EBAÕNNESTUS" +"\r\n"+ "http://www.develok.ee/BauhofWMS/Install/BauhofWMS.UWP_" + txtBkLatestVersionValue.Text + "_x64_arm.appxbundle" + "\r\n" + ex.Message);
            //}
            //if (proceed)
            //{
            //    if (File.Exists(destinationFile))
            //    {
            //        string DeviceNameAsSeenInMyComputer = "";
            //        MediaDevice device = null;
            //        var devices = MediaDevice.GetDevices();
            //        if (devices.Any())
            //        {
            //            if (devices.Count() == 1)
            //            {
            //                device = devices.First();
            //                DeviceNameAsSeenInMyComputer = devices.First().Description;
            //            }
            //            else
            //            {
            //                MessageBox.Show("LEITI ROHKEM KUI 1 ÜHENDATUD VÄLINE SEADE!");
            //            }

            //        }
            //        else
            //        {
            //            MessageBox.Show("VÄLIST SEADET EI LEITUD!");
            //        }
            //        if (!string.IsNullOrEmpty(DeviceNameAsSeenInMyComputer))
            //        {
            //            device.Connect();
            //            var dcimDIR = device.GetDirectoryInfo(@"\Internal shared storage\DCIM");
            //            var files = dcimDIR.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly);
            //            foreach (var file in files)
            //            {
            //                if (file.Name == "BauhofWMS.UWP_" + txtBkLatestVersionValue.Text + "_x64_arm.appxbundle")
            //                {
            //                    try
            //                    {
            //                        device.DeleteFile(dcimDIR.FullName + @"\" + "BauhofWMS.UWP_" + txtBkLatestVersionValue.Text + "_x64_arm.appxbundle");
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        MessageBox.Show("SKÄNNERIL EI ÕNNESTUNUD KUSTUTADA FAILI:" + "\r\n" + dcimDIR.FullName + @"\" + "BauhofWMS.UWP_" + txtBkLatestVersionValue.Text + "_x64_arm.appxbundle" + "\r\n" + ex.Message);
            //                    }
            //                }
            //            }
            //            device.UploadFile(destinationFile, dcimDIR.FullName + @"\" + "BauhofWMS.UWP_" + txtBkLatestVersionValue.Text + "_x64_arm.appxbundle");
            //            device.Disconnect();
            //            btnUpdate.Visibility = Visibility.Collapsed;
            //            MessageBox.Show("UUENDUS ON SKÄNNERISSE LAETUD!");
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("SKÄNNERISSE SAADETAVAT FAILI EI LEITUD:" + "\r\n" + destinationFile);
            //    }
            //}
        }
    }
}
