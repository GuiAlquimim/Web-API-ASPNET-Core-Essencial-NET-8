using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public PagedList<Produto> GetProdutosPagination(ProdutosParameters produtosParams)
    {
        var produtos = GetAll().OrderBy(p => p.ProdutoId).AsQueryable();

        var produtosOrdenados = PagedList<Produto>.ToPagedList(produtos, produtosParams.PageNumber, produtosParams.PageSize);

        return produtosOrdenados;
    }

    //public IEnumerable<Produto> GetProdutosPagination(ProdutosParameters produtosParams)
    //{
    //    return GetAll()
    //        .OrderBy(p => p.Nome)
    //        .Skip((produtosParams.PageNumber - 1 ) * produtosParams.PageSize)
    //        .Take(produtosParams.PageSize).ToList();
    //}



    public IEnumerable<Produto> GetProdutosPorCategoria(int id)
    {
        return GetAll().Where(c => c.CategoriaId == id);
    }
}