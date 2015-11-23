using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using UnseentalentsApp.Enums;
using UnseentalentsApp.Helpers;
using UnseentalentsApp.Models.Db;

namespace UnseentalentsApp.Models.Repository
{
    public class UnseenTalentsMethod
    {
        private readonly string _siteUrl = AppSettingHelper.GetSiteUrl();

        public string ReturnImageFullPath(string imagePath)
        {
            return
                imagePath =
                    string.IsNullOrWhiteSpace(imagePath)
                        ? ""
                        : imagePath.IndexOf("http", StringComparison.Ordinal) >= 0
                            ? imagePath
                            : AppSettingHelper.GetImagePath() + imagePath;
        }

        public string ReturnProfilePicture(string imagePath)
        {
            return
                imagePath =
                    string.IsNullOrWhiteSpace(imagePath)
                        ? ""
                        : imagePath.IndexOf("http", StringComparison.Ordinal) >= 0
                            ? imagePath
                            : AppSettingHelper.GetProfileImagesPath() + imagePath;
        }

        public string VideoFullPath(string imagePath)
        {
            return
                imagePath =
                    string.IsNullOrWhiteSpace(imagePath)
                        ? ""
                        : imagePath.IndexOf("http", StringComparison.Ordinal) >= 0
                            ? imagePath
                            : AppSettingHelper.GetImagePath() + "/Video/" + imagePath;
        }

        public List<GetAllevents> GetAllEventsForAdmin()
        {
            var objlist = new List<GetAllevents>();
            using (var db = new UnseentalentdbDataContext())
            {
                foreach (Event item in db.Events.ToList().Where(d => d.IsDeleted == false))
                {
                    var objevent = new GetAllevents();
                    objevent.eventtitle = item.EventTitle;
                    objevent.creatorid = Convert.ToInt64(item.CreatorId);
                    objevent.eventdesc = item.EventDesc;
                    objevent.eventstartdate = Convert.ToString(item.EventStartDate);
                    objevent.eventenddate = Convert.ToString(item.EventEndDate);
                    objevent.isactive = Convert.ToString(item.IsActive);
                    objevent.isdeleted = Convert.ToString(item.IsDeleted);
                    objevent.eventid = item.Id;
                    objevent.eventImage = ReturnImageFullPath(item.EventImage);
                    objlist.Add(objevent);
                }
            }
            return objlist;
        }

        public List<GetAllevents> GetAllEventsForVideoUpload()
        {
            var objlist = new List<GetAllevents>();
            using (var db = new UnseentalentdbDataContext())
            {
                foreach (
                    Event item in db.Events.ToList().Where(d => d.IsDeleted == false && d.EventEndDate >= DateTime.Now))
                {
                    var objevent = new GetAllevents();
                    objevent.eventtitle = item.EventTitle;
                    objevent.creatorid = Convert.ToInt64(item.CreatorId);
                    objevent.eventdesc = item.EventDesc;
                    objevent.eventstartdate = Convert.ToString(item.EventStartDate);
                    objevent.eventenddate = Convert.ToString(item.EventEndDate);
                    objevent.isactive = Convert.ToString(item.IsActive);
                    objevent.isdeleted = Convert.ToString(item.IsDeleted);
                    objevent.eventid = item.Id;
                    objevent.eventImage = ReturnImageFullPath(item.EventImage);
                    objlist.Add(objevent);
                }
            }
            return objlist;
        }

