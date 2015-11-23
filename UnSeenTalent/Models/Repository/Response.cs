using System;
using System.Collections.Generic;
using System.Web;

namespace UnseentalentsApp.Models.Repository
{
    [Serializable]
    public class Response<T>
    {
        public Int16 Error;
        public string Message;
        public T Result;
        public bool Success;

        public void Create(bool success, Int16 error, string message, T result)
        {
            Success = success;
            Error = error;
            Message = message;
            Result = result;
        }
    }

    public class Messages
    {
        public const string AppVersion = "1.0";
        public const string Null_VALUES = "null value has passed to update";
        public const string INVALID_REQ = "Invalid Request";
        public const string NORECORD_FOUND = "No record found.";
        public const string INVALID_USER = "Invalid User.";
        public const string Email_NOT_EXISTS = "This Email {0} does not exist.";
        public const string SUCCESS = "{0} successfully.";
        public const string Email_exists = "Email already exists.";
        public const string Ivnalid_Password = "Invalid old password.";
        public const string Passwordsent = "Password sent successfully on you listed mail.";
        public const string EmailNotExists = "Email not exist.";
        public const string INVALID_Request = "Invalid Request. Please try again.";
        public const string dataAdded = "Added successfully.";
        public const string dataUpdated = "Updated successfully.";
        public const string Already = "Already exist.";
        public const string ErrorOccure = "Error Occurred.";
        public const string block = "Block Sucessfully";
        public const string unBlock = "Unblock Sucessfully";
        public const string accept = "Accepted Sucessfully";
        public const string reject = "Rejected Sucessfully";
        public const string delete = "Delete Sucessfully";
        public const string deliver = "Delivered Sucessfully";
        public const string CannotProceed = "Sorry not allow to cancel order it's too late now.";
        public const string SuccessMessageForFileUpload = "File Uploaded Successfully";
        public const string InvalidFormat = "File not in Correct Format.There might be some data which is incorrect.";
        public const string notVarifiedProduct = "NotVerifiedProductType";
        public const string notVarifiedService = "NotVerifiedServiceType";
        public const string feedbackAdded = "Your Feedback Send Successfully";
        public const string notVarified = "Not Verified";
        public const string Varified = "Verified";

        public const string shortedQyt =
            "Sorry, Your order can not be proceed due to insufficient quantity. Please Update your quantity to proceed this order.";

        public const string rechargeSucess = "Recharged Successfully";
        public const string emailSendSuccess = "Email Sent Successfully";
        public const string emailSendFail = "Email not Sent. Please check the details you enter";
        public const string messageSendSuccess = "Message Sent Successfully";
        public const string messageSendFail = "Message not Sent. Try again";
        public const string readMsg = "Read Sucessfully";
        public const string replyMsg = "Replied Sucessfully";

        public const string vendorNotVerified =
            "You are not verified still, our representative will send you a email after that you would upload your documents !";

        public const string docsNotVerified =
            "Your documents are not verified still,our representative will send you a email when your documents is verified !";

        public const string nextPhaseVerification =
            "Your documents are verified but you are not confirmed still,our representative will send you a email after that you are confirmed !";

        public const string tempBlocked =
            "Your are temporarily blocked ,For more information please contact to our representative !";


        public static string FormatMessage(string message, params string[] args)
        {
            return String.Format(message, args);
        }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }


