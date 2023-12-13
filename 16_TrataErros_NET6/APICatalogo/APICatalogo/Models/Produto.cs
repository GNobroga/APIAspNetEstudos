using APICatalogo.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models
{
    [Table("Produtos")]
    public class Produto : IValidatableObject
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(80, ErrorMessage = "O nome deve ter no máximo {1} e no mínimo {2} caracteres",
           MinimumLength = 5)]
        [PrimeiraLetraMaiuscula]
        public string? Nome { get; set; }

        [Required]
        [StringLength(300)]
        public string? Descricao { get; set; }

        [Required]
        [Range(1, 10000, ErrorMessage = "O preço deve estar entre {1} e {2}")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Preco { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 10)]
        public string? ImagemUrl { get; set; }

        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }
        public int CategoriaId { get; set; }

        [JsonIgnore]
        public Categoria? Categoria { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(this.Nome))
            {
                var primeiraLetra = this.Nome[0].ToString();
                if (primeiraLetra != primeiraLetra.ToUpper())
                {
                    yield return new
                        ValidationResult("A primeira letra do produto deve ser maiúscula",
                        new[]
                        { nameof(this.Nome) }
                        );
                }
            }

            if (this.Estoque <= 0)
            {
                yield return new
                       ValidationResult("O estoque deve ser maior que zero",
                       new[]
                       { nameof(this.Estoque) }
                       );
            }
        }
    }
}