        public List<TokenModel> GetAvaliableTokenByUserId()
        {
            var cUser = new CurrentUser();
            var objlist = new List<TokenModel>();
            using (var db = new UnseentalentdbDataContext())
            {
                foreach (
                    Managetoken item in
                        db.Managetokens.ToList()
                            .Where(
                                d =>
                                    d.IsActive && d.UserId == cUser.userid && d.WillExpireOn >= DateTime.Now &&
                                    d.RemainingUploadCount > 0))
                {
                    var objevent = new TokenModel();
                    objevent.Id = item.Token.Id;
                    objevent.Name = item.Token.Name;
                    objevent.Amount = item.Token.Amount;
                    objevent.WillExpireOn = item.WillExpireOn;
                    objevent.ExpireDurationInDays = item.Token.ExpireDurationInDays;
                    objevent.NoOfUploadsAllowed = item.Token.NoOfUploadsAllowed;
                    objevent.UniqueTokenId = item.UniqueTokenId;
                    objevent.NoOfUploadRemaining = item.RemainingUploadCount;
                    objevent.CreateDate = item.CreateDate;
                    objlist.Add(objevent);
                }
            }
            return objlist;
        }

        public List<TokenModel> GetAllToken()
        {
            var objlist = new List<TokenModel>();
            using (var db = new UnseentalentdbDataContext())
            {
                foreach (Token item in db.Tokens.ToList().Where(d => d.IsActive))
                {
                    var objevent = new TokenModel();
                    objevent.Id = item.Id;
                    objevent.Name = item.Name;
                    objevent.Amount = item.Amount;
                    objevent.ExpireDurationInDays = item.ExpireDurationInDays;
                    objevent.NoOfUploadsAllowed = item.NoOfUploadsAllowed;
                    objlist.Add(objevent);
                }
            }
            return objlist;
        }

        public List<CategoryModel> GetAllEventCategories()
        {
            var objlist = new List<CategoryModel>();
            using (var db = new UnseentalentdbDataContext())
            {
                objlist.AddRange(
                    db.Categories.ToList()
                        .Select(
                            item => new CategoryModel {CategoryId = item.Id, CategoryName = Convert.ToString(item.Name)}));
            }
            return objlist;
        }

        public bool SaveEditEvents(GetAllevents objReq)
        {
            bool rst = false;
            try
            {
                using (var db = new UnseentalentdbDataContext())
                {
                    var objUser = new CurrentUser();
                    if (objReq.eventid == 0)
                    {
                        var objNew = new Event();
                        objNew.CreatorId = objUser.userid;
                        objNew.CreateDate = DateTime.UtcNow;
                        objNew.EventDesc = objReq.eventdesc;
                        objNew.EventTitle = objReq.eventtitle;
                        objNew.EventStartDate = Convert.ToDateTime(objReq.eventstartdate);
                        objNew.EventEndDate = Convert.ToDateTime(objReq.eventstartdate).AddDays(42);
                        objNew.IsActive = true;
                        objNew.IsDeleted = false;
                        db.Events.InsertOnSubmit(objNew);
                        if (!string.IsNullOrWhiteSpace(objReq.eventImage))
                            objNew.EventImage = objReq.eventImage;
                        db.SubmitChanges();
                        rst = true;
                    }
                    else
                    {
                        Event evt = db.Events.FirstOrDefault(e => e.Id == objReq.eventid);
                        if (evt != null)
                        {
                            evt.CreateDate = DateTime.UtcNow;
                            evt.EventDesc = objReq.eventdesc;
                            evt.EventTitle = objReq.eventtitle;
                            evt.EventStartDate = Convert.ToDateTime(objReq.eventstartdate);
                            evt.EventEndDate = Convert.ToDateTime(objReq.eventstartdate).AddDays(42);
                            if (!string.IsNullOrWhiteSpace(objReq.eventImage))
                                evt.EventImage = objReq.eventImage;
                        }
                        db.SubmitChanges();
                        rst = true;
                    }
                }
            }
            catch
            {
            }
            return rst;
        }

        public string EventAction(ActionModel model)
        {
            string value = "";
            using (var db = new UnseentalentdbDataContext())
            {
                Event events = db.Events.FirstOrDefault(t => t.Id == Convert.ToInt64(model.ID));
                if (events != null)
                {
                    switch (model.Type)
                    {
                        case ActionType.Delete:
                            events.IsDeleted = true;
                            value = "1";
                            break;
                        case ActionType.Block:
                            events.IsActive = false;
                            value = "2";
                            break;
                        case ActionType.UnBlock:
                            events.IsActive = true;
                            value = "3";
                            break;
                        default:
                            break;
                    }

                    db.SubmitChanges();
                }
                else
                {
                    value = "0";
                }
                return value;
            }
        }

