namespace WebAppCore31.ModelsDTO
{
    public class CourseDTO
    {
        public CourseDTO(Course course, string authorName)
        {
            Id = course.Id;
            Subject = course.Subject;
            Title = course.Title;
            ContentCourse = course.ContentCourse;
            AuthorId = course.AuthorId;
            AuthorName = authorName;
        }
        public string Id { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public string ContentCourse { get; set; }
        public string AuthorName { get; set; }
        public string AuthorId { get; set; }

    }
}
