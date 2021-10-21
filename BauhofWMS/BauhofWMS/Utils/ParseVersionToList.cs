using System;
using System.Collections.Generic;
using System.Text;
using BauhofWMSDLL.ListDefinitions;
using System.Diagnostics;

namespace BauhofWMS.Utils
{
    public class ParseVersionToList
    {
        public List<ListOfVersion> Get(string version)
        {
            try
            {
                Debug.WriteLine("ParseVersionToList " + version);
                if (!string.IsNullOrEmpty(version))
                {
                    var lstResult = new List<ListOfVersion>();
                    var resultSplitted = version.Split(new string[] { "." }, StringSplitOptions.None);
                    var Build = Convert.ToInt32(resultSplitted[0].ToString());
                    var Major = Convert.ToInt32(resultSplitted[1].ToString());
                    var Minor = Convert.ToInt32(resultSplitted[2].ToString());
                    var Revision = Convert.ToInt32(resultSplitted[3].ToString());
                    var row = new ListOfVersion
                    {
                        Build = Build,
                        Major = Major,
                        Minor = Minor,
                        Revision = Revision

                    };
                    lstResult.Add(row);
                    Debug.WriteLine("ParseVersionToList.Count =  " + lstResult.Count);
                    return lstResult;
                }
                else
                {
                    return new List<ListOfVersion>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ParseVersionToList " + ex.Message);
                return new List<ListOfVersion>();
            }
        }
    }
}