        public List<Role> GetAllRole()
        {
            var roles = new List<Role>();
            using (var db = new UnseentalentdbDataContext())
            {
                roles.AddRange(db.Roles.ToList().Select(role => new Role
                {
                    Id = role.Id,
                    Name = role.Name
                }));
            }
            return roles;
        }

        public bool SendEmail()
        {
            //INITIALIZE AWS CLIENT//
            AmazonSimpleEmailServiceConfig amConfig = new AmazonSimpleEmailServiceConfig();
            //amConfig.UseSecureStringForAwsSecretKey = false;
            AmazonSimpleEmailServiceClient amzClient = new AmazonSimpleEmailServiceClient(
              AppSettingHelper.GetAmazonAccessKey(),AppSettingHelper.GetAmazonSecretKey(),//ConfigurationManager.AppSettings["AWSAccessKey"].ToString(),
               RegionEndpoint.USEast1);// ConfigurationManager.AppSettings["AWSSecretKey"].ToString(), amConfig);

            //ArrayList that holds To Emails. It can hold 1 Email to any
            //number of emails in case what to send same message to many users.
            var to = new List<string>();
            to.Add("unseentalents@gmail.com");

            //Create Your Bcc Addresses as well as Message Body and Subject
            Destination dest = new Destination();
            dest.BccAddresses.Add("aakash.pawar.cs@gmail.com");
            //string body = Body;
            string subject = "Subject : " + "Demo Mail";
            Body bdy = new Body();
            bdy.Html = new Amazon.SimpleEmail.Model.Content("Test Mail");
            Amazon.SimpleEmail.Model.Content title = new Amazon.SimpleEmail.Model.Content(subject);
            Message message = new Message(title, bdy);

            //Create A Request to send Email to this ArrayList with this body and subject
            try
            {
                SendEmailRequest ser = new SendEmailRequest("unseentalents@gmail.com", dest, message);
                SendEmailResponse seResponse = amzClient.SendEmail(ser);
               // SendEmailResult seResult = seResponse.SendEmailResult;
            }
            catch (Exception ex)
            {

            }

            return true;
        }

        private void SendVerifyEmail(string email, string type, Int64 id)
        {
            try
            {
                string desctr = Encryption.EncryptURL(id + "," + type);
                desctr = desctr.Replace("/", "_").Replace("+", "-");
                // var sanitized = HttpUtility.UrlEncode(Desctr);
                //HttpServerUtility.UrlTokenEncode(Desctr);
                string sanitized = HttpUtility.UrlEncode(desctr);
                string body = Common.ReadEmailformats("signup.html");
                string path = _siteUrl + "/webapp/accountactivate/" + sanitized;
                body = body.Replace("$$UserEmailId$$", email);
                body = body.Replace("$$SignUpLink$$", "<a href='" + path + "'> Click here to verify your email </a>");
                var objSendMail = new SendMail();
                string response = objSendMail.SendEmail(email, AppSettingHelper.GetAdminEmail(), "Account Created", body,
                    "");
            }
            catch (Exception e)
            {
            }
        }

        public List<getEventModel> GetAllevntsforWeb()
        {
            var objlist = new List<getEventModel>();
            using (var db = new UnseentalentdbDataContext())
            {
                IEnumerable<getEventModel> items = db.usp_GetUpcomingEvents().Select(n => new getEventModel
                {
                    eventtitle = n.eventtitle,
                    creatorid = 1,
                    eventdesc = n.eventdesc,
                    eventstartdate = Convert.ToString(n.createdate),
                    eventenddate = Convert.ToString(n.eventenddate),
                    eventid = n.id,
                    eventImage = ReturnImageFullPath(n.eventImage),
                    urlKey = n.eventtitle.ToSeoUrl()
                });

                objlist.AddRange(items);
            }
            return objlist;
        }

