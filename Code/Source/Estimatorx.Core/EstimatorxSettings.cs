using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KickStart;

namespace Estimatorx.Core
{
    public static class EstimatorxSettings
    {
        public static string GoogleClientId
        {
            get { return GetValue("Google:ClientId"); }
        }
        public static string GoogleClientSecret
        {
            get { return GetValue("Google:ClientSecret"); }
        }

        public static string MicrosoftClientId
        {
            get { return GetValue("Microsoft:ClientId"); }
        }
        public static string MicrosoftClientSecret
        {
            get { return GetValue("Microsoft:ClientSecret"); }
        }

        public static string TwitterConsumerKey
        {
            get { return GetValue("Twitter:ConsumerKey"); }
        }
        public static string TwitterConsumerSecret
        {
            get { return GetValue("Twitter:ConsumerSecret"); }
        }

        public static string FacebookApplicationId
        {
            get { return GetValue("Facebook:ApplicationId"); }
        }
        public static string FacebookApplicationSecret
        {
            get { return GetValue("Facebook:ApplicationSecret"); }
        }

        public static string GitHubClientId
        {
            get { return GetValue("GitHub:ClientId"); }
        }
        public static string GitHubClientSecret
        {
            get { return GetValue("GitHub:ClientSecret"); }
        }


        public static string GetValue(string name, string defaultValue = null)
        {
            string value = ConfigurationManager.AppSettings[name];
            if (string.IsNullOrEmpty(value))
                return defaultValue;

            return value;
        }

        public static string GetConnectionString(string name, string defaultValue = null)
        {
            var value = ConfigurationManager.ConnectionStrings[name];
            if (value == null || string.IsNullOrEmpty(value.ConnectionString))
                return defaultValue;

            return value.ConnectionString;
        }

    }
}
