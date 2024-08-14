using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Produto> GetProdutos()
    {
        return _context.Produtos.ToList();
    }

    public Produto GetProduto(int id)
    {
        return _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
    }

    public Produto Create(Produto produto)
    {
        if (produto is null)
            throw new ArgumentNullException(nameof(produto));

        _context.Produtos.Add(produto);
        _context.SaveChanges();

        return produto;
    }

    public Produto Update(Produto produto)
    {
        if (produto is null)
            throw new ArgumentNullException(nameof(produto));

        if (_context.Produtos.Any(p => p.ProdutoId == produto.ProdutoId))
        {
            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();
            return produto;
        }

        throw new InvalidOperationException($"Produto não encontrado, verifique se o Id corresponde");
    }

    public Produto Delete(int id)
    {
        var produto = GetProduto(id);
        //var produto = _context.Produtos.Find(id);

        if (produto is null)
            throw new InvalidOperationException($"Produto com Id {id} não encontrado");

        _context.Remove(produto);
        _context.SaveChanges();

        return produto;
    }
}