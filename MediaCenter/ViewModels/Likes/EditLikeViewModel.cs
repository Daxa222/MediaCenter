using System.ComponentModel.DataAnnotations;

namespace MediaCenter.ViewModels.Likes
{
    public class EditLikeViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Дата установки лайка")]
        public DateTime InstallationDate { get; set; }

        //т.к. у каждого лайка есть пользователь, то нужно указывать внешний ключ
        [Required]
        [Display(Name = "Пользователь")]
        public string IdUser { get; set; }

        //т.к. у каждого лайка есть пост, то нужно указывать внешний ключ
        [Display(Name = "Пост")]
        public int? IdPost { get; set; }
    }
}
