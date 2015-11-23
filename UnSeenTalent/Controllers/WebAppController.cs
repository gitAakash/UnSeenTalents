using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Xml;
using UnseentalentsApp.Helpers;
using UnseentalentsApp.Models;
using UnseentalentsApp.Models.Repository;

namespace UnseentalentsApp.Controllers
{
    public class WebAppController : Controller
    {
        //
        // GET: /WebApp/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UploadTalent()
        {
            return View();
        }

        public ActionResult Login()
        {
            var cUser = new CurrentUser();
            if (cUser.userid != 0)
            {
                return Redirect("Index");
            }
            return View();
        }

        public ActionResult Registration()
        {
            var cUser = new CurrentUser();
            if (cUser.userid != 0)
            {
                return Redirect("Index");
            }
            return View();
        }

        public ActionResult Accountactivate(string id)
        {
            if (!string.IsNullOrEmpty(id) && id != "Update")
            {
                string encryptedString = id.Replace("_", "/").Replace("-", "+");
                string Desctr = Encryption.DecryptURL(encryptedString);

                string[] objArray = Desctr.Split(',');
                string uid = objArray[0];
                string type = objArray[1];
                var dbMethod = new UnseenTalentsMethod();
                bool rst = dbMethod.VerifyEmail(Convert.ToInt64(uid), type);
                ViewBag.done = rst.ToString().ToLower();
                ViewBag.email = id;
            }
            if (id == "Update")
            {
                ViewBag.expire = "Update";
            }

            return View();
        }

        public ActionResult Welcome()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult HowToApply()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "webapp");
        }

        public ActionResult UpcomingEvents()
        {
            return View();
        }

        public ActionResult EventDetail()
        {
            return View();
        }

        public ActionResult Payment()
        {
            return View();
        }

        [HttpPost]
        public string UploadFile(UploadedFile model)
        {
            string jsonResult = null; 
            var statuses = new Files();
            UploadedFile response = AmazonS3Helper.UploadToS3Images(model.File, model.SelectedCategory);
            if (Request.Url != null)
            {
                string requiredPath = Request.Url.GetLeftPart(UriPartial.Authority) +
                                      Url.Content("~/Assets/Images/videoDoc.png");
                if (response.UploadStatus)
                {
                    statuses.files.Add( new ViewDataUploadFilesResult
                    {
                        name = response.FileName,
                        size = model.File.ContentLength,
                        type = model.File.ContentType,
                        url = response.FileUrl,
                        deleteUrl = response.FileUrl,
                        thumbnailUrl = requiredPath,
                        deleteType = "GET",
                    });
                }
            }
            var serializer = new JavaScriptSerializer
            {
                MaxJsonLength = Int32.MaxValue,
                RecursionLimit = 100
            };
            jsonResult = serializer.Serialize(statuses);

            return jsonResult;
        }


        public string CreateXMLForGoogleIndexing()
        {
            const string Headers = "<?xml version='1.0' encoding='UTF-8' ?>";
            //Create XmlDocument
            var xmlDoc = new XmlDocument();

            //Create the root element
            XmlNode outputsElement = xmlDoc.CreateElement("OnDemandIndex");

            //Create the child element
            XmlNode Element = xmlDoc.CreateElement("Pages");

            XmlElement ChildElement = xmlDoc.CreateElement("Page");

            //Create "url" Attribute
            XmlAttribute urlAtt = xmlDoc.CreateAttribute("url");
            urlAtt.Value = "http://www.8-12-14-auburn-road-hawthorn.com/";
            ChildElement.Attributes.Append(urlAtt);

            Element.AppendChild(ChildElement);

            //Append child element into root element
            outputsElement.AppendChild(Element);
            xmlDoc.AppendChild(outputsElement);
            return Headers + xmlDoc.OuterXml;
        }

        [HttpGet]
        public string CreateGoogleIndex()
        {
            string id = "91r1zqwghyw";
            string creator = "002536573461655477099";

            string requestXml = CreateXMLForGoogleIndexing();
            string destinationUrl = "http://cse.google.com/api/002536573461655477099/cse/default";
            // Uri uri = new Uri("http://8-12-14-auburn-road-hawthorn.com/");


            //string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
            //string _auth = string.Format("{0}:{1}", username, password);
            //string _enc = Convert.ToBase64String(Encoding.ASCII.GetBytes(_auth));
            //string cred = string.Format("{0} {1}", "Basic", enc);

            string UserName = "aakash.pawar.cs@wwindia.com";
            string Password = "TMP!3m06";
            // var AuthToken= "https://www.google.com/accounts/ClientLogin"
            //string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
            // String username = "abc";
            // String password = "123";
            // var encoded = "https://www.google.com/accounts/ClientLogin -d Email=" + UserName + "&&Passwd=" + Password + "&&accountType=GOOGLE&&service=cprose"; //System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(UserName + ":" + Password));


            var request = (HttpWebRequest) WebRequest.Create(destinationUrl);
            //   request.Headers.Add("Authorization", "GoogleLogin auth=sfBcFYnmfLWX5dUaNzJPjDFp");
            //request.Credentials = new NetworkCredential(UserName, Password);
            request.Headers.Add("Authorization: GoogleLogin auth=AIzaSyAighYPCZPRaYHd-TWcrRFSfdrEc2ZF67w");
            //request.get();
            // request.Headers.Add(HttpRequestHeader.Authorization, "GoogleLogin auth=" + encoded);
            // request.Headers.Add("Authorization", "Basic " + encoded);
            //  request.Credentials = new CredentialCache { { uri, "Basic", new NetworkCredential(UserName, Password) } };
            // request.Headers.Add("Authorization", "Basic " + svcCredentials);
            //request.Headers[HttpRequestHeader.Authorization] = _cred;
            //  request.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;//= new NetworkCredential(username, password);
            // request.Headers.Add("GoogleLogin", "auth=" + svcCredentials);
            // 
            // request.Credentials.GetCredential(uri, "Basic"); //= "IM6F7Cx2fo0TAiwlhNVdSE8Ov8hw6aHV";
            byte[] bytes;
            bytes = Encoding.ASCII.GetBytes(requestXml);
            request.ContentType = "text/xml; encoding='utf-8'";
            request.ContentLength = bytes.Length;

            request.Method = "POST";
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse) request.GetResponse();
            }
            catch (Exception ex)
            {
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                string responseStr = new StreamReader(responseStream).ReadToEnd();
                return responseStr;
            }
            return null;
        }
    }
}