        public int SaveUploadedFileDetails(UploadedFile objReq)
        {
            try
            {
                var objUser = new CurrentUser();
                using (var db = new UnseentalentdbDataContext())
                {
                    var objNew = new Video();
                    objNew.UserId = objUser.userid;
                    objNew.EventId = objReq.SelectedEvent;
                    objNew.VideoPath = objReq.FileUrl;
                    objNew.VideoTitle = objReq.FileName;
                    objNew.VideoDesc = objReq.Description;
                    objNew.CreatedDate = objReq.CreatedAt;
                    objNew.IsActive = true;
                    objNew.IsDeleted = false;
                    objNew.TokenUniqueId = objReq.SelectedToken;
                    objNew.CategoryId = objReq.SelectedCategory;
                    db.Videos.InsertOnSubmit(objNew);
                    db.SubmitChanges();

                    Managetoken tokenUsed = db.Managetokens.FirstOrDefault(t => t.UniqueTokenId == objReq.SelectedToken);
                    if (tokenUsed != null) tokenUsed.RemainingUploadCount -= 1;
                    db.SubmitChanges();
                    return 1;
                }
            }
            catch
            {
                return 0;
            }
        }

        public string SaveEventPost(ReqSaveEventPost objReq)
        {
            string rst = "0";
            try
            {
                using (var db = new UnseentalentdbDataContext())
                {
                    var objNew = new Video();
                    objNew.CreatedDate = DateTime.UtcNow;
                    objNew.EventId = Convert.ToInt64(objReq.eventId);
                    objNew.IsActive = true;
                    objNew.IsDeleted = false;
                    objNew.UserId = Convert.ToInt64(objReq.userId);
                    objNew.VideoDesc = "";
                    objNew.VideoTitle = objReq.postText;
                    objNew.VideoPath = objReq.file;
                    db.Videos.InsertOnSubmit(objNew);
                    db.SubmitChanges();
                    rst = "1";
                }
            }
            catch
            {
            }
            return rst;
        }

        public List<RespGetEventPosts> GetEventPosts(ReqGetEventPosts objReq)
        {
            var objlist = new List<RespGetEventPosts>();
            using (var db = new UnseentalentdbDataContext())
            {
                IEnumerable<RespGetEventPosts> items =
                    db.usp_GetEventPosts(Convert.ToInt64(objReq.eventId), Convert.ToInt64(objReq.userId))
                        .Select(n => new RespGetEventPosts
                        {
                            eventId = Convert.ToString(n.Eventid),
                            videoId = Convert.ToString(n.videoId),
                            userId = Convert.ToString(n.userId),
                            userName = n.Username,
                            profilePic = ReturnProfilePicture(n.profilePic),
                            eventTitle = n.videoTitle,
                            videoDescription = n.VideoDesc,
                            timeAgo = n.CreatedDate.ToShortDateString(),
                            video = VideoFullPath(n.videoPath),
                            commentCount = Convert.ToString(n.videoCount),
                            voteCount = Convert.ToString(n.voteCount),
                            isVote = n.isVote,
                            postComments = new List<RespGetVideoComments>()
                        });

                objlist.AddRange(items);
            }
            return objlist;
        }

        public bool SaveVideoComments(ReqAddComment objReq)
        {
            bool rst = false;
            try
            {
                using (var db = new UnseentalentdbDataContext())
                {
                    var objNew = new Comment();
                    objNew.CreatedDate = DateTime.UtcNow;
                    objNew.VideoId = Convert.ToInt64(objReq.videoId);
                    objNew.IsActive = true;
                    objNew.IsDeleted = false;
                    objNew.UserId = Convert.ToInt64(objReq.userId);
                    objNew.CommentText = objReq.commentText;
                    db.Comments.InsertOnSubmit(objNew);
                    db.SubmitChanges();
                    rst = true;
                }
            }
            catch
            {
            }
            return rst;
        }

