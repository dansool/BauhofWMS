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
                    device.Connect();
                    var photoDir = device.GetDirectoryInfo(@"\Internal shared storage\DCIM");
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

                    var photoDir = device.GetDirectoryInfo(@"\Internal shared storage\DCIM");
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
    }
}
