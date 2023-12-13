using System.ComponentModel.DataAnnotations;

namespace MediaCenter.ViewModels.Chats
{
    public class EditChatViewModel
    {
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
    }
}
