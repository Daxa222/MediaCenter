using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaCenter.Models.Data
{
    public class Post
    {
        //Key - первичный ключ
        //DatabaseGenerated(DatabaseGeneratedOption.Identity) - поле автоинкрементное
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required(ErrorMessage = "Введите название поста")]
        [Display(Name = "Название поста")]
        public string StatusPostTitlePost { get; set; }

        
        [Display(Name = "Описание поста")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Введите ссылку на мультимедиа поста")]
        [Display(Name = "Мультимедиа поста")]
        public string Multimedia { get; set; }

        [Required]
        [Display(Name = "Дата публикации поста")]
        public DateTime DatePublication { get; set; }

        //т.к. у каждого поста есть автор, то нужно указывать внешний ключ
        [Required]
        [Display(Name = "Автор поста")]
        public string IdAuthor { get; set; }

        //т.к. у каждого поста есть статус поста, то нужно указывать внешний ключ
        [Required]
        [Display(Name = "Статус поста")]
        public byte IdStatus { get; set; }


        //Навигационные свойства
        //свойство нужно для более правильного отображения данных в представлениях
        [Display(Name = "Автор поста")]
        [ForeignKey("IdAuthor")]
        public User User { get; set; }


        [Display(Name = "Статус поста")]
        [ForeignKey("IdStatus")]
        public PostStatus PostStatus { get; set; }
    }
}
