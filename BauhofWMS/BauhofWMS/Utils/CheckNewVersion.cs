using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using System.Diagnostics;
using BauhofWMSDLL.ListDefinitions;

namespace BauhofWMS.Utils
{
    public class CheckNewVersion
    {
        ParseVersionToList ParseVersionToList = new ParseVersionToList();
        public GetPublishedVersion GetPublishedVersion = new GetPublishedVersion();

        public async Task<Tuple<bool, string, string, string, string>> Check(string company, string pin)
        {
            try
            {
                string currentVersion = null;
                string publishedVersion = null;
                var p = 0;

                Debug.WriteLine("GetPublishedVersion");
                var result = await GetPublishedVersion.Get();
                if (result.Item1)
                {
                    string majorPublished = "";
                    string minorPublished = "";
                    bool userAllowedMinor = false;

                    var getCompany = result.Item3.Split(new[] { ("%_%") }, StringSplitOptions.None);
                    foreach (var c in getCompany)
                    {
                        if (!string.IsNullOrEmpty(c))
                        {
                            if (c.TrimStart().TrimEnd().ToUpper().Replace("\r\n", "").StartsWith(company.ToUpper()))
                            {
                                var getCompanySettings = c.TrimStart().TrimEnd().Replace("\r\n", "").Split(new[] { ("#_#") }, StringSplitOptions.None);
                                majorPublished = getCompanySettings[1].Replace("MAJOR:", "");
                                minorPublished = getCompanySettings[2].Replace("MINOR:", "");
                                var users = getCompanySettings[3].Split(new[] { (",") }, StringSplitOptions.None);
                                foreach (var u in users)
                                {
                                    if (u == pin)
                                    {
                                        userAllowedMinor = true;
                                    }
                                }
                            }
                        }
                    }


                    publishedVersion = majorPublished;
                    if (Device.RuntimePlatform == Device.UWP)
                    {
                        currentVersion = DependencyService.Get<IPlatformDetailsUWP>().GetPlatformName();
                    }
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        var build = DependencyService.Get<IAppVersion>().GetBuild();
                        var version = DependencyService.Get<IAppVersion>().GetVersion();
                        currentVersion = build + ".0." + version;
                        p = 3;
                    }

                    if (!string.IsNullOrEmpty(currentVersion))
                    {
                        Debug.WriteLine("currentVersion: " + currentVersion + " publishedVersion: " + publishedVersion + " minorPublished: " + minorPublished + " userAllowedMinor: " + userAllowedMinor);
                        if (userAllowedMinor)
                        {
                            var lstPublishedVersion = ParseVersionToList.Get(minorPublished);
                            var lstCurrentVersion = ParseVersionToList.Get(currentVersion);
                            if (currentVersion == minorPublished || lstCurrentVersion.First().Minor > lstPublishedVersion.First().Minor)
                            {
                                return new Tuple<bool, string, string, string, string>(true, null, publishedVersion, currentVersion, null);
                            }
                            else
                            {
                                if (lstCurrentVersion.First().Minor < lstPublishedVersion.First().Minor)
                                {
                                    return new Tuple<bool, string, string, string, string>(false, "LEITUD UUS TESTVERSIOON " + publishedVersion + "\r\n" + "KAS UUENDADA TARKVARA?", minorPublished, currentVersion, lstPublishedVersion.First().Minor.ToString());
                                }
                            }
                        }
                        else
                        {
                            var lstPublishedVersion = ParseVersionToList.Get(publishedVersion);
                            var lstCurrentVersion = ParseVersionToList.Get(currentVersion);
                            if (currentVersion == publishedVersion || lstCurrentVersion.First().Minor > lstPublishedVersion.First().Minor)
                            {
                                return new Tuple<bool, string, string, string, string>(true, null, publishedVersion, currentVersion, null);
                            }
                            else
                            {

                                if ((lstCurrentVersion.First().Minor < lstPublishedVersion.First().Minor))
                                {
                                    return new Tuple<bool, string, string, string, string>(false, "LEITUD UUS VERSIOON " + publishedVersion + "\r\n" + "KAS UUENDADA TARKVARA?", publishedVersion, currentVersion, lstPublishedVersion.First().Minor.ToString());
                                }
                            }
                        }
                    }
                    else
                    {
                        return new Tuple<bool, string, string, string, string>(false, "TARKVARA VERSIOONI EI SUUDETUD LEIDA!", null, null, null);
                    }

                    return new Tuple<bool, string, string, string, string>(true, null, publishedVersion, currentVersion, null);
                }
                else
                {
                    return new Tuple<bool, string, string, string, string>(false, "TARKVARA VERSIOONI KONTROLL EBAÕNNESTUS!", null, null, null);
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Tuple<bool, string, string, string, string>(false, "AAAAA " + ex.Message, null, null, null);
            }
        }

    }

}