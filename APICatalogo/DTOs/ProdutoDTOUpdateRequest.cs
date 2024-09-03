using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

public class ProdutoDTOUpdateRequest : IValidatableObject
{
    [Range(0, 9999, ErrorMessage = "O estoque deve estar entre 1 e 9999")]
    public float Estoque { get; set; }
    public DateTime DataCadastro { get; set; } = DateTime.Now;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DataCadastro <= DateTime.Now.Date)
        {
            yield return new ValidationResult("A data inserida deve ser maior do que a data atual",
                new[] { nameof(DataCadastro) });
        }
    }
}
