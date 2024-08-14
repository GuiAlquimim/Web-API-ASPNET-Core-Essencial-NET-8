using APICatalogo.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models;

public class Produto
{
    [Key]
    public int ProdutoId { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório!")]
    [StringLength(80)]
    [UpperFirst]
    public string? Nome { get; set; }

    [Required]
    [StringLength(300)]
    public string? Descricao { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    [Range(1,(double)decimal.MaxValue)]
    public decimal Preco { get; set; }

    [Required]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }

    public float Estoque { get; set; }
    public DateTime DataCadastro { get; set; }

    [JsonIgnore]
    public Categoria? Categoria { get; set; }
    public int CategoriaId { get; set; }
}