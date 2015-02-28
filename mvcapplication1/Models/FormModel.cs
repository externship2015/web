using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Web.Security;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcApplication1.Models
{
    public class FormModel
    {
        [Display(Name = "Начало периода")]
        public string date1 { get; set; }

        [Display(Name = "Конец периода")]
        public string date2 { get; set; }

        [Required]
        [Display(Name = "Регион")]
        [StringLength(100, ErrorMessage = "Необходимо заполнить", MinimumLength = 2)]
       
        public string regions { get; set; }

        [Required]
        [Display(Name = "Город")]
        [StringLength(100, ErrorMessage = "Необходимо заполнить", MinimumLength = 2)]

        public string cities { get; set; }

    }
}
