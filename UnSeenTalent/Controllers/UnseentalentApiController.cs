using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using Newtonsoft.Json;
using UnseentalentsApp.Models.Db;
using UnseentalentsApp.Models.Repository;

namespace UnseentalentsApp.Controllers
{
    public class UnseentalentApiController : ApiController
    {
        [HttpPost]
        public Response<byte> AdminLogin(LoginModel Data)
        {
            var objresponse = new Response<byte>();
            var unSeenTalentsMethod = new UnseenTalentsMethod();
            if (Data != null)
            {
                using (var db = new UnseentalentdbDataContext())
                {
                    User user = db.Users.FirstOrDefault(t => t.Email == Data.Email && t.Password == Data.Password);
                    byte result = 0;

                    if (user != null)
                    {
                        var currentUser = new CurrentUser
                        {
                            Email = user.Email,
                            FirstName = user.FirstName,
                            isEmailVerify = true,
                            LastName = user.LastName,
                            ProfilePic = user.ProfilePic,
                            userid = user.Id,
                            userrole = unSeenTalentsMethod.GetUserRole(user.Id),
                        };
                        string json = JsonConvert.SerializeObject(currentUser);
                        FormsAuthentication.SetAuthCookie(json, Data.RememberMe);
                        result = 2; //sucess login
                        objresponse.Create(true, 0, "Admin login successfully", result);
                    }
                    else
                    {
                        result = 0; //Username password not valid
                        objresponse.Create(false, 1, "Admin login Failed", result);
                    }
                }
            }
            return objresponse;
        }

        /* public List<userrole> GetUserRole(Int64 userid)
        {
            var objuserrole = new List<userrole>();
            using (var db = new UnseentalentdbDataContext())
            {
                foreach (UserRole item in db.UserRoles.Where(u => u.UserId == userid).ToList())
                {
                    var objrole = new userrole();
                    objrole.roleid = Convert.ToInt32(item.userrole);
                    objuserrole.Add(objrole);
                }
            }
            return objuserrole;
        }*/