        public List<RespGetVideoComments> GetVideoComments(ReqGetVideoComments objReq)
        {
            var objlist = new List<RespGetVideoComments>();
            using (var db = new UnseentalentdbDataContext())
            {
                IEnumerable<RespGetVideoComments> items =
                    db.usp_GetVideoComments(Convert.ToInt64(objReq.videoId)).Select(n => new RespGetVideoComments
                    {
                        commentId = Convert.ToString(n.commentId),
                        userId = Convert.ToString(n.userId),
                        userName = n.username,
                        profilePic = ReturnProfilePicture(n.profilePic),
                        commentText = n.commentText,
                        timeAgo = "",
                    });

                objlist.AddRange(items);
            }
            return objlist;
        }

        public string DoPaymentByUserId(ReqPayment objreq)
        {
            string returnval = "0";
            try
            {
                var objmodel = new paymentrequestfortoken();
                objmodel.cardnumber = objreq.cardnumber;
                objmodel.expiryear = objreq.year;
                objmodel.expirymonth = objreq.cardexpmonth;
                objmodel.Name = objreq.nameoncard;

                string tokenId = StripPayment.GetTokenId(objmodel);
                if (!string.IsNullOrEmpty(tokenId))
                {
                    var objchargemodel = new paymentrequestforcharge();
                    objchargemodel.amount = objreq.amount;
                    objchargemodel.description = "Payment from User " + objreq.userId;
                    objchargemodel.tokenid = tokenId;
                    string chargeid = StripPayment.ChargeCustomer(objchargemodel);
                    if (!string.IsNullOrEmpty(chargeid))
                    {
                        InsertTransaction(Convert.ToInt64(objreq.userId), chargeid, objreq.amount,
                            (int) TransactionType.Credit, objreq.tokenId);
                        returnval = "1";
                    }
                }
                else
                {
                    returnval = "-1";
                }
            }
            catch (Exception ex)
            {
                returnval = ex.Message;
            }
            return returnval;
        }

        public List<Token> GetAllTokens()
        {
            var tokens = new List<Token>();
            using (var db = new UnseentalentdbDataContext())
            {
                tokens.AddRange(db.Tokens.ToList().Where(t => t.IsActive).Select(token => new Token
                {
                    Id = token.Id,
                    Name = token.Name,
                    Amount = token.Amount,
                    ExpireDurationInDays = token.ExpireDurationInDays,
                }));
            }
            return tokens;
        }

        public TokenModel GetTokenById(int tokenId)
        {
            TokenModel token;
            using (var db = new UnseentalentdbDataContext())
            {
                token = db.Tokens.Where(t => t.Id == tokenId).Select(t => new TokenModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Amount = t.Amount,
                    ExpireDurationInDays = t.ExpireDurationInDays,
                    NoOfUploadsAllowed = t.NoOfUploadsAllowed,
                }).FirstOrDefault();
            }
            return token;
        }

        private void InsertTransaction(Int64 userid, string transactionnumber, string amount, int tokencredit,
            int tokenId)
        {
            TokenModel token = GetTokenById(tokenId);
            DateTime tokenExpiresOn = DateTime.UtcNow.AddDays(token.ExpireDurationInDays);
            using (var db = new UnseentalentdbDataContext())
            {
                var objpayment = new PaymentTransaction();
                objpayment.Amount = Convert.ToDouble(amount);
                objpayment.IsActive = true;
                objpayment.IsDeleted = false;
                objpayment.TokenId = tokenId;
                objpayment.TransactionDate = DateTime.UtcNow;
                objpayment.TransactionNumber = transactionnumber;
                objpayment.UserId = userid;
                db.PaymentTransactions.InsertOnSubmit(objpayment);
                db.SubmitChanges();

                var objtoken = new Managetoken();
                objtoken.CreateDate = DateTime.UtcNow;
                objtoken.IsActive = true;
                objtoken.TokenId = tokenId;
                objtoken.UserId = userid;
                objtoken.UniqueTokenId = "UST-" + CommonHelpers.GetUniqueKey(10);
                objtoken.WillExpireOn = tokenExpiresOn;
                objtoken.RemainingUploadCount = token.NoOfUploadsAllowed;
                db.Managetokens.InsertOnSubmit(objtoken);
                db.SubmitChanges();
            }
        }

