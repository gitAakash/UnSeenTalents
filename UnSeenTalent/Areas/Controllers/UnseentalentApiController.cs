using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using UnseentalentsApp;
using UnseentalentsApp.Models.Db;
using UnseentalentsApp.Models.Repository;
using UnseentalentsApp.Models;
using System.Web;
using System.Drawing;
using System.IO;

namespace UnseentalentsApp.Controllers
{
    public class UnseentalentApiController : ApiController
    {

        #region For Super Admin

        #region AdminLogin
        [HttpPost]
        public Response<byte> AdminLogin(LoginModel Data)
        {
            Response<byte> objresponse = new Response<byte>();
            byte result = 0;
            if (Data != null)
            {

                // string pwd = Encryption.Encrypt(Data.Password, ConstantValues.EncryptionKey);
                using (var db = new UnseentalentdbDataContext())
                {
                    var user = db.tbl_users.Where(t => t.email == Data.Email).FirstOrDefault();
                    if (user != null)
                    {

                        if (user.password == Data.Password)
                        {
                            CurrentUser currentUser = new CurrentUser()
                            {
                                Email = user.email,
                                FirstName = user.firstname,
                                isEmailVerify = true,
                                LastName = user.lastname,
                                ProfilePic = user.profilepic,
                                userid = user.id,
                                userrole = GetUserRole(user.id),


                            };
                            string json = JsonConvert.SerializeObject(currentUser);
                            FormsAuthentication.SetAuthCookie(json, Data.RememberMe);
                            result = 2;//sucess login

                            objresponse.Create(true, 0, "Admin login successfully", result);
                        }
                        else
                        {
                            result = 0; //Username password not valid
                            objresponse.Create(false, 1, "Admin login successfully", result);
                        }
                    }
                    else
                    {
                        result = 0;
                        objresponse.Create(false, 1, "Admin login successfully", result);
                    }


                }
            }
            return objresponse;
        }


        public List<userrole> GetUserRole(Int64 userid)
        {
            List<userrole> objuserrole = new List<userrole>();
            using (var db = new UnseentalentdbDataContext())
            {
                foreach (var item in db.tbl_userroles.Where(u => u.userid == userid).ToList())
                {
                    userrole objrole = new userrole();
                    objrole.roleid = Convert.ToInt32(item.userrole);
                    objuserrole.Add(objrole);
                }

            }
            return objuserrole;
        }

        #endregion

        #region Getevents
        [HttpPost]
        public Response<GetEventResponse> GetAllevntsforadmin(object objreq)
        {
            Response<GetEventResponse> objresponse = new Response<GetEventResponse>();
            List<GetAllevents> objlist = new List<GetAllevents>();
            UnseenTalentsMethod objmethod = new UnseenTalentsMethod();
            GetEventResponse objeventresp = new GetEventResponse();
            try
            {
                objeventresp.EventList = objmethod.GetalleventsforAdmin();
                if (objeventresp.EventList.Count() > 0)
                {
                    objeventresp.total = objeventresp.EventList.Count();
                    objresponse.Create(true, 0, "Events for admin", objeventresp);
                }
                else
                {
                    objeventresp.total = 0;
                    objresponse.Create(false, 1, "Events for admin", objeventresp);
                }

            }
            catch (Exception ex)
            {
                objeventresp.total = 0;
                objresponse.Create(false, -1, "Events for admin", objeventresp);
            }
            return objresponse;


        }

        #endregion

        #region SaveEditEvents
        [HttpPost]
        public Response<bool> SaveEditEvents(GetAllevents objreq)
        {
            bool rst = false;
            Response<bool> objresponse = new Response<bool>();
            UnseenTalentsMethod objmethod = new UnseenTalentsMethod();

            try
            {
                rst = objmethod.SaveEditEvents(objreq);
                objresponse.Create(true, 0, "Events for admin", rst);


            }
            catch (Exception ex)
            {
                objresponse.Create(false, -1, "", rst);
            }
            return objresponse;


        }

        #endregion