        [HttpPost]
        public Response<GetEventResponse> GetAllEventsForAdmin(object objreq)
        {
            var objresponse = new Response<GetEventResponse>();
            var objlist = new List<GetAllevents>();
            var objmethod = new UnseenTalentsMethod();
            var objeventresp = new GetEventResponse();
            try
            {
                objeventresp.EventList = objmethod.GetAllEventsForAdmin();
                if (objeventresp.EventList.Any())
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

        [HttpPost]
        public Response<GetEventResponse> GetAllEventsForVideoUpload(object objreq)
        {
            var objresponse = new Response<GetEventResponse>();
            var objlist = new List<GetAllevents>();
            var objmethod = new UnseenTalentsMethod();
            var objeventresp = new GetEventResponse();
            try
            {
                objeventresp.EventList = objmethod.GetAllEventsForVideoUpload();
                if (objeventresp.EventList.Any())
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

        [HttpPost]
        public Response<List<TokenModel>> GetAllToken()
        {
            var objresponse = new Response<List<TokenModel>>();
            var objlist = new List<TokenModel>();
            var objmethod = new UnseenTalentsMethod();

            try
            {
                objlist = objmethod.GetAllToken();
                if (objlist.Any())
                {
                    objresponse.Create(true, 0, "Events for admin", objlist);
                }
                else
                {
                    objresponse.Create(false, 1, "Events for admin", objlist);
                }
            }
            catch (Exception ex)
            {
                objresponse.Create(false, -1, "Events for admin", objlist);
            }
            return objresponse;
        }

        [HttpPost]
        public Response<List<TokenModel>> GetAvaliableTokenForUser()
        {
            var objresponse = new Response<List<TokenModel>>();
            var objlist = new List<TokenModel>();
            var objmethod = new UnseenTalentsMethod();
          
            try
            {
                objlist = objmethod.GetAvaliableTokenByUserId();
               
                if (objlist.Any())
                {
                    objresponse.Create(true, 0, "Events for admin", objlist);
                }
                else
                {
                    objresponse.Create(false, 1, "Events for admin", objlist);
                }
            }
            catch (Exception ex)
            {
                objresponse.Create(false, -1, "Events for admin", objlist);
            }
            return objresponse;
        }

        [HttpPost]
        public Response<bool> SaveEditEvents(GetAllevents objreq)
        {
            bool rst = false;
            var objresponse = new Response<bool>();
            var objmethod = new UnseenTalentsMethod();

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

        [HttpPost]
        public Response<string> EventAction(ActionModel model)
        {
            var response = new Response<string>();
            string value = "";


            var obj = new UnseenTalentsMethod();
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

        [HttpPost]
        public Response<int> UserRegistration(ReqRegistration objreq)
        {
            int rst = 0;
            var objresponse = new Response<int>();
            var objmethod = new UnseenTalentsMethod();

            try
            {
                rst = objmethod.UserRegistration(objreq);
                objresponse.Create(true, 0, "User Registration Success", rst);
            }
            catch (Exception ex)
            {
                objresponse.Create(false, -1, "User Registration Failed", rst);
            }
            return objresponse;
        }

        [HttpPost]
        public Response<byte> UserLogin(LoginModel Data)
        {
            var objresponse = new Response<byte>();
            var unSeenTalentsMethod = new UnseenTalentsMethod();
            byte result = 0;
            if (Data != null)
            {
                // string pwd = Encryption.Encrypt(Data.Password, ConstantValues.EncryptionKey);
                using (var db = new UnseentalentdbDataContext())
                {
                    User user =
                        db.Users.FirstOrDefault(
                            t =>
                                (t.Email == Data.Email || t.UserName == Data.Email) && t.IsActive == true &&
                                t.IsDeleted == false);
                    if (user != null)
                    {
                        if (user.Password == Data.Password)
                        {
                            var objmethod = new UnseenTalentsMethod();
                            var currentUser = new CurrentUser
                            {
                                Email = user.Email,
                                FirstName = user.UserName,
                                isEmailVerify = true,
                                LastName = user.LastName,
                                ProfilePic = objmethod.ReturnProfilePicture(user.ProfilePic),
                                userid = user.Id,
                                isToken = user.IsToken != null && Convert.ToBoolean(user.IsToken),
                                userrole = unSeenTalentsMethod.GetUserRole(user.Id)
                            };
                            string json = JsonConvert.SerializeObject(currentUser);
                            FormsAuthentication.SetAuthCookie(json, Data.RememberMe);
                            result = 2; //sucess login

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

        private HttpContextWrapper GetHttpContext(HttpRequestMessage request = null)
        {
            request = request ?? Request;

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper) request.Properties["MS_HttpContext"]);
            }
            if (HttpContext.Current != null)
            {
                return new HttpContextWrapper(HttpContext.Current);
            }
            return null;
        }

        [HttpPost]
        public Response<List<getEventModel>> GetAllevntsforWeb()
        {
            var objresponse = new Response<List<getEventModel>>();
            var objlist = new List<getEventModel>();
            var objmethod = new UnseenTalentsMethod();

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

        [HttpPost]
        public Response<string> SavePost()
        {
            var response = new Response<string>();
            string rst = "";
            try
            {
                HttpContextWrapper objwrapper = GetHttpContext(Request);
                HttpFileCollectionBase collection = objwrapper.Request.Files;

                string jsonvalue = objwrapper.Request.Form["json"];

                if (!string.IsNullOrEmpty(jsonvalue))
                {
                    var objitem = JsonConvert.DeserializeObject<ReqSaveEventPost>(jsonvalue);


                    objitem.file = "";
                    //int count = 0;
                    foreach (string file in collection)
                    {
                        HttpPostedFileBase file1 = collection.Get(file);
                        //  HttpPostedFileBase file1 = collection.Get(file);
                        if (file1.ContentType == "video/quicktime" || file1.ContentType == "video/mp4")
                        {
                            var videofile = new byte[file1.ContentLength];
                            file1.InputStream.Read(videofile, 0, file1.ContentLength);
                            BinaryWriter Writer = null;
                            if (videofile.Length > 0)
                            {
                                string videoname = Guid.NewGuid().ToString().Substring(0, 7) + "_.MOV";
                                if (file1.ContentType == "video/mp4")
                                {
                                    videoname = Guid.NewGuid().ToString().Substring(0, 7) + "_.mp4";
                                }
                                Writer =
                                    new BinaryWriter(
                                        File.OpenWrite(HttpContext.Current.Request.PhysicalApplicationPath +
                                                       "WebImages\\Video\\" + videoname));
                                Writer.Write(videofile);
                                Writer.Flush();
                                Writer.Close();
                                objitem.file = videoname;
                                // count = 1;
                            }
                        }
                    }
                    var objmethod = new UnseenTalentsMethod();

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
            return response;
        }

        [HttpPost]
        public Response<List<RespGetEventPosts>> GetEventPosts(ReqGetEventPosts objReq)
        {
            var objresponse = new Response<List<RespGetEventPosts>>();
            var objlist = new List<RespGetEventPosts>();
            var objmethod = new UnseenTalentsMethod();

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

        [HttpPost]
        public Response<List<RespGetRecentEvents>> GetRecentEvents()
        {
            var objresponse = new Response<List<RespGetRecentEvents>>();
            var objlist = new List<RespGetRecentEvents>();
            var objmethod = new UnseenTalentsMethod();

            try
            {
                objlist = objmethod.GetRecentEvents();
                objresponse.Create(true, 0, "Events for admin", objlist);
            }
            catch (Exception ex)
            {
                objresponse.Create(false, -1, "Events for admin", objlist);
            }
            return objresponse;
        }

        [HttpPost]
        public Response<bool> AddComment(ReqAddComment objreq)
        {
            bool rst = false;
            var objresponse = new Response<bool>();
            var objmethod = new UnseenTalentsMethod();

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

        [HttpPost]
        public Response<List<RespGetVideoComments>> GetVideoComments(ReqGetVideoComments objReq)
        {
            var objresponse = new Response<List<RespGetVideoComments>>();
            var objlist = new List<RespGetVideoComments>();
            var objmethod = new UnseenTalentsMethod();

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

        [HttpPost]
        public Response<string> DoPayment(ReqPayment objreq)
        {
            string rst = "";
            var objresponse = new Response<string>();
            var objmethod = new UnseenTalentsMethod();

            try
            {
                rst = objmethod.DoPaymentByUserId(objreq);
                objresponse.Create(true, 0, "payment successfuly done", rst);
            }
            catch (Exception ex)
            {
                objresponse.Create(false, -1, "", rst);
            }
            return objresponse;
        }

        [HttpPost]
        public Response<string> subscribeNewsLetter(ReqNewsLetter objnewsLetter)
        {
            var response = new Response<string>();
            string rst = "";
            try
            {
                var objmethod = new UnseenTalentsMethod();
                rst = objmethod.SubscribeNewsLetter(objnewsLetter);
                if (rst == "1")
                {
                    response.Create(true, 1, "Subscripe successfully", rst);
                }
                else
                {
                    response.Create(false, 1, "Subscripe successfully", rst);
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

        [HttpPost]
        public Response<int> SaveUploadedFileDetails(UploadedFile model)
        {
            int rst = 0;
            var objresponse = new Response<int>();
            var objmethod = new UnseenTalentsMethod();

            try
            {
                rst = objmethod.SaveUploadedFileDetails(model);
                objresponse.Create(true, 0, rst == 1 ? "Video has been upload" : "Video failed to upload", rst);
            }
            catch (Exception ex)
            {
                objresponse.Create(false, -1, "", rst);
            }
            return objresponse;
        }

        [HttpPost]
        public Response<List<RespEventForSearch>> GetEventsforSearch(ReqSearch objSearch)
        {
            var objresponse = new Response<List<RespEventForSearch>>();
            var objlist = new List<RespEventForSearch>();
            var objmethod = new UnseenTalentsMethod();

            try
            {
                objlist = objmethod.GetEventsforSearch(objSearch);

                objresponse.Create(true, 0, "Events for admin", objlist);
            }
            catch (Exception ex)
            {
                objresponse.Create(false, -1, "Events for admin", objlist);
            }
            return objresponse;
        }

        [HttpGet]
        public Response<List<CategoryModel>> GetAllCategories()
        {
            var objresponse = new Response<List<CategoryModel>>();
            var objlist = new List<CategoryModel>();
            var objmethod = new UnseenTalentsMethod();

            try
            {
                objlist = objmethod.GetAllEventCategories();

                objresponse.Create(true, 0, "All Event Categories", objlist);
            }
            catch (Exception ex)
            {
                objresponse.Create(false, -1, "No Event Categories", objlist);
            }
            return objresponse;
        }

        [HttpPost]
        public Response<string> postContactUs(ReqContactUs objReq)
        {
            var response = new Response<string>();
            string rst = "";
            try
            {
                var objmethod = new UnseenTalentsMethod();
                rst = objmethod.PostContactUs(objReq);
                if (rst == "1")
                {
                    response.Create(true, 1, "Request send successfully", rst);
                }
                else
                {
                    response.Create(false, 1, "Invalid request", rst);
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

        [HttpPost]
        public Response<string> postVote(ReqVote objReq)
        {
            var response = new Response<string>();
            string rst = "";
            try
            {
                var objmethod = new UnseenTalentsMethod();
                rst = objmethod.PostVote(objReq);
                if (rst == "1")
                {
                    response.Create(true, 1, "successfully", rst);
                }
                else
                {
                    response.Create(false, 1, "Invalid request", rst);
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

        [HttpPost]
        public Response<int> CheckEventStatus(ReqGetEventPosts objReq)
        {
            var response = new Response<int>();
            int rst = 0;
            try
            {
                var objmethod = new UnseenTalentsMethod();
                rst = objmethod.CheckEventStatus(objReq);
                response.Create(true, 1, "successfully", rst);
            }
            catch (Exception ex)
            {
                response.Create(false, -1, Messages.FormatMessage(ex.Message), rst);
            }
            finally
            {
                // return response;
            }
            return response;
        }
    }
}