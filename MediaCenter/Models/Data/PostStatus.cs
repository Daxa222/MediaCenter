using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaCenter.Models.Data
{
    public class PostStatus
    {
        //Key - первичный ключ
        //DatabaseGenerated(DatabaseGeneratedOption.Identity) - поле автоинкрементное
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }


        [Required(ErrorMessage = "Введите статус поста")]
        [Display(Name ="Статус поста")]
        public string StatusPost { get; set; }

        //Навигационные свойства
        [Required]
        public ICollection<Post> Posts { get; set; }
    }
}
