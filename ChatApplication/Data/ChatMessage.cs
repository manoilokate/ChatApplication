using System.ComponentModel.DataAnnotations;

namespace ChatApplication.Data
{
    public class ChatMessage
    {
        [Key]
        public int IdMessage { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(500)]
        public string Message { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
