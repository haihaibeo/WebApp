using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Entities
{
    public class Comment
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string CommentString { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string CourseId { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
