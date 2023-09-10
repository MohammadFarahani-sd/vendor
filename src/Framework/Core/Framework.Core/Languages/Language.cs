using System.ComponentModel.DataAnnotations;

namespace Framework.Core.Languages;

public enum Language
{
    None = 0,

    [Display(Name = "en")] English = 1,

    [Display(Name = "ar")] Arabic = 2
}