        public List<RespGetRecentEvents> GetRecentEvents()
        {
            var objlist = new List<RespGetRecentEvents>();
            using (var db = new UnseentalentdbDataContext())
            {
                IEnumerable<RespGetRecentEvents> items = db.usp_GetRecentEvents().Select(n => new RespGetRecentEvents
                {
                    eventId = Convert.ToString(n.id),
                    urlKey = n.eventtitle.ToSeoUrl(),
                    eventTitle = n.eventtitle
                });

                objlist.AddRange(items);
            }
            return objlist;
        }

        public string SubscribeNewsLetter(ReqNewsLetter objnewsLetter)
        {
            string rst = "0";
            if (!string.IsNullOrWhiteSpace(objnewsLetter.email))
            {
                try
                {
                    string body = Common.ReadEmailformats("newsletter.html");
                    body = body.Replace("$$UserEmailId$$", objnewsLetter.email);
                    var objSendMail = new SendMail();
                    objSendMail.SendEmail(objnewsLetter.email, AppSettingHelper.GetAdminEmail(), "Account Created", body,
                        "");
                    rst = "1";
                }
                catch
                {
                }
            }
            return rst;
        }

        public List<RespEventForSearch> GetEventsforSearch(ReqSearch objSearch)
        {
            var objlist = new List<RespEventForSearch>();
            if (string.IsNullOrWhiteSpace(objSearch.searchText))
            {
                using (var db = new UnseentalentdbDataContext())
                {
                    IEnumerable<RespEventForSearch> items =
                        db.usp_GetUpcomingEvents().Select(n => new RespEventForSearch
                        {
                            eventId = Convert.ToString(n.id),
                            eventName = n.eventtitle,
                            urlKey = n.eventtitle.ToSeoUrl()
                        });

                    objlist.AddRange(items);
                }
            }
            return objlist;
        }

        public string PostContactUs(ReqContactUs objReq)
        {
            string rst = "0";
            if (!string.IsNullOrWhiteSpace(objReq.emailId))
            {
                try
                {
                    using (var db = new UnseentalentdbDataContext())
                    {
                        var objNew = new ContactInfo();
                        objNew.contactName = objReq.name;
                        objNew.emailId = objReq.emailId;
                        objNew.description = objReq.description;

                        db.ContactInfos.InsertOnSubmit(objNew);
                        db.SubmitChanges();
                        rst = "1";
                    }
                }
                catch
                {
                }
            }
            return rst;
        }

        public string PostVote(ReqVote objReq)
        {
            string rst = "0";
            try
            {
                if (!string.IsNullOrWhiteSpace(objReq.postId))
                {
                    using (var db = new UnseentalentdbDataContext())
                    {
                        Vote votedByUser =
                            db.Votes.FirstOrDefault(x => x.UserId == Convert.ToInt64(objReq.userId) &&
                                                         x.VideoId == Convert.ToInt64(objReq.postId));

                        if (votedByUser == null)
                        {
                            var objNew = new Vote();
                            objNew.VideoId = Convert.ToInt64(objReq.postId);
                            objNew.UserId = Convert.ToInt64(objReq.userId);
                            objNew.IsVote = objReq.isLike == "True";
                            objNew.CreatedDate = DateTime.UtcNow;

                            db.Votes.InsertOnSubmit(objNew);
                        }
                        else
                        {
                            votedByUser.IsVote = objReq.isLike == "True";
                            votedByUser.CreatedDate = DateTime.UtcNow;
                        }
                        db.SubmitChanges();
                        rst = "1";
                    }
                }
            }
            catch
            {
            }
            return rst;
        }

