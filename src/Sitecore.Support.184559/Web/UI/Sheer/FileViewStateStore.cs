using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.IO;
using Sitecore.Web;
using Sitecore.Web.UI.Sheer;
using System;
using System.Text;

namespace Sitecore.Support.Web.UI.Sheer
{
    public class FileViewStateStore : IViewStateStore
    {
        private string GetFilename(string key)
        {
            StringBuilder builder = new StringBuilder();
            char[] chArray = key.ToCharArray();
            builder.Append(FileUtil.MapPath(Settings.DataFolder));
            builder.Append("/viewstate");
            for (int i = 0; i < 4; i++)
            {
                if (i < key.Length)
                {
                    builder.Append('/');
                    builder.Append(chArray[i]);
                }
            }
            builder.Append('/');
            builder.Append(key);
            builder.Append(".txt");
            return builder.ToString();
        }

        public string Load(string key)
        {
            Error.AssertString(key, "key", false);
            string filename = this.GetFilename(key);
            if (FileUtil.Exists(filename))
            {
                return FileUtil.ReadFromFile(filename);
            }
            return null;
        }

        public void Remove(string key)
        {
            Error.AssertString(key, "key", false);
            FileUtil.Delete(this.GetFilename(key));
        }

        public void Save(string key, string viewstate)
        {
            Error.AssertString(key, "key", false);
            Error.AssertString(viewstate, "viewstate", false);
            string filename = this.GetFilename(key);

            //start of modified part
            string ribbonId = WebUtil.GetQueryString("ribbonId");

            if (!string.IsNullOrEmpty(ribbonId) && ribbonId == "{E557DCD5-7EF6-4104-9C5C-E005BAFF55FF}")
            {
                try
                {
                    FileUtil.EnsureFolder(filename);
                    FileUtil.WriteToFile(filename, viewstate);
                }
                catch (Exception ex)
                {
                    Log.Info("Exception in the Sitecore.Support.184559: " + ex.Message, this);
                }
            }
            else
            {
                FileUtil.EnsureFolder(filename);
                FileUtil.WriteToFile(filename, viewstate);
            }

            //end of the modified part

        }
    }
}
