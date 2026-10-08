using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models
{
    public enum Importance
    {
        [Display(Name = "Lav")]
        Low,

        [Display(Name = "Middels")]
        Medium,

        [Display(Name = "Høy")]
        High
    }
}