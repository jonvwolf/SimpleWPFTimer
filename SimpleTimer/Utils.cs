using System;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Resources;

namespace SimpleTimer
{
    public static class Utils
    {
        public static Stream GetResourceStream(string resourceName)
        {
            //ex. SimpleTimer
            string assembly = typeof(Utils).Assembly.FullName;

            string resourcePath = "resources/" + resourceName;
            string url = string.Format(
                null,
                "pack://application:,,,/{0};component//{1}",
                assembly,
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
                throw;
            }
        }
    }
}
