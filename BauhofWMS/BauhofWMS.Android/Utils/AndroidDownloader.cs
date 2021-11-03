using Android.Content;
using Android.OS;
using Android.Webkit;
using BauhofWMS.Droid.Utils;
using Java.IO;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidDownloader))]

namespace BauhofWMS.Droid.Utils
{
    public class AndroidDownloader : IDownloader
    {
        public event EventHandler<DownloadEventArgs> OnFileDownloaded;
        string pathToNewFolder = "";
        string fileName = "";
        string urlGet = "";
        const string PACKAGE_INSTALLED_ACTION = "com.example.android.apis.content.SESSION_API_PACKAGE_INSTALLED";

        public void DownloadFile(string url)
        {
            pathToNewFolder = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "Pictures", "Screenshots");
            try
            {
                urlGet = url;
                using (WebClient webClient = new WebClient())
                {
                    webClient.OpenRead(url);
                    double totalBytes = Convert.ToDouble(webClient.ResponseHeaders["Content-Length"]);

                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                    webClient.DownloadProgressChanged += (o, e) =>
                    {
                        double bytesIn = e.BytesReceived;
                        double percentage = ((bytesIn / totalBytes) * 100);
                        MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "downloadUpdateProgress", Math.Truncate(percentage).ToString());
                    };

                    fileName = Path.GetFileName(url);
                    string pathToNewFile = Path.Combine(pathToNewFolder, Path.GetFileName(url));
                    webClient.DownloadFileAsync(new Uri(url), pathToNewFile);
                }
            }
            catch (Exception ex)
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
            }
        }


        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    if (OnFileDownloaded != null)
                        OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
                }
                else
                {
                    MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "downloadComplete", "true");
                    StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
                    StrictMode.SetVmPolicy(builder.Build());

                    Java.IO.File file = new Java.IO.File(pathToNewFolder, Path.GetFileName(urlGet));
                    string extension = MimeTypeMap.GetFileExtensionFromUrl(Android.Net.Uri.FromFile(file).ToString());
                    string mimeType = MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension);
                    Intent intent = new Intent(Intent.ActionView);
                    intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                    intent.SetDataAndType(Android.Net.Uri.FromFile(file), "application/vnd.android.package-archive");

                    intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    Forms.Context.StartActivity(Intent.CreateChooser(intent, "Your title"));

                    if (OnFileDownloaded != null)
                        OnFileDownloaded.Invoke(this, new DownloadEventArgs(true));
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        public void LaunchLocalFile(string fileName)
        {
            try
            {
                pathToNewFolder = Android.OS.Environment.DirectoryDcim;
                String sdcardRoot = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "Download"); //Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                String apkSavePath = sdcardRoot + "/" + fileName;

                Java.IO.File file = new Java.IO.File(apkSavePath);
                Intent install = new Intent(Intent.ActionView);

                // Old Approach
                if (Android.OS.Build.VERSION.SdkInt < BuildVersionCodes.N)
                {
                    install.SetFlags(ActivityFlags.NewTask | ActivityFlags.GrantReadUriPermission);
                    install.SetDataAndType(Android.Net.Uri.FromFile(file), "application/vnd.android.package-archive"); //mimeType
                }
                else
                {
                    Android.Net.Uri apkURI = Android.Support.V4.Content.FileProvider.GetUriForFile(Forms.Context, Forms.Context.ApplicationContext.PackageName + ".fileprovider", file);
                    install.SetDataAndType(apkURI, "application/vnd.android.package-archive");
                    install.AddFlags(ActivityFlags.NewTask);
                    install.AddFlags(ActivityFlags.GrantReadUriPermission);
                }

                Forms.Context.StartActivity(Intent.CreateChooser(install, "Your title"));

            }
            catch (ActivityNotFoundException ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
    }
}