        #region EventAction()
        [HttpPost]
        public Response<string> EventAction(ActionModel model)
        {
            Response<string> response = new Response<string>();
            string value = "";


            UnseenTalentsMethod obj = new UnseenTalentsMethod();
            value = obj.EventAction(model);
            if (value == "1")
            {
                response.Create(true, 0, Messages.delete, value);
            }
            else if (value == "2")
            {
                response.Create(true, 0, Messages.block, value);
            }
            else if (value == "3")
            {
                response.Create(true, 0, Messages.unBlock, value);
            }
            else
            {
                response.Create(false, 0, Messages.INVALID_REQ, value);
            }
            return response;


        }
        #endregion

        #endregion


        #region For Web Panel

        #region UserRegistration
        [HttpPost]
        public Response<int> UserRegistration(ReqRegistration objreq)
        {
            int rst = 0;
            Response<int> objresponse = new Response<int>();
            UnseenTalentsMethod objmethod = new UnseenTalentsMethod();

            try
            {
                rst = objmethod.UserRegistration(objreq);
                objresponse.Create(true, 0, "Events for admin", rst);


            }
            catch (Exception ex)
            {
                objresponse.Create(false, -1, "", rst);
            }
            return objresponse;


        }

        #endregion
        #region UserLogin
        [HttpPost]
        public Response<byte> UserLogin(LoginModel Data)
        {
            Response<byte> objresponse = new Response<byte>();
            byte result = 0;
            if (Data != null)
            {

                // string pwd = Encryption.Encrypt(Data.Password, ConstantValues.EncryptionKey);
                using (var db = new UnseentalentdbDataContext())
                {
                    var user = db.tbl_users.Where(t => t.email == Data.Email && t.isasctive == true && t.isdeleted == false).FirstOrDefault();
                    if (user != null)
                    {

                        if (user.password == Data.Password)
                        {
                            UnseenTalentsMethod objmethod = new UnseenTalentsMethod();
                            CurrentUser currentUser = new CurrentUser()
                            {
                                Email = user.email,
                                FirstName = user.username,
                                isEmailVerify = true,
                                LastName = user.lastname,
                                ProfilePic = objmethod.ReturnProfilePicture(user.profilepic),
                                userid = user.id,
                                isToken = user.isToken == null ? false : Convert.ToBoolean(user.isToken)


                            };
                            string json = JsonConvert.SerializeObject(currentUser);
                            FormsAuthentication.SetAuthCookie(json, Data.RememberMe);
                            result = 2;//sucess login

                            objresponse.Create(true, 0, "User login successfully", result);
                        }
                        else
                        {
                            result = 0; //Username password not valid
                            objresponse.Create(false, 1, "User login successfully", result);
                        }
                    }
                    else
                    {
                        result = 0;
                        objresponse.Create(false, 1, "User login successfully", result);
                    }


                }
            }
            return objresponse;
        }

        #endregion

