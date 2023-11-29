using System.ComponentModel.DataAnnotations;

namespace MediaCenter.ViewModels.PostStatuses
{
    public class CreatePostStatusViewModel
    {
        [Required(ErrorMessage = "Введите название статуса поста")]
        [Display(Name = "Статус поста")]
        public string StatusPost { get; set; }
    }
}
