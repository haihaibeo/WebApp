using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore31.ModelsDTO
{
    public class PublishDTO
    {
        [Required(ErrorMessage = "Title required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Subject required")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Content Required")]
        public string ContentCourse { get; set; }
    }
}
