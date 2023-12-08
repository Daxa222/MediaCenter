using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaCenter.Models.Data
{
    public class Like
    {
        //Key - первичный ключ
        //DatabaseGenerated(DatabaseGeneratedOption.Identity) - поле автоинкрементное
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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


        //Навигационные свойства
        //свойство нужно для более правильного отображения данных в представлениях
        [Display(Name = "Пользователь")]
        [ForeignKey("IdUser")]
        public User User { get; set; }


        [Display(Name = "Пост")]
        [ForeignKey("IdPost")]
        public Post Post { get; set; }
    }
}
