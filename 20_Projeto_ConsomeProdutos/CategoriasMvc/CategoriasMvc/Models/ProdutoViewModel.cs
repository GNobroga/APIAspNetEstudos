using System.ComponentModel.DataAnnotations;

namespace CategoriasMvc.Models;

public class ProdutoViewModel
{
    public int ProdutoId { get; set; }
    [Required(ErrorMessage = "O nome do produto é obrigatório")]
    public string? Nome { get; set; }
    [Required(ErrorMessage = "A descrição do produto é obrigatória")]
    public string? Descricao { get; set; }
    [Required(ErrorMessage = "Informe o preço do produto")]
    public decimal Preco { get; set; }
    [Required(ErrorMessage = "Informe o caminho da imagem do produto")]
    [Display(Name = "Caminho da Imagem")]
    public string? ImagemUrl { get; set; }

    [Display(Name = "Categoria")]
    public int CategoriaId { get; set; }
}
