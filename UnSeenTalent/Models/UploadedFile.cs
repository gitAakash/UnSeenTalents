using System;
using System.Web;

namespace UnseentalentsApp.Models
{
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

        public int Token { get; set; }

        public string FileUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}