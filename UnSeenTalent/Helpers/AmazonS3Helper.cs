using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using Amazon;
using Amazon.S3;
using Amazon.S3.IO;
using Amazon.S3.Model;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using UnseentalentsApp.Models;
using UnseentalentsApp.Models.Repository;

namespace UnseentalentsApp.Helpers
{
    public class AmazonS3Helper
    {
        public static IAmazonS3 GetAmazonClient()
        {
            return AWSClientFactory.CreateAmazonS3Client(AppSettingHelper.GetAmazonAccessKey(),
                AppSettingHelper.GetAmazonSecretKey(), RegionEndpoint.USEast1);
        }

        public static PutObjectRequest GetAmazonObjectRequest(string key)
        {
            return new PutObjectRequest
            {
                BucketName = AppSettingHelper.GetAmazonBucketName(),
                CannedACL = S3CannedACL.PublicRead, //PERMISSION TO FILE PUBLIC ACCESIBLE
                Key = key,
            };
        }

        public static UploadedFile UploadToS3Images(HttpPostedFileBase file, int eventId)
        {
            var uploadedFile = new UploadedFile();
           // string fileName = file.FileName.Replace(" ", "+");
            string fileName = Regex.Replace(file.FileName, @"\s+", "");
            try
            {
                IAmazonS3 client;
                using (client = GetAmazonClient())
                {
                    string fileUploadKey = Convert.ToString(eventId) + "/";
                    var s3FileInfo = new S3FileInfo(client, AppSettingHelper.GetAmazonBucketName(),
                        fileUploadKey + fileName);
                    if (s3FileInfo.Exists)
                    {
                        // If file exists
                        string strRandomFileName = Path.GetRandomFileName();

                        //This method returns a random file name of 11 characters
                        string fileExtension = Path.GetExtension(fileName);
                        strRandomFileName = strRandomFileName.Replace(".", "") + fileExtension;
                        
                        #region Code For if file exist on amazon for a particular EventId.

                        fileUploadKey = Convert.ToString(eventId) + "/" + strRandomFileName;
                        fileName = strRandomFileName;

                        #endregion
                    }
                    else
                    {
                        #region Code For if File does not exist in event bucket on Amazon

                        fileUploadKey = Convert.ToString(eventId) + "/" + fileName;
                     
                        #endregion
                    }

                    PutObjectRequest request = GetAmazonObjectRequest(fileUploadKey);
                    request.InputStream = file.InputStream;
                    PutObjectResponse response = client.PutObject(request);

                    if (response.HttpStatusCode.ToString() == "OK")
                    {
                       #region Construct model for Uploaded video file

                        uploadedFile.FileName = fileName;
                        uploadedFile.FileUrl = AppSettingHelper.GetAmazonFileUrl() +
                                               AppSettingHelper.GetAmazonBucketName() + "/" + fileUploadKey;
                        uploadedFile.UploadStatus = true;

                        #endregion
                    }
                }
                return uploadedFile;
            }
            catch (Exception ex)
            {
                return uploadedFile;
            }
        }

      /*  public bool SendEmail()
        {
            //INITIALIZE AWS CLIENT//
            AmazonSimpleEmailServiceConfig amConfig = new AmazonSimpleEmailServiceConfig();
           // amConfig.UseSecureStringForAwsSecretKey = false;
            AmazonSimpleEmailServiceClient amzClient = new AmazonSimpleEmailServiceClient(
              ConfigurationManager.AppSettings["AWSAccessKey"].ToString(),
              ConfigurationManager.AppSettings["AWSSecretKey"].ToString(), amConfig);

            //ArrayList that holds To Emails. It can hold 1 Email to any
            //number of emails in case what to send same message to many users.
            ArrayList to = new ArrayList();
            to.Add(EMAIL_HERE);

            //Create Your Bcc Addresses as well as Message Body and Subject
            Destination dest = new Destination(new List<string>());
            //dest.WithBccAddresses((string[])to.ToArray(typeof(string)));
            string body = Body;
            string subject = "Subject : " + txtSubject.Text;
            Body bdy = new Body();
            bdy.Html = new Amazon.SimpleEmail.Model.Content(body);
            Amazon.SimpleEmail.Model.Content title = new Amazon.SimpleEmail.Model.Content(subject);
            Message message = new Message(title, bdy);

            //Create A Request to send Email to this ArrayList with this body and subject
            try
            {
                SendEmailRequest ser = new SendEmailRequest(Send_From_Email_MUST_BE_VERIFIED_BY_AMAZON, dest, message);
                SendEmailResponse seResponse = amzClient.SendEmail(ser);
                SendEmailResult seResult = seResponse.SendEmailResult;
            }
            catch (Exception ex)
            {

            }
            return true;
        }

        public void ListVerifiedEmail()
        {
            //INITIALIZE AWS CLIENT//
            AmazonSimpleEmailServiceConfig amConfig = new AmazonSimpleEmailServiceConfig();
           // amConfig.UseSecureStringForAwsSecretKey = false;
            amConfig.AuthenticationRegion = Convert.ToString(Amazon.RegionEndpoint.USEast1);
            AmazonSimpleEmailServiceClient amzClient =
              new AmazonSimpleEmailServiceClient(ConfigurationManager.AppSettings["AWSAccessKey"].ToString(),
              ConfigurationManager.AppSettings["AWSSecretKey"].ToString(), amConfig);

            //LIST VERIFIED EMAILS//
            ListVerifiedEmailAddressesRequest lveReq = new ListVerifiedEmailAddressesRequest();
            ListVerifiedEmailAddressesResponse lveResp = amzClient.ListVerifiedEmailAddresses(lveReq);
            ListVerifiedEmailAddressesResult lveResult = lveResp.ListVerifiedEmailAddressesResult;

            foreach (Object email in lveResult.VerifiedEmailAddresses)
            {
               // Response.Write(email.ToString() + ",");
            }
        }*/
    }
}