using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjFunc.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Informe o usuário")]
        [Display(Name = "Usuário:")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Informe a senha")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha:")]
        public string Senha { get; set; }

        [Display(Name = "Lembrar")]
        public bool LembrarMe { get; set; }
    }
}