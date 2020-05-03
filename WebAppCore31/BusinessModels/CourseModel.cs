using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore31
{
    public class CourseModel
    {
        public CourseModel(Course course)
        {
            Id = course.Id;
            Subject = course.Subject;
            Title = course.Title;
            ContentCourse = course.ContentCourse;
        }
        public string Id { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public string ContentCourse { get; set; }
    }
}
