using System.Configuration;

namespace UnseentalentsApp.Helpers
{
    public static class AppSettingHelper
    {
        public static string GetAmazonAccessKey()
        {
            return ConfigurationManager.AppSettings["AccessKeyId"];
        }

        public static string GetAmazonSecretKey()
        {
            return ConfigurationManager.AppSettings["SecretAccessKey"];
        }
        public static string GetAmazonUserName()
        {
            return ConfigurationManager.AppSettings["UserName"];
        }

        public static string GetAmazonBucketName()
        {
            return ConfigurationManager.AppSettings["AmazonBucketName"];
        }

        public static string GetSiteUrl()
        {
            return ConfigurationManager.AppSettings["SiteUrl"];
        }

        public static string GetImagePath()
        {
            return ConfigurationManager.AppSettings["ImagePath"];
        }

        public static string GetProfileImagesPath()
        {
            return ConfigurationManager.AppSettings["ImagePath"];
        }

        public static string GetAdminEmail()
        {
            return ConfigurationManager.AppSettings["AdminEmail"];
        }

        public static string GetAmazonFileUrl()
        {
            return ConfigurationManager.AppSettings["AmazonFileUrl"];
        }

    }

}