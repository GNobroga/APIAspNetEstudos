using System.ComponentModel.DataAnnotations;

namespace CategoriasMvc.Models;
public class CategoriaViewModel
{
    public int CategoriaId { get; set; }
    [Required(ErrorMessage = "O nome da categoria é obrigatório")]
    public string? Nome { get; set; } = string.Empty;
    [Required]
    [Display(Name = "Imagem")]
    public string? ImagemUrl { get; set; } = string.Empty;
}
