using APICatalogo.Validations;
using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

public class CategoriaDTO
{
    [Key]
    public int CategoriaId { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório!")]
    [StringLength(80)]
    [UpperFirst]
    public string? Nome { get; set; }

    [Required]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }
}
