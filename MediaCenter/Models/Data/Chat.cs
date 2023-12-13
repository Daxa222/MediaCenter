using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaCenter.Models.Data
{
    public class Chat
    {
        //Key - первичный ключ
        //DatabaseGenerated(DatabaseGeneratedOption.Identity) - поле автоинкрементное
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Дата отправки сообщения")]
        public DateTime DepartureDate { get; set; }

        [Display(Name = "Сообщение")]
        public string? Description { get; set; }

        //т.к. у каждого смс есть отправитель, то нужно указывать внешний ключ
        [Required]
        [Display(Name = "Отправитель")]
        public string? IdSender { get; set; }

        //т.к. у каждого смс есть получатель, то нужно указывать внешний ключ
        [Required]
        [Display(Name = "Получатель")]
        public string? IdRecipient { get; set; }

        //Навигационные свойства
        //свойство нужно для более правильного отображения данных в представлениях
        [Display(Name = "Отправитель")]
        [ForeignKey("IdSender")]
        public User User { get; set; }

    }
}
