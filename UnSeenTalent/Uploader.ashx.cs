using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace JustDeliver
{
    /// <summary>
    /// Summary description for Uploader
    /// </summary>
    public class Uploader : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string filePath = context.Server.MapPath("~/WebImages/");

            //write your handler implementation here.
            if (context.Request.Files.Count > 0)
            {
                for (int i = 0; i < context.Request.Files.Count; ++i)
                {
                    HttpPostedFile file = context.Request.Files[i];
                    var ext = Path.GetExtension(file.FileName);
                    string UniqueFileName = Guid.NewGuid().ToString("n") + ext;     
                    file.SaveAs(filePath + UniqueFileName);
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(UniqueFileName);
                }
            }
            else
            {

               // throw new Exception("No file uploaded");
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}