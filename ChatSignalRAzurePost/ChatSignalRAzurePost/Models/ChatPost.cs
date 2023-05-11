using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatSignalRAzurePost.Models
{
    [Table("CHAT_POST")]
    public class ChatPost
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("MESSAGE")]
        public string Message { get; set; }
        [Column("SENDER")]
        public string Sender { get; set; }
    }
}