    public class TokenModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public DateTime WillExpireOn { get; set; }
        public int ExpireDurationInDays { get; set; }
        public int NoOfUploadsAllowed { get; set; }
        public string UniqueTokenId { get; set; }
        public DateTime CreateDate { get; set; }
        public int NoOfUploadRemaining { get; set; }
        public bool Selected { get; set; }
    }

    public class GetEventResponse
    {
        public List<GetAllevents> EventList { get; set; }
        public Int32 total { get; set; }
    }


    public enum ActionType
    {
        Delete = 1,
        Block = 2,
        UnBlock = 3,
        Accept = 4,
        Reject = 5,
    }

    public class ActionModel
    {
        public ActionType Type { get; set; }
        public Int64 ID { get; set; }
    }

    public class GetAllevents
    {
        public string eventtitle { get; set; }
        public Int64 creatorid { get; set; }
        public string eventdesc { get; set; }
        public string eventstartdate { get; set; }
        public string eventenddate { get; set; }
        public string isactive { get; set; }
        public string isdeleted { get; set; }
        public Int64 eventid { get; set; }
        public string eventImage { get; set; }
    }

    public class ReqRegistration
    {
        public string userName { get; set; }
        public string emailId { get; set; }
        public string password { get; set; }
        public string image { get; set; }
    }

    public class paymentrequestfortoken
    {
        public string cardnumber { get; set; }
        public string expirymonth { get; set; }
        public string expiryear { get; set; }
        public string Name { get; set; }
    }

    public class paymentrequestforcharge
    {
        public string tokenid { get; set; }
        public string amount { get; set; }
        public string description { get; set; }
    }

    public class getEventModel : GetAllevents
    {
        public string urlKey { get; set; }
    }

    public class ReqSaveEventPost
    {
        public string eventId { get; set; }
        public string userId { get; set; }
        public string postText { get; set; }
        public string file { get; set; }
    }


    public class ReqGetEventPosts
    {
        public string eventId { get; set; }
        public string userId { get; set; }
    }

    public class RespGetEventPosts
    {
        public string eventId { get; set; }
        public string videoId { get; set; }
        public string userId { get; set; }
        public string userName { get; set; }
        public string profilePic { get; set; }
        public string eventTitle { get; set; }
        public string videoDescription { get; set; }
        public string timeAgo { get; set; }
        public string commentCount { get; set; }
        public string voteCount { get; set; }
        public string video { get; set; }
        public string isVote { get; set; }
        public List<RespGetVideoComments> postComments { get; set; }
    }

    public class ReqAddComment
    {
        public string eventId { get; set; }
        public string userId { get; set; }
        public string commentText { get; set; }
        public string videoId { get; set; }
    }

    public class ReqGetVideoComments
    {
        public string videoId { get; set; }
    }

    public class RespGetVideoComments
    {
        public string commentId { get; set; }
        public string userId { get; set; }
        public string userName { get; set; }
        public string profilePic { get; set; }
        public string timeAgo { get; set; }
        public string commentText { get; set; }
    }


    public class Files
    {
        public Files()
        {
            files = new List<ViewDataUploadFilesResult>();
        }

        public List<ViewDataUploadFilesResult> files { get; set; }
    }


    public class ViewDataUploadFilesResult
    {
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string deleteUrl { get; set; }
        public string thumbnailUrl { get; set; }
        public string deleteType { get; set; }
    }

    public class ReqPayment
    {
        public string nameoncard { get; set; }
        public string cardnumber { get; set; }
        public string cardexpmonth { get; set; }
        public string year { get; set; }
        public string userId { get; set; }
        public string amount { get; set; }
        public string token { get; set; }
        public int tokenId { get; set; }
    }

    public class RespGetRecentEvents
    {
        public string eventId { get; set; }
        public string eventTitle { get; set; }
        public string urlKey { get; set; }
    }

    public class ReqNewsLetter
    {
        public string email { get; set; }
    }

    public class ReqSearch
    {
        public string searchText { get; set; }
    }

    public class RespEventForSearch
    {
        public string eventId { get; set; }
        public string eventName { get; set; }
        public string urlKey { get; set; }
    }

    public class CategoryModel
    {
        public Int64 CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    public class RolesModel
    {
        public Int64 RoleId { get; set; }
        public string RoleName { get; set; }
    }


    public class ReqContactUs
    {
        public string name { get; set; }
        public string emailId { get; set; }
        public string description { get; set; }
    }

    public class ReqVote
    {
        public string postId { get; set; }
        public string eventId { get; set; }
        public string userId { get; set; }
        public string isLike { get; set; }
    }

    public class RespEventStatus
    {
        public int isUploading { get; set; }
    }

    public class UploadedFile
    {
        public UploadedFile()
        {
            UploadStatus = false;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public long Id { get; set; }

        public string FileName { get; set; }

        public string Description { get; set; }

        public bool UploadStatus { get; set; }

        public int SelectedCategory { get; set; }

        public HttpPostedFileBase File { get; set; }

        public int SelectedEvent { get; set; }

        public string SelectedToken { get; set; }

        public string FileUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}