using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MediaCenter.Models.Data
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "Введите фамилию")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Дата регистрации")]
        public DateTime RegDate { get; set; }


        //Навигационные свойства
        public ICollection<Post> Posts { get; set; }

        public ICollection<Like> Likes { get; set; }
    }
}
