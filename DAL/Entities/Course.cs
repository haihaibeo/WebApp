using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Entities
{
    public class Course
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public Course()
        {
            Comments = new HashSet<Comment>();
        }

        [StringLength(50)]
        public string Subject { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string ContentCourse { get; set; }

        public string AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public Author Author { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
