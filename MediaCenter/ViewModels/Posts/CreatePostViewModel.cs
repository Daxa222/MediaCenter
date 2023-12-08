using System.ComponentModel.DataAnnotations;

namespace MediaCenter.ViewModels.Posts
{
    public class CreatePostViewModel
    {
        [Required(ErrorMessage = "Введите название поста")]
        [Display(Name = "Название поста")]
        public string StatusPostTitlePost { get; set; }

        [Required(ErrorMessage = "Введите описание поста")]
        [Display(Name = "Описание поста")]
        public string Description { get; set; }

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
    }
}
