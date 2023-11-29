using System.ComponentModel.DataAnnotations;

namespace MediaCenter.ViewModels.PostStatuses
{
    public class EditPostStatusViewModel
    {
        public byte Id { get; set; }

        [Required(ErrorMessage = "Введите статус")]
        [Display(Name = "Статус")]
        public string StatusPost { get; set; }
    }
}
