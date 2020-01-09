using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Text;
using System.Windows;
using System.Windows.Resources;

namespace SimpleTimer
{
    public static class Utils
    {
        public static Stream GetResourceStream(string resourceName)
        {
            string resourcePath = "resources/" + resourceName;
            string url = string.Format(
                null,
                "pack://application:,,,/{0};component//{1}",
                "SimpleTimer",
                resourcePath);

            try
            {
                string s = PackUriHelper.UriSchemePack;
                var uri = new Uri(url);
                StreamResourceInfo sri = Application.GetResourceStream(uri);
                return sri.Stream;
            }
            catch (Exception)
            {
                //TODO log
                return null;
            }
        }
    }
}
