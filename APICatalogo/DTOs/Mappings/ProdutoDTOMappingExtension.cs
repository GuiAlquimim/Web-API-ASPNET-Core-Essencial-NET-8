using APICatalogo.Models;

namespace APICatalogo.DTOs.Mappings;

public static class ProdutoDTOMappingExtension
{
    public static ProdutoDTO? ToProdutoDTO(this Produto produto)
    {
        if (produto is null)
            return null;

        return new ProdutoDTO()
        {
            ProdutoId = produto.ProdutoId,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Preco = produto.Preco,
            ImagemUrl = produto.ImagemUrl,
            CategoriaId = produto.CategoriaId
        };
    }

    public static Produto? ToProduto(this ProdutoDTO produtoDTO)
    {
        if (produtoDTO is null)
            return null;

        return new Produto()
        {
            ProdutoId = produtoDTO.ProdutoId,
            Nome = produtoDTO.Nome,
            Descricao = produtoDTO.Descricao,
            Preco = produtoDTO.Preco,
            ImagemUrl = produtoDTO.ImagemUrl,
            CategoriaId = produtoDTO.CategoriaId
        };
    }

    public static IEnumerable<ProdutoDTO>? ToProdutoDTOList(this IEnumerable<Produto> produtos)
    {
        if (produtos is null || !produtos.Any())
            return null;

        var produtosDTO = new List<ProdutoDTO>();

        foreach (var produto in produtos)
        {
            var produtoDTO = new ProdutoDTO();

            produtoDTO.ProdutoId = produto.ProdutoId;
            produtoDTO.Nome = produto.Nome;
            produtoDTO.Descricao = produto.Descricao;
            produtoDTO.Preco = produto.Preco;
            produtoDTO.ImagemUrl = produto.ImagemUrl;
            produtoDTO.CategoriaId = produto.CategoriaId;

            produtosDTO.Add(produtoDTO);
        }

        return produtosDTO;
    }
}
