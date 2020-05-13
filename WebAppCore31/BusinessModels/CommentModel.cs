using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore31
{
    public class CommentModel
    {
        public CommentModel(Comment comment)
        {
            Id = comment.Id;
            StudentId = comment.StudentId;
            CourseId = comment.CourseId;
            DateTime = comment.DateTime.ToShortDateString();
            CommentString = comment.CommentString;
        }

        public string Id { get; set; }
        public string StudentId { get; set; }
        public string CourseId { get; set; }
        public string CommentString { get; set; }
        public string DateTime { get; set; }
    }
}