        #region GetHttpContext()
        private HttpContextWrapper GetHttpContext(HttpRequestMessage request = null)
        {
            request = request ?? Request;

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]);
            }
            else if (HttpContext.Current != null)
            {
                return new HttpContextWrapper(HttpContext.Current);
            }
            else
            {
                return null;
            }
        }

        #endregion

        [HttpPost]
        public Response<List<getEventModel>> GetAllevntsforWeb()
        {
            Response<List<getEventModel>> objresponse = new Response<List<getEventModel>>();
            List<getEventModel> objlist = new List<getEventModel>();
            UnseenTalentsMethod objmethod = new UnseenTalentsMethod();

            try
            {
                objlist = objmethod.GetAllevntsforWeb();

                objresponse.Create(true, 0, "Events for admin", objlist);


            }
            catch (Exception ex)
            {
                objresponse.Create(false, -1, "Events for admin", objlist);
            }
            return objresponse;


        }

        #region SavePost
        [HttpPost]
        public Response<string> SavePost()
        {

            Response<string> response = new Response<string>();
            string rst = "";
            try
            {

                HttpContextWrapper objwrapper = GetHttpContext(this.Request);
                HttpFileCollectionBase collection = objwrapper.Request.Files;

                string jsonvalue = objwrapper.Request.Form["json"];

                if (!string.IsNullOrEmpty(jsonvalue))
                {
                    ReqSaveEventPost objitem = JsonConvert.DeserializeObject<ReqSaveEventPost>(jsonvalue);


                    objitem.file = "";
                    //int count = 0;
                    foreach (string file in collection)
                    {
                        HttpPostedFileBase file1 = collection.Get(file);
                        //  HttpPostedFileBase file1 = collection.Get(file);
                        if (file1.ContentType == "video/quicktime" || file1.ContentType == "video/mp4")
                        {

                            byte[] videofile = new byte[file1.ContentLength];
                            file1.InputStream.Read(videofile, 0, file1.ContentLength);
                            BinaryWriter Writer = null;
                            if (videofile.Length > 0)
                            {
                                string videoname = Guid.NewGuid().ToString().Substring(0, 7) + "_.MOV";
                                if (file1.ContentType == "video/mp4")
                                {
                                    videoname = Guid.NewGuid().ToString().Substring(0, 7) + "_.mp4";
                                }
                                Writer = new BinaryWriter(File.OpenWrite(HttpContext.Current.Request.PhysicalApplicationPath + "WebImages\\Video\\" + videoname));
                                Writer.Write(videofile);
                                Writer.Flush();
                                Writer.Close();
                                objitem.file = videoname;
                                // count = 1;
                            }
                        }


                    }
                    UnseenTalentsMethod objmethod = new UnseenTalentsMethod();

                    rst = objmethod.SaveEventPost(objitem);
                    response.Create(true, 1, "User login successfully", rst);

                }
                else
                {
                    response.Create(false, 1, "Data not found.", "0");
                }


            }
            catch (Exception ex)
            {

                response.Create(false, -1, Messages.FormatMessage(ex.Message), "0");
            }
            finally
            {
                // return response;
            }
            return response;
        }
        #endregion



        #region GetEventPosts
        [HttpPost]
        public Response<List<RespGetEventPosts>> GetEventPosts(ReqGetEventPosts objReq)
        {
            Response<List<RespGetEventPosts>> objresponse = new Response<List<RespGetEventPosts>>();
            List<RespGetEventPosts> objlist = new List<RespGetEventPosts>();
            UnseenTalentsMethod objmethod = new UnseenTalentsMethod();

            try
            {
                objlist = objmethod.GetEventPosts(objReq);

                objresponse.Create(true, 0, "Events for admin", objlist);


            }
            catch (Exception ex)
            {
                objresponse.Create(false, -1, "Events for admin", objlist);
            }
            return objresponse;


        }
        #endregion


        #region AddComment
        [HttpPost]
        public Response<bool> AddComment(ReqAddComment objreq)
        {
            bool rst = false;
            Response<bool> objresponse = new Response<bool>();
            UnseenTalentsMethod objmethod = new UnseenTalentsMethod();

            try
            {
                rst = objmethod.SaveVideoComments(objreq);
                objresponse.Create(true, 0, "Events for admin", rst);


            }
            catch (Exception ex)
            {
                objresponse.Create(false, -1, "", rst);
            }
            return objresponse;


        }

        #endregion


        #region GetVideoComments
        [HttpPost]
        public Response<List<RespGetVideoComments>> GetVideoComments(ReqGetVideoComments objReq)
        {
            Response<List<RespGetVideoComments>> objresponse = new Response<List<RespGetVideoComments>>();
            List<RespGetVideoComments> objlist = new List<RespGetVideoComments>();
            UnseenTalentsMethod objmethod = new UnseenTalentsMethod();

            try
            {
                objlist = objmethod.GetVideoComments(objReq);

                objresponse.Create(true, 0, "Events for admin", objlist);


            }
            catch (Exception ex)
            {
                objresponse.Create(false, -1, "Events for admin", objlist);
            }
            return objresponse;


        }
        #endregion

        #region Payment
        [HttpPost]
        public Response<string> DoPayment(ReqPayment objreq)
        {
            string rst = "";
            Response<string> objresponse = new Response<string>();
            UnseenTalentsMethod objmethod = new UnseenTalentsMethod();

            try
            {
                rst = objmethod.DoPaymentBYUserid(objreq);
                objresponse.Create(true, 0, "pament successfully done", rst);


            }
            catch (Exception ex)
            {
                objresponse.Create(false, -1, "", rst);
            }
            return objresponse;


        }

        #endregion

        #endregion
    }
}