        public int CheckEventStatus(ReqGetEventPosts objReq)
        {
            int value = 0;
            using (var db = new UnseentalentdbDataContext())
            {
                usp_CheckEventStatusResult result =
                    db.usp_CheckEventStatus(Convert.ToInt64(objReq.eventId), Convert.ToInt64(objReq.userId))
                        .FirstOrDefault();
                if (result != null) value = Convert.ToInt16(result.isUploading);
            }
            return value;
        }

        public Tokenresponse GetUserToken(Int64 userId)
        {
            var tokenresponse = new Tokenresponse();
            using (var db = new UnseentalentdbDataContext())
            {
                List<Managetoken> result =
                    db.Managetokens.Where(t => t.IsActive && t.UserId == userId).ToList();
                tokenresponse.Totaltoken = result.Count();
            }
            return tokenresponse;
        }

        public bool VerifyEmail(Int64 id, string type)
        {
            string res = ""; //Not exist
            var objdbcontext = new UnseentalentdbDataContext();
            try
            {
                //  objdbcontext.Connection.Open();
                User user = objdbcontext.Users.FirstOrDefault(u => u.Id == id);
                if (user != null)
                {
                    user.IsActive = true;
                    objdbcontext.SubmitChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                objdbcontext.Connection.Close();
            }
        }

        public int UserRegistration(ReqRegistration objReq)
        {
            int rst = 0;
            try
            {
                using (var db = new UnseentalentdbDataContext())
                {
                    User user = db.Users.FirstOrDefault(p => p.Email == objReq.emailId.ToLower());
                    if (user == null)
                    {
                        var objNew = new User();
                        objNew.UserName = objReq.userName;
                        objNew.Email = objReq.emailId.ToLower();
                        objNew.Password = objReq.password;
                        objNew.CreatedDate = DateTime.UtcNow;
                        objNew.IsActive = false;
                        objNew.IsDeleted = false;
                        objNew.IsToken = true;
                        objNew.ProfilePic = objReq.image;
                        objNew.NumberOfToken = 0;
                        db.Users.InsertOnSubmit(objNew);
                        db.SubmitChanges();


                        /*var objRole = new tbl_userrole();
                        objRole.createdate = DateTime.UtcNow;
                        objRole.isactive = false;
                        objRole.userid = objNew.id;
                        objRole.isdeleted = false;
                        objRole.userrole = 2;
                        db.tbl_userroles.InsertOnSubmit(objRole);*/
                        var objRole = new UserRole();
                        objRole.UserId = objNew.Id;
                        Role firstOrDefault =
                            GetAllRole().FirstOrDefault(r => r.Name == CommonHelpers.ToEnumDescription(RolesEnum.User));
                        objRole.RoleId = firstOrDefault.Id;
                        db.UserRoles.InsertOnSubmit(objRole);

                        db.SubmitChanges();

                        rst = 1; //Successfully Registred
                        SendVerifyEmail(objReq.emailId.ToLower(), "1", objNew.Id);
                    }
                    else if (user.IsActive == false)
                    {
                        rst = 1; //Successfully Registred
                        SendVerifyEmail(objReq.emailId.ToLower(), "1", user.Id);
                    }
                    else
                    {
                        rst = 3; //Email Id already exists
                    }
                }
            }
            catch
            {
                rst = 2; //Error Occured
            }
            return rst;
        }

        public List<RolesModel> GetUserRole(Int64 userid)
        {
            var objuserrole = new List<RolesModel>();
            using (var db = new UnseentalentdbDataContext())
            {
                foreach (UserRole item in db.UserRoles.Where(u => u.UserId == userid).ToList())
                {
                    var objrole = new RolesModel();
                    objrole.RoleId = item.Role.Id;
                    objrole.RoleName = item.Role.Name;
                    objuserrole.Add(objrole);
                }
            }
            return objuserrole;
        }
    }
}