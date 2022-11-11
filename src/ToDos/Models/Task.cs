using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ToDos.Models
{
    public class Task
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [MaxLength(10)]
        public string Category { get; set; }
        [DefaultValue(true)]    
        public bool IsActive { get; set; }

    